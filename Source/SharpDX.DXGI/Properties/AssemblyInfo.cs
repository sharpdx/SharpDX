// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyProduct("SharpDX.DXGI")]
[assembly: AssemblyTitle("SharpDX.DXGI")]
[assembly: AssemblyDescription("Assembly providing DirectX - DXGI 1.0, 1.1 and 1.2 managed API")]

#if SHARPDX_SIGNED
[assembly: InternalsVisibleTo("SharpDX.Direct3D10,PublicKey=00240000048000009400000006020000002400005253413100040000010001000941e9108cca1ea51fbd8d3c1a834aab15d98ee7dbee30b31b2c5a8e399fe012d91e5b849ca36812696ee573283399eb487822154971496b06304fd02637ef8e9026904ab2632bd5c9f6f6e13c2bf449157ae79d4a12871e7743404f8f40dbd66dca7321fc507f6a25eb87f5c52d7f5e6145e4172092eecca14425714dc5609a")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D11,PublicKey=00240000048000009400000006020000002400005253413100040000010001000941e9108cca1ea51fbd8d3c1a834aab15d98ee7dbee30b31b2c5a8e399fe012d91e5b849ca36812696ee573283399eb487822154971496b06304fd02637ef8e9026904ab2632bd5c9f6f6e13c2bf449157ae79d4a12871e7743404f8f40dbd66dca7321fc507f6a25eb87f5c52d7f5e6145e4172092eecca14425714dc5609a")]
#else
// Make internals SharpDX visible to all SharpDX assemblies
[assembly: InternalsVisibleTo("SharpDX.Direct3D10")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D11")]
#endif

