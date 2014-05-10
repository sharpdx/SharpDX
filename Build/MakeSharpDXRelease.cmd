@echo off
setlocal
call MakeSharpDX.cmd /verbosity:minimal /t:Build;PackageRelease
