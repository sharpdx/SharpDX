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

using System.Collections;
using System.Collections.Generic;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData 
    {
        /// <summary>
        /// Describes link to shaders for each pipeline <see cref="EffectShaderType"/>
        /// </summary>
        public sealed class Pipeline : IDataSerializable, IEnumerable<ShaderLink>
        {
            public ShaderLink[] Links;

            /// <summary>
            /// Initializes a new instance of the <see cref="Pipeline" /> class.
            /// </summary>
            public Pipeline()
            {
                Links = new ShaderLink[6];
            }

            /// <summary>
            /// Clones this instance.
            /// </summary>
            /// <returns>Pipeline.</returns>
            public Pipeline Clone()
            {
                var pipeline = (Pipeline)MemberwiseClone();
                pipeline.Links = new ShaderLink[Links.Length];
                for (int i = 0; i < Links.Length; i++)
                {
                    var link = Links[i];
                    if (link != null)
                    {
                        pipeline.Links[i] = link.Clone();
                    }
                }
                return pipeline;
            }


            /// <summary>
            /// Gets or sets the <see cref="ShaderLink" /> with the specified stage type.
            /// </summary>
            /// <param name="effectShaderType">Type of the stage.</param>
            /// <returns>A <see cref="ShaderLink"/></returns>
            /// <remarks>
            /// The return value can be null if there is no shaders associated for this particular stage.
            /// </remarks>
            public ShaderLink this[EffectShaderType effectShaderType]
            {
                get { return Links[(int)effectShaderType]; }
                set { Links[(int)effectShaderType] = value; }
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Links, SerializeFlags.Nullable);
            }

            public IEnumerator<ShaderLink> GetEnumerator()
            {
                foreach (var shaderLink in Links)
                    yield return shaderLink;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}