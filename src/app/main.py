from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from crewai import Agent, Task, Crew, Process, LLM
import os
from dotenv import load_dotenv
from fastapi.responses import JSONResponse
from crews.utils import run_crew_async
from crews.crew_xUnit import xunit_generation
from crews.crew_gherkin import generate_gherkin

import uuid

app = FastAPI()

origins = [
    "http://localhost",
    "http://127.0.0.1:5500",
]

# Adicione o middleware de CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,  # Permitir as origens definidas acima
    allow_credentials=True,  # Permitir envio de cookies
    allow_methods=["*"],  # Permitir todos os métodos (GET, POST, etc)
    allow_headers=["*"],  # Permitir todos os cabeçalhos
)

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
    for i in range(1, 6, 2):
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

#@app.get("/test")
#async def teste():
#    return FileResponse(path="C:/Users/gabri/test_generation_ai/features/analise_evento_3b552e54-4343-4952-9db1-fd412d869e9e.feature", media_type='text/plain', filename="analiase.feature")

@app.post("/gherkin",
    responses={
        200: {
            "content": {
                "text/plain": {
                    "example": "Teste"
                }
            }
        }
    }
)
async def generate_gherkin_file(evento: Evento):
    feature = run_crew_async(generate_gherkin, user_case=evento.evento)
    body = {
        "feature": feature
    }
    return JSONResponse(body)

@app.post('xunit')
async def generate_xunit(params):
    ...

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", reload=True)