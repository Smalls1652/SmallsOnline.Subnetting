[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string]$Version
)

$outputManifestPath = Join-Path -Path $PSScriptRoot -ChildPath "SmallsOnline.Subnetting.Pwsh.psd1"

$manifest = Get-Content -Path $outputManifestPath -Raw

$manifest = $manifest.Replace("{{ VERSION }}", $Version)

$manifest | Set-Content -Path $outputManifestPath -Force
