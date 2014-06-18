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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class BaseEffect
    {
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::GetString([In] D3DXHANDLE hParameter,[Out] const void** ppString)</unmanaged>
        public string GetString(EffectHandle parameter)
        {
            return Marshal.PtrToStringAnsi(GetString_(parameter));
        }

        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <param name="parameter">Handle of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::GetValue([In] D3DXHANDLE hParameter,[In] void* pData,[In] unsigned int Bytes)</unmanaged>
        public T GetValue<T>(EffectHandle parameter) where T : struct
        {
            unsafe
            {
                var value = default(T);
                GetValue(parameter, (IntPtr)Interop.Fixed(ref value), Utilities.SizeOf<T>());
                return value;
            }
        }

        /// <summary>
        /// Gets the value of the specified parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">Handle of the parameter.</param>
        /// <param name="count">The count.</param>
        /// <returns>
        /// The value of the parameter.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::GetValue([In] D3DXHANDLE hParameter,[In] void* pData,[In] unsigned int Bytes)</unmanaged>
        public T[] GetValue<T>(EffectHandle parameter, int count) where T : struct
        {
            unsafe
            {
                var value = new T[count];
                GetValue(parameter, (IntPtr)Interop.Fixed(value), Utilities.SizeOf<T>());
                return value;
            }
        }

        /// <summary>
        /// Sets a bool value.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetBool([In] D3DXHANDLE hConstant,[In] BOOL b)</unmanaged>
        public void SetValue(EffectHandle effectHandle, bool value)
        {
            SetBool(effectHandle, value);
        }

        /// <summary>
        /// Sets a float value.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetFloat([In] D3DXHANDLE hConstant,[In] float f)</unmanaged>
        public void SetValue(EffectHandle effectHandle, float value)
        {
            SetFloat(effectHandle, value);
        }

        /// <summary>
        /// Sets an int value.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetInt([In] D3DXHANDLE hConstant,[In] int n)</unmanaged>
        public void SetValue(EffectHandle effectHandle, int value)
        {
            SetInt(effectHandle, value);
        }

        /// <summary>
        /// Sets a matrix.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetMatrix([In] D3DXHANDLE hConstant,[In] const D3DXMATRIX* pMatrix)</unmanaged>
        public void SetValue(EffectHandle effectHandle, RawMatrix value)
        {
            SetMatrix(effectHandle, ref value);
        }

        /// <summary>
        /// Sets a 4D vector.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetVector([In] D3DXHANDLE hConstant,[In] const D3DXVECTOR4* pVector)</unmanaged>
        public void SetValue(EffectHandle effectHandle, RawVector4 value)
        {
            SetVector(effectHandle, value);
        }

        /// <summary>
        /// Sets a typed value.
        /// </summary>
        /// <typeparam name="T">Type of the value to set</typeparam>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetValue([In] D3DXHANDLE hConstant,[In] const void* pData,[In] unsigned int Bytes)</unmanaged>
        public void SetValue<T>(EffectHandle effectHandle, T value) where T : struct
        {
            unsafe
            {
                SetValue(effectHandle, (IntPtr)Interop.Fixed(ref value), Utilities.SizeOf<T>());
            }
        }

        /// <summary>
        /// Sets an array of bools.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetBoolArray([In] D3DXHANDLE hConstant,[In, Buffer] const BOOL* pb,[In] unsigned int Count)</unmanaged>
        public void SetValue(EffectHandle effectHandle, bool[] values)
        {
            var tempArray = Utilities.ConvertToIntArray(values);
            SetBoolArray(effectHandle, tempArray, values.Length);
        }

        /// <summary>
        /// Sets an array of floats.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetFloatArray([In] D3DXHANDLE hConstant,[In, Buffer] const float* pf,[In] unsigned int Count)</unmanaged>
        public void SetValue(EffectHandle effectHandle, float[] values)
        {
            SetFloatArray(effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of ints.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetIntArray([In] D3DXHANDLE hConstant,[In, Buffer] const int* pn,[In] unsigned int Count)</unmanaged>
        public void SetValue(EffectHandle effectHandle, int[] values)
        {
            SetIntArray(effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of matrices.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetMatrixArray([In] D3DXHANDLE hConstant,[In, Buffer] const D3DXMATRIX* pMatrix,[In] unsigned int Count)</unmanaged>
        public void SetValue(EffectHandle effectHandle, RawMatrix[] values)
        {
            SetMatrixArray(effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of 4D vectors.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetVectorArray([In] D3DXHANDLE hConstant,[In, Buffer] const D3DXVECTOR4* pVector,[In] unsigned int Count)</unmanaged>
        public void SetValue(EffectHandle effectHandle, RawVector4[] values)
        {
            SetVectorArray(effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of elements.
        /// </summary>
        /// <typeparam name="T">Type of the array element</typeparam>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXBaseEffect::SetValue([In] D3DXHANDLE hConstant,[In] const void* pData,[In] unsigned int Bytes)</unmanaged>
        public void SetValue<T>(EffectHandle effectHandle, T[] values) where T : struct
        {
            unsafe
            {
                SetValue(effectHandle, (IntPtr)Interop.Fixed(values), Utilities.SizeOf<T>() * values.Length);
            }
        }
    }
}