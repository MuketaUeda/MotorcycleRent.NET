# Motorcycle Rent API - Backend Challenge

Seja muito bem-vindo ao desafio backend da Mottu, obrigado pelo interesse em fazer parte do nosso time e ajudar a melhorar a vida de milhares de pessoas.

## 🏗️ Estrutura do Projeto

```
moto-backend/
├── src/
│   ├── Moto.Api/                    # Camada de Apresentação
│   │   ├── Controllers/             # Controllers da API
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   │   ├── Motorcycles/         # DTOs para motos
│   │   │   ├── Couriers/            # DTOs para entregadores
│   │   │   └── Rentals/             # DTOs para aluguéis
│   │   ├── Program.cs               # Configuração da aplicação
│   │   └── appsettings.json         # Configurações
│   │
│   ├── Moto.Application/            # Camada de Aplicação
│   │   ├── Services/                # Serviços de negócio
│   │   ├── DTOs/                    # DTOs internos da aplicação
│   │   ├── Mappings/                # AutoMapper profiles
│   │   └── Validators/              # Validações
│   │
│   ├── Moto.Domain/                 # Camada de Domínio
│   │   ├── Entities/                # Entidades de domínio
│   │   ├── Interfaces/              # Contratos dos repositórios
│   │   ├── Enums/                   # Enumerações
│   │   ├── Exceptions/              # Exceções de domínio
│   │   └── ValueObjects/            # Objetos de valor
│   │
│   ├── Moto.Infrastructure/         # Camada de Infraestrutura
│   │   ├── DbContext/               # Entity Framework Context
│   │   ├── Repositories/            # Implementações dos repositórios
│   │   ├── Migrations/              # Migrações do banco
│   │   └── DependencyInjection.cs   # Configuração de DI
│   │
│   └── Moto.Worker/                 # Worker para processamento de mensagens
│       ├── Handlers/                # Handlers de eventos
│       └── Worker.cs                # Worker principal
│
├── tests/                           # Testes
│   ├── Moto.Api.IntegrationTests/   # Testes de integração da API
│   ├── Moto.Application.UnitTests/  # Testes unitários da aplicação
│   └── Moto.Domain.UnitTests/       # Testes unitários do domínio
│
├── docker-compose.yml               # Configuração Docker
└── moto-backend.sln                 # Solução do projeto
```

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **RabbitMQ** - Sistema de mensageria
- **Docker & Docker Compose** - Containerização
- **Swagger/OpenAPI** - Documentação da API
- **AutoMapper** - Mapeamento de objetos
- **FluentValidation** - Validações

## 🏛️ Arquitetura

O projeto segue os princípios da **Clean Architecture** com as seguintes camadas:

- **Domain**: Entidades, interfaces e regras de negócio
- **Application**: Casos de uso e serviços de aplicação
- **Infrastructure**: Implementações de repositórios e banco de dados
- **API**: Controllers e DTOs de apresentação

## 🛠️ Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- Docker e Docker Compose

### Passos para execução

1. **Clone o repositório**
```bash
git clone <repository-url>
cd moto-backend
```

2. **Inicie os serviços do Docker**
```bash
docker-compose up -d
```

3. **Execute as migrações do banco**
```bash
dotnet ef database update --project src/Moto.Infrastructure --startup-project src/Moto.Api
```

4. **Execute a API**
```bash
dotnet run --project src/Moto.Api
```

5. **Acesse a documentação**
```
http://localhost:5215
```

## 📋 Endpoints Disponíveis

### Motorcycles
- `POST /api/motorcycles` - Criar moto
- `GET /api/motorcycles` - Listar motos
- `GET /api/motorcycles/{id}` - Buscar moto por ID
- `PUT /api/motorcycles/{id}` - Atualizar moto
- `DELETE /api/motorcycles/{id}` - Remover moto

### Couriers
- `POST /api/couriers` - Cadastrar entregador
- `GET /api/couriers` - Listar entregadores
- `PUT /api/couriers/{id}/cnh` - Atualizar foto da CNH

