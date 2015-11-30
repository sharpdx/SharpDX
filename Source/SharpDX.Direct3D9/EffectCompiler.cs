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

using System;
using System.IO;
using System.Runtime.InteropServices;

using SharpDX.Direct3D;

namespace SharpDX.Direct3D9
{
    public partial class EffectCompiler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectCompiler"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defines">The defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="flags">The flags.</param>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[In] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public EffectCompiler(string data, Macro[] defines, Include includeFile, ShaderFlags flags) : base(IntPtr.Zero)
        {
            IntPtr dataPtr = Marshal.StringToHGlobalAnsi(data);
            try
            {

                CreateEffectCompiler(dataPtr, data.Length, defines, includeFile, flags, this);
            }
            finally
            {
                Marshal.FreeHGlobal(dataPtr);
            }
        }

        /// <summary>
        /// Compile an effect.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <exception cref="CompilationException">If a compilation errors occurs</exception>
        /// <returns>Buffer containing the compiled effect.</returns>
        /// <unmanaged>HRESULT ID3DXEffectCompiler::CompileEffect([In] unsigned int Flags,[In] ID3DXBuffer** ppEffect,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public DataStream CompileEffect(ShaderFlags flags)
        {
            Blob effect;
            Blob blobForErrors = null;
            try
            {
                CompileEffect((int)flags, out effect, out blobForErrors);
            }
            catch (SharpDXException ex)
            {
                if (blobForErrors != null)
                    throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                throw;
            }
            return new DataStream(effect);
        }

        /// <summary>
        /// Compiles a shader from an effect that contains one or more functions.
        /// </summary>
        /// <param name="functionHandle">The function handle.</param>
        /// <param name="target">The target.</param>
        /// <param name="flags">The flags.</param>
        /// <exception cref="CompilationException">If a compilation errors occurs</exception>
        /// <returns>The bytecode of the effect.</returns>
        /// <unmanaged>HRESULT ID3DXEffectCompiler::CompileShader([In] D3DXHANDLE hFunction,[In] const char* pTarget,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public ShaderBytecode CompileShader(EffectHandle functionHandle, string target, ShaderFlags flags)
        {
            ConstantTable constantTable;
            var result = CompileShader(functionHandle, target, flags, out constantTable);
            constantTable.Dispose();
            return result;
        }

        /// <summary>
        /// Compiles a shader from an effect that contains one or more functions.
        /// </summary>
        /// <param name="functionHandle">The function handle.</param>
        /// <param name="target">The target.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="constantTable">The constant table.</param>
        /// <exception cref="CompilationException">If a compilation errors occurs</exception>
        /// <returns>The bytecode of the effect.</returns>
        /// <unmanaged>HRESULT ID3DXEffectCompiler::CompileShader([In] D3DXHANDLE hFunction,[In] const char* pTarget,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public ShaderBytecode CompileShader(EffectHandle functionHandle, string target, ShaderFlags flags, out ConstantTable constantTable)
        {
            Blob shaderBytecode;
            Blob blobForErrors = null;
            try
            {
                CompileShader(functionHandle, target, (int)flags, out shaderBytecode, out blobForErrors, out constantTable);
            }
            catch (SharpDXException ex)
            {
                if (blobForErrors != null)
                    throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                throw;
            }
            return new ShaderBytecode(shaderBytecode);
        }

        /// <summary>
        /// Creates an effect compiler from a file on disk containing an ASCII effect description .
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An instance of <see cref="EffectCompiler"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[Out, Fast] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public static EffectCompiler FromFile(string fileName, ShaderFlags flags)
        {
            return new EffectCompiler(File.ReadAllText(fileName), null, null, flags);
        }

        /// <summary>
        /// Creates an effect compiler from a file on disk containing an ASCII effect description .
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="defines">The defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An instance of <see cref="EffectCompiler"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[Out, Fast] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public static EffectCompiler FromFile(string fileName, Macro[] defines, Include includeFile, ShaderFlags flags)
        {
            return new EffectCompiler(File.ReadAllText(fileName), defines, includeFile, flags);
        }

        /// <summary>
        /// Creates an effect compiler from a memory buffer containing an ASCII effect description .
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An instance of <see cref="EffectCompiler"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[Out, Fast] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public static EffectCompiler FromMemory(byte[] data, ShaderFlags flags)
        {
            return FromMemory(data, null, null, flags);
        }

        /// <summary>
        /// Creates an effect compiler from a memory buffer containing an ASCII effect description .
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="defines">The defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An instance of <see cref="EffectCompiler"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[Out, Fast] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public static EffectCompiler FromMemory(byte[] data, Macro[] defines, Include includeFile, ShaderFlags flags)
        {
            unsafe
            {
                var compiler = new EffectCompiler(IntPtr.Zero);
                fixed (void* pData = data)
                    CreateEffectCompiler((IntPtr)pData, data.Length, defines, includeFile, flags, compiler);
                return compiler;
            }
        }

        /// <summary>
        /// Creates an effect compiler from a stream containing an ASCII effect description .
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An instance of <see cref="EffectCompiler"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[Out, Fast] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public static EffectCompiler FromStream(Stream stream, ShaderFlags flags)
        {
            return FromStream(stream, null, null, flags);
        }

        /// <summary>
        /// Creates an effect compiler from a stream containing an ASCII effect description .
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="defines">The defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>An instance of <see cref="EffectCompiler"/></returns>
        /// <unmanaged>HRESULT D3DXCreateEffectCompiler([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[Out, Fast] ID3DXEffectCompiler** ppCompiler,[In] ID3DXBuffer** ppParseErrors)</unmanaged>
        public static EffectCompiler FromStream(Stream stream, Macro[] defines, Include includeFile, ShaderFlags flags)
        {
            if (stream is DataStream)
            {
                var compiler = new EffectCompiler(IntPtr.Zero);
                CreateEffectCompiler(((DataStream)stream).PositionPointer, (int)(stream.Length - stream.Position), defines, includeFile, flags, compiler);
                return compiler;
            } 
            return FromMemory(Utilities.ReadStream(stream), defines, includeFile, flags);
        }


        private static void CreateEffectCompiler(IntPtr data, int length, Macro[] defines, Include includeFile, ShaderFlags flags, EffectCompiler instance)
        {
            Blob blobForErrors = null;
            try
            {
                D3DX9.CreateEffectCompiler(data, length, defines, IncludeShadow.ToIntPtr(includeFile), (int)flags, instance, out blobForErrors);
            }
            catch (SharpDXException ex)
            {
                if (blobForErrors != null)
                    throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                throw;
            }
        }
    }
}