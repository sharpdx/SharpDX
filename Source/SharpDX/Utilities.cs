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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using SharpDX.Direct3D;

namespace SharpDX
{
    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Force an ini
        /// </summary>
        static Utilities()
        {
            CppObject.Init();
        }

        /// <summary>
        /// Native memcpy.
        /// </summary>
        /// <param name="dest">The destination memory location</param>
        /// <param name="src">The source memory location.</param>
        /// <param name="sizeInBytesToCopy">The count.</param>
        /// <returns></returns>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl,
            SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr CopyMemory(IntPtr dest, IntPtr src, ulong sizeInBytesToCopy);

        /// <summary>
        /// Native memcpy.
        /// </summary>
        /// <param name="dest">The destination memory location</param>
        /// <param name="src">The source memory location.</param>
        /// <param name="sizeInBytesToCopy">The count.</param>
        /// <returns></returns>
        public static IntPtr CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy)
        {
            // TODO plug in Interop a pluggable CopyMemory using cpblk or memcpy based on architecture
            return CopyMemory(dest, src, (ulong) sizeInBytesToCopy);
        }

        /// <summary>
        /// Pin a local structure. The structure declare local to a method in order for this method to be safe.
        /// </summary>
        /// <typeparam name="T">a struct to pin</typeparam>
        /// <returns>a pointer to this struct</returns>
        public static IntPtr Pin<T>(ref T data) where T : struct
        {
            unsafe
            {
                return new IntPtr(SharpDX.Interop.Pin<T>(ref data));
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

        internal unsafe static void ConvertToIntArray(bool[] array, int* dest)
        {
            for (int i = 0; i < array.Length; i++)
                dest[i] = array[i] ? 1 : 0;
        }

        internal unsafe static bool[] ConvertToBoolArray(int* array, int length)
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
        internal static int[] ConvertToIntArray(bool[] array)
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
        internal static bool[] ConvertToBoolArray(int[] array)
        {
            var temp = new bool[array.Length];
            for (int i = 0; i < temp.Length; i++)
                temp[i] = array[i] != 0;
            return temp;
        }

        internal static void ConvertRECTToRectangle(ref System.Drawing.Rectangle rect)
        {
            rect = new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width - rect.X, rect.Height - rect.Y);
        }

        internal static void ConvertRectangleToRect(ref System.Drawing.Rectangle rect)
        {
            rect = new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width - rect.X, rect.Height - rect.Y);
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an ansi null string.</param>
        /// <param name="maxLength">Maximum length of the string</param>
        /// <returns></returns>
        public static string PtrToStringAnsi(IntPtr pointer, int maxLength)
        {
            unsafe
            {
                var pStr = (byte*)pointer;
                for (int i = 0; i < maxLength; i++) 
                    if (*pStr++ == 0 )
                        return new string((sbyte*)pointer);
                return new string((sbyte*)pointer, 0, maxLength);
            }
        }

        /// <summary>
        /// Converts a pointer to a null-terminating string up to maxLength characters to a .Net string.
        /// </summary>
        /// <param name="pointer">The pointer to an unicode null string.</param>
        /// <param name="maxLength">Maximum length of the string</param>
        /// <returns></returns>
        public static string PtrToStringUni(IntPtr pointer, int maxLength)
        {
            unsafe
            {
                var pStr = (char*)pointer;
                for (int i = 0; i < maxLength; i++)
                    if (*pStr++ == 0)
                        return new string((char*)pointer);
                return new string((char*)pointer, 0, maxLength);
            }
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
            return string.Join(separator, Array.ConvertAll(array, from => from.ToString()));
        }


        /// <summary>
        /// String helper join method to display an enumrable of object as a single string.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="elements">The enumerable.</param>
        /// <returns>a string with array elements serparated by the seperator</returns>
        public static string Join<T>(string separator, IEnumerable<T> elements)
        {
            var elementList = new List<string>();
            foreach (var element in elements)
                elementList.Add(element.ToString());

            return string.Join(separator, Array.ConvertAll(elementList.ToArray(), from => from.ToString()));
        }

        [Flags]
        internal enum CLSCTX : uint
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

        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = true)]
        static internal extern Result CoCreateInstance([In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pUnkOuter, CLSCTX dwClsContext, [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr comObject);

        internal static string BlobToString(Blob blob)
        {
            string output;
            unsafe
            {
                output = new string((sbyte*) blob.GetBufferPointer());
            }
            blob.Release();
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

        /// <summary>
        /// Loads a native library.
        /// </summary>
        /// <param name="dllName">Name of the DLL.</param>
        /// <exception cref="DllNotFoundException">If dll was not found</exception>
        /// <returns></returns>
        internal static IntPtr LoadLibrary(string dllName)
        {
            IntPtr result = LoadLibrary_(dllName);
            if (result == IntPtr.Zero)
                throw new DllNotFoundException(dllName);
            return result;
        }
        [DllImport("kernel32", EntryPoint = "LoadLibrary", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary_(string lpFileName);


        /// <summary>
        /// Gets the proc address of a dll.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="dllFunctionToImport">The DLL function to import.</param>
        /// <exception cref="EntryPointNotFoundException">If the function was not found</exception>
        /// <returns></returns>
        internal static IntPtr GetProcAddress(IntPtr handle, string dllFunctionToImport)
        {
            IntPtr result = GetProcAddress_(handle, dllFunctionToImport);
            if (result == IntPtr.Zero)
                throw new EntryPointNotFoundException(dllFunctionToImport);
            return result;
        }
        [DllImport("kernel32", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress_(IntPtr hModule, string procName);
    }
}