from crewai import LLM, Agent, Task
import os
from typing import Dict, Union, TypedDict, Callable, Any
from dotenv import load_dotenv
from yaml import safe_load
import asyncio

load_dotenv()

class UserCaseDict(TypedDict):
    user_case: str

class FeatureDict(TypedDict):
    feature: str

def run_crew_async(func: Callable, **params: Any) -> str:
    return asyncio.run(func(params))

def init_llm(config = {}, temp = 0.0, api_key = os.getenv("GOOGLE_API_KEY"), model = os.getenv("DEFAULT_LLM_MODEL")) -> LLM:
    return LLM(
        model=config.get("model", model),
        temperature=config.get("temperature", temp),
        api_key=config.get("api_key", api_key),
        timeout=config.get("timeout", None),
        max_tokens=config.get("max_tokens", None),
        top_p=config.get("top_p", None),
        frequency_penalty=config.get("frequency_penalty", None),
        presence_penalty=config.get("presence_penalty", None),
        response_format=config.get("response_format", None),
        seed=config.get("seed", None)
    )

def init_agent(config: dict[str, str], llm , tools=[]) -> Union[Agent, None]:
    if "role" not in config: 
        print("Agent role must be defined")
        return
    if "goal" not in config:
        print("Agent goal must be defined")
        return
    if "backstory" not in config: 
        print("Agent backstory must be defined")
        return 
    return Agent(
        role = config.get("role"),
        goal = config.get("goal"),
        backstory= config.get("backstory"),
        llm = llm,
        config = None,
        cache = config.get("cache", True),
        verbose = config.get("verbose", False),
        max_rpm = config.get("max_rpm", None),
        allow_delegation = config.get("allow_delegation", False),
        tools = tools,
        max_iter = config.get("max_iter", 25),
        function_calling_llm = config.get("function_calling_llm", None),
        max_execution_time = config.get("max_execution_time", None),
        step_callback = config.get("step_callback", None),
        system_template = config.get("system_template", None),
        prompt_template = config.get("prompt_template", None),
        response_template = config.get("response_template", None),
        allow_code_execution = config.get("allow_code_execution", False),
        max_retry_limit = config.get("max_retry_limit", 2),
        use_system_prompt = config.get("use_system_prompt", True),
        respect_context_window = config.get("respect_context_window", True),
        code_execution_mode = config.get("code_execution_mode", 'safe'),
    )

def init_task(config: Dict[str, str], agent: Agent = None, context=None, tools=[], output_file="") -> Union[Task, None]:
    if "description" not in config:
        print("Task description must be defined")
        return
    if "expected_output" not in config:
        print("Task expected_output must be defined")
        return

    return Task(
        description=config.get("description"),
        expected_output=config.get("expected_output"),
        agent=agent,
        tools=tools,
        async_execution = config.get("async_execution", False),
        context=context,
        config = None,
        output_json = config.get("output_json", None),
        output_pydantic = config.get("output_pydantic", None),
        output_file = output_file,
        human_input = config.get("human_input", False),
        converter_cls = config.get("converter_cls", None),
        callback = config.get("callback", None)
    )

def read_yaml(file_path: str) -> Dict[str, str]:
    with open(file_path) as file:
        return safe_load(file)

def get_yaml_config(base_path: str, files=["agents", "tasks"]) -> Dict[str, Dict[str, str]]:
    dict_return: Dict[str, Dict] = {}
    for file in files:
        dict_return[file] = read_yaml(f"{base_path}/{file}.yaml")
    return dict_return

# def init_agent(
#     agent_profile: AgentProfile,
#     llm: LLM,
#     config = None,
#     cache = True,
#     verbose = False,
#     max_rpm = None,
#     allow_delegation = False,
#     tools = [],
#     max_iter = 25,
#     function_calling_llm = None,
#     max_execution_time = None,
#     step_callback = None,
#     system_template = None,
#     prompt_template = None,
#     response_template = None,
#     allow_code_execution = False,
#     max_retry_limit = 2,
#     use_system_prompt = True,
#     respect_context_window = True,
#     code_execution_mode = 'safe'
# ) -> Agent:
#     return Agent(
#         role = agent_profile["role"],
#         goal = agent_profile["goal"],
#         backstory = agent_profile["backstory"],
#         llm = llm,
#         config = config,
#         cache = cache,
#         verbose = verbose,
#         max_rpm = max_rpm,
#         allow_delegation = allow_delegation,
#         tools = tools,
#         max_iter = max_iter,
#         function_calling_llm = function_calling_llm,
#         max_execution_time = max_execution_time,
#         step_callback = step_callback,
#         system_template = system_template,
#         prompt_template = prompt_template,
#         response_template = response_template,
#         allow_code_execution = allow_code_execution,
#         max_retry_limit = max_retry_limit,
#         use_system_prompt = use_system_prompt,
#         respect_context_window = respect_context_window,
#         code_execution_mode = code_execution_mode
#     )

# def init_llm(model: str = os.getenv("DEFAULT_LLM_MODEL"), temp: float = 0.0, key: str = os.getenv("GOOGLE_API_KEY")) -> LLM:    
    
#     return LLM(
#         model=model,
#         temperature=temp,
#         api_key=key,
#     )

# def init_task(
#     task_profile,
#     agent,
#     tools = [],
#     async_execution = False,
#     context = None,
#     config = None,
#     output_json = None,
#     output_pydantic = None,
#     output_file = "",
#     human_input = False,
#     converter_cls = None,
#     callback = None
# ) -> Task:
#     bind_output_example(task_profile["description"], task_profile["output_example"])
#     return Task(
#         description = task_profile["description"],
#         expected_output = task_profile["expected_output"],
#         agent = agent,
#         tools = tools,
#         async_execution = async_execution,
#         context = context,
#         config = config,
#         output_json = output_json,
#         output_pydantic = output_pydantic,
#         output_file = output_file,
#         human_input = human_input,
#         converter_cls = converter_cls,
#         callback = callback
#     )

# def bind_output_example(*tasks) -> None:
#     print(tasks)
    #task["description"] = task["description"] + task["output_example"]

# def init_crew(agents, tasks):
#     print(agents, tasks)

#print(os.getcwd())