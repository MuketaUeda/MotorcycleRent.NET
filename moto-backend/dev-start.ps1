# Script para iniciar ambiente de desenvolvimento
Write-Host "🚀 Iniciando ambiente de desenvolvimento..." -ForegroundColor Green

# Parar containers existentes
Write-Host "📦 Parando containers existentes..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml down

# Iniciar apenas PostgreSQL e RabbitMQ
Write-Host "🐘 Iniciando PostgreSQL e RabbitMQ..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml up -d

# Aguardar serviços ficarem prontos
Write-Host "⏳ Aguardando serviços ficarem prontos..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Verificar status
Write-Host "✅ Verificando status dos serviços..." -ForegroundColor Green
docker-compose -f docker-compose.dev.yml ps

Write-Host ""
Write-Host "🎯 Ambiente de desenvolvimento pronto!" -ForegroundColor Green
Write-Host "📊 PostgreSQL: localhost:5432" -ForegroundColor Cyan
Write-Host "🐰 RabbitMQ: localhost:5672" -ForegroundColor Cyan
Write-Host "🌐 RabbitMQ Management: http://localhost:15672" -ForegroundColor Cyan
Write-Host ""
Write-Host "💡 Para rodar a API localmente:" -ForegroundColor Yellow
Write-Host "   cd src/Moto.Api && dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "💡 Para rodar o Worker localmente:" -ForegroundColor Yellow
Write-Host "   cd src/Moto.Worker && dotnet run" -ForegroundColor White
