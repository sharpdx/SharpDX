@echo off
setlocal
set PATH="C:\Program Files (x86)\Windows Kits\8.1\bin\x86";%PATH%
fxc.exe %*
endlocal