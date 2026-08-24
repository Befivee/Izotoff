# Локальный деплой на сервер (альтернатива GitHub Actions).
# Не трогает Waldau: каталог /var/www/izotoff, порт 5010.

$server = "root@188.225.45.211"
$path = "/var/www/izotoff"
$project = $PSScriptRoot

Write-Host "Publishing project..."
dotnet publish "$project\Izotoff.csproj" -c Release -o "$project\publish"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Uploading to $path ..."
scp -r "$project\publish\*" "${server}:$path/"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Restarting izotoff..."
ssh $server "systemctl restart izotoff && systemctl is-active izotoff"

Write-Host "Done. Site: http://188.225.45.211:8080"
