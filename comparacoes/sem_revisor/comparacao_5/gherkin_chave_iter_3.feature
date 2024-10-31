Feature: Listar Modalidades

  Scenario Outline: Listar modalidades com sucesso
    Given o servidor Fapes acessa a tela de listagem de modalidades
    When o servidor Fapes solicita a listagem de modalidades
    Then o sistema exibe a lista de modalidades com as seguintes informações:
      | Sigla | Resolução | Nome | Versão Ativa | Em Edição |
      | <sigla> | <resolução> | <nome> | <versao_ativa> | <em_edicao> |
    Examples:
      | sigla | resolução | nome | versao_ativa | em_edicao |
      | ABC | 123 | Modalidade A | Ativa | Sim |
      | DEF | 456 | Modalidade B | Ativa | Não |

  Scenario Outline: Listar modalidades com filtro de texto
    Given o servidor Fapes acessa a tela de listagem de modalidades
    And o servidor Fapes informa o filtro de texto "<filtro>"
    When o servidor Fapes solicita a listagem de modalidades
    Then o sistema exibe a lista de modalidades filtradas por "<filtro>"
    Examples:
      | filtro |
      | Modalidade A |
      | DEF |

  Scenario Outline: Listar modalidades com erro
    Given o servidor Fapes acessa a tela de listagem de modalidades
    When o sistema retorna um erro
    Then o sistema exibe a mensagem de erro "<mensagem_erro>"
    Examples:
      | mensagem_erro |
      | Erro ao listar modalidades |

  Scenario Outline: Selecionar modalidade
    Given o servidor Fapes acessa a tela de listagem de modalidades
    And o servidor Fapes seleciona a modalidade com sigla "<sigla>"
    When o servidor Fapes realiza a seleção da modalidade
    Then o sistema exibe os detalhes da modalidade com sigla "<sigla>"
    Examples:
      | sigla |
      | ABC |

  Scenario Outline: Selecionar modalidade com erro
    Given o servidor Fapes acessa a tela de listagem de modalidades
    And o servidor Fapes seleciona a modalidade com sigla "<sigla>"
    When o sistema retorna um erro
    Then o sistema exibe a mensagem de erro "<mensagem_erro>"
    Examples:
      | sigla | mensagem_erro |
      | XYZ | Modalidade não encontrada |