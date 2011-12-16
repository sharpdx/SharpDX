// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

namespace SharpDX.Win32
{
    /// <summary>
    /// Variant COM.
    /// </summary>
    /// <unmanaged>PROPVARIANT</unmanaged>
    [StructLayout(LayoutKind.Sequential)]
    public struct Variant
    {
        private ushort vt;
        private ushort reserved1;
        private ushort reserved2;
        private ushort reserved3;
        private VariantValue variantValue;

        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>
        /// The type of the element.
        /// </value>
        public VariantElementType ElementType
        {
            get
            {
                return (VariantElementType)(vt & 0x0fff);
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public VariantType Type
        {
            get
            {
                return (VariantType)(vt & 0xf000);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
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
                                return variantValue.recordValue.RecordPointer;
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
                                return Marshal.PtrToStringBSTR(variantValue.pointerValue);
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
                            default:
                                return null;
                        }
                    case VariantType.Vector:
                        int size = (int)variantValue.recordValue.RecordInfo;
                        switch (ElementType)
                        {
                            case VariantElementType.Bool:
                                {
                                    var array = new int[size];
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
                                    var array = new string[size];
                                    for (int i = 0; i < size; i++)
                                        array[i] = Marshal.PtrToStringBSTR(((IntPtr*)variantValue.recordValue.RecordPointer)[i]);
                                    return array;
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
                            default:
                                return null;
                        }
                }
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct VariantValue
        {
            [FieldOffset(0)]
            public byte byteValue;
            [FieldOffset(0)]
            public sbyte signedByteValue;
            [FieldOffset(0)]
            public ushort ushortValue;
            [FieldOffset(0)]
            public short shortValue;
            [FieldOffset(0)]
            public uint uintValue;
            [FieldOffset(0)]
            public int intValue;
            [FieldOffset(0)]
            public ulong ulongValue;
            [FieldOffset(0)]
            public long longValue;
            [FieldOffset(0)]
            public float floatValue;
            [FieldOffset(0)]
            public double doubleValue;
            [FieldOffset(0)]
            public IntPtr pointerValue;
            [FieldOffset(0)]
            public CurrencyValue currencyValue;
            [FieldOffset(0)]
            public RecordValue recordValue;

            [StructLayout(LayoutKind.Sequential)]
            public struct CurrencyLowHigh
            {
                public uint LowValue;
                public int HighValue;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct CurrencyValue
            {
                [FieldOffset(0)]
                public CurrencyLowHigh LowHigh;
                [FieldOffset(0)]
                public long longValue;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct RecordValue
            {
                public IntPtr RecordInfo;
                public IntPtr RecordPointer;
            }
        };
    }
}

