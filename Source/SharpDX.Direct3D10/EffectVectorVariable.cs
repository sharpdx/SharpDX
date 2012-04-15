// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Diagnostics;

namespace SharpDX.Direct3D10
{
    public partial class EffectVectorVariable
    {
        private const string VectorInvalidSize = "Invalid Vector size: Must be 16 bytes or 4 x 4 bytes";

        /// <summary>	
        /// Get a four-component vector that contains integer data.	
        /// </summary>	
        /// <returns>Returns a four-component vector that contains integer data </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetIntVector([Out] int* pData)</unmanaged>
        public SharpDX.Int4 GetIntVector()
        {
            Int4 temp;
            GetIntVector(out temp);
            return temp;
        }

        /// <summary>	
        /// Get a four-component vector that contains floating-point data.	
        /// </summary>	
        /// <returns>Returns a four-component vector that contains floating-point data.</returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetFloatVector([Out] float* pData)</unmanaged>
        public SharpDX.Vector4 GetFloatVector()
        {
            SharpDX.Vector4 temp;
            GetFloatVector(out temp);
            return temp;
        }

        /// <summary>	
        /// Get a four-component vector that contains boolean data.	
        /// </summary>	
        /// <returns>a four-component vector that contains boolean data. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetBoolVector([Out, Buffer] BOOL* pData)</unmanaged>
        public Bool4 GetBoolVector()
        {
            Bool4 temp;
            GetBoolVector(out temp);
            return temp;
        }

        /// <summary>	
        /// Get a four-component vector.	
        /// </summary>	
        /// <typeparam name="T">Type of the four-component vector</typeparam>
        /// <returns>a four-component vector. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetFloatVector([Out, Buffer] BOOL* pData)</unmanaged>
        public unsafe T GetVector<T>() where T : struct
        {
            T temp;
            GetIntVector(out *(Int4*)Interop.CastOut(out temp));
            return temp;
        }

        /// <summary>	
        /// Set an array of four-component vectors that contain integer data.	
        /// </summary>	
        /// <param name="array">A reference to the start of the data to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetIntVectorArray([In, Buffer] int* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result Set(SharpDX.Int4[] array)
        {
            return Set(array, 0, array.Length);
        }

        /// <summary>	
        /// Set an array of four-component vectors that contain floating-point data.	
        /// </summary>	
        /// <param name="array">A reference to the start of the data to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVectorArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result Set(SharpDX.Vector4[] array)
        {
            return Set(array, 0, array.Length);
        }

        /// <summary>
        /// Set an array of four-component vectors that contain floating-point data.
        /// </summary>
        /// <typeparam name="T">Type of the four-component vector</typeparam>
        /// <param name="array">A reference to the start of the data to set.</param>
        /// <returns>
        /// Returns one of the following {{Direct3D 10 Return Codes}}.
        /// </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVectorArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result Set<T>(T[] array) where T : struct
        {
            Trace.Assert(Utilities.SizeOf<T>() == 16, VectorInvalidSize);
            return Set(Interop.CastArray<Vector4, T>(array), 0, array.Length);
        }

        /// <summary>	
        /// Set a x-component vector.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public SharpDX.Result Set<T>(T value) where T : struct
        {
            return Set(ref value);
        }

        /// <summary>	
        /// Set a x-component vector.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public unsafe SharpDX.Result Set<T>(ref T value) where T : struct
        {
            Trace.Assert(Utilities.SizeOf<T>() <= 16, VectorInvalidSize);
            return SetRawValue(new IntPtr(Interop.Fixed(ref value)), 0, Utilities.SizeOf<T>());
        }

        /// <summary>	
        /// Set a two-component vector that contains floating-point data.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public SharpDX.Result Set(SharpDX.Vector2 value)
        {
            unsafe
            {
                return SetRawValue(new IntPtr(&value), 0, Utilities.SizeOf<SharpDX.Vector2>());
            }
        }

        /// <summary>	
        /// Set a three-component vector that contains floating-point data.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public SharpDX.Result Set(SharpDX.Vector3 value)
        {
            unsafe
            {
                return SetRawValue(new IntPtr(&value), 0, Utilities.SizeOf<SharpDX.Vector3>());
            }
        }

        /// <summary>	
        /// Set a four-component color that contains floating-point data.	
        /// </summary>	
        /// <param name="value">A reference to the first component. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVector([In] float* pData)</unmanaged>
        public SharpDX.Result Set(SharpDX.Color4 value)
        {
            unsafe
            {
                return SetRawValue(new IntPtr(&value), 0, Utilities.SizeOf<SharpDX.Color4>());
            }
        }

        /// <summary>	
        /// Set an array of four-component color that contain floating-point data.	
        /// </summary>	
        /// <param name="array">A reference to the start of the data to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::SetFloatVectorArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result Set(SharpDX.Color4[] array)
        {
            unsafe
            {
                fixed (void* pArray = &array[0]) return SetRawValue((IntPtr)pArray, 0, array.Length * Utilities.SizeOf<SharpDX.Color4>());
            }
        }

        /// <summary>	
        /// Get an array of four-component vectors that contain integer data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>Returns an array of four-component vectors that contain integer data. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetIntVectorArray([Out, Buffer] int* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Int4[] GetIntVectorArray(int count)
        {
            var temp = new Int4[count];
            GetIntVectorArray(temp, 0, count);
            return temp;
        }

        /// <summary>	
        /// Get an array of four-component vectors that contain floating-point data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>Returns an array of four-component vectors that contain floating-point data. </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetFloatVectorArray([None] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Vector4[] GetFloatVectorArray(int count)
        {
            var temp = new SharpDX.Vector4[count];
            GetFloatVectorArray(temp, 0, count);
            return temp;
        }

        /// <summary>	
        /// Get an array of four-component vectors that contain boolean data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>an array of four-component vectors that contain boolean data.	 </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetBoolVectorArray([Out, Buffer] BOOL* pData,[None] int Offset,[None] int Count)</unmanaged>
        public Bool4[] GetBoolVectorArray(int count)
        {
            var temp = new Bool4[count];
            GetBoolVectorArray(temp, 0, count);
            return temp;
        }


        /// <summary>	
        /// Get an array of four-component vectors that contain boolean data.	
        /// </summary>	
        /// <param name="count">The number of array elements to set. </param>
        /// <returns>an array of four-component vectors that contain boolean data.	 </returns>
        /// <unmanaged>HRESULT ID3D10EffectVectorVariable::GetBoolVectorArray([Out, Buffer] BOOL* pData,[None] int Offset,[None] int Count)</unmanaged>
        public T[] GetVectorArray<T>(int count) where T : struct
        {
            var temp = new T[count];
            GetIntVectorArray(Interop.CastArray<Int4, T>(temp), 0, count);
            return temp;
        }
    }
}