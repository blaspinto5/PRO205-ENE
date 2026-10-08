param([switch]$BuildOnly)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$sdkExecutable = & (Join-Path $PSScriptRoot 'get-sdk.ps1')
Push-Location $projectRoot
try {
    if ($BuildOnly) {
        & $sdkExecutable build Codice.sln --configuration Release
    } else {
        & $sdkExecutable build Codice.sln --configuration Release
        if ($LASTEXITCODE -ne 0) { throw 'Build failed before testing.' }
        foreach ($suite in @('Unit', 'Integration', 'Desktop')) {
            $testProject = "tests/Codice.${suite}Tests/Codice.${suite}Tests.csproj"
            & $sdkExecutable test $testProject --configuration Release --no-build --logger 'console;verbosity=normal' --logger "trx;LogFileName=$($suite.ToLowerInvariant())-tests.trx" --results-directory 'docs/03-codice/test-evidence/current'
            if ($LASTEXITCODE -ne 0) { throw "$suite tests failed with exit code $LASTEXITCODE." }
        }
    }
    if ($LASTEXITCODE -ne 0) { throw "Verification failed with exit code $LASTEXITCODE." }
} finally {
    Pop-Location
}
