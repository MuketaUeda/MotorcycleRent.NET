# Dockerfile para o Worker Moto
# Multi-stage build para processamento de mensagens

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["Moto.Worker/Moto.Worker.csproj", "Moto.Worker/"]
COPY ["Moto.Application/Moto.Application.csproj", "Moto.Application/"]
COPY ["Moto.Domain/Moto.Domain.csproj", "Moto.Domain/"]
COPY ["Moto.Infrastructure/Moto.Infrastructure.csproj", "Moto.Infrastructure/"]

RUN dotnet restore "Moto.Worker/Moto.Worker.csproj"

# Copiar código fonte
COPY . .
WORKDIR "/src/Moto.Worker"

# Build da aplicação
RUN dotnet build "Moto.Worker.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Moto.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

# Copiar aplicação publicada
COPY --from=publish /app/publish .

# Comando de inicialização
ENTRYPOINT ["dotnet", "Moto.Worker.dll"]
