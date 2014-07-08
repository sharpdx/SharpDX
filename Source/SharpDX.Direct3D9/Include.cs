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
using System.IO;

namespace SharpDX.Direct3D9
{
    [Shadow(typeof(IncludeShadow))]
    public partial interface Include
    {
        /// <summary>
        /// A user-implemented method for opening and reading the contents of a shader #include file.
        /// </summary>
        /// <param name="type">A <see cref="SharpDX.D3DCompiler.IncludeType"/>-typed value that indicates the location of the #include file.</param>
        /// <param name="fileName">Name of the #include file.</param>
        /// <param name="parentStream">Pointer to the container that includes the #include file.</param>
        /// <returns>Stream that is associated with fileName to be read. This reference remains valid until <see cref="SharpDX.D3DCompiler.Include.Close"/> is called.</returns>
        /// <unmanaged>HRESULT Open([None] D3D_INCLUDE_TYPE IncludeType,[None] const char* pFileName,[None] LPCVOID pParentData,[None] LPCVOID* ppData,[None] UINT* pBytes)</unmanaged>
        //SharpDX.Result Open(SharpDX.D3DCompiler.IncludeType includeType, string fileNameRef, IntPtr pParentData, IntPtr dataRef, IntPtr bytesRef);
        Stream Open(IncludeType type, string fileName, Stream parentStream);

        /// <summary>	
        /// A user-implemented method for closing a shader #include file.	
        /// </summary>	
        /// <remarks>	
        /// If <see cref="SharpDX.D3DCompiler.Include.Open"/> was successful, Close is guaranteed to be called before the API using the <see cref="SharpDX.D3DCompiler.Include"/> interface returns.	
        /// </remarks>	
        /// <param name="stream">This is a reference that was returned by the corresponding <see cref="SharpDX.D3DCompiler.Include.Open"/> call.</param>
        /// <unmanaged>HRESULT Close([None] LPCVOID pData)</unmanaged>
        void Close(Stream stream);
    }
}
