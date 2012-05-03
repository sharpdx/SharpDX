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
    public class ShaderSignature : DisposeBase
    {
        private DataStream _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="ptr">A pointer to a shader signature bytecode.</param>
        /// <param name="size">The size.</param>
        public ShaderSignature(IntPtr ptr, int size)
        {
            _data = new DataStream(ptr, size, true, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        public ShaderSignature(Blob blob)
        {
            _data = new DataStream(blob);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ShaderSignature(DataStream data)
        {
            _data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="makeACopy">if set to <c>true</c> [make A copy] else the buffer is pinned.</param>
        public ShaderSignature(byte[] data, bool makeACopy = true)
        {
            _data = DataStream.Create(data, true, true, makeACopy);
        }

        /// <summary>
        /// Gets the buffer pointer.
        /// </summary>
        public IntPtr BufferPointer
        {
            get
            {
                return Data.DataPointer;
            }
        }

        /// <summary>
        /// Gets the size of the buffer.
        /// </summary>
        /// <value>
        /// The size of the buffer.
        /// </value>
        public int BufferSize
        {
            get
            {
                return (int)Data.Length;
            }
        }

        /// <summary>
        ///   Gets the raw data of the shader signature.
        /// </summary>
        public DataStream Data
        {
            get { return _data; }
        }

        /// <summary>
        ///   Extracts the input and output signatures from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The input and output signatures of the shader or effect.</returns>
        /// <msdn-id>dd607329</msdn-id>	
        /// <unmanaged>HRESULT D3DGetInputAndOutputSignatureBlob([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[Out] ID3D10Blob** ppSignatureBlob)</unmanaged>	
        /// <unmanaged-short>D3DGetInputAndOutputSignatureBlob</unmanaged-short>	
        public static ShaderSignature GetInputOutputSignature(ShaderBytecode shaderBytecode)
        {
            Blob shaderSignature;
            if (D3D.GetInputAndOutputSignatureBlob(shaderBytecode.BufferPointer, shaderBytecode.BufferSize, out shaderSignature).Failure)
                return null;
            return new ShaderSignature(shaderSignature);
        }

        /// <summary>
        ///   Extracts the input signature from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The input signature of the shader or effect.</returns>
        /// <msdn-id>dd607330</msdn-id>	
        /// <unmanaged>HRESULT D3DGetInputSignatureBlob([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[Out] ID3D10Blob** ppSignatureBlob)</unmanaged>	
        /// <unmanaged-short>D3DGetInputSignatureBlob</unmanaged-short>	
        public static ShaderSignature GetInputSignature(ShaderBytecode shaderBytecode)
        {
            Blob shaderSignature;
            if (D3D.GetInputSignatureBlob(shaderBytecode.BufferPointer, shaderBytecode.BufferSize, out shaderSignature).Failure)
                return null;
            return new ShaderSignature(shaderSignature);
        }

        /// <summary>
        ///   Extracts the output signature from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The output signature of the shader or effect.</returns>
        /// <unmanaged>HRESULT D3DGetOutputSignatureBlob([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[Out] ID3D10Blob** ppSignatureBlob)</unmanaged>
        /// <msdn-id>dd607331</msdn-id>	
        /// <unmanaged>HRESULT D3DGetOutputSignatureBlob([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[Out] ID3D10Blob** ppSignatureBlob)</unmanaged>	
        /// <unmanaged-short>D3DGetOutputSignatureBlob</unmanaged-short>	
        public static ShaderSignature GetOutputSignature(ShaderBytecode shaderBytecode)
        {
            Blob shaderSignature;
            if (D3D.GetOutputSignatureBlob(shaderBytecode.BufferPointer, shaderBytecode.BufferSize, out shaderSignature).Failure)
                return null;
            return new ShaderSignature(shaderSignature);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_data != null)
                    _data.Dispose();
                _data = null;
            }
        }
    }
}