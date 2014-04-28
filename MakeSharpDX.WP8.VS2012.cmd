@echo off
setlocal
set PATH=C:\Program Files (x86)\Git\bin\;%PATH%
call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\vc\vcvarsall.bat" x86
echo BUILD WP8-x86, WP8-ARM >> BuildErrors.log
msbuild /tv:4.0 /p:BuildSignedSharpDX=false /t:WP8-x86;WP8-ARM /verbosity:quiet /clp:ErrorsOnly /fl /flp:logfile=BuildErrors.log;Append=true %* SharpDX.build
