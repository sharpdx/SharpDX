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

namespace SharpDX.Direct3D10
{
    public partial class EffectShaderResourceVariable
    {
        /// <summary>	
        /// Set an array of shader resources.	
        /// </summary>	
        /// <param name="resourcesRef">The address of an array of shader-resource-view interfaces. See <see cref="SharpDX.Direct3D10.ShaderResourceView"/>. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectShaderResourceVariable::SetResourceArray([None] ID3D10ShaderResourceView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public void SetResourceArray(SharpDX.Direct3D10.ShaderResourceView[] resourcesRef)
        {
            SetResourceArray(resourcesRef, 0);
        }

        /// <summary>	
        /// Set an array of shader resources.	
        /// </summary>	
        /// <param name="resourcesRef">The address of an array of shader-resource-view interfaces. See <see cref="SharpDX.Direct3D10.ShaderResourceView"/>. </param>
        /// <param name="offset">The zero-based array index to get the first interface. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectShaderResourceVariable::SetResourceArray([None] ID3D10ShaderResourceView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public void SetResourceArray(SharpDX.Direct3D10.ShaderResourceView[] resourcesRef, int offset)
        {
            SetResourceArray(resourcesRef, offset, resourcesRef.Length);
        }

        /// <summary>	
        /// Get an array of shader resources.	
        /// </summary>	
        /// <param name="count">The number of elements in the array. </param>
        /// <returns>Returns an array of shader-resource-view interfaces. See <see cref="SharpDX.Direct3D10.ShaderResourceView"/>. </returns>
        /// <unmanaged>HRESULT ID3D10EffectShaderResourceVariable::GetResourceArray([Out] ID3D10ShaderResourceView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Direct3D10.ShaderResourceView[] GetResourceArray(int count)
        {
            return GetResourceArray(0, count);
        }        

        /// <summary>	
        /// Get an array of shader resources.	
        /// </summary>	
        /// <param name="offset">The zero-based array index to get the first interface. </param>
        /// <param name="count">The number of elements in the array. </param>
        /// <returns>Returns an array of shader-resource-view interfaces. See <see cref="SharpDX.Direct3D10.ShaderResourceView"/>. </returns>
        /// <unmanaged>HRESULT ID3D10EffectShaderResourceVariable::GetResourceArray([Out] ID3D10ShaderResourceView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Direct3D10.ShaderResourceView[] GetResourceArray(int offset, int count)
        {
            ShaderResourceView[] temp = new ShaderResourceView[count];
            GetResourceArray(temp, offset, count);
            return temp;
        }        
    }
}