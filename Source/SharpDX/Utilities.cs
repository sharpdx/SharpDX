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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using SharpDX.Direct3D;
using System.Reflection;
#if W8CORE
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using SharpDX.Text;

#endif

namespace SharpDX
{

#if !NET35Plus
    /// <summary>
    /// Encapsulates a method that has no parameters and returns a value of the type specified by the TResult parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates. This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. </typeparam>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    public delegate TResult Func<out TResult>();
#endif

    public delegate void VoidAction();

    /// <summary>
    /// A Delegate to get a property value from an object.
    /// </summary>
    /// <typeparam name="T">Type of the getter</typeparam>
    /// <param name="obj">The obj to get the property from</param>
    /// <param name="value">The value to get</param>
    public delegate void GetValueFastDelegate<T>(object obj, out T value);

    /// <summary>
    /// A Delegate to set a property value to an object.
    /// </summary>
    /// <typeparam name="T">Type of the setter</typeparam>
    /// <param name="obj">The obj to set the property from</param>
    /// <param name="value">The value to set</param>
    public delegate void SetValueFastDelegate<T>(object obj, ref T value);

    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Utilities
    {
        ///// <summary>
        ///// Native memcpy.
        ///// </summary>
        ///// <param name="dest">The destination memory location</param>
        ///// <param name="src">The source memory location.</param>
        ///// <param name="sizeInBytesToCopy">The count.</param>
        ///// <returns></returns>
        //[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
        //    SetLastError = false), SuppressUnmanagedCodeSecurity]
        //public static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, ulong sizeInBytesToCopy);

        /// <summary>
        /// Native memcpy.
        /// </summary>
        /// <param name="dest">The destination memory location</param>
        /// <param name="src">The source memory location.</param>
        /// <param name="sizeInBytesToCopy">The count.</param>
        /// <returns></returns>
        public static void CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy)
        {
            unsafe
            {
                // TODO plug in Interop a pluggable CopyMemory using cpblk or memcpy based on architecture
                Interop.memcpy((void*)dest, (void*)src, sizeInBytesToCopy);
            }
        }

        /// <summary>
        /// Compares two block of memory.
        /// </summary>
        /// <param name="from">The pointer to compare from.</param>
        /// <param name="against">The pointer to compare against.</param>
        /// <param name="sizeToCompare">The size in bytes to compare.</param>
        /// <returns>True if the buffers are equivalent, false otherwise.</returns>
        public unsafe static bool CompareMemory(IntPtr from, IntPtr against, int sizeToCompare)
        {
            var pSrc = (byte*)@from;
            var pDst = (byte*)against;

            // Compare 8 bytes.
            int numberOf = sizeToCompare >> 3;
            while (numberOf > 0)
            {
                if (*(long*)pSrc != *(long*)pDst)
                    return false;
                pSrc += 8;
                pDst += 8;
                numberOf--;
            }

            // Compare remaining bytes.
            numberOf = sizeToCompare & 7;
            while (numberOf > 0)
            {
                if (*pSrc != *pDst)
                    return false;
                pSrc++;
                pDst++;
                numberOf--;
            }

            return true;
        }

        /// <summary>
        /// Clears the memory.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="value">The value.</param>
        /// <param name="sizeInBytesToClear">The size in bytes to clear.</param>
        public static void ClearMemory(IntPtr dest, byte value, int sizeInBytesToClear)
        {
            unsafe
            {
                Interop.memset((void*)dest, value, sizeInBytesToClear);
            }
        }

        /// <summary>
        /// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">a struct to evaluate</typeparam>
        /// <returns>sizeof this struct</returns>
        public static int SizeOf<T>() where T : struct
        {
            return Interop.SizeOf<T>();            
        }

        /// <summary>
        /// Return the sizeof an array of struct. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">a struct</typeparam>
        /// <param name="array">The array of struct to evaluate.</param>
        /// <returns>sizeof in bytes of this array of struct</returns>
        public static int SizeOf<T>(T[] array) where T : struct
        {
            return array == null ? 0 : array.Length * Interop.SizeOf<T>();
        }

        /// <summary>
        /// Pins the specified source and call an action with the pinned pointer.
        /// </summary>
        /// <typeparam name="T">The type of the structure to pin</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="pinAction">The pin action to perform on the pinned pointer.</param>
        public static void Pin<T>(ref T source, Action<IntPtr> pinAction) where T : struct
        {
            unsafe
            {
                pinAction((IntPtr)Interop.Fixed(ref source));
            }
        }

        /// <summary>
        /// Pins the specified source and call an action with the pinned pointer.
        /// </summary>
        /// <typeparam name="T">The type of the structure to pin</typeparam>
        /// <param name="source">The source array.</param>
        /// <param name="pinAction">The pin action to perform on the pinned pointer.</param>
        public static void Pin<T>(T[] source, Action<IntPtr> pinAction) where T : struct
        {
            unsafe
            {
                pinAction(source == null ? IntPtr.Zero : (IntPtr)Interop.Fixed(source));
            }
        }

