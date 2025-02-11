from pathlib import Path
import difflib
import os

file_path = Path("leds-conectafapes-backend-admin-main\src\ConectaFapes\ConectaFapes.Application\Services\CadastroModalidadesBolsas\VersaoModalidadeService.cs")
base_directory = "leds-conectafapes-backend-admin-main/src/ConectaFapes"

def find_file(base_directory: str, file_name: str) -> str:
    
    """
    Recursivamente procura por um arquivo no diretório fornecido e seus subdiretórios.
    Se não for encontrado um correspondência exata, retorna a correspondência mais próxima usando correspondência difusa.
    """
      
    found_files = []
    for root, _, files in os.walk(base_directory):
        if file_name in files:
            return os.path.join(root, file_name)
        found_files.extend([os.path.join(root, file) for file in files])

    file_names = [os.path.basename(path) for path in found_files]
    closest_match = difflib.get_close_matches(file_name, file_names, n=1)
    if closest_match:
        for path in found_files:
            if os.path.basename(path) == closest_match[0]:
                return path
    return None


with open(file_path, 'r', encoding='utf-8') as file:
    cs_file_content = file.read()

def process_test_and_related_files(found_paths: list):
    existing_test_content = ""
    if found_paths and "Test" in found_paths[-1]:
        test_file_path = found_paths.pop()
        with open(test_file_path, "r", encoding="utf-8") as f:
            existing_test_content = f.read()

    related_files_content = []
    for related_file in found_paths:
        with open(related_file, "r", encoding="utf-8") as f:
            related_files_content.append(f.read())
    related_files_content = "\n\n".join(related_files_content)

    return existing_test_content, related_files_content
