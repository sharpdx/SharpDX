@echo off
setlocal
set PATH=C:\Program Files (x86)\Git\bin\;%PATH%
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\vc\vcvarsall.bat" x86
msbuild /tv:4.0 /p:BuildSignedSharpDX=false /t:Build;BuildSamples /verbosity:quiet /clp:ErrorsOnly /fl /flp:logfile=BuildErrors.log;ErrorsOnly %* SharpDX.build
if NOT ERRORLEVEL 0 {
	pause
}
