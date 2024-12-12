import requests

url = "http://localhost:8000/xunit"

request_dto_file = open(r"C:\Users\gabri\OneDrive\Documentos\leds-conectafapes-backend-admin-test-bdd-ia\src\ConectaFapes\ConectaFapes.Application\DTOs\CadastroModalidadesBolsas\Request\ModalidadeBolsaRequestDTO.cs")
response_dto_file = open(r"C:\Users\gabri\OneDrive\Documentos\leds-conectafapes-backend-admin-test-bdd-ia\src\ConectaFapes\ConectaFapes.Application\DTOs\CadastroModalidadesBolsas\Response\ModalidadeBolsaResponseDTO.cs")
feature_file = open(r"C:\Users\gabri\leds-conectafapes-testai-1\features\ListarBolsaFeature.feature")

request_dto = request_dto_file.read()
response_dto = response_dto_file.read()
feature = feature_file.read()

dto = request_dto + response_dto
api_path = """
PUT /api/modalidadebolsa/modalidadebolsa/{id}/ativar
PUT /api/modalidadebolsa/modalidadebolsa/{id}/desativar
GET /api/modalidadebolsa/modalidadebolsa
POST /api/modalidadebolsa/modalidadebolsa
GET /api/modalidadebolsa/modalidadebolsa/{id}
DELETE /api/modalidadebolsa/modalidadebolsa/{id}
PUT /api/modalidadebolsa/modalidadebolsa/{id}
"""

data = {
    "feature": feature,
    "api_url": api_path,
    "dto_code": dto
}

headers = {
    "Content-Type": "application/json",  # Tipo de conteúdo
}

response = requests.post(url, json=data, headers=headers)

if response.status_code == 200:
    print("requisição bem sucedida!")
    print(response.json())
else:
    print('Erro na requisição:', response.status_code)
    print('Detalhes:', response.text)

with open("result.cs", "w") as f:
    f.write(response.json()["xunit"])