# NetProjectParser
Simple, version-independent XML-based parser of csproj files

## Why another CSPROJ parser? Why not MSBuild
I needed a simple tool that will extract basic project info from any csproj file.
Using MSBuild related assemblies turned out to be troublesome due to incompatibilities between versions of msbuild <> csproj.

In short, I could not find any version of msbuild .dll that would handle very old .Net Framework csproj files, new .Net Framework csproj files and .Net Core packages.  
Got tired of random 'project parsing errors', decided to quickly put together own XML based solution.
