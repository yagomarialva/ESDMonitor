# Use uma imagem base oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 5051
EXPOSE 7080
# Use outra imagem para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
 
# Copia o arquivo de projeto e restaura dependências
COPY ["BiometricFaceApi.csproj", "./"]
RUN dotnet restore
 
# Copia todo o código fonte e compila o projeto
COPY . .
WORKDIR "/src/."
RUN dotnet build -c Release -o /app/build
 
# Publica a aplicação
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish
 
# Configura o ambiente de execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BiometricFaceApi.dll"]