        /// <summary>
        /// Covnerts a structured array to an equivalent byte array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static byte[] ToByteArray<T>(T[] source) where T : struct
        {
            if (source == null) return null;

            var buffer = new byte[SizeOf<T>() * source.Length];

            if (source.Length == 0)
                return buffer;

            unsafe
            {
                fixed (void* pBuffer = buffer)
                    Interop.Write(pBuffer, source, 0, source.Length);
            }
            return buffer;
        }

        /// <summary>
        /// Reads the specified T data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <returns>The data read from the memory location</returns>
        public static T Read<T>(IntPtr source) where T : struct
        {
            unsafe
            {
                return Interop.ReadInline<T>((void*)source);
            }
        }

        /// <summary>
        /// Reads the specified T data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T)</returns>
        public static void Read<T>(IntPtr source, ref T data) where T : struct
        {
            unsafe
            {
                Interop.CopyInline(ref data, (void*)source);
            }
        }

        /// <summary>
        /// Reads the specified T data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T)</returns>
        public static void ReadOut<T>(IntPtr source, out T data) where T : struct
        {
            unsafe
            {
                Interop.CopyInlineOut(out data, (void*)source);
            }
        }

        /// <summary>
        /// Reads the specified T data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T)</returns>
        public static IntPtr ReadAndPosition<T>(IntPtr source, ref T data) where T : struct
        {
            unsafe
            {
                return (IntPtr)Interop.Read((void*)source, ref data);
            }
        }

        /// <summary>
        /// Reads the specified array T[] data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <param name="offset">The offset in the array to write to.</param>
        /// <param name="count">The number of T element to read from the memory location</param>
        /// <returns>source pointer + sizeof(T) * count</returns>
        public static IntPtr Read<T>(IntPtr source, T[] data, int offset, int count) where T : struct
        {
            unsafe
            {
                return (IntPtr)Interop.Read((void*)source, data, offset, count);
            }
        }

        /// <summary>
        /// Writes the specified T data to a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to write</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The data to write.</param>
        /// <returns>destination pointer + sizeof(T)</returns>
        public static void Write<T>(IntPtr destination, ref T data) where T : struct
        {
            unsafe
            {
                Interop.CopyInline((void*)destination, ref data);
            }
        }

        /// <summary>
        /// Writes the specified T data to a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to write</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The data to write.</param>
        /// <returns>destination pointer + sizeof(T)</returns>
        public static IntPtr WriteAndPosition<T>(IntPtr destination, ref T data) where T : struct
        {
            unsafe
            {
                return (IntPtr)Interop.Write((void*)destination, ref data);
            }
        }

        /// <summary>
        /// Writes the specified array T[] data to a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to write</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The array of T data to write.</param>
        /// <param name="offset">The offset in the array to read from.</param>
        /// <param name="count">The number of T element to write to the memory location</param>
        /// <returns>destination pointer + sizeof(T) * count</returns>
        public static IntPtr Write<T>(IntPtr destination, T[] data, int offset, int count) where T : struct
        {
            unsafe
            {
                return (IntPtr)Interop.Write((void*)destination, data, offset, count);
            }
        }

        public unsafe static void ConvertToIntArray(bool[] array, int* dest)
        {
            for (int i = 0; i < array.Length; i++)
                dest[i] = array[i] ? 1 : 0;
        }

        public unsafe static bool[] ConvertToBoolArray(int* array, int length)
        {
            var temp = new bool[length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i] != 0;
            return temp;
        }

        /// <summary>
        /// Converts to int array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static Bool[] ConvertToIntArray(bool[] array)
        {
            var temp = new Bool[array.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i];
            return temp;
        }

        /// <summary>
        /// Converts to bool array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static bool[] ConvertToBoolArray(Bool[] array)
        {
            var temp = new bool[array.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i];
            return temp;
        }

        /// <summary>
        /// Gets the <see cref="System.Guid"/> from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The guid associated with this type</returns>
        public static Guid GetGuidFromType(Type type)
        {
#if W8CORE
            return type.GetTypeInfo().GUID;
#else
            return type.GUID;
#endif
        }

        /// <summary>
        /// Allocate an aligned memory buffer.
        /// </summary>
        /// <param name="sizeInBytes">Size of the buffer to allocate.</param>
        /// <param name="align">Alignment, 16 bytes by default.</param>
        /// <returns>A pointer to a buffer aligned.</returns>
        /// <remarks>
        /// To free this buffer, call <see cref="FreeMemory"/>
        /// </remarks>
        public unsafe static IntPtr AllocateMemory(int sizeInBytes, int align = 16)
        {
            int mask = align - 1;
            var memPtr = Marshal.AllocHGlobal(sizeInBytes + mask + IntPtr.Size);
            var ptr = (long)((byte*)memPtr + sizeof(void*) + mask) & ~mask;
            ((IntPtr*)ptr)[-1] = memPtr;
            return new IntPtr(ptr);
        }

