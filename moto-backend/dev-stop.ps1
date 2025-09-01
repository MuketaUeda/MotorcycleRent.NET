# Script para parar ambiente de desenvolvimento
Write-Host "🛑 Parando ambiente de desenvolvimento..." -ForegroundColor Red

# Parar containers
Write-Host "📦 Parando containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.dev.yml down

Write-Host "✅ Ambiente de desenvolvimento parado!" -ForegroundColor Green
