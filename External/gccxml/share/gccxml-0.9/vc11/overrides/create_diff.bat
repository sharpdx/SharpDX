setlocal
SET PATH="C:\Program Files (x86)\Git\bin";%PATH%
pushd shared
for /f %%f in ('dir /b *.h') do git diff --no-prefix "C:\Code\Win8CP-Include\shared\%%~nxf"  %%f > %%~nf.diff
popd

pushd um
for /f %%f in ('dir /b *.h') do git diff --no-prefix "C:\Code\Win8CP-Include\um\%%~nxf"  %%f > %%~nf.diff
popd

for /f %%f in ('dir /b *.h') do git diff --no-prefix "C:\Program Files (x86)\Microsoft DirectX SDK (June 2010)\Include\%%~nxf"  %%f > %%~nf.diff
