$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$sdkExecutable = & (Join-Path $PSScriptRoot 'get-sdk.ps1')
$buildDirectory = Join-Path $workspaceRoot 'src/Codice.Desktop/bin/Release/net10.0-windows'
if (-not (Test-Path -LiteralPath (Join-Path $buildDirectory 'Codice.Desktop.dll'))) {
    throw 'Compile Release before capturing the interface.'
}
$previewRoot = Join-Path $workspaceRoot ('.tools/qa-ui-' + [Guid]::NewGuid().ToString('N'))
$output = Join-Path $workspaceRoot 'docs/03-codice/test-evidence/current/ui'
New-Item -ItemType Directory -Path $previewRoot -Force | Out-Null
Get-ChildItem -LiteralPath $buildDirectory | Where-Object Name -ne 'win-x64' | ForEach-Object {
    Copy-Item -LiteralPath $_.FullName -Destination $previewRoot -Recurse
}
$previousRoot = $env:DOTNET_ROOT
try {
    $env:DOTNET_ROOT = Split-Path $sdkExecutable -Parent
    $previewArguments = @(('"' + (Join-Path $previewRoot 'Codice.Desktop.dll') + '"'), '--capture-preview', ('"' + $output + '"'))
    $process = Start-Process -FilePath $sdkExecutable -ArgumentList $previewArguments -WindowStyle Hidden -PassThru
    if (-not $process.WaitForExit(45000)) { $process.Kill(); throw 'Interface rendering timed out.' }
    $process.Refresh()
    if ($process.ExitCode -ne 0) { throw "Interface verification failed. Review $output/preview-error.txt and binding-errors.txt." }
    Write-Output "Actual WPF renderings saved in $output"
    Write-Output 'Fictitious data were created only in an isolated copy, never in the delivery or working database.'
} finally {
    $env:DOTNET_ROOT = $previousRoot
}
