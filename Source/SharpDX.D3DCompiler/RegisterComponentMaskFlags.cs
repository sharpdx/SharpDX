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
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_FLAG</unmanaged>
    [Flags]
    public enum RegisterComponentMaskFlags : byte {	
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_ALL</unmanaged>
        All = unchecked((int)15),			
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_COMPONENT_W</unmanaged>
        ComponentW = unchecked((int)8),			
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_COMPONENT_X</unmanaged>
        ComponentX = unchecked((int)1),			
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_COMPONENT_Y</unmanaged>
        ComponentY = unchecked((int)2),			
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D11_REGISTER_COMPONENT_MASK_COMPONENT_Z</unmanaged>
        ComponentZ = unchecked((int)4),			
        
        /// <summary>	
        /// None.	
        /// </summary>	
        /// <unmanaged>None</unmanaged>
        None = unchecked((int)0),			
    }
}