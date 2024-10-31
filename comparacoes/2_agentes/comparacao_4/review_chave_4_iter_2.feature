Feature: Excluir Resolução

  Scenario Outline: Excluir resolução com sucesso
    Given o servidor Fapes seleciona a resolução "<resolução>"
    When o servidor confirma a exclusão da resolução
    Then o sistema deve excluir a resolução com sucesso

    Examples:
      | resolução |
      | Resolução 1 |
      | Resolução 2 |

  Scenario Outline: Excluir resolução com erro
    Given o servidor Fapes seleciona a resolução "<resolução>"
    When o servidor confirma a exclusão da resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro                                         |
      | Resolução 3 | A resolução não pode ser excluída pois possui modalidades associadas. |
      | Resolução 4 | A resolução não existe.                                  |
      | Resolução 5 | A resolução é obrigatória.                                 |
      | Resolução 6 | Erro ao excluir resolução.                                |