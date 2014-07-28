REM @ECHO OFF
REM Run the generator from the current configuration
setlocal
pushd "%~dp0\Bin\%1"
ECHO "%~dp0msdndoc.zip"
xcopy /D /Y "%~dp0MSDNDoc.zip" .
SharpGen.exe --doc --gccxml ..\..\..\..\..\External\gccxml\bin\gccxml.exe ..\..\..\..\Mapping.xml
set LOCALERROR = %ERRORLEVEL%
xcopy /D /Y MSDNDoc.zip %~dp0
exit /B %LOCALERROR%