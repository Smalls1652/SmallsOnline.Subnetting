[CmdletBinding(DefaultParameterSetName = "AllVersions")]
param(
    [Parameter(Position = 0, ParameterSetName = "SpecificVersions")]
    [ValidateSet(
        "net5.0",
        "netstandard2.0"
    )]
    [string[]]$FrameworkVersion = "net5.0",
    [Parameter(Position = 0, ParameterSetName = "AllVersions")]
    [switch]$BuildAllFrameworkVersions
)

$frameworkVersions = $null
switch ($PSCmdlet.ParameterSetName) {
    "AllVersions" {
        $frameworkVersions = @(
            "net5.0",
            "netstandard2.0"
        )
        break
    }

    Default {
        $frameworkVersions = $FrameworkVersion
        break
    }
}

Write-Verbose "The following frameworks will be targetted for building: $($frameworkVersions -join ', ')"

$scriptRoot = $PSScriptRoot

Push-Location -Path $scriptRoot

try {
    # Builds the library.
    Write-Verbose "Running 'dotnet restore'."
    dotnet restore --nologo --verbosity quiet

    Write-Verbose "Running 'dotnet clean'."
    dotnet clean --nologo --verbosity quiet

    foreach ($frameworkVersionItem in $frameworkVersions) {
        Write-Verbose "Running 'dotnet publish --framework $($frameworkVersionItem)'."
        dotnet publish --nologo --verbosity minimal --framework $frameworkVersionItem
    }
}
finally {
    Pop-Location
}