from crewai import Agent, Task, Crew, LLM
import os
from dotenv import load_dotenv
import ast
import Agents
import Tasks
import Files

load_dotenv()

language_model = LLM(
    model='gemini/gemini-1.5-flash',
    temperature=0.0,
    api_key=os.getenv("GOOGLE_API_KEY"),
)

dependency_finder_agent = Agent(**Agents.dict_dependency_finder_agent, llm=language_model)
test_generator_agent = Agent(**Agents.dict_test_generator_agent, llm=language_model)

analyze_code_task = Task(**Tasks.create_analyze_code_task(Files.cs_file_content), agent=dependency_finder_agent)

crew_dependency = Crew(
    agents=[dependency_finder_agent],
    tasks=[analyze_code_task],
    verbose=True,
)

dependency_results = crew_dependency.kickoff()
dependency_results = list(ast.literal_eval(dependency_results.raw))

existing_file = Files.find_file(Files.base_directory, "some_filename.cs")
existing_test = f"{existing_file}Test.cs"
dependency_results.append(existing_test)
print(dependency_results)

found_paths = Files.find_file(Files.base_directory, "some_related_file.cs")


existing_test_content, related_files_content = Files.process_test_and_related_files(found_paths)
generate_test_task = Task(**Tasks.create_generate_test_task(Files.cs_file_content, related_files_content, existing_test_content))


generate_test_task = Task(**Tasks.dict_generate_test_task, agent=test_generator_agent)

crew_test_generation = Crew(
    agents=[test_generator_agent],
    tasks=[generate_test_task],
    verbose=True,
)

test_results = crew_test_generation.kickoff()
print("Testes Gerados:", test_results)