        /// <summary>
        /// Allocate an aligned memory buffer and clear it with a specified value (0 by defaault).
        /// </summary>
        /// <param name="sizeInBytes">Size of the buffer to allocate.</param>
        /// <param name="clearValue">Default value used to clear the buffer.</param>
        /// <param name="align">Alignment, 16 bytes by default.</param>
        /// <returns>A pointer to a buffer aligned.</returns>
        /// <remarks>
        /// To free this buffer, call <see cref="FreeMemory"/>
        /// </remarks>
        public static IntPtr AllocateClearedMemory(int sizeInBytes, byte clearValue = 0, int align = 16)
        {
            var ptr = AllocateMemory(sizeInBytes, align);
            ClearMemory(ptr, clearValue, sizeInBytes);
            return ptr;
        }

        /// <summary>
        /// Determines whether the specified memory pointer is aligned in memory.
        /// </summary>
        /// <param name="memoryPtr">The memory pointer.</param>
        /// <param name="align">The align.</param>
        /// <returns><c>true</c> if the specified memory pointer is aligned in memory; otherwise, <c>false</c>.</returns>
        public static bool IsMemoryAligned(IntPtr memoryPtr, int align = 16)
        {
            return ((memoryPtr.ToInt64() & (align-1)) == 0);
        }

