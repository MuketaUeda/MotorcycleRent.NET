# 🏍️ MotorcycleRent.NET - Sistema de Aluguel de Motos

<div align="center">

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

**Sistema completo de gerenciamento de aluguel de motos com arquitetura limpa e microserviços**

[🚀 Começar](#-como-executar) • [📚 Documentação](#-documentação-da-api) • [🧪 Testes](#-testes) • [🏗️ Arquitetura](#️-arquitetura)

</div>

---

## 📋 Índice

- [🎯 Sobre o Projeto](#-sobre-o-projeto)
- [✨ Funcionalidades](#-funcionalidades)
- [🏗️ Arquitetura](#️-arquitetura)
- [🛠️ Tecnologias](#️-tecnologias)
- [🚀 Como Executar](#-como-executar)
- [📚 Documentação da API](#-documentação-da-api)
- [🧪 Testes](#-testes)
- [🐳 Docker](#-docker)
- [📁 Estrutura do Projeto](#-estrutura-do-projeto)
- [🔧 Configuração](#-configuração)
- [📊 Banco de Dados](#-banco-de-dados)
- [📨 Sistema de Mensageria](#-sistema-de-mensageria)
- [🔄 Eventos e Notificações](#-eventos-e-notificações)
- [📝 Casos de Uso](#-casos-de-uso)
- [🚨 Tratamento de Erros](#-tratamento-de-erros)
- [📈 Logs e Monitoramento](#-logs-e-monitoramento)
- [🔒 Segurança](#-segurança)
- [📦 Deploy](#-deploy)
- [🤝 Contribuição](#-contribuição)
- [📄 Licença](#-licença)

---

## 🎯 Sobre o Projeto

**MotorcycleRent.NET** é um sistema empresarial completo para gerenciamento de aluguel de motos, desenvolvido com as melhores práticas de arquitetura de software. O sistema permite que administradores gerenciem motos e entregadores, enquanto entregadores podem alugar motos e realizar entregas na plataforma.

### 🎖️ Características Principais

- **🏗️ Arquitetura Limpa**: Seguindo princípios SOLID e Clean Architecture
- **🐳 Containerização**: Docker e Docker Compose para fácil deploy
- **🧪 Testes Abrangentes**: 92 testes unitários e de integração
- **📊 Banco Robusto**: PostgreSQL com Entity Framework Core
- **📨 Mensageria Assíncrona**: RabbitMQ para processamento de eventos
- **📚 API RESTful**: Documentada com Swagger/OpenAPI
- **🔒 Validações**: FluentValidation para regras de negócio
- **📈 Logs Estruturados**: ILogger em toda a aplicação (API, Services e Worker)

---

## ✨ Funcionalidades

### 🏍️ Gestão de Motos
- ✅ Cadastro de motos com validação de placa única
- ✅ Consulta e filtros por placa
- ✅ Atualização de informações
- ✅ Remoção (apenas se não houver aluguéis ativos)
- ✅ Eventos automáticos para motos cadastradas

### 🚚 Gestão de Entregadores
- ✅ Cadastro completo com validações
- ✅ Upload de foto da CNH (PNG/BMP)
- ✅ Validação de tipos de CNH (A, B, A+B)
- ✅ Verificação de CNPJ e CNH únicos
- ✅ Atualização de dados pessoais

### 📅 Sistema de Aluguéis
- ✅ 5 planos de aluguel (7, 15, 30, 45, 50 dias)
- ✅ Cálculo automático de valores
- ✅ Validação de datas e disponibilidade
- ✅ Sistema de multas por devolução antecipada
- ✅ Cobrança por dias adicionais

### 🔔 Sistema de Eventos
- ✅ Notificações automáticas via RabbitMQ
- ✅ Processamento assíncrono de eventos
- ✅ Armazenamento de histórico de eventos
- ✅ Consumidor específico para motos 2024

---

## 🏗️ Arquitetura

### 🏛️ Clean Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐  │
│  │   Moto.Api      │  │  Moto.Worker    │  │   DTOs      │  │
│  │   (Controllers) │  │ (Event Handlers)│  │ (External)  │  │
│  └─────────────────┘  └─────────────────┘  └─────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                   Application Layer                         │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐  │
│  │    Services     │  │     DTOs        │  │  Validators │  │
│  │ (Business Logic)│  │  (Internal)     │  │ (Rules)     │  │
│  └─────────────────┘  └─────────────────┘  └─────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                     Domain Layer                            │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐  │
│  │    Entities     │  │   Interfaces    │  │  Exceptions │  │
│  │ (Core Business) │  │ (Contracts)     │  │ (Domain)    │  │
│  └─────────────────┘  └─────────────────┘  └─────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              │
┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                       │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────┐  │
│  │  Repositories   │  │   DbContext     │  │  External   │  │
│  │ (Data Access)   │  │ (EF Core)       │  │  Services   │  │
│  └─────────────────┘  └─────────────────┘  └─────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

### 🔄 Fluxo de Dados

```
1. Controller recebe requisição HTTP
2. Validação com FluentValidation
3. Mapeamento com AutoMapper
4. Serviço de aplicação processa regra de negócio
5. Repositório persiste no banco
6. Evento é publicado no RabbitMQ
7. Worker processa evento assincronamente
```

---

## 🛠️ Tecnologias

### 🎯 Core Framework
- **.NET 8.0** - Framework principal com C# 12
- **Entity Framework Core 9.0.8** - ORM para PostgreSQL
- **AutoMapper 12.0.1** - Mapeamento de objetos
- **FluentValidation 12.0.0** - Validações de entrada

### 🗄️ Banco de Dados
- **PostgreSQL 15** - Banco de dados principal
- **Npgsql 9.0.4** - Provider para EF Core
- **EF Core Migrations** - Versionamento do banco

### 📨 Mensageria
- **RabbitMQ 3.12** - Message broker
- **RabbitMQ.Client 6.8.1** - Cliente .NET

### 🐳 Containerização
- **Docker** - Containerização das aplicações
- **Docker Compose** - Orquestração dos serviços

### 🧪 Testes
- **xUnit 2.6.6** - Framework de testes
- **FluentAssertions 8.6.0** - Assertions expressivas
- **Microsoft.AspNetCore.Mvc.Testing** - Testes de integração

### 📚 Documentação
- **Swagger/OpenAPI 6.6.2** - Documentação da API
- **ILogger** - Sistema de logs estruturados em toda a aplicação

---

## 🚀 Como Executar

### 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### 🔧 Instalação e Configuração

#### 1. **Clone o Repositório**
```bash
git clone <repository-url>
cd MotorcycleRent.NET/moto-backend
```

#### 2. **Inicie os Serviços de Infraestrutura**
```bash
# Inicia PostgreSQL e RabbitMQ
docker-compose up -d

# Verifica status dos serviços
docker-compose ps
```

#### 3. **Execute as Migrações do Banco**
```bash
# Restaura dependências
dotnet restore

# Executa migrações
dotnet ef database update --project src/Moto.Infrastructure --startup-project src/Moto.Api
```

#### 4. **Execute a Aplicação**
```bash
# Terminal 1: API
cd src/Moto.Api
dotnet run

# Terminal 2: Worker (opcional)
cd src/Moto.Worker
dotnet run
```

#### 5. **Acesse a Aplicação**
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000

---

## 📚 Documentação da API

### 🏍️ Endpoints de Motos

#### `POST /api/motorcycles` - Cadastrar Moto
```json
{
  "plate": "ABC1234",
  "year": 2024,
  "model": "Honda CG 160"
}
```

#### `GET /api/motorcycles` - Listar Motos
```
GET /api/motorcycles?plate=ABC1234
```

#### `PUT /api/motorcycles/{id}` - Atualizar Moto
```json
{
  "plate": "XYZ9876",
  "year": 2024,
  "model": "Honda CG 160"
}
```

#### `DELETE /api/motorcycles/{id}` - Remover Moto
```
DELETE /api/motorcycles/1
```

### 🚚 Endpoints de Entregadores

#### `POST /api/couriers` - Cadastrar Entregador
```json
{
  "name": "João Silva",
  "cnpj": "12.345.678/0001-90",
  "dateOfBirth": "1990-01-01",
  "licenseNumber": "12345678901",
  "licenseType": "A"
}
```

#### `PUT /api/couriers/{id}/cnh` - Atualizar Foto da CNH
```
PUT /api/couriers/1/cnh
Content-Type: multipart/form-data
```

### 📅 Endpoints de Aluguéis

#### `POST /api/rentals` - Criar Aluguel
```json
{
  "motorcycleId": 1,
  "courierId": 1,
  "plan": 7,
  "startDate": "2024-01-15",
  "endDate": "2024-01-22"
}
```

#### `PUT /api/rentals/{id}/return` - Finalizar Aluguel
```json
{
  "returnDate": "2024-01-20"
}
```

---

## 🧪 Testes

### 🚀 Executar Todos os Testes
```bash
dotnet test
```

### 🎯 Executar Testes Específicos
```bash
# Testes de domínio
dotnet test tests/Moto.Domain.UnitTests/

# Testes de aplicação
dotnet test tests/Moto.Application.UnitTests/

# Testes de integração da API
dotnet test tests/Moto.Api.IntegrationTests/
```

### 📊 Cobertura de Testes
```bash
# Instalar ferramenta de cobertura
dotnet tool install --global coverlet.collector

# Executar com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### 📈 Resultados dos Testes
- **Domain UnitTests**: 41/41 ✅
- **Application UnitTests**: 36/36 ✅
- **API IntegrationTests**: 15/15 ✅
- **Total**: 92/92 ✅

---

## 🐳 Docker

### 🚀 Comandos Principais
```bash
# Iniciar todos os serviços
docker-compose up -d

# Parar todos os serviços
docker-compose down

# Ver logs em tempo real
docker-compose logs -f

# Ver status dos containers
docker-compose ps
```

### 🏗️ Build das Imagens
```bash
# Build da API
docker build -t moto-api src/Moto.Api/

# Build do Worker
docker build -t moto-worker src/Moto.Worker/

# Build de todas as imagens
docker-compose build
```

### 🔍 Serviços Disponíveis
- **PostgreSQL**: localhost:5432
- **RabbitMQ**: localhost:5672
- **RabbitMQ Management**: http://localhost:15672
- **API**: localhost:5000
- **Worker**: Rodando em background

---

## 📁 Estrutura do Projeto

```
moto-backend/
├── 📁 src/                          # Código fonte
│   ├── 🏠 Moto.Api/                # Camada de apresentação
│   │   ├── 📁 Controllers/         # Controllers da API
│   │   ├── 📁 DTOs/               # DTOs externos
│   │   ├── 📁 Mappings/           # Perfis do AutoMapper
│   │   ├── 📁 Properties/         # Configurações de debug
│   │   ├── 📄 Program.cs          # Configuração da aplicação
│   │   ├── 📄 appsettings.json    # Configurações base
│   │   ├── 📄 Moto.Api.http       # Coleção de requisições HTTP
│   │   └── 🐳 Dockerfile          # Container da API
│   │
│   ├── ⚙️ Moto.Application/        # Camada de aplicação
│   │   ├── 📁 Services/           # Serviços de negócio
│   │   ├── 📁 DTOs/               # DTOs internos
│   │   ├── 📁 Mappings/           # Perfis do AutoMapper
│   │   ├── 📁 Validators/         # Validações FluentValidation
│   │   ├── 📁 Interfaces/         # Contratos dos serviços
│   │   └── 📄 DependencyInjection.cs # Configuração de DI
│   │
│   ├── 🏛️ Moto.Domain/            # Camada de domínio
│   │   ├── 📁 Entities/           # Entidades de domínio
│   │   ├── 📁 Interfaces/         # Contratos dos repositórios
│   │   ├── 📁 Enums/              # Enumerações
│   │   ├── 📁 Exceptions/         # Exceções de domínio
│   │   └── 📁 ValueObjects/       # Objetos de valor
│   │
│   ├── 🗄️ Moto.Infrastructure/    # Camada de infraestrutura
│   │   ├── 📁 DbContext/          # Entity Framework Context
│   │   ├── 📁 Repositories/       # Implementações dos repositórios
│   │   ├── 📁 Services/           # Serviços de infraestrutura
│   │   ├── 📁 Migrations/         # Migrações do banco
│   │   └── 📄 DependencyInjection.cs # Configuração de DI
│   │
│   └── 🔄 Moto.Worker/            # Worker para processamento
│       ├── 📁 Handlers/           # Handlers de eventos
│       ├── 📁 Properties/         # Configurações de debug
│       ├── 📄 Worker.cs           # Worker principal
│       ├── 📄 Program.cs          # Configuração da aplicação
│       ├── 📄 appsettings.json    # Configurações base
│       └── 🐳 Dockerfile          # Container do Worker
│
├── 🧪 tests/                       # Testes
│   ├── 📁 Moto.Api.IntegrationTests/  # Testes de integração
│   ├── 📁 Moto.Application.UnitTests/ # Testes unitários da aplicação
│   └── 📁 Moto.Domain.UnitTests/      # Testes unitários do domínio
│
├── 🐳 docker-compose.yml          # Orquestração Docker
├── 🐳 .dockerignore               # Arquivos ignorados pelo Docker
├── 📄 .gitignore                  # Arquivos ignorados pelo Git
├── 📄 moto-backend.sln            # Solução do projeto
└── 📄 README.md                   # Este arquivo
```

---

## 🔧 Configuração

### ⚙️ Arquivos de Configuração

#### `appsettings.json` (Base)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=moto_db;Username=moto_user;Password=moto_password"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "moto_user",
    "Password": "moto_password"
  }
}
```

#### `appsettings.Development.json` (Desenvolvimento)
```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### 🔐 Variáveis de Ambiente
```bash
# Banco de dados
DATABASE_CONNECTION_STRING=Host=localhost;Database=moto_db;Username=moto_user;Password=moto_password

# RabbitMQ
RABBITMQ_HOST=localhost
RABBITMQ_PORT=5672
RABBITMQ_USERNAME=moto_user
RABBITMQ_PASSWORD=moto_password

# API
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000;https://localhost:5001
```

---

## 📊 Banco de Dados

### 🗄️ Esquema Principal

#### **Motorcycles** (Motos)
```sql
CREATE TABLE Motorcycles (
    Id SERIAL PRIMARY KEY,
    Plate VARCHAR(10) UNIQUE NOT NULL,
    Year INTEGER NOT NULL,
    Model VARCHAR(100) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### **Couriers** (Entregadores)
```sql
CREATE TABLE Couriers (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    CNPJ VARCHAR(18) UNIQUE NOT NULL,
    DateOfBirth DATE NOT NULL,
    LicenseNumber VARCHAR(20) UNIQUE NOT NULL,
    LicenseType VARCHAR(5) NOT NULL,
    CNHImagePath VARCHAR(500),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### **Rentals** (Aluguéis)
```sql
CREATE TABLE Rentals (
    Id SERIAL PRIMARY KEY,
    MotorcycleId INTEGER REFERENCES Motorcycles(Id),
    CourierId INTEGER REFERENCES Couriers(Id),
    Plan INTEGER NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    ExpectedEndDate DATE NOT NULL,
    ReturnDate DATE,
    TotalCost DECIMAL(10,2),
    Status VARCHAR(20) DEFAULT 'Active',
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### **MotorcycleEvents** (Eventos de Motos)
```sql
CREATE TABLE MotorcycleEvents (
    Id SERIAL PRIMARY KEY,
    MotorcycleId INTEGER REFERENCES Motorcycles(Id),
    EventType VARCHAR(50) NOT NULL,
    EventData JSONB,
    ProcessedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### 🔄 Migrações
```bash
# Criar nova migração
dotnet ef migrations add InitialCreate --project src/Moto.Infrastructure --startup-project src/Moto.Api

# Aplicar migrações
dotnet ef database update --project src/Moto.Infrastructure --startup-project src/Moto.Api

# Remover última migração
dotnet ef migrations remove --project src/Moto.Infrastructure --startup-project src/Moto.Api
```

---

## 📨 Sistema de Mensageria

### 🐰 RabbitMQ

#### **Configuração**
- **Host**: localhost
- **Port**: 5672
- **Management UI**: http://localhost:15672
- **Usuário**: moto_user
- **Senha**: moto_password

#### **Exchanges e Filas** (Implementado)
```
motorcycle.events (Exchange)
└── motorcycle.created (Queue)
```

### 🔄 Fluxo de Eventos

#### **1. Moto Cadastrada**
```
API → Publica evento → RabbitMQ → Worker consome → Armazena no banco
```

#### **2. Processamento Assíncrono**
```
Evento publicado → Fila RabbitMQ → Worker processa → Logs estruturados
```

---

## 🔄 Eventos e Notificações

### 📢 Tipos de Eventos


#### **MotorcycleCreatedEvent**
```json
{
  "id": "1",
  "model": "Honda CG 160",
  "plate": "ABC1234",
  "year": 2024
}
```

### 🎯 Handlers de Eventos

#### **MotorcycleCreatedHandler**
- Processa eventos de motos criadas
- Armazena no banco para consulta futura
- Logs estruturados para auditoria
- Notificações específicas para motos 2024

---

## 📝 Casos de Uso

### 🏍️ Gestão de Motos

#### **UC001: Cadastrar Nova Moto**
**Ator**: Administrador
**Pré-condições**: Acesso ao sistema
**Fluxo Principal**:
1. Admin informa dados da moto (placa, ano, modelo)
2. Sistema valida placa única
3. Sistema persiste moto no banco
4. Sistema publica evento `MotorcycleCreated`
5. Sistema confirma cadastro

**Regras de Negócio**:
- Placa deve ser única no sistema
- Ano deve ser válido (não futuro)
- Modelo é obrigatório

#### **UC002: Consultar Motos**
**Ator**: Administrador
**Pré-condições**: Acesso ao sistema
**Fluxo Principal**:
1. Admin solicita lista de motos
2. Sistema retorna motos com filtros opcionais

**Filtros Disponíveis**:
- Por placa (exata ou parcial)

### 🚚 Gestão de Entregadores

#### **UC003: Cadastrar Entregador**
**Ator**: Entregador
**Pré-condições**: Entregador não cadastrado
**Fluxo Principal**:
1. Entregador informa dados pessoais
2. Sistema valida CNPJ e CNH únicos
3. Sistema valida tipo de CNH (A, B, A+B)
4. Sistema persiste entregador no banco
5. Sistema confirma cadastro

**Regras de Negócio**:
- CNPJ deve ser único
- Número da CNH deve ser único
- Tipo de CNH deve ser A, B ou A+B
- Data de nascimento deve ser válida

#### **UC004: Atualizar Foto da CNH**
**Ator**: Entregador
**Pré-condições**: Entregador cadastrado
**Fluxo Principal**:
1. Entregador faz upload da foto
2. Sistema valida formato (PNG/BMP)
3. Sistema armazena em storage externo
4. Sistema atualiza caminho no banco
5. Sistema confirma atualização

### 📅 Sistema de Aluguéis

#### **UC005: Criar Aluguel**
**Ator**: Entregador
**Pré-condições**: Entregador com CNH tipo A
**Fluxo Principal**:
1. Entregador seleciona moto disponível
2. Entregador escolhe plano de aluguel
3. Sistema valida disponibilidade
4. Sistema calcula custo total
5. Sistema cria aluguel no banco
6. Sistema confirma criação do aluguel

**Planos Disponíveis**:
- 7 dias: R$ 30,00/dia
- 15 dias: R$ 28,00/dia
- 30 dias: R$ 22,00/dia
- 45 dias: R$ 20,00/dia
- 50 dias: R$ 18,00/dia

#### **UC006: Finalizar Aluguel**
**Ator**: Entregador
**Pré-condições**: Aluguel ativo
**Fluxo Principal**:
1. Entregador informa data de devolução
2. Sistema calcula custo final
3. Sistema aplica multas se necessário
4. Sistema finaliza aluguel
5. Sistema libera moto para novos aluguéis

**Sistema de Multas**:
- Devolução antecipada: 20% (7 dias) ou 40% (15+ dias)
- Devolução tardia: R$ 50,00 por dia adicional

---

## 🚨 Tratamento de Erros

### 📋 Códigos de Status HTTP

#### **2xx - Sucesso**
- `200 OK`: Operação realizada com sucesso
- `201 Created`: Recurso criado com sucesso

#### **4xx - Erro do Cliente**
- `400 Bad Request`: Dados inválidos
- `404 Not Found`: Recurso não encontrado
- `409 Conflict`: Conflito (ex: placa duplicada)
- `422 Unprocessable Entity`: Validação falhou

#### **5xx - Erro do Servidor**
- `500 Internal Server Error`: Erro interno
- `503 Service Unavailable`: Serviço indisponível

### 🔍 Estrutura de Erro
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Plate": [
      "A placa 'ABC1234' já está em uso."
    ]
  },
  "traceId": "00-1234567890abcdef-1234567890abcdef-00"
}
```

### 🛡️ Validações Implementadas

#### **Entidades**
- Placa única para motos
- CNPJ único para entregadores
- CNH única para entregadores
- Datas válidas para aluguéis

#### **Regras de Negócio**
- Apenas CNH tipo A pode alugar
- Moto deve estar disponível
- Datas de aluguel devem ser válidas
- Cálculo correto de multas

---

## 📈 Logs e Monitoramento

### 📝 Estrutura de Logs

#### **Logging Configuration**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  }
}
```

**Nota**: ILogger está configurado em toda a aplicação (API, Services e Worker) para logging estruturado.

#### **Serviços com Logging Estruturado**
- **MotorcycleService**: Logs de criação, atualização, exclusão e consultas
- **CourierService**: Logs de cadastro, atualização de CNH e validações
- **RentalService**: Logs de criação de aluguéis e validações de negócio
- **Worker**: Logs de processamento de eventos e consumo de filas

### 🔍 Níveis de Log

#### **Information**
- Operações de CRUD
- Eventos de negócio
- Início/fim de transações

#### **Warning**
- Validações falharam
- Tentativas de operação inválida
- Recursos não encontrados

#### **Error**
- Exceções não tratadas
- Falhas de conexão
- Erros de validação crítica

### 📊 Métricas Disponíveis

#### **Endpoints Disponíveis**
- `/` - Swagger UI (documentação da API)
- `/api/motorcycles` - Endpoints de motos
- `/api/couriers` - Endpoints de entregadores
- `/api/rentals` - Endpoints de aluguéis

---

## 🔒 Segurança

### 🛡️ Validações de Entrada

#### **FluentValidation**
- Validação de tipos de dados
- Validação de formatos (CNPJ, CNH)
- Validação de regras de negócio
- Sanitização de entrada

#### **Model Binding**
- Validação automática de modelos
- Prevenção de over-posting
- Validação de tipos primitivos

---

## 📦 Deploy

### 🐳 Docker Production

#### **Build das Imagens**
```bash
# Build otimizado para produção
docker build -t moto-api:latest src/Moto.Api/
docker build -t moto-worker:latest src/Moto.Worker/

# Push para registry
docker tag moto-api:latest your-registry/moto-api:latest
docker push your-registry/moto-api:latest
```

#### **Docker Compose Production**
```yaml
version: '3.8'
services:
  api:
    image: your-registry/moto-api:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    ports:
      - "80:8080"
    depends_on:
      - postgres
      - rabbitmq
```

### 🧪 Padrões de Teste

#### **Nomenclatura**
```csharp
[Fact]
public void Should_CreateMotorcycle_When_ValidDataProvided()
{
    // Arrange
    // Act
    // Assert
}
```

#### **Cobertura Mínima**
- **Domain**: 100%
- **Application**: 90%
- **API**: 80%

---

## 📄 Licença

Este projeto está sob a licença **MIT**. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 📞 Suporte

### 🐛 Reportar Bugs
- Use as [Issues](https://github.com/MuketaUeda/MotorcycleRent.NET/issues) do GitHub
- Inclua logs e passos para reproduzir
- Descreva o comportamento esperado vs. atual

<div align="center">

**Desenvolvido com ❤️ usando .NET 8 e Clean Architecture**

[⬆️ Voltar ao Topo](#-motorcyclerentnet---sistema-de-aluguel-de-motos)

</div>
  

