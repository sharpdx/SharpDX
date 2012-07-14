// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
[assembly: InternalsVisibleTo("SharpDX.Direct3D10,PublicKey=0024000004800000940000000602000000240000525341310004000001000100375a9dd7e968f90d45ff9bd8257ffdc69997de6590c3e74aaab2de0b791f84bcd367dcbae699bbb23e3df7a888b3c8d086159f3646e7e72224a35f2423b5fca176120cb84a8648ff6768fd7b4a0a43d8446344a77a75772d6c109aee43270114dc664555040505a0f842821d511732a1aea08d6c9afb0056af72e6fb756f1bbd")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D11,PublicKey=0024000004800000940000000602000000240000525341310004000001000100375a9dd7e968f90d45ff9bd8257ffdc69997de6590c3e74aaab2de0b791f84bcd367dcbae699bbb23e3df7a888b3c8d086159f3646e7e72224a35f2423b5fca176120cb84a8648ff6768fd7b4a0a43d8446344a77a75772d6c109aee43270114dc664555040505a0f842821d511732a1aea08d6c9afb0056af72e6fb756f1bbd")]
#else
// Make internals SharpDX visible to all SharpDX assemblies
[assembly: InternalsVisibleTo("SharpDX.Direct3D10")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D11")]
#endif

