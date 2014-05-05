@echo off
setlocal
RMDIR /Q /S .\Bin
IF EXIST BuildErrors.log del /F /Q BuildErrors.log
call MakeSharpDX.Assemblies.VS2013.cmd
call MakeSharpDX.Samples.VS2013.cmd
pause