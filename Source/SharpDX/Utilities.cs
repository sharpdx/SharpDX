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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

using SharpDX.Direct3D;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using SharpDX.Text;

using SharpDX.Mathematics.Interop;

namespace SharpDX
{

    /// <summary>
    /// A Delegate to get a property value from an object.
    /// </summary>
    /// <typeparam name="T">Type of the getter.</typeparam>
    /// <param name="obj">The obj to get the property from.</param>
    /// <param name="value">The value to get.</param>
    public delegate void GetValueFastDelegate<T>(object obj, out T value);

    /// <summary>
    /// A Delegate to set a property value to an object.
    /// </summary>
    /// <typeparam name="T">Type of the setter.</typeparam>
    /// <param name="obj">The obj to set the property from.</param>
    /// <param name="value">The value to set.</param>
    public delegate void SetValueFastDelegate<T>(object obj, ref T value);

    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Utilities
    {
        ///// <summary>
        ///// Native memcpy.
        ///// </summary>
        ///// <param name="dest">The destination memory location.</param>
        ///// <param name="src">The source memory location.</param>
        ///// <param name="sizeInBytesToCopy">The count.</param>
        ///// <returns></returns>
        //[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
        //    SetLastError = false), SuppressUnmanagedCodeSecurity]
        //public static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, ulong sizeInBytesToCopy);

        /// <summary>
        /// Native memcpy.
        /// </summary>
        /// <param name="dest">The destination memory location.</param>
        /// <param name="src">The source memory location.</param>
        /// <param name="sizeInBytesToCopy">The byte count.</param>
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
        /// <returns><c>true</c> if the buffers are equivalent; otherwise, <c>false</c>.</returns>
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
        /// <typeparam name="T">A struct to evaluate.</typeparam>
        /// <returns>Size of this struct.</returns>
        public static int SizeOf<T>() where T : struct
        {
            return Interop.SizeOf<T>();            
        }

        /// <summary>
        /// Return the sizeof an array of struct. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">A struct.</typeparam>
        /// <param name="array">The array of struct to evaluate.</param>
        /// <returns>Size in bytes of this array of struct.</returns>
        public static int SizeOf<T>(T[] array) where T : struct
        {
            return array == null ? 0 : array.Length * Interop.SizeOf<T>();
        }

        /// <summary>
        /// Pins the specified source and call an action with the pinned pointer.
        /// </summary>
        /// <typeparam name="T">The type of the structure to pin.</typeparam>
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
        /// <typeparam name="T">The type of the structure to pin.</typeparam>
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
        /// Converts a structured array to an equivalent byte array.
        /// </summary>
        /// <typeparam name="T">The type of source array.</typeparam>
        /// <param name="source">The source array.</param>
        /// <returns>Converted byte array.</returns>
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
        /// Swaps the value between two references.
        /// </summary>
        /// <typeparam name="T">Type of a data to swap.</typeparam>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        public static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }

        /// <summary>
        /// Reads the specified T data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read.</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <returns>The data read from the memory location.</returns>
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
        /// <typeparam name="T">Type of a data to read.</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T).</returns>
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
        /// <typeparam name="T">Type of a data to read.</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T).</returns>
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
        /// <typeparam name="T">Type of a data to read.</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T).</returns>
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
        /// <typeparam name="T">Type of a data to read.</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <param name="offset">The offset in the array to write to.</param>
        /// <param name="count">The number of T element to read from the memory location.</param>
        /// <returns>source pointer + sizeof(T) * count.</returns>
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
        /// <typeparam name="T">Type of a data to write.</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The data to write.</param>
        /// <returns>destination pointer + sizeof(T).</returns>
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
        /// <typeparam name="T">Type of a data to write.</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The data to write.</param>
        /// <returns>destination pointer + sizeof(T).</returns>
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
        /// <typeparam name="T">Type of a data to write.</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The array of T data to write.</param>
        /// <param name="offset">The offset in the array to read from.</param>
        /// <param name="count">The number of T element to write to the memory location.</param>
        /// <returns>destination pointer + sizeof(T) * count.</returns>
        public static IntPtr Write<T>(IntPtr destination, T[] data, int offset, int count) where T : struct
        {
            unsafe
            {
                return (IntPtr)Interop.Write((void*)destination, data, offset, count);
            }
        }

    /// <summary>
        /// Converts bool array to integer pointers array.
        /// </summary>
        /// <param name="array">The bool array.</param>
        /// <param name="dest">The destination array of int pointers.</param>
        public unsafe static void ConvertToIntArray(bool[] array, int* dest)
        {
            for (int i = 0; i < array.Length; i++)
                dest[i] = array[i] ? 1 : 0;
        }
 
        /// <summary>
        /// Converts bool array to <see cref="RawBool"/> array.
        /// </summary>
        /// <param name="array">The bool array.</param>
        /// <returns>Converted array of <see cref="RawBool"/>.</returns>
        public static RawBool[] ConvertToIntArray(bool[] array)
        {
            var temp = new RawBool[array.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i];
            return temp;
        }

        /// <summary>
        /// Converts integer pointer array to bool array.
        /// </summary>
        /// <param name="array">The array of integer pointers.</param>
        /// <param name="length">Array size.</param>
        /// <returns>Converted array of bool.</returns>
        public static unsafe bool[] ConvertToBoolArray(int* array, int length)
        {
            var temp = new bool[length];
            for(int i = 0; i < temp.Length; i++)
                temp[i] = array[i] != 0;
            return temp;
        }

        /// <summary>
        /// Converts <see cref="RawBool"/> array to bool array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>Converted array of bool.</returns>
        public static bool[] ConvertToBoolArray(RawBool[] array)
        {
            var temp = new bool[array.Length];
            for(int i = 0; i < temp.Length; i++)
                temp[i] = array[i];
            return temp;
        }

        /// <summary>
        /// Gets the <see cref="System.Guid"/> from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The guid associated with this type.</returns>
        public static Guid GetGuidFromType(Type type)
        {
            return type.GetTypeInfo().GUID;
        }

        /// <summary>
        /// Determines whether a given type inherits from a generic type.
        /// </summary>
        /// <param name="givenType">Type of the class to check if it inherits from generic type.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns><c>true</c> if [is assignable to generic type] [the specified given type]; otherwise, <c>false</c>.</returns>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            // from http://stackoverflow.com/a/1075059/1356325
#if BEFORE_NET45
            var interfaceTypes = givenType.GetTypeInfo().GetInterfaces();
#else
            var interfaceTypes = givenType.GetTypeInfo().ImplementedInterfaces;
#endif

            foreach (var it in interfaceTypes)
            {
                if (it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.GetTypeInfo().IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.GetTypeInfo().BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        /// <summary>
        /// Allocate an aligned memory buffer.
        /// </summary>
        /// <param name="sizeInBytes">Size of the buffer to allocate.</param>
        /// <param name="align">Alignment, 16 bytes by default.</param>
        /// <returns>A pointer to a buffer aligned.</returns>
        /// <remarks>
        /// To free this buffer, call <see cref="FreeMemory"/>.
        /// </remarks>
        public unsafe static IntPtr AllocateMemory(int sizeInBytes, int align = 16)
        {
            int mask = align - 1;
            var memPtr = Marshal.AllocHGlobal(sizeInBytes + mask + IntPtr.Size);
            var ptr = (long)((byte*)memPtr + sizeof(void*) + mask) & ~mask;
            ((IntPtr*)ptr)[-1] = memPtr;
            return new IntPtr((void*)ptr);
        }

        /// <summary>
        /// Allocate an aligned memory buffer and clear it with a specified value (0 by default).
        /// </summary>
        /// <param name="sizeInBytes">Size of the buffer to allocate.</param>
        /// <param name="clearValue">Default value used to clear the buffer.</param>
        /// <param name="align">Alignment, 16 bytes by default.</param>
        /// <returns>A pointer to a buffer aligned.</returns>
        /// <remarks>
        /// To free this buffer, call <see cref="FreeMemory"/>.
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
        /// The buffer must have been allocated with <see cref="AllocateMemory"/>.
        /// </remarks>
        public unsafe static void FreeMemory(IntPtr alignedBuffer)
        {
            if (alignedBuffer == IntPtr.Zero) return;
            Marshal.FreeHGlobal(((IntPtr*) alignedBuffer)[-1]);
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an ANSI null string.</param>
        /// <param name="maxLength">Maximum length of the string.</param>
        /// <returns>The converted string.</returns>
        public static string PtrToStringAnsi(IntPtr pointer, int maxLength)
        {
            string managedString = Marshal.PtrToStringAnsi(pointer); // copy null-terminating unmanaged text from pointer to a managed string
            if (managedString != null && managedString.Length > maxLength)
                managedString = managedString.Substring(0, maxLength);

            return managedString;
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an Unicode null string.</param>
        /// <param name="maxLength">Maximum length of the string.</param>
        /// <returns>The converted string.</returns>
        public static string PtrToStringUni(IntPtr pointer, int maxLength)
        {
            string managedString = Marshal.PtrToStringUni(pointer); // copy null-terminating unmanaged text from pointer to a managed string
            if (managedString != null && managedString.Length > maxLength)
                managedString = managedString.Substring(0, maxLength);

            return managedString;
        }

        /// <summary>
        /// Copies the contents of a managed String into unmanaged memory, converting into ANSI format as it copies.
        /// </summary>
        /// <param name="s">A managed string to be copied.</param> 
        /// <returns>The address, in unmanaged memory, to where s was copied, or IntPtr.Zero if s is null.</returns>
        public static unsafe IntPtr StringToHGlobalAnsi(string s)
        {
            return Marshal.StringToHGlobalAnsi(s);
        }

        /// <summary>
        /// Copies the contents of a managed String into unmanaged memory.
        /// </summary>
        /// <param name="s">A managed string to be copied.</param> 
        /// <returns>The address, in unmanaged memory, to where s was copied, or IntPtr.Zero if s is null.</returns>
        public static unsafe IntPtr StringToHGlobalUni(string s)
        {
            return Marshal.StringToHGlobalUni(s);
        }

        /// <summary>
        /// Copies the contents of a managed String into unmanaged memory using <see cref="Marshal.AllocCoTaskMem"/>
        /// </summary>
        /// <param name="s">A managed string to be copied.</param> 
        /// <returns>The address, in unmanaged memory, to where s was copied, or IntPtr.Zero if s is null.</returns>
        public static unsafe IntPtr StringToCoTaskMemUni(string s)
        {
            if (s == null)
            {
                return IntPtr.Zero;
            }
            int num = (s.Length + 1) * 2;
            if (num < s.Length)
            {
                throw new ArgumentOutOfRangeException("s");
            }
            IntPtr ptr2 = Marshal.AllocCoTaskMem(num);
            if (ptr2 == IntPtr.Zero)
            {
                throw new OutOfMemoryException();
            }
            CopyStringToUnmanaged(ptr2, s);
            return ptr2;
        }

        private unsafe static void CopyStringToUnmanaged(IntPtr ptr, string str)
        {
            fixed (char* pStr = str)
            {
                CopyMemory(ptr, new IntPtr(pStr), (str.Length + 1 ) * 2);
            }
        }

        /// <summary>
        /// Gets the IUnknown from object. Similar to <see cref="Marshal.GetIUnknownForObject"/> but accept null object
        /// by returning an IntPtr.Zero IUnknown pointer.
        /// </summary>
        /// <param name="obj">The managed object.</param>
        /// <returns>An IUnknown pointer to a  managed object.</returns>
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
        /// <param name="iunknownPtr">an IUnknown pointer to a  managed object.</param>
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
        /// <returns>A string with array elements separated by the separator.</returns>
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
        /// String helper join method to display an enumerable of object as a single string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="elements">The enumerable.</param>
        /// <returns>A string with array elements separated by the separator.</returns>
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
        /// String helper join method to display an enumerable of object as a single string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="elements">The enumerable.</param>
        /// <returns>A string with array elements separated by the separator.</returns>
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
            output = Marshal.PtrToStringAnsi(blob.BufferPointer);
            blob.Dispose();
            return output;
        }

        /// <summary>
        /// Equivalent to IntPtr.Add method from 3.5+ .NET Framework.
        /// Adds an offset to the value of a pointer.
        /// </summary>
        /// <param name="ptr">A native pointer.</param>
        /// <param name="offset">The offset to add (number of bytes).</param>
        /// <returns>A new pointer that reflects the addition of offset to pointer.</returns>
        public unsafe static IntPtr IntPtrAdd(IntPtr ptr, int offset)
        {
            return new IntPtr(((byte*) ptr) + offset);
        }

        /// <summary>
        /// Read stream to a byte[] buffer.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <returns>A byte[] buffer.</returns>
        public static byte[] ReadStream(Stream stream)
        {
            int readLength = 0;
            return ReadStream(stream, ref readLength);
        }

        /// <summary>
        /// Read stream to a byte[] buffer.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="readLength">Length to read.</param>
        /// <returns>A byte[] buffer.</returns>
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
        /// <returns><c>true</c> if lists are identical, <c>false</c> otherwise.</returns>
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
        /// <returns><c>true</c> if lists are identical; otherwise, <c>false</c>.</returns>
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
        /// <param name="right">The collection to compare to.</param>
        /// <returns><c>true</c> if lists are identical (but not necessarily of the same time); otherwise , <c>false</c>.</returns>
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
        /// <typeparam name="T">Type of the custom attribute.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherited">if set to <c>true</c> [inherited].</param>
        /// <returns>The custom attribute or null if not found.</returns>
        public static T GetCustomAttribute<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
        {
            return memberInfo.GetCustomAttribute<T>(inherited);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T">Type of the custom attribute.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherited">if set to <c>true</c> [inherited].</param>
        /// <returns>The custom attribute or null if not found.</returns>
        public static IEnumerable<T> GetCustomAttributes<T>(MemberInfo memberInfo, bool inherited = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes<T>(inherited);
        }

        /// <summary>
        /// Determines whether fromType can be assigned to toType.
        /// </summary>
        /// <param name="toType">To type.</param>
        /// <param name="fromType">From type.</param>
        /// <returns>
        /// <c>true</c> if [is assignable from] [the specified to type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableFrom(Type toType, Type fromType)
        {
            return toType.GetTypeInfo().IsAssignableFrom(fromType.GetTypeInfo());
        }

        /// <summary>
        /// Determines whether the specified type to test is an enum.
        /// </summary>
        /// <param name="typeToTest">The type to test.</param>
        /// <returns>
        /// <c>true</c> if the specified type to test is an enum; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEnum(Type typeToTest)
        {
            return typeToTest.GetTypeInfo().IsEnum;
        }

        /// <summary>
        /// Determines whether the specified type to test is a value type.
        /// </summary>
        /// <param name="typeToTest">The type to test.</param>
        /// <returns>
        /// <c>true</c> if the specified type to test is a value type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValueType(Type typeToTest)
        {
            return typeToTest.GetTypeInfo().IsValueType;
        }

        private static MethodInfo GetMethod(Type type, string name, Type[] typeArgs) {
#if BEFORE_NET45
            foreach( var method in type.GetTypeInfo().GetMethods(BindingFlags.Public|BindingFlags.Instance))
            {
                if(method.Name != name)
                {
                    continue;
                }
#else
            foreach( var method in type.GetTypeInfo().GetDeclaredMethods(name))
            {
#endif
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
        }

        /// <summary>
        /// Builds a fast property getter from a type and a property info.
        /// </summary>
        /// <typeparam name="T">Type of the getter.</typeparam>
        /// <param name="customEffectType">Type of the custom effect.</param>
        /// <param name="propertyInfo">The property info to get the value from.</param>
        /// <returns>A compiled delegate.</returns>
        public static GetValueFastDelegate<T> BuildPropertyGetter<T>(Type customEffectType, PropertyInfo propertyInfo)
        {
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
        }

        /// <summary>
        /// Builds a fast property setter from a type and a property info.
        /// </summary>
        /// <typeparam name="T">Type of the setter.</typeparam>
        /// <param name="customEffectType">Type of the custom effect.</param>
        /// <param name="propertyInfo">The property info to set the value to.</param>
        /// <returns>A compiled delegate.</returns>
        public static SetValueFastDelegate<T> BuildPropertySetter<T>(Type customEffectType, PropertyInfo propertyInfo)
        {
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
        }

        /// <summary>
        /// Finds an explicit conversion between a source type and a target type.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns>The method to perform the conversion. null if not found.</returns>
        private static MethodInfo FindExplicitConverstion(Type sourceType, Type targetType)
        {
            // No need for cast for similar source and target type
            if (sourceType == targetType)
                return null;

            var methods = new List<MethodInfo>();

            var tempType = sourceType;
            while (tempType != null)
            {
#if BEFORE_NET45
                methods.AddRange(tempType.GetTypeInfo().GetMethods(BindingFlags.Public)); //target methods will be favored in the search
#else
                methods.AddRange(tempType.GetTypeInfo().DeclaredMethods); //target methods will be favored in the search
#endif
                tempType = tempType.GetTypeInfo().BaseType;
            }

            tempType = targetType;
            while (tempType != null)
            {
#if BEFORE_NET45
                methods.AddRange(tempType.GetTypeInfo().GetMethods(BindingFlags.Public)); //target methods will be favored in the search
#else
                methods.AddRange(tempType.GetTypeInfo().DeclaredMethods); //target methods will be favored in the search
#endif
                tempType = tempType.GetTypeInfo().BaseType;
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

#if WINDOWS_UWP
        [StructLayout(LayoutKind.Sequential)]
        public struct MultiQueryInterface
        {
            public IntPtr InterfaceIID;
            public IntPtr IUnknownPointer;
            public Result ResultCode;
        };


        [DllImport("api-ms-win-core-com-l1-1-0.dll", ExactSpelling = true, EntryPoint = "CoCreateInstanceFromApp", PreserveSig = true)]
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

        internal unsafe static bool TryCreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, ComObject comObject)
        {
            MultiQueryInterface localQuery = new MultiQueryInterface()
            {
                InterfaceIID = new IntPtr(&riid),
                IUnknownPointer = IntPtr.Zero,
                ResultCode = 0,
            };

            var result = CoCreateInstanceFromApp(clsid, IntPtr.Zero, clsctx, IntPtr.Zero, 1, ref localQuery);
            comObject.NativePointer = localQuery.IUnknownPointer;
            return result.Success && localQuery.ResultCode.Success;
        }

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

        internal static bool TryCreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, ComObject comObject)
        {
            IntPtr pointer;
            var result = CoCreateInstance(clsid, IntPtr.Zero, clsctx, riid, out pointer);
            comObject.NativePointer = pointer;
            return result.Success;
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

#if WINDOWS_UWP
        [DllImport("api-ms-win-core-handle-l1-1-0.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr handle);
#else
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr handle);
#endif

        /// <summary>
        /// Gets the proc address of a DLL.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="dllFunctionToImport">The DLL function to import.</param>
        /// <exception cref="SharpDXException">If the function was not found.</exception>
        /// <returns>Pointer to address of the exported function or variable.</returns>
        public static IntPtr GetProcAddress(IntPtr handle, string dllFunctionToImport)
        {
            IntPtr result = GetProcAddress_(handle, dllFunctionToImport);
            if (result == IntPtr.Zero)
                throw new SharpDXException(dllFunctionToImport);
            return result;
        }

#if WINDOWS_UWP
        [DllImport("api-ms-win-core-libraryloader-l1-1-1.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress_(IntPtr hModule, string procName);
#else
        // http://www.pinvoke.net/default.aspx/kernel32.getprocaddress
        // http://stackoverflow.com/questions/3754264/c-sharp-getprocaddress-returns-zero
        [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress_(IntPtr hModule, string procName);
#endif

        /// <summary>
        /// Compute a FNV1-modified Hash from <a href="http://bretm.home.comcast.net/~bretm/hash/6.html">Fowler/Noll/Vo Hash</a> improved version.
        /// </summary>
        /// <param name="data">Data to compute the hash from.</param>
        /// <returns>A hash value.</returns>
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

        /// <summary>
        /// Safely dispose a reference if not null, and set it to null after dispose.
        /// </summary>
        /// <typeparam name="T">The type of COM interface to dispose.</typeparam>
        /// <param name="comObject">Object to dispose.</param>
        /// <remarks>
        /// The reference will be set to null after dispose.
        /// </remarks>
        public static void Dispose<T>(ref T comObject) where T : class, IDisposable
        {
            if (comObject != null)
            {
                comObject.Dispose();
                comObject = null;
            }
        }

        /// <summary>
        /// Transforms an <see cref="IEnumerable{T}"/> to an array of T.
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="source">The enumerable source.</param>
        /// <returns>an array of T</returns>
        public static T[] ToArray<T>(IEnumerable<T> source)
        {
            return new Buffer<T>(source).ToArray();
        }

        /// <summary>
        /// Test if there is an element in this enumeration.
        /// </summary>
        /// <typeparam name="T">Type of the element</typeparam>
        /// <param name="source">The enumerable source.</param>
        /// <returns><c>true</c> if there is an element in this enumeration, <c>false</c> otherwise</returns>
        public static bool Any<T>(IEnumerable<T> source)
        {
            return source.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// Select elements from an enumeration.
        /// </summary>
        /// <typeparam name="TSource">The type of the T source.</typeparam>
        /// <typeparam name="TResult">The type of the T result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>A enumeration of selected values</returns>
        public static IEnumerable<TResult> SelectMany<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            foreach (TSource sourceItem in source)
            {
                foreach (TResult result in selector(sourceItem))
                    yield return result;
            }
        }

        /// <summary>
        /// Selects distinct elements from an enumeration.
        /// </summary>
        /// <typeparam name="TSource">The type of the T source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>A enumeration of selected values</returns>
        public static IEnumerable<TSource> Distinct<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<TSource>.Default;
            
            // using Dictionary is not really efficient but easy to implement
            var values = new Dictionary<TSource, object>(comparer);
            foreach (TSource sourceItem in source)
            {
                if (!values.ContainsKey(sourceItem))
                {
                    values.Add(sourceItem, null);
                    yield return sourceItem;
                }
            }
        }

        internal struct Buffer<TElement>
        {
            internal TElement[] items;
            internal int count;

            internal Buffer(IEnumerable<TElement> source)
            {
                var array = (TElement[])null;
                int length = 0;
                var collection = source as ICollection<TElement>;
                if (collection != null)
                {
                    length = collection.Count;
                    if (length > 0)
                    {
                        array = new TElement[length];
                        collection.CopyTo(array, 0);
                    }
                }
                else
                {
                    foreach (TElement element in source)
                    {
                        if (array == null)
                            array = new TElement[4];
                        else if (array.Length == length)
                        {
                            var elementArray = new TElement[checked(length * 2)];
                            Array.Copy(array, 0, elementArray, 0, length);
                            array = elementArray;
                        }
                        array[length] = element;
                        ++length;
                    }
                }
                items = array;
                count = length;
            }

            internal TElement[] ToArray()
            {
                if (count == 0)
                    return new TElement[0];
                if (items.Length == count)
                    return items;
                var elementArray = new TElement[count];
                Array.Copy(items, 0, elementArray, 0, count);
                return elementArray;
            }
        }

        /// <summary>
        /// Determines whether the type inherits from the specified type (used to determine a type without using an explicit type instance).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parentType">Name of the parent type to find in inheritance hierarchy of type.</param>
        /// <returns><c>true</c> if the type inherits from the specified type; otherwise, <c>false</c>.</returns>
        public static bool IsTypeInheritFrom(Type type, string parentType)
        {
            while (type != null)
            {
                if (type.FullName == parentType)
                {
                    return true;
                }
                type = type.GetTypeInfo().BaseType;
            }

            return false;
        }
    }
}
