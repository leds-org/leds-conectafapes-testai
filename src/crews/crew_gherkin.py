from crewai import Agent, Task, Crew, Process, LLM
from crewai_tools import FileWriterTool
from dotenv import load_dotenv
from typing import Callable, AnyStr
from crews.utils import(
    init_task,
    init_agent, 
    init_llm,
    get_yaml_config, 
    UserCaseDict
)
import asyncio

load_dotenv()

yaml_config = get_yaml_config(base_path="src/config", files=["agents", "tasks", "output_examples"])
agents_config = yaml_config["agents"]
tasks_config = yaml_config["tasks"]
outputs_examples = yaml_config["output_examples"]

llm_low_temp: LLM = init_llm()
llm_high_temp: LLM = init_llm(temp=0.6)

def crew_generation_gherkin(user_case: str) -> Crew:

    crew_agents: dict[str, str] = agents_config["gherkin_crew"]
    crew_tasks: dict[str, str] = tasks_config["gherkin_tasks"]

    gherkin_writer: Agent = init_agent(crew_agents["gherkin_writer"], llm_high_temp)
    crew_tasks["gherkin_code"]["description"] = crew_tasks["gherkin_code"]["description"].format(user_case=user_case)
    gherkin_code: Task = init_task(
        crew_tasks["gherkin_code"],
        gherkin_writer,
        #output_file=f"etapas_geracao/rodada_{turn}.cs"
    )

    gherkin_reviewer: Agent = init_agent(crew_agents["gherkin_reviewer"], llm_low_temp)
    crew_tasks["gherkin_review"]["description"] = crew_tasks["gherkin_review"]["description"].format(user_case=user_case)
    gherkin_review: Task = init_task(
        crew_tasks["gherkin_review"],
        gherkin_reviewer,
        context=[gherkin_code],
        #output_file=f"etapas_geracao/revisao_{turn}.cs"
    )
    

    return Crew(
        agents=[gherkin_writer, gherkin_reviewer],
        tasks=[gherkin_code, gherkin_review],
        max_rpm=10,
        output_log_file="crew_log.txt",
        #manager_agent=manager_agent,
        manager_llm=llm_low_temp,
        process=Process.sequential,
        verbose=True
        
    )
    
def manager_crew(reviews: list[str]) -> Crew:
    crew_agents: dict[str, str] = agents_config["gherkin_crew"]
    crew_tasks: dict[str, str] = tasks_config["gherkin_tasks"]

    crew_tasks["manager_gherkin_task"]["description"] = crew_tasks["manager_gherkin_task"]["description"] \
    .format(reviews[0], reviews[1], reviews[2])
    manager: Agent = init_agent(
        crew_agents["manager_gherkin"],
        llm=llm_low_temp,
        tools=[FileWriterTool()],
    )
    final_task: Task = init_task(
        crew_tasks["manager_gherkin_task"],
        agent=manager,
        output_file="features/ListarBolsaFeature.feature"
    )

    return Crew(
        agents=[manager],
        tasks=[final_task],
        max_rpm=10,
        process=Process.sequential,
        verbose=True
    )

async def generate_gherkin(user_case: UserCaseDict) -> None:
    user_case = user_case["user_case"]
    crew_gherkin: Crew = crew_generation_gherkin(user_case)
    result1 = crew_gherkin.kickoff_async()
    result2 = crew_gherkin.kickoff_async()
    result3 = crew_gherkin.kickoff_async()

    results = await asyncio.gather(result1, result2, result3)
    return manager_crew(results).kickoff()
