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
    public partial class EffectDepthStencilViewVariable
    {
        /// <summary>	
        /// Set an array of depth-stencil-view resources.	
        /// </summary>	
        /// <param name="resourcesRef"> A pointer to an array of depth-stencil-view interfaces. See <see cref="SharpDX.Direct3D10.DepthStencilView"/>. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectDepthStencilViewVariable::SetDepthStencilArray([In, Buffer] ID3D10DepthStencilView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public void SetDepthStencilArray(SharpDX.Direct3D10.DepthStencilView[] resourcesRef)
        {
            SetDepthStencilArray(resourcesRef, 0, resourcesRef.Length);
        }

        /// <summary>	
        /// Set an array of depth-stencil-view resources.	
        /// </summary>	
        /// <param name="resourcesRef"> A pointer to an array of depth-stencil-view interfaces. See <see cref="SharpDX.Direct3D10.DepthStencilView"/>. </param>
        /// <param name="offset"> The zero-based array index to set the first interface. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectDepthStencilViewVariable::SetDepthStencilArray([In, Buffer] ID3D10DepthStencilView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public void SetDepthStencilArray(SharpDX.Direct3D10.DepthStencilView[] resourcesRef, int offset)
        {
            SetDepthStencilArray(resourcesRef, offset, resourcesRef.Length-offset);
        }

        /// <summary>	
        /// Get an array of depth-stencil-view resources.	
        /// </summary>	
        /// <param name="count"> The number of elements in the array. </param>
        /// <returns>Returns an array of depth-stencil-view interfaces. See <see cref="SharpDX.Direct3D10.DepthStencilView"/>. </returns>
        /// <unmanaged>HRESULT ID3D10EffectDepthStencilViewVariable::GetDepthStencilArray([Out, Buffer] ID3D10DepthStencilView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Direct3D10.DepthStencilView[] GetDepthStencilArray(int count)
        {
            return GetDepthStencilArray(0, count);
        }        

        
        /// <summary>	
        /// Get an array of depth-stencil-view resources.	
        /// </summary>	
        /// <param name="offset"> The zero-based array index to get the first interface. </param>
        /// <param name="count"> The number of elements in the array. </param>
        /// <returns>Returns an array of depth-stencil-view interfaces. See <see cref="SharpDX.Direct3D10.DepthStencilView"/>. </returns>
        /// <unmanaged>HRESULT ID3D10EffectDepthStencilViewVariable::GetDepthStencilArray([Out, Buffer] ID3D10DepthStencilView** ppResources,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Direct3D10.DepthStencilView[] GetDepthStencilArray(int offset, int count)
        {
            DepthStencilView[] temp = new DepthStencilView[count];
            GetDepthStencilArray(temp, offset, count);
            return temp;
        }        
    }
}