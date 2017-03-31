REM @ECHO OFF
REM Run the generator from the current configuration
setlocal
pushd "%~dp0\Bin\%1"
ECHO "%~dp0msdndoc.zip"
xcopy /D /Y "%~dp0MSDNDoc.zip" .

REM Find a VS 2017 installation with the C++ toolset installed.
set InstallDir=
for /f "usebackq tokens=*" %%i in (`..\..\..\..\..\External\vswhere\vswhere -latest -products * -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 Microsoft.VisualStudio.Component.Windows10SDK.14393 -property installationPath`) do (
  set InstallDir=%%i
)
set ToolsVersion=
for /F %%A in ('type "%InstallDir%\VC\Auxiliary\Build\Microsoft.VCToolsVersion.default.txt"') do (
    set "ToolsVersion=%%A"
)
set VCToolsInstallDir=
if exist "%InstallDir%\VC\Tools\MSVC\%ToolsVersion%\" (
    set "VCToolsInstallDir=%InstallDir%\VC\Tools\MSVC\%ToolsVersion%\"
)
SharpGen.exe --doc --castxml ..\..\..\..\..\External\castxml\bin\castxml.exe ..\..\..\..\Mapping.xml --apptype %2 --vctools "%VCToolsInstallDir%
set LOCALERROR = %ERRORLEVEL%
xcopy /D /Y MSDNDoc.zip %~dp0
exit /B %LOCALERROR%