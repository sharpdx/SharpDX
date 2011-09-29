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

namespace SharpDX.D3DCompiler
{
    /// <summary>
    ///   Represents the compiled bytecode of a shader or effect.
    /// </summary>
    /// <unmanaged>Blob</unmanaged>
    public class ShaderBytecode : DisposeBase
    {
        private bool isOwner;
        private Blob blob;

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
            CreateFromPointer(data.DataPointer, (int) data.Length);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderBytecode" /> class.
        /// </summary>
        /// <param name = "data">A <see cref = "T:System.IO.Stream" /> containing the compiled bytecode.</param>
        public ShaderBytecode(Stream data)
        {
            int size = (int) (data.Length - data.Position);

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
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.ShaderBytecode" /> class.
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

        private void CreateFromBuffer(byte[] buffer)
        {
            unsafe
            {
                fixed (void* pBuffer = &buffer[0])
                    CreateFromPointer((IntPtr) pBuffer, buffer.Length);
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
        public SharpDX.Size BufferSize { get; set; }

#if WIN8
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
        public static ShaderBytecode Compile(string shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
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
        public static ShaderBytecode Compile(string shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
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
        public static ShaderBytecode Compile(string shaderSource, string profile, ShaderFlags shaderFlags,
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
        public static ShaderBytecode Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags,
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
        public static ShaderBytecode Compile(string shaderSource, string entryPoint, string profile,
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
        public static ShaderBytecode Compile(byte[] shaderSource, string entryPoint, string profile,
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
        public static ShaderBytecode Compile(IntPtr textSource, int textSize, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, string sourceFileName = "unknown", SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            unsafe
            {
                ShaderBytecode bytecode;

                Blob blobForCode = null;
                Blob blobForErrors = null;

                try
                {
                    D3D.Compile2(
                        (IntPtr)textSource,
                        textSize,
                        sourceFileName,
                        PrepareMacros(defines),
                        IncludeShadow.ToIntPtr(include),
                        entryPoint,
                        profile,
                        shaderFlags,
                        effectFlags,
                        secondaryDataFlags,
                        secondaryData != null ? secondaryData.DataPointer : IntPtr.Zero,
                        secondaryData != null ? (int)secondaryData.Length : 0,
                        out blobForCode,
                        out blobForErrors);
                }
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                    {
                        var compilationErrors = Utilities.BlobToString(blobForErrors);
                        throw new CompilationException(ex.ResultCode, compilationErrors);
                    }
                    throw;
                }

                bytecode = new ShaderBytecode(blobForCode);

                return bytecode;
            }
        }
#else

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags,
                                                     EffectFlags effectFlags, SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, null, null, out compilationErrors, secondaryDataFlags, secondaryData);
        }

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode CompileFromFile(string fileName, string entryPoint, string profile,
                                                     ShaderFlags shaderFlags, EffectFlags effectFlags, SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, entryPoint, profile, shaderFlags, effectFlags, null, null,
                                   out compilationErrors, secondaryDataFlags, secondaryData);
        }

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags,
                                                     EffectFlags effectFlags, ShaderMacro[] defines, Include include, SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, defines, include,
                                   out compilationErrors, secondaryDataFlags, secondaryData);
        }

        /// <summary>
        /// Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name="fileName">The name of the source file to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="defines">A set of macros to define during compilation.</param>
        /// <param name="include">An interface for handling include files.</param>
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags,
                                                     EffectFlags effectFlags, ShaderMacro[] defines, Include include,
                                                     out string compilationErrors, SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, defines, include,
                                   out compilationErrors, secondaryDataFlags, secondaryData);
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
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode CompileFromFile(string fileName, string entryPoint, string profile,
                                                     ShaderFlags shaderFlags, EffectFlags effectFlags,
                                                     ShaderMacro[] defines, Include include, SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, entryPoint, profile, shaderFlags, effectFlags, defines, include,
                                   out compilationErrors, secondaryDataFlags, secondaryData);
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
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <param name="secondaryDataFlags">The secondary data flags.</param>
        /// <param name="secondaryData">The secondary data.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode CompileFromFile(string fileName, string entryPoint, string profile,
                                                     ShaderFlags shaderFlags, EffectFlags effectFlags,
                                                     ShaderMacro[] defines, Include include,
                                                     out string compilationErrors, SecondaryDataFlags secondaryDataFlags = SecondaryDataFlags.None, DataStream secondaryData = null)
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
            return Compile(File.ReadAllText(fileName), entryPoint, profile, shaderFlags, effectFlags,
                           PrepareMacros(defines), include, out compilationErrors, fileName, secondaryDataFlags, secondaryData);
        }
        
