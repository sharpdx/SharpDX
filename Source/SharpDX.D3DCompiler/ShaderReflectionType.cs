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

namespace SharpDX.D3DCompiler
{
    public partial class ShaderReflectionType
    {

        /// <summary>	
        /// Indicates whether two <see cref="SharpDX.D3DCompiler.ShaderReflectionType"/> references have the same underlying type.	
        /// </summary>	
        /// <remarks>	
        /// IsEqual indicates whether the sources of the <see cref="SharpDX.D3DCompiler.ShaderReflectionType"/> references have the same underlying type. For example, if two ID3D11ShaderReflectionType Interface references were retrieved from variables, IsEqual can be used to see if  the variables have the same type. This method's interface is hosted in the out-of-box DLL D3DCompiler_xx.dll. 	
        /// </remarks>	
        /// <param name="typeRef">A reference to a <see cref="SharpDX.D3DCompiler.ShaderReflectionType"/>. </param>
        /// <returns>Returns true if the references have the same underlying type; otherwise returns false. </returns>
        /// <unmanaged>HRESULT ID3D11ShaderReflectionType::IsEqual([In] ID3D11ShaderReflectionType* pType)</unmanaged>
        public bool IsEqual(SharpDX.D3DCompiler.ShaderReflectionType typeRef)
        {
            return IsEqual_(typeRef) == Result.Ok;
        }


        /// <summary>	
        /// Indicates whether a variable is of the specified type.	
        /// </summary>	
        /// <remarks>	
        /// This method's interface is hosted in the out-of-box DLL D3DCompiler_xx.dll. 	
        /// </remarks>	
        /// <param name="typeRef">A reference to a <see cref="SharpDX.D3DCompiler.ShaderReflectionType"/>. </param>
        /// <returns>Returns true if object being queried is equal to or inherits from the type in the pType parameter; otherwise returns false. </returns>
        /// <unmanaged>HRESULT ID3D11ShaderReflectionType::IsOfType([In] ID3D11ShaderReflectionType* pType)</unmanaged>
        public bool IsOfType(SharpDX.D3DCompiler.ShaderReflectionType typeRef)
        {
            return IsOfType_(typeRef) == Result.Ok;
        }

        /// <summary>	
        /// Indicates whether a class type implements an interface.	
        /// </summary>	
        /// <remarks>	
        /// This method's interface is hosted in the out-of-box DLL D3DCompiler_xx.dll. 	
        /// </remarks>	
        /// <param name="baseRef">A reference to a <see cref="SharpDX.D3DCompiler.ShaderReflectionType"/>. </param>
        /// <returns>Returns true if the interface is implemented; otherwise return false. </returns>
        /// <unmanaged>HRESULT ID3D11ShaderReflectionType::ImplementsInterface([In] ID3D11ShaderReflectionType* pBase)</unmanaged>
        public bool ImplementsInterface(SharpDX.D3DCompiler.ShaderReflectionType baseRef)
        {
            return ImplementsInterface_(baseRef) == Result.Ok;
        }
    }
}