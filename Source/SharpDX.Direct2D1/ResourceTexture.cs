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
    public partial class ResourceTexture
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BlendTransform"/> class
        /// </summary>
        /// <param name="context">The effect context</param>
        /// <param name="resourceId">A unique identifier to the resource</param>
        /// <param name="resourceTextureProperties">The description of the resource</param>
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateResourceTexture([In, Optional] const GUID* resourceId,[In] const D2D1_RESOURCE_TEXTURE_PROPERTIES* resourceTextureProperties,[In, Buffer, Optional] const unsigned char* data,[In, Buffer, Optional] const unsigned int* strides,[In] unsigned int dataSize,[Out] ID2D1ResourceTexture** resourceTexture)</unmanaged>	
        public ResourceTexture(EffectContext context, System.Guid resourceId, SharpDX.Direct2D1.ResourceTextureProperties resourceTextureProperties) : base(IntPtr.Zero) 
        {
            CreateResourceTexture(context, resourceId, resourceTextureProperties, null, null, this);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BlendTransform"/> class
        /// </summary>
        /// <param name="context">The effect context</param>
        /// <param name="resourceId">A unique identifier to the resource</param>
        /// <param name="resourceTextureProperties">The description of the resource</param>
        /// <param name="data">The data to be loaded into the resource texture.</param>
        /// <param name="strides">Reference to the stride to advance through the resource texture, according to dimension.</param>
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateResourceTexture([In, Optional] const GUID* resourceId,[In] const D2D1_RESOURCE_TEXTURE_PROPERTIES* resourceTextureProperties,[In, Buffer, Optional] const unsigned char* data,[In, Buffer, Optional] const unsigned int* strides,[In] unsigned int dataSize,[Out] ID2D1ResourceTexture** resourceTexture)</unmanaged>	
        public ResourceTexture(EffectContext context, System.Guid resourceId, SharpDX.Direct2D1.ResourceTextureProperties resourceTextureProperties, byte[] data, int[] strides)
            : base(IntPtr.Zero)
        {
            CreateResourceTexture(context, resourceId, resourceTextureProperties, data, strides, this);
        }

        private unsafe static void CreateResourceTexture(EffectContext context, System.Guid resourceId, SharpDX.Direct2D1.ResourceTextureProperties resourceTextureProperties, byte[] data, int[] strides, ResourceTexture outTexture) 
        {
            var resourceTexturePropertiesNative = new ResourceTextureProperties.__Native();
            resourceTextureProperties.__MarshalTo(ref resourceTexturePropertiesNative);

            if (resourceTextureProperties.Extents == null || resourceTextureProperties.Extents.Length != resourceTextureProperties.Dimensions)
                throw new ArgumentException("Extents array must be same size than dimensions", "resourceTextureProperties");

            if (resourceTextureProperties.ExtendModes == null || resourceTextureProperties.ExtendModes.Length != resourceTextureProperties.Dimensions)
                throw new ArgumentException("ExtendModes array must be same size than dimensions", "resourceTextureProperties");

            fixed (void* pExtents = resourceTextureProperties.Extents) {
                fixed (void* pExtendModes = resourceTextureProperties.ExtendModes) {
                    resourceTexturePropertiesNative.ExtentsPointer = (IntPtr)pExtents;
                    resourceTexturePropertiesNative.ExtendModesPointer = (IntPtr)pExtendModes;
                    context.CreateResourceTexture(resourceId, new IntPtr(&resourceTexturePropertiesNative), data, strides, data == null ? 0 : data.Length, outTexture);
                }
            }
        }
    }
}