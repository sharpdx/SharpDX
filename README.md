# SharpDX

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/sharpdx/SharpDX/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/21v2akj26ytuyml6?svg=true)](https://ci.appveyor.com/project/xoofx/sharpdx) 
[![NuGet](https://img.shields.io/nuget/v/SharpDX.svg)](https://www.nuget.org/packages?q=Tags%3A%22SharpDX%22)

Official web site: [sharpdx.org](http://sharpdx.org)

SharpDX is an open-source project delivering the **full DirectX API for .Net on all Windows platforms**, allowing the development of high performance game, 2D and 3D graphics rendering as well as realtime sound application.

## Download

All SharpDX packages are available as NuGet packages: [![NuGet](https://img.shields.io/nuget/v/SharpDX.svg)](https://www.nuget.org/packages?q=Tags%3A%22SharpDX%22)

Nightly packages can be download by adding the NuGet feed "https://ci.appveyor.com/nuget/sharpdx" to your `NuGet.config` file:

```xml
 <configuration>
   <packageSources>
     <!-- ... -->
     <add key="appveyor sharpdx" value="https://ci.appveyor.com/nuget/sharpdx" />
     <!-- ... -->
   </packageSources>
</configuration>     
```

## Wiki Documentation

You can find more documentation on the [Wiki](http://sharpdx.org/wiki)

## Build

In order to compile SharpDX, you need to install **Visual Studio 2017 or newer** with the following workloads and components:

- [x] Visual C++ Toolset Component
- [x] Windows 10 SDK (10.0.14393.0) Component
- [x] Windows 10 - 1809 SDK (10.0.17763.0) Component
- [x] C# Development Workload
- [x] .NET Core Cross Platform Development Workload

## Samples

A collection of [Samples](https://github.com/sharpdx/SharpDX-Samples) using SharpDX exists as a separate github project.
