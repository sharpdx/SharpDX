// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
// -----------------------------------------------------------------------------
// Original code comments from SlimDX project, ported from C++/CLI.
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using SharpDX.Direct3D;

namespace SharpDX.D3DCompiler
{
    /// <summary>
    ///   Represents a shader signature.
    /// </summary>
    public class ShaderSignature : Blob
    {
        private DataStream _data; 

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderSignature" /> class.
        /// </summary>
        /// <param name = "ptr">A pointer to a Blob object.</param>
        public ShaderSignature(IntPtr ptr)
            : base(ptr)
        {
            _data = new DataStream(this);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderSignature" /> class.
        /// </summary>
        /// <param name = "ptr">A pointer to a Blob object.</param>
        private ShaderSignature(Blob blob) : base(IntPtr.Zero)
        {
            FromTemp(blob);
            _data = new DataStream(this);
        }

        /// <summary>
        ///   Extracts the input and output signatures from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The input and output signatures of the shader or effect.</returns>
        public static ShaderSignature GetInputOutputSignature(ShaderBytecode shaderBytecode)
        {
            Blob shaderSignature;
            try
            {
                D3D.GetInputAndOutputSignatureBlob(shaderBytecode.GetBufferPointer(),
                                                   shaderBytecode.GetBufferSize(), out shaderSignature);
            }
            catch (SharpDXException)
            {
                return null;
            }
            return new ShaderSignature(shaderSignature);
        }

        /// <summary>
        ///   Extracts the input signature from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The input signature of the shader or effect.</returns>
        public static ShaderSignature GetInputSignature(ShaderBytecode shaderBytecode)
        {
            Blob shaderSignature;
            try
            {
                D3D.GetInputSignatureBlob(shaderBytecode.GetBufferPointer(),
                                          shaderBytecode.GetBufferSize(), out shaderSignature);
            }
            catch (SharpDXException)
            {
                return null;
            }
            return new ShaderSignature(shaderSignature);
        }

        /// <summary>
        ///   Extracts the output signature from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The output signature of the shader or effect.</returns>
        public static ShaderSignature GetOutputSignature(ShaderBytecode shaderBytecode)
        {
            Blob shaderSignature;
            try
            {
                D3D.GetOutputSignatureBlob(shaderBytecode.GetBufferPointer(),
                                           shaderBytecode.GetBufferSize(), out shaderSignature);
            }
            catch (SharpDXException)
            {
                return null;
            }
            return new ShaderSignature(shaderSignature);
        }

        /// <summary>
        ///   Gets the raw data of the shader signature.
        /// </summary>
        public DataStream Data
        {
            get { return _data; }
        }
    }
}