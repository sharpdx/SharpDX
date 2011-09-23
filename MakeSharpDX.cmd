@echo off
setlocal
call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\vc\vcvarsall.bat" x86
set DXROOT=C:\Program Files\Sandcastle
%~dp0\External\nant\bin\nant %*