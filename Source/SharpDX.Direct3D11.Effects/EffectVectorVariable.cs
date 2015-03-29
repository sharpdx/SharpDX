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

using SharpDX.Mathematics.Interop;
using System;
using System.Diagnostics;

namespace SharpDX.Direct3D11
{
    public partial class EffectVectorVariable
    {
        private const string VectorInvalidSize = "Invalid Vector size: Must be 16 bytes or 4 x 4 bytes";

        /// <summary>	
        /// Get a four-component vector that contains integer data.	
        /// </summary>	
        /// <returns>Returns a four-component vector that contains integer data </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetIntVector([Out] int* pData)</unmanaged>
        public RawInt4 GetIntVector()
        {
            RawInt4 temp;
            GetIntVector(out temp);
            return temp;
        }

        /// <summary>	
        /// Get a four-component vector that contains floating-point data.	
        /// </summary>	
        /// <returns>Returns a four-component vector that contains floating-point data.</returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetFloatVector([Out] float* pData)</unmanaged>
        public RawVector4 GetFloatVector()
        {
            RawVector4 temp;
            GetFloatVector(out temp);
            return temp;
        }

        /// <summary>	
        /// Get a four-component vector that contains boolean data.	
        /// </summary>	
        /// <returns>a four-component vector that contains boolean data. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetBoolVector([Out, Buffer] BOOL* pData)</unmanaged>
        public RawBool4 GetBoolVector()
        {
            RawBool4 temp;
            GetBoolVector(out temp);
            return temp;
        }

        /// <summary>	
        /// Get a four-component vector.	
        /// </summary>	
        /// <typeparam name="T">Type of the four-component vector</typeparam>
        /// <returns>a four-component vector. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetFloatVector([Out, Buffer] BOOL* pData)</unmanaged>
        public unsafe T GetVector<T>() where T : struct
        {
            T temp;
            GetIntVector(out *(RawInt4*)Interop.CastOut(out temp));
            return temp;
        }

        /// <summary>	
        /// Set an array of four-component vectors that contain integer data.	
        /// </summary>	
        /// <param name="array">A reference to the start of the data to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetIntVectorArray([In, Buffer] int* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void Set(RawInt4[] array)
        {
            Set(array, 0, array.Length);
        }

        /// <summary>	
        /// Set an array of four-component vectors that contain floating-point data.	
        /// </summary>	
        /// <param name="array">A reference to the start of the data to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVectorArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void Set(RawVector4[] array)
        {
            Set(array, 0, array.Length);
        }

        /// <summary>
        /// Set an array of four-component vectors that contain floating-point data.
        /// </summary>
        /// <typeparam name="T">Type of the four-component vector</typeparam>
        /// <param name="array">A reference to the start of the data to set.</param>
        /// <returns>
        /// Returns one of the following {{Direct3D 10 Return Codes}}.
        /// </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVectorArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void Set<T>(T[] array) where T : struct
        {
            System.Diagnostics.Debug.Assert(Utilities.SizeOf<T>() == 16, VectorInvalidSize);
            Set(Interop.CastArray<RawVector4,T>(array), 0, array.Length);
        }

        /// <summary>	
        /// Set a x-component vector.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public void Set<T>(T value) where T : struct
        {
            Set(ref value);
        }

        /// <summary>	
        /// Set a x-component vector.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public unsafe void Set<T>(ref T value) where T : struct
        {
            System.Diagnostics.Debug.Assert(Utilities.SizeOf<T>() <= 16, VectorInvalidSize);
            SetRawValue(new IntPtr(Interop.Fixed(ref value)), 0, Utilities.SizeOf<T>());
        }

        /// <summary>	
        /// Set a two-component vector that contains floating-point data.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public void Set(RawVector2 value)
        {
            unsafe
            {
                SetRawValue(new IntPtr(&value), 0, Utilities.SizeOf<RawVector2>());
            }
        }

        /// <summary>	
        /// Set a three-component vector that contains floating-point data.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public void Set(RawVector3 value)
        {
            unsafe
            {
                SetRawValue(new IntPtr(&value), 0, Utilities.SizeOf<RawVector3>());
            }
        }

        /// <summary>	
        /// Set a four-component color that contains floating-point data.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public void Set(RawColor4 value)
        {
            unsafe
            {
                SetRawValue(new IntPtr(&value), 0, Utilities.SizeOf<RawColor4>());
            }
        }

        /// <summary>	
        /// Set an array of four-component color that contain floating-point data.	
        /// </summary>	
        /// <param name="array">A reference to the start of the data to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::SetFloatVectorArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void Set(RawColor4[] array)
        {
            unsafe
            {
                fixed (void* pArray = &array[0]) SetRawValue((IntPtr)pArray, 0, array.Length * Utilities.SizeOf<RawColor4>());
            }
        }
      
        /// <summary>	
        /// Get an array of four-component vectors that contain integer data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>Returns an array of four-component vectors that contain integer data. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetIntVectorArray([Out, Buffer] int* pData,[None] int Offset,[None] int Count)</unmanaged>
        public RawInt4[] GetIntVectorArray(int count)
        {
            var temp = new RawInt4[count];
            GetIntVectorArray(temp, 0, count);
            return temp;            
        }

        /// <summary>	
        /// Get an array of four-component vectors that contain floating-point data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>Returns an array of four-component vectors that contain floating-point data. </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetFloatVectorArray([None] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public RawVector4[] GetFloatVectorArray(int count)
        {
            var temp = new RawVector4[count];
            GetFloatVectorArray(temp, 0, count);
            return temp;            
        }

        /// <summary>	
        /// Get an array of four-component vectors that contain boolean data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>an array of four-component vectors that contain boolean data.	 </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetBoolVectorArray([Out, Buffer] BOOL* pData,[None] int Offset,[None] int Count)</unmanaged>
        public RawBool4[] GetBoolVectorArray(int count)
        {
            var temp = new RawBool4[count];
            GetBoolVectorArray(temp, 0, count);
            return temp;
        }


        /// <summary>	
        /// Get an array of four-component vectors that contain boolean data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>an array of four-component vectors that contain boolean data.	 </returns>
        /// <unmanaged>HRESULT ID3D11EffectVectorVariable::GetBoolVectorArray([Out, Buffer] BOOL* pData,[None] int Offset,[None] int Count)</unmanaged>
        public T[] GetVectorArray<T>(int count) where T : struct
        {
            var temp = new T[count];
            GetIntVectorArray(Interop.CastArray<RawInt4,T>(temp), 0, count);
            return temp;
        }    
    }
}