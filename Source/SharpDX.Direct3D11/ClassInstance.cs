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
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D11
{
    public partial class ClassInstance
    {
        /// <summary>	
        /// Initializes a class-instance object that represents an HLSL class instance.	
        /// </summary>	
        /// <remarks>	
        /// Instances can be created (or gotten) before or after a shader is created. Use the same shader linkage object to acquire a class instance and create the shader the instance is going to be used in. For more information about using the <see cref="SharpDX.Direct3D11.ClassLinkage"/> interface, see {{Dynamic Linking}}. 	
        /// </remarks>	
        /// <param name="linkage">An instance of <see cref="ClassLinkage"/>.</param>
        /// <param name="classTypeName">The type name of a class to initialize. </param>
        /// <param name="constantBufferOffset">Identifies the constant buffer that contains the class data. </param>
        /// <param name="constantVectorOffset">The four-component vector offset from the start of the constant buffer where the class data will begin. Consequently, this is not a byte offset. </param>
        /// <param name="textureOffset">The texture slot for the first texture; there may be multiple textures following the offset. </param>
        /// <param name="samplerOffset">The sampler slot for the first sampler; there may be multiple samplers following the offset. </param>
        /// <returns>Returns S_OK if successful; otherwise, returns one of the following {{Direct3D 11 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11ClassLinkage::CreateClassInstance([In] const char* pClassTypeName,[In] int ConstantBufferOffset,[In] int ConstantVectorOffset,[In] int TextureOffset,[In] int SamplerOffset,[Out] ID3D11ClassInstance** ppInstance)</unmanaged>
        public ClassInstance(ClassLinkage linkage, string classTypeName, int constantBufferOffset, int constantVectorOffset, int textureOffset, int samplerOffset) : base(IntPtr.Zero)
        {
            linkage.CreateClassInstance(classTypeName, constantBufferOffset, constantVectorOffset, textureOffset, samplerOffset, this);
        }

        /// <summary>
        ///   Gets the instance name of the current HLSL class.
        /// </summary>
        /// <remarks>
        ///   GetInstanceName will return a valid name only for instances acquired using <see cref = "SharpDX.Direct3D11.ClassLinkage.GetClassInstance" />.For more information about using the <see cref = "SharpDX.Direct3D11.ClassInstance" /> interface, see {{Dynamic Linking}}.
        /// </remarks>
        /// <returns>The instance name of the current HLSL class.</returns>
        /// <unmanaged>void GetInstanceName([Out, Buffer, Optional] LPSTR pInstanceName,[InOut] SIZE_T* pBufferLength)</unmanaged>
        public string InstanceName
        {
            get
            {
                unsafe
                {
                    PointerSize size = new PointerSize();
                    GetInstanceName(IntPtr.Zero, ref size);
                    sbyte* pBuffer = stackalloc sbyte[size];
                    GetInstanceName((IntPtr) pBuffer, ref size);
                    return Marshal.PtrToStringAnsi((IntPtr)pBuffer);
                }
            }
        }

        /// <summary>
        ///   Gets the type of the current HLSL class.
        /// </summary>
        /// <remarks>
        ///   GetTypeName will return a valid name only for instances acquired using <see cref = "SharpDX.Direct3D11.ClassLinkage.GetClassInstance" />.For more information about using the <see cref = "SharpDX.Direct3D11.ClassInstance" /> interface, see {{Dynamic Linking}}.
        /// </remarks>
        /// <returns>Type of the current HLSL class.</returns>
        /// <unmanaged>void GetTypeName([Out, Buffer, Optional] LPSTR pTypeName,[InOut] SIZE_T* pBufferLength)</unmanaged>
        public string TypeName
        {
            get
            {
                unsafe
                {
                    PointerSize size = new PointerSize();
                    GetInstanceName(IntPtr.Zero, ref size);
                    sbyte* pBuffer = stackalloc sbyte[size];
                    GetTypeName((IntPtr) pBuffer, ref size);
                    return Marshal.PtrToStringAnsi((IntPtr)pBuffer);
                }
            }
        }
    }
}