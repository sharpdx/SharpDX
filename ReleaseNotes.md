## SharpDX 3.0.0 (alpha01)

This is the first alpha release of the 3.0.0 branch bringing some major new features and changes:

- Add support for **Direct3D12** to get the best of your graphics card (only compatible with Windows 10 10041)
- Thanks to the work from the [diet branch](https://github.com/sharpdx/SharpDX/issues/398), SharpDX main assembly is a bit more lightweight. 
- We have simplified the distribution and packaging by supporting only 2 platforms and .NET versions:
	- **Desktop** with `.NET 4.5`
	- **StoreApp** with `PCL .NET 4.5` valid for Windows 8.1+ and Windows Phone 8.1+ development
- All Mathematics (`Vector3`, `Vector4`...) have been moved to a dedicated assembly. The reason behind this change is to put SharpDX on a diet, where higher level API (like Paradox, MonoGame...) are no longer forced to include this API. In order to support this, all SharpDX assemblies are now using interop types (`RawVector3`, `RawVector4` from         `SharpDX.Mathematics.Interop` namespace) 
- A new assembly `SharpDX.Desktop` that contains `RenderForm`, `RenderLoop` previously in `SharpDX` assembly

Assemblies are both available from a **zip** and from **NuGet**. 
Note that distribution from NuGet should be now more stable and usable than previous versions due to the diet work.

We have also clean-up the API: 
- The DirectX June 2010 SDK is no longer supported. For example, methods like DX11 are no longer supported (e.g. `Texture.FromFile`...)
- `Direct3D10` API has been removed
- The Toolkit is no longer supported and distributed
- `Direct3D9`, `DirectSound`, `DirectInput`, `RawInput` have been frozen so that the code generated from SharpGen codegen from C++ is no longer running. It means that these APIs will no longer receive major changes and should be considered as deprecated.

Projects that are still requiring to use `.NET 2.0` or `.NET 4.0`, or API like `Direct3D10` will have to continue using `SharpDX 2.6.3`.

### Building from the sources

In order to compile SharpDX from the sources, It requires to install the [Windows SDK for Windows 10](https://dev.windows.com/en-us/featured/hardware/windows-10-hardware-preview-tools)

### How to use the samples

The [samples](https://github.com/sharpdx/SharpDX-Samples) are now part of a submodule of the SharpDX repository. In order to use the sample, you can download the SharpDX zip binaries and unzip the `Bin` directory so that it is at the same level than the `Samples` directory:
- `Bin\DesktopApp\SharpDX.dll`...etc.
- `Samples\Desktop\...`... etc.

Some scripts will be added later to simplify the setup of the samples.

### About Direct3D12

This release includes a preview of the `Direct3D12` API along a very basic sample `HelloWorld` available from the [SharpDX Samples](https://github.com/sharpdx/SharpDX-Samples) repository.

Compare to previous Direct3D API in SharpDX, the API is now closer to the C++ API (e.g keep methods like `Device.CreateXXX(...)` instead of `new XXX(device, ...)`)

In order to use this API, make sure that you have **Windows 10 10041 and the latest graphics drivers installed**

Due to NDA, I'm not yet allowed to publish ported samples from Direct3D12 Early Access Program, sorry for that!
 
### Known issues

- The `Direct3D12` API is still a work in progress. Some structures are not yet usable or not correctly marshalled. Most of the work done so far was to get the `HelloTriangle` working, which is using a very small part of the `Direct3D12` API.
- `XInput` 1.4 and `XAudio2` 2.8 are only generated for the Windows 8 versions (for both desktop and stores). We will restore previous versions in a later alpha release (`XInput 9.1.0`, `XAudio 2.7`)  

### Feedback

As this is a major release, if you have any questions or remarks concerning this release, join the discussion on github: [Feedback from 3.0.0 alpha01](https://github.com/sharpdx/SharpDX/issues/567)

Best,
xoofx