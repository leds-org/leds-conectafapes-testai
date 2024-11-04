Feature: Alterar Resolução

  Scenario Outline: Alterar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor informa os dados da resolução "<sigla>", "<nome>", "<descrição>", "<percentual>", "<data_início>", "<modalidades_bolsa>"
    When o servidor salva as alterações da resolução
    Then o sistema deve atualizar a resolução com status "Em edição"

    Examples:
      | resolução | sigla | nome | descrição | percentual | data_início | modalidades_bolsa |
      | Res1      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1            |
      | Res2      | DEF   | Nome2 | Desc2     | 20         | 2025-02-02  | Bolsa2, Bolsa3     |

  Scenario Outline: Alterar resolução com erro - Atributo faltante
    Given o servidor seleciona a resolução "<resolução>"
    When o servidor informa os dados da resolução com "<atributo_faltante>" faltando
    And o servidor salva as alterações da resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | atributo_faltante | mensagem_erro                                                                                                                              |
      | Res1      | sigla              | A sigla da resolução é obrigatória                                                                                                                              |
      | Res2      | nome               | O nome da resolução é obrigatório                                                                                                                               |
      | Res3      | descrição          | A descrição da resolução é obrigatória                                                                                                                            |
      | Res4      | percentual         | O percentual da resolução é obrigatório                                                                                                                           |
      | Res5      | data_início        | A data de início da resolução é obrigatória                                                                                                                         |
      | Res6      | modalidades_bolsa | As modalidades de bolsa da resolução são obrigatórias                                                                                                           |

  Scenario Outline: Alterar resolução com erro - Regra de integridade
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor informa os dados da resolução "<sigla>", "<nome>", "<descrição>", "<percentual>", "<data_início>", "<modalidades_bolsa>"
    When o servidor salva as alterações da resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | sigla | nome | descrição | percentual | data_início | modalidades_bolsa | mensagem_erro                                                                                                                                               |
      | Res1      | ABC   | Nome | Desc      | -10        | 2024-01-01  | Bolsa1            | O percentual da resolução deve ser um valor positivo.                                                                                                            |
      | Res2      | ABC   | Nome | Desc      | 10         | 2023-12-31  | Bolsa1            | A data de início da resolução deve ser maior que a data de início da resolução anterior.                                                                      |
      | Res3      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa4            | As modalidades de bolsa da resolução devem estar dentro do conjunto de modalidades de bolsa permitidas.                                                               |
      | Res4      | ABC   | Nome | Desc      | 10         | 2024-01-01  | Bolsa1, Bolsa5     | As modalidades de bolsa da resolução devem estar dentro do conjunto de modalidades de bolsa permitidas.                                                               |