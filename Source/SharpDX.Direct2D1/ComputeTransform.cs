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

using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    [ShadowAttribute(typeof(ComputeTransformShadow))]
    public partial interface ComputeTransform
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="computeInfo">No documentation.</param>	
        /// <unmanaged>HRESULT ID2D1ComputeTransform::SetComputeInfo([In] ID2D1ComputeInfo* computeInfo)</unmanaged>	
        void SetComputeInformation(SharpDX.Direct2D1.ComputeInformation computeInfo);

        /// <summary>	
        /// [This documentation is preliminary and is subject to change.]	
        /// </summary>	
        /// <param name="outputRect"><para>The output rectangle that will be filled by the compute transform.</para></param>
        /// <returns>An <see cref="Int3"/> containing the number of threads of x,y,z dimensions.</returns>	
        /// <remarks>	
        /// If this call fails, the corresponding <see cref="SharpDX.Direct2D1.Effect"/> instance is placed into an error state and fails to draw.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1ComputeTransform::CalculateThreadgroups([In] const RECT* outputRect,[Out] unsigned int* dimensionX,[Out] unsigned int* dimensionY,[Out] unsigned int* dimensionZ)</unmanaged>	
        RawInt3 CalculateThreadgroups(RawRectangle outputRect);
    }
}