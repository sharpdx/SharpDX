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
// Original code from SlimDX project, ported from C++/CLI.
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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using SharpDX.Direct3D;

namespace SharpDX.Direct3D9
{
    /// <summary>
    ///   Represents the compiled bytecode of a shader or effect.
    /// </summary>
    /// <unmanaged>Blob</unmanaged>
    public class ShaderBytecode : DisposeBase
    {
        private bool isOwner;
        private Blob blob;
        private ConstantTable constantTable;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SharpDX.Direct3D9.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "data">A <see cref = "SharpDX.DataStream" /> containing the compiled bytecode.</param>
        public ShaderBytecode(DataStream data)
        {
            CreateFromPointer(data.PositionPointer, (int)(data.Length - data.Position));
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SharpDX.Direct3D9.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "data">A <see cref = "T:System.IO.Stream" /> containing the compiled bytecode.</param>
        public ShaderBytecode(Stream data)
        {
            int size = (int)(data.Length - data.Position);

            byte[] localBuffer = new byte[size];
            data.Read(localBuffer, 0, size);
            CreateFromBuffer(localBuffer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBytecode"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public ShaderBytecode(byte[] buffer)
        {
            CreateFromBuffer(buffer);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SharpDX.Direct3D9.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "buffer">a pointer to a compiler bytecode</param>
        /// <param name = "sizeInBytes">size of the bytecode</param>
        public ShaderBytecode(IntPtr buffer, int sizeInBytes)
        {
            BufferPointer = buffer;
            BufferSize = sizeInBytes;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBytecode"/> class.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        protected internal ShaderBytecode(Blob blob)
        {
            this.blob = blob;
            BufferPointer = blob.BufferPointer;
            BufferSize = blob.BufferSize;
        }

        /// <summary>
        /// Gets the buffer pointer.
        /// </summary>
        public System.IntPtr BufferPointer { get; private set; }

        /// <summary>
        /// Gets or sets the size of the buffer.
        /// </summary>
        /// <value>
        /// The size of the buffer.
        /// </value>
        public SharpDX.PointerSize BufferSize { get; set; }

        /// <summary>
        /// Gets the shader constant table.
        /// </summary>
        /// <unmanaged>HRESULT D3DXGetShaderConstantTable([In] const void* pFunction,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public ConstantTable ConstantTable
        {
            get
            {
                if (constantTable != null)
                    return constantTable;
                return constantTable = D3DX9.GetShaderConstantTable(BufferPointer);
            }
        }

        /// <summary>
        /// Gets the version of the shader.
        /// </summary>
        /// <unmanaged>unsigned int D3DXGetShaderVersion([In] const void* pFunction)</unmanaged>
        public int Version
        {
            get
            {
                return D3DX9.GetShaderVersion(BufferPointer);
            }
        }

        /// <summary>
        /// Gets the size of the shader from a function pointer.
        /// </summary>
        /// <param name="shaderFunctionPtr">The shader function pointer.</param>
        /// <returns>Size of the shader</returns>
        public static int GetShaderSize(IntPtr shaderFunctionPtr)
        {
            return D3DX9.GetShaderSize(shaderFunctionPtr);
        }
            
        /// <summary>
        /// Assembles a shader from the given source data.
        /// </summary>
        /// <param name="sourceData">The source shader data.</param>
        /// <param name="flags">Compilation options.</param>
        /// <returns>A <see cref="SharpDX.Direct3D9.ShaderBytecode" /> object representing the raw shader stream.</returns>
        /// <unmanaged>HRESULT D3DXAssembleShader([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static CompilationResult Assemble(byte[] sourceData, ShaderFlags flags)
        {
            return Assemble(sourceData, null, null, flags);            
        }

        /// <summary>
        /// Assembles a shader from the given source data.
        /// </summary>
        /// <param name="sourceData">The source shader data.</param>
        /// <param name="flags">Compilation options.</param>
        /// <returns>A <see cref="SharpDX.Direct3D9.CompilationResult" /> object representing the raw shader stream.</returns>
        /// <unmanaged>HRESULT D3DXAssembleShader([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static CompilationResult Assemble(string sourceData, ShaderFlags flags)
        {
            return Assemble(sourceData, null, null, flags);
        }

        /// <summary>
        /// Assembles a shader from the given source data.
        /// </summary>
        /// <param name="sourceData">The source shader data.</param>
        /// <param name="defines">Macro definitions.</param>
        /// <param name="includeFile">An <see cref="SharpDX.Direct3D9.Include" /> interface to use for handling #include directives.</param>
        /// <param name="flags">Compilation options.</param>
        /// <returns>A <see cref="SharpDX.Direct3D9.CompilationResult" /> object representing the raw shader stream.</returns>
        /// <unmanaged>HRESULT D3DXAssembleShader([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static CompilationResult Assemble(string sourceData, Macro[] defines, Include includeFile, ShaderFlags flags)
        {
            return Assemble(Encoding.ASCII.GetBytes(sourceData), defines, includeFile, flags);
        }

        /// <summary>
        /// Assembles a shader from the given source data.
        /// </summary>
        /// <param name="sourceData">The source shader data.</param>
        /// <param name="defines">Macro definitions.</param>
        /// <param name="includeFile">An <see cref="SharpDX.Direct3D9.Include" /> interface to use for handling #include directives.</param>
        /// <param name="flags">Compilation options.</param>
        /// <returns>A <see cref="SharpDX.Direct3D9.CompilationResult" /> object representing the raw shader stream.</returns>
        /// <unmanaged>HRESULT D3DXAssembleShader([In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static CompilationResult Assemble(byte[] sourceData, Macro[] defines, Include includeFile, ShaderFlags flags)
        {
            unsafe
            {
                var resultCode = Result.Ok;

                Blob blobForCode = null;
                Blob blobForErrors = null;

                try
                {
                    fixed (void* pData = sourceData)
                        D3DX9.AssembleShader(
                            (IntPtr)pData,
                            sourceData.Length,
                            PrepareMacros(defines),
                            IncludeShadow.ToIntPtr(includeFile),
                            (int)flags,
                            out blobForCode,
                            out blobForErrors);
                }
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                    {
                        resultCode = ex.ResultCode;
                        if (Configuration.ThrowOnShaderCompileError)
                            throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                    }
                    else
                    {
                        throw;
                    }
                }

                return new CompilationResult(blobForCode != null ? new ShaderBytecode(blobForCode) : null, resultCode, Utilities.BlobToString(blobForErrors));
            }       
        }

        /// <summary>
        /// Assembles a shader from file.
        /// </summary>
        /// <param name="fileName">Name of the shader file.</param>
        /// <param name="flags">Compilation options.</param>
        /// <returns>A <see cref="SharpDX.Direct3D9.CompilationResult" /> object representing the raw shader stream.</returns>
        public static CompilationResult AssembleFromFile(string fileName, ShaderFlags flags)
        {
            return AssembleFromFile(fileName, null, null, flags);
        }

        /// <summary>
        /// Assembles a shader from file.
        /// </summary>
        /// <param name="fileName">Name of the shader file.</param>
        /// <param name="defines">Macro definitions.</param>
        /// <param name="includeFile">An <see cref="SharpDX.Direct3D9.Include"/> interface to use for handling #include directives.</param>
        /// <param name="flags">Compilation options.</param>
        /// <returns>
        /// A <see cref="SharpDX.Direct3D9.CompilationResult"/> object representing the raw shader stream.
        /// </returns>
        public static CompilationResult AssembleFromFile(string fileName, Macro[] defines, Include includeFile, ShaderFlags flags)
        {
            return Assemble(File.ReadAllText(fileName), defines, includeFile, flags);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(string shaderSource, string profile, ShaderFlags shaderFlags)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), null, profile, shaderFlags, null, null);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags)
        {
            return Compile(shaderSource, null, profile, shaderFlags, null, null);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(string shaderSource, string entryPoint, string profile, ShaderFlags shaderFlags)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), entryPoint, profile, shaderFlags, null, null);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(byte[] shaderSource, string entryPoint, string profile, ShaderFlags shaderFlags)
        {
            return Compile(shaderSource, entryPoint, profile, shaderFlags, null, null);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(string shaderSource, string profile, ShaderFlags shaderFlags, Macro[] defines, Include include)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), null, profile, shaderFlags, defines, include);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags, Macro[] defines, Include include)
        {
            return Compile(shaderSource, null, profile, shaderFlags, defines, include);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(string shaderSource, string entryPoint, string profile, ShaderFlags shaderFlags, Macro[] defines, Include include)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), entryPoint, profile, shaderFlags, defines, include);
        }

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags = ShaderFlags.None, Macro[] defines = null, Include include = null)
        {
            return CompileFromFile(fileName, null, profile, shaderFlags, defines, include);
        }

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult CompileFromFile(string fileName, string entryPoint, string profile, ShaderFlags shaderFlags = ShaderFlags.None, Macro[] defines = null, Include include = null)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not open the shader or effect file.", fileName);
            }
            return Compile(File.ReadAllText(fileName), entryPoint, profile, shaderFlags, PrepareMacros(defines), include);
        }


        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        /// <unmanaged>HRESULT D3DXCompileShader([In] const char* pSrcData,[In] unsigned int SrcDataLen,[In] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pFunctionName,[In] const char* pProfile,[In] unsigned int Flags,[In] ID3DXBuffer** ppShader,[In] ID3DXBuffer** ppErrorMsgs,[In] ID3DXConstantTable** ppConstantTable)</unmanaged>
        public static CompilationResult Compile(byte[] shaderSource, string entryPoint, string profile, ShaderFlags shaderFlags, Macro[] defines, Include include)
        {
            unsafe
            {
                var resultCode = Result.Ok;

                Blob blobForCode = null;
                Blob blobForErrors = null;
                ConstantTable constantTable = null;

                try
                {
                    fixed (void* pData = &shaderSource[0])
                        D3DX9.CompileShader(
                            (IntPtr)pData,
                            shaderSource.Length,
                            PrepareMacros(defines),
                            IncludeShadow.ToIntPtr(include),
                            entryPoint,
                            profile,
                            (int)shaderFlags,
                            out blobForCode,
                            out blobForErrors,
                            out constantTable);
                } 
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                    {
                        resultCode = ex.ResultCode;
                        if (Configuration.ThrowOnShaderCompileError)
                            throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    if (constantTable != null)
                        constantTable.Dispose();
                }

                return new CompilationResult(blobForCode != null ? new ShaderBytecode(blobForCode) : null, resultCode, Utilities.BlobToString(blobForErrors));
            }
        }

        /// <summary>
        ///   Disassembles compiled HLSL code back into textual source.
        /// </summary>
        /// <returns>The textual source of the shader or effect.</returns>
        /// <unmanaged>HRESULT D3DXDisassembleShader([In] const void* pShader,[In] BOOL EnableColorCode,[In] const char* pComments,[In] ID3DXBuffer** ppDisassembly)</unmanaged>
        public string Disassemble()
        {
            return this.Disassemble(false, null);
        }

        /// <summary>
        /// Disassembles compiled HLSL code back into textual source.
        /// </summary>
        /// <param name="enableColorCode">if set to <c>true</c> [enable color code].</param>
        /// <returns>
        /// The textual source of the shader or effect.
        /// </returns>
        /// <unmanaged>HRESULT D3DXDisassembleShader([In] const void* pShader,[In] BOOL EnableColorCode,[In] const char* pComments,[In] ID3DXBuffer** ppDisassembly)</unmanaged>
        public string Disassemble(bool enableColorCode)
        {
            return this.Disassemble(enableColorCode, null);
        }

        /// <summary>
        /// Disassembles compiled HLSL code back into textual source.
        /// </summary>
        /// <param name="enableColorCode">if set to <c>true</c> [enable color code].</param>
        /// <param name="comments">Commenting information to embed in the disassembly.</param>
        /// <returns>
        /// The textual source of the shader or effect.
        /// </returns>
        /// <unmanaged>HRESULT D3DXDisassembleShader([In] const void* pShader,[In] BOOL EnableColorCode,[In] const char* pComments,[In] ID3DXBuffer** ppDisassembly)</unmanaged>
        public string Disassemble(bool enableColorCode, string comments)
        {
            Blob output;
            D3DX9.DisassembleShader(BufferPointer, enableColorCode, comments, out output);
            return Utilities.BlobToString(output);
        }

        /// <summary>
        /// Searches through the shader for the specified comment.
        /// </summary>
        /// <param name="fourCC">A FOURCC code used to identify the comment.</param>
        /// <returns>The comment data.</returns>
        /// <unmanaged>HRESULT D3DXFindShaderComment([In] const void* pFunction,[In] unsigned int FourCC,[Out] const void** ppData,[Out] unsigned int* pSizeInBytes)</unmanaged>
        public DataStream FindComment(Format fourCC)
        {
            IntPtr buffer;
            int size;

            D3DX9.FindShaderComment(BufferPointer, (int)fourCC, out buffer, out size);

            return new DataStream(buffer, size, true, true);
        }

        /// <summary>
        /// Gets the set of semantics for shader inputs.
        /// </summary>
        /// <returns>The set of semantics for shader inputs.</returns>
        /// <unmanaged>HRESULT D3DXGetShaderInputSemantics([In] const void* pFunction,[In, Out, Buffer] D3DXSEMANTIC* pSemantics,[InOut] unsigned int* pCount)</unmanaged>
        public ShaderSemantic[] GetInputSemantics()
        {
            int count = 0;
            D3DX9.GetShaderInputSemantics(BufferPointer, null, ref count);
            if (count == 0)
                return null;

            var buffer = new ShaderSemantic[count];
            D3DX9.GetShaderInputSemantics(BufferPointer, buffer, ref count);

            return buffer;
        }

        /// <summary>
        /// Gets the set of semantics for shader outputs.
        /// </summary>
        /// <returns>The set of semantics for shader outputs.</returns>
        /// <unmanaged>HRESULT D3DXGetShaderOutputSemantics([In] const void* pFunction,[In, Out, Buffer] D3DXSEMANTIC* pSemantics,[InOut] unsigned int* pCount)</unmanaged>
        public ShaderSemantic[] GetOutputSemantics()
        {
            int count = 0;
            D3DX9.GetShaderOutputSemantics(BufferPointer, null, ref count);
            if (count == 0)
                return null;

            var buffer = new ShaderSemantic[count];
            D3DX9.GetShaderOutputSemantics(BufferPointer, buffer, ref count);

            return buffer;
        }

        /// <summary>
        /// Gets the sampler names references in the shader.
        /// </summary>
        /// <returns>The set of referenced sampler names.</returns>
        /// <unmanaged>HRESULT D3DXGetShaderSamplers([In] const void* pFunction,[In] const char** pSamplers,[In] unsigned int* pCount)</unmanaged>
        public string[] GetSamplers()
        {
            unsafe
            {
                int count = 0;
                D3DX9.GetShaderSamplers(BufferPointer, IntPtr.Zero, ref count);
                if (count == 0)
                    return null;

                var result = new string[count];

                if (count < 1024)
                {
                    var pointers = stackalloc IntPtr[count];
                    D3DX9.GetShaderSamplers(BufferPointer, (IntPtr)pointers, ref count);
                    for (int i = 0; i < count; i++)
                        result[i] = Marshal.PtrToStringAnsi(pointers[i]);
                } 
                else
                {
                    var pointers = new IntPtr[count];
                    fixed (void* pPointers = pointers)
                        D3DX9.GetShaderSamplers(BufferPointer, (IntPtr)pPointers, ref count);
                    for (int i = 0; i < count; i++)
                        result[i] = Marshal.PtrToStringAnsi(pointers[i]);
                }
                return result;
            }
        }

        /// <summary>
        /// Extracts the major version component of a shader version number.
        /// </summary>
        /// <param name="version">The shader version number.</param>
        /// <returns>The major version component.</returns>
        public static int MajorVersion(int version)
        {
            return ((version >> 8) & 0xff);
        }

        /// <summary>
        /// Extracts the minor version component of a shader version number.
        /// </summary>
        /// <param name="version">The shader version number.</param>
        /// <returns>The minor version component.</returns>
        public static int MinorVersion(int version)
        {
            return (version & 0xff);
        }

        /// <summary>
        /// Converts a shader version number into a managed <see cref="T:System.Version" /> object.
        /// </summary>
        /// <param name="version">The shader version number.</param>
        /// <returns>The parsed shader version information.</returns>
        public static Version ParseVersion(int version)
        {
            return new Version((version >> 8) & 0xff, version & 0xff);
        }

        /// <summary>
        /// Loads from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>A shader bytecode</returns>
        public static ShaderBytecode Load(Stream stream)
        {
            var buffer = Utilities.ReadStream(stream);
            return new ShaderBytecode(buffer);
        }

        /// <summary>
        /// Saves to the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                Save(stream);
        }

        /// <summary>
        /// Saves this bytecode to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            if (BufferSize == 0) return;

            var buffer = new byte[BufferSize];
            Utilities.Read(BufferPointer, buffer, 0, buffer.Length);
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Create a ShaderBytecode from a pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns></returns>
        public static ShaderBytecode FromPointer(IntPtr pointer)
        {
            // TODO: check that pointer is a blob?
            return new ShaderBytecode(new Blob(pointer));
        }

        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">A string containing the source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string Preprocess(string shaderSource, Macro[] defines = null, Include include = null)
        {
            string errors = null;
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Preprocess(shaderSourcePtr, shaderSource.Length, defines, include, out errors);
            }
            finally
            {
                if (shaderSourcePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(shaderSourcePtr);
            }
        }

        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">An array of bytes containing the raw source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string Preprocess(byte[] shaderSource, Macro[] defines = null, Include include = null)
        {
            string errors = null;
            return Preprocess(shaderSource, defines, include, out errors);
        }

        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">An array of bytes containing the raw source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string Preprocess(byte[] shaderSource, Macro[] defines, Include include, out string compilationErrors)
        {
            unsafe
            {
                fixed (void* pData = &shaderSource[0])
                    return Preprocess((IntPtr)pData, shaderSource.Length, defines, include, out compilationErrors);
            }
        }


        /// <summary>
        /// Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSourcePtr">The shader source PTR.</param>
        /// <param name="shaderSourceLength">Length of the shader source.</param>
        /// <param name="defines">A set of macros to define during preprocessing.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <returns>
        /// The preprocessed shader source.
        /// </returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string Preprocess(IntPtr shaderSourcePtr, int shaderSourceLength, Macro[] defines, Include include, out string compilationErrors)
        {
            unsafe
            {
                Blob blobForText = null;
                Blob blobForErrors = null;
                compilationErrors = null;

                try
                {
                    D3DX9.PreprocessShader(shaderSourcePtr, shaderSourceLength, PrepareMacros(defines), IncludeShadow.ToIntPtr(include), out blobForText, out blobForErrors);
                }
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                    {
                        compilationErrors = Utilities.BlobToString(blobForErrors);
                        throw new CompilationException(ex.ResultCode, compilationErrors);
                    }
                    throw;
                }
                return Utilities.BlobToString(blobForText);
            }
        }

        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">A string containing the source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string Preprocess(string shaderSource, Macro[] defines, Include include, out string compilationErrors)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Preprocess(shaderSourcePtr, shaderSource.Length, defines, include, out compilationErrors);
            }
            finally
            {
                if (shaderSourcePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(shaderSourcePtr);
            }
        }

        /// <summary>
        ///   Preprocesses a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string PreprocessFromFile(string fileName)
        {
            string errors = null;
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not open the shader or effect file.", fileName);
            }
            string str = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("fileName");
            }
            return Preprocess(Encoding.ASCII.GetBytes(str), null, null, out errors);
        }

        /// <summary>
        ///   Preprocesses a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string PreprocessFromFile(string fileName, Macro[] defines, Include include)
        {
            string errors = null;
            return PreprocessFromFile(fileName, defines, include, out errors);
        }

        /// <summary>
        ///   Preprocesses a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <returns>The preprocessed shader source.</returns>
        /// <unmanaged>HRESULT D3DXPreprocessShader([In] const void* pSrcData,[In] unsigned int SrcDataSize,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] ID3DXBuffer** ppShaderText,[In] ID3DXBuffer** ppErrorMsgs)</unmanaged>
        public static string PreprocessFromFile(string fileName, Macro[] defines, Include include, out string compilationErrors)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not open the shader or effect file.", fileName);
            }
            return Preprocess(File.ReadAllText(fileName), defines, include, out compilationErrors);
        }

        /// <summary>
        ///   Gets the raw data of the compiled bytecode.
        /// </summary>
        public DataStream Data
        {
            get { return new DataStream(BufferPointer, BufferSize, true, true); }
        }

        /// <summary>
        ///   Read a compiled shader bytecode from a Stream and return a ShaderBytecode
        /// </summary>
        /// <param name = "stream"></param>
        /// <returns></returns>
        public static ShaderBytecode FromStream(Stream stream)
        {
            return new ShaderBytecode(stream);
        }

        /// <summary>
        ///   Read a compiled shader bytecode from a Stream and return a ShaderBytecode
        /// </summary>
        /// <param name = "fileName"></param>
        /// <returns></returns>
        public static ShaderBytecode FromFile(string fileName)
        {
            return new ShaderBytecode(File.ReadAllBytes(fileName));
        }

        internal static Macro[] PrepareMacros(Macro[] macros)
        {
            if (macros == null)
                return null;

            if (macros.Length == 0)
                return null;

            if (macros[macros.Length - 1].Name == null && macros[macros.Length - 1].Definition == null)
                return macros;

            var macroArray = new Macro[macros.Length + 1];

            Array.Copy(macros, macroArray, macros.Length);

            macroArray[macros.Length] = new Macro(null, null);
            return macroArray;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (blob != null)
                {
                    blob.Dispose();
                    blob = null;
                }

                if (constantTable != null)
                {
                    constantTable.Dispose();
                    constantTable = null;
                }
            }
            if (isOwner && BufferPointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(BufferPointer);
                BufferPointer = IntPtr.Zero;
                BufferSize = 0;
            }
        }

        private void CreateFromBuffer(byte[] buffer)
        {
            unsafe
            {
                fixed (void* pBuffer = &buffer[0])
                    CreateFromPointer((IntPtr)pBuffer, buffer.Length);
            }
        }

        private void CreateFromPointer(IntPtr buffer, int sizeInBytes)
        {
            // D3DCommon.CreateBlob(sizeInBytes, this);
            BufferPointer = Marshal.AllocHGlobal(sizeInBytes);
            BufferSize = sizeInBytes;
            isOwner = true;
            Utilities.CopyMemory(BufferPointer, buffer, sizeInBytes);
        }
    }
}