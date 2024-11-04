Feature: Listar Modalidades

  Scenario Outline: Listar modalidades com sucesso
    Given o servidor Fapes acessa a tela de listagem de modalidades
    When o servidor Fapes solicita a listagem de modalidades
    Then o sistema deve exibir a lista de modalidades
    And cada modalidade deve conter:
      * Sigla: <sigla>
      * Nome: <nome>
      * Resolução da versão ativa: <resolução>
      * Nome da versão ativa: <nome_versao>
      * Indicativo de versão em edição: <em_edicao>
    
    Examples:
      | sigla | nome | resolução | nome_versao | em_edicao |
      | ABC   | Nome da Modalidade 1 | Resolução 1 | Versão Ativa 1 | Sim |
      | DEF   | Nome da Modalidade 2 | Resolução 2 | Versão Ativa 2 | Não |

  Scenario Outline: Listar modalidades com filtro de texto
    Given o servidor Fapes acessa a tela de listagem de modalidades
    And o servidor Fapes informa o filtro de texto "<filtro>"
    When o servidor Fapes solicita a listagem de modalidades
    Then o sistema deve exibir a lista de modalidades filtradas
    And cada modalidade deve conter:
      * Sigla: <sigla>
      * Nome: <nome>
      * Resolução da versão ativa: <resolução>
      * Nome da versão ativa: <nome_versao>
      * Indicativo de versão em edição: <em_edicao>
    
    Examples:
      | filtro | sigla | nome | resolução | nome_versao | em_edicao |
      | ABC   | ABC   | Nome da Modalidade 1 | Resolução 1 | Versão Ativa 1 | Sim |
      | 1     | DEF   | Nome da Modalidade 2 | Resolução 2 | Versão Ativa 2 | Não |

  Scenario: Selecionar modalidade
    Given o servidor Fapes acessa a tela de listagem de modalidades
    When o servidor Fapes seleciona a modalidade com sigla "ABC"
    Then o sistema deve direcionar o servidor Fapes para a tela de detalhes da modalidade
    And a tela de detalhes da modalidade deve exibir os dados da modalidade selecionada

  Scenario: Selecionar modalidade inexistente
    Given o servidor Fapes acessa a tela de listagem de modalidades
    When o servidor Fapes seleciona a modalidade com sigla "XYZ"
    Then o sistema deve exibir uma mensagem de erro "Modalidade não encontrada"

  Scenario Outline: Listar modalidades com erro
    Given o servidor Fapes acessa a tela de listagem de modalidades
    When o servidor Fapes solicita a listagem de modalidades
    Then o sistema deve exibir uma mensagem de erro "<mensagem_erro>"

    Examples:
      | mensagem_erro |
      | Erro ao listar modalidades |
      | Nenhuma modalidade encontrada |