        /// <summary>
        /// Allocate an aligned memory buffer.
        /// </summary>
        /// <returns>A pointer to a buffer aligned.</returns>
        /// <remarks>
        /// The buffer must have been allocated with <see cref="AllocateMemory"/>
        /// </remarks>
        public unsafe static void FreeMemory(IntPtr alignedBuffer)
        {
            Marshal.FreeHGlobal(((IntPtr*) alignedBuffer)[-1]);
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an ansi null string.</param>
        /// <param name="maxLength">Maximum length of the string</param>
        /// <returns></returns>
        public static string PtrToStringAnsi(IntPtr pointer, int maxLength)
        {
#if W8CORE
            return Marshal.PtrToStringAnsi(pointer, maxLength);
#else
            unsafe
            {
                var pStr = (byte*)pointer;
                for (int i = 0; i < maxLength; i++) 
                    if (*pStr++ == 0 )
                        return new string((sbyte*)pointer);
                return new string((sbyte*)pointer, 0, maxLength);
            }
#endif
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an unicode null string.</param>
        /// <param name="maxLength">Maximum length of the string</param>
        /// <returns></returns>
        public static string PtrToStringUni(IntPtr pointer, int maxLength)
        {
#if W8CORE
            return Marshal.PtrToStringUni(pointer, maxLength);
#else
            unsafe
            {
                var pStr = (char*)pointer;
                for (int i = 0; i < maxLength; i++)
                    if (*pStr++ == 0)
                        return new string((char*)pointer);
                return new string((char*)pointer, 0, maxLength);
            }
#endif
        }

        public static unsafe IntPtr StringToHGlobalAnsi(string s)
        {
#if WP8
            if (s == null)
            {
                return IntPtr.Zero;
            }
            int cbNativeBuffer = (s.Length + 1) * 4;
            var ptr2 = Marshal.AllocHGlobal(cbNativeBuffer);
            if (ptr2 == IntPtr.Zero)
            {
                throw new OutOfMemoryException();
            }
            fixed (char* chRef = s)
            {
                int count = ASCIIEncoding.ASCII.GetBytes(chRef, s.Length, (byte*) ptr2, cbNativeBuffer);
                ((byte*) ptr2)[count] = 0;
            }

            return ptr2;
#else
            return Marshal.StringToHGlobalAnsi(s);
#endif
        }


        public static unsafe IntPtr StringToHGlobalUni(string s)
        {
#if WP8
            if (s == null)
            {
                return IntPtr.Zero;
            }
            int num = (s.Length + 1) * 2;
            if (num < s.Length)
            {
                throw new ArgumentOutOfRangeException("s");
            }
            IntPtr ptr2 = Marshal.AllocHGlobal(num);
            if (ptr2 == IntPtr.Zero)
            {
                throw new OutOfMemoryException();
            }
            // Completely unefficient, but this is the only to the a string in WP8
            var localArray = s.ToCharArray();
            fixed (char* str = localArray)
            {
                char* pSrc = str;
                Utilities.CopyMemory(ptr2, new IntPtr(pSrc), s.Length + 1);
            }
            return ptr2;
#else
            return Marshal.StringToHGlobalUni(s);
#endif
        }
 



        /// <summary>
        /// Gets the IUnknown from object. Similar to <see cref="Marshal.GetIUnknownForObject"/> but accept null object
        /// by returning an IntPtr.Zero IUnknown pointer.
        /// </summary>
        /// <param name="obj">The managed object.</param>
        /// <returns>an IUnknown pointer to a  managed object</returns>
        public static IntPtr GetIUnknownForObject(object obj)
        {
            IntPtr objPtr =  obj == null ? IntPtr.Zero : Marshal.GetIUnknownForObject(obj);
            //if (obj is ComObject && ((ComObject)obj).NativePointer == IntPtr.Zero) 
            //    (((ComObject)obj).NativePointer) = objPtr;
            return objPtr;
        }

        /// <summary>
        /// Gets an object from an IUnknown pointer. Similar to <see cref="Marshal.GetObjectForIUnknown"/> but accept IntPtr.Zero
        /// by returning a null object.
        /// </summary>
        /// <param name="iunknownPtr">an IUnknown pointer to a  managed object</param>
        /// <returns>The managed object.</returns>
        public static object GetObjectForIUnknown(IntPtr iunknownPtr)
        {
            return iunknownPtr == IntPtr.Zero ? null : Marshal.GetObjectForIUnknown(iunknownPtr);
        }

        /// <summary>
        /// String helper join method to display an array of object as a single string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="array">The array.</param>
        /// <returns>a string with array elements serparated by the seperator</returns>
        public static string Join<T>(string separator, T[] array)
        {
            var text = new StringBuilder();
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0) text.Append(separator);
                    text.Append(array[i]);
                }
            }
            return text.ToString();
        }

        /// <summary>
        /// String helper join method to display an enumrable of object as a single string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="elements">The enumerable.</param>
        /// <returns>a string with array elements serparated by the seperator</returns>
        public static string Join(string separator, IEnumerable elements)
        {
            var elementList = new List<string>();
            foreach (var element in elements)
                elementList.Add(element.ToString());

            var text = new StringBuilder();
            for (int i = 0; i < elementList.Count; i++)
            {
                var element = elementList[i];
                if (i > 0) text.Append(separator);
                text.Append(element);
            }
            return text.ToString();
        }

        /// <summary>
        /// String helper join method to display an enumrable of object as a single string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="elements">The enumerable.</param>
        /// <returns>a string with array elements serparated by the seperator</returns>
        public static string Join(string separator, IEnumerator elements)
        {
            var elementList = new List<string>();
            while (elements.MoveNext())
                elementList.Add(elements.Current.ToString());

            var text = new StringBuilder();
            for (int i = 0; i < elementList.Count; i++)
            {
                var element = elementList[i];
                if (i > 0) text.Append(separator);
                text.Append(element);
            }
            return text.ToString();
        }

        /// <summary>
        /// Converts a blob to a string.
        /// </summary>
        /// <param name="blob">A blob.</param>
        /// <returns>A string extracted from a blob.</returns>
        public static string BlobToString(Blob blob)
        {
            if (blob == null) return null;
            string output;
#if W8CORE
            output = Marshal.PtrToStringAnsi(blob.BufferPointer);
#else
            unsafe
            {
                output = new string((sbyte*) blob.BufferPointer);
            }
#endif
            blob.Dispose();
            return output;
        }

        /// <summary>
        /// Equivalent to IntPtr.Add method from 3.5+ .NET Framework.
        /// </summary>
        /// <param name="ptr">A native pointer</param>
        /// <param name="offset">The offset to add (number of bytes)</param>
        /// <returns></returns>
        public unsafe static IntPtr IntPtrAdd(IntPtr ptr, int offset)
        {
            return new IntPtr(((byte*) ptr) + offset);
        }

        /// <summary>
        ///   Read stream to a byte[] buffer
        /// </summary>
        /// <param name = "stream">input stream</param>
        /// <returns>a byte[] buffer</returns>
        public static byte[] ReadStream(Stream stream)
        {
            int readLength = 0;
            return ReadStream(stream, ref readLength);
        }

        /// <summary>
        ///   Read stream to a byte[] buffer
        /// </summary>
        /// <param name = "stream">input stream</param>
        /// <param name = "readLength">length to read</param>
        /// <returns>a byte[] buffer</returns>
        public static byte[] ReadStream(Stream stream, ref int readLength)
        {
            Debug.Assert(stream != null);
            Debug.Assert(stream.CanRead);
            int num = readLength;
            Debug.Assert(num <= (stream.Length - stream.Position));
            if (num == 0)
                readLength = (int) (stream.Length - stream.Position);
            num = readLength;

            Debug.Assert(num >= 0);
            if (num == 0)
                return new byte[0];

            byte[] buffer = new byte[num];
            int bytesRead = 0;
            if (num > 0)
            {
                do
                {
                    bytesRead += stream.Read(buffer, bytesRead, readLength - bytesRead);
                } while (bytesRead < readLength);
            }
            return buffer;
        }

        /// <summary>
        /// Compares two collection, element by elements.
        /// </summary>
        /// <param name="left">A "from" enumerator.</param>
        /// <param name="right">A "to" enumerator.</param>
        /// <returns>True if lists are identical. False otherwise.</returns>
        public static bool Compare(IEnumerable left, IEnumerable right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return Compare(left.GetEnumerator(), right.GetEnumerator());
        }

        /// <summary>
        /// Compares two collection, element by elements.
        /// </summary>
        /// <param name="leftIt">A "from" enumerator.</param>
        /// <param name="rightIt">A "to" enumerator.</param>
        /// <returns>True if lists are identical. False otherwise.</returns>
        public static bool Compare(IEnumerator leftIt, IEnumerator rightIt)
        {
            if (ReferenceEquals(leftIt, rightIt))
                return true;
            if (ReferenceEquals(leftIt, null) || ReferenceEquals(rightIt, null))
                return false;

            bool hasLeftNext;
            bool hasRightNext;
            while (true)
            {
                hasLeftNext = leftIt.MoveNext();
                hasRightNext = rightIt.MoveNext();
                if (!hasLeftNext || !hasRightNext)
                    break;

                if (!Equals(leftIt.Current, rightIt.Current))
                    return false;
            }

            // If there is any left element
            if (hasLeftNext != hasRightNext)
                return false;

            return true;
        }

        /// <summary>
        /// Compares two collection, element by elements.
        /// </summary>
        /// <param name="left">The collection to compare from.</param>
        /// <param name="right">The colllection to compare to.</param>
        /// <returns>True if lists are identical (but no necessarely of the same time). False otherwise.</returns>
        public static bool Compare(ICollection left, ICollection right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            if (left.Count != right.Count)
                return false;

            int count = 0;
            var leftIt = left.GetEnumerator();
            var rightIt = right.GetEnumerator();
            while (leftIt.MoveNext() && rightIt.MoveNext())
            {
                if (!Equals(leftIt.Current, rightIt.Current))
                    return false;
                count++;
            }

            // Just double check to make sure that the iterator actually returns
            // the exact number of elements
            if (count != left.Count)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T">Type of the custom attribute</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherited">if set to <c>true</c> [inherited].</param>
        /// <returns>The custom attribute or null if not found</returns>
        public static T GetCustomAttribute<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
        {
#if W8CORE
            return memberInfo.GetCustomAttribute<T>(inherited);
#else
            var result = memberInfo.GetCustomAttributes(typeof(T), inherited);
            if (result.Length == 0)
                return default(T);
            return (T)result[0];
#endif
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T">Type of the custom attribute</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherited">if set to <c>true</c> [inherited].</param>
        /// <returns>The custom attribute or null if not found</returns>
        public static IEnumerable<T> GetCustomAttributes<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
        {
#if W8CORE
            return memberInfo.GetCustomAttributes<T>(inherited);
#else
            var result = memberInfo.GetCustomAttributes(typeof(T), inherited);
            if (result.Length == 0)
                return new T[0];
            var typedResult = new T[result.Length];
            Array.Copy(result, typedResult, result.Length);
            return typedResult;
#endif
        }

        /// <summary>
        /// Determines whether fromType can be assigned to toType.
        /// </summary>
        /// <param name="toType">To type.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>
        ///   <c>true</c> if [is assignable from] [the specified to type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableFrom(Type toType, Type fromType)
        {
#if W8CORE
            return toType.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
#else
            return toType.IsAssignableFrom(fromType);
#endif
        }

        /// <summary>
        /// Determines whether the specified type to test is an enum.
        /// </summary>
        /// <param name="typeToTest">The type to test.</param>
        /// <returns>
        ///   <c>true</c> if the specified type to test is an enum; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEnum(Type typeToTest)
        {
#if W8CORE
            return typeToTest.GetTypeInfo().IsEnum;
#else
            return typeToTest.IsEnum;
#endif
        }

        /// <summary>
        /// Determines whether the specified type to test is a valuetype.
        /// </summary>
        /// <param name="typeToTest">The type to test.</param>
        /// <returns>
        ///   <c>true</c> if the specified type to test is a valuetype; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValueType(Type typeToTest)
        {
#if W8CORE
            return typeToTest.GetTypeInfo().IsValueType;
#else
            return typeToTest.IsValueType;
#endif
        }

        private static MethodInfo GetMethod(Type type, string name, Type[] typeArgs) {
#if W8CORE

            foreach( var method in type.GetTypeInfo().GetDeclaredMethods(name)) {
                if ( method.GetParameters().Length == typeArgs.Length) {
                    var parameters = method.GetParameters();
                    bool methodFound = true;
                    for (int i = 0; i < typeArgs.Length; i++)
			        {
                        if (parameters[i].ParameterType != typeArgs[i]) {
                            methodFound = false;
                            break;
                        }
                    }
                    if (methodFound) {
                        return method;
                    }
                }
            }
            return null;
#else
            return type.GetMethod(name, typeArgs);
#endif
        }

        /// <summary>
        /// Builds a fast property getter from a type and a property info.
        /// </summary>
        /// <typeparam name="T">Type of the getter</typeparam>
        /// <param name="customEffectType">Type of the custom effect.</param>
        /// <param name="propertyInfo">The property info to get the value from.</param>
        /// <returns>A compiled delegate </returns>
        public static GetValueFastDelegate<T> BuildPropertyGetter<T>(Type customEffectType, PropertyInfo propertyInfo)
        {
#if W8CORE

            var valueParam = Expression.Parameter(typeof(T).MakeByRefType());
            var objectParam = Expression.Parameter(typeof(object));
            var castParam = Expression.Convert(objectParam, customEffectType);
            var propertyAccessor = Expression.Property(castParam, propertyInfo);

            Expression convertExpression;
            if (propertyInfo.PropertyType == typeof(bool))
            {
                // Convert bool to int: effect.Property ? 1 : 0
                convertExpression = Expression.Condition(propertyAccessor, Expression.Constant(1), Expression.Constant(0));
            }
            else
            {
                convertExpression = Expression.Convert(propertyAccessor, typeof(T));
            }
            return Expression.Lambda<GetValueFastDelegate<T>>(Expression.Assign(valueParam, convertExpression), objectParam, valueParam).Compile();
#else
            var typeT = typeof(T);
            var propertyType = propertyInfo.PropertyType;
            var method = new DynamicMethod("GetValueDelegate", typeof(void), new[] { typeof(object), typeT.MakeByRefType() });

            var ilGenerator = method.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, customEffectType);
            ilGenerator.EmitCall(OpCodes.Callvirt, propertyInfo.GetGetMethod(), null);

            if (typeT == typeof(byte) || typeT == typeof(sbyte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
            }
            else if (typeT == typeof(short) || typeT == typeof(ushort))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
            }
            else if (typeT == typeof(int) || typeT == typeof(uint))
            {
                // If property type is bool, convert it to int first
                if (propertyType == typeof(bool))
                {
                    ilGenerator.EmitCall(OpCodes.Call,  GetMethod(typeof(Convert), "ToInt32", new[] { typeof(bool) }), null);
                }
                ilGenerator.Emit(OpCodes.Stind_I4);
            }
            else if (typeT == typeof(long) || typeT == typeof(ulong))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
            }
            else if (typeT == typeof(float))
            {
                ilGenerator.Emit(OpCodes.Stind_R4);
            }
            else if (typeT == typeof(double))
            {
                ilGenerator.Emit(OpCodes.Stind_R8);
            }
            else
            {
                var castMethod = FindExplicitConverstion(propertyType, typeT);
                if (castMethod != null)
                {
                    ilGenerator.EmitCall(OpCodes.Call, castMethod, null);
                }
                ilGenerator.Emit(OpCodes.Stobj, typeof(T));
            }
            ilGenerator.Emit(OpCodes.Ret);
            return (GetValueFastDelegate<T>)method.CreateDelegate(typeof(GetValueFastDelegate<T>));
#endif
        }

        /// <summary>
        /// Builds a fast property setter from a type and a property info.
        /// </summary>
        /// <typeparam name="T">Type of the setter</typeparam>
        /// <param name="customEffectType">Type of the custom effect.</param>
        /// <param name="propertyInfo">The property info to set the value to.</param>
        /// <returns>A compiled delegate</returns>
        public static SetValueFastDelegate<T> BuildPropertySetter<T>(Type customEffectType, PropertyInfo propertyInfo)
        {
#if W8CORE
            var valueParam = Expression.Parameter(typeof(T).MakeByRefType());
            var objectParam = Expression.Parameter(typeof(object));
            var castParam = Expression.Convert(objectParam, customEffectType);
            var propertyAccessor = Expression.Property(castParam, propertyInfo);

            Expression convertExpression;
            if (propertyInfo.PropertyType == typeof(bool))
            {
                // Convert int to bool: value != 0
                convertExpression = Expression.NotEqual(valueParam, Expression.Constant(0));
            }
            else
            {
                convertExpression = Expression.Convert(valueParam, propertyInfo.PropertyType);
            }
            return Expression.Lambda<SetValueFastDelegate<T>>(Expression.Assign(propertyAccessor, convertExpression), objectParam, valueParam).Compile();
#else

            var typeT = typeof(T);
            var propertyType = propertyInfo.PropertyType;
            var method = new DynamicMethod("SetValueDelegate", typeof(void), new[] { typeof(object), typeT.MakeByRefType() });

            //L_0000: nop 
            //L_0001: ldarg.0 
            //L_0002: castclass TestEmitGetSet.MyCustomEffect
            //L_0007: ldarg.1 
            //L_0008: ldind.i4 
            //L_0009: callvirt instance void TestEmitGetSet.MyCustomEffect::set_Toto(int32)
            //L_000e: nop 
            //L_000f: ret 

            var ilGenerator = method.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, customEffectType);
            ilGenerator.Emit(OpCodes.Ldarg_1);

            if (typeT == typeof(byte) || typeT == typeof(sbyte))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
            }
            else if (typeT == typeof(short) || typeT == typeof(ushort))
            {
                ilGenerator.Emit(OpCodes.Ldind_I2);
            }
            else if (typeT == typeof(int) || typeT == typeof(uint))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                // If property type is bool, convert it to int first
                if (propertyType == typeof(bool))
                {
                    ilGenerator.EmitCall(OpCodes.Call, GetMethod(typeof(Convert),"ToBoolean", new[] { typeT }), null);
                }
            }
            else if (typeT == typeof(long) || typeT == typeof(ulong))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
            }
            else if (typeT == typeof(float))
            {
                ilGenerator.Emit(OpCodes.Ldind_R4);
            }
            else if (typeT == typeof(double))
            {
                ilGenerator.Emit(OpCodes.Ldind_R8);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldobj, typeof(T));

                var castMethod = FindExplicitConverstion(typeT, propertyType);
                if (castMethod != null)
                {
                    ilGenerator.EmitCall(OpCodes.Call, castMethod, null);
                }
            }

            ilGenerator.EmitCall(OpCodes.Callvirt, propertyInfo.GetSetMethod(), null);

            ilGenerator.Emit(OpCodes.Ret);
            return (SetValueFastDelegate<T>)method.CreateDelegate(typeof(SetValueFastDelegate<T>));
