// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    public static partial class D2D1
    {
        /// <summary>
        /// The default tolerance for geometric flattening operations.
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd370975%28v=vs.85%29.aspx
        /// </summary>
        public const float DefaultFlatteningTolerance = 0.25f;

        /// <summary>
        /// The default DPI value.
        /// </summary>
        public const float DefaultDpi = 96f;

        /// <summary>
        /// Computes the appropriate flattening tolerance to pass to APIs that take a flattening tolerance (for instance, <see cref="DeviceContext.CreateFilledGeometryRealization"/>).
        /// </summary>
        /// <param name="matrix">The matrix that will be applied subsequently to the geometry being flattened.</param>
        /// <param name="dpiX">The horizontal DPI of the render target that the geometry will be rendered onto (a choice of 96 implies no DPI correction).</param>
        /// <param name="dpiY">The vertical DPI of the render target that the geometry will be rendered onto (a choice of 96 implies no DPI correction).</param>
        /// <param name="maxZoomFactor">The maximum amount of additional scaling (on top of any scaling implied by the matrix or the DPI) that will be applied to the geometry.</param>
        /// <returns>The flattening tolerance.</returns>
        public static float ComputeFlatteningTolerance(ref RawMatrix3x2 matrix, float dpiX = DefaultDpi, float dpiY = DefaultDpi, float maxZoomFactor = 1f)
        {
            var scaleX = dpiX / DefaultDpi;
            var scaleY = dpiY / DefaultDpi;

            var dpiDependentTransform = new RawMatrix3x2
                                        {
                                            M11 = matrix.M11 * scaleX,
                                            M12 = matrix.M12 * scaleY,
                                            M21 = matrix.M21 * scaleX,
                                            M22 = matrix.M22 * scaleY,
                                            M31 = matrix.M31 * scaleX,
                                            M32 = matrix.M32 * scaleY
                                        };

            var absMaxZoomFactor = maxZoomFactor > 0f ? maxZoomFactor : -maxZoomFactor;
            return DefaultFlatteningTolerance / (absMaxZoomFactor * ComputeMaximumScaleFactor(ref dpiDependentTransform));
        }
    }
}