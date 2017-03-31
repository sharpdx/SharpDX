msbuild SharpDX-Tools.sln /t:Clean;Build /m /p:Configuration=Release;Platform="Any CPU"
msbuild SharpDX-Sources.sln /t:Clean /m /p:Configuration=Release;Platform="Any CPU"
dotnet restore SharpDX-Sources.sln
msbuild SharpDX-Sources.sln /t:Build /m /p:Configuration=Release;Platform="Any CPU"
msbuild SharpDX-Sources.sln /t:Pack /p:NuGetBuildTasksPackTargets="workaround" /m