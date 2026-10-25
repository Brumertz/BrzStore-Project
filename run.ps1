$root = $PSScriptRoot

# Start BrzSaleApi backend in a separate terminal
Write-Host "Starting BrzSaleApi..." -ForegroundColor Cyan
Start-Process powershell -WorkingDirectory $root -ArgumentList "-NoExit", "-Command", "dotnet run --project BrzSaleApi\BrzSaleApi"

# Wait for the API to initialize before starting the web frontend
Write-Host "Waiting 4 seconds for API to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 4

# Start BrzSaleWebApp frontend in a separate terminal
Write-Host "Starting BrzSaleWebApp..." -ForegroundColor Cyan
Start-Process powershell -WorkingDirectory $root -ArgumentList "-NoExit", "-Command", "dotnet run --project BrzSaleApi\BrzSaleWebApp"

Write-Host "Both projects are now running in their respective windows." -ForegroundColor Green
