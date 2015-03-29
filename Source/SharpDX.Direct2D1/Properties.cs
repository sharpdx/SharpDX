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

namespace SharpDX.Direct2D1
{
    public partial class Properties
    {
        /// <summary>
        /// Gets or sets Cached property.
        /// </summary>
        public bool Cached
        {
            get
            {
                return GetBoolValue((int)Property.Cached);
            }
            set
            {
                SetValue((int)Property.Cached, value);
            }
        }

        /// <summary>	
        /// Gets the number of characters for the given property name.
        /// </summary>	
        /// <param name="index"><para>The index of the property for which the name is being returned.</para></param>	
        /// <returns>The name of the property</returns>	
        /// <remarks>	
        /// This method returns an empty string if index is invalid.
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1Properties::GetPropertyName([In] unsigned int index,[Out, Buffer] wchar_t* name,[In] unsigned int nameCount)</unmanaged>	
        public unsafe string GetPropertyName(int index)
        {
            int length = GetPropertyNameLength(index);
            if (length == 0)
                return null;
            // D2D runtime returns the property name as a null-terminated string, so we need an additional char at the end
            var bufferLength = length + 1;
            var pText = stackalloc char[bufferLength];
            GetPropertyName(index, new IntPtr(pText), bufferLength);
            return new string(pText, 0, length); // do not use the last '\0' char
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe int GetIntValue(int index)
        {
            int value;
            this.GetValue(index, PropertyType.Int32, new IntPtr(&value), sizeof(int));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe uint GetUIntValue(int index)
        {
            uint value;
            this.GetValue(index, PropertyType.UInt32, new IntPtr(&value), sizeof(uint));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe float GetFloatValue(int index)
        {
            float value;
            this.GetValue(index, PropertyType.Float, new IntPtr(&value), sizeof(float));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe bool GetBoolValue(int index)
        {
            int value;
            this.GetValue(index, PropertyType.Bool, new IntPtr(&value), sizeof(int));
            return value != 0;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe Guid GetGuidValue(int index)
        {
            Guid value;
            this.GetValue(index, PropertyType.Clsid, new IntPtr(&value), Utilities.SizeOf<Guid>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawVector2 GetVector2Value(int index)
        {
            RawVector2 value;
            this.GetValue(index, PropertyType.Vector2, new IntPtr(&value), Utilities.SizeOf<RawVector2>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawVector3 GetVector3Value(int index)
        {
            RawVector3 value;
            this.GetValue(index, PropertyType.Vector3, new IntPtr(&value), Utilities.SizeOf<RawVector3>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawColor3 GetColor3Value(int index)
        {
            RawColor3 value;
            this.GetValue(index, PropertyType.Vector3, new IntPtr(&value), Utilities.SizeOf<RawColor3>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawVector4 GetVector4Value(int index)
        {
            RawVector4 value;
            this.GetValue(index, PropertyType.Vector4, new IntPtr(&value), Utilities.SizeOf<RawVector4>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawRectangleF GetRectangleFValue(int index)
        {
            RawRectangleF value;
            this.GetValue(index, PropertyType.Vector4, new IntPtr(&value), Utilities.SizeOf<RawRectangleF>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawColor4 GetColor4Value(int index)
        {
            RawColor4 value;
            this.GetValue(index, PropertyType.Vector4, new IntPtr(&value), Utilities.SizeOf<RawColor4>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawMatrix GetMatrixValue(int index)
        {
            RawMatrix value;
            this.GetValue(index, PropertyType.Matrix4x4, new IntPtr(&value), Utilities.SizeOf<RawMatrix>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawMatrix3x2 GetMatrix3x2Value(int index)
        {
            RawMatrix3x2 value;
            this.GetValue(index, PropertyType.Matrix3x2, new IntPtr(&value), Utilities.SizeOf<RawMatrix3x2>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawMatrix5x4 GetMatrix5x4Value(int index)
        {
            RawMatrix5x4 value;
            this.GetValue(index, PropertyType.Matrix5x4, new IntPtr(&value), Utilities.SizeOf<RawMatrix5x4>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe T GetEnumValue<T>(int index) where T : struct
        {
            if (Utilities.SizeOf<T>() != sizeof(int))
                throw new ArgumentException("value", "enum must be sizeof(int)");
            T value = default(T);
            this.GetValue(index, PropertyType.Enum, (IntPtr)Interop.Cast<T>(ref value), sizeof(int));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe T GetComObjectValue<T>(int index) where T : ComObject
        {
            IntPtr ptr;
            this.GetValue(index, PropertyType.IUnknown, new IntPtr(&ptr), Utilities.SizeOf<IntPtr>());
            return ptr == IntPtr.Zero ? null : As<T>(ptr);
        }

        /// <summary>	
        /// Gets the value of the specified property by index.
        /// </summary>	
        /// <param name="index"><para>The index of the property from which the data is to be obtained.</para></param>	
        /// <returns>The value of the specified property by index.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValue([In] unsigned int index,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe T GetValue<T>(int index, PropertyType type) where T : struct
        {
            T value = default(T);
            this.GetValue(index, type, (IntPtr)Interop.Cast<T>(ref value), Utilities.SizeOf<T>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe uint GetUIntValueByName(string name)
        {
            uint value;
            this.GetValueByName(name, PropertyType.UInt32, new IntPtr(&value), sizeof(uint));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe float GetFloatValueByName(string name)
        {
            float value;
            this.GetValueByName(name, PropertyType.Float, new IntPtr(&value), sizeof(float));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe bool GetBoolValueByName(string name)
        {
            int value;
            this.GetValueByName(name, PropertyType.Bool, new IntPtr(&value), sizeof(int));
            return value != 0;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe Guid GetGuidValueByName(string name)
        {
            Guid value;
            this.GetValueByName(name, PropertyType.Clsid, new IntPtr(&value), Utilities.SizeOf<Guid>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawVector2 GetVector2ValueByName(string name)
        {
            RawVector2 value;
            this.GetValueByName(name, PropertyType.Vector2, new IntPtr(&value), Utilities.SizeOf<RawVector2>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawVector3 GetVector3ValueByName(string name)
        {
            RawVector3 value;
            this.GetValueByName(name, PropertyType.Vector3, new IntPtr(&value), Utilities.SizeOf<RawVector3>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawColor3 GetColor3ValueByName(string name)
        {
            RawColor3 value;
            this.GetValueByName(name, PropertyType.Vector3, new IntPtr(&value), Utilities.SizeOf<RawColor3>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawVector4 GetVector4ValueByName(string name)
        {
            RawVector4 value;
            this.GetValueByName(name, PropertyType.Vector4, new IntPtr(&value), Utilities.SizeOf<RawVector4>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawRectangleF GetRectangleFValueByName(string name)
        {
            RawRectangleF value;
            this.GetValueByName(name, PropertyType.Vector4, new IntPtr(&value), Utilities.SizeOf<RawRectangleF>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawColor4 GetColor4ValueByName(string name)
        {
            RawColor4 value;
            this.GetValueByName(name, PropertyType.Vector4, new IntPtr(&value), Utilities.SizeOf<RawColor4>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawMatrix GetMatrixValueByName(string name)
        {
            RawMatrix value;
            this.GetValueByName(name, PropertyType.Matrix4x4, new IntPtr(&value), Utilities.SizeOf<RawMatrix>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawMatrix3x2 GetMatrix3x2ValueByName(string name)
        {
            RawMatrix3x2 value;
            this.GetValueByName(name, PropertyType.Matrix3x2, new IntPtr(&value), Utilities.SizeOf<RawMatrix3x2>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe RawMatrix5x4 GetMatrix5x4ValueByName(string name)
        {
            RawMatrix5x4 value;
            this.GetValueByName(name, PropertyType.Matrix5x4, new IntPtr(&value), Utilities.SizeOf<RawMatrix5x4>());
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe T GetEnumValueByName<T>(string name) where T : struct
        {
            if (Utilities.SizeOf<T>() != sizeof(int))
                throw new ArgumentException("value", "enum must be sizeof(int)");
            T value = default(T);
            this.GetValueByName(name, PropertyType.Enum, (IntPtr)Interop.Cast<T>(ref value), sizeof(int));
            return value;
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>	
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe T GetComObjectValueByName<T>(string name) where T : ComObject
        {
            IntPtr ptr;
            this.GetValueByName(name, PropertyType.IUnknown, new IntPtr(&ptr), Utilities.SizeOf<IntPtr>());
            return ptr == IntPtr.Zero ? null : As<T>(ptr);
        }

        /// <summary>	
        /// Gets the value of the specified property by name.
        /// </summary>	
        /// <param name="name">The name of the property.</param>
        /// <param name="type">Specifies the type of property to get.</param>
        /// <returns>The value of the specified property by name.</returns>	
        /// <unmanaged>HRESULT ID2D1Properties::GetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[Out, Buffer] void* data,[In] unsigned int dataSize)</unmanaged>	
        public unsafe T GetValue<T>(string name, PropertyType type) where T : struct
        {
            T value = default(T);
            this.GetValueByName(name, type, (IntPtr)Interop.Cast<T>(ref value), Utilities.SizeOf<T>());
            return value;
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, int value)
        {
            SetValueByName(name, PropertyType.Int32, new IntPtr(&value), sizeof(int));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, uint value)
        {
            SetValueByName(name, PropertyType.UInt32, new IntPtr(&value), sizeof(uint));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, bool value)
        {
            int boolValue = value ? 1 : 0;
            SetValueByName(name, PropertyType.Bool, new IntPtr(&boolValue), sizeof(int));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, Guid value)
        {
            SetValueByName(name, PropertyType.Clsid, new IntPtr(&value), Utilities.SizeOf<Guid>());
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, float value)
        {
            SetValueByName(name, PropertyType.Float, new IntPtr(&value), sizeof(float));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawVector2 value)
        {
            SetValueByName(name, PropertyType.Vector2, new IntPtr(&value), sizeof(RawVector2));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawColor3 value)
        {
            SetValueByName(name, PropertyType.Vector3, new IntPtr(&value), sizeof(RawColor3));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawVector4 value)
        {
            SetValueByName(name, PropertyType.Vector4, new IntPtr(&value), sizeof(RawVector4));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawRectangleF value)
        {
            SetValueByName(name, PropertyType.Vector4, new IntPtr(&value), sizeof(RawRectangleF));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawColor4 value)
        {
            SetValueByName(name, PropertyType.Vector4, new IntPtr(&value), sizeof(RawColor4));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawMatrix3x2 value)
        {
            SetValueByName(name, PropertyType.Matrix3x2, new IntPtr(&value), sizeof(RawMatrix3x2));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawMatrix value)
        {
            SetValueByName(name, PropertyType.Matrix4x4, new IntPtr(&value), sizeof(RawMatrix));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, RawMatrix5x4 value)
        {
            SetValueByName(name, PropertyType.Matrix5x4, new IntPtr(&value), sizeof(RawMatrix5x4));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName(string name, string value)
        {
            var pValue = Marshal.StringToHGlobalUni(value);
            SetValueByName(name, PropertyType.String, pValue, value != null ? value.Length : 0);
            Marshal.FreeHGlobal(pValue);
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName<T>(string name, T value) where T : ComObject
        {
            var pValue = value == null ? IntPtr.Zero : value.NativePointer;
            SetValueByName(name, PropertyType.IUnknown, new IntPtr(&pValue), Utilities.SizeOf<IntPtr>());
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="type">Specifies the type of property to set.</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValueByName<T>(string name, PropertyType type, T value) where T : struct
        {
            SetValueByName(name, type, (IntPtr)Interop.Cast<T>(ref value), Utilities.SizeOf<T>());
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValueByName([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, int value)
        {
            SetValue(index, PropertyType.Int32, new IntPtr(&value), sizeof(int));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, uint value)
        {
            SetValue(index, PropertyType.UInt32, new IntPtr(&value), sizeof(uint));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, bool value)
        {
            int boolValue = value ? 1 : 0;
            SetValue(index, PropertyType.Bool, new IntPtr(&boolValue), sizeof(int));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, Guid value)
        {
            SetValue(index, PropertyType.Clsid, new IntPtr(&value), Utilities.SizeOf<Guid>());
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, float value)
        {
            SetValue(index, PropertyType.Float, new IntPtr(&value), sizeof(float));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawVector2 value)
        {
            SetValue(index, PropertyType.Vector2, new IntPtr(&value), sizeof(RawVector2));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawVector3 value)
        {
            SetValue(index, PropertyType.Vector3, new IntPtr(&value), sizeof(RawVector3));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawColor3 value)
        {
            SetValue(index, PropertyType.Vector3, new IntPtr(&value), sizeof(RawColor3));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawVector4 value)
        {
            SetValue(index, PropertyType.Vector4, new IntPtr(&value), sizeof(RawVector4));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawRectangleF value)
        {
            SetValue(index, PropertyType.Vector4, new IntPtr(&value), sizeof(RawRectangleF));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawColor4 value)
        {
            SetValue(index, PropertyType.Vector4, new IntPtr(&value), sizeof(RawColor4));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawMatrix3x2 value)
        {
            SetValue(index, PropertyType.Matrix3x2, new IntPtr(&value), sizeof(RawMatrix3x2));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawMatrix value)
        {
            SetValue(index, PropertyType.Matrix4x4, new IntPtr(&value), sizeof(RawMatrix));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, RawMatrix5x4 value)
        {
            SetValue(index, PropertyType.Matrix5x4, new IntPtr(&value), sizeof(RawMatrix5x4));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue(int index, string value)
        {
            var pValue = Marshal.StringToHGlobalUni(value);
            SetValue(index, PropertyType.String, pValue, value != null ? value.Length : 0);
            Marshal.FreeHGlobal(pValue);
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetEnumValue<T>(int index, T value) where T : struct
        {
            if (Utilities.SizeOf<T>() != sizeof(int))
                throw new ArgumentException("value", "enum must be sizeof(int)");
            SetValue(index, PropertyType.Enum, (IntPtr)Interop.Cast<T>(ref value), sizeof(int));
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue<T>(int index, T value) where T : ComObject
        {
            var pValue = value == null ? IntPtr.Zero : value.NativePointer;
            SetValue(index, PropertyType.IUnknown, new IntPtr(&pValue), Utilities.SizeOf<IntPtr>());
        }

        /// <summary>
        /// Sets the named property to the given value.
        /// </summary>
        /// <param name="index">Index of the property</param>
        /// <param name="type">Specifies the type of property to set.</param>
        /// <param name="value">Value of the property</param>
        /// <unmanaged>HRESULT ID2D1Properties::SetValue([In] const wchar_t* name,[In] D2D1_PROPERTY_TYPE type,[In, Buffer] const void* data,[In] unsigned int dataSize)</unmanaged>
        public unsafe void SetValue<T>(int index, PropertyType type, T value) where T : struct
        {
            SetValue(index, type, (IntPtr)Interop.Cast<T>(ref value), Utilities.SizeOf<T>());
        }
    }
}