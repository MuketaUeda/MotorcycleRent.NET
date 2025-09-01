# 🚀 Guia de Desenvolvimento - MotorcycleRent.NET

## 📋 **Opções de Execução**

### **Opção 1: Desenvolvimento Local (Recomendado)**
Rode apenas PostgreSQL e RabbitMQ em containers, e execute a aplicação localmente.

### **Opção 2: Docker Completo**
Rode toda a aplicação em containers Docker.

---

## 🔧 **Desenvolvimento Local**

### **1. Iniciar Infraestrutura**
```powershell
# Usar script automatizado
.\dev-start.ps1

# Ou manualmente
docker-compose -f docker-compose.dev.yml up -d
```

### **2. Executar Migrações**
```powershell
cd src/Moto.Api
dotnet ef database update
```

### **3. Rodar API Localmente**
```powershell
cd src/Moto.Api
dotnet run
```
**Acesso:** http://localhost:5215 (Swagger)

### **4. Rodar Worker Localmente**
```powershell
cd src/Moto.Worker
dotnet run
```

### **5. Parar Infraestrutura**
```powershell
# Usar script automatizado
.\dev-stop.ps1

# Ou manualmente
docker-compose -f docker-compose.dev.yml down
```

---

## 🐳 **Docker Completo**

### **1. Iniciar Tudo**
```powershell
docker-compose up -d
```

### **2. Verificar Status**
```powershell
docker-compose ps
```

### **3. Ver Logs**
```powershell
docker-compose logs [serviço]
# Ex: docker-compose logs moto-api
```

### **4. Parar Tudo**
```powershell
docker-compose down
```

---

## 🌐 **Acessos**

| Serviço | Local | Docker | Credenciais |
|---------|-------|--------|-------------|
| **API/Swagger** | http://localhost:5215 | http://localhost:5000 | - |
| **PostgreSQL** | localhost:5432 | localhost:5432 | moto_user/moto_password |
| **RabbitMQ** | localhost:5672 | localhost:5672 | moto_user/moto_password |
| **RabbitMQ Management** | http://localhost:15672 | http://localhost:15672 | moto_user/moto_password |

---

## 🛠️ **Ferramentas Úteis**

### **PostgreSQL**
- **pgAdmin**: Interface gráfica para PostgreSQL
- **DBeaver**: Cliente universal de banco de dados
- **psql**: Cliente de linha de comando

### **RabbitMQ**
- **Management UI**: http://localhost:15672
- **Credenciais**: moto_user / moto_password

---

## 🔍 **Troubleshooting**

### **Problema: Não consigo conectar ao PostgreSQL**
```powershell
# Verificar se o container está rodando
docker-compose -f docker-compose.dev.yml ps

# Ver logs do PostgreSQL
docker-compose -f docker-compose.dev.yml logs postgres

# Testar conexão
docker exec -it moto-postgres-dev psql -U moto_user -d moto_db
```

### **Problema: API não inicia**
```powershell
# Verificar se as migrações foram aplicadas
cd src/Moto.Api
dotnet ef database update

# Verificar configurações
cat appsettings.Development.json
```

### **Problema: Worker não conecta ao RabbitMQ**
```powershell
# Verificar se RabbitMQ está rodando
docker-compose -f docker-compose.dev.yml logs rabbitmq

# Acessar Management UI
# http://localhost:15672
```

---

## 📝 **Comandos Úteis**

### **Desenvolvimento**
```powershell
# Limpar e restaurar pacotes
dotnet clean
dotnet restore

# Build
dotnet build

# Testes
dotnet test

# Migrações
dotnet ef migrations add NomeDaMigracao
dotnet ef database update
```

### **Docker**
```powershell
# Rebuild imagens
docker-compose build

# Rebuild específico
docker-compose build moto-api

# Ver logs em tempo real
docker-compose logs -f [serviço]

# Executar comando no container
docker exec -it [container] [comando]
```
