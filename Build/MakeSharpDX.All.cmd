@echo off
setlocal
RMDIR /Q /S %~dp0\..\Bin
IF EXIST BuildErrors.log del /F /Q BuildErrors.log
call MakeSharpDX.Assemblies.VS2013.cmd
pause