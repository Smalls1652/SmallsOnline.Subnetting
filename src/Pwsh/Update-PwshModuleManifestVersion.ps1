[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string]$Version,
    [Parameter(Position = 1)]
    [string]$PrereleaseVersion
)

$outputManifestPath = Join-Path -Path $PSScriptRoot -ChildPath "SmallsOnline.Subnetting.Pwsh.psd1"

$manifest = Get-Content -Path $outputManifestPath -Raw

$manifest = $manifest.Replace("{{ VERSION }}", $Version)

if ($null -ne $PrereleaseVersion -and [string]::IsNullOrEmpty($PrereleaseVersion) -eq $false) {
    $manifest = $manifest.Replace("{{ PRERELEASE_VERSION }}", $PrereleaseVersion)
}

$manifest | Set-Content -Path $outputManifestPath -Force
