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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// An Interface Callback
    /// </summary>
    internal class CppObjectCallback : CppObject, IDisposable
    {
        private readonly List<Delegate> _methods;
        private readonly IntPtr _thisIUnknown;
        private readonly IntPtr _vtblPtr;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="numberOfCallbackMethods">number of methods to allocate in the VTBL</param>
        internal CppObjectCallback(int numberOfCallbackMethods)
        {
            unsafe
            {
                // Allocate ptr to vtbl + vtbl together
                NativePointer = Marshal.AllocHGlobal(IntPtr.Size*(numberOfCallbackMethods + 2)*2);

                // Allocate additionnal infos
                _thisIUnknown = new IntPtr(((byte*)NativePointer) + IntPtr.Size);
                _vtblPtr = new IntPtr(((byte*)_thisIUnknown) + IntPtr.Size);

                // Store This to IUnknown pointer
                Marshal.WriteIntPtr(_thisIUnknown, Marshal.GetIUnknownForObject(this));

                // Store VTBL
                Marshal.WriteIntPtr(NativePointer, _vtblPtr);
                _methods = new List<Delegate>(numberOfCallbackMethods);
            }
        }

        /// <summary>
        /// Gets a managed callback from an unknown.
        /// </summary>
        /// <param name="ptr">The PTR.</param>
        /// <returns></returns>
        public static object GetCallbackFromIUnknown(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return null;
            return Utilities.GetObjectForIUnknown(Marshal.ReadIntPtr(ptr, IntPtr.Size));
        }

        /// <summary>
        /// Add a method supported by this interface. This method is typically called from inherited constructor.
        /// </summary>
        /// <param name="method">the managed delegate method</param>
        internal void AddMethod(Delegate method)
        {
            unsafe
            {
                int index = _methods.Count;
                _methods.Add(method);
                Marshal.WriteIntPtr(new IntPtr(((byte*) _vtblPtr) + index*IntPtr.Size), Marshal.GetFunctionPointerForDelegate(method));
            }
        }

        ~CppObjectCallback()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            if (NativePointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(NativePointer);
                NativePointer = IntPtr.Zero;
            }
        }
    }
}
