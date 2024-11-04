Feature: Consultar Resolução

  Scenario Outline: Consultar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve exibir os dados da resolução:
      | Campo             | Valor        |
      | ---------------- | ------------- |
      | Número da resolução | <número>       |
      | Data da resolução  | <data>        |
      | Assunto           | <assunto>     |
      | Conteúdo         | <conteúdo>   |

    Examples:
      | resolução | número | data        | assunto    | conteúdo |
      | Res1      | 10     | 2024-01-01 | Assunto 1 | Conteúdo 1 |

  Scenario Outline: Consultar resolução com erro
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro                                          |
      | Res2      | Resolução não encontrada.                              |
      | Res3      | Ocorreu um erro ao consultar a resolução.             |
      | Res4      | Você não tem permissão para consultar esta resolução. |