        /// <summary>
        /// Compiles the provided shader or effect source.
        /// </summary>
        /// <param name="shaderSource">A string containing the source of the shader or effect to compile.</param>
        /// <param name="profile">The shader target or set of shader features to compile against.</param>
        /// <param name="shaderFlags">Shader compilation options.</param>
        /// <param name="effectFlags">Effect compilation options.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(string shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), null, profile, shaderFlags, effectFlags, null, null,
                           out compilationErrors, sourceFileName);
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
        public static ShaderBytecode Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            return Compile(shaderSource, null, profile, shaderFlags, effectFlags, null, null, out compilationErrors, sourceFileName);
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
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(string shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), entryPoint, profile, shaderFlags, effectFlags, null,
                           null, out compilationErrors, sourceFileName);
        }

        /// <summary>
        ///   Compiles the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">An array of bytes containing the raw source of the shader or effect to compile.</param>
        /// <param name = "entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static ShaderBytecode Compile(byte[] shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            return Compile(shaderSource, entryPoint, profile, shaderFlags, effectFlags, null, null,
                           out compilationErrors, sourceFileName);
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
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(string shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, ShaderMacro[] defines, Include include, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), null, profile, shaderFlags, effectFlags, defines,
                           include, out compilationErrors, sourceFileName);
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
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, ShaderMacro[] defines, Include include, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            return Compile(shaderSource, null, profile, shaderFlags, effectFlags, defines, include,
                           out compilationErrors, sourceFileName);
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
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(string shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, ShaderMacro[] defines, Include include,
                                             out string compilationErrors, string sourceFileName = "unknown")
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), null, profile, shaderFlags, effectFlags, defines,
                           include, out compilationErrors, sourceFileName);
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
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(byte[] shaderSource, string profile, ShaderFlags shaderFlags,
                                             EffectFlags effectFlags, ShaderMacro[] defines, Include include,
                                             out string compilationErrors, string sourceFileName = "unknown")
        {
            return Compile(shaderSource, null, profile, shaderFlags, effectFlags, defines, include,
                           out compilationErrors, sourceFileName);
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
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(string shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), entryPoint, profile, shaderFlags, effectFlags, defines,
                           include, out compilationErrors, sourceFileName);
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
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(byte[] shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, string sourceFileName = "unknown")
        {
            string compilationErrors = null;
            return Compile(shaderSource, entryPoint, profile, shaderFlags, effectFlags, defines, include,
                           out compilationErrors);
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
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <param name="sourceFileName">Name of the source file.</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(string shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, out string compilationErrors, string sourceFileName = "unknown")
        {
            if (string.IsNullOrEmpty(shaderSource))
            {
                throw new ArgumentNullException("shaderSource");
            }
            return Compile(Encoding.ASCII.GetBytes(shaderSource), entryPoint, profile, shaderFlags, effectFlags, defines,
                           include, out compilationErrors, sourceFileName);
        }

        /// <summary>
        ///   Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static ShaderBytecode CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags,
                                                     EffectFlags effectFlags)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, null, null, out compilationErrors);
        }

        /// <summary>
        ///   Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static ShaderBytecode CompileFromFile(string fileName, string entryPoint, string profile,
                                                     ShaderFlags shaderFlags, EffectFlags effectFlags)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, entryPoint, profile, shaderFlags, effectFlags, null, null,
                                   out compilationErrors);
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
        public static ShaderBytecode CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags,
                                                     EffectFlags effectFlags, ShaderMacro[] defines, Include include)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, defines, include,
                                   out compilationErrors);
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
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static ShaderBytecode CompileFromFile(string fileName, string profile, ShaderFlags shaderFlags,
                                                     EffectFlags effectFlags, ShaderMacro[] defines, Include include,
                                                     out string compilationErrors)
        {
            return CompileFromFile(fileName, null, profile, shaderFlags, effectFlags, defines, include,
                                   out compilationErrors);
        }

        /// <summary>
        ///   Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <param name = "defines">A set of macros to define during compilation.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static ShaderBytecode CompileFromFile(string fileName, string entryPoint, string profile,
                                                     ShaderFlags shaderFlags, EffectFlags effectFlags,
                                                     ShaderMacro[] defines, Include include)
        {
            string compilationErrors = null;
            return CompileFromFile(fileName, entryPoint, profile, shaderFlags, effectFlags, defines, include,
                                   out compilationErrors);
        }

        /// <summary>
        ///   Compiles a shader or effect from a file on disk.
        /// </summary>
        /// <param name = "fileName">The name of the source file to compile.</param>
        /// <param name = "entryPoint">The name of the shader entry-point function, or <c>null</c> for an effect file.</param>
        /// <param name = "profile">The shader target or set of shader features to compile against.</param>
        /// <param name = "shaderFlags">Shader compilation options.</param>
        /// <param name = "effectFlags">Effect compilation options.</param>
        /// <param name = "defines">A set of macros to define during compilation.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <returns>The compiled shader bytecode, or <c>null</c> if the method fails.</returns>
        public static ShaderBytecode CompileFromFile(string fileName, string entryPoint, string profile,
                                                     ShaderFlags shaderFlags, EffectFlags effectFlags,
                                                     ShaderMacro[] defines, Include include,
                                                     out string compilationErrors)
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
            return Compile(File.ReadAllText(fileName), entryPoint, profile, shaderFlags, effectFlags,
                           PrepareMacros(defines), include, out compilationErrors, fileName);
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
        /// <param name="compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if compilation succeeded.</param>
        /// <param name="sourceFileName">Name of the source file used for reporting errors. Default is "unknown"</param>
        /// <returns>
        /// The compiled shader bytecode, or <c>null</c> if the method fails.
        /// </returns>
        public static ShaderBytecode Compile(byte[] shaderSource, string entryPoint, string profile,
                                             ShaderFlags shaderFlags, EffectFlags effectFlags, ShaderMacro[] defines,
                                             Include include, out string compilationErrors, string sourceFileName = "unknown")
        {
            unsafe
            {
                ShaderBytecode bytecode;

                Blob blobForCode = null;
                Blob blobForErrors = null;
                compilationErrors = null;

                CompilationException compilationException = null;

                try
                {
                    if ((shaderFlags & Effect10) != 0)
                    {
                        shaderFlags ^= Effect10;

                        fixed (void* pData = &shaderSource[0])
                            D3D.CompileEffect10FromMemory(
                                (IntPtr)pData,
                                shaderSource.Length,
                                sourceFileName,
                                PrepareMacros(defines),
                                IncludeShadow.ToIntPtr(include),
                                shaderFlags,
                                effectFlags,
                                out blobForCode,
                                out blobForErrors);
                    }
                    else
                    {
                        fixed (void* pData = &shaderSource[0])
                            D3D.Compile(
                                (IntPtr)pData,
                                shaderSource.Length,
                                sourceFileName,
                                PrepareMacros(defines),
                                IncludeShadow.ToIntPtr(include),
                                entryPoint,
                                profile,
                                shaderFlags,
                                effectFlags,
                                out blobForCode,
                                out blobForErrors);
                    }
                }
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                    {
                        compilationErrors = Utilities.BlobToString(blobForErrors);
                        compilationException = CompilationException.Check(ex.ResultCode, compilationErrors);
                    }
                    else
                    {
                        ex.Data.Add("CompilationErrors", compilationErrors);
                        throw;
                    }
                }

                if (compilationException != null)
                    throw compilationException;

                bytecode = new ShaderBytecode(blobForCode);

                return bytecode;
            }
        }
