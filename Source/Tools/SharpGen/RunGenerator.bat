REM @ECHO OFF
REM Run the generator from the current configuration
pushd "%~dp0\Bin\%1"
SharpGen.exe -d DocProviderFromMsdn.exe --gccxml ..\..\..\..\..\External\gccxml\bin\gccxml.exe ..\..\..\..\Mapping.xml
popd
exit %ERRORLEVEL%