// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectBytecode 
    {
        /// <summary>
        /// An attribute defined for a <see cref="Pass"/>.
        /// </summary>
        public sealed class Attribute : IDataSerializable
        {
            public const string BlendStateName = "BlendState";
            public const string BlendStateColorName = "BlendStateColor";
            public const string BlendStateSampleMaskName = "BlendStateSampleMask";
            public const string DepthStencilStateName = "DepthStencilState";
            public const string DepthStencilReferenceName = "DepthStencilReference";
            public const string RasterizerStateName = "RasterizerState";

            /// <summary>
            /// Name of this attribute.
            /// </summary>
            public string Name;

            /// <summary>
            /// Value of this attribute.
            /// </summary>
            public object Value;

            public override string ToString()
            {
                return string.Format("{0} = {1}", Name, Value);
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.AllowNull = true;
                serializer.SerializeDynamic(ref Value);
                serializer.AllowNull = false;
            }
        }
    }
}