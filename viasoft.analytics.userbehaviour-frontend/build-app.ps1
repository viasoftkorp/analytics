Write-Host "------------------------------------------------------"
Write-Host "                  BUILD PORTAL APP                    "
Write-Host "------------------------------------------------------"

Write-Host ""
$CDN_URL = Read-Host "Informe a URL do CDN"
$BACKEND_URL = Read-Host "Informe a URL do Gateway"
$APP_NAME = Read-Host "Informe o nome do APP(Ex.: aps)"

if($CDN_URL.Substring($CDN_URL.Length - 1) -eq '/') {
    $CDN_URL = $CDN_URL -replace ".$"
}

Write-Host ""
Write-Host "Instalando/Atualizando pacotes NPM..."
Write-Host ""
npm install

Write-Host ""
Write-Host "Realizando build do app..."
Write-Host ""
ng build $APP_NAME-portal --configuration production --deployUrl=$CDN_URL/$APP_NAME/v1.0.0/

Write-Host ""
Write-Host "Substituindo URL do Gateway..."
Write-Host ""
(Get-Content .\dist\**\assets\app-settings\appsettings.json).replace('!BACKEND_URL', $BACKEND_URL) | Set-Content .\dist\**\assets\app-settings\appsettings.json

Write-Host ""
Write-Host "Build concluído com sucesso!"
Write-Host ""

Start-Sleep 5
