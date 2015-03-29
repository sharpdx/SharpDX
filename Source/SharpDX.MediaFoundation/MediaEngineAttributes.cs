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
using System;

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Attributes used when instantiating <see cref="MediaEngine"/> class.
    /// </summary>
    public partial class MediaEngineAttributes : MediaAttributes
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MediaEngineAttributes"/> class.
        /// </summary>
        /// <param name="nativePtr">A native COM pointer to a MediaEngineAttributes</param>
        public MediaEngineAttributes(System.IntPtr nativePtr)
            : base(nativePtr)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MediaEngineAttributes"/> class.
        /// </summary>
        /// <param name="initialSizeInBytes">Size of the data to allocate</param>
        public MediaEngineAttributes(int initialSizeInBytes = 0) : base(initialSizeInBytes)
        {
        }

        public SharpDX.Multimedia.AudioStreamCategory AudioCategory
        {
            get
            {
                return Get(MediaEngineAttributeKeys.AudioCategory);
            }
            set
            {
                Set(MediaEngineAttributeKeys.AudioCategory, value);
            }
        }

        public SharpDX.Multimedia.AudioEndpointRole AudioEndpointRole
        {
            get
            {
                return Get(MediaEngineAttributeKeys.AudioEndpointRole);
            }
            set
            {
                Set(MediaEngineAttributeKeys.AudioEndpointRole, value);
            }
        }

        public MediaEngineProtectionFlags ContentProtectionFlags
        {
            get
            {
                return Get(MediaEngineAttributeKeys.ContentProtectionFlags);
            }
            set
            {
                Set(MediaEngineAttributeKeys.ContentProtectionFlags, value);
            }
        }

        public ComObject ContentProtectionManager
        {
            get
            {
                return Get(MediaEngineAttributeKeys.ContentProtectionManager);
            }
            set
            {
                Set(MediaEngineAttributeKeys.ContentProtectionManager, value);
            }
        }

        public DXGIDeviceManager DxgiManager
        {
            get
            {
                return Get(MediaEngineAttributeKeys.DxgiManager);
            }
            set
            {
                Set(MediaEngineAttributeKeys.DxgiManager, value);
            }
        }

        public MediaEngineExtension Extension
        {
            get
            {
                return Get(MediaEngineAttributeKeys.Extension);
            }
            set
            {
                Set(MediaEngineAttributeKeys.Extension, value);
            }
        }

        public int VideoOutputFormat
        {
            get
            {
                return Get(MediaEngineAttributeKeys.VideoOutputFormat);
            }
            set
            {
                Set(MediaEngineAttributeKeys.VideoOutputFormat, value);
            }
        }
    }
}