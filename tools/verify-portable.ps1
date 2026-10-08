param([string]$ReleaseDirectory)
$ErrorActionPreference = 'Stop'
$workspaceRoot = Split-Path $PSScriptRoot -Parent
if (-not $ReleaseDirectory) {
    $ReleaseDirectory = (Get-ChildItem -LiteralPath (Join-Path $workspaceRoot 'entrega') -Directory | Sort-Object Name -Descending | Select-Object -First 1).FullName
}
$release = (Resolve-Path -LiteralPath $ReleaseDirectory).Path
if (-not (Test-Path -LiteralPath (Join-Path $release 'Manual.pdf'))) { throw 'Final manual is missing.' }
foreach ($line in Get-Content -LiteralPath (Join-Path $release 'SHA256SUMS.txt')) {
    if ($line.Length -lt 67) { throw 'Invalid checksum record.' }
    $expected = $line.Substring(0, 64)
    $relative = $line.Substring(66)
    if ((Get-FileHash -LiteralPath (Join-Path $release $relative) -Algorithm SHA256).Hash -ne $expected) {
        throw "Package checksum mismatch: $relative"
    }
}
$copyRoot = Join-Path $workspaceRoot ('.tools/Verificación portable ' + [Guid]::NewGuid().ToString('N'))
Copy-Item -LiteralPath $release -Destination $copyRoot -Recurse
$previousRoot = $env:DOTNET_ROOT
$previousTrace = $env:COREHOST_TRACE
$previousTraceFile = $env:COREHOST_TRACEFILE
$previousPath = $env:PATH
function Invoke-Startup([string]$folder) {
    $env:COREHOST_TRACEFILE = Join-Path $folder 'host-trace.txt'
    $process = Start-Process -FilePath (Join-Path $folder 'Codice.Desktop.exe') -ArgumentList '--verify-startup' -WorkingDirectory $env:TEMP -WindowStyle Hidden -PassThru
    if (-not $process.WaitForExit(30000)) { $process.Kill(); throw 'Portable startup timed out.' }
    $process.Refresh()
    return $process.ExitCode
}
try {
    $env:DOTNET_ROOT = Join-Path $copyRoot 'no-shared-runtime'
    $env:PATH = $env:TEMP
    $env:COREHOST_TRACE = '1'
    $startup = Invoke-Startup $copyRoot
    if ($startup -ne 0) { throw 'Self-contained startup failed.' }
    $trace = @(Select-String -LiteralPath (Join-Path $copyRoot 'host-trace.txt') -Pattern 'Checking if CoreCLR path|Executing as a self-contained' | ForEach-Object Line)
    if ($trace.Count -lt 2) { throw 'Host trace did not confirm the bundled runtime.' }
    $data = Join-Path $copyRoot 'datos'
    $lock = [IO.FileStream]::new((Join-Path $data '.instance'), [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None)
    try { $secondInstance = Invoke-Startup $copyRoot } finally { $lock.Dispose() }
    if ($secondInstance -ne 3) { throw 'Second instance was not rejected.' }
    $absoluteData = [IO.Path]::GetFullPath($data)
    if (-not $absoluteData.StartsWith([IO.Path]::GetFullPath((Join-Path $workspaceRoot '.tools')) + [IO.Path]::DirectorySeparatorChar)) {
        throw 'Permission-test target is outside the disposable test directory.'
    }
    $originalAcl = Get-Acl -LiteralPath $data
    $testAcl = Get-Acl -LiteralPath $data
    $sid = [Security.Principal.WindowsIdentity]::GetCurrent().User
    $rule = [Security.AccessControl.FileSystemAccessRule]::new($sid, [Security.AccessControl.FileSystemRights]::Write,
        [Security.AccessControl.InheritanceFlags]'ContainerInherit,ObjectInherit', [Security.AccessControl.PropagationFlags]::None,
        [Security.AccessControl.AccessControlType]::Deny)
    $testAcl.AddAccessRule($rule)
    try {
        Set-Acl -LiteralPath $data -AclObject $testAcl
        $noWrite = Invoke-Startup $copyRoot
    } finally { Set-Acl -LiteralPath $data -AclObject $originalAcl }
    if ($noWrite -ne 3) { throw 'Write-denied folder was not rejected.' }
    $movedRoot = $copyRoot + ' trasladada'
    $absoluteCopy = [IO.Path]::GetFullPath($copyRoot)
    $absoluteMoved = [IO.Path]::GetFullPath($movedRoot)
    $allowedRoot = [IO.Path]::GetFullPath((Join-Path $workspaceRoot '.tools')) + [IO.Path]::DirectorySeparatorChar
    if (-not $absoluteCopy.StartsWith($allowedRoot) -or -not $absoluteMoved.StartsWith($allowedRoot)) { throw 'Move target is outside scratch directory.' }
    Move-Item -LiteralPath $copyRoot -Destination $movedRoot
    $moved = Invoke-Startup $movedRoot
    if ($moved -ne 0) { throw 'Startup after moving the closed folder failed.' }
    $evidence = [ordered]@{
        VerifiedAtUtc = [DateTimeOffset]::UtcNow.ToString('O')
        Release = Split-Path $release -Leaf
        StartupExitCode = $startup
        RuntimeLoadedFromPortableFolder = $true
        SharedRuntimeEnvironmentRemoved = $true
        PathIncludesSpacesAndAccents = $true
        DifferentWorkingDirectory = $true
        DatabaseCreatedBesideExecutable = Test-Path -LiteralPath (Join-Path $movedRoot 'datos/codice.db')
        SecondInstanceExitCode = $secondInstance
        WriteDeniedExitCode = $noWrite
        ClosedFolderMovedExitCode = $moved
        OriginalPackageDatabaseCount = @(Get-ChildItem -LiteralPath $release -Recurse -Filter '*.db').Count
        IncludesManual = $true
        PackageChecksumsVerified = $true
        CleanMachineVerified = $false
        VisualInterfaceVerified = $false
        ManualInteractionVerified = $false
        VerificationScope = 'Startup, runtime, locking, permissions, relocation and hashes only; interface review is separate.'
        PackageManifestSha256 = (Get-FileHash -LiteralPath (Join-Path $release 'SHA256SUMS.txt') -Algorithm SHA256).Hash
        HostTraceEvidence = $trace
    }
    $evidence | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $workspaceRoot 'docs/03-codice/test-evidence/current/portable-startup.json') -Encoding utf8
    Write-Output 'Portable checks passed: startup, bundled runtime, second instance, write denial, relocation and package hashes.'
    Write-Output 'Verified on this computer; a separate clean Windows machine remains unverified.'
} finally {
    $env:DOTNET_ROOT = $previousRoot
    $env:COREHOST_TRACE = $previousTrace
    $env:COREHOST_TRACEFILE = $previousTraceFile
    $env:PATH = $previousPath
}
