param([string]$ReleaseDirectory)
$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
if (-not $ReleaseDirectory) {
    $ReleaseDirectory = (Get-ChildItem -LiteralPath (Join-Path $workspaceRoot 'entrega') -Directory | Sort-Object Name -Descending | Select-Object -First 1).FullName
}
$release = (Resolve-Path -LiteralPath $ReleaseDirectory).Path
$scratch = Join-Path $workspaceRoot ('.tools/ui-release-' + [Guid]::NewGuid().ToString('N'))
$copy = Join-Path $scratch 'app'
$renders = Join-Path $scratch 'renders'
New-Item -ItemType Directory -Path $scratch -Force | Out-Null
Copy-Item -LiteralPath $release -Destination $copy -Recurse
$process = Start-Process -FilePath (Join-Path $copy 'Codice.Desktop.exe') -ArgumentList @('--capture-preview', ('"' + $renders + '"')) -WorkingDirectory $env:TEMP -WindowStyle Hidden -PassThru
if (-not $process.WaitForExit(45000)) { $process.Kill(); throw 'Release UI rendering timed out.' }
$process.Refresh()
if ($process.ExitCode -ne 0) { throw "Release UI rendering failed; inspect $renders." }
$images = @(Get-ChildItem -LiteralPath $renders -Filter '*.png')
if ($images.Count -ne 12) { throw "Expected 12 fresh previews, found $($images.Count)." }
$bindingLog = Join-Path $renders 'binding-errors.txt'
if (-not (Test-Path -LiteralPath $bindingLog) -or (Get-Item -LiteralPath $bindingLog).Length -ne 0) { throw 'Missing binding evidence or binding errors detected.' }
$destination = Join-Path $workspaceRoot 'docs/03-codice/test-evidence/current/ui'
New-Item -ItemType Directory -Path $destination -Force | Out-Null
foreach ($file in Get-ChildItem -LiteralPath $renders -File) { Copy-Item -LiteralPath $file.FullName -Destination $destination -Force }
$evidence = [ordered]@{
    VerifiedAtUtc = [DateTimeOffset]::UtcNow.ToString('O')
    Release = Split-Path $release -Leaf
    ExecutableSha256 = (Get-FileHash -LiteralPath (Join-Path $release 'Codice.Desktop.exe') -Algorithm SHA256).Hash
    DesktopAssemblySha256 = (Get-FileHash -LiteralPath (Join-Path $release 'Codice.Desktop.dll') -Algorithm SHA256).Hash
    PackageManifestSha256 = (Get-FileHash -LiteralPath (Join-Path $release 'SHA256SUMS.txt') -Algorithm SHA256).Hash
    ExitCode = $process.ExitCode
    FreshPreviewCount = $images.Count
    BindingErrorCount = 0
    ManualInteractionVerified = $false
    HumanVisualReviewVerified = $false
    CleanMachineVerified = $false
    Scope = 'Automatic WPF rendering of the delivered executable in an isolated copy; manual and human visual acceptance are separate.'
    Images = @($images | ForEach-Object { [ordered]@{ Name = $_.Name; Sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash } })
}
$evidence | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $workspaceRoot 'docs/03-codice/test-evidence/current/release-ui.json') -Encoding utf8
Write-Output 'Release UI: 12 fresh WPF previews, zero binding errors. Manual acceptance remains separate.'
