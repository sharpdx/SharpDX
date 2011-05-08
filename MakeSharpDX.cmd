setlocal
call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\vc\vcvarsall.bat" x86
set DXROOT=C:\Program Files\Sandcastle
set PATH=..\nant\bin;%PATH%
nant %1 %2 %3 %4 %5 %6
pause