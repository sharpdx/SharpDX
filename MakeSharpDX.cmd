@echo off
setlocal
set PATH=C:\Program Files (x86)\Git\bin\;%PATH%
REM call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\vc\vcvarsall.bat" x86
call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\vc\vcvarsall.bat" x86
msbuild /tv:4.0 %* SharpDX.build
