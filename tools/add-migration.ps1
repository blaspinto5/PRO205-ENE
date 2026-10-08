param([Parameter(Mandatory = $true)][ValidatePattern('^[A-Za-z][A-Za-z0-9]*$')][string]$Name)
$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$sdkExecutable = & (Join-Path $PSScriptRoot 'get-sdk.ps1')
$previousPath = $env:PATH
$previousRoot = $env:DOTNET_ROOT
Push-Location $workspaceRoot
try {
    $env:DOTNET_ROOT = Split-Path $sdkExecutable -Parent
    $env:PATH = $env:DOTNET_ROOT + ';' + $env:PATH
    & $sdkExecutable tool restore
    if ($LASTEXITCODE -ne 0) { throw 'EF tool restoration failed.' }
    & $sdkExecutable tool run dotnet-ef migrations add $Name --project src/Codice.Infrastructure --output-dir Migrations
    if ($LASTEXITCODE -ne 0) { throw 'Migration generation failed.' }
} finally {
    $env:PATH = $previousPath
    $env:DOTNET_ROOT = $previousRoot
    Pop-Location
}
