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

namespace SharpDX
{
    /// <summary>
    /// A COM Interface Callback
    /// </summary>
    internal class ComObjectCallback : ComObject
    {
        private readonly CppObjectCallback _cppCallback;
        private object _callback;
        private int _count;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="callback">the client callback</param>
        /// <param name="numberOfCallbackMethods">number of methods to allocate in the VTBL</param>
        protected ComObjectCallback(object callback, int numberOfCallbackMethods)
        {
            unsafe
            {
                _callback = callback;
                _cppCallback = new CppObjectCallback(numberOfCallbackMethods + 3);
                NativePointer = _cppCallback.NativePointer;

                AddMethod(new QueryInterfaceDelegate(QueryInterfaceImpl));
                AddMethod(new AddRefDelegate(AddRefImpl));
                AddMethod(new ReleaseDelegate(ReleaseImpl));
            }
        }

        /// <summary>
        /// Gets a managed callback from an unknown.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <returns></returns>
        public static object GetCallbackFromIUnknown(IntPtr ptr)
        {
            return CppObjectCallback.GetCallbackFromIUnknown(ptr);
        }

        /// <summary>
        /// Add a method supported by this interface. This method is typically called from inherited constructor.
        /// </summary>
        /// <param name="method">the managed delegate method</param>
        protected void AddMethod(Delegate method)
        {
            _cppCallback.AddMethod(method);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public unsafe delegate int QueryInterfaceDelegate(IntPtr thisObject, Guid* guid, out IntPtr output);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public  delegate int AddRefDelegate(IntPtr thisObject);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ReleaseDelegate(IntPtr thisObject);

        private static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        protected unsafe virtual int QueryInterfaceImpl(IntPtr thisObject, Guid* guid, out IntPtr output)
        {
            if (*guid == _callback.GetType().GUID)
            {
                AddRefImpl(thisObject);
                output = NativePointer;
                return Result.Ok.Code;
            }

            if (*guid == GetType().GUID)
            {
                AddRefImpl(thisObject);
                output = NativePointer;
                return Result.Ok.Code;
            }

            if (*guid == IID_IUnknown)
            {
                AddRefImpl(thisObject);
                output = NativePointer;
                return Result.Ok.Code;
            }
            output = IntPtr.Zero;
            return Result.NoInterface.Code;
        }

        protected virtual int AddRefImpl(IntPtr thisObject)
        {
            _count++;
            return _count;
        }

        protected virtual int ReleaseImpl(IntPtr thisObject)
        {
            _count--;
            return _count;
        }

        public override void Dispose()
        {
            // Only dispose the CppCallback
            _cppCallback.Dispose();
        }
    }
}