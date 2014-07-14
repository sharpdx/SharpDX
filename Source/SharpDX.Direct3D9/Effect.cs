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
    public partial class Effect
    {
        /// <summary>
        /// Starts an active technique.
        /// </summary>
        /// <returns>The number of passes needed to render the current technique. </returns>
        /// <unmanaged>HRESULT ID3DXEffect::Begin([Out] unsigned int* pPasses,[In] D3DXFX Flags)</unmanaged>
        public int Begin()
        {
            return Begin(FX.None);
        }

        /// <summary>
        /// Gets or sets the current technique.
        /// </summary>
        /// <value>
        /// The technique.
        /// </value>
        /// <unmanaged>D3DXHANDLE ID3DXEffect::GetCurrentTechnique()</unmanaged>	
        /// <unmanaged>HRESULT ID3DXEffect::SetTechnique([In] D3DXHANDLE hTechnique)</unmanaged>	
        public EffectHandle Technique
        {
            get
            {
                return GetCurrentTechnique();
            }

            set
            {
                SetTechnique(value);
            }
        }

        /// <summary>
        /// Compiles an effect from a file.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromFile(Device device, string fileName, ShaderFlags flags)
        {
            return FromFile(device, fileName, null, null, null, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a file.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromFile(Device device, string fileName, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags)
        {
            return FromFile(device, fileName, preprocessorDefines, includeFile, skipConstants, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a file.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromFile(Device device, string fileName, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            return FromString(device, File.ReadAllText(fileName), preprocessorDefines, includeFile, skipConstants, flags, pool);
        }

        /// <summary>
        /// Compiles an effect from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="memory">The buffer.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromMemory(Device device, byte[] memory, ShaderFlags flags)
        {
            return FromMemory(device, memory, null, null, null, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="memory">The buffer.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromMemory(Device device, byte[] memory, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags)
        {
            return FromMemory(device, memory, preprocessorDefines, includeFile, skipConstants, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a memory buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="memory">The buffer.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>An <see cref="Effect"/></returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromMemory(Device device, byte[] memory, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
           unsafe
            {
                Effect effect = null;
                Blob blobForErrors = null;

                try
                {
                    fixed (void* pData = memory)
                        D3DX9.CreateEffectEx(
                            device,
                            (IntPtr)pData,
                            memory.Length,
                            PrepareMacros(preprocessorDefines),
                            IncludeShadow.ToIntPtr(includeFile),
                            skipConstants,
                            (int)flags,
                            pool,
                            out effect,
                            out blobForErrors);
                }
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                        throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                    throw;
                }
                return effect;
            }
        }

        /// <summary>
        /// Compiles an effect from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromStream(Device device, Stream stream, ShaderFlags flags)
        {
            return FromStream(device, stream, null, null, null, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromStream(Device device, Stream stream, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags)
        {
            return FromStream(device, stream, preprocessorDefines, includeFile, skipConstants, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a stream.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>An <see cref="Effect"/></returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromStream(Device device, Stream stream, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            unsafe
            {
                Effect effect = null;
                Blob blobForErrors = null;

                try
                {
                    if (stream is DataStream)
                    {
                        D3DX9.CreateEffectEx(
                            device,
                            ((DataStream)stream).PositionPointer,
                            (int)(stream.Length - stream.Position),
                            PrepareMacros(preprocessorDefines),
                            IncludeShadow.ToIntPtr(includeFile),
                            skipConstants,
                            (int)flags,
                            pool,
                            out effect,
                            out blobForErrors);
                    } 
                    else
                    {
                        var data = Utilities.ReadStream(stream);
                        fixed (void* pData = data)
                        D3DX9.CreateEffectEx(
                            device,
                            (IntPtr)pData,
                            data.Length,
                            PrepareMacros(preprocessorDefines),
                            IncludeShadow.ToIntPtr(includeFile),
                            skipConstants,
                            (int)flags,
                            pool,
                            out effect,
                            out blobForErrors);
                        
                    }
                    stream.Position = stream.Length;
                }
                catch (SharpDXException ex)
                {
                    if (blobForErrors != null)
                        throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                    throw;
                }
                return effect;
            }
        }

        /// <summary>
        /// Compiles an effect from a string.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="sourceData">The source data.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromString(Device device, string sourceData, ShaderFlags flags)
        {
            return FromString(device, sourceData, null, null, null, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a string.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="sourceData">The source data.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromString(Device device, string sourceData, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags)
        {
            return FromString(device, sourceData, preprocessorDefines, includeFile, skipConstants, flags, null);
        }

        /// <summary>
        /// Compiles an effect from a string.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="sourceData">The source data.</param>
        /// <param name="preprocessorDefines">The preprocessor defines.</param>
        /// <param name="includeFile">The include file.</param>
        /// <param name="skipConstants">The skip constants.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="pool">The pool.</param>
        /// <returns>
        /// An <see cref="Effect"/>
        /// </returns>
        /// <unmanaged>HRESULT D3DXCreateEffectEx([In] IDirect3DDevice9* pDevice,[In] const void* pSrcData,[In] unsigned int SrcDataLen,[In, Buffer] const D3DXMACRO* pDefines,[In] ID3DXInclude* pInclude,[In] const char* pSkipConstants,[In] unsigned int Flags,[In] ID3DXEffectPool* pPool,[In] ID3DXEffect** ppEffect,[In] ID3DXBuffer** ppCompilationErrors)</unmanaged>
        public static Effect FromString(Device device, string sourceData, Macro[] preprocessorDefines, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            Effect effect = null;
            Blob blobForErrors = null;

            var buffer = Marshal.StringToHGlobalAnsi(sourceData);
            try
            {
                    
                    D3DX9.CreateEffectEx(
                        device,
                        buffer,
                        sourceData.Length,
                        PrepareMacros(preprocessorDefines),
                        IncludeShadow.ToIntPtr(includeFile),
                        skipConstants,
                        (int)flags,
                        pool,
                        out effect,
                        out blobForErrors);
            }
            catch (SharpDXException ex)
            {
                if (blobForErrors != null)
                    throw new CompilationException(ex.ResultCode, Utilities.BlobToString(blobForErrors));
                throw;
            }
            finally 
            {
                Marshal.FreeHGlobal(buffer);
            }
            return effect;
        }

        /// <summary>
        /// Set a contiguous range of shader constants with a memory copy.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXEffect::SetRawValue([In] D3DXHANDLE hParameter,[In] const void* pData,[In] unsigned int ByteOffset,[In] unsigned int Bytes)</unmanaged>
        public void SetRawValue(EffectHandle handle, float[] data)
        {
            SetRawValue(handle, data, 0, data.Length);
        }

        /// <summary>
        /// Set a contiguous range of shader constants with a memory copy.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="data">The data.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXEffect::SetRawValue([In] D3DXHANDLE hParameter,[In] const void* pData,[In] unsigned int ByteOffset,[In] unsigned int Bytes)</unmanaged>
        public void SetRawValue(EffectHandle handle, DataStream data)
        {
            SetRawValue(handle, data, 0, (int)data.Length);
        }

        /// <summary>
        /// Set a contiguous range of shader constants with a memory copy.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="countInBytes">The count in bytes.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXEffect::SetRawValue([In] D3DXHANDLE hParameter,[In] const void* pData,[In] unsigned int ByteOffset,[In] unsigned int Bytes)</unmanaged>
        public void SetRawValue(EffectHandle handle, DataStream data, int offset, int countInBytes)
        {
            SetRawValue(handle, (IntPtr)data.PositionPointer, offset, countInBytes);             
        }

        /// <summary>
        /// Set a contiguous range of shader constants with a memory copy.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="data">The data.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DXEffect::SetRawValue([In] D3DXHANDLE hParameter,[In] const void* pData,[In] unsigned int ByteOffset,[In] unsigned int Bytes)</unmanaged>	
        public void SetRawValue(EffectHandle handle, float[] data, int startIndex, int count)
        {
            unsafe
            {
                fixed (void* pData = &data[startIndex])
                SetRawValue(handle, (IntPtr)pData, 0, count << 2); 
            }
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
    }
}