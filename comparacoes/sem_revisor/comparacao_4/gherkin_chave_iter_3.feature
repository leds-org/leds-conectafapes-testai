Feature: Excluir Resolução

  Scenario Outline: Excluir resolução com sucesso
    Given o servidor seleciona a resolução "<resolucao>"
    When o servidor confirma a exclusão da resolução
    Then o sistema deve excluir a resolução
    And o sistema deve retornar uma mensagem de sucesso "Resolução excluída com sucesso"

    Examples:
      | resolucao |
      | Res1       |

  Scenario Outline: Excluir resolução com erro - Resolução não encontrada
    Given o servidor seleciona a resolução "<resolucao>"
    When o servidor confirma a exclusão da resolução
    Then o sistema não deve excluir a resolução
    And o sistema deve retornar uma mensagem de erro "Resolução não encontrada"

    Examples:
      | resolucao |
      | Res2       |

  Scenario Outline: Excluir resolução com erro - Resolução com modalidades associadas
    Given o servidor seleciona a resolução "<resolucao>"
    When o servidor confirma a exclusão da resolução
    Then o sistema não deve excluir a resolução
    And o sistema deve retornar uma mensagem de erro "Resolução possui modalidades associadas"

    Examples:
      | resolucao |
      | Res3       |