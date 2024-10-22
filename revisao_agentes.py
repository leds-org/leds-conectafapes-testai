from crewai import Agent, Task, Crew, Process, LLM
import os
from dotenv import load_dotenv

load_dotenv()

json = open("data.json", "r", encoding="utf-8").read()

llm_low_temp = LLM(
    #model="ollama/llama3.1:8b",
    model='gemini/gemini-1.5-flash',
    temperature=0.0,
    #base_url="http://localhost:11434"
    api_key=os.getenv("GOOGLE_API_KEY"),
)

llm_high_temp = LLM(
    #model="ollama/llama3.1:8b",
    model='gemini/gemini-1.5-flash',
    temperature=0.8,
    #base_url="http://localhost:11434"
    api_key=os.getenv("GOOGLE_API_KEY"),
)


tasks = []
agents = []
for i in range(1, 6, 2):
    gherkin_writer_agent = Agent(
    role=f"Escritor de cenários Gherkin {i}",
    goal="Criar cenários Gherkin compreensivos, claros e concisos para descrever o comportomaneto do sistema",
    backstory="""Esse agente tem conhecimento extensivo em behavioral-drive development e possui um entendimento profundo
                    em histórias de usuários e requisitos de sistemas. Se esforça em garantir que os cenários Gherkin descrevam
                    precisamente comportamentos do sistema esperados, fazendo que os requisitos sejam entendidos por técnicos
                    e não técnicos.""",
    llm=llm_high_temp,
    verbose=True
    )

    gherkin_reviewer_agent = Agent(
    role=f"Revisor de cenários Gherkin {i}",
    goal="Revisar cenários Gherkin para garantir clareza, consistência and alinhamento com as requisições do projeto",
    backstory="O agente é altamente qualificado em metodologias BDD, com um profundo entendimento de como cenários Gherkin bem escritos contribuem para uma comunicação eficaz entre as partes interessadas. Ele revisa os cenários Gherkin criados pelo Gherkin Scenario Writer Agent, garantindo que sejam fáceis de entender, livres de ambiguidade e capturem adequadamente o comportamento pretendido do usuário. O Gherkin Scenario Reviewer também garante consistência na linguagem usada e verifica se os cenários estão alinhados com as metas e padrões gerais do projeto.",
    llm=llm_high_temp,
    verbose=True
    )
    agents.append(gherkin_writer_agent)
    agents.append(gherkin_reviewer_agent)
    task_gherkin_code = Task(
        description=f"""
        Transforme o seguinte caso de uso em arquivos BDD com cenários outlines para casos de sucesso e erro.
        Para cada atributo gerar uma mensagem de erro personalizada quando ela não for informada.
        Caso exista uma regra de integridade, gera uma mensagem de erro personalizada quando ela não for obedecida:
        {json}
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
                | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro               |
                | ABC   | Nome | Desc      | -10        | 2024-01-01  | Bolsa1            | Res1      | Percentual não pode ser negativo |
        """,
        expected_output="ONLY the gherkin code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
        context=[tasks[i-2]] if len(tasks) > 0 else None,
        agent=gherkin_writer_agent,
    )

    task_gherkin_review = Task(
        #tools=[file_read_tool],
        description=f"""Revise e ajuste se necessário o código gherkin gerado. Fique atento a inconsistências de escrita e 
        a erros de síntaxe. Verifique se os cenários contemplem todas as possibilidades.""",
        expected_output="ONLY the gherkin code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
        context=[task_gherkin_code],
        agent=gherkin_reviewer_agent
    )
    tasks.append(task_gherkin_code)
    tasks.append(task_gherkin_review)



manager = Agent(
    role="Gerente e revisor de códigos Gherkin",
    goal="Responsável por gerar a versão final dos cenários gherkin contendo os pontos positivos de todos os outros exemplos",
    backstory="""Você, no papel de um especialista em código Gherkin deve revisar os cenários gerados e produzir uma 
    versão final sem erros e com melhores pontos de cada um""",
    llm=llm_low_temp
)

final_task = Task(
    description="""Leia e compare todos os códigos Gherkin gerados e desenvolva uma versão final com base neles""",
    expected_output="ONLY the gherkin code generated without the code block like ```, DO NOT USE ANY MARKDOWN TAG",
    output_file="features/analise_evento7.feature",
    agent=manager,
    context=tasks
)


crew = Crew(
    agents=agents+[manager],
    tasks=tasks+[final_task],
    max_rpm=10,
    output_log_file="crew_log.txt",
    #manager_agent=manager_agent,
    manager_llm=llm_low_temp,
    process=Process.sequential,
    verbose=True
    
)
resultado = crew.kickoff()