#endif
        }

        /// <summary>
        /// Suspends the current thread of a <see cref="sleepTimeInMillis" />.
        /// </summary>
        /// <param name="sleepTimeInMillis">The duration to sleep in milliseconds.</param>
        public static void Sleep(TimeSpan sleepTimeInMillis)
        {
#if WIN8METRO
            Task.Delay(sleepTimeInMillis).Wait();
#else
            System.Threading.Thread.Sleep(sleepTimeInMillis);
#endif            
        }

        /// <summary>
        /// Finds an explicit converstion between a source type and a target type
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>The method to perform the conversion. null if not found</returns>
        private static MethodInfo FindExplicitConverstion(Type sourceType, Type targetType)
        {
            // No need for cast for similar source and target type
            if (sourceType == targetType)
                return null;

            var methods = new List<MethodInfo>();

            var tempType = sourceType;
            while (tempType != null)
            {
#if W8CORE
                methods.AddRange(tempType.GetTypeInfo().DeclaredMethods); //target methods will be favored in the search
                tempType = tempType.GetTypeInfo().BaseType;
#else
                methods.AddRange(tempType.GetMethods(BindingFlags.Static | BindingFlags.Public)); //target methods will be favored in the search
                tempType = tempType.BaseType;
#endif
            }

            tempType = targetType;
            while (tempType != null)
            {
#if W8CORE
                methods.AddRange(tempType.GetTypeInfo().DeclaredMethods); //target methods will be favored in the search
                tempType = tempType.GetTypeInfo().BaseType;
#else
                methods.AddRange(tempType.GetMethods(BindingFlags.Static | BindingFlags.Public)); //target methods will be favored in the search
                tempType = tempType.BaseType;
#endif
            }

            foreach (MethodInfo mi in methods)
            {
                if (mi.Name == "op_Explicit") //will return target and take one parameter
                    if (mi.ReturnType == targetType)
                        if (IsAssignableFrom(mi.GetParameters()[0].ParameterType, sourceType))
                            return mi;
            }

            return null;
        }

        [Flags]
        public enum CLSCTX : uint
        {
            ClsctxInprocServer = 0x1,
            ClsctxInprocHandler = 0x2,
            ClsctxLocalServer = 0x4,
            ClsctxInprocServer16 = 0x8,
            ClsctxRemoteServer = 0x10,
            ClsctxInprocHandler16 = 0x20,
            ClsctxReserved1 = 0x40,
            ClsctxReserved2 = 0x80,
            ClsctxReserved3 = 0x100,
            ClsctxReserved4 = 0x200,
            ClsctxNoCodeDownload = 0x400,
            ClsctxReserved5 = 0x800,
            ClsctxNoCustomMarshal = 0x1000,
            ClsctxEnableCodeDownload = 0x2000,
            ClsctxNoFailureLog = 0x4000,
            ClsctxDisableAaa = 0x8000,
            ClsctxEnableAaa = 0x10000,
            ClsctxFromDefaultContext = 0x20000,
            ClsctxInproc = ClsctxInprocServer | ClsctxInprocHandler,
            ClsctxServer = ClsctxInprocServer | ClsctxLocalServer | ClsctxRemoteServer,
            ClsctxAll = ClsctxServer | ClsctxInprocHandler
        }

