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
    public class ShaderSignature : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="ptr">A pointer to a shader signature bytecode.</param>
        /// <param name="size">The size.</param>
        public ShaderSignature(IntPtr ptr, int size)
        {
            Data = new byte[size];
            Utilities.Read(ptr, Data, 0, Data.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        public ShaderSignature(Blob blob)
        {
            Data = new byte[blob.BufferSize];
            Utilities.Read(blob.BufferPointer, Data, 0, Data.Length);
            blob.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ShaderSignature(DataStream data)
        {
            Data = new byte[data.Length];
            data.Read(Data, 0, Data.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SharpDX.D3DCompiler.ShaderSignature"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ShaderSignature(byte[] data)
        {
            Data = data;
        }

        /// <summary>
        ///   Gets the raw data of the shader signature.
        /// </summary>
        public byte[] Data
        {
            get;
            private set;
        }

        /// <summary>
        ///   Extracts the input and output signatures from a compiled shader or effect.
        /// </summary>
        /// <param name = "shaderBytecode">The bytecode of the compiled shader or effect.</param>
        /// <returns>The input and output signatures of the shader or effect.</returns>
        /// <msdn-id>dd607329</msdn-id>	
        /// <unmanaged>HRESULT D3DGetInputAndOutputSignatureBlob([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[Out] ID3D10Blob** ppSignatureBlob)</unmanaged>	
        /// <unmanaged-short>D3DGetInputAndOutputSignatureBlob</unmanaged-short>	
        public unsafe static ShaderSignature GetInputOutputSignature(byte[] shaderBytecode)
        {
            Blob shaderSignature;
            fixed (void* ptr = shaderBytecode)
            if (D3D.GetInputAndOutputSignatureBlob((IntPtr)ptr, shaderBytecode.Length, out shaderSignature).Failure)
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
        public unsafe static ShaderSignature GetInputSignature(byte[] shaderBytecode)
        {
            Blob shaderSignature;
            fixed (void* ptr = shaderBytecode)
            if (D3D.GetInputSignatureBlob((IntPtr)ptr, shaderBytecode.Length, out shaderSignature).Failure)
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
        public unsafe static ShaderSignature GetOutputSignature(byte[] shaderBytecode)
        {
            Blob shaderSignature;
            fixed (void* ptr = shaderBytecode)
            if (D3D.GetOutputSignatureBlob((IntPtr)ptr, shaderBytecode.Length, out shaderSignature).Failure)
                return null;
            return new ShaderSignature(shaderSignature);
        }

        /// <summary>
        /// Cast this <see cref="ShaderSignature"/> to the underlying byte buffer.
        /// </summary>
        /// <param name="shaderSignature"></param>
        /// <returns>A byte buffer</returns>
        public static implicit operator byte[](ShaderSignature shaderSignature)
        {
            return (shaderSignature != null) ? shaderSignature.Data : null;
        }

        public void Dispose()
        {
            // Obsolete, just here for backward compatibility
        }
    }
}
