Feature: Incluir Resolução

  Scenario Outline: Incluir resolução com sucesso
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e salva a resolução
    Then o sistema deve salvar a resolução com status "Em edição"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |
      | 67890  | 2024-02-15 | Resolução sobre auxílio financeiro para estudantes de graduação e pós-graduação | https://www.example.com/resolucoes/67890                                                |

  Scenario Outline: Incluir resolução com erro - Número da resolução não informado
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O número da resolução é obrigatório"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      |        | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |

  Scenario Outline: Incluir resolução com erro - Data da resolução não informada
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "A data da resolução é obrigatória"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  |            | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |

  Scenario Outline: Incluir resolução com erro - Ementa da resolução não informada
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "A ementa da resolução é obrigatória"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  | 2024-01-01 |                                                                          | https://www.example.com/resolucoes/12345                                                |

  Scenario Outline: Incluir resolução com erro - Link da resolução não informado
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O link da resolução é obrigatório"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         |                                                                                        |

  Scenario Outline: Incluir resolução com erro - Data da resolução inválida
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "A data da resolução é inválida"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  | 2024-01-32 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |

  Scenario Outline: Incluir resolução com erro - Link da resolução inválido
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O link da resolução é inválido"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | www.example.com                                                                        |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com                                                               |