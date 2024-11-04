Feature: Incluir Resolução

  Scenario Outline: Incluir resolução com sucesso
    Given o servidor informa os dados da resolução com número "<número>", data "<data>", ementa "<ementa>" e link "<link>"
    When o sistema valida e salva a resolução
    Then o sistema deve salvar a resolução com status "Em edição"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de estudo         | https://www.example.com/resolucoes/12345                 |
      | 67890  | 2024-02-15 | Normas para o programa de pesquisa       | https://www.example.com/resolucoes/67890                 |
      | 11111  | 2024-03-01 | Diretrizes para o edital de projetos     | https://www.example.com/resolucoes/11111                 |

  Scenario Outline: Incluir resolução com erro - Número da resolução ausente
    Given o servidor informa os dados da resolução com número "", data "<data>", ementa "<ementa>" e link "<link>"
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O número da resolução é obrigatório"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      |        | 2024-01-01 | Resolução sobre bolsas de estudo         | https://www.example.com/resolucoes/12345                 |
      |        | 2024-02-15 | Normas para o programa de pesquisa       | https://www.example.com/resolucoes/67890                 |
      |        | 2024-03-01 | Diretrizes para o edital de projetos     | https://www.example.com/resolucoes/11111                 |

  Scenario Outline: Incluir resolução com erro - Data da resolução ausente
    Given o servidor informa os dados da resolução com número "<número>", data "", ementa "<ementa>" e link "<link>"
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "A data da resolução é obrigatória"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      | 12345  |             | Resolução sobre bolsas de estudo         | https://www.example.com/resolucoes/12345                 |
      | 67890  |             | Normas para o programa de pesquisa       | https://www.example.com/resolucoes/67890                 |
      | 11111  |             | Diretrizes para o edital de projetos     | https://www.example.com/resolucoes/11111                 |

  Scenario Outline: Incluir resolução com erro - Ementa da resolução ausente
    Given o servidor informa os dados da resolução com número "<número>", data "<data>", ementa "" e link "<link>"
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "A ementa da resolução é obrigatória"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      | 12345  | 2024-01-01 |                                       | https://www.example.com/resolucoes/12345                 |
      | 67890  | 2024-02-15 |                                       | https://www.example.com/resolucoes/67890                 |
      | 11111  | 2024-03-01 |                                       | https://www.example.com/resolucoes/11111                 |

  Scenario Outline: Incluir resolução com erro - Link da resolução ausente
    Given o servidor informa os dados da resolução com número "<número>", data "<data>", ementa "<ementa>" e link ""
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O link da resolução é obrigatório"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de estudo         |                                                        |
      | 67890  | 2024-02-15 | Normas para o programa de pesquisa       |                                                        |
      | 11111  | 2024-03-01 | Diretrizes para o edital de projetos     |                                                        |

  Scenario Outline: Incluir resolução com erro - Link da resolução inválido
    Given o servidor informa os dados da resolução com número "<número>", data "<data>", ementa "<ementa>" e link "<link>"
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O link da resolução é inválido"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de estudo         | https://www.example.com/resolucoes/12345/invalido      |
      | 67890  | 2024-02-15 | Normas para o programa de pesquisa       | https://www.example.com/resolucoes/67890/invalido      |
      | 11111  | 2024-03-01 | Diretrizes para o edital de projetos     | https://www.example.com/resolucoes/11111/invalido      |

  Scenario Outline: Incluir resolução com erro - Data da resolução inválida
    Given o servidor informa os dados da resolução com número "<número>", data "<data>", ementa "<ementa>" e link "<link>"
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "A data da resolução é inválida"

    Examples:
      | número | data        | ementa                                  | link                                                    |
      | 12345  | 2024-01-32 | Resolução sobre bolsas de estudo         | https://www.example.com/resolucoes/12345                 |
      | 67890  | 2024-02-30 | Normas para o programa de pesquisa       | https://www.example.com/resolucoes/67890                 |
      | 11111  | 2024-03-40 | Diretrizes para o edital de projetos     | https://www.example.com/resolucoes/11111                 |