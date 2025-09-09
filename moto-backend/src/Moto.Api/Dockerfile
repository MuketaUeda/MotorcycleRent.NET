# Dockerfile for the Moto API
# Multi-stage build to optimize image size

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Moto.Api/Moto.Api.csproj", "Moto.Api/"]
COPY ["Moto.Application/Moto.Application.csproj", "Moto.Application/"]
COPY ["Moto.Domain/Moto.Domain.csproj", "Moto.Domain/"]
COPY ["Moto.Infrastructure/Moto.Infrastructure.csproj", "Moto.Infrastructure/"]

RUN dotnet restore "Moto.Api/Moto.Api.csproj"

# Copy source code
COPY . .
WORKDIR "/src/Moto.Api"

# Build the application
RUN dotnet build "Moto.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Moto.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install system dependencies if necessary
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Configure environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080

# Expose ports (correspond to docker-compose)
EXPOSE 8080
EXPOSE 443

# Health check (using existing endpoint)
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/swagger || exit 1

# Initialization command
ENTRYPOINT ["dotnet", "Moto.Api.dll"]
