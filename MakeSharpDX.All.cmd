@echo off
setlocal
RMDIR /Q /S .\Bin
IF EXIST BuildErrors.log del /F /Q BuildErrors.log
call MakeSharpDX.Desktop.Metro.VS2013.cmd
call MakeSharpDX.WP8.VS2012.cmd
call MakeSharpDX.Samples.VS2013.cmd
pause