// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

#if SHARPDX_SIGNED
[assembly: InternalsVisibleTo("SharpDX.Direct3D10,PublicKey=00240000048000009400000006020000002400005253413100040000010001004543d77b41222cfd48f4e0d8dd9b2f83dc15fbede312a422a7454a0b723e988718ebba619773fc8dfed2bc69c97aec4063f51dc5821f5eaa72f331b2782755754dfd998ade0dcbf92a734e532870f661cbe4388f544befa2f32a8e4568e0be071a90fa546c8b4e6efcea755703ae03f6479e787632688be8f6aaae808f6f43ba")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D11,PublicKey=00240000048000009400000006020000002400005253413100040000010001004543d77b41222cfd48f4e0d8dd9b2f83dc15fbede312a422a7454a0b723e988718ebba619773fc8dfed2bc69c97aec4063f51dc5821f5eaa72f331b2782755754dfd998ade0dcbf92a734e532870f661cbe4388f544befa2f32a8e4568e0be071a90fa546c8b4e6efcea755703ae03f6479e787632688be8f6aaae808f6f43ba")]
[assembly: InternalsVisibleTo("SharpDX.Direct2D1,PublicKey=00240000048000009400000006020000002400005253413100040000010001004543d77b41222cfd48f4e0d8dd9b2f83dc15fbede312a422a7454a0b723e988718ebba619773fc8dfed2bc69c97aec4063f51dc5821f5eaa72f331b2782755754dfd998ade0dcbf92a734e532870f661cbe4388f544befa2f32a8e4568e0be071a90fa546c8b4e6efcea755703ae03f6479e787632688be8f6aaae808f6f43ba")]
#else
// Make internals SharpDX visible to all SharpDX assemblies
[assembly: InternalsVisibleTo("SharpDX.Direct3D10")]
[assembly: InternalsVisibleTo("SharpDX.Direct3D11")]
[assembly: InternalsVisibleTo("SharpDX.Direct2D1")]
#endif

