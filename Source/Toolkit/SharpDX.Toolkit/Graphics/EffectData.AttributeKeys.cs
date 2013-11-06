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

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData 
    {
        /// <summary>The property keys class.</summary>
        public sealed class PropertyKeys
        {
            /// <summary>The blending.</summary>
            public const string Blending = "Blending";

            /// <summary>The blending color.</summary>
            public const string BlendingColor = "BlendingColor";

            /// <summary>The blending sample mask.</summary>
            public const string BlendingSampleMask = "BlendingSampleMask";

            /// <summary>The depth stencil.</summary>
            public const string DepthStencil = "DepthStencil";

            /// <summary>The depth stencil reference.</summary>
            public const string DepthStencilReference = "DepthStencilReference";

            /// <summary>The rasterizer.</summary>
            public const string Rasterizer = "Rasterizer";
        }
    }
}