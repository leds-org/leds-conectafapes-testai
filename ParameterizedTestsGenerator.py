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
    model='gemini/gemini-1.5-pro',
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
        "### Certifique-se de que os testes gerados sigam a mesma estrutura e estilo dos exemplos funcionais fornecidos, como o código abaixo:\n"
        f"{existing_test_content}\n\n"
        "- Use esse exemplo como referência para garantir que os testes gerados sejam consistentes, funcionais e alinhados com as práticas já estabelecidas.\n"
        "### Requisitos para os testes:\n"
        "1. Verifique a acessibilidade dos métodos antes de criar os testes.\n"
        "2. Use o framework xUnit para todos os testes.\n"
        "3. Garanta cobertura completa de todos os métodos públicos.\n"
        "4. Inclua casos de teste para caminho feliz, valores de limite, e cenários de erro.\n"
        "5. Utilize Moq ou NSubstitute para simular dependências externas. Configure os mocks de forma consistente com os testes existentes, especialmente ao simular `requests`, `Returns`, e outras interações. Não use métodos assíncronos como `ReturnsAsync` ou `CreateAsync`. Siga a estrutura de mock já utilizada nos testes funcionais fornecidos.\n"
        "6. Siga o padrão de nomenclatura MethodName_StateUnderTest_ExpectedBehavior.\n"
        "7. Organize os testes em classes lógicas.\n"
        "8. Utilize IClassFixture ou IDisposable para configuração e limpeza de recursos, se necessário.\n"
        "9. Implemente testes data-driven com Theory e InlineData.\n"
        "10. Garanta isolamento entre os testes.\n"
        "11. A cobertura mínima deve ser de 90% do código.\n"
        "12. **Gere pelo menos 25 testes por arquivo.**\n"
        "13. **Não utilize métodos assíncronos.** Evite o uso de métodos como `CreateAsync` e outros métodos assíncronos nas simulações de mocks e chamadas. Ao invés disso, utilize os testes já existentes como exemplo para configuração, incluindo a criação de `requests` e `entities` como mostrado.\n"
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
        "- Minimize erros de sintaxe, conversão de objetos, e foco nas implementações de mock e retorno direto como nos testes já existentes.\n"
        "- Ao criar testes, use a estrutura original de mock e requisições conforme mostrado nos testes existentes. Não altere a estrutura para usar `Fixture` ou outra abordagem diferente da que já está sendo usada, como a configuração do mock, chamadas ao banco de dados e API. O código deve ser o mais fiel possível ao exemplo fornecido.\n"
        "- **Não utilize `DbSet` para simular interações com o banco de dados.** Use a abordagem exata dos testes fornecidos, que configuram as interações com o banco de dados ou API de acordo com a estrutura original dos testes, incluindo como os mocks são configurados para garantir que os testes sejam executados corretamente.\n"
        "- **Não use métodos assíncronos** como `CreateAsync` nas simulações ou nas interações. Siga a estrutura do teste existente que utiliza uma abordagem de requisição simples e simulação de retorno direto sem métodos assíncronos.\n"
        "- Exemplo de configuração de mock válida: `_repositoryMock.Setup(r => r.SiglaExiste(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(false);`. Observe que o retorno é direto (`Returns`) e não assíncrono (`ReturnsAsync`).\n"
        "- Sempre valide os resultados utilizando `Assert` com mensagens claras e descritivas.\n"
        "\n"
        "### Estrutura de Testes:\n"
        "- **Request:** É **OBRIGATÓRIO** que cada teste contenha um objeto `request` que represente a entrada do método sob teste. Esse objeto deve ser instanciado usando o DTO correspondente (por exemplo, `ResolucaoRequestDTO`).\n"
        "- **Entity:** É **OBRIGATÓRIO** que para cada teste seja configurado um objeto `entity` que represente a entidade de domínio correspondente (por exemplo, `Resolucao`). Esse objeto deve ser configurado no mock do mapeador (`_mapperMock`) para garantir que o serviço funcione corretamente.\n"
        "- **Mock de Dependências:** Configure os mocks das dependências (como repositórios e mapeadores) de forma a refletir o comportamento esperado do sistema. Por exemplo:\n"
        "  - `_repositoryMock.Setup(x => x.CheckIfNumRastreioExists(...)).Returns(true);`\n"
        "  - `_mapperMock.Setup(x => x.Map<Entity>(request)).Returns(entity);`\n"
        "- **Validação de Resultados:** Use `Assert` para verificar o comportamento esperado do método, como o sucesso ou falha da operação, mensagens de erro específicas, ou valores retornados.\n"
        "\n"
        "### Exemplo de Teste Completo:\n"
        "```csharp\n"
        "[Fact]\n"
        "public async Task Create_NumRastreioExists_ReturnsBadRequest()\n"
        "{\n"
        "    // Arrange\n"
        "    var request = new ResolucaoRequestDTO { NumRastreioEdocs = \"123\" };\n"
        "    var entity = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao { NumRastreioEdocs = \"123\" };\n"
        "    _repositoryMock.Setup(x => x.CheckIfResolucaoExists(It.IsAny<int>(), It.IsAny<CancellationToken>()))\n"
        "                   .Returns(Task.FromResult(false));\n"
        "    _repositoryMock.Setup(x => x.CheckIfNumRastreioExists(It.IsAny<string>(), It.IsAny<CancellationToken>()))\n"
        "                   .Returns(Task.FromResult(true));\n"
        "    _mapperMock.Setup(x => x.Map<ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao>(request))\n"
        "               .Returns(entity);\n"
        "\n"
        "    // Act\n"
        "    var result = await _resolucaoService.Create(request, CancellationToken.None);\n"
        "\n"
        "    // Assert\n"
        "    Assert.False(result.IsSuccess);\n"
        "    Assert.Contains(\"O número de rastreio já está em uso!\", result.Errors.Select(e => e.Message));\n"
        "}\n"
        "```\n"
        "\n"
        "### Diretrizes Finais:\n"
        "- **Objetivo Principal:** Garanta que os testes sejam executáveis, funcionais e consistentes com os exemplos fornecidos.\n"
        "- **Evite Uso de `DbSet`:** Nunca use `DbSet` ou qualquer outra abordagem relacionada ao Entity Framework para simular interações com o banco de dados. Em vez disso, use mocks configurados diretamente para simular o comportamento esperado.\n"
        "- **Use Mocks Diretos:** Configure os mocks para retornar valores diretamente (usando `Returns`) e evite métodos assíncronos (`ReturnsAsync`).\n"
        "- **Teste Todos os Cenários:** Inclua testes para cenários positivos (caminho feliz), negativos (erros) e casos de borda.\n"
        "- **Mensagens Claras:** Use mensagens descritivas em todas as asserções para facilitar a depuração.\n"
        "- **Isolamento Total:** Garanta que cada teste seja independente e não dependa de estado compartilhado.\n"
        "- **Estrutura Obrigatória:** É **MANDATÓRIO** seguir a estrutura de `request` (DTO) e `entity` em todos os testes. Essa abordagem garante consistência, resiliência e reflexo do fluxo real do sistema. Mesmo em cenários onde o `request` ou `entity` pareça desnecessário, configure-os para evitar problemas de nulidade e garantir que o teste seja resiliente a mudanças futuras.\n"
        "- **Foco em Métodos de Criação:** \n"
        "  - **Implemente apenas os testes para métodos de criação (`Create`)**. Esses testes devem ser totalmente funcionais e executáveis.\n"
        "  - **Para métodos de atualização (`Update`) e exclusão (`Delete`):** Os testes podem ser criados, mas devem ser deixados em formato de comentário. Isso ocorre porque essas funcionalidades ainda não foram implementadas no código original, mas os testes comentados podem ser úteis no futuro quando essas funcionalidades forem desenvolvidas.\n"
        "  - Exemplo de teste comentado para `Update`:\n"
        "    ```csharp\n"
        "    /*\n"
        "    [Fact]\n"
        "    public async Task Update_ResolucaoNotFound_ReturnsNotFound()\n"
        "    {\n"
        "        // Arrange\n"
        "        var request = new ResolucaoRequestDTO { Id = Guid.NewGuid(), NumRastreioEdocs = \"123\" };\n"
        "        var entity = new ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao { Id = request.Id, NumRastreioEdocs = request.NumRastreioEdocs };\n"
        "        _repositoryMock.Setup(x => x.GetById(It.IsAny<Guid>()))\n"
        "                       .Returns(new List<Resolucao>().AsQueryable());\n"
        "        _mapperMock.Setup(x => x.Map<ConectaFapes.Domain.Entities.CadastroModalidadesBolsas.Resolucao>(request))\n"
        "                   .Returns(entity);\n"
        "\n"
        "        // Act\n"
        "        var result = await _resolucaoService.Update(request, CancellationToken.None);\n"
        "\n"
        "        // Assert\n"
        "        Assert.False(result.IsSuccess);\n"
        "        Assert.Contains(\"A Resolução não existe!\", result.Errors.Select(e => e.Message));\n"
        "    }\n"
        "    */\n"
        "    ```\n"
        "  - Exemplo de teste comentado para `Delete`:\n"
        "    ```csharp\n"
        "    /*\n"
        "    [Fact]\n"
        "    public async Task Delete_ResolucaoNotFound_ReturnsNotFound()\n"
        "    {\n"
        "        // Arrange\n"
        "        var id = Guid.NewGuid();\n"
        "        _repositoryMock.Setup(x => x.GetById(id))\n"
        "                       .Returns(new List<Resolucao>().AsQueryable());\n"
        "\n"
        "        // Act\n"
        "        var result = await _resolucaoService.Delete(id, CancellationToken.None);\n"
        "\n"
        "        // Assert\n"
        "        Assert.False(result.IsSuccess);\n"
        "        Assert.Contains(\"A resolução não existe!\", result.Errors.Select(e => e.Message));\n"
        "    }\n"
        "    */\n"
        "    ```\n"
    ),
    expected_output=(
        "Um arquivo C# contendo uma suíte de testes xUnit abrangente, bem organizada e funcional, com pelo menos 25 testes que cobrem todos os métodos públicos e cenários relevantes. "
        "Se houver métodos privados, os testes devem incluir abordagens adequadas para validá-los sem modificar a classe original. "
        "O código gerado deve ser testável, executável e baseado apenas em métodos e propriedades reais da classe em questão. "
        "Todas as instâncias de classes devem ser criadas utilizando o namespace completo `Entities.CadastroModalidadesBolsas.`. "
        "Os testes devem seguir a mesma estrutura e estilo dos exemplos funcionais fornecidos, utilizando mocks configurados de forma consistente e retornos diretos (não assíncronos)."
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
