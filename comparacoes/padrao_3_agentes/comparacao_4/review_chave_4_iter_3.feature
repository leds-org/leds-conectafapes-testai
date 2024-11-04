Feature: Excluir Resolução

  Scenario Outline: Excluir resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor confirma a exclusão da resolução
    When o sistema exclui a resolução
    Then o sistema deve exibir uma mensagem de sucesso "Resolução excluída com sucesso."

    Examples:
      | resolução |
      | Resolução 1 |
      | Resolução 2 |
      | Resolução 3 |

  Scenario Outline: Excluir resolução com erro
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor confirma a exclusão da resolução
    When o sistema não consegue excluir a resolução
    Then o sistema deve exibir uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro |
      | Resolução 4 | A resolução não pode ser excluída porque possui modalidades associadas. |
      | Resolução 5 | A resolução não existe. |

  Scenario: Excluir resolução sem informar a resolução
    Given o servidor não informa a resolução
    When o servidor confirma a exclusão da resolução
    Then o sistema deve exibir uma mensagem de erro "A resolução deve ser informada."

  Scenario: Excluir resolução com modalidade associada
    Given o servidor seleciona a resolução "<resolução>" com modalidade associada
    And o servidor confirma a exclusão da resolução
    Then o sistema deve exibir uma mensagem de erro "A resolução não pode ser excluída porque possui modalidades associadas."