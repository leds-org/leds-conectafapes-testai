Feature: Listar Modalidades

  Background:
    Given o sistema está ativo

  Scenario Outline: Listar modalidades com sucesso
    When o servidor lista as modalidades
    Then o sistema deve exibir as modalidades com as seguintes informações:
      | Sigla | Resolução Ativa | Nome da Versão Ativa | Em Edição |
      | <sigla> | <resolução_ativa> | <nome_versao_ativa> | <em_edicao> |

    Examples:
      | sigla | resolução_ativa | nome_versao_ativa | em_edicao |
      | ABC   | Res1              | Nome da Versão 1   | false       |
      | DEF   | Res2              | Nome da Versão 2   | true        |

  Scenario Outline: Filtrar modalidades por texto com sucesso
    When o servidor filtra as modalidades pelo texto "<texto>"
    Then o sistema deve exibir as modalidades que contém "<texto>" no nome ou sigla
    And o sistema deve exibir as informações:
      | Sigla | Resolução Ativa | Nome da Versão Ativa | Em Edição |
      | <sigla> | <resolução_ativa> | <nome_versao_ativa> | <em_edicao> |

    Examples:
      | texto | sigla | resolução_ativa | nome_versao_ativa | em_edicao |
      | ABC   | ABC   | Res1              | Nome da Versão 1   | false       |
      | Versão | DEF   | Res2              | Nome da Versão 2   | true        |

  Scenario Outline: Listar modalidades com erro
    When o servidor lista as modalidades
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | mensagem_erro |
      | Erro ao listar modalidades |

  Scenario Outline: Filtrar modalidades com erro
    When o servidor filtra as modalidades pelo texto "<texto>"
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | texto | mensagem_erro |
      |  | Erro ao filtrar modalidades |

  Scenario Outline: Selecionar modalidade com sucesso
    When o servidor seleciona a modalidade com sigla "<sigla>"
    Then o sistema deve exibir os detalhes da modalidade com sigla "<sigla>"

    Examples:
      | sigla |
      | ABC   |

  Scenario Outline: Selecionar modalidade com erro
    When o servidor seleciona a modalidade com sigla "<sigla>"
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | mensagem_erro |
      | XYZ   | Modalidade não encontrada |

Feature: Incluir Modalidade

  Background:
    Given o sistema está ativo

  Scenario Outline: Incluir modalidade com sucesso
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e salva a modalidade
    Then o sistema deve salvar a modalidade com status "Em edição"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução |
      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            | Res1      |

  Scenario Outline: Incluir modalidade com erro
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode salvar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro               |
      | ABC   | Nome | Desc      | -10        | 2024-01-01  | Bolsa1            | Res1      | Percentual não pode ser negativo |
      |  | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            | Res1      | Sigla da modalidade é obrigatória |
      | ABC   |  | Desc      | 10         | 2024-01-01  | Bolsa1            | Res1      | Nome da modalidade é obrigatório |
      | ABC   | Nome |  | 10         | 2024-01-01  | Bolsa1            | Res1      | Descrição da modalidade é obrigatória |
      | ABC   | Nome | Desc      |  | 2024-01-01  | Bolsa1            | Res1      | Percentual da modalidade é obrigatório |
      | ABC   | Nome | Desc      | 10         |  | Bolsa1            | Res1      | Data de início da modalidade é obrigatória |
      | ABC   | Nome | Desc      | 10         | 2024-01-01  |  | Res1      | Tipo de bolsa é obrigatório |
      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            |  | Resolução da modalidade é obrigatória |
      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            | Res3      | Resolução inválida |
      | ABC   | Nome | Desc      | 10         | 2023-12-31  | Bolsa1            | Res1      | Data de início inválida |
      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa2            | Res1      | Tipo de bolsa inválido |

  Scenario Outline: Incluir modalidade com erro - regra de integridade
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode salvar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                         |
      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            | Res1      | Já existe uma modalidade com a sigla "ABC" e status "Em edição" | 