### Rentals
- `POST /api/rentals` - Criar aluguel
- `GET /api/rentals` - Listar aluguéis
- `PUT /api/rentals/{id}/return` - Finalizar aluguel

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes específicos
dotnet test tests/Moto.Domain.UnitTests/
dotnet test tests/Moto.Application.UnitTests/
dotnet test tests/Moto.Api.IntegrationTests/
```

## 📦 Docker

```bash
# Iniciar serviços
docker-compose up -d

# Parar serviços
docker-compose down

# Ver logs
docker-compose logs -f
```

---

## Instruções Originais do Desafio

- O desafio é válido para diversos níveis, portanto não se preocupe se não conseguir resolver por completo.
- A aplicação só será avaliada se estiver rodando, se necessário crie um passo a passo para isso.
- Faça um clone do repositório em seu git pessoal para iniciar o desenvolvimento e não cite nada relacionado a Mottu.
- Após teste realizado, favor encaminha-lo via Link abaixo:
Link: [Formulário - Mottu - Desafio Backend](https://forms.office.com/r/25yMPCax5S)

## Requisitos não funcionais 
- A aplicação deverá ser construida com .Net utilizando C#.
- Utilizar apenas os seguintes bancos de dados (Postgress, MongoDB)
    - Não utilizar PL/pgSQL
- Escolha o sistema de mensageria de sua preferencia( RabbitMq, Sqs/Sns , Kafka, Gooogle Pub/Sub ou qualquer outro)

## Aplicação a ser desenvolvida
Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. Quando um entregador estiver registrado e com uma locação ativa poderá também efetuar entregas de pedidos disponíveis na plataforma.

Iremos executar um teste de integração para validar os cenários de uso. Por isso, sua aplicação deve seguir exatamente as especificações de API`s Rest do nosso Swager: request, response e status code.
Garanta que os atributos dos JSON`s e estão de acordo com o Swagger abaixo.

Swagger de referência:
https://app.swaggerhub.com/apis-docs/Mottu/mottu_desafio_backend/1.0.0

### Casos de uso
- Eu como usuário admin quero cadastrar uma nova moto.
  - Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
  - A placa é um dado único e não pode se repetir.
  - Quando a moto for cadastrada a aplicação deverá gerar um evento de moto cadastrada
    - A notificação deverá ser publicada por mensageria.
    - Criar um consumidor para notificar quando o ano da moto for "2024"
    - Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.
- Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
    - Os dados do entregador são( identificador, nome, cnpj, data de nascimento, número da CNHh, tipo da CNH, imagemCNH)
    - Os tipos de cnh válidos são A, B ou ambas A+B.
    - O cnpj é único e não pode se repetir.
    - O número da CNH é único e não pode se repetir.
- Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - O formato do arquivo deve ser png ou bmp.
    - A foto não poderá ser armazenada no banco de dados, você pode utilizar um serviço de storage( disco local, amazon s3, minIO ou outros).
- Eu como entregador quero alugar uma moto por um período.
    - Os planos disponíveis para locação são:
        - 7 dias com um custo de R$30,00 por dia
        - 15 dias com um custo de R$28,00 por dia
        - 30 dias com um custo de R$22,00 por dia
        - 45 dias com um custo de R$20,00 por dia
        - 50 dias com um custo de R$18,00 por dia
    - A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término.
    - O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
    - Somente entregadores habilitados na categoria A podem efetuar uma locação
- Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
    - Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
        - Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
        - Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
    - Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.

## Diferenciais 🚀
- ✅ Testes unitários
- ✅ Testes de integração
- ✅ EntityFramework e/ou Dapper
- ✅ Docker e Docker Compose
- ✅ Design Patterns
- ✅ Documentação
- ✅ Tratamento de erros
- ✅ Arquitetura e modelagem de dados
- ✅ Código escrito em língua inglesa
- ✅ Código limpo e organizado
- ✅ Logs bem estruturados
- ✅ Seguir convenções utilizadas pela comunidade
  

