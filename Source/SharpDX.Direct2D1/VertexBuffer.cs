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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1
{
    public partial class VertexBuffer
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VertexBuffer"/> class.
        /// </summary>
        /// <param name="context">Instance of an effect context</param>
        /// <param name="resourceId"></param>
        /// <param name="vertexBufferProperties"></param>
        /// <param name="customVertexBufferProperties"></param>
        public VertexBuffer(EffectContext context, System.Guid resourceId, SharpDX.Direct2D1.VertexBufferProperties vertexBufferProperties, CustomVertexBufferProperties customVertexBufferProperties) : base(IntPtr.Zero)
        {
            unsafe {            
                var customVertexBufferPropertiesNative = new CustomVertexBufferProperties.__Native();
                customVertexBufferProperties.__MarshalTo(ref customVertexBufferPropertiesNative);

                var inputElementsNative = new InputElement.__Native[customVertexBufferProperties.InputElements.Length];
                try
                {
                    for (int i = 0; i < customVertexBufferProperties.InputElements.Length; i++)
                        customVertexBufferProperties.InputElements[i].__MarshalTo(ref inputElementsNative[i]);

                    fixed (void* pInputElements = inputElementsNative)
                    {
                        customVertexBufferPropertiesNative.InputElementsPointer = (IntPtr)pInputElements;
                        customVertexBufferPropertiesNative.ElementCount = customVertexBufferProperties.InputElements.Length;
                        fixed (void* pInputSignature = customVertexBufferProperties.InputSignature)
                        {
                            customVertexBufferPropertiesNative.ShaderBufferSize = customVertexBufferProperties.InputSignature.Length;
                            customVertexBufferPropertiesNative.ShaderBufferWithInputSignature = (IntPtr)pInputSignature;

                            context.CreateVertexBuffer(vertexBufferProperties, resourceId, new IntPtr(&customVertexBufferPropertiesNative), this);
                        }
                    }
                }
                finally
                {
                    for (int i = 0; i < inputElementsNative.Length; i++)
                        customVertexBufferProperties.InputElements[i].__MarshalFree(ref inputElementsNative[i]);
                }
            }
        }
    }
}