#endif

        /// <summary>	
        /// Compresses a set of shaders into a more compact form. 	
        /// </summary>	
        /// <param name="shaderBytecodes">An array of <see cref="SharpDX.D3DCompiler.ShaderBytecode"/> structures that describe the set of shaders to compress. </param>
        /// <returns>A compressed <see cref="SharpDX.D3DCompiler.ShaderBytecode"/>. </returns>
        /// <unmanaged>HRESULT D3DCompressShaders([In] int uNumShaders,[In, Buffer] D3D_SHADER_DATA* pShaderData,[In] int uFlags,[Out] ID3DBlob** ppCompressedData)</unmanaged>
        public static ShaderBytecode Compress(ShaderBytecode[] shaderBytecodes)
        {
            // D3D.CompressShaders()
            ShaderData[] temp = new ShaderData[shaderBytecodes.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = new ShaderData {BytecodePtr = shaderBytecodes[i].BufferPointer, BytecodeLength = shaderBytecodes[i].BufferSize};
            }
            Blob blob;
            D3D.CompressShaders(shaderBytecodes.Length, temp, 1, out blob);
            var result = new ShaderBytecode(blob);
            result.IsCompressed = true;
            return result;
        }

        // TODO Decompress, how to use it?
        //public ShaderBytecode[] Decompress()
        //{
        //    D3D.DecompressShaders(BufferPointer, BufferSize, ??? )
        //}

        /// <summary>
        /// Gets this instance is composed of compressed shaders.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is compressed; otherwise, <c>false</c>.
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
        public string Disassemble(DisassemblyFlags flags, string comments)
        {
            Blob output;
            D3D.Disassemble(BufferPointer, BufferSize, flags, comments, out output);
            return Utilities.BlobToString(output);
        }

