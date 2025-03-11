# Imagem oficial do Cypress
FROM cypress/included:12.17.1

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos necessários
COPY package.json package-lock.json ./
RUN npm install

# Copia os arquivos do projeto
COPY . .

# Comando para rodar os testes
CMD ["npx", "cypress", "run"]
