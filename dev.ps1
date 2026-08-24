# Локальная разработка с автоперезапуском (dotnet watch).
# Запуск: .\dev.ps1
# С SCSS:   .\dev.ps1 -Scss

param([switch]$Scss)

$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot

if ($Scss) {
    if (-not (Get-Command npm -ErrorAction SilentlyContinue)) {
        Write-Warning "npm не найден — SCSS watch пропущен. Установите Node.js или правьте wwwroot/css/main.css напрямую."
    }
    else {
        if (-not (Test-Path "node_modules")) {
            Write-Host "npm install..."
            npm install
        }
        Write-Host "SCSS watch → wwwroot/css/main.css"
        Start-Process powershell -ArgumentList @(
            "-NoExit", "-Command",
            "Set-Location '$PSScriptRoot'; npm run scss:watch"
        ) | Out-Null
    }
}

Write-Host ""
Write-Host "Сайт: http://localhost:5210" -ForegroundColor Green
Write-Host "Сохраняйте файлы — dotnet watch пересоберёт проект автоматически." -ForegroundColor DarkGray
Write-Host "Остановка: Ctrl+C" -ForegroundColor DarkGray
Write-Host ""

dotnet watch run --launch-profile http --project Izotoff.csproj
