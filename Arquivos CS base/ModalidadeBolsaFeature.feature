Feature: ModalidadeBolsa CRUD

Background: 
    Given I have access to the ModalidadeBolsa API

Scenario Outline: Create a new ModalidadeBolsa
    When I send a POST request to /modalidadebolsa with the following ModalidadeBolsa details: "<Sigla>", "<Nome>"
    Then the API response should be: "<StatusCode>"
Examples: 
    | Sigla   | Nome              | Descricao | StatusCode |
    | <empty> | Modalidade Criada | Descrição | 400        |
    | MB2     | Modalidade Criada | Descrição | 400        |
    | MB1     | <empty>           | Descrição | 400        |
    | MB1     | Modalidade Criada | Descrição | 201        |

Scenario Outline: Update an existing ModalidadeBolsa
    When I send a PUT request to /modalidadebolsa/"<ModalidadeBolsaId>" with the following ModalidadeBolsa details: "<Sigla>", "<Nome>"
    Then the API response should be: "<StatusCode>"
Examples: 
    | ModalidadeBolsaId                    | Sigla   | Nome                  | StatusCode |
    | 3af1227c-3688-4a39-9fac-93dfa19b09c4 | <empty> | Modalidade Atualizada | 400        |
    | 3af1227c-3688-4a39-9fac-93dfa19b09c4 | MBX     | <empty>               | 400        |
    | 3af1227c-3688-4a39-9fac-93dfa19b09c4 | MB2     | Modalidade Atualizada | 400        |
    | 968b0b91-fb06-445f-8e3e-dfef86c94d95 | MBX     | Modalidade Atualizada | 200        |

Scenario Outline: Retrieve an existing ModalidadeBolsa
    When I send a GET request to /modalidadebolsa/"<ModalidadeBolsaId>"
    Then the API response should be: "<StatusCode>"
Examples: 
    | ModalidadeBolsaId                     | StatusCode |
    | 4008dc39-2512-46fb-8e49-41ad64e9014a  | 200        |

Scenario Outline: Delete an existing ModalidadeBolsa
    When I send a DELETE request to /modalidadebolsa/"<ModalidadeBolsaId>"
    Then the API response should be: "<StatusCode>"
Examples: 
    | ModalidadeBolsaId                    | StatusCode |
    | 49ac8058-4dc3-44ae-a369-f018360b6ad8 | 400        |
    | 9a59a0ab-0b8f-4593-92f0-ba1239e09155 | 404        |
    | e24de187-bebc-4964-974b-2c83f80eae9e | 200        |

Scenario Outline: Active an existing ModalidadeBolsa
    When I send a PUT request to /modalidadebolsa/"<ModalidadeBolsaId>"/ativar
    Then the API response should be: "<StatusCode>"
Examples: 
    | ModalidadeBolsaId                     | StatusCode |
    | 95ccfdc8-f1f4-4372-b510-0bcc35061d16  | 400        |
    | 3af1227c-3688-4a39-9fac-93dfa19b09c4  | 400        |
    | 74068c31-38da-4067-a841-7e1da5006a79  | 404        |
    | ea9c900d-7b6b-4d62-8bd9-4ecb505f0852  | 200        |
    | 651c6d04-14c9-41ca-817a-e37e65d7be1d  | 200        |

Scenario Outline: Disable an existing ModalidadeBolsa
    When I send a PUT request to /modalidadebolsa/"<ModalidadeBolsaId>"/desativar
    Then the API response should be: "<StatusCode>"
Examples: 
    | ModalidadeBolsaId                     | StatusCode |
    | 456510ef-e5fb-4305-bff8-92e8faf8d871  | 400        |
    | 4fe0946c-1123-4891-8f2c-6ad615be4f67  | 400        |
    | 74068c31-38da-4067-a841-7e1da5006a79  | 404        |
    | 3af1227c-3688-4a39-9fac-93dfa19b09c4  | 200        |
    | 49ac8058-4dc3-44ae-a369-f018360b6ad8  | 200        |