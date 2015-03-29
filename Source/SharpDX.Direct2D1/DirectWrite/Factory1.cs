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
    /// <p>Creates a rendering parameters object with the specified properties.</p>	
    /// </summary>	
    /// <include file='..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDWriteFactory1']/*"/>	
    /// <msdn-id>Hh780402</msdn-id>	
    /// <unmanaged>IDWriteFactory1</unmanaged>	
    /// <unmanaged-short>IDWriteFactory1</unmanaged-short>	
    public partial class Factory1
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Factory1"/> class with the <see cref="FactoryType.Shared"/> type.
        /// </summary>
        public Factory1()
            : this(FactoryType.Shared)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Factory1"/> class.
        /// </summary>
        /// <param name="factoryType">The factory type.</param>
        public Factory1(FactoryType factoryType)
            : base(IntPtr.Zero)
        {
            DWrite.CreateFactory(factoryType, Utilities.GetGuidFromType(typeof(Factory1)), this);
        }
    }
}