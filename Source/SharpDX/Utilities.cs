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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using SharpDX.Direct3D;
using System.Reflection;

namespace SharpDX
{
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
        /// Clears the memory.
        /// </summary>
        /// <param name="dest">The dest.</param>
        /// <param name="value">The value.</param>
        /// <param name="sizeInBytesToClear">The size in bytes to clear.</param>
        public static void ClearMemory(IntPtr dest, int value, int sizeInBytesToClear)
        {
            unsafe
            {
                // TODO plug in Interop a pluggable CopyMemory using cpblk or memcpy based on architecture
                if (sizeof(void*) == 8)
                    Interop.memsetx64((void*)dest, value, sizeInBytesToClear);
                else
                    Interop.memsetx86((void*)dest, value, sizeInBytesToClear);
            }
        }

        /// <summary>
        /// Return the sizeof a struct from a CLR. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">a struct to evaluate</typeparam>
        /// <returns>sizeof this struct</returns>
        public static int SizeOf<T>() where T : struct
        {
            return SharpDX.Interop.SizeOf<T>();            
        }

        /// <summary>
        /// Return the sizeof an array of struct. Equivalent to sizeof operator but works on generics too.
        /// </summary>
        /// <typeparam name="T">a struct</typeparam>
        /// <param name="array">The array of struct to evaluate.</param>
        /// <returns>sizeof in bytes of this array of struct</returns>
        public static int SizeOf<T>(T[] array) where T : struct
        {
            return array == null ? 0 : array.Length * SharpDX.Interop.SizeOf<T>();
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
                    SharpDX.Interop.Write<T>(pBuffer, source, 0, source.Length);
            }
            return buffer;
        }

        /// <summary>
        /// Reads the specified T data from a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to read</typeparam>
        /// <param name="source">Memory location to read from.</param>
        /// <param name="data">The data write to.</param>
        /// <returns>source pointer + sizeof(T)</returns>
        public static IntPtr Read<T>(IntPtr source, ref T data) where T : struct
        {
            unsafe
            {
                return (IntPtr)SharpDX.Interop.Read<T>((void*) source, ref data);
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
                return (IntPtr)SharpDX.Interop.Read<T>((void*)source, data, offset, count);
            }
        }

        /// <summary>
        /// Writes the specified T data to a memory location.
        /// </summary>
        /// <typeparam name="T">Type of a data to write</typeparam>
        /// <param name="destination">Memory location to write to.</param>
        /// <param name="data">The data to write.</param>
        /// <returns>destination pointer + sizeof(T)</returns>
        public static IntPtr Write<T>(IntPtr destination, ref T data) where T : struct
        {
            unsafe
            {
                return (IntPtr)SharpDX.Interop.Write<T>((void*)destination, ref data);
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
                return (IntPtr)SharpDX.Interop.Write<T>((void*)destination, data, offset, count);
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
        public static int[] ConvertToIntArray(bool[] array)
        {
            var temp = new int[array.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i] ? 1 : 0;
            return temp;
        }

        /// <summary>
        /// Converts to bool array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static bool[] ConvertToBoolArray(int[] array)
        {
            var temp = new bool[array.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i] != 0;
            return temp;
        }

        /// <summary>
        /// Gets the <see cref="System.Guid"/> from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The guid associated with this type</returns>
        public static Guid GetGuidFromType(Type type)
        {
#if WIN8
            return type.GetTypeInfo().GUID;
#else
            return type.GUID;
#endif
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an ansi null string.</param>
        /// <param name="maxLength">Maximum length of the string</param>
        /// <returns></returns>
        public static string PtrToStringAnsi(IntPtr pointer, int maxLength)
        {
#if WIN8
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
#if WIN8
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
            string output;
#if WIN8
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
            System.Diagnostics.Debug.Assert(stream != null);
            System.Diagnostics.Debug.Assert(stream.CanRead);
            int num = readLength;
            System.Diagnostics.Debug.Assert(num <= (stream.Length - stream.Position));
            if (num == 0)
                readLength = (int) (stream.Length - stream.Position);
            num = readLength;

            System.Diagnostics.Debug.Assert(num >= 0);
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

        [DllImport("ole32.dll", ExactSpelling = true, EntryPoint = "CoCreateInstance", PreserveSig = true)]
        private static extern Result CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr comObject);

        internal static void CreateComInstance(Guid clsid, CLSCTX clsctx, Guid riid, ComObject comObject)
        {
            IntPtr pointer;
            var result = CoCreateInstance(clsid, IntPtr.Zero, clsctx, riid, out pointer);
            result.CheckError();
            comObject.NativePointer = pointer;
        }

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

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr handle);
#if !WIN8
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
        [DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary_(string lpFileName);

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
        [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress_(IntPtr hModule, string procName);
#endif
    }
}