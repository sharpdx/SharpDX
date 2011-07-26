// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

[assembly: AssemblyProduct("SharpDX")]
[assembly: AssemblyTitle("SharpDX")]
[assembly: AssemblyDescription("Core assembly for all SharpDX assemblies.")]

// Make internals SharpDX visible to all SharpDX assemblies
[assembly: InternalsVisibleTo("SharpDX.DXGI")]
[assembly: InternalsVisibleTo("SharpDX.D3DCompiler")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D9")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D10")]
// [assembly: InternalsVisibleTo("SharpDX.Direct3D11")]
// [assembly: InternalsVisibleTo("SharpDX.Direct3D11.Effects")]
[assembly: InternalsVisibleTo("SharpDX.Direct2D1")]
[assembly: InternalsVisibleTo("SharpDX.DirectInput")]
[assembly: InternalsVisibleTo("SharpDX.DirectSound")]
[assembly: InternalsVisibleTo("SharpDX.XAPO")]
[assembly: InternalsVisibleTo("SharpDX.XAudio2")]
[assembly: InternalsVisibleTo("SharpDX.XACT3")]

[assembly: InternalsVisibleTo("SharpDX.Framework.Graphics")]

