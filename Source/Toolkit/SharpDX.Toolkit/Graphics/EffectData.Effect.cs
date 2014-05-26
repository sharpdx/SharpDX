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

using System;
using System.Collections.Generic;
using System.IO;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Diagnostics;
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Container for shader bytecodes and effect metadata.
    /// </summary>
    /// <remarks>
    /// This class is responsible to store shader bytecodes, effect, techniques, passes...etc.
    /// It is serializable using <see cref="Load(Stream)"/> and <see cref="Save(Stream)"/> method.
    /// </remarks>
    public sealed partial class EffectData : IDataSerializable
    {
        public sealed class Effect : IDataSerializable
        {
            /// <summary>
            /// Name of the effect.
            /// </summary>
            public string Name;

            /// <summary>
            /// Share constant buffers.
            /// </summary>
            public bool ShareConstantBuffers;

            /// <summary>
            /// List of <see cref="Technique"/>.
            /// </summary>
            public List<Technique> Techniques;

            /// <summary>
            /// The compiler arguments used to compile this effect. This field is null if the effect is not compiled with the option "AllowDynamicRecompiling".
            /// </summary>
            public CompilerArguments Arguments;

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.Serialize(ref ShareConstantBuffers);
                serializer.Serialize(ref Techniques);
                serializer.Serialize(ref Arguments, SerializeFlags.Nullable);
            }
        }
   }
}