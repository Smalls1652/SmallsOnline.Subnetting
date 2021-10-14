[CmdletBinding()]
param()

# Builds the library.

dotnet restore
dotnet clean
dotnet publish --framework net5.0
dotnet publish --framework netstandard2.1
dotnet publish --framework netframework4.5