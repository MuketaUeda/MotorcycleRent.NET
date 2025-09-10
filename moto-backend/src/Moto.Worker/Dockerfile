# Dockerfile for the Moto Worker
# Multi-stage build for message processing

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Moto.Worker/Moto.Worker.csproj", "Moto.Worker/"]
COPY ["Moto.Application/Moto.Application.csproj", "Moto.Application/"]
COPY ["Moto.Domain/Moto.Domain.csproj", "Moto.Domain/"]
COPY ["Moto.Infrastructure/Moto.Infrastructure.csproj", "Moto.Infrastructure/"]

RUN dotnet restore "Moto.Worker/Moto.Worker.csproj"

# Copy source code
COPY . .
WORKDIR "/src/Moto.Worker"

# Build application
RUN dotnet build "Moto.Worker.csproj" -c Release -o /app/build

# Publish stage --> Intermediate stage
FROM build AS publish
RUN dotnet publish "Moto.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage --> Final stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

# Configure environment variables
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published application
COPY --from=publish /app/publish .

# Initialization command
ENTRYPOINT ["dotnet", "Moto.Worker.dll"]
