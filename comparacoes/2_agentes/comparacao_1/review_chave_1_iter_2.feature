Feature: Incluir Resolução

  Scenario Outline: Incluir resolução com sucesso
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e salva a resolução
    Then o sistema deve salvar a resolução com status "Em edição"

    Examples:
      | numero | data       | ementa                                                                     | link                                                                                   |
      | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |
      | 67890  | 2024-02-15 | Resolução sobre auxílio financeiro para estudantes de graduação e pós-graduação | https://www.example.com/resolucoes/67890                                                |

  Scenario Outline: Incluir resolução com erro - Campo obrigatório não informado
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O campo '<campo>' é obrigatório"

    Examples:
      | campo             | numero | data       | ementa                                                                     | link                                                                                   |
      | Número da resolução |        | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |
      | Data da resolução  | 12345  |            | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |
      | Ementa da resolução | 12345  | 2024-01-01 |                                                                          | https://www.example.com/resolucoes/12345                                                |
      | Link da resolução   | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         |                                                                                        |

  Scenario Outline: Incluir resolução com erro - Campo inválido
    Given o servidor informa os dados da resolução <numero>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "O campo '<campo>' é inválido"

    Examples:
      | campo             | numero | data       | ementa                                                                     | link                                                                                   |
      | Data da resolução  | 12345  | 2024-01-32 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |
      | Link da resolução   | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | www.example.com                                                                        |
      | Link da resolução   | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com                                                               |
      | Número da resolução | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |
      | Ementa da resolução | 12345  | 2024-01-01 | Resolução sobre bolsas de pesquisa                                         | https://www.example.com/resolucoes/12345                                                |