import tiktoken
from crewai import Agent, Task, Crew, LLM
import os
from dotenv import load_dotenv
from pathlib import Path

# Função para estimar o número de tokens em um texto
def estimate_tokens(text):
    encoder = tiktoken.encoding_for_model("gpt-4")
    return len(encoder.encode(text))

# Variável global para rastrear o total de tokens processados
total_tokens_processed = 0

# Carregar variáveis de ambiente
load_dotenv()

# Configurar o modelo de linguagem
language_model = LLM(
    model='gemini/gemini-1.5-flash',
    temperature=0.0,
    api_key=os.getenv("GOOGLE_API_KEY"),
)

# Definir o agente responsável pela geração de testes
test_generator_agent = Agent(
    name="Gerador de Testes xUnit",
    role="Engenheiro Sênior de Testes em C# e Especialista em xUnit",
    backstory="Profissional experiente com ampla vivência na criação de suítes de testes unitários usando xUnit para C#.",
    goal="Gerar uma suíte de testes unitários detalhada e abrangente em C# usando xUnit, garantindo alta cobertura de código e cenários.",
    llm=language_model
)

# Diretório contendo arquivos .cs
cs_directory = Path("leds-conectafapes-backend-admin-main/src/ConectaFapes/ConectaFapes.Domain/Entities/CadastroModalidadesBolsas")

# Verificar se o diretório existe
if not cs_directory.exists():
    raise FileNotFoundError(f"O diretório '{cs_directory}' não foi encontrado.")

