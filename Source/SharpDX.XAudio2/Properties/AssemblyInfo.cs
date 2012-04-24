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

[assembly: AssemblyProduct("SharpDX.XAudio2")]
[assembly: AssemblyTitle("SharpDX.XAudio2")]
[assembly: AssemblyDescription("Assembly providing DirectX - XAudio2 and XAPO managed API")]

#if SHARPDX_SIGNED
[assembly: InternalsVisibleTo("SharpDX.XACT3,PublicKey=00240000048000009400000006020000002400005253413100040000010001000b4163ffddd3e2145eeb7b2ccfc027d314ce5bc7a219b805565d75f8fc6ce36c3c3b32b9030063cb55acc5021ef3e3e7fa701b44f65f3d349c6dda79d1cc013e8a29880d13e5e75a561b01507fef4a96ff68ad4c2538ff7b1182a2fe2822580e40d4b1c5d5a4429b73996277128863e3f289b6d56ff585a5888606ea23fe18a5")]
#else
[assembly: InternalsVisibleTo("SharpDX.XACT3")]
#endif