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
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public partial class EffectData
    {
        public class CompilerArguments : IDataSerializable
        {
            /// <summary>
            /// The absolute path to the FX source file used to compile this effect.
            /// </summary>
            public string FilePath;

            /// <summary>
            /// The absolute path to dependency file path generated when compiling this effect.
            /// </summary>
            public string DependencyFilePath;

            /// <summary>
            /// The flags used to compile an effect.
            /// </summary>
            public EffectCompilerFlags CompilerFlags;

            /// <summary>
            /// The macros used to compile this effect (may be null).
            /// </summary>
            public List<ShaderMacro> Macros;

            /// <summary>
            /// The list of include directory used to compile this file (may be null)
            /// </summary>
            public List<string> IncludeDirectoryList;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref FilePath);
                serializer.Serialize(ref DependencyFilePath);
                serializer.SerializeEnum(ref CompilerFlags);
                serializer.Serialize(ref Macros, SerializeFlags.Nullable);
                serializer.Serialize(ref IncludeDirectoryList, serializer.Serialize, SerializeFlags.Nullable);
            }

            public override string ToString()
            {
                return string.Format("FilePath: {0}, CompilerFlags: {1}", FilePath, CompilerFlags);
            }
        }
    }
}