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

using System.Collections.Generic;
using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData 
    {
        /// <summary>
        /// Describes a technique.
        /// </summary>
        public sealed class Technique : IDataSerializable
        {
            /// <summary>
            /// Name of this technique.
            /// </summary>
            /// <remarks>
            /// This value can be null.
            /// </remarks>
            public string Name;

            /// <summary>
            /// List of <see cref="Pass"/>.
            /// </summary>
            public List<Pass> Passes;

            public override string ToString()
            {
                return string.Format("Technique: [{0}], Passes({1})", Name, Passes.Count);
            }

            /// <summary>
            /// Clones this instance.
            /// </summary>
            /// <returns>Technique.</returns>
            public Technique Clone()
            {
                var technique = (Technique)MemberwiseClone();
                if (Passes != null)
                {
                    technique.Passes = new List<Pass>(Passes.Count);
                    for (int i = 0; i < Passes.Count; i++)
                    {
                        var pass = Passes[i];
                        technique.Passes.Add(pass != null ? pass.Clone() : null);
                    }
                }
                return technique;
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name, SerializeFlags.Nullable);

                serializer.Serialize(ref Passes);
            }
        }
    }
}