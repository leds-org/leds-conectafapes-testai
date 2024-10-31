Feature: Alterar Resolução

  Scenario Outline: Alterar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor visualiza os dados da resolução:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla>            |
      | Nome          | <nome>             |
      | Descrição     | <descrição>        |
      | Percentual    | <percentual>       |
      | Data de Início | <data_início>       |
      | Modalidades de Bolsa | <modalidades_bolsa> |
    When o servidor altera a resolução com os dados:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla_nova>       |
      | Nome          | <nome_novo>        |
      | Descrição     | <descrição_nova>    |
      | Percentual    | <percentual_novo>  |
      | Data de Início | <data_início_nova> |
      | Modalidades de Bolsa | <modalidades_bolsa_nova> |
    Then o sistema deve salvar a resolução com sucesso
    And o sistema deve exibir uma mensagem de sucesso

    Examples:
      | resolução | sigla     | nome     | descrição     | percentual | data_início | modalidades_bolsa | sigla_nova | nome_novo | descrição_nova | percentual_novo | data_início_nova | modalidades_bolsa_nova |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               |

  Scenario Outline: Alterar resolução com erro - campo obrigatório
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor visualiza os dados da resolução:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla>            |
      | Nome          | <nome>             |
      | Descrição     | <descrição>        |
      | Percentual    | <percentual>       |
      | Data de Início | <data_início>       |
      | Modalidades de Bolsa | <modalidades_bolsa> |
    When o servidor altera a resolução com os dados:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla_nova>       |
      | Nome          | <nome_novo>        |
      | Descrição     | <descrição_nova>    |
      | Percentual    | <percentual_novo>  |
      | Data de Início | <data_início_nova> |
      | Modalidades de Bolsa | <modalidades_bolsa_nova> |
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | sigla     | nome     | descrição     | percentual | data_início | modalidades_bolsa | sigla_nova | nome_novo | descrição_nova | percentual_novo | data_início_nova | modalidades_bolsa_nova | mensagem_erro                                    |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             |            | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O campo Sigla é obrigatório                       |
      | Res1      | ABC       |          | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       |            | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O campo Nome é obrigatório                         |
      | Res1      | ABC       | Nome     |               | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome |                  | 20          | 2025-01-01     | Bolsa2               | O campo Descrição é obrigatório                    |
      | Res1      | ABC       | Nome     | Descrição     |            | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O campo Percentual é obrigatório                   |
      | Res1      | ABC       | Nome     | Descrição     | 10         |             | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O campo Data de Início é obrigatório              |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  |                 | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O campo Modalidades de Bolsa é obrigatório         |

  Scenario Outline: Alterar resolução com erro - formato inválido
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor visualiza os dados da resolução:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla>            |
      | Nome          | <nome>             |
      | Descrição     | <descrição>        |
      | Percentual    | <percentual>       |
      | Data de Início | <data_início>       |
      | Modalidades de Bolsa | <modalidades_bolsa> |
    When o servidor altera a resolução com os dados:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla_nova>       |
      | Nome          | <nome_novo>        |
      | Descrição     | <descrição_nova>    |
      | Percentual    | <percentual_novo>  |
      | Data de Início | <data_início_nova> |
      | Modalidades de Bolsa | <modalidades_bolsa_nova> |
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | sigla     | nome     | descrição     | percentual | data_início | modalidades_bolsa | sigla_nova | nome_novo | descrição_nova | percentual_novo | data_início_nova | modalidades_bolsa_nova | mensagem_erro                                                                                 |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | 123       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | A Sigla deve ter no máximo 3 caracteres                                                         |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | 1234567890 | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O Nome deve ter no máximo 10 caracteres                                                        |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | abc        | 2025-01-01     | Bolsa2               | O Percentual deve ser um número                                                                |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | A Data de Início deve estar no formato AAAA-MM-DD                                              |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | abcde              | A Modalidade de Bolsa deve ser um valor válido                                                  |

  Scenario Outline: Alterar resolução com erro - regra de negócio
    Given o servidor seleciona a resolução "<resolução>"
    And o servidor visualiza os dados da resolução:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla>            |
      | Nome          | <nome>             |
      | Descrição     | <descrição>        |
      | Percentual    | <percentual>       |
      | Data de Início | <data_início>       |
      | Modalidades de Bolsa | <modalidades_bolsa> |
    When o servidor altera a resolução com os dados:
      | Campo         | Valor             |
      |---------------|--------------------|
      | Sigla         | <sigla_nova>       |
      | Nome          | <nome_novo>        |
      | Descrição     | <descrição_nova>    |
      | Percentual    | <percentual_novo>  |
      | Data de Início | <data_início_nova> |
      | Modalidades de Bolsa | <modalidades_bolsa_nova> |
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | sigla     | nome     | descrição     | percentual | data_início | modalidades_bolsa | sigla_nova | nome_novo | descrição_nova | percentual_novo | data_início_nova | modalidades_bolsa_nova | mensagem_erro                                                                                                                  |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | -10         | 2025-01-01     | Bolsa2               | O Percentual deve ser um valor positivo                                                                                           |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2024-12-31     | Bolsa2               | A Data de Início deve ser maior que a data de início da resolução anterior                                                      |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa3               | A Modalidade de Bolsa deve ser uma modalidade de bolsa válida e existente no sistema                                            |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa1               | A Modalidade de Bolsa deve ser diferente da modalidade de bolsa da resolução anterior caso a data de início seja a mesma             |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa1               | A Sigla já está em uso. Utilize outra sigla                                                                                      |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | O Nome já está em uso. Utilize outro nome                                                                                     |
      | Res1      | ABC       | Nome     | Descrição     | 10         | 2024-01-01  | Bolsa1             | DEF       | Novo Nome | Nova Descrição    | 20          | 2025-01-01     | Bolsa2               | A Descrição já está em uso. Utilize outra descrição                                                                           |