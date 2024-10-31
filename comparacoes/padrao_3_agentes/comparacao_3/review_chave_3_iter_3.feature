Feature: Consultar Resolução

  Scenario Outline: Consultar resolução com sucesso
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve exibir os dados da resolução:
      | Campo           | Valor        |
      |-----------------|--------------|
      | Número          | <número>     |
      | Data de Publicação | <data_publicacao> |
      | Assunto         | <assunto>    |
      | Conteúdo        | <conteudo>   |

    Examples:
      | resolução | número | data_publicacao | assunto        | conteudo   |
      | <resolução> | <número> | <data_publicacao> | <assunto>    | <conteudo>   |

  Scenario Outline: Consultar resolução com erro
    Given o servidor seleciona a resolução "<resolução>"
    When o sistema consulta a resolução
    Then o sistema deve retornar uma mensagem de erro "<mensagem_erro>"

    Examples:
      | resolução | mensagem_erro                               |
      | <resolução> | Resolução não encontrada                     |
      | <resolução> | O servidor não tem permissão para acessar esta resolução |
      | <resolução> | Resolução inválida                           |