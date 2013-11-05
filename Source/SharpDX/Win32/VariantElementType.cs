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
namespace SharpDX.Win32
{
    /// <summary>
    /// Type of a simple variant value.
    /// </summary>
    public enum VariantElementType : ushort
    {
        /// <summary>The empty.</summary>
        Empty = 0,

        /// <summary>The null.</summary>
        Null = 1,

        /// <summary>The short.</summary>
        Short = 2,

        /// <summary>The int.</summary>
        Int = 3,

        /// <summary>The float.</summary>
        Float = 4,

        /// <summary>The double.</summary>
        Double = 5,

        /// <summary>The currency.</summary>
        Currency = 6,

        /// <summary>The date.</summary>
        Date = 7,

        /// <summary>The binary string.</summary>
        BinaryString = 8,

        /// <summary>The dispatch.</summary>
        Dispatch = 9,

        /// <summary>The error.</summary>
        Error = 10,

        /// <summary>The bool.</summary>
        Bool = 11,

        /// <summary>The variant.</summary>
        Variant = 12,

        /// <summary>The COM unknown.</summary>
        ComUnknown = 13,

        /// <summary>The decimal.</summary>
        Decimal = 14,

        /// <summary>The byte.</summary>
        Byte = 16,

        /// <summary>The unsigned byte.</summary>
        UByte = 17,

        /// <summary>The unsigned short.</summary>
        UShort = 18,

        /// <summary>The unsigned int.</summary>
        UInt = 19,

        /// <summary>The long.</summary>
        Long = 20,

        /// <summary>The unsigned long.</summary>
        ULong = 21,

        /// <summary>The int1.</summary>
        Int1 = 22,

        /// <summary>The unsigned int1.</summary>
        UInt1 = 23,

        /// <summary>The void.</summary>
        Void = 24,

        /// <summary>The result.</summary>
        Result = 25,

        /// <summary>The pointer.</summary>
        Pointer = 26,

        /// <summary>The safe array.</summary>
        SafeArray = 27,

        /// <summary>The constant array.</summary>
        ConstantArray = 28,

        /// <summary>The user defined.</summary>
        UserDefined = 29,

        /// <summary>The string pointer.</summary>
        StringPointer = 30,

        /// <summary>The "W" string pointer.</summary>
        WStringPointer = 31,

        /// <summary>The recor.</summary>
        Recor = 36,

        /// <summary>The int pointer.</summary>
        IntPointer = 37,

        /// <summary>The unsigned int pointer.</summary>
        UIntPointer = 38,

        /// <summary>The file time.</summary>
        FileTime = 64,

        /// <summary>The BLOB.</summary>
        Blob = 65,

        /// <summary>The stream.</summary>
        Stream = 66,

        /// <summary>The storage.</summary>
        Storage = 67,

        /// <summary>The streamed object.</summary>
        StreamedObject = 68,

        /// <summary>The stored object.</summary>
        StoredObject = 69,

        /// <summary>The BLOB object.</summary>
        BlobObject = 70,

        /// <summary>The clip data.</summary>
        ClipData = 71,

        /// <summary>The CLSID.</summary>
        Clsid = 72,

        /// <summary>The stream with a version.</summary>
        VersionedStream = 73,

        /// <summary>The binary string BLOB.</summary>
        BinaryStringBlob = 0xfff,
    }
}
