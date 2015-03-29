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
    public partial class EffectStringVariable
    {
        /// <summary>	
        /// Get the string.	
        /// </summary>	
        /// <returns>Returns a reference to the string.</returns>
        /// <unmanaged>HRESULT ID3D10EffectStringVariable::GetString([Out] const char** ppString)</unmanaged>
        public string GetString()
        {
            unsafe {
                IntPtr temp;
                GetString(out temp);
                return Marshal.PtrToStringAnsi( temp);
            }
        }

        /// <summary>	
        /// Get an array of strings.	
        /// </summary>	
        /// <param name="count">The number of strings in the returned array. </param>
        /// <returns>Returns a reference to the first string in the array.</returns>
        /// <unmanaged>HRESULT ID3D10EffectStringVariable::GetStringArray([Out, Buffer] const char** ppStrings,[None] int Offset,[None] int Count)</unmanaged>
        public string[] GetStringArray(int count)
        {
            return GetStringArray(0, count);
        }       

        /// <summary>	
        /// Get an array of strings.	
        /// </summary>	
        /// <param name="offset">The offset (in number of strings) between the start of the array and the first string to get. </param>
        /// <param name="count">The number of strings in the returned array. </param>
        /// <returns>Returns a reference to the first string in the array.</returns>
        /// <unmanaged>HRESULT ID3D10EffectStringVariable::GetStringArray([Out, Buffer] const char** ppStrings,[None] int Offset,[None] int Count)</unmanaged>
        public string[] GetStringArray(int offset, int count)
        {
            unsafe {
                IntPtr* temp = stackalloc IntPtr[count];
                string[] result = new string[count];
                GetStringArray((IntPtr) temp, offset, count);
                for (int i = 0; i < result.Length; i++)
                    result[i] = Marshal.PtrToStringAnsi( temp[i]);
                return result;
            }
        }        
    }
}