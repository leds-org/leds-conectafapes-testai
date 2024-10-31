Feature: Alterar Resolução

  Scenario Outline: Alterar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor informa os dados da resolução "<sigla>", "<nome>", "<descrição>", "<percentual>", "<data_início>", "<modalidades_bolsa>"
    When o sistema valida e salva as alterações da resolução
    Then o sistema deve atualizar a resolução com status "Em edição"

    Examples:
      | resolução | sigla | nome | descrição | percentual | data_início | modalidades_bolsa |
      | Res1      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            |

  Scenario Outline: Alterar resolução com erro - Atributo faltante
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor informa os dados da resolução <sigla>, <nome>, <descrição>, <percentual>, <data_início>, <modalidades_bolsa>
    When o sistema valida e não pode salvar as alterações da resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | mensagem_erro                                                                                                                              |
      | Res1      |       | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            | A sigla da resolução é obrigatória                                                                                                                              |
      | Res1      | ABC   |       | Desc      | 10         | 2024-01-01  | Bolsa1            | O nome da resolução é obrigatório                                                                                                                               |
      | Res1      | ABC   | Nome |           | 10         | 2024-01-01  | Bolsa1            | A descrição da resolução é obrigatória                                                                                                                            |
      | Res1      | ABC   | Nome | Desc      |           | 2024-01-01  | Bolsa1            | O percentual da resolução é obrigatório                                                                                                                           |
      | Res1      | ABC   | Nome | Desc      | 10         |             | Bolsa1            | A data de início da resolução é obrigatória                                                                                                                         |
      | Res1      | ABC   | Nome | Desc      | 10         | 2024-01-01  |                 | As modalidades de bolsa da resolução são obrigatórias                                                                                                           |

  Scenario Outline: Alterar resolução com erro - Regra de integridade
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor informa os dados da resolução "<sigla>", "<nome>", "<descrição>", "<percentual>", "<data_início>", "<modalidades_bolsa>"
    When o sistema valida e não pode salvar as alterações da resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | mensagem_erro                                                                                                                                               |
      | Res1      | ABC   | Nome | Desc      | -10        | 2024-01-01  | Bolsa1            | O percentual da resolução deve ser um valor positivo.                                                                                                            |
      | Res1      | ABC   | Nome | Desc      | 10         | 2023-12-31  | Bolsa1            | A data de início da resolução deve ser maior que a data de início da resolução anterior.                                                                      |
      | Res1      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa2            | As modalidades de bolsa da resolução devem estar dentro do conjunto de modalidades de bolsa permitidas.                                                               |