# Processar cada arquivo .cs no diretório
for cs_file in cs_directory.glob("*.cs"):
    output_directory = cs_directory / f"{cs_file.stem}-Testes"
    output_directory.mkdir(exist_ok=True)

    # Ler o conteúdo do arquivo .cs
    with open(cs_file, 'r', encoding='utf-8') as file:
        cs_file_content = file.read()
        total_tokens_processed += estimate_tokens(cs_file_content)

    print(f"Conteúdo do arquivo: {cs_file.name}")
    print(cs_file_content)

    # Criar a tarefa para geração de testes
    generate_test_task = Task(
    description=(
            f"Com base no seguinte código C#, crie uma suíte de testes unitários abrangente usando xUnit:\n\n"
            f"{cs_file_content}\n\n"
            "### Requisitos para os testes:\n"
            "1. **Verifique a acessibilidade dos métodos antes de criar os testes**:\n"
            "   - Se o método for `public`, gere um teste normal.\n"
            "   - Se o método for `private` ou `protected`, adapte o código para testá-lo (por exemplo, usando reflexão ou criando métodos auxiliares).\n"
            "   - Se um método for inacessível para testes unitários diretos, documente isso claramente nos comentários do teste.\n"
            "2. **Uso do framework correto**: Todos os testes devem ser escritos usando **xUnit**.\n"
            "3. **Cobertura completa**: Teste todos os métodos públicos da classe, incluindo métodos estáticos, sobrecargas e métodos de extensão (se houver).\n"
            "4. **Casos de teste obrigatórios**:\n"
            "   - Caminho feliz (casos normais).\n"
            "   - Valores de limite (boundary values).\n"
            "   - Casos de erro (valores inválidos, `null`, exceções esperadas).\n"
            "5. **Mocks para dependências**: Use **Moq** ou **NSubstitute** para simular dependências externas (bancos de dados, APIs, serviços).\n"
            "6. **Padrões de nomenclatura**: Siga o formato **`MethodName_StateUnderTest_ExpectedBehavior`**.\n"
            "7. **Estrutura clara**: Organize os testes em classes lógicas, agrupando-os por funcionalidade ou método.\n"
            "8. **Setup e Teardown**: Utilize **`IClassFixture`** ou **`IDisposable`**, se necessário, para configuração e limpeza de recursos compartilhados.\n"
            "9. **Data-Driven Tests**: Use `[Theory]` com `[InlineData]` ou `[MemberData]` para testar diferentes cenários.\n"
            "10. **Isolamento dos testes**: Nenhum teste deve compartilhar estado global.\n"
            "11. **Cobertura mínima**: Certifique-se de que os testes cobrem **pelo menos 90% do código**, medido por ferramentas como **Coverlet** ou **dotCover**.\n"
            "12. **Mínimo de 25 testes**: Distribua os testes de maneira equilibrada entre os métodos.\n"
            "13. **Testes assíncronos**: Se aplicável, use `async/await` e `Task`.\n"
            "14. **Validação de exceções personalizadas**, se houver.\n"
            "15. **Eficiência**: Evite duplicação de código e mantenha os testes rápidos.\n"
            "16. **Imutabilidade**: Se houver objetos imutáveis, valide seu comportamento corretamente.\n"
            "17. **Testes de concorrência**: Se a classe usa `Task` ou `Thread`, verifique possíveis problemas de `race condition`.\n"
            "18. **Documentação interna**: Adicione comentários explicando a lógica de cada teste.\n"
            "19. **Boas práticas**: A suíte de testes deve seguir os princípios **SOLID** e **Clean Code**.\n"
            "\n"
            "### Restrições Estritas:\n"
            "20. **Verifique quais métodos e propriedades realmente existem no código antes de gerar os testes**.\n"
            "21. **Não invente métodos, propriedades ou comportamentos que não estejam explicitamente no código fornecido**.\n"
            "22. **Se houver dúvida sobre a existência de um método, não o inclua nos testes**.\n"
            "23. **Não modifique a assinatura dos métodos originais**.\n"
            "24. **Não assuma que um método lança exceções se isso não estiver claro no código**.\n"
            "25. **Os testes devem refletir apenas o que é definido no código original, sem extrapolações ou suposições não verificáveis**.\n"
            "26. **Se um método for privado ou protegido, adapte o teste para torná-lo executável, sem modificar a classe original**.\n"
            "27. **Garanta que todos os testes gerados sejam realmente executados e validados corretamente**.\n"
            "\n"
            "### Considerações Adicionais:\n"
            "- **Conferência de pronomes e gêneros**: Sempre verifique o gênero de classes, enums ou propriedades ao escrever testes. Por exemplo, se uma classe ou enum tiver um nome feminino, ajuste os testes para usar o pronome correto (ex.: 'EstadoVersaoModalidade.ATIVA' ao invés de 'EstadoVersaoModalidade.ATIVO').\n"
            "- **Uso de namespaces completos**: Ao instanciar **qualquer classe**, sempre utilize o namespace completo `Entities.CadastroModalidadesBolsas.`. Por exemplo, ao criar uma nova instância de `RequisitoBolsa`, use `new Entities.CadastroModalidadesBolsas.RequisitoBolsa()` em vez de apenas `new RequisitoBolsa()`. Essa regra deve ser aplicada consistentemente para todas as classes.\n"
            "- **Contexto explícito**: Sempre que possível, forneça contexto explícito para evitar ambiguidades. Isso inclui verificar nomes de variáveis, métodos e propriedades para garantir que eles correspondam exatamente ao código fornecido.\n"
            "- **Testes robustos**: Garanta que os testes sejam resilientes a mudanças futuras no código, desde que essas mudanças não alterem a lógica subjacente testada.\n"
            "- **Feedback claro**: Inclua mensagens de asserção claras e descritivas para facilitar a depuração em caso de falhas.\n"
        ),
        expected_output=(
            "Um arquivo C# contendo uma suíte de testes xUnit abrangente, bem organizada e funcional, com pelo menos 25 testes que cobrem todos os métodos públicos e cenários relevantes. "
            "Se houver métodos privados, os testes devem incluir abordagens adequadas para validá-los sem modificar a classe original. "
            "O código gerado deve ser **testável**, **executável** e **baseado apenas em métodos e propriedades reais** da classe em questão. "
            "Além disso, **todas as instâncias de classes** devem ser criadas utilizando o namespace completo `Entities.CadastroModalidadesBolsas.`. "
            "O uso de pronomes corretos e gêneros também deve ser consistente em toda a suíte de testes."
        ),
        agent=test_generator_agent
    )
    # Estimar tokens da descrição da tarefa
    total_tokens_processed += estimate_tokens(generate_test_task.description)

    # Criar a equipe para execução da tarefa
    test_crew = Crew(
        agents=[test_generator_agent],
        tasks=[generate_test_task],
        verbose=True
    )

    # Executar a tarefa e obter os resultados
    test_results = test_crew.kickoff()
    total_tokens_processed += estimate_tokens(str(test_results))

    # Salvar os testes gerados em um arquivo
    test_output_file = output_directory / f"{cs_file.stem}Tests.cs"
    with open(test_output_file, 'w', encoding='utf-8') as file:
        file.write(str(test_results))

    print(f"✅ Testes gerados para '{cs_file.name}'. Arquivo de testes: '{test_output_file}'.")

print("✅ Todos os arquivos .cs foram processados e os testes foram gerados com sucesso!")
print(f"Total de tokens estimados: {total_tokens_processed}")