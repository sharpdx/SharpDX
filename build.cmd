msbuild SharpDX.sln /t:Clean /m /p:Configuration=Release;Platform="Any CPU"

msbuild SharpDX.sln /restore /t:Build;Pack /p:Configuration=Release;Platform="Any CPU" /m