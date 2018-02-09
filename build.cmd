msbuild SharpDX.sln /t:Clean /m /p:Configuration=Release;Platform="Any CPU"

REM Build SharpDX.csproj first not in parallel so we don't have to worry about file locking issues
REM with shared files between the 4 different generators.
msbuild Source/SharpDX/SharpDX.csproj /restore /t:Build /p:Configuration=Release

msbuild SharpDX.sln /restore /t:Build;Pack /p:Configuration=Release;Platform="Any CPU" /m