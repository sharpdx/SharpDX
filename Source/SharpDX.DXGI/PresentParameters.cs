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

using System.Runtime.InteropServices;
using SharpDX.Mathematics.Interop;

namespace SharpDX.DXGI
{
    public partial struct PresentParameters 
    {
        /// <summary>	
        /// <para>A list of updated rectangles that you update in the back buffer for the presented frame. An application must update every single pixel in each rectangle that it reports to the runtime; the application cannot assume that the pixels are saved from the previous frame. For more information about updating dirty rectangles, see Remarks. You can set this member to <c>null</c> if DirtyRectsCount is 0. An application must not update any pixel outside of the dirty rectangles.</para>	
        /// </summary>	
        /// <unmanaged>RECT* pDirtyRects</unmanaged>	
        public RawRectangle[] DirtyRectangles;

        /// <summary>	
        /// <para> A reference to the scrolled rectangle. The scrolled rectangle is the rectangle of the previous frame from which the runtime bit-block transfers (bitblts) content. The runtime also uses the scrolled rectangle to optimize presentation in terminal server and indirect display scenarios.</para>	
        ///  <para>The scrolled rectangle also describes the destination rectangle, that is, the region on the current frame that is filled with scrolled content. You can set this member to <c>null</c> to indicate that no content is scrolled from the previous frame.</para>	
        /// </summary>	
        /// <unmanaged>RECT* pScrollRect</unmanaged>	
        public RawRectangle? ScrollRectangle;

        /// <summary>	
        /// <para>A reference to the offset of the scrolled area that goes from the source rectangle (of previous frame) to the destination rectangle (of current frame). You can set this member to <c>null</c> to indicate no offset.</para>	
        /// </summary>	
        /// <unmanaged>POINT* pScrollOffset</unmanaged>	
        public RawPoint? ScrollOffset;

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public int DirtyRectsCount;
            public System.IntPtr PDirtyRects;
            public System.IntPtr PScrollRect;
            public System.IntPtr PScrollOffset;
        }
    }
}