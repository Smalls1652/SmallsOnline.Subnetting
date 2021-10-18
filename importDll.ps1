[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet(
        "net5.0",
        "netstandard2.0"
    )]
    [string]$FrameworkVersion = "net5.0"
)

# Imports the compiled library into the current PowerShell console.
# Useful for testing the classes housed in the library.

$scriptRoot = $PSScriptRoot

$dllPaths = [System.Collections.Generic.List[string]]::new()

$frameworkBasedDll = Join-Path -Path $scriptRoot -ChildPath "src\SmallsOnline.Subnetting.Lib\bin\Debug\$($FrameworkVersion)\publish\SmallsOnline.Subnetting.Lib.dll"
$dllPaths.Add($frameworkBasedDll)

foreach ($dllItem in $dllPaths) {
    $dllItemObj = Get-Item -Path $dllItem
    Write-Verbose "Importing '$($dllItemObj.Name)' into console."

    Add-Type -Path $dllItem
}