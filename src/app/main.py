from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from crewai import Agent, Task, Crew, Process, LLM
import os
from dotenv import load_dotenv
from fastapi.responses import JSONResponse
from src.crews.crew_xUnit import xunit_generation
from src.crews.crew_gherkin import generate_gherkin

import uuid

app = FastAPI()

origins = [
    "http://localhost",
    "http://127.0.0.1:5500",
]

# Adicione o middleware de CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,  # Permitir as origens definidas acima
    allow_credentials=True,  # Permitir envio de cookies
    allow_methods=["*"],  # Permitir todos os métodos (GET, POST, etc)
    allow_headers=["*"],  # Permitir todos os cabeçalhos
)

load_dotenv()

# Pydantic model to receive the payload
class Evento(BaseModel):
    evento: str

class XunitPayload(BaseModel):
    feature: str
    api_url: str
    dto_code: str

@app.get("/")
async def home():
    return "Rodando"

#@app.get("/test")
#async def teste():
#    return FileResponse(path="C:/Users/gabri/test_generation_ai/features/analise_evento_3b552e54-4343-4952-9db1-fd412d869e9e.feature", media_type='text/plain', filename="analiase.feature")

@app.post("/gherkin",
    responses={
        200: {
            "content": {
                "text/plain": {
                    "example": "Teste"
                }
            }
        }
    }
)
async def generate_gherkin_file(evento: Evento):
    feature = generate_gherkin(evento.evento)
    body = {
        "feature": feature
    }
    return JSONResponse(body)

@app.post('/xunit')
async def generate_xunit(payload: XunitPayload):
    try:
        xunit = xunit_generation(payload.feature)
        body = {
            "xunit": xunit
        }
        return JSONResponse(body)
    except Exception as e:
        return JSONResponse({
            "erro": e
        })

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", reload=True)