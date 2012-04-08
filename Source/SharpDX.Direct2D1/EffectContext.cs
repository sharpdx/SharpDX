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
#if WIN8
using System;
using System.Reflection;

namespace SharpDX.Direct2D1
{
    public partial class EffectContext
    {

        /// <summary>
        /// Gets the DPI.
        /// </summary>
        public DrawingPointF Dpi
        {
            get
            {
                DrawingPointF dpi;
                GetDpi(out dpi.X, out dpi.Y);
                return dpi;
            }
            /// <unmanaged>HRESULT ID2D1EffectContext::GetMaximumSupportedFeatureLevel([In, Buffer] const D3D_FEATURE_LEVEL* featureLevels,[In] unsigned int featureLevelsCount,[Out] D3D_FEATURE_LEVEL* maximumSupportedFeatureLevel)</unmanaged>	
        }

        /// <summary>
        /// Gets the maximum feature level supported by this instance.
        /// </summary>
        /// <param name="featureLevels">An array of feature levels</param>
        /// <returns>The maximum feature level selected from the array</returns>
        public SharpDX.Direct3D.FeatureLevel GetMaximumSupportedFeatureLevel(SharpDX.Direct3D.FeatureLevel[] featureLevels)
        {
            return GetMaximumSupportedFeatureLevel(featureLevels, featureLevels.Length);
        }
    }
}
#endif