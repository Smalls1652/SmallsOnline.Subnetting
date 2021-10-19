[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet(
        "Debug",
        "Release"
    )]
    [string]$ConfigurationName = "Debug",
    [Parameter(Position = 1)]
    [switch]$CreateBuildArtifact,
    [Parameter(Position = 2)]
    [switch]$SkipDotnetBuild
)

$writeInfoSplat = @{
    "InformationAction" = "Continue";
}

$scriptRoot = $PSScriptRoot

# Generate the necessary paths for building.
$buildDirPath = Join-Path -Path $scriptRoot -ChildPath "build" # The main build directory path.
$compiledModuleDirPath = Join-Path -Path $buildDirPath -ChildPath "SmallsOnline.Subnetting.Pwsh" # The built compiled module directory path.
$pwshSlnFilePath = Join-Path -Path $scriptRoot -ChildPath "SmallsOnline.Subnetting.Pwsh.sln" # The solution file for the PowerShell binary module project.
$coreModuleManifestPath = Join-Path -Path $scriptRoot -ChildPath "src\SmallsOnline.Subnetting.Pwsh.ModuleManifest\SmallsOnline.Subnetting.Pwsh.psd1" # The module manifest file.
$compiledBinaryFiles = @(
    (Join-Path -Path $scriptRoot -ChildPath "src\SmallsOnline.Subnetting.Pwsh\bin\Release\netstandard2.0\publish\SmallsOnline.Subnetting.Lib.dll"),
    (Join-Path -Path $scriptRoot -ChildPath "src\SmallsOnline.Subnetting.Pwsh\bin\Release\netstandard2.0\publish\SmallsOnline.Subnetting.Pwsh.dll")
)

if ($CreateBuildArtifact) {
    $artifactsPath = Join-Path -Path $buildDirPath -ChildPath "artifacts"
    $moduleArtifactsPath = Join-Path -Path $artifactsPath -ChildPath "pwsh-module"

    $datetimeString = [System.DateTime]::Now.ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss")
    $moduleArtifactFileName = Join-Path -Path $moduleArtifactsPath -ChildPath "SmallsOnline.Subnetting.Pwsh_build_$($datetimeString).zip"
}

# Check to see if the build directory needs to be created.
Write-Information @writeInfoSplat -MessageData "- Checking for build directory."
if ((Test-Path -Path $buildDirPath) -eq $false) {
    Write-Warning "Build directory does not exist already."
    $null = New-Item -Path $buildDirPath -ItemType "Directory"
}

# If necessary, delete an existing compiled module directory and then create a clean directory.
Write-Information @writeInfoSplat -MessageData "- Checking for compiled module directory."
if (Test-Path -Path $compiledModuleDirPath) {
    Write-Warning "Compiled module directory already exists. Removing."
    Remove-Item -Path $compiledModuleDirPath -Recurse -Force
}
Write-Information @writeInfoSplat -MessageData "- Creating a clean compiled module directory."
$null = New-Item -Path $compiledModuleDirPath -ItemType "Directory"

if ($CreateBuildArtifact) {
    Write-Information @writeInfoSplat -MessageData "- Checking for build artifacts directory."
    if ((Test-Path -Path $artifactsPath) -eq $false) {
        Write-Information @writeInfoSplat -MessageData "`t- Creating build artifacts directory."
        $null = New-Item -Path $artifactsPath -ItemType "Directory"
    }

    Write-Information @writeInfoSplat -MessageData "- Checking for PowerShell module artifacts directory."
    if ((Test-Path -Path $moduleArtifactsPath) -eq $false) {
        Write-Information @writeInfoSplat -MessageData "`t- Creating PowerShell module artifacts directory."
        $null = New-Item -Path $moduleArtifactsPath -ItemType "Directory"
    }
}

if ($SkipDotnetBuild -eq $false) {
# Compile the C# projects.
Write-Information @writeInfoSplat -MessageData "- Compiling C# projects."
Write-Information @writeInfoSplat -MessageData "`t- Running 'dotnet restore'."
dotnet restore "$($pwshSlnFilePath)" --nologo --verbosity "quiet"

Write-Information @writeInfoSplat -MessageData "`t- Running 'dotnet clean'."
dotnet clean "$($pwshSlnFilePath)" --nologo --verbosity "quiet"

Write-Information @writeInfoSplat -MessageData "`t- Running 'dotnet publish'."
dotnet publish "$($pwshSlnFilePath)" --framework "netstandard2.0" --configuration "Release" --no-restore --nologo --verbosity "quiet"
}

# Copy the required DLL files to the compiled module directory.
Write-Information @writeInfoSplat -MessageData "- Populating compiled module directory."
foreach ($dllFile in $compiledBinaryFiles) {
    $dllFileItem = Get-Item -Path $dllFile
    Write-Information @writeInfoSplat -MessageData "`t- Copying '$($dllFileItem.Name)'."
    Copy-Item -Path $dllFile -Destination $compiledModuleDirPath
}

# Copy the module manifest to the compiled module directory.
$coreModuleManifestFileItem = Get-Item -Path $coreModuleManifestPath
Write-Information @writeInfoSplat -MessageData "`t- Copying '$($coreModuleManifestFileItem.Name)'."
Copy-Item -Path $coreModuleManifestPath -Destination $compiledModuleDirPath

if ($CreateBuildArtifact) {
    Write-Information @writeInfoSplat -MessageData "- Creating build artifact for PowerShell module."
    Compress-Archive -Path $compiledModuleDirPath -DestinationPath $moduleArtifactFileName -CompressionLevel "Optimal"
}

Write-Information @writeInfoSplat -MessageData "Build complete.`n"
Write-Information @writeInfoSplat -MessageData "Compiled module can be found at '$($compiledModuleDirPath)'."
if ($CreateBuildArtifact) {
    Write-Information @writeInfoSplat -MessageData "Compiled module build artifact can be found at '$($moduleArtifactFileName)'."
}