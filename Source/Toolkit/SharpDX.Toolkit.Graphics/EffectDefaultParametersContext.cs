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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A context used by <see cref="EffectDefaultParameters.Apply(ref EffectDefaultParametersContext, ref SharpDX.Matrix,ref SharpDX.Matrix,ref SharpDX.Matrix)"/> in order to store/share intermediate result for an effect.
    /// </summary>
    public struct EffectDefaultParametersContext
    {
        /// <summary>
        /// The view projection. Set <see cref="IsViewProjectionCalculated"/> to true if this value is already calculated.
        /// </summary>
        public Matrix ViewProjection;

        /// <summary>
        /// A boolean indicating whether the <see cref="ViewProjection"/> matrix is already calculated.
        /// </summary>
        public bool IsViewProjectionCalculated;

        public Matrix ViewInverse;

        public bool IsViewInverseCalculated;
    }
}