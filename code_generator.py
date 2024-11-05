from crewai import Agent, Task, Crew, Process, LLM
import os
from dotenv import load_dotenv
import uuid

load_dotenv()
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
    for turn in range(1, 4):
        xunit_writer_agent = Agent(
            role=f"Escritor de Código xUnit {turn}",
            goal="Criar código xUnit conciso, baseado no código C# e no arquivo Feature",
            backstory="""Esse agente tem conhecimento extensivo em behavioral-driven development e possui um entendimento profundo
                         em histórias de usuários e requisitos de sistemas. Se esforça em garantir que os cenários Gherkin sejam 
                         devidamente utilizados no código xUnit.""",
            llm=llm_high_temp,
            verbose=True
        )

        xunit_reviewer_agent = Agent(
            role=f"Revisor de código xUnit {turn}",
            goal="Revisar código xUnit para garantir clareza, consistência e alinhamento com os arquivos Feature e C# do projeto",
            backstory="""O agente é altamente qualificado em metodologias BDD, em códigos C# e em código xUnit. É experiente em análise
            de código e em correção de erros.""",
            llm=llm_high_temp,
            verbose=True
        )

        agents.append(xunit_writer_agent)
        agents.append(xunit_reviewer_agent)

        task_xunit_code = Task(
            description=f"""
            Utilize o seguinte arquivo Feature
            {json_payload.feature}
            e o seguinte arquivo de C# 
            {json_payload.codigo}
            e desenvolva um arquivo xUnit para testes automatizados.
            """,
            expected_output="ONLY the xunit code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
            agent=xunit_writer_agent,
        )

        task_xunit_review = Task(
            description=f"Revise o código xunit gerado e ajuste conforme necessário. Foque em corrigir funções, erros de síntaxe e divergências com o arquivo feature: {json_payload.feature}",
            expected_output="ONLY the xunit code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
            context=[task_xunit_code],
            agent=xunit_reviewer_agent
        )

        tasks.append(task_xunit_code)
        tasks.append(task_xunit_review)

    manager = Agent(
        role="Gerente e revisor de código xUnit",
        goal="Responsável por gerar a versão final do código xUnit contendo os pontos positivos de todos os outros exemplos",
        backstory="Você, no papel de um especialista em código xUnit deve revisar o código e funções gerados e produzir uma versão final sem erros e com melhores pontos de cada um",
        llm=llm_low_temp
    )

    file_name = f"codigo_cs/xunit_code{uuid.uuid4()}.cs"

    final_task = Task(
        description="Leia e compare todos os código xUnit gerados e desenvolva uma versão final com base neles.",
        expected_output="ONLY the xunit code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
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

class Request:
    feature: str