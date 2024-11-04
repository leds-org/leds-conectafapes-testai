Feature: Incluir Modalidade

  Scenario Outline: Incluir modalidade com sucesso
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o servidor inclui os requisitos da versão da modalidade
    Then o sistema deve salvar a modalidade com status "Em edição"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução |
      | ABC   | Nome | Descrição | 10         | 2024-01-01  | Bolsa1            | Res1      |

  Scenario Outline: Incluir modalidade com erro - Atributo faltante
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o servidor inclui os requisitos da versão da modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                                                                   |
      |       | Nome | Descrição | 10         | 2024-01-01  | Bolsa1            | Res1      | A sigla da modalidade é obrigatória.                                                              |
      | ABC   |       | Descrição | 10         | 2024-01-01  | Bolsa1            | Res1      | O nome da modalidade é obrigatório.                                                                |
      | ABC   | Nome |           | 10         | 2024-01-01  | Bolsa1            | Res1      | A descrição da modalidade é obrigatória.                                                           |
      | ABC   | Nome | Descrição |           | 2024-01-01  | Bolsa1            | Res1      | O percentual de redução por vínculo é obrigatório.                                                    |
      | ABC   | Nome | Descrição | 10         |             | Bolsa1            | Res1      | A data de início da vigência da modalidade é obrigatória.                                         |
      | ABC   | Nome | Descrição | 10         | 2024-01-01  |                 | Res1      | As modalidades de bolsa compatíveis com a modalidade são obrigatórias.                               |
      | ABC   | Nome | Descrição | 10         | 2024-01-01  | Bolsa1            |           | A resolução que define a modalidade é obrigatória.                                                    |

  Scenario Outline: Incluir modalidade com erro - Violação de regra de integridade
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o servidor inclui os requisitos da versão da modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                                                         |
      | ABC   | Nome | Descrição | -10        | 2024-01-01  | Bolsa1            | Res1      | O percentual de redução por vínculo deve ser um valor positivo.                             |
      | ABC   | Nome | Descrição | 10         | 2023-12-31  | Bolsa1            | Res1      | A data de início da vigência da modalidade deve ser uma data futura ou a data atual. |

  Scenario Outline: Incluir modalidade com erro - Duplicidade
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o servidor inclui os requisitos da versão da modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                               |
      | ABC   | Nome | Descrição | 10         | 2024-01-01  | Bolsa1            | Res1      | Já existe uma modalidade com a sigla 'ABC'.                    |

  Scenario Outline: Incluir modalidade com erro - Formato inválido
    Given o servidor informa os dados da modalidade <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    And o servidor seleciona a resolução <resolução>
    When o servidor inclui os requisitos da versão da modalidade
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | resolução | mensagem_erro                                                                                                                         |
      | ABC   | Nome | Descrição | 10.5       | 2024-01-01  | Bolsa1            | Res1      | O percentual de redução por vínculo deve ser um número inteiro.                                                                      |
      | ABC   | Nome | Descrição | 10         | 2024-01-32  | Bolsa1            | Res1      | A data de início da vigência da modalidade deve estar em um formato válido (AAAA-MM-DD).                                          |
      | ABC   | Nome | Descrição | 10         | 2024-01-01  | Bolsa2, Bolsa3    | Res1      | As modalidades de bolsa compatíveis com a modalidade devem ser um array de strings (separadas por vírgula, sem espaços). |