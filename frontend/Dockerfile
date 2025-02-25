# Etapa de build
FROM node:16.15.0 AS build

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia o arquivo package.json e package-lock.json para o contêiner
COPY package*.json ./

# Limpa o cache do npm para reduzir o tamanho da imagem
RUN npm cache clean --force

# Configura o npm para ignorar SSL e instala as dependências forçando a atualização
RUN npm config set strict-ssl false
RUN npm install --force && npm cache clean --force

# Instala a biblioteca SignalR
RUN npm install @microsoft/signalr --force

# Copia todos os arquivos do projeto para o contêiner
COPY . .

# Executa o build do aplicativo React
RUN npm run build

# Etapa de produção
FROM nginx:alpine

# Remove o arquivo de configuração padrão do Nginx para evitar conflitos
RUN rm /etc/nginx/conf.d/default.conf

# Copia o build do React para o diretório padrão do Nginx
COPY --from=build /app/build /usr/share/nginx/html

# Copia o arquivo de configuração personalizado do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exposição da porta padrão do Nginx
EXPOSE 80

# Inicia o servidor Nginx
CMD ["nginx", "-g", "daemon off;"]
