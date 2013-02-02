@echo off
setlocal
set PATH=C:\Program Files (x86)\Git\bin\;%PATH%
REM call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\vc\vcvarsall.bat" x86
call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\vc\vcvarsall.bat" x86
set DXROOT=C:\Program Files\Sandcastle
%~dp0\External\nant\bin\nant -buildfile:SharpDXNant.build %*