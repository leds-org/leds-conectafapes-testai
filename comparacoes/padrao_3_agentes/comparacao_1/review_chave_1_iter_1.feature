Feature: Incluir Resolução

  Scenario Outline: Incluir resolução com sucesso
    Given o servidor informa os dados da resolução <número>, <data>, <ementa>, <link>
    When o sistema valida e salva a resolução
    Then o sistema deve salvar a resolução com status "Em edição"

    Examples:
      | número | data       | ementa                                       | link                                                                   |
      | 12345  | 2024-01-01 | Esta é a ementa da resolução 12345             | https://www.example.com/resolucoes/12345                               |
      | 67890  | 2024-02-15 | Outra ementa de resolução, agora com mais texto | https://www.example.com/resolucoes/67890?utm_source=sistema_fapes&utm_medium=link |

  Scenario Outline: Incluir resolução com erro
    Given o servidor informa os dados da resolução <número>, <data>, <ementa>, <link>
    When o sistema valida e não pode salvar a resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_de_erro>"

    Examples:
      | número | data       | ementa                                       | link                                                                   | mensagem_de_erro                                    |
      | abcde  | 2024-01-01 | Esta é a ementa da resolução abcde             | https://www.example.com/resolucoes/abcde                               | Número da resolução inválido                            |
      | 12345  | 2024-01-32 | Esta é a ementa da resolução 12345             | https://www.example.com/resolucoes/12345                               | Data inválida                                         |
      | 67890  | 2024-02-15 | Ementa muito curta                              | https://www.example.com/resolucoes/67890?utm_source=sistema_fapes&utm_medium=link | Ementa inválida                                       |
      | 12345  | 2024-01-01 | Esta é a ementa da resolução 12345             | http://www.example.com                                                  | Link inválido                                          |
      | 67890  | 2024-02-15 | Outra ementa de resolução, agora com mais texto | www.example.com                                                         | Link inválido                                          |