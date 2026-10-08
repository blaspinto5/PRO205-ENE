$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$workspaceSdk = Join-Path $workspaceRoot '.tools/dotnet/dotnet.exe'
$userSdk = Join-Path $env:LOCALAPPDATA 'Codice/dotnet/dotnet.exe'
if (Test-Path -LiteralPath $workspaceSdk) {
    $workspaceSdk
} elseif (Test-Path -LiteralPath $userSdk) {
    $userSdk
} else {
    (Get-Command dotnet -ErrorAction Stop).Source
}
