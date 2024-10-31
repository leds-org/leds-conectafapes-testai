Feature: Incluir Modalidade

  Scenario Outline: Incluir modalidade com sucesso
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve salvar a modalidade com status "Em edição"
    And o sistema deve incluir os níveis da modalidade

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   | Nome da Modalidade | Descrição da Modalidade | 10 | 2024-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Sigla inválida
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Sigla inválida"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    |       | Nome da Modalidade | Descrição da Modalidade | 10 | 2024-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Nome inválido
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Nome inválido"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   |  | Descrição da Modalidade | 10 | 2024-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Descrição inválida
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Descrição inválida"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   | Nome da Modalidade |  | 10 | 2024-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Redução por vínculo inválida
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Redução por vínculo inválida"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   | Nome da Modalidade | Descrição da Modalidade | -10 | 2024-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |
    | ABC   | Nome da Modalidade | Descrição da Modalidade | 100 | 2024-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Data de início da vigência inválida
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Data de início da vigência inválida"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   | Nome da Modalidade | Descrição da Modalidade | 10 | 2023-01-01 | Bolsa 1, Bolsa 2 | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Modalidades de bolsa inválidas
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Modalidades de bolsa inválidas"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   | Nome da Modalidade | Descrição da Modalidade | 10 | 2024-01-01 |  | Resolução 1 |

  Scenario Outline: Incluir modalidade com erro - Resolução inválida
    Given o servidor informa os dados da modalidade com sigla "<sigla>", nome "<nome>", descrição "<descrição>", redução por vínculo "<redução_por_vinculo>", data de início da vigência "<data_início_vigência>" e modalidades de bolsa "<modalidades_bolsa>"
    And o servidor seleciona a resolução "<resolução>"
    When o servidor inclui os requisitos da modalidade
    Then o sistema deve retornar uma mensagem de erro "Resolução inválida"

  Examples:
    | sigla | nome | descrição | redução_por_vinculo | data_início_vigência | modalidades_bolsa | resolução |
    | ABC   | Nome da Modalidade | Descrição da Modalidade | 10 | 2024-01-01 | Bolsa 1, Bolsa 2 |  |