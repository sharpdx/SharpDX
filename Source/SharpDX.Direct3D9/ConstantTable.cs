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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class ConstantTable
    {
        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public DataStream Buffer
        {
            get
            {
                return new DataStream(BufferPointer, BufferSize, true, true);
            }
        }

        /// <summary>
        /// Gets a single constant description in the constant table.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <returns>The constant description</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::GetConstantDesc([In] D3DXHANDLE hConstant,[Out, Buffer] D3DXCONSTANT_DESC* pConstantDesc,[InOut] unsigned int* pCount)</unmanaged>
        public ConstantDescription GetConstantDescription(SharpDX.Direct3D9.EffectHandle effectHandle)
        {
            int count = 1;
            var descriptions = new ConstantDescription[1];
            GetConstantDescription(effectHandle, descriptions, ref count);
            return descriptions[0];
        }

        /// <summary>
        /// Gets an array of constant descriptions in the constant table.
        /// </summary>
        /// <param name="effectHandle">The effect handle.</param>
        /// <returns>An array of constant descriptions</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::GetConstantDesc([In] D3DXHANDLE hConstant,[Out, Buffer] D3DXCONSTANT_DESC* pConstantDesc,[InOut] unsigned int* pCount)</unmanaged>
        public ConstantDescription[] GetConstantDescriptionArray(SharpDX.Direct3D9.EffectHandle effectHandle)
        {
            int count = 0;
            GetConstantDescription(effectHandle, null, ref count);
            var descriptions = new ConstantDescription[count];
            GetConstantDescription(effectHandle, descriptions, ref count);
            return descriptions;
        }

        /// <summary>
        /// Sets a bool value.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetBool([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] BOOL b)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, bool value)
        {
            SetBool(device, effectHandle, value);
        }

        /// <summary>
        /// Sets a float value.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetFloat([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] float f)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, float value)
        {
            SetFloat(device, effectHandle, value);
        }

        /// <summary>
        /// Sets an int value.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetInt([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] int n)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, int value)
        {
            SetInt(device, effectHandle, value);
        }

        /// <summary>
        /// Sets a matrix.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetMatrix([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] const D3DXMATRIX* pMatrix)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, RawMatrix value)
        {
            SetMatrix(device, effectHandle, ref value);
        }

        /// <summary>
        /// Sets a 4D vector.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetVector([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] const D3DXVECTOR4* pVector)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, RawVector4 value)
        {
            SetVector(device, effectHandle, value);
        }

        /// <summary>
        /// Sets a typed value.
        /// </summary>
        /// <typeparam name="T">Type of the value to set</typeparam>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetValue([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] const void* pData,[In] unsigned int Bytes)</unmanaged>
        public void SetValue<T>(Device device, EffectHandle effectHandle, T value) where T : struct
        {
            unsafe
            {
                SetValue(device, effectHandle, (IntPtr)Interop.Fixed(ref value), Utilities.SizeOf<T>());
            }
        }

        /// <summary>
        /// Sets an array of bools.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetBoolArray([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In, Buffer] const BOOL* pb,[In] unsigned int Count)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, bool[] values)
        {
            var tempArray = Utilities.ConvertToIntArray(values);
            SetBoolArray(device, effectHandle, tempArray, values.Length);
        }

        /// <summary>
        /// Sets an array of floats.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetFloatArray([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In, Buffer] const float* pf,[In] unsigned int Count)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, float[] values)
        {
            SetFloatArray(device, effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of ints.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetIntArray([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In, Buffer] const int* pn,[In] unsigned int Count)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, int[] values)
        {
            SetIntArray(device, effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of matrices.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetMatrixArray([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In, Buffer] const D3DXMATRIX* pMatrix,[In] unsigned int Count)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, RawMatrix[] values)
        {
            SetMatrixArray(device, effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of 4D vectors.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetVectorArray([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In, Buffer] const D3DXVECTOR4* pVector,[In] unsigned int Count)</unmanaged>
        public void SetValue(Device device, EffectHandle effectHandle, RawVector4[] values)
        {
            SetVectorArray(device, effectHandle, values, values.Length);
        }

        /// <summary>
        /// Sets an array of elements.
        /// </summary>
        /// <typeparam name="T">Type of the array element</typeparam>
        /// <param name="device">The device.</param>
        /// <param name="effectHandle">The effect handle.</param>
        /// <param name="values">The values.</param>
        /// <returns>
        /// A <see cref="SharpDX.Result"/> object describing the result of the operation.
        /// </returns>
        /// <unmanaged>HRESULT ID3DXConstantTable::SetValue([In] IDirect3DDevice9* pDevice,[In] D3DXHANDLE hConstant,[In] const void* pData,[In] unsigned int Bytes)</unmanaged>
        public void SetValue<T>(Device device, EffectHandle effectHandle, T[] values) where T : struct
        {
            unsafe
            {
                SetValue(device, effectHandle, (IntPtr)Interop.Fixed(values), Utilities.SizeOf<T>() * values.Length);
            }
        }
    }
}