#if WIN8
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
        public string DisassembleRegion(DisassemblyFlags flags, string comments, Size startByteOffset, Size numberOfInstructions, out SharpDX.Size finishByteOffsetRef)
        {
            Blob output;
            D3D.DisassembleRegion(BufferPointer, BufferSize, (int)flags, comments, startByteOffset, numberOfInstructions, out finishByteOffsetRef, out output);
            return Utilities.BlobToString(output);
        }


        /// <summary>
        /// Gets the trace instruction offsets.
        /// </summary>
        /// <param name="isIncludingNonExecutableCode">if set to <c>true</c> [is including non executable code].</param>
        /// <param name="startInstIndex">Start index of the inst.</param>
        /// <param name="numInsts">The num insts.</param>
        /// <param name="totalInstsRef">The total insts ref.</param>
        /// <returns>An offset</returns>
        /// <unmanaged>HRESULT D3DGetTraceInstructionOffsets([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] unsigned int Flags,[In] SIZE_T StartInstIndex,[In] SIZE_T NumInsts,[Out, Buffer, Optional] SIZE_T* pOffsets,[Out, Optional] SIZE_T* pTotalInsts)</unmanaged>
        public Size GetTraceInstructionOffsets(bool isIncludingNonExecutableCode, Size startInstIndex, Size numInsts, out SharpDX.Size totalInstsRef)
        {
            return D3D.GetTraceInstructionOffsets(BufferPointer, BufferSize, isIncludingNonExecutableCode ? 1 : 0, startInstIndex, numInsts, out totalInstsRef);
        }
#endif

        /// <summary>	
        /// Retrieves a specific part from a compilation result.	
        /// </summary>	
        /// <remarks>	
        /// D3DGetBlobPart retrieves the part of a blob (arbitrary length data buffer) that contains the type of data that the  Part parameter specifies. 	
        /// </remarks>	
        /// <param name="part">A <see cref="SharpDX.D3DCompiler.ShaderBytecodePart"/>-typed value that specifies the part of the buffer to retrieve. </param>
        /// <returns>Returns the extracted part. </returns>
        /// <unmanaged>HRESULT D3DGetBlobPart([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] D3D_BLOB_PART Part,[In] int Flags,[Out] ID3DBlob** ppPart)</unmanaged>
        public ShaderBytecode GetPart(ShaderBytecodePart part)
        {
            Blob blob;
            D3D.GetBlobPart(this.BufferPointer, this.BufferSize, part, 0, out blob);
            return new ShaderBytecode(blob);
        }

