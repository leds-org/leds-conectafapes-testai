Feature: Consultar Resolução

  Scenario Outline: Consultar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve exibir os dados da resolução:
      | Campo             | Valor     |
      | Resolução         | <resolução> |
      | Nome               | <nome>     |
      | Descrição         | <descrição> |
      | Data de Publicação | <data>     |
      | Autor             | <autor>    |
      | Área de Aplicação | <area>     |

    Examples:
      | resolução | nome         | descrição      | data       | autor     | area              |
      | Res1      | Resolução 1 | Descrição 1   | 2023-10-27 | João Silva | Desenvolvimento |
      | Res2      | Resolução 2 | Descrição 2   | 2023-11-01 | Maria Santos | Pesquisa          |

  Scenario Outline: Consultar resolução com erro - Resolução inexistente
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve retornar uma mensagem de erro: "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro                                         |
      | Res999    | Resolução não encontrada.                             |
      | Res000    | Resolução inválida.                                   |

  Scenario: Consultar resolução com erro - Formato inválido
    Given o servidor seleciona a resolução "Res3"
    When o sistema consulta a resolução
    Then o sistema deve retornar uma mensagem de erro: "Formato de resolução inválido."