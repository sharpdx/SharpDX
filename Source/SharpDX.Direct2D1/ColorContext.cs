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
    public partial class ColorContext
    {
        /// <summary>	
        /// Initializes a new instance of <see cref="ColorContext"/> class from a color profile.
        /// </summary>
        /// <param name="context">The effect context.</param>
        /// <param name="space">The space of color context to create.</param>	
        /// <param name="profileRef">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateColorContext([In] D2D1_COLOR_SPACE space,[In, Buffer, Optional] const unsigned char* profile,[In] unsigned int profileSize,[Out] ID2D1ColorContext** colorContext)</unmanaged>	
        public ColorContext(EffectContext context, SharpDX.Direct2D1.ColorSpace space, byte[] profileRef) : base(IntPtr.Zero)
        {
            context.CreateColorContext(space, profileRef, profileRef.Length, this);
        }

        /// <summary>	
        /// Initializes a new instance of <see cref="ColorContext"/> class from a filename.
        /// </summary>	
        /// <param name="context">The effect context.</param>	
        /// <param name="filename">The path to the file containing the profile bytes to initialize the color context with..</param>	
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateColorContextFromFilename([In] const wchar_t* filename,[Out] ID2D1ColorContext** colorContext)</unmanaged>	
        public ColorContext(EffectContext context, string filename)
            : base(IntPtr.Zero)
        {
            context.CreateColorContextFromFilename(filename, this);
        }

        /// <summary>	
        /// Initializes a new instance of <see cref="ColorContext"/> class from WIC color context.
        /// </summary>	
        /// <param name="context">No documentation.</param>	
        /// <param name="wicColorContext">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateColorContextFromWicColorContext([In] IWICColorContext* wicColorContext,[Out] ID2D1ColorContext** colorContext)</unmanaged>	
        public ColorContext(EffectContext context, SharpDX.WIC.ColorContext wicColorContext)
            : base(IntPtr.Zero)
        {
            context.CreateColorContextFromWicColorContext(wicColorContext, this);
        }

        /// <summary>
        /// Gets the profile data.
        /// </summary>
        public byte[] ProfileData
        {
            get
            {
                var profileData = new byte[ProfileSize];
                GetProfile(profileData, profileData.Length);
                return profileData;
            }
        }

    }
}