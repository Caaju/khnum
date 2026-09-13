[CmdletBinding()]
param(
    [ValidateSet('verify', 'metadata', 'architecture', 'test')]
    [string]$Command = 'verify',
    [string]$Feature = 'feature-login'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

function Assert-Path {
    param([string]$RelativePath)

    $path = Join-Path $root $RelativePath
    if (-not (Test-Path $path)) {
        throw "Required path is missing: $RelativePath"
    }
}

function Invoke-MetadataCheck {
    Assert-Path '.khnum/constitution.md'
    Assert-Path '.khnum/quality-gates.md'
    Assert-Path '.khnum/feature-template.md'
    Assert-Path '.github/copilot-instructions.md'
    Assert-Path 'tools/khnum.ps1'
    Assert-Path "specs/$Feature/feature.yaml"

    $spec = Join-Path $root "specs/$Feature"
    if (-not (Test-Path (Join-Path $spec 'docs'))) {
        throw "Feature docs directory is missing: specs/$Feature/docs"
    }

    Write-Host "Metadata checks passed for $Feature."
}

function Invoke-ArchitectureCheck {
    $projects = Get-ChildItem -Path (Join-Path $root 'src') -Filter '*.csproj' -Recurse -ErrorAction SilentlyContinue
    if (-not $projects) {
        Write-Warning 'No .csproj files found under src; architecture check deferred until projects exist.'
        return
    }

    $forbidden = @{
        'Khnum.Domain' = @('Khnum.Api', 'Khnum.Infrastructure')
        'Khnum.Application' = @('Khnum.Api', 'Khnum.Infrastructure')
    }

    foreach ($project in $projects) {
        $projectName = [IO.Path]::GetFileNameWithoutExtension($project.Name)
        if (-not $forbidden.ContainsKey($projectName)) {
            continue
        }

        $content = Get-Content -Raw $project.FullName
        foreach ($dependency in $forbidden[$projectName]) {
            if ($content -match [regex]::Escape($dependency)) {
                throw "$projectName must not reference $dependency."
            }
        }
    }

    Write-Host 'Architecture checks passed.'
}

function Invoke-DotnetCheck {
    $solution = Join-Path $root 'khnum.slnx'
    $projects = Get-ChildItem -Path (Join-Path $root 'src') -Filter '*.csproj' -Recurse -ErrorAction SilentlyContinue
    if (-not $projects) {
        Write-Warning 'No .csproj files found under src; restore, build, and test deferred until the solution is scaffolded.'
        return
    }

    Push-Location $root
    try {
        dotnet restore $solution
        if ($LASTEXITCODE -ne 0) { throw 'dotnet restore failed.' }
        dotnet build $solution --configuration Release --no-restore
        if ($LASTEXITCODE -ne 0) { throw 'dotnet build failed.' }
        dotnet test $solution --configuration Release --no-build
        if ($LASTEXITCODE -ne 0) { throw 'dotnet test failed.' }
    }
    finally {
        Pop-Location
    }
}

Push-Location $root
try {
    switch ($Command) {
        'metadata' { Invoke-MetadataCheck }
        'architecture' { Invoke-ArchitectureCheck }
        'test' { Invoke-DotnetCheck }
        'verify' {
            Invoke-MetadataCheck
            Invoke-ArchitectureCheck
            Invoke-DotnetCheck
            Write-Host 'Khnum harness verification completed.'
        }
    }
}
finally {
    Pop-Location
}
