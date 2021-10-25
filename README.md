# `SmallsOnline.Subnetting`

_**A C#-based subnet calculator**_

This repository is for a C# class library to perform subnetting of IPv4 address spaces. It is currently a work-in-progress.

## Related projects

| Name | Link |
| ---- | ---- |
| **Blazor Web App** | [@Smalls1652/SmallsOnline.Subnetting.BlazorWasm](https://github.com/Smalls1652/SmallsOnline.Subnetting.BlazorWasm) |

## 🧱 Build Status

This is the current status of the builds for the two core branches when new commits are pushed:

| Branch | Status |
| ------ | ------ |
| **main** | [![Build](https://github.com/Smalls1652/SmallsOnline.Subnetting/actions/workflows/build.yml/badge.svg?branch=main&event=push)](https://github.com/Smalls1652/SmallsOnline.Subnetting/actions/workflows/build.yml) |
| **dev** | [![Build](https://github.com/Smalls1652/SmallsOnline.Subnetting/actions/workflows/build.yml/badge.svg?branch=dev&event=push)](https://github.com/Smalls1652/SmallsOnline.Subnetting/actions/workflows/build.yml) |

## 🏗️ Building the library

### 🧰 Prerequisites

- [.NET 5.0 (or higher)](https://dotnet.microsoft.com/download)
- Any operating system that supports .NET
  - Windows
  - macOS
  - Linux

### 🔨 Build

Right now the compiled library is in the default folders for their respective framework:

- `src/SmallsOnline.Subnetting.Lib/bin/Debug/net5.0/publish/`
- `src/SmallsOnline.Subnetting.Lib/bin/Debug/netstandard2.0/publish/`

The compiled library will be called: `SmallsOnline.Subnetting.Lib.dll`

#### Using the build script

1. Launch `powershell` (or `pwsh`) and navigate to the source code directory.
2. Run `.\buildLib.ps1`.

#### Manually building

1. Launch a terminal and navigate to the source code directory.
2. Run `dotnet restore`
3. Run `dotnet publish --framework [framework-version]`.
    - Replace `[framework-version]` with one of the following:
      - `net5.0` for .NET 5.0
      - `netstandard2.0` for .NET Standard 2.0 (Supports .NET Framework 4.6.1 and higher)
    - Be sure to run `dotnet clean` if you're rebuilding.

## 🏃🪠🚽 Testing

My current methods for testing the library is by importing the compiled DLL into a PowerShell console (I'm using PowerShell 7.1) and manually checking if certain things are working. I've made a PowerShell script called `importDll.ps1` at the root of the source directory to do all of that. From there I can test methods and creation of objects directly from PowerShell.

Depending on the version of PowerShell you're running, you'll need to run it with one of these parameters:

```powershell
# For PowerShell 7.1 and higher
PS > .\importDll.ps1

# For Windows PowerShell 5.1 and higher
PS > .\importDll.ps1 -FrameworkVersion "netstandard2.0"
```

After importing the DLL, you can do something like this:

```powershell
PS > [SmallsOnline.Subnetting.Lib.Core.Calculator]::GetMaxAddresses(26) # Get the max addresses for a /26 network
64

PS > [SmallsOnline.Subnetting.Lib.Core.Calculator]::GetSubnetMask(21) # Get the subnet mask of a /21 network

AddressFamily      : InterNetwork
ScopeId            :
IsIPv6Multicast    : False
IsIPv6LinkLocal    : False
IsIPv6SiteLocal    : False
IsIPv6Teredo       : False
IsIPv4MappedToIPv6 : False
Address            : 16318463
IPAddressToString  : 255.255.248.0

PS > [SmallsOnline.Subnetting.Lib.Core.Calculator]::GetWildCardBytes(21) # Get the wildcard mask of a /21 network
0
0
7
255

PS > [SmallsOnline.Subnetting.Lib.Core.Calculator]::GetWildCardBytes(21) -join "." # Get the wildcard mask of a /21 network and make it a readable string
0.0.7.255
```