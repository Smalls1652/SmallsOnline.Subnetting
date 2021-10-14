# `SmallsOnline.Subnetting`

_**A C#-based subnet calculator**_

This repository is for a C# class library to perform subnetting of IPv4 address spaces. It is currently a work-in-progress.

## 🏗️ Building the library

### 🧰 Prerequisites

- [.NET 5.0 (or higher)](https://dotnet.microsoft.com/download)
- Any operating system that supports .NET
  - Windows
  - macOS
  - Linux

### 🔨 Build

Right now the compiled library is in the default folder:

`src/SmallsOnline.Subnetting.Lib/bin/Debug/net5.0/publish/`

The compiled library will be called: `SmallsOnline.Subnetting.Lib.dll`

#### Using the build script

1. Launch `powershell` (or `pwsh`) and navigate to the source code directory.
2. Run `.\buildLib.ps1`.

#### Manually building

1. Launch a terminal and navigate to the source code directory.
2. Run `dotnet publish`.
    - Be sure to run `dotnet clean` if you're rebuilding.
    - You do not need to run `dotnet restore` beforehand. `dotnet publish` implies that a restore will be done before it builds.

## 🏃🪠🚽 Testing

My current methods for testing the library is by importing the compiled DLL into a PowerShell console (I'm using PowerShell 7.1) and manually checking if certain things are working. I've made a PowerShell script called `importDll.ps1` at the root of the source directory to do all of that. From there I can test methods and creation of objects directly from PowerShell.

For example, after importing the DLL, I can do something like this:

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