[CmdletBinding()]
param()

# Imports the compiled library into the current PowerShell console.
# Useful for testing the classes housed in the library.

$scriptRoot = $PSScriptRoot

$dllPaths = [System.Collections.Generic.List[string]]::new()
$dllPaths.Add((Join-Path -Path $scriptRoot -ChildPath "src\SmallsOnline.Subnetting.Lib\bin\Debug\net5.0\publish\SmallsOnline.Subnetting.Lib.dll"))

foreach ($dllItem in $dllPaths) {
    $dllItemObj = Get-Item -Path $dllItem
    Write-Verbose "Importing '$($dllItemObj.Name)' into console."

    $null = [System.Reflection.Assembly]::LoadFrom($dllItem)
}