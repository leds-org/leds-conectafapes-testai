import streamlit as st
import requests
from dotenv import load_dotenv
import base64
load_dotenv()

def download_file2(file_content, filename):
    b64 = base64.b64encode(file_content).decode()  # Codifica o conteúdo em base64
    href = f'<a href="data:file/txt;base64,{b64}" download="{filename}">Baixar {filename}</a>'
    st.markdown(href, unsafe_allow_html=True)


def download_file(file_content, filename):
    st.download_button(
        label="Baixar Arquivo",
        data=file_content,
        file_name=filename,
        mime="text/plain"
    )

# Inicializando o histórico de mensagens na sessão do Streamlit
st.set_page_config(layout='wide')

#st.title("ConnectAI War V0: Llama 3.1 (localhost) Vs Gemini (web)")

st.title("Utilizando a API")

# st.write("Converse com as IAs LLaMA e Gemini ao mesmo tempo.")

st.markdown("""
    <style>
    .main {
        max-width: 100%;  /* Aumenta a largura da página para 100% */
        padding-left: 0rem;
        padding-right: 0rem;
    }
    </style>
    """, unsafe_allow_html=True)

# Caixa de entrada para o usuário
user_input = st.text_input("Digite sua mensagem:", "")

if st.button("Gerar e Baixar via POST"):
    if user_input:
        try :
            # Requisição POST para a API FastAPI
            response = requests.post(f"http://localhost:8000/gherkin", json={"evento": user_input})

            if response.status_code == 200:
                # Supomos que a API retorna o conteúdo do arquivo como texto
                file_content = response.content  # Conteúdo do arquivo
                filename = "output.feature"  # Nome do arquivo
                download_file(file_content, filename)  # Exibe o link de download
            else:
                st.error(f"Erro na requisição: {response.status_code}")
        except Exception as e:
            st.error(f"Ocorreu um erro: {str(e)}")
    else:
        st.warning("Por favor, insira uma mensagem.")