@echo off
setlocal
set PATH=C:\Program Files (x86)\Git\bin\;%PATH%
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\vc\vcvarsall.bat" x86
msbuild /tv:4.0 /p:BuildSignedSharpDX=false /t:DesktopNet20;DesktopNet40;MetroWin8;DesktopDx112 /verbosity:quiet /clp:ErrorsOnly /fl /flp:logfile=BuildErrors.log;ErrorsOnly;Append=true %* SharpDX.build
