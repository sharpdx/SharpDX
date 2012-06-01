setlocal
SET PATH="C:\Program Files (x86)\Git\bin";%PATH%
pushd shared
for /f %%f in ('dir /b *.diff') do patch -p0 -o  %%~nf.h "C:\Program Files (x86)\Windows Kits\8.0\Include\shared\%%~nf.h" %%~nxf 
popd

pushd um
for /f %%f in ('dir /b *.diff') do patch -p0 -o  %%~nf.h "C:\Program Files (x86)\Windows Kits\8.0\Include\um\%%~nf.h" %%~nxf 
popd

for /f %%f in ('dir /b *.diff') do patch -p0 -o  %%~nf.h "C:\Program Files (x86)\Microsoft DirectX SDK (June 2010)\Include\%%~nf.h" %%~nxf

