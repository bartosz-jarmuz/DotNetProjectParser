# DotNetProjectParser ![Nuget](https://img.shields.io/nuget/dt/DotNetProjectParser?label=Nuget%20Downloads)
Simple, version-independent XML-based parser of csproj files

## Why another CSPROJ parser?
I needed a simple tool that will extract basic project info from any csproj file, regardless of target framework (Core, Standard, Framework) and version of Visual Studio that spawned them.

## Why not MSBuild?
Using MSBuild related assemblies turned out to be troublesome due to incompatibilities between versions of msbuild <> csproj.

I could not find any version of msbuild .dll that would handle very old .Net Framework csproj files, new .Net Framework csproj files and .Net Core packages.  

Got tired of random 'project parsing errors', decided to quickly put together own XML based solution.

## NuGet
https://www.nuget.org/packages/DotNetProjectParser/  
```
PM > Install-Package DotNetProjectParser -Version 1.0.0
```

## Free resources attribution
Icon by https://www.flaticon.com/authors/flat-icons from https://www.flaticon.com/
