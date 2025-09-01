# Dockerfile para a API Moto
# Multi-stage build para otimizar o tamanho da imagem

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["Moto.Api/Moto.Api.csproj", "Moto.Api/"]
COPY ["Moto.Application/Moto.Application.csproj", "Moto.Application/"]
COPY ["Moto.Domain/Moto.Domain.csproj", "Moto.Domain/"]
COPY ["Moto.Infrastructure/Moto.Infrastructure.csproj", "Moto.Infrastructure/"]

RUN dotnet restore "Moto.Api/Moto.Api.csproj"

# Copiar código fonte
COPY . .
WORKDIR "/src/Moto.Api"

# Build da aplicação
RUN dotnet build "Moto.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Moto.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Instalar dependências do sistema se necessário
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar aplicação publicada
COPY --from=publish /app/publish .

# Configurar variáveis de ambiente
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080;https://+:443

# Expor portas (correspondem ao docker-compose)
EXPOSE 8080
EXPOSE 443

# Health check (usando endpoint que existe)
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/swagger || exit 1

# Comando de inicialização
ENTRYPOINT ["dotnet", "Moto.Api.dll"]
