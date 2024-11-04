from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from crewai import Agent, Task, Crew, Process, LLM
import os
from dotenv import load_dotenv
from fastapi.responses import JSONResponse

import uuid

app = FastAPI()

# options = [
#     'http:localhost',
#     "http://127.0.0.1:5500"
# ]

# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=options,
#     allow_credentials=True,
#     allow_methods=["*"],
#     allow_headers=["*"]
# )

load_dotenv()

# Pydantic model to receive the payload
class Evento(BaseModel):
    evento: str

# Define LLM models for low and high temperature
llm_low_temp = LLM(
    model='gemini/gemini-1.5-flash',
    temperature=0.0,
    api_key=os.getenv("GOOGLE_API_KEY"),
)

llm_high_temp = LLM(
    model='gemini/gemini-1.5-flash',
    temperature=0.8,
    api_key=os.getenv("GOOGLE_API_KEY"),
)

# Define agents and tasks as in your provided code
def generate_gherkin_feature(json_payload):
    tasks = []
    agents = []
    for i in range(1, 4):
        gherkin_writer_agent = Agent(
            role=f"Escritor de cenários Gherkin {i}",
            goal="Criar cenários Gherkin compreensivos, claros e concisos para descrever o comportomaneto do sistema",
            backstory="""Esse agente tem conhecimento extensivo em behavioral-driven development e possui um entendimento profundo
                         em histórias de usuários e requisitos de sistemas. Se esforça em garantir que os cenários Gherkin descrevam
                         precisamente comportamentos do sistema esperados.""",
            llm=llm_high_temp,
            verbose=True
        )

        gherkin_reviewer_agent = Agent(
            role=f"Revisor de cenários Gherkin {i}",
            goal="Revisar cenários Gherkin para garantir clareza, consistência and alinhamento com as requisições do projeto",
            backstory="O agente é altamente qualificado em metodologias BDD, com um profundo entendimento de como cenários Gherkin bem escritos contribuem para uma comunicação eficaz entre as partes interessadas.",
            llm=llm_high_temp,
            verbose=True
        )

        agents.append(gherkin_writer_agent)
        agents.append(gherkin_reviewer_agent)

        task_gherkin_code = Task(
            description=f"""
            Transforme o seguinte caso de uso em arquivos BDD com cenários outlines para casos de sucesso e erro.
            Para cada atributo gerar uma mensagem de erro personalizada quando ela não for informada:
            {json_payload} 
            Exemplo de formato de saída:
                Scenario Outline: Incluir modalidade com sucesso
                        Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
                        And o servidor seleciona a resolução <resolução>
                        When o sistema valida e salva a modalidade
                        Then o sistema deve salvar a modalidade com status "Em edição"
                        
                    Examples:
                        | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução |
                        | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            | Res1      |
                
                    Scenario Outline: Incluir modalidade com erro
                        Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
                        And o servidor seleciona a resolução <resolução>
                        When o sistema valida e não pode salvar a modalidade
                        Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"
                        
                    Examples:
                        | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                    |
                        | ABC   | Nome | Desc      | -10        | 2024-01-01  | Bolsa1            | Res1      | Percentual não pode ser negativo |
            """,
            expected_output="ONLY the gherkin code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
            agent=gherkin_writer_agent,
        )

        task_gherkin_review = Task(
            description="Revise o código gherkin gerado e ajuste conforme necessário.",
            expected_output="ONLY the gherkin code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
            context=[task_gherkin_code],
            agent=gherkin_reviewer_agent
        )

        tasks.append(task_gherkin_code)
        tasks.append(task_gherkin_review)

    manager = Agent(
        role="Gerente e revisor de códigos Gherkin",
        goal="Responsável por gerar a versão final dos cenários gherkin contendo os pontos positivos de todos os outros exemplos",
        backstory="Você, no papel de um especialista em código Gherkin deve revisar os cenários gerados e produzir uma versão final sem erros e com melhores pontos de cada um",
        llm=llm_low_temp
    )

    file_name = f"features/analise_evento_{uuid.uuid4()}.feature"

    final_task = Task(
        description="Leia e compare todos os códigos Gherkin gerados e desenvolva uma versão final com base neles",
        expected_output="ONLY the gherkin code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
        output_file=file_name,
        agent=manager,
        context=tasks
    )

    crew = Crew(
        agents=agents + [manager],
        tasks=tasks + [final_task],
        max_rpm=10,
        output_log_file="crew_log.txt",
        manager_llm=llm_low_temp,
        process=Process.sequential,
        verbose=True
    )

    resultado = crew.kickoff()
    return resultado.raw


@app.get("/")
async def home():
    return "Rodando"

@app.post("/gherkin", responses={
    200: {
        "content": {
            "application/json": {
                "example": {"feature": "Feature: Excluir Resolução\n\n Scenario Outline: Nome Scenario\n    Given exemplo de given\n    When exemplo de when\n    Then exemplo de then\n\n    Examples:\n      | exemplos |\n    | exemplo1 |"}
            }
            }
        }
    })
async def generate_gherkin_file(evento: Evento):
    try:
        # Call the function to process the evento and generate the feature file
        feature_file = generate_gherkin_feature(evento.evento)
        # return FileResponse(path=feature_file, media_type='text/plain', filename=feature_file.split('/')[-1])
        return JSONResponse({"feature": feature_file})
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))
