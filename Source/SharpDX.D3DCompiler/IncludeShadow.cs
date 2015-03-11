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
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using SharpDX.Collections;

namespace SharpDX.D3DCompiler
{
    /// <summary>
    /// Shadow callback for <see cref="Include"/>.
    /// </summary>
    internal class IncludeShadow : CppObjectShadow
    {
        private static readonly IncludeVtbl Vtbl = new IncludeVtbl();
        private readonly Dictionary<IntPtr, Frame> frames = new Dictionary<IntPtr, Frame>(EqualityComparer.DefaultIntPtr);

        private struct Frame
        {
            public Frame(Stream stream, GCHandle handle)
            {
                Stream = stream;
                Handle = handle;
            }

            public Stream Stream;
            public GCHandle Handle;

            public void Close()
            {
                if (Handle.IsAllocated)
                    Handle.Free();
            }
        }

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(Include callback)
        {
            return ToCallbackPtr<Include>(callback);
        }

        /// <summary>
        /// Internal Include Callback
        /// </summary>
        private class IncludeVtbl : CppObjectVtbl
        {
            public IncludeVtbl() : base(2)
            {
                AddMethod(new OpenDelegate(OpenImpl));
                AddMethod(new CloseDelegate(CloseImpl));
            }

            /// <summary>	
            /// A user-implemented method for opening and reading the contents of a shader #include file.	
            /// </summary>
            /// <param name="thisPtr">This pointer</param>
            /// <param name="includeType">A <see cref="SharpDX.D3DCompiler.IncludeType"/>-typed value that indicates the location of the #include file. </param>
            /// <param name="fileNameRef">Name of the #include file.</param>
            /// <param name="pParentData">Pointer to the container that includes the #include file.</param>
            /// <param name="dataRef">Pointer to the buffer that Open returns that contains the include directives. This pointer remains valid until <see cref="SharpDX.D3DCompiler.Include.Close"/> is called.</param>
            /// <param name="bytesRef">Pointer to the number of bytes that Open returns in ppData.</param>
            /// <returns>The user-implemented method should return S_OK. If Open fails when reading the #include file, the application programming interface (API) that caused Open to be called fails. This failure can occur in one of the following situations:The high-level shader language (HLSL) shader fails one of the D3D10CompileShader*** functions.The effect fails one of the D3D10CreateEffect*** functions.</returns>
            /// <unmanaged>HRESULT Open([None] D3D_INCLUDE_TYPE IncludeType,[None] const char* pFileName,[None] LPCVOID pParentData,[None] LPCVOID* ppData,[None] UINT* pBytes)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate SharpDX.Result OpenDelegate(IntPtr thisPtr, SharpDX.D3DCompiler.IncludeType includeType, IntPtr fileNameRef, IntPtr pParentData, ref IntPtr dataRef, ref int bytesRef);
            private static SharpDX.Result OpenImpl(IntPtr thisPtr, SharpDX.D3DCompiler.IncludeType includeType, IntPtr fileNameRef, IntPtr pParentData, ref IntPtr dataRef, ref int bytesRef)
            {
                unsafe
                {
                    try
                    {
                        var shadow = ToShadow<IncludeShadow>(thisPtr);
                        var callback = (Include) shadow.Callback;

                        Stream stream = null;
                        Stream parentStream = null;

                        if (shadow.frames.ContainsKey(pParentData))
                            parentStream = shadow.frames[pParentData].Stream;

                        stream = callback.Open(includeType, Marshal.PtrToStringAnsi(fileNameRef), parentStream);
                        if (stream == null)
                            return Result.Fail;

                        GCHandle handle;

                        if (stream is DataStream)
                        {
                            // Magic shortcut if we happen to get a DataStream
                            var data = (DataStream)stream;
                            dataRef = data.PositionPointer;
                            bytesRef = (int)(data.Length - data.Position);
                            handle = new GCHandle();
                        }
                        else
                        {
                            // Read the stream into a byte array and pin it
                            byte[] data = SharpDX.Utilities.ReadStream(stream);
                            handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                            dataRef = handle.AddrOfPinnedObject();
                            bytesRef = data.Length;
                        }

                        shadow.frames.Add(dataRef, new Frame(stream, handle));

                        return Result.Ok;
                    }
                    catch (SharpDXException exception)
                    {
                        return exception.ResultCode.Code;
                    }
                    catch (Exception)
                    {
                        return Result.Fail;
                    }
                }
            }

            /// <summary>	
            /// A user-implemented method for closing a shader #include file.	
            /// </summary>	
            /// <remarks>	
            /// If <see cref="SharpDX.D3DCompiler.Include.Open"/> was successful, Close is guaranteed to be called before the API using the <see cref="SharpDX.D3DCompiler.Include"/> interface returns.	
            /// </remarks>
            /// <param name="thisPtr">This pointer</param>
            /// <param name="pData">Pointer to the buffer that contains the include directives. This is the pointer that was returned by the corresponding <see cref="SharpDX.D3DCompiler.Include.Open"/> call.</param>
            /// <returns>The user-implemented Close method should return S_OK. If Close fails when it closes the #include file, the application programming interface (API) that caused Close to be called fails. This failure can occur in one of the following situations:The high-level shader language (HLSL) shader fails one of the D3D10CompileShader*** functions.The effect fails one of the D3D10CreateEffect*** functions.</returns>
            /// <unmanaged>HRESULT Close([None] LPCVOID pData)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate SharpDX.Result CloseDelegate(IntPtr thisPtr, IntPtr pData);
            private static SharpDX.Result CloseImpl(IntPtr thisPtr, IntPtr pData)
            {
                try
                {
                    var shadow = ToShadow<IncludeShadow>(thisPtr);
                    var callback = (Include)shadow.Callback;

                    Frame frame;
                    if (shadow.frames.TryGetValue(pData, out frame))
                    {
                        shadow.frames.Remove(pData);
                        callback.Close(frame.Stream);
                        frame.Close();
                    }
                    return Result.Ok;
                }
                catch (SharpDXException exception)
                {
                    return exception.ResultCode.Code;
                }
                catch (Exception)
                {
                    return Result.Fail;
                }
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}