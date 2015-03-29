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

namespace SharpDX.Direct3D11
{
    public partial struct BlendStateDescription1
    {
        /// <summary>
        /// Returns default values for <see cref="BlendStateDescription1"/>. 
        /// </summary>
        /// <remarks>
        /// See MSDN documentation for default values.
        /// </remarks>
        public static BlendStateDescription1 Default()
        {
            var description = new BlendStateDescription1()
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
            };
            var renderTargets = description.RenderTarget;
            for (int i = 0; i < renderTargets.Length; i++)
            {
                renderTargets[i].IsBlendEnabled = false;
                renderTargets[i].IsLogicOperationEnabled = false;

                renderTargets[i].SourceBlend = BlendOption.One;
                renderTargets[i].DestinationBlend = BlendOption.Zero;
                renderTargets[i].BlendOperation = BlendOperation.Add;

                renderTargets[i].SourceAlphaBlend = BlendOption.One;
                renderTargets[i].DestinationAlphaBlend = BlendOption.Zero;
                renderTargets[i].AlphaBlendOperation = BlendOperation.Add;

                renderTargets[i].RenderTargetWriteMask = ColorWriteMaskFlags.All;

                renderTargets[i].LogicOperation = LogicOperation.Noop;
            }

            return description;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this instance.</returns>
        /// <remarks>
        /// Because this structure contains an array, it is not possible to modify it without making an explicit clone method.
        /// </remarks>
        public BlendStateDescription1 Clone()
        {
            var description = new BlendStateDescription1 {AlphaToCoverageEnable = AlphaToCoverageEnable, IndependentBlendEnable = IndependentBlendEnable};
            var sourceRenderTargets = RenderTarget;
            var destRenderTargets = description.RenderTarget;
            for (int i = 0; i < sourceRenderTargets.Length; i++)
                destRenderTargets[i] = sourceRenderTargets[i];
            return description;
        }
    }
}