#if WIN8
        /// <summary>
        /// Sets information in a compilation result.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="partData">The part data.</param>
        /// <returns>The new shader in which the new part data is set.</returns>
        /// <unmanaged>HRESULT D3DSetBlobPart([In, Buffer] const void* pSrcData,[In] SIZE_T SrcDataSize,[In] D3D_BLOB_PART Part,[In] unsigned int Flags,[In, Buffer] const void* pPart,[In] SIZE_T PartSize,[Out] ID3D10Blob** ppNewShader)</unmanaged>
        public ShaderBytecode SetPart(ShaderBytecodePart part, DataStream partData)
        {
            Blob blob;
            D3D.SetBlobPart(BufferPointer, BufferSize, part, 0, partData.DataPointer, (int)partData.Length, out blob);
            return new ShaderBytecode(blob);
        }
#endif

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
        /// Saves this bycode to the specified stream.
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
        /// Froms the pointer.
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
        public static string Preprocess(string shaderSource, ShaderMacro[] defines = null, Include include = null)
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
        public static string Preprocess(byte[] shaderSource, ShaderMacro[] defines = null, Include include = null)
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
        public static string Preprocess(byte[] shaderSource, ShaderMacro[] defines, Include include, out string compilationErrors)
        {
            unsafe
            {
                fixed (void* pData = &shaderSource[0])
                    return Preprocess((IntPtr)pData, shaderSource.Length, defines, include, out compilationErrors);
            }
        }


        /// <summary>
        ///   Preprocesses the provided shader or effect source.
        /// </summary>
        /// <param name = "shaderSource">An array of bytes containing the raw source of the shader or effect to preprocess.</param>
        /// <param name = "defines">A set of macros to define during preprocessing.</param>
        /// <param name = "include">An interface for handling include files.</param>
        /// <param name = "compilationErrors">When the method completes, contains a string of compilation errors, or an empty string if preprocessing succeeded.</param>
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(IntPtr shaderSourcePtr, int shaderSourceLength, ShaderMacro[] defines, Include include,
                                        out string compilationErrors)
        {
            unsafe
            {
                Blob blobForText = null;
                Blob blobForErrors = null;
                compilationErrors = null;

                try
                {
                    D3D.Preprocess(shaderSourcePtr, shaderSourceLength, "", PrepareMacros(defines), IncludeShadow.ToIntPtr(include),
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
        /// <returns>The preprocessed shader source.</returns>
        public static string Preprocess(string shaderSource, ShaderMacro[] defines, Include include, out string compilationErrors)
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
#if !WIN8
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
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not open the shader or effect file.", fileName);
            }
            string str = File.ReadAllText(fileName);
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("shaderSource");
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
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Could not open the shader or effect file.", fileName);
            }
            return Preprocess(File.ReadAllText(fileName), defines, include, out compilationErrors);
        }
#endif
        /// <summary>
        ///   Strips extraneous information from a compiled shader or effect.
        /// </summary>
        /// <param name = "flags">Options specifying what to remove from the shader.</param>
        /// <returns>A string containing any errors that may have occurred.</returns>
        public ShaderBytecode Strip(StripFlags flags)
        {
            try
            {
                Blob blob;
                D3D.StripShader(BufferPointer, BufferSize, flags, out blob);
                return new ShaderBytecode(blob);
            }
            catch (SharpDXException)
            {
                return null;
            }
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
#if !WIN8
        /// <summary>
        ///   Read a compiled shader bytecode from a Stream and return a ShaderBytecode
        /// </summary>
        /// <param name = "fileName"></param>
        /// <returns></returns>
        public static ShaderBytecode FromFile(string fileName)
        {
            return new ShaderBytecode(File.ReadAllBytes(fileName));
        }
#endif
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (blob != null)
                {
                    blob.Dispose();
                    blob = null;
                }
            }
            if (isOwner && BufferPointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(BufferPointer);
                BufferPointer = IntPtr.Zero;
                BufferSize = 0;
            }
        }
    }
}