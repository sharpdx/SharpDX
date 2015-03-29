// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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
    public partial class LibraryReflection
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.D3DCompiler.LibraryReflection" /> class from a <see cref = "T:SharpDX.D3DCompiler.ShaderBytecode" />.
        /// </summary>
        /// <param name = "libraryBytecode"></param>
        public unsafe LibraryReflection(byte[] libraryBytecode)
        {
            IntPtr temp;
            fixed (void* ptr = libraryBytecode)
                D3D.ReflectLibrary((IntPtr)ptr, libraryBytecode.Length, Utilities.GetGuidFromType(GetType()), out temp);
            NativePointer = temp;
        }

        /// <summary>	
        /// <p>Returns all function reflectors provided by this library</p>	
        /// </summary>	
        /// <returns><p>All references to <strong><see cref="SharpDX.D3DCompiler.FunctionReflection"/></strong> interfaces that represents the function reflectors.</p></returns>	
        public FunctionReflection[] Functions
        {
            get
            {
                var result = new FunctionReflection[this.Description.FunctionCount];
                for (int i = 0; i < result.Length;i++)
                {
                    result[i] = this.GetFunctionByIndex(i);
                }
                return result;
            }
        }
    }
}