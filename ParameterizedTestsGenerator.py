import tiktoken
from crewai import Agent, Task, Crew, LLM
import os
from dotenv import load_dotenv
from pathlib import Path

def estimate_tokens(text):
    encoder = tiktoken.encoding_for_model("gpt-4")
    return len(encoder.encode(text))

total_tokens_processed = 0

load_dotenv()

language_model = LLM(
    model='gemini/gemini-1.5-flash',
    temperature=0.0,
    api_key=os.getenv("GOOGLE_API_KEY"),
)

test_generator_agent = Agent(
    name="Gerador de Testes xUnit",
    role="Engenheiro Sênior de Testes em C# e Especialista em xUnit",
    backstory="Profissional experiente com ampla vivência na criação de suítes de testes unitários usando xUnit para C#.",
    goal="Gerar uma suíte de testes unitários detalhada e abrangente em C# usando xUnit, garantindo alta cobertura de código e cenários.",
    llm=language_model
)

cs_directory = Path("leds-conectafapes-backend-admin-main/src/ConectaFapes/ConectaFapes.Application/Services/CadastroModalidadesBolsas")
existing_tests_directory = Path("leds-conectafapes-backend-admin-main/src/ConectaFapes/ConectaFapes.Application.Test/Services")
output_directory = existing_tests_directory / "Testes-Gerados"

if not cs_directory.exists():
    raise FileNotFoundError(f"O diretório '{cs_directory}' não foi encontrado.")
if not existing_tests_directory.exists():
    raise FileNotFoundError(f"O diretório '{existing_tests_directory}' não foi encontrado.")

output_directory.mkdir(exist_ok=True)

for cs_file in cs_directory.glob("*.cs"):
    with open(cs_file, 'r', encoding='utf-8') as file:
        cs_file_content = file.read()
        total_tokens_processed += estimate_tokens(cs_file_content)
    
    existing_test_file = existing_tests_directory / f"{cs_file.stem}Test.cs"
    existing_test_content = ""
    if existing_test_file.exists():
        with open(existing_test_file, 'r', encoding='utf-8') as file:
            existing_test_content = file.read()
            total_tokens_processed += estimate_tokens(existing_test_content)
    
    generate_test_task = Task(
        description=(
            f"Com base no seguinte código C#, crie uma suíte de testes unitários abrangente usando xUnit:\n\n"
            f"{cs_file_content}\n\n"
            "### Testes Pré-Existentes:\n"
            f"{existing_test_content}\n\n"
            "### Requisitos para os testes:\n"
            "1. Verifique a acessibilidade dos métodos antes de criar os testes.\n"
            "2. Use o framework xUnit para todos os testes.\n"
            "3. Garanta cobertura completa de todos os métodos públicos.\n"
            "4. Inclua casos de teste para caminho feliz, valores de limite, e cenários de erro.\n"
            "5. Utilize Moq ou NSubstitute para simular dependências externas. Quando utilizar `mock`, preste atenção nos testes existentes, especialmente ao simular `requests`, `ReturnsAsync`, etc. Veja como os mocks estão configurados para garantir consistência.\n"
            "6. Siga o padrão de nomenclatura MethodName_StateUnderTest_ExpectedBehavior.\n"
            "7. Organize os testes em classes lógicas.\n"
            "8. Utilize IClassFixture ou IDisposable para configuração e limpeza de recursos.\n"
            "9. Implemente testes data-driven com Theory e InlineData.\n"
            "10. Garanta isolamento entre os testes.\n"
            "11. A cobertura mínima deve ser de 90% do código.\n"
            "12. Gere pelo menos 25 testes por arquivo.\n"
            "13. Implemente testes assíncronos quando aplicável. Utilize `ReturnsAsync` para simular retornos assíncronos como nos testes existentes.\n"
            "14. Valide exceções personalizadas, se houver.\n"
            "15. Evite duplicação de código e mantenha os testes eficientes.\n"
            "16. Valide corretamente objetos imutáveis.\n"
            "17. Teste possíveis problemas de concorrência.\n"
            "18. Adicione comentários explicando a lógica de cada teste.\n"
            "19. Siga os princípios SOLID e Clean Code.\n"
            "\n"
            "### Restrições Estritas:\n"
            "20. Não invente métodos, propriedades ou comportamentos que não estejam explicitamente no código fornecido.\n"
            "21. Não modifique a assinatura dos métodos originais.\n"
            "22. Não assuma que um método lança exceções sem evidência no código.\n"
            "23. Reflita apenas o que está definido no código original.\n"
            "24. Adapte testes para métodos privados sem modificar a classe original.\n"
            "25. Garanta que todos os testes gerados sejam executáveis e validados corretamente.\n"
            "\n"
            "### Considerações Adicionais:\n"
            "- Use namespaces completos para instanciar classes.\n"
            "- Forneça contexto explícito para evitar ambiguidades.\n"
            "- Garanta que os testes sejam resilientes a mudanças futuras.\n"
            "- Inclua mensagens de asserção claras e descritivas.\n"
            "- Ao acessar resultados, sempre utilize `result.Value` ao invés de `result.IsSuccess` ou `result.IsFailure`, e comente qualquer uso de propriedades como `result.Message` ou `result.Errors`, se houver, para evitar inconsistências.\n"
            "- Minimize erros de sintaxe, conversão de objetos, e foco nas implementações de mock e retorno assíncrono como nos testes já existentes.\n"
            "- Ao criar testes, use a estrutura original de mock e requisições conforme mostrado nos testes existentes. Não altere a estrutura para usar `Fixture` ou outra abordagem diferente da que já está sendo usada, como a configuração do mock, chamadas ao banco de dados e API. O código deve ser o mais fiel possível ao exemplo fornecido.\n"
            "- **Não utilize `DbSet` para simular interações com o banco de dados.** Use a abordagem exata dos testes fornecidos, que configuram as interações com o banco de dados ou API de acordo com a estrutura original dos testes, incluindo como os mocks são configurados para garantir que os testes sejam executados corretamente."
        ),
        expected_output=(
            "Um arquivo C# contendo uma suíte de testes xUnit abrangente, bem organizada e funcional, com pelo menos 25 testes que cobrem todos os métodos públicos e cenários relevantes. "
            "Se houver métodos privados, os testes devem incluir abordagens adequadas para validá-los sem modificar a classe original. "
            "O código gerado deve ser testável, executável e baseado apenas em métodos e propriedades reais da classe em questão. "
            "Todas as instâncias de classes devem ser criadas utilizando o namespace completo `Entities.CadastroModalidadesBolsas.`."
        ),
        agent=test_generator_agent
    )

    total_tokens_processed += estimate_tokens(generate_test_task.description)

    test_crew = Crew(
        agents=[test_generator_agent],
        tasks=[generate_test_task],
        verbose=True
    )

    test_results = test_crew.kickoff()
    total_tokens_processed += estimate_tokens(str(test_results))
    
    test_output_file = output_directory / f"{cs_file.stem}Tests.cs"
    with open(test_output_file, 'w', encoding='utf-8') as file:
        file.write(str(test_results))

print(f"Total de tokens estimados: {total_tokens_processed}")
