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
namespace SharpDX.Direct3D11
{
    public partial struct RasterizerStateDescription1
    {
        /// <summary>
        /// Returns default values for <see cref="RasterizerStateDescription1"/>. 
        /// </summary>
        /// <remarks>
        /// See MSDN documentation for default values.
        /// </remarks>
        public static RasterizerStateDescription1 Default()
        {
            return new RasterizerStateDescription1()
                       {
                           FillMode = FillMode.Solid,
                           CullMode = CullMode.Back,
                           IsFrontCounterClockwise = false,
                           DepthBias = 0,
                           SlopeScaledDepthBias = 0.0f,
                           DepthBiasClamp = 0.0f,
                           IsDepthClipEnabled = true,
                           IsScissorEnabled = false,
                           IsMultisampleEnabled = false,
                           IsAntialiasedLineEnabled = false,
                           ForcedSampleCount = 0,
                       };
        }
    }
}