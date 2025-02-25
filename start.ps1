# # Muda o diretório atual para a pasta "backend"
# Set-Location -Path ".\backend"

# # Atualiza o pip para a versão mais recente
# python.exe -m pip install --upgrade pip

# # Instala as dependências necessárias
# pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org psutil

# # Executa o script Python para gerar o arquivo .env
# python generate_env.py

# Set-Location -Path ".\frontend"
# python generate_env_frontend.py
# # Inicia o Docker Compose com a variável de ambiente configurada
# docker-compose up -d --build
# Muda o diretório atual para a pasta "backend"
Set-Location -Path "./backend"

# Atualiza o pip para a versão mais recente
python.exe -m pip install --upgrade pip

# Instala as dependências necessárias
pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org psutil

# Executa o script Python para gerar o arquivo .env
python generate_env.py

# Volta para o diretório raiz antes de mudar para "frontend"
Set-Location -Path ".."

# Muda o diretório atual para a pasta "frontend"
Set-Location -Path "./frontend"

# Executa o script Python para gerar o arquivo .env para o frontend
python generate_env_frontend.py

# Volta para o diretório raiz antes de iniciar o Docker Compose
Set-Location -Path ".."

# Inicia o Docker Compose com a variável de ambiente configurada
docker-compose up -d --build
