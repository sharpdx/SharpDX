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
using SharpDX.IO;
using SharpDX.Text;
using Encoding = SharpDX.Text.Encoding;

namespace SharpDX.D3DCompiler
{
    using Multimedia;

    /// <summary>
    ///   Represents the compiled bytecode of a shader or effect.
    /// </summary>
    /// <unmanaged>Blob</unmanaged>
    public class ShaderBytecode : IDisposable
    {
        /// <summary>
        /// Use this ShaderFlags constant in order to compile an effect with old D3D10CompileEffectFromMemory.
        /// </summary>
        public const ShaderFlags Effect10 = (ShaderFlags)0x40000000;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "data">A <see cref = "T:SharpDX.DataStream" /> containing the compiled bytecode.</param>
        public ShaderBytecode(DataStream data)
        {
            Data = new byte[data.Length];
            data.Read(Data, 0, Data.Length);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "data">A <see cref = "T:System.IO.Stream" /> containing the compiled bytecode.</param>
        public ShaderBytecode(Stream data)
        {
            int size = (int)(data.Length - data.Position);

            byte[] localBuffer = new byte[size];
            data.Read(localBuffer, 0, size);
            Data = localBuffer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBytecode"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public ShaderBytecode(byte[] buffer)
        {
            Data = buffer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "buffer">a pointer to a compiler bytecode</param>
        /// <param name = "sizeInBytes">size of the bytecode</param>
        public ShaderBytecode(IntPtr buffer, int sizeInBytes)
        {
            Data = new byte[sizeInBytes];
            Utilities.Read(buffer, Data, 0, Data.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBytecode"/> class.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        protected internal ShaderBytecode(Blob blob)
        {
            Data = new byte[blob.BufferSize];
            Utilities.Read(blob.BufferPointer, Data, 0, Data.Length);
            blob.Dispose();
        }

        /// <summary>
        /// Gets the buffer pointer.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(string shaderSource, string profile, ShaderFlags shaderFlags = ShaderFlags.None,
                                             EffectFlags effectFlags = EffectFlags.None, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Compile(
                    shaderSourcePtr,
                    shaderSource.Length,
                    null,
                    profile,
                    shaderFlags,
                    effectFlags,
                    null,
                    null,              
                    sourceFileName,
                    secondaryDataFlags,
                    secondaryData);
            } finally
            {
                if (shaderSourcePtr != IntPtr.Zero) Marshal.FreeHGlobal(shaderSourcePtr);
            }
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(string shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags = ShaderFlags.None, EffectFlags effectFlags = EffectFlags.None, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Compile(shaderSourcePtr, shaderSource.Length, entryPoint, profile, shaderFlags, effectFlags, null,
                               null, sourceFileName, secondaryDataFlags, secondaryData);
            }
            finally
            {
                if (shaderSourcePtr != IntPtr.Zero) Marshal.FreeHGlobal(shaderSourcePtr);
            }
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(string shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, ShaderMacro[] defines, Include include,
                                             string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Compile(shaderSourcePtr, shaderSource.Length, null, profile, shaderFlags, effectFlags, defines,
                               include, sourceFileName, secondaryDataFlags, secondaryData);
            }
            finally
            {
                if (shaderSourcePtr != IntPtr.Zero) Marshal.FreeHGlobal(shaderSourcePtr);
            }
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, ShaderMacro[] defines, Include include,
                                             string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            return Compile(shaderSource, null, profile, shaderFlags, effectFlags, defines, include,
                           sourceFileName, secondaryDataFlags, secondaryData);
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(string shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }

            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Compile(shaderSourcePtr, shaderSource.Length, entryPoint, profile, shaderFlags, effectFlags, defines,
                               include, sourceFileName, secondaryDataFlags, secondaryData);
            }
            finally
            {
                if (shaderSourcePtr != IntPtr.Zero) Marshal.FreeHGlobal(shaderSourcePtr);
            }
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="sourceFileName">Name of the source file used for reporting errors. Default is "unknown"</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(byte[] shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            unsafe
            {
                fixed (void* pData = &shaderSource[0])
                    return Compile(
                        (IntPtr)pData,
                        shaderSource.Length,
                        entryPoint,
                        profile,
                        shaderFlags,
                        effectFlags,
                        defines,
                        include,
                        sourceFileName,
                        secondaryDataFlags,
                        secondaryData);
            }
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="textSource">The shader data.</param>
        /// <param name="textSize">Size of the shader.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="sourceFileName">Name of the source file used for reporting errors. Default is "unknown"</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(IntPtr textSource, int textSize, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            unsafe
            {
                if (string.IsNullOrWhiteSpace(profile))
                    throw new ArgumentNullException("profile");

                if (!(profile.ToUpperInvariant().StartsWith("FX_") || profile.ToUpperInvariant().StartsWith("LIB_")) && string.IsNullOrWhiteSpace(entryPoint))
                    throw new ArgumentNullException("entryPoint");

                Blob blobForCode = null;
                Blob blobForErrors = null;

                var resultCode = D3D.Compile2(
                    (IntPtr)textSource,
                    textSize,
                    sourceFileName,
                    PrepareMacros(defines),
                    include,
                    entryPoint,
                    profile,
                    shaderFlags,
                    effectFlags,
                    secondaryDataFlags,
                    secondaryData != null ? secondaryData.DataPointer : IntPtr.Zero,
                    secondaryData != null ? (int)secondaryData.Length : 0,
                    out blobForCode,
                    out blobForErrors);

                if (resultCode.Failure)
                {
                    if (blobForErrors != null)
                    {
                        if (Configuration.ThrowOnShaderCompileError)
                            throw new CompilationException(resultCode, Utilities.BlobToString(blobForErrors));
                    }
                    else
                    {
                        throw new SharpDXException(resultCode);
                    }
                }

                return new CompilationResult(blobForCode != null ? new ShaderBytecode(blobForCode) : null, resultCode, Utilities.BlobToString(blobForErrors));
            }
        }

        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags = ShaderFlags.None,
                                             EffectFlags effectFlags = EffectFlags.None, string sourceFileName = "unknown")
        {
            return Compile(shaderSource, null, profile, shaderFlags, effectFlags, null, null, sourceFileName);
        }

        /// <summary>
        ///   Compiles the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name = "entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static CompilationResult Compile(byte[] shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags = ShaderFlags.None,
                                             EffectFlags effectFlags = EffectFlags.None, string sourceFileName = "unknown")
        {
            return Compile(shaderSource, entryPoint, profile, shaderFlags, effectFlags, null, null,
                           sourceFileName);
        }

        /// <summary>
        ///   Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <param name = "defines">A set of macros to define during compilation.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static CompilationResult CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags = ShaderFlags.None, EffectFlags effectFlags = EffectFlags.None, ShaderMacro[] defines = null, Include include = null)
        {
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, defines, include);
        }

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static CompilationResult CompileFromFile(string fileName,
            string entryPoint,
            string profile,
            ShaderFlags shaderFlags = ShaderFlags.None,
            EffectFlags effectFlags = EffectFlags.None,
            ShaderMacro[] defines = null,
            Include include = null)
        {
            return Compile(NativeFile.ReadAllText(fileName), entryPoint, profile, shaderFlags, effectFlags, defines, include, fileName);
        }

