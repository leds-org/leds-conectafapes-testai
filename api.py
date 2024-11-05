from fastapi import FastAPI, HTTPException
# from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from revisao_agentes import generate_gherkin_feature
from fastapi.responses import JSONResponse


app = FastAPI()

# options = [
#     'http:localhost',
#     "http://127.0.0.1:5500"
# ]

# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=options,
#     allow_credentials=True,
#     allow_methods=["*"],
#     allow_headers=["*"]
# )

# Pydantic model to receive the payload
class Evento(BaseModel):
    evento: str


class Feature(BaseModel):
    feature: str
    codigo: str


@app.get("/")
async def home():
    return "Rodando"

@app.post("/gherkin", responses={
    200: {
        "content": {
            "application/json": {
                "example": {"feature": "Feature: Excluir Resolução\n\n Scenario Outline: Nome Scenario\n    Given exemplo de given\n    When exemplo de when\n    Then exemplo de then\n\n    Examples:\n      | exemplos |\n    | exemplo1 |"}
            }
            }
        }
    })
async def generate_gherkin_file(evento: Evento):
    try:
        # Call the function to process the evento and generate the feature file
        feature_file = generate_gherkin_feature(evento.evento)
        # return FileResponse(path=feature_file, media_type='text/plain', filename=feature_file.split('/')[-1])
        return JSONResponse({"feature": feature_file})
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))


@app.post("/code-generator")
async def generate_code_tests(feature: Feature):
    try:
        code_file = ...
        return JSONResponse({"code": code_file})
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))