#if W8CORE
        [StructLayout(LayoutKind.Sequential)]
        public struct MultiQueryInterface
        {
            public IntPtr InterfaceIID;
            public IntPtr IUnknownPointer;
            public Result ResultCode;
        };


#if WP8

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate Result CoCreateInstanceFromAppDelegate([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, 
            IntPtr pUnkOuter, 
            CLSCTX dwClsContext, 
            IntPtr reserved,
            int countMultiQuery,
            ref MultiQueryInterface query);

        private static CoCreateInstanceFromAppDelegate CoCreateInstanceFromApp;

        internal unsafe static void CreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, ComObject comObject)
        {
            if (CoCreateInstanceFromApp == null)
            {
                CoCreateInstanceFromApp =
                    (CoCreateInstanceFromAppDelegate)
                    Marshal.GetDelegateForFunctionPointer(new IntPtr(SharpDX.WP8.Interop.CoCreateInstanceFromApp()),
                                                          typeof (CoCreateInstanceFromAppDelegate));
            }

            MultiQueryInterface localQuery = new MultiQueryInterface()
            {
                InterfaceIID = new IntPtr(&riid),
                IUnknownPointer = IntPtr.Zero,
                ResultCode = 0,
            };

            var result = CoCreateInstanceFromApp(clsid, IntPtr.Zero, clsctx, IntPtr.Zero, 1, ref localQuery);
            result.CheckError();
            localQuery.ResultCode.CheckError();
            comObject.NativePointer = localQuery.IUnknownPointer;
        }
