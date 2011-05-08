# SharpDX
SharpDX is an open-source DirectX and Windows Multimedia .Net Managed API. The *API is generated automatically from DirectX SDK headers*, with *AnyCpu target*, meaning that you can run your application on x86 and x64 platform, without recompiling your project or installing assemblies into the GAC. SharpDX is also [http://code4k.blogspot.com/2011/03/benchmarking-cnet-direct3d-11-apis-vs.html the fastest Managed-DirectX implementation].

### News
* 8 May 2011, moving to github. First alpha of v2, new code generator.
* 8 April 2011, Working on SharpDX v2 with lots of new APIs, enhancements and more... unfortunately, current SVN dev version cannot be compiled. This will be fix in a couple of weeks, stay tuned.
* *17 February 2011, SharpDX 1.3 is released*. *Full support for mono 2.10* (Mono 2.10 includes a bugfix necessary for SharpDX to work with it). MessagePump renamed to RenderLoop and some other small issues. see [http://code.google.com/p/sharpdx/source/browse/trunk/ChangeLog.txt?r=109 changelog]
* 5 February 2011, SharpDX 1.2.2 is released. Small maintenance release to bugfix an issue with MessagePump.Run that was delaying to to show a form and some other smaller issues. see [http://code.google.com/p/sharpdx/source/browse/trunk/ChangeLog.txt?r=104 changelog]
* 27 January 2011, SharpDX 1.2.1 is released. Small maintenance release to bugfix a severe issue on 32bit machine introduce by new interop system
* 26 January 2011, SharpDX 1.2 is released. This release provides:
 * Support for Mono Runtime (Still Direct3D samples are not working due to a bug in Application.Idle and Winform on Mono, help is needed!)
 * DirectWrite is now supporting a broader range of callback samples, a genuine SharpDX exclusivity, as you cannot do this with any other legacy DirectX .Net API! (thanks to Fadi!).
 * Improved native interop system, safer, faster, MonoLinker/Obfuscator friendly 
 * Various small bugfixes on names and enhancement, see [http://code.google.com/p/sharpdx/source/browse/trunk/ChangeLog.txt?r=94 changelog].
* 19 December 2010, SharpDX 1.1 is released. This release provides full support for all .Net Frameworks 2.0, 3.0, 3.5, 4.0. Various small bugfixes on names and enhancement, see [http://code.google.com/p/sharpdx/source/browse/trunk/ChangeLog.txt?r=65 changelog].
* 30 November 2010, SharpDX 1.0 final is released. Full support for Direct3D10, Direct3D10.1, Direct3D11, Direct2D1, DirectWrite, D3DCompiler, DXGI 1.0, DXGI 1.1, DirectSound, XAudio2, XAPO.

### Features
The key features and benefits of this API are:
* *API is generated from DirectX SDK headers* : meaning *a complete and reliable API* and *an easy support for future API*.
* *Full support for the following DirectX API*:
 * Direct3D10
 * Direct3D10.1
 * Direct3D11
 * Direct2D1 (including custom rendering, tessellation callbacks)
 * DirectWrite (including custom client callbacks)
 * D3DCompiler
 * DXGI
 * DXGI 1.1
 * DirectSound
 * XAudio2
 * XAPO
 * An integrated math API directly ported from SlimMath
* *Managed platform independent .NET API* : assemblies are compiled with AnyCpu target. You can run your code on a x64 or a x86 machine with the same assemblies, without recompiling your project.
* *Lightweight [AssemblyDlls individual assemblies]*  : a core assembly - SharpDX - containing common classes and an assembly for each subgroup API (Direct3D10, Direct3D11, DXGI, D3DCompiler...etc.). Assemblies are also lightweight.
* *Fast Interop* : the framework is using a genuine way to avoid any C++/CLI while still achieving better performance than existing managed API. Check this [http://code4k.blogspot.com/2011/03/benchmarking-cnet-direct3d-11-apis-vs.html benchmark].
* *API naming convention mostly compatible with SlimDX API*.
* *Raw DirectX object life management* : No overhead of ObjectTable or RCW mechanism, the API is using direct native management with classic COM method "Release".
* *Easily mergeable / obfuscatable* : If you need to obfuscate SharpDX assemblies, they are easily obfusctable due to the fact the framework is not using any mixed assemblies. You can also merge SharpDX assemblies into a single exe using with tool like [http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx ILMerge].

An *optional assembly SharDX.Error* is delivered and can be added to your project in order to have an explicit error messages when there is a DirectX functions returning an error code. This assembly can be used in development but is not mandatory.

### Documentation
* Getting started (Todo)
* [AssemblyDlls Assembly dependencies] : Explain SharpDX assembly dependencies between SharpDX component as well as identifying native dlls dependencies.
* [MigrateFrom Migrate from SlimDX to SharpDX], explains the process to migrate to SharpDX.
* A [SamplesGallery samples gallery] including screenshots, a brief description and links to the source code
 
 ### Samples

Checkout the [SamplesGallery Samples Gallery]

The samples archive contains several small samples for various API, including :
* Direct3D10
 * MiniTri Sample (port from SlimDX)
 * Font Sample
* Direct3D11
 * MiniTri Sample
* Direct2D1 
 * MiniRect
 * Tessellation
 * Text with Direct2D and DirectWrite
* DirectWrite, with several Samples back-ported from  [http://msdn.microsoft.com/en-us/library/dd756563%28v=VS.85%29.aspx Windows 7 SDK DirectWrite samples]
 * SimpleHelloWorld
 * FontEnumeration
 * CustomFont loading using DirectWrite callback API
 * TextDialog 
* DirectSound
 * Play a sound
* XAudio2/XAPO 
 * Play a sound with reverb
 * Play a sound with a custom audio processor (XAPO)

### Requirements
* *Operating system: Windows XP, Vista, 7*
* *.Net framework 2.0, 3.0, 3.5, 4.0* are supported.
* The current code is based on *DirecX SDK June 2010*. You need to have at least the latest [http://www.microsoft.com/downloads/en/details.aspx?displaylang=en&FamilyID=3b170b25-abab-4bc3-ae91-50ceb6d8fa8d DirectX runtime] to develop with SharpDX. If you need to debug DirectX calls, you will need also to install the [http://www.microsoft.com/downloads/en/details.aspx?displaylang=en&FamilyID=3021d52b-514e-41d3-ad02-438a3ba730ba DirectX SDK].

### Future Plans
* Support for XACT3, X3DAudio, XInput, DirectInput is expected to be release for next version of SharpDX.
* Support for Direct3D 9
* Framework of higher level, similar to XNA but built on top of DirectX 11 (but with support for Direct3D9, Direct3D10 hardware)