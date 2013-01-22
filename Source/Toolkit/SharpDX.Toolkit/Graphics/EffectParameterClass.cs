// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// Values that identify the class of a shader variable.
    /// </summary>	
    /// <remarks>	
    /// The class of a shader variable is not a programming class; the class identifies the variable class such as scalar, vector, object, and so on.
    /// </remarks>	
    /// <msdn-id>ff728733</msdn-id>	
    /// <unmanaged>D3D_SHADER_VARIABLE_CLASS</unmanaged>	
    /// <unmanaged-short>D3D_SHADER_VARIABLE_CLASS</unmanaged-short>	
    public enum EffectParameterClass : byte
    {
        /// <summary>	
        /// <dd> <p>The shader variable is a scalar.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_SCALAR</unmanaged>	
        /// <unmanaged-short>D3D_SVC_SCALAR</unmanaged-short>	
        Scalar = unchecked((int)0),

        /// <summary>	
        /// <dd> <p>The shader variable is a vector.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_VECTOR</unmanaged>	
        /// <unmanaged-short>D3D_SVC_VECTOR</unmanaged-short>	
        Vector = unchecked((int)1),

        /// <summary>	
        /// <dd> <p>The shader variable is a row-major matrix.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_MATRIX_ROWS</unmanaged>	
        /// <unmanaged-short>D3D_SVC_MATRIX_ROWS</unmanaged-short>	
        MatrixRows = unchecked((int)2),

        /// <summary>	
        /// <dd> <p>The shader variable is a column-major matrix.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_MATRIX_COLUMNS</unmanaged>	
        /// <unmanaged-short>D3D_SVC_MATRIX_COLUMNS</unmanaged-short>	
        MatrixColumns = unchecked((int)3),

        /// <summary>	
        /// <dd> <p>The shader variable is an object.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_OBJECT</unmanaged>	
        /// <unmanaged-short>D3D_SVC_OBJECT</unmanaged-short>	
        Object = unchecked((int)4),

        /// <summary>	
        /// <dd> <p>The shader variable is a structure.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_STRUCT</unmanaged>	
        /// <unmanaged-short>D3D_SVC_STRUCT</unmanaged-short>	
        Struct = unchecked((int)5),

        /// <summary>	
        /// <dd> <p>The shader variable is a class.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_INTERFACE_CLASS</unmanaged>	
        /// <unmanaged-short>D3D_SVC_INTERFACE_CLASS</unmanaged-short>	
        InterfaceClass = unchecked((int)6),

        /// <summary>	
        /// <dd> <p>The shader variable is an interface.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728733</msdn-id>	
        /// <unmanaged>D3D_SVC_INTERFACE_POINTER</unmanaged>	
        /// <unmanaged-short>D3D_SVC_INTERFACE_POINTER</unmanaged-short>	
        InterfacePointer = unchecked((int)7),
    }
}