#else
        // TODO THIS IS NOT TESTED under W8CORE
        [DllImport("ole32.dll", ExactSpelling = true, EntryPoint = "CoCreateInstanceFromApp", PreserveSig = true)]
        private static extern Result CoCreateInstanceFromApp([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, 
            IntPtr pUnkOuter, 
            CLSCTX dwClsContext, 
            IntPtr reserved,
            int countMultiQuery,
            ref MultiQueryInterface query);

        internal unsafe static void CreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, ComObject comObject)
        {
            MultiQueryInterface localQuery = new MultiQueryInterface()
            {
                InterfaceIID = new IntPtr(&riid),
                IUnknownPointer = IntPtr.Zero,
                ResultCode = 0,
            };

            var result = CoCreateInstanceFromApp(clsid, IntPtr.Zero, clsctx, IntPtr.Zero, 1, ref localQuery);
            result.CheckError();
            localQuery.ResultCode.CheckError();
            comObject.NativePointer = localQuery.IUnknownPointer;
        }
#endif



#else
        [DllImport("ole32.dll", ExactSpelling = true, EntryPoint = "CoCreateInstance", PreserveSig = true)]
        private static extern Result CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr comObject);

        internal static void CreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, ComObject comObject)
        {
            IntPtr pointer;
            var result = CoCreateInstance(clsid, IntPtr.Zero, clsctx, riid, out pointer);
            result.CheckError();
            comObject.NativePointer = pointer;
        }
