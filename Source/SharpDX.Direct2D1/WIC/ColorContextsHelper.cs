// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// Contributions by Peter Verswyvelen - www.strongly-typed.net
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
using System.Diagnostics;

namespace SharpDX.WIC
{
    /// <summary>The color contexts provider delegate.</summary>
    /// <param name="count">The count.</param>
    /// <param name="colorContexts">The color contexts.</param>
    /// <param name="actualCountRef">The actual count preference.</param>
    /// <returns>Result.</returns>
    internal delegate Result ColorContextsProvider(int count, ColorContext[] colorContexts, out int actualCountRef);

    /// <summary>The color contexts helper class.</summary>
    public static class ColorContextsHelper
    {
        /// <summary>Tries the get color contexts.</summary>
        /// <param name="getColorContexts">The get color contexts.</param>
        /// <param name="imagingFactory">The imaging factory.</param>
        /// <param name="colorContexts">The color contexts.</param>
        /// <returns>Result.</returns>
        internal static Result TryGetColorContexts(ColorContextsProvider getColorContexts, ImagingFactory imagingFactory, out ColorContext[] colorContexts)
        {
            colorContexts = null;

            int count;
            Result result = getColorContexts(0, null, out count);

            if (result.Success)
            {
                colorContexts = new ColorContext[count];

                if (count > 0)
                {
                    // http://msdn.microsoft.com/en-us/library/windows/desktop/ee690135(v=vs.85).aspx
                    // The ppIColorContexts array must be filled with valid data: each IWICColorContext* in the array must 
                    // have been created using IWICImagingFactory::CreateColorContext.

                    for (int i = 0; i < count; i++)
                        colorContexts[i] = new ColorContext(imagingFactory);
                    int actualCount;
                    getColorContexts(count, colorContexts, out actualCount);
                    Debug.Assert(count == actualCount);
                }
            }

            return result;
        }

        /// <summary>Tries the get color contexts.</summary>
        /// <param name="getColorContexts">The get color contexts.</param>
        /// <param name="imagingFactory">The imaging factory.</param>
        /// <returns>ColorContext[][].</returns>
        internal static ColorContext[] TryGetColorContexts(ColorContextsProvider getColorContexts, ImagingFactory imagingFactory)
        {
            ColorContext[] colorContexts;
            Result result = TryGetColorContexts(getColorContexts, imagingFactory, out colorContexts);

            if (ResultCode.UnsupportedOperation != result)
                result.CheckError();

            return colorContexts;
        }

        /// <summary>Gets the color contexts.</summary>
        /// <param name="getColorContexts">The get color contexts.</param>
        /// <param name="imagingFactory">The imaging factory.</param>
        /// <returns>ColorContext[][].</returns>
        internal static ColorContext[] GetColorContexts(ColorContextsProvider getColorContexts, ImagingFactory imagingFactory)
        {
            ColorContext[] colorContexts;
            Result result = TryGetColorContexts(getColorContexts, imagingFactory, out colorContexts);
            result.CheckError();
            return colorContexts;
        }
    }
}