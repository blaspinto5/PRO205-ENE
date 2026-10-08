param([switch]$IncludeManual)
$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$desktopProject = Join-Path $workspaceRoot 'src/Codice.Desktop/Codice.Desktop.csproj'
if (-not (Test-Path -LiteralPath $desktopProject)) {
    throw 'Portable publishing is prepared, but the Desktop project must be implemented in phase 6 first.'
}
$manualPath = Join-Path $workspaceRoot 'output/pdf/codice-technical-manual.pdf'
if ($IncludeManual -and -not (Test-Path -LiteralPath $manualPath)) {
    throw 'The verified technical manual is missing. Generate it before packaging the final delivery.'
}
$sdkExecutable = & (Join-Path $PSScriptRoot 'get-sdk.ps1')
$releaseId = 'Codice-win-x64-' + [DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8)
$releaseDirectory = Join-Path $workspaceRoot ('entrega/' + $releaseId)
Push-Location $workspaceRoot
try {
    & $sdkExecutable publish $desktopProject --configuration Release --runtime win-x64 --self-contained true '-p:PublishProfile=PortableWinX64' --output $releaseDirectory
    if ($LASTEXITCODE -ne 0) { throw 'Portable publishing failed; do not distribute its output.' }
    if (-not (Test-Path -LiteralPath (Join-Path $releaseDirectory 'Codice.Desktop.exe'))) {
        throw 'The expected application executable is missing; check the Desktop assembly name.'
    }
    $packagedDatabases = @(Get-ChildItem -LiteralPath $releaseDirectory -Recurse -File | Where-Object {
        $_.Name -match '\.(db|sqlite|sqlite3)(-wal|-shm|-journal)?$'
    })
    if ($packagedDatabases.Count -gt 0) {
        throw 'Published output contains a database. Remove database content from the project publishing configuration and publish again.'
    }
    New-Item -ItemType Directory -Path (Join-Path $releaseDirectory 'datos') -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $workspaceRoot 'docs/03-codice/portable/LEEME.txt') -Destination $releaseDirectory
    if ($IncludeManual) {
        Copy-Item -LiteralPath $manualPath -Destination (Join-Path $releaseDirectory 'Manual.pdf')
    }
    $manifest = [ordered]@{
        PublishedAtUtc = [DateTimeOffset]::UtcNow.ToString('O')
        RuntimeIdentifier = 'win-x64'
        SelfContained = $true
        SdkVersion = (& $sdkExecutable --version | Out-String).Trim()
        IncludesManual = [bool]$IncludeManual
        ValidationStatus = 'Pending portable acceptance checks'
    }
    $manifest | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $releaseDirectory 'publication.json') -Encoding utf8
    $checksums = Get-ChildItem -LiteralPath $releaseDirectory -Recurse -File | ForEach-Object {
        $relativePath = $_.FullName.Substring($releaseDirectory.Length + 1)
        (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash + '  ' + $relativePath
    }
    $checksums | Set-Content -LiteralPath (Join-Path $releaseDirectory 'SHA256SUMS.txt') -Encoding utf8
    Write-Output "Published folder: $releaseDirectory"
    Write-Output 'Publication is not acceptance verification. Complete the portable checklist before delivery.'
} finally {
    Pop-Location
}
