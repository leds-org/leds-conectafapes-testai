Feature: Consultar Resolução

  Scenario Outline: Consultar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve exibir os dados da resolução:
      | Campo           | Valor        |
      |-----------------|--------------|
      | Número          | <número>     |
      | Data de Publicação | <data_publicacao> |
      | Assunto         | <assunto>    |
      | Conteúdo        | <conteudo>   |

    Examples:
      | resolução | número | data_publicacao | assunto        | conteudo   |
      | Res1      | 123   | 2024-01-01       | Resolução 123 | Conteúdo 1 |
      | Res2      | 456   | 2024-02-15       | Resolução 456 | Conteúdo 2 |

  Scenario Outline: Consultar resolução com erro
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro                               |
      | Res3      | Resolução não encontrada                     |
      | Res4      | O servidor não tem permissão para acessar esta resolução |
      | Res5      | Resolução inválida                           |