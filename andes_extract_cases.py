import json
import re
with open("message.andes", encoding="utf-8") as file:
    input_text = file.read()
# input_text = """
#    usecase UC05 {
#     name: "Definir Calendário das Folhas"
#     description: "Definir Calendário das Folhas"
#     performer: Roberto

#     event E021 {
#         name: "Visualizar Calendário Anual"
#         description: "O objetivo deste evento é visualizar os calendários definidos em cada mês, em um dado ano."
#         depend: UC05.E022
#         action: "
#             O sistema exibe um conjunto de opções de ano do calendário, com o ano atual selecionado por padrão.
#             O Gerente GEPOF seleciona o ano em que deseja visualizar o calendário. 
#             O sistema exibe os marcos definidos para cada mês do ano selecionado. Os marcos são: (M1) Data Limite de Solicitação de Bolsas, (M2) Data Prevista de Geração da Folha Normal, e (M3) Data de Pagamento da Folha Normal.
#         "
#     }

#     event E022 {
#         name: "Definir Marcos da Folha"
#         description: "O objetivo deste evento é definir marcos a serem praticados em cada mês, em um ano específico."
#         depend: UC06.E023
#         action: "
#         O Gerente GEPOF visualiza o calendário usando o evento Visualizar Calendário Anual ou cria um novo calendário selecionando o ano subsequente ao atual.
#         O Gerente GEPOF define (ou altera), para cada mês, exatamente três marcos: (M1) Data Limite de Solicitação de Bolsas, (M2) Data Prevista de Geração da Folha Normal, e (M3) Data de Pagamento da Folha Normal.
#         Os marcos somente podem ser alterados quando:
#             M1: é menor ou igual à data atual (RN04),
#             M2: é menor que a data atual (RN05),
#             M3: não foi gerada a folha normal do mês (RN06).
#         Para cada mês, tanto na definição como na alteração, o sistema verifica se:
#             M1 está dentro do mês em questão ou do mês anterior (RN01),
#             M2 está dentro do mês em questão (RN02),
#             M3 está dentro do mês em questão ou do subsequente (RN03),
#             M1 ocorre antes de M2 e M2 ocorre antes de M3 (RN07),
#             M2 de um dado mês ocorre antes do M1 do mês seguinte (RN08).
#         Caso alguma dessas condições não seja atendida, uma mensagem de erro é exibida, para que o usuário corrija as definições feitas.
#         Para cada mês, tanto na definição como na alteração, o sistema verifica se:
#             Há uma distância maior que 5 dias entre M1 e M2 e entre M2 e M3 (RN09).
#         Caso alguma dessas condições não seja atendida, um alerta é emitido e o usuário tem a opção de rever ou manter as definições feitas.
#         O sistema registra os marcos informados.

#         "
#     }
# }
# """

# Regex patterns to extract use cases and events
usecase_pattern = r"usecase\s+(\w+)\s*{\s*name:\s*\"([^\"]+)\".*?description:\s*\"([^\"]+)\".*?performer:\s*(\w+).*?(event.*?})\s*}"
event_pattern = r"event\s+(\w+)\s*{\s*name:\s*\"([^\"]+)\".*?description:\s*\"([^\"]+)\".*?action:\s*\"([^\"]+)\"\s*}"

# Extract all use cases and their events
usecases = []
for usecase_match in re.finditer(usecase_pattern, input_text, re.DOTALL):
    usecase_id = usecase_match.group(1)
    usecase_name = usecase_match.group(2)
    usecase_description = usecase_match.group(3)
    usecase_performer = usecase_match.group(4)
    events_block = usecase_match.group(5)
    
    # Extract events within the use case
    events = []
    for event_match in re.finditer(event_pattern, events_block, re.DOTALL):
        event_id = event_match.group(1)
        event_name = event_match.group(2)
        event_description = event_match.group(3)
        event_action = event_match.group(4)
        events.append({
            "event_id": event_id,
            "name": event_name,
            "description": event_description,
            "action": event_action.strip()
        })
    
    usecases.append({
        "usecase_id": usecase_id,
        "name": usecase_name,
        "description": usecase_description,
        "performer": usecase_performer,
        "events": events
    })

# Convert the extracted data to JSON
usecases_json = json.dumps(usecases, indent=4, ensure_ascii=False)

# Display JSON
print(usecases_json)