Feature: Editar Modalidade

  Background:
    Given o sistema está ativo
    And a modalidade com sigla "ABC" e status "Em edição" está cadastrada

  Scenario Outline: Editar modalidade com sucesso
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e edita a modalidade
    Then o sistema deve atualizar a modalidade com status "Em edição"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      |

  Scenario Outline: Editar modalidade com erro
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode editar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro               |
      | ABC   | Nome1 | Desc1     | -11        | 2024-02-01  | Bolsa2            | Res2      | Percentual não pode ser negativo |
      |  | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Sigla da modalidade é obrigatória |
      | ABC   |  | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Nome da modalidade é obrigatório |
      | ABC   | Nome1 |  | 11         | 2024-02-01  | Bolsa2            | Res2      | Descrição da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     |  | 2024-02-01  | Bolsa2            | Res2      | Percentual da modalidade é obrigatório |
      | ABC   | Nome1 | Desc1     | 11         |  | Bolsa2            | Res2      | Data de início da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  |  | Res2      | Tipo de bolsa é obrigatório |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            |  | Resolução da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res3      | Resolução inválida |
      | ABC   | Nome1 | Desc1     | 11         | 2023-12-31  | Bolsa2            | Res2      | Data de início inválida |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa3            | Res2      | Tipo de bolsa inválido |
      | DEF   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Modalidade com sigla "DEF" não encontrada |

  Scenario Outline: Editar modalidade com erro - regra de integridade
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode editar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                         |
      | DEF   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Já existe uma modalidade com a sigla "DEF" e status "Em edição" | 

Feature: Publicar Modalidade

  Background:
    Given o sistema está ativo
    And a modalidade com sigla "ABC" e status "Em edição" está cadastrada

  Scenario Outline: Publicar modalidade com sucesso
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e publica a modalidade
    Then o sistema deve atualizar a modalidade com status "Ativa"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      |

  Scenario Outline: Publicar modalidade com erro
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode publicar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro               |
      | ABC   | Nome1 | Desc1     | -11        | 2024-02-01  | Bolsa2            | Res2      | Percentual não pode ser negativo |
      |  | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Sigla da modalidade é obrigatória |
      | ABC   |  | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Nome da modalidade é obrigatório |
      | ABC   | Nome1 |  | 11         | 2024-02-01  | Bolsa2            | Res2      | Descrição da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     |  | 2024-02-01  | Bolsa2            | Res2      | Percentual da modalidade é obrigatório |
      | ABC   | Nome1 | Desc1     | 11         |  | Bolsa2            | Res2      | Data de início da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  |  | Res2      | Tipo de bolsa é obrigatório |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            |  | Resolução da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res3      | Resolução inválida |
      | ABC   | Nome1 | Desc1     | 11         | 2023-12-31  | Bolsa2            | Res2      | Data de início inválida |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa3            | Res2      | Tipo de bolsa inválido |
      | DEF   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Modalidade com sigla "DEF" não encontrada |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Modalidade já está ativa |

  Scenario Outline: Publicar modalidade com erro - regra de integridade
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode publicar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                         |
      | DEF   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Já existe uma modalidade com a sigla "DEF" e status "Ativa" | 

Feature: Desativar Modalidade

  Background:
    Given o sistema está ativo
    And a modalidade com sigla "ABC" e status "Ativa" está cadastrada

  Scenario Outline: Desativar modalidade com sucesso
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e desativa a modalidade
    Then o sistema deve atualizar a modalidade com status "Desativada"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      |

  Scenario Outline: Desativar modalidade com erro
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode desativar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro               |
      | ABC   | Nome1 | Desc1     | -11        | 2024-02-01  | Bolsa2            | Res2      | Percentual não pode ser negativo |
      |  | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Sigla da modalidade é obrigatória |
      | ABC   |  | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Nome da modalidade é obrigatório |
      | ABC   | Nome1 |  | 11         | 2024-02-01  | Bolsa2            | Res2      | Descrição da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     |  | 2024-02-01  | Bolsa2            | Res2      | Percentual da modalidade é obrigatório |
      | ABC   | Nome1 | Desc1     | 11         |  | Bolsa2            | Res2      | Data de início da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  |  | Res2      | Tipo de bolsa é obrigatório |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            |  | Resolução da modalidade é obrigatória |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res3      | Resolução inválida |
      | ABC   | Nome1 | Desc1     | 11         | 2023-12-31  | Bolsa2            | Res2      | Data de início inválida |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa3            | Res2      | Tipo de bolsa inválido |
      | DEF   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Modalidade com sigla "DEF" não encontrada |
      | ABC   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Modalidade já está desativada |

  Scenario Outline: Desativar modalidade com erro - regra de integridade
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o sistema valida e não pode desativar a modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                         |
      | DEF   | Nome1 | Desc1     | 11         | 2024-02-01  | Bolsa2            | Res2      | Já existe uma modalidade com a sigla "DEF" e status "Desativada" |