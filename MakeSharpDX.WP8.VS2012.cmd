@echo off
setlocal
set PATH=C:\Program Files (x86)\Git\bin\;%PATH%
call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\vc\vcvarsall.bat" x86
msbuild /tv:4.0 /p:BuildSignedSharpDX=false /t:WP8x86;WP8ARM /verbosity:quiet /clp:ErrorsOnly /fl /flp:logfile=BuildErrors.log;ErrorsOnly;Append=true %* SharpDX.build
