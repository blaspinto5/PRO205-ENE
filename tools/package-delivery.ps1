$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$deliveryRoot = Join-Path $workspaceRoot 'entrega'
$release = Get-ChildItem -LiteralPath $deliveryRoot -Directory | Sort-Object Name -Descending | Select-Object -First 1
if (-not $release) { throw 'Publish the application first.' }
$verification = Get-Content -LiteralPath (Join-Path $workspaceRoot 'docs/03-codice/test-evidence/current/portable-startup.json') -Raw | ConvertFrom-Json
if ($verification.Release -ne $release.Name -or $verification.StartupExitCode -ne 0 -or -not $verification.IncludesManual) {
    throw 'The current publication must be verified with its manual before packaging.'
}
Compress-Archive -LiteralPath $release.FullName -DestinationPath (Join-Path $deliveryRoot 'Codice-win-x64.zip') -CompressionLevel Optimal -Force
$sourceStage = Join-Path $workspaceRoot ('.tools/source-package-' + [Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $sourceStage -Force | Out-Null
foreach ($directory in @('src', 'tests', 'docs', 'tools', 'output')) {
    foreach ($file in Get-ChildItem -LiteralPath (Join-Path $workspaceRoot $directory) -Recurse -File -Force) {
        if ($file.FullName -match '\\(bin|obj)\\' -or $file.Name -match '^\.env|\.db(-wal|-shm|-journal)?$|\.sqlite3?$|\.user$|\.suo$' -or $file.Name -eq '.gitkeep') { continue }
        $relative = $file.FullName.Substring($workspaceRoot.Length + 1)
        $destination = Join-Path $sourceStage $relative
        New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
        Copy-Item -LiteralPath $file.FullName -Destination $destination
    }
}
foreach ($file in @('README.md', 'Codice.sln', 'Directory.Build.props', 'global.json', '.editorconfig', '.gitignore', '.gitattributes', '.config/dotnet-tools.json')) {
    $destination = Join-Path $sourceStage $file
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $workspaceRoot $file) -Destination $destination
}
# ZipFile preserves dotfiles needed to restore the development environment.
Add-Type -AssemblyName System.IO.Compression.FileSystem
$sourceZip = Join-Path $deliveryRoot 'PRO205-fuentes-y-evidencias.zip'
if (Test-Path -LiteralPath $sourceZip) { Remove-Item -LiteralPath $sourceZip }
[IO.Compression.ZipFile]::CreateFromDirectory($sourceStage, $sourceZip, [IO.Compression.CompressionLevel]::Optimal, $false)
Get-ChildItem -LiteralPath $deliveryRoot -File -Filter '*.zip' | ForEach-Object {
    (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash + '  ' + $_.Name
} | Set-Content -LiteralPath (Join-Path $deliveryRoot 'SHA256SUMS.txt') -Encoding utf8
Write-Output 'Application ZIP and separate source/documentation/evidence ZIP created.'
