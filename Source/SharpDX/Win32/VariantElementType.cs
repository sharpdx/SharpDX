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
namespace SharpDX.Win32
{
    /// <summary>
    /// Type of a simple variant value.
    /// </summary>
    public enum VariantElementType : ushort
    {
        Empty = 0,
        Null = 1,
        Short = 2,
        Int = 3,
        Float = 4,
        Double = 5,
        Currency = 6,
        Date = 7,
        BinaryString = 8,
        Dispatch = 9,
        Error = 10,
        Bool = 11,
        Variant = 12,
        ComUnknown = 13,
        Decimal = 14,
        Byte = 16,
        UByte = 17,
        UShort = 18,
        UInt = 19,
        Long = 20,
        ULong = 21,
        Int1 = 22,
        UInt1 = 23,
        Void = 24,
        Result = 25,
        Pointer = 26,
        SafeArray = 27,
        ConstantArray = 28,
        UserDefined = 29,
        StringPointer = 30,
        WStringPointer = 31,
        Recor = 36,
        IntPointer = 37,
        UIntPointer = 38,
        FileTime = 64,
        Blob = 65,
        Stream = 66,
        Storage = 67,
        StreamedObject = 68,
        StoredObject = 69,
        BlobObject = 70,
        ClipData = 71,
        Clsid = 72,
        VersionedStream = 73,
        BinaryStringBlob = 0xfff,
    }
}
