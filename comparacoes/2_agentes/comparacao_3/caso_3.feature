Feature: Consultar Resolução

  Scenario Outline: Consultar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve exibir os dados da resolução:
      | Campo             | Valor     |
      | Resolução         | <resolução> |
      | Nome               | <nome>     |
      | Descrição         | <descrição> |
      | Data de Publicação | <data>       |

    Examples:
      | resolução | nome         | descrição      | data       |
      | Res1      | Resolução 1 | Descrição 1   | 2023-10-27 |
      | Res2      | Resolução 2 | Descrição 2   | 2023-11-01 |
      | Res3      | Resolução 3 | Descrição 3   | 2023-11-15 |

  Scenario Outline: Consultar resolução com erro - Resolução inexistente
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro                                         |
      | Res999    | Resolução não encontrada.                             |
      | Res000    | Resolução inválida.                                   |
      | Res456    | Resolução inexistente.                                |