        // Win 8.1 SDK removed the corresponding functions from the WinRT platform
#if DESKTOP_APP
        /// <summary>	
        /// Compresses a set of shaders into a more compact form. 	
        /// </summary>	
        /// <param name="shaderBytecodes">An array of <see cref="SharpDX.D3DCompiler.ShaderBytecode"/> structures that describe the set of shaders to compress. </param>
        /// <returns>A compressed <see cref="SharpDX.D3DCompiler.ShaderBytecode"/>. </returns>
        /// <unmanaged>HRESULT D3DCompressShaders([In] int uNumShaders,[In, Buffer] D3D_SHADER_DATA* pShaderData,[In] int uFlags,[Out] ID3DBlob** ppCompressedData)</unmanaged>
        public static ShaderBytecode Compress(params ShaderBytecode[] shaderBytecodes)
        {
            Blob blob;
            // D3D.CompressShaders()
            var temp = new ShaderData[shaderBytecodes.Length];
            var handles = new GCHandle[shaderBytecodes.Length];
            try
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    handles[i] = GCHandle.Alloc(shaderBytecodes[i].Data, GCHandleType.Pinned);

                    temp[i] = new ShaderData
                                  {
                                      BytecodePtr = handles[i].AddrOfPinnedObject(),
                                      BytecodeLength = shaderBytecodes[i].Data.Length
                                  };
                }
                D3D.CompressShaders(shaderBytecodes.Length, temp, 1, out blob);
            }
            finally
            {
                foreach (var gcHandle in handles)
                    gcHandle.Free();
            }
            return new ShaderBytecode(blob) { IsCompressed = true };
        }

        /// <summary>	
        /// Decompresses all shaders from a compressed set.	
        /// </summary>	
        /// <returns>Returns an array of decompress shader bytecode.</returns>	
        /// <unmanaged>HRESULT D3DDecompressShaders([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] unsigned int uNumShaders,[In] unsigned int uStartIndex,[In, Buffer, Optional] unsigned int* pIndices,[In] unsigned int uFlags,[Out, Buffer] ID3D10Blob** ppShaders,[Out, Optional] unsigned int* pTotalShaders)</unmanaged>	
        public unsafe ShaderBytecode[] Decompress()
        {
            //First we call D3D.DecompressShaders with empty parameters to get the number of shaders in the compressed byte code (totalShadersRef)
            //I set shadersBlobs to an array of one null blob just to make shadersOut_ parameter in DecompressShaders function valid 
            //which doesn't seem to accept zero pointer, I didn't find any other work around.
            var shadersBlobs = new Blob[1];
            int totalShadersRef;
            fixed (void* bufferPtr = Data)
                D3D.DecompressShaders((IntPtr)bufferPtr, Data.Length, 0, 0, null, 0, shadersBlobs, out totalShadersRef);

            //Then we call D3D.DecompressShaders again and we know how much shaders we will get
            return Decompress(0, totalShadersRef);
        }

        /// <summary>	
        /// Decompresses one or more shaders from a compressed set.	
        /// </summary>	
        /// <param name="numShaders"><para>The number of shaders to decompress.</para></param>	
        /// <param name="startIndex"><para>The index of the first shader to decompress.</para></param>	
        /// <returns>Returns an array of decompress shader bytecode.</returns>	
        /// <unmanaged>HRESULT D3DDecompressShaders([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] unsigned int uNumShaders,[In] unsigned int uStartIndex,[In, Buffer, Optional] unsigned int* pIndices,[In] unsigned int uFlags,[Out, Buffer] ID3D10Blob** ppShaders,[Out, Optional] unsigned int* pTotalShaders)</unmanaged>	
        public unsafe ShaderBytecode[] Decompress(int startIndex, int numShaders)
        {
            if (numShaders == 0)
                return null;
            var shadersBlobs = new Blob[numShaders];
            int totalShadersRef;
            fixed (void* bufferPtr = Data)
                D3D.DecompressShaders((IntPtr)bufferPtr, Data.Length, numShaders, startIndex, null, 0, shadersBlobs, out totalShadersRef);

            //The size of shadersBlobs will not change
            //if the compressed shader contains less than requested in numShaders, null entries will appear in the result array
            var shadersByteArr = new ShaderBytecode[shadersBlobs.Length];
            for (int i = 0; i < shadersBlobs.Length; i++)
                if (shadersBlobs[i] != null)
                    shadersByteArr[i] = new ShaderBytecode(shadersBlobs[i]);

            return shadersByteArr;
        }

        /// <summary>	
        /// Decompresses one or more shaders from a compressed set.	
        /// </summary>	
        /// <param name="indices"><para>An array of indexes that represent the shaders to decompress.</para></param>	
        /// <returns>Returns an array of decompress shader bytecode.</returns>	
        /// <unmanaged>HRESULT D3DDecompressShaders([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] unsigned int uNumShaders,[In] unsigned int uStartIndex,[In, Buffer, Optional] unsigned int* pIndices,[In] unsigned int uFlags,[Out, Buffer] ID3D10Blob** ppShaders,[Out, Optional] unsigned int* pTotalShaders)</unmanaged>	
        public unsafe ShaderBytecode[] Decompress(int[] indices)
        {
            if (indices.Length == 0)
                return null;
            var shadersBlobs = new Blob[indices.Length];
            int totalShadersRef;
            fixed (void* bufferPtr = Data)
                D3D.DecompressShaders((IntPtr)bufferPtr, Data.Length, indices.Length, 0, indices, 0, shadersBlobs, out totalShadersRef);

            //The size of shadersBlobs will not change
            //if the compressed shader contains less than requested in numShaders, null entries will appear in the result array
            var shadersByteArr = new ShaderBytecode[shadersBlobs.Length];
            for (int i = 0; i < shadersBlobs.Length; i++)
                if (shadersBlobs[i] != null)
                    shadersByteArr[i] = new ShaderBytecode(shadersBlobs[i]);

            return shadersByteArr;
        }

