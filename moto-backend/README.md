# 🏍️ Moto Backend

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-4169E1?style=flat-square&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.12-FF6600?style=flat-square&logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&logo=docker&logoColor=white)](https://www.docker.com/)

## 📋 Descrição

Sistema completo de **aluguel de motos** e **gerenciamento de entregadores** desenvolvido com as melhores práticas de arquitetura de software. O projeto implementa uma solução robusta e escalável para gestão de frotas de motocicletas e operações de entrega.

## 🎯 Objetivos do Projeto

- **Gestão de Motocicletas**: CRUD completo com validações e regras de negócio
- **Cadastro de Entregadores**: Sistema de registro com validação de CNH
- **Sistema de Locação**: Planos flexíveis com cálculo automático de valores
- **Mensageria Assíncrona**: Processamento de eventos com RabbitMQ
- **API RESTful**: Endpoints documentados com Swagger/OpenAPI

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** e **Domain-Driven Design (DDD)**:

```
📦 moto-backend
├── 🌐 src/
│   ├── 🎯 Moto.Api/              # Camada de Apresentação (HTTP)
│   ├── ⚙️ Moto.Worker/           # Camada de Apresentação (Mensageria)  
│   ├── 🔄 Moto.Application/      # Camada de Aplicação (Casos de Uso)
│   ├── 💎 Moto.Domain/           # Camada de Domínio (Regras de Negócio)
│   └── 🔧 Moto.Infrastructure/   # Camada de Infraestrutura (Dados/I/O)
└── 🧪 tests/
    ├── Moto.Domain.UnitTests/
    ├── Moto.Application.UnitTests/
    └── Moto.Api.IntegrationTests/
```

### 🎨 Padrões Implementados

- ✅ **Clean Architecture** - Separação clara de responsabilidades
- ✅ **Domain-Driven Design** - Modelagem rica do domínio  
- ✅ **CQRS Pattern** - Separação de comandos e consultas
- ✅ **Event-Driven Architecture** - Comunicação assíncrona
- ✅ **Repository Pattern** - Abstração de acesso a dados
- ✅ **Dependency Injection** - Inversão de controle

## 🛠️ Tecnologias

### Backend
- **[.NET 8](https://dotnet.microsoft.com/)** - Framework principal
- **[ASP.NET Core Web API](https://docs.microsoft.com/aspnet/core/web-api/)** - API RESTful
- **[Entity Framework Core](https://docs.microsoft.com/ef/core/)** - ORM para acesso a dados
- **[AutoMapper](https://automapper.org/)** - Mapeamento de objetos
- **[FluentValidation](https://fluentvalidation.net/)** - Validação de dados

### Infraestrutura
- **[PostgreSQL 15](https://www.postgresql.org/)** - Banco de dados principal
- **[RabbitMQ](https://www.rabbitmq.com/)** - Message broker
- **[Docker](https://www.docker.com/)** - Containerização
- **[Swagger/OpenAPI](https://swagger.io/)** - Documentação da API

### Testes
- **[xUnit](https://xunit.net/)** - Framework de testes
- **[Coverlet](https://github.com/coverlet-coverage/coverlet)** - Cobertura de código

## 🚀 Como Executar

### Pré-requisitos

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download)
- ✅ [Docker Desktop](https://www.docker.com/products/docker-desktop)
- ✅ [Git](https://git-scm.com/)

### 🐳 Executando com Docker

1. **Clone o repositório**
   ```bash
   git clone <url-do-repositorio>
   cd moto-backend
   ```

2. **Inicie os serviços de infraestrutura**
   ```bash
   docker-compose up -d
   ```

3. **Execute a aplicação**
   ```bash
   dotnet run --project src/Moto.Api
   ```

4. **Acesse a documentação da API**
   - 🌐 **Swagger UI**: http://localhost:5000/swagger
   - 📊 **RabbitMQ Management**: http://localhost:15672
     - 👤 **Usuário**: `moto_user`
     - 🔑 **Senha**: `moto_password`

### 🔧 Executando Localmente

```bash
# Restaurar dependências
dotnet restore

# Compilar a solução
dotnet build

# Executar testes
dotnet test

# Executar a API
dotnet run --project src/Moto.Api
```

## 🗃️ Banco de Dados

### Configuração do PostgreSQL

```yaml
# docker-compose.yml
postgres:
  image: postgres:15
  environment:
    POSTGRES_DB: moto_db
    POSTGRES_USER: moto_user
    POSTGRES_PASSWORD: moto_password
  ports:
    - "5432:5432"
```

### Entidades Principais

- 🏍️ **Motorcycle** - Gestão de motocicletas
- 👤 **Courier** - Cadastro de entregadores  
- 📋 **Rental** - Sistema de locações

## 📡 API Endpoints

### 🏍️ Motocicletas
```http
GET    /api/motorcycles          # Listar motocicletas
POST   /api/motorcycles          # Cadastrar motocicleta
GET    /api/motorcycles/{id}     # Buscar por ID
PUT    /api/motorcycles/{id}     # Atualizar motocicleta
DELETE /api/motorcycles/{id}     # Remover motocicleta
```

### 👤 Entregadores
```http
GET    /api/couriers             # Listar entregadores
POST   /api/couriers             # Cadastrar entregador
GET    /api/couriers/{id}        # Buscar por ID
PUT    /api/couriers/{id}        # Atualizar perfil
POST   /api/couriers/{id}/cnh    # Upload foto CNH
```

### 📋 Locações
```http
GET    /api/rentals              # Listar locações
POST   /api/rentals              # Criar locação
GET    /api/rentals/{id}         # Buscar por ID
PUT    /api/rentals/{id}/return  # Finalizar locação
```

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Executar testes específicos
dotnet test tests/Moto.Domain.UnitTests/
dotnet test tests/Moto.Application.UnitTests/
dotnet test tests/Moto.Api.IntegrationTests/
```

## 📊 Status do Projeto

### ✅ Concluído

- [x] Estrutura da solução (.NET 8)
- [x] Configuração de projetos e dependências
- [x] Clean Architecture implementada
- [x] Entidades de domínio definidas
- [x] Docker Compose configurado
- [x] PostgreSQL e RabbitMQ funcionando
- [x] Compilação bem-sucedida
- [x] Projetos de teste estruturados

### 🚧 Em Desenvolvimento

- [ ] Implementação dos repositórios
- [ ] Configuração do Entity Framework
- [ ] Implementação dos serviços de aplicação
- [ ] Controllers da API
- [ ] Validações com FluentValidation
- [ ] Mapeamento com AutoMapper
- [ ] Workers para processamento de mensagens
- [ ] Testes unitários e de integração

### 🔮 Próximas Funcionalidades

- [ ] Sistema de autenticação/autorização
- [ ] Logs estruturados com Serilog
- [ ] Monitoramento e métricas
- [ ] Cache com Redis
- [ ] Upload de arquivos para cloud storage
- [ ] Notificações push
- [ ] API de geolocalização

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-funcionalidade`)
3. Commit suas mudanças (`git commit -am 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/nova-funcionalidade`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

---

<div align="center">

**Desenvolvido com ❤️ usando .NET 8**

[![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)

</div>