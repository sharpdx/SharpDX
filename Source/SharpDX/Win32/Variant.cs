// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
    /// <summary>Variant COM.</summary>
    /// <unmanaged>PROPVARIANT</unmanaged>
    [StructLayout(LayoutKind.Sequential)]
    public struct Variant
    {
        /// <summary>The vt.</summary>
        private ushort vt;
        /// <summary>The reserved1.</summary>
        private readonly ushort reserved1;
        /// <summary>The reserved2.</summary>
        private readonly ushort reserved2;
        /// <summary>The reserved3.</summary>
        private readonly ushort reserved3;
        /// <summary>The variant value.</summary>
        private VariantValue variantValue;

        /// <summary>Gets the type of the element.</summary>
        /// <value>The type of the element.</value>
        public VariantElementType ElementType
        {
            get
            {
                return (VariantElementType)(vt & 0x0fff);
            }
            set
            {
                vt = (ushort)((vt & 0xf000) | (ushort)value);
            }
        }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public VariantType Type
        {
            get
            {
                return (VariantType)(vt & 0xf000);
            }
            set
            {
                vt = (ushort)((vt & 0x0fff) | (ushort)value);
            }
        }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        /// <exception cref="System.NotSupportedException">
        /// </exception>
        /// <exception cref="System.ArgumentException"></exception>
        public unsafe object Value
        {
            get
            {
                switch (Type)
                {
                    case VariantType.Default:
                        switch (ElementType)
                        {
                            case VariantElementType.Empty:
                            case VariantElementType.Null:
                                return null;
                            case VariantElementType.Blob:
                                {
                                    var buffer = new byte[(int)variantValue.recordValue.RecordInfo];
                                    if (buffer.Length > 0)
                                    {
                                        Utilities.Read(variantValue.recordValue.RecordPointer, buffer, 0, buffer.Length);
                                    }
                                    return buffer;
                                }
                            case VariantElementType.Bool:
                                return variantValue.intValue != 0;
                            case VariantElementType.Byte:
                                return variantValue.signedByteValue;
                            case VariantElementType.UByte:
                                return variantValue.byteValue;
                            case VariantElementType.UShort:
                                return variantValue.ushortValue;
                            case VariantElementType.Short:
                                return variantValue.shortValue;
                            case VariantElementType.UInt:
                            case VariantElementType.UInt1:
                                return variantValue.uintValue;
                            case VariantElementType.Int:
                            case VariantElementType.Int1:
                                return variantValue.intValue;
                            case VariantElementType.ULong:
                                return variantValue.ulongValue;
                            case VariantElementType.Long:
                                return variantValue.longValue;
                            case VariantElementType.Float:
                                return variantValue.floatValue;
                            case VariantElementType.Double:
                                return variantValue.doubleValue;
                            case VariantElementType.BinaryString:
                                throw new NotSupportedException();
                                //return Marshal.PtrToStringBSTR(variantValue.pointerValue);
                            case VariantElementType.StringPointer:
                                return Marshal.PtrToStringAnsi(variantValue.pointerValue);
                            case VariantElementType.WStringPointer:
                                return Marshal.PtrToStringUni(variantValue.pointerValue);
                            case VariantElementType.ComUnknown:
                            case VariantElementType.Dispatch:
                                return new ComObject(variantValue.pointerValue);
                            case VariantElementType.IntPointer:
                            case VariantElementType.Pointer:
                                return variantValue.pointerValue;
                            case VariantElementType.FileTime:
                                return DateTime.FromFileTime(variantValue.longValue);
                            default:
                                return null;
                        }
                    case VariantType.Vector:
                        int size = (int)variantValue.recordValue.RecordInfo;
                        switch (ElementType)
                        {
                            case VariantElementType.Bool:
                                {
                                    var array = new Bool[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return Utilities.ConvertToBoolArray(array);
                                }
                            case VariantElementType.Byte:
                                {
                                    var array = new sbyte[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.UByte:
                                {
                                    var array = new byte[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.UShort:
                                {
                                    var array = new ushort[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.Short:
                                {
                                    var array = new short[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.UInt:
                            case VariantElementType.UInt1:
                                {
                                    var array = new uint[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.Int:
                            case VariantElementType.Int1:
                                {
                                    var array = new int[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.ULong:
                                {
                                    var array = new ulong[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.Long:
                                {
                                    var array = new long[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.Float:
                                {
                                    var array = new float[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.Double:
                                {
                                    var array = new double[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.BinaryString:
                                {
                                    throw new NotSupportedException();
                                    //var array = new string[size];
                                    //for (int i = 0; i < size; i++)
                                    //    array[i] = Marshal.PtrToStringBSTR(((IntPtr*)variantValue.recordValue.RecordPointer)[i]);
                                    //return array;
                                }
                            case VariantElementType.StringPointer:
                                {
                                    var array = new string[size];
                                    for (int i = 0; i < size; i++)
                                        array[i] = Marshal.PtrToStringAnsi(((IntPtr*)variantValue.recordValue.RecordPointer)[i]);
                                    return array;
                                }
                            case VariantElementType.WStringPointer:
                                {
                                    var array = new string[size];
                                    for (int i = 0; i < size; i++)
                                        array[i] = Marshal.PtrToStringUni(((IntPtr*)variantValue.recordValue.RecordPointer)[i]);
                                    return array;
                                }
                            case VariantElementType.ComUnknown:
                            case VariantElementType.Dispatch:
                                {
                                    var comArray = new ComObject[size];
                                    for (int i = 0; i < size; i++)
                                        comArray[i] = new ComObject(((IntPtr*)variantValue.recordValue.RecordPointer)[i]);
                                    return comArray;
                                }
                            case VariantElementType.IntPointer:
                            case VariantElementType.Pointer:
                                {
                                    var array = new IntPtr[size];
                                    Utilities.Read(variantValue.recordValue.RecordPointer, array, 0, size);
                                    return array;
                                }
                            case VariantElementType.FileTime:
                                {
                                    var fileTimeArray = new DateTime[size];
                                    for (int i = 0; i < size; i++)
                                        fileTimeArray[i] = DateTime.FromFileTime(((long*)variantValue.recordValue.RecordPointer)[i]);
                                    return fileTimeArray;
                                }
                            default:
                                return null;
                        }
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    Type = VariantType.Default;
                    ElementType = VariantElementType.Null;
                    return;
                }
                var type = value.GetType();

                Type = VariantType.Default;
#if W8CORE
                if (type.GetTypeInfo().IsPrimitive)
#else
                if (type.IsPrimitive)
#endif
                {
                    if (type == typeof(int))
                    {
                        ElementType = VariantElementType.Int;
                        variantValue.intValue = (int)value;
                        return;
                    }

                    if (type == typeof(uint))
                    {
                        ElementType = VariantElementType.UInt;
                        variantValue.uintValue = (uint)value;
                        return;
                    }

                    if (type == typeof(long))
                    {
                        ElementType = VariantElementType.Long;
                        variantValue.longValue= (long)value;
                        return;
                    }

                    if (type == typeof(ulong))
                    {
                        ElementType = VariantElementType.ULong;
                        variantValue.ulongValue = (ulong)value;
                        return;
                    }

                    if (type == typeof(short))
                    {
                        ElementType = VariantElementType.Short;
                        variantValue.shortValue= (short)value;
                        return;
                    }

                    if (type == typeof(ushort))
                    {
                        ElementType = VariantElementType.UShort;
                        variantValue.ushortValue = (ushort)value;
                        return;
                    }

                    if (type == typeof(float))
                    {
                        ElementType = VariantElementType.Float;
                        variantValue.floatValue = (float)value;
                        return;
                    }

                    if (type == typeof(double))
                    {
                        ElementType = VariantElementType.Double;
                        variantValue.doubleValue = (double)value;
                        return;
                    }
                }
                else if (value is ComObject)
                {
                    ElementType = VariantElementType.ComUnknown;
                    variantValue.pointerValue = ((ComObject)value).NativePointer;
                    return;
                }
                else if (value is DateTime)
                {
                    ElementType = VariantElementType.FileTime;
                    variantValue.longValue = ((DateTime)value).ToFileTime();
                    return;
                }
#if !WP8
                else if (value is string)
                {
                    ElementType = VariantElementType.WStringPointer;
                    variantValue.pointerValue = Marshal.StringToCoTaskMemUni((string)value);
                    return;
                }
#endif
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Type [{0}] is not handled", type.Name));
            }
        }

        /// <summary>The variant value struct.</summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct VariantValue
        {
            /// <summary>The byte value.</summary>
            [FieldOffset(0)]
            public readonly byte byteValue;
            /// <summary>The signed byte value.</summary>
            [FieldOffset(0)]
            public readonly sbyte signedByteValue;
            /// <summary>The unsigned short value.</summary>
            [FieldOffset(0)]
            public ushort ushortValue;
            /// <summary>The short value.</summary>
            [FieldOffset(0)]
            public short shortValue;
            /// <summary>The unsigned int value.</summary>
            [FieldOffset(0)]
            public uint uintValue;
            /// <summary>The int value.</summary>
            [FieldOffset(0)]
            public int intValue;
            /// <summary>The unsigned long value.</summary>
            [FieldOffset(0)]
            public ulong ulongValue;
            /// <summary>The long value.</summary>
            [FieldOffset(0)]
            public long longValue;
            /// <summary>The float value.</summary>
            [FieldOffset(0)]
            public float floatValue;
            /// <summary>The double value.</summary>
            [FieldOffset(0)]
            public double doubleValue;
            /// <summary>The pointer value.</summary>
            [FieldOffset(0)]
            public IntPtr pointerValue;
            /// <summary>The currency value.</summary>
            [FieldOffset(0)]
            private readonly CurrencyValue currencyValue;
            /// <summary>The record value.</summary>
            [FieldOffset(0)]
            public RecordValue recordValue;

            /// <summary>The currency low high struct.</summary>
            [StructLayout(LayoutKind.Sequential)]
            private struct CurrencyLowHigh
            {
                /// <summary>The low value.</summary>
                private readonly uint LowValue;
                /// <summary>The high value.</summary>
                private readonly int HighValue;
            }

            /// <summary>The currency value struct.</summary>
            [StructLayout(LayoutKind.Explicit)]
            private struct CurrencyValue
            {
                /// <summary>The low high.</summary>
                [FieldOffset(0)]
                private readonly CurrencyLowHigh LowHigh;
                /// <summary>The long value.</summary>
                [FieldOffset(0)]
                private readonly long longValue;
            }

            /// <summary>The record value struct.</summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct RecordValue
            {
                /// <summary>The record information.</summary>
                public readonly IntPtr RecordInfo;
                /// <summary>The record pointer.</summary>
                public readonly IntPtr RecordPointer;
            }
        };
    }
}