#endif

        /// <summary>
        /// Gets this instance is composed of compressed shaders.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is compressed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompressed
        {
            get;
            private set;
        }

        /// <summary>
        ///   Disassembles compiled HLSL code back into textual source.
        /// </summary>
        /// <returns>The textual source of the shader or effect.</returns>
        public string Disassemble()
        {
            return this.Disassemble(DisassemblyFlags.None, null);
        }

        /// <summary>
        ///   Disassembles compiled HLSL code back into textual source.
        /// </summary>
        /// <param name = "flags">Flags affecting the output of the disassembly.</param>
        /// <returns>The textual source of the shader or effect.</returns>
        public string Disassemble(DisassemblyFlags flags)
        {
            return this.Disassemble(flags, null);
        }

        /// <summary>
        ///   Disassembles compiled HLSL code back into textual source.
        /// </summary>
        /// <param name = "flags">Flags affecting the output of the disassembly.</param>
        /// <param name = "comments">Commenting information to embed in the disassembly.</param>
        /// <returns>The textual source of the shader or effect.</returns>
        public unsafe string Disassemble(DisassemblyFlags flags, string comments)
        {
            Blob output;
            fixed (void* bufferPtr = Data)
                D3D.Disassemble((IntPtr)bufferPtr, Data.Length, flags, comments, out output);
            return Utilities.BlobToString(output);
        }


        /// <summary>
        ///   Disassembles a region of a compiled HLSL code back into textual source.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="comments">The comments.</param>
        /// <param name="startByteOffset">The start byte offset.</param>
        /// <param name="numberOfInstructions">The number of instructions.</param>
        /// <param name="finishByteOffsetRef">The finish byte offset ref.</param>
        /// <returns>The textual source of the shader or effect.</returns>
        /// <unmanaged>HRESULT D3DDisassembleRegion([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] unsigned int Flags,[In, Optional] const char* szComments,[In] SIZE_T StartByteOffset,[In] SIZE_T NumInsts,[Out, Optional] SIZE_T* pFinishByteOffset,[Out] ID3D10Blob** ppDisassembly)</unmanaged>	
        public unsafe string DisassembleRegion(DisassemblyFlags flags, string comments, PointerSize startByteOffset, PointerSize numberOfInstructions, out SharpDX.PointerSize finishByteOffsetRef)
        {
            Blob output;
            fixed (void* bufferPtr = Data)
                D3D.DisassembleRegion((IntPtr)bufferPtr, Data.Length, (int)flags, comments, startByteOffset, numberOfInstructions, out finishByteOffsetRef, out output);
            return Utilities.BlobToString(output);
        }


        /// <summary>
        /// Gets the trace instruction offsets.
        /// </summary>
        /// <param name="isIncludingNonExecutableCode">if set to <c>true</c> [is including non executable code].</param>
        /// <param name="startInstIndex">Start index of the instructions.</param>
        /// <param name="numInsts">The number of instructions.</param>
        /// <param name="totalInstsRef">The total instructions ref.</param>
        /// <returns>An offset</returns>
        /// <unmanaged>HRESULT D3DGetTraceInstructionOffsets([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] unsigned int Flags,[In] SIZE_T StartInstIndex,[In] SIZE_T NumInsts,[Out, Buffer, Optional] SIZE_T* pOffsets,[Out, Optional] SIZE_T* pTotalInsts)</unmanaged>
        public unsafe PointerSize GetTraceInstructionOffsets(bool isIncludingNonExecutableCode, PointerSize startInstIndex, PointerSize numInsts, out SharpDX.PointerSize totalInstsRef)
        {
            fixed (void* bufferPtr = Data)
                return D3D.GetTraceInstructionOffsets((IntPtr)bufferPtr, Data.Length, isIncludingNonExecutableCode ? 1 : 0, startInstIndex, numInsts, out totalInstsRef);
        }

        /// <summary>	
        /// Retrieves a specific part from a compilation result.	
        /// </summary>	
        /// <remarks>	
        /// D3DGetBlobPart retrieves the part of a blob (arbitrary length data buffer) that contains the type of data that the  Part parameter specifies. 	
        /// </remarks>	
        /// <param name="part">A <see cref="SharpDX.D3DCompiler.ShaderBytecodePart"/>-typed value that specifies the part of the buffer to retrieve. </param>
        /// <returns>Returns the extracted part. </returns>
        /// <unmanaged>HRESULT D3DGetBlobPart([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] D3D_BLOB_PART Part,[In] int Flags,[Out] ID3DBlob** ppPart)</unmanaged>
        public unsafe ShaderBytecode GetPart(ShaderBytecodePart part)
        {
            Blob blob;
            fixed (void* bufferPtr = Data)
                D3D.GetBlobPart((IntPtr)bufferPtr, Data.Length, part, 0, out blob);
            return new ShaderBytecode(blob);
        }

        /// <summary>
        /// Sets information in a compilation result.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="partData">The part data.</param>
        /// <returns>The new shader in which the new part data is set.</returns>
        /// <unmanaged>HRESULT D3DSetBlobPart([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] D3D_BLOB_PART Part,[In] unsigned int Flags,[In, Buffer] const void* pPart,[In] SIZE_T PartSize,[Out] ID3D10Blob** ppNewShader)</unmanaged>
        public unsafe ShaderBytecode SetPart(ShaderBytecodePart part, DataStream partData)
        {
            Blob blob;
            fixed (void* bufferPtr = Data)
                D3D.SetBlobPart((IntPtr)bufferPtr, Data.Length, part, 0, partData.DataPointer, (int)partData.Length, out blob);
            return new ShaderBytecode(blob);
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
        /// Saves this bytecode to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            if (Data.Length == 0) return;

            stream.Write(Data, 0, Data.Length);
        }

        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">A string containing the source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(string shaderSource, ShaderMacro[] defines = null, Include include = null, string sourceFileName = "")
        {
            string errors = null;
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Preprocess(shaderSourcePtr, shaderSource.Length, defines, include, out errors, sourceFileName);
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
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(byte[] shaderSource, ShaderMacro[] defines = null, Include include = null, string sourceFileName = "")
        {
            string errors = null;
            return Preprocess(shaderSource, defines, include, out errors, sourceFileName);
        }

        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">An array of bytes containing the raw source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(byte[] shaderSource, ShaderMacro[] defines, Include include, out string compilationErrors, string sourceFileName = "")
        {
            unsafe
            {
                fixed (void* pData = &shaderSource[0])
                    return Preprocess((IntPtr)pData, shaderSource.Length, defines, include, out compilationErrors, sourceFileName);
            }
        }


        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSourcePtr">An array of bytes containing the raw source of the shader or effect to preprocess.</param>
        /// <param name = "shaderSourceLength"></param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(IntPtr shaderSourcePtr, int shaderSourceLength, ShaderMacro[] defines, Include include, out string compilationErrors, string sourceFileName = "")
        {
            unsafe
            {
                Blob blobForText = null;
                Blob blobForErrors = null;
                compilationErrors = null;

                try
                {
                    D3D.Preprocess(shaderSourcePtr, shaderSourceLength, sourceFileName, PrepareMacros(defines),include,
                                    out blobForText, out blobForErrors);
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
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(string shaderSource, ShaderMacro[] defines, Include include, out string compilationErrors, string sourceFileName = "")
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            var shaderSourcePtr = Marshal.StringToHGlobalAnsi(shaderSource);
            try
            {
                return Preprocess(shaderSourcePtr, shaderSource.Length, defines, include, out compilationErrors, sourceFileName);
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
        public static string PreprocessFromFile(string fileName)
        {
            string errors = null;
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            string str = NativeFile.ReadAllText(fileName);
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("fileName");
            }
            return Preprocess(Encoding.ASCII.GetBytes(str), null, null, out errors, fileName);
        }

        /// <summary>
        ///   Preprocesses a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string PreprocessFromFile(string fileName, ShaderMacro[] defines, Include include)
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
        public static string PreprocessFromFile(string fileName, ShaderMacro[] defines, Include include,
                                                out string compilationErrors)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            return Preprocess(NativeFile.ReadAllText(fileName), defines, include, out compilationErrors);
        }

        /// <summary>
        ///   Strips extraneous information from a compiled shader or effect.
        /// </summary>
        /// <param name = "flags">Options specifying what to remove from the shader.</param>
        /// <returns>A string containing any errors that may have occurred.</returns>
        /// <unmanaged>HRESULT D3DStripShader([In, Buffer] const void* pShaderBytecode,[In] SIZE_T BytecodeLength,[In] D3DCOMPILER_STRIP_FLAGS uStripFlags,[Out] ID3D10Blob** ppStrippedBlob)</unmanaged>
        public unsafe ShaderBytecode Strip(StripFlags flags)
        {
            Blob blob;
            fixed (void* bufferPtr = Data)
                if (D3D.StripShader((IntPtr)bufferPtr, Data.Length, flags, out blob).Failure)
                    return null;
            return new ShaderBytecode(blob);
        }

        /// <summary>
        /// Cast this <see cref="ShaderBytecode"/> to the underlying byte buffer.
        /// </summary>
        /// <param name="shaderBytecode"></param>
        /// <returns>A byte buffer</returns>
        public static implicit operator byte[](ShaderBytecode shaderBytecode)
        {
            return shaderBytecode.Data;
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
            return new ShaderBytecode(NativeFile.ReadAllBytes(fileName));
        }

        internal static ShaderMacro[] PrepareMacros(ShaderMacro[] macros)
        {
            if (macros == null)
                return null;

            if (macros.Length == 0)
                return null;

            if (macros[macros.Length - 1].Name == null && macros[macros.Length - 1].Definition == null)
                return macros;

            var macroArray = new ShaderMacro[macros.Length + 1];

            Array.Copy(macros, macroArray, macros.Length);

            macroArray[macros.Length] = new ShaderMacro(null, null);
            return macroArray;
        }

        /// <summary>
        /// Disposes all resource for the allocated object
        /// </summary>
        /// <remarks>This is kept for backwards compatibility only, this actually does nothing in that case</remarks>
        public void Dispose()
        {
            // Just to keep backward compatibility
        }

        /// <summary>
        /// Gets the shader type and version string from the provided bytecode.
        /// </summary>
        /// <returns>The type and version string of the provided shader bytecode.</returns>
        /// <exception cref="ArgumentException">Is thrown when bytecode contains invalid data or the version could not be read.</exception>
        /// <exception cref="IndexOutOfRangeException">Is thrown when bytecode contains invalid data.</exception>
        public ShaderProfile GetVersion()
        {
            // the offset where chunks count is stored
            const int chunksCountOffset = 4 * 7;
            // the offset of the version bytes from the start of the chunk
            const int shaderCodeVersionOffset = 4 * 2;
            // invalid offset marker - used to find out if the shader version cannot be read
            const int invalidOffset = -1;

            var data = Data;

            // read the number of data chunks in the bytecode
            var chunksCount = BitConverter.ToUInt32(data, chunksCountOffset);

            // find the offset of the chunk where we can read the data:
            var chunkOffset = invalidOffset;
            var dx9ChunkOffset = invalidOffset;
            for (var i = 0; i < chunksCount; i++)
            {
                var offset = BitConverter.ToInt32(data, chunksCountOffset + 4 + 4 * i);
                var code = (FourCC)BitConverter.ToUInt32(data, offset);
                // these chunks contain the shader version
                if (code == "SHEX" || code == "SHDR")
                    chunkOffset = offset;

                // this chunk is present only for d3d9-level hardware
                if (code == "Aon9")
                    dx9ChunkOffset = offset;
            }

            if (chunkOffset == invalidOffset)
                throw new ArgumentException("Cannot find the chunk with version in provided bytecode");

            // read the shader version
            var versionValue = BitConverter.ToInt32(data, chunkOffset + shaderCodeVersionOffset);
            var minor = DecodeValue(versionValue, 0, 3);
            var major = DecodeValue(versionValue, 4, 7);
            var type = (ShaderVersion)DecodeValue(versionValue, 16, 31);

            var profileMinor = 0;
            var profileMajor = 0;

            // we have a DX9 chunk here - try to decode profile level version
            // this is possible only for SM 4.0
            if (dx9ChunkOffset != invalidOffset && major == 4 && minor == 0)
            {
                // http://msdn.microsoft.com/en-us/library/ff570156%28v=vs.85%29.aspx
                // 0xFFFE - Vertex Shader
                // 0xFFFF - Pixel Shader
                // order is inversed due to endianess
                byte magicByte1 = (byte)(type == ShaderVersion.VertexShader ? 0xfe : 0xff);
                byte magicByte2 = 0xff;

                // skip 16 bytes (4 dwords): FourCC, ChunkSize,
                // other 2 are some magic numbers that may mess with the VersionToken detection
                var startOffset = dx9ChunkOffset + 16;
                // chunk size is the second dword at the chunk offset
                var endOffset = dx9ChunkOffset + 8 + BitConverter.ToUInt32(data, dx9ChunkOffset + 4);
                for (var j = startOffset; j < endOffset; j += 4)
                {
                    if (data[j + 2] == magicByte1 && data[j + 3] == magicByte2)
                    {
                        // first byte is the minor version, 0 maps to 4_0_level_9_1 and 1 maps to 4_0_level_9_3
                        profileMinor = data[j] == 1 ? 3 : 1;

                        System.Diagnostics.Debug.Assert(data[j] == 0 || data[j] == 1);
                        System.Diagnostics.Debug.Assert(data[j + 1] == 2);
                        break;
                    }
                }

                profileMajor = 9;
            }

            // just make sure we computed everything correctly
            System.Diagnostics.Debug.Assert((profileMajor == 9 && (profileMinor == 1 || profileMinor == 3)) || (profileMajor == 0 && profileMinor == 0));

            return new ShaderProfile(type, major, minor, profileMajor, profileMinor);
        }

        /// <summary>
        /// Reads the value between start and end bits from the provided token.
        /// </summary>
        /// <param name="token">The source of the data to read.</param>
        /// <param name="start">The start bit.</param>
        /// <param name="end">The end bit.</param>
        /// <returns></returns>
        private static int DecodeValue(int token, int start, int end)
        {
            // create the mask to read the data
            var mask = 0;
            for (var i = start; i <= end; i++)
                mask |= 1 << i;

            // read the needed bits and shift them accordingly
            return (token & mask) >> start;
        }
    }
}