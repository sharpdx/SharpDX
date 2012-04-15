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
#if DIRECT3D11_1
using System;

namespace SharpDX.Direct2D1
{
    public partial class CommandList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandList"/> class.
        /// </summary>
        /// <param name="deviceContext">The device context.</param>
        public CommandList(DeviceContext deviceContext) : base(IntPtr.Zero)
        {
            deviceContext.CreateCommandList(this);
        }

        /// <summary>
        /// Streams the contents of the command list to the specified command sink. 
        /// </summary>
        /// <param name="sink">The sink into which the command list will be streamed.</param>
        public void Stream(CommandSink sink)
        {
            Stream_(CommandSinkShadow.ToIntPtr(sink));
        }
    }
}
#endif