#endif

        /// <summary>Determines the concurrency model used for incoming calls to objects created by this thread. This concurrency model can be either apartment-threaded or multi-threaded.</summary>
        public enum CoInit
        {
            /// <summary>
            /// Initializes the thread for apartment-threaded object concurrency.
            /// </summary>
            MultiThreaded = 0x0,

            /// <summary>
            /// Initializes the thread for multi-threaded object concurrency.
            /// </summary>
            ApartmentThreaded = 0x2,

            /// <summary>
            /// Disables DDE for OLE1 support.
            /// </summary>
            DisableOle1Dde = 0x4,

            /// <summary>
            /// Trade memory for speed.
            /// </summary>
            SpeedOverMemory = 0x8
        }

#if WP8
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate bool CloseHandleDelegate(IntPtr handle);
        private static CloseHandleDelegate closeHandle;
        internal static CloseHandleDelegate CloseHandle
        {
            get { return closeHandle ?? (closeHandle = (CloseHandleDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(SharpDX.WP8.Interop.CloseHandle()), typeof(CloseHandleDelegate))); }
        }
#else
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr handle);
#endif

        /// <summary>
        /// Loads a native library.
        /// </summary>
        /// <param name="dllName">Name of the DLL.</param>
        /// <exception cref="SharpDXException">If dll was not found</exception>
        /// <returns></returns>
        public static IntPtr LoadLibrary(string dllName)
        {
            IntPtr result = LoadLibrary_(dllName);
            if (result == IntPtr.Zero)
                throw new SharpDXException(dllName);
            return result;
        }

#if WP8
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr LoadLibraryDelegate([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, int reserved = 0);
        private static LoadLibraryDelegate loadLibrary_;
        private static LoadLibraryDelegate LoadLibrary_
        {
            get { return loadLibrary_ ?? (loadLibrary_ = (LoadLibraryDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(SharpDX.WP8.Interop.LoadPackagedLibrary()), typeof(LoadLibraryDelegate))); }
        }
#elif WIN8METRO
        [DllImport("kernel32", EntryPoint = "LoadPackagedLibrary", SetLastError = true)]
        static extern IntPtr LoadLibrary_(string lpFileName, int reserved = 0);
#else
        [DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary_(string lpFileName);
#endif

        /// <summary>
        /// Gets the proc address of a dll.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="dllFunctionToImport">The DLL function to import.</param>
        /// <exception cref="SharpDXException">If the function was not found</exception>
        /// <returns></returns>
        public static IntPtr GetProcAddress(IntPtr handle, string dllFunctionToImport)
        {
            IntPtr result = GetProcAddress_(handle, dllFunctionToImport);
            if (result == IntPtr.Zero)
                throw new SharpDXException(dllFunctionToImport);
            return result;
        }

#if WP8
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr GetProcAddressDelegate(IntPtr hModule, [MarshalAs(UnmanagedType.LPWStr)] string procName);
        private static GetProcAddressDelegate getProcAddress_;
        private static GetProcAddressDelegate GetProcAddress_
        {
            get { return getProcAddress_ ?? (getProcAddress_ = (GetProcAddressDelegate)Marshal.GetDelegateForFunctionPointer(new IntPtr(SharpDX.WP8.Interop.GetProcAddress()), typeof(GetProcAddressDelegate))); }
        }
#else
        [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress_(IntPtr hModule, string procName);
#endif

        /// <summary>
        /// Compute a FNV1-modified Hash from <a href="http://bretm.home.comcast.net/~bretm/hash/6.html">Fowler/Noll/Vo Hash</a> improved version.
        /// </summary>
        /// <param name="data">Data to compute the hash from.</param>
        /// <returns>A hash value</returns>
        public static int ComputeHashFNVModified(byte[] data)
        {
            const uint p = 16777619;
            uint hash = 2166136261;
            foreach (byte b in data)
                hash = (hash ^ b) * p;
            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;
            return unchecked((int)hash);
        }
    }
}