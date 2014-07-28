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
using System;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// An inline object for trimming, using an ellipsis as the omission sign.
    /// </summary>
    public partial class EllipsisTrimming : InlineObjectNative
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EllipsisTrimming"/> class.
        /// </summary>
        /// <param name="nativePtr">The native PTR.</param>
        protected EllipsisTrimming(IntPtr nativePtr) : base(nativePtr)
        {
        }

        /// <summary>	
        /// Creates an inline object for trimming, using an ellipsis as the omission sign. 	
        /// </summary>	
        /// <remarks>	
        /// The ellipsis will be created using the current settings of the format, including base font, style, and any effects. Alternate omission signs can be created by the application by implementing <see cref="SharpDX.DirectWrite.InlineObject"/>.  	
        /// </remarks>
        /// <param name="factory">a <see cref="Factory"/></param>
        /// <param name="textFormat">A text format object, created with {{CreateTextFormat}}, used for text layout. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateEllipsisTrimmingSign([None] IDWriteTextFormat* textFormat,[Out] IDWriteInlineObject** trimmingSign)</unmanaged>
        public EllipsisTrimming(Factory factory, TextFormat textFormat) : this(IntPtr.Zero)
        {
            factory.CreateEllipsisTrimmingSign(textFormat, this);
        }
    }
}