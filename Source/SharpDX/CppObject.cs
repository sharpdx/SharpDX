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

using SharpDX.Diagnostics;

namespace SharpDX
{
    /// <summary>
    /// Root class for all Cpp interop object.
    /// </summary>
    public class CppObject : DisposeBase
    {
        /// <summary>
        /// The native pointer
        /// </summary>
        protected internal unsafe void* _nativePointer;

        /// <summary>
        /// Gets or sets a custom user tag object to associate with this instance..
        /// </summary>
        /// <value>The tag object.</value>
        public object Tag { get; set; }

        /// <summary>
        ///   Default constructor.
        /// </summary>
        /// <param name = "pointer">Pointer to Cpp Object</param>
        public CppObject(IntPtr pointer)
        {
            NativePointer = pointer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CppObject"/> class.
        /// </summary>
        protected CppObject()
        {
        }

        /// <summary>
        ///   Get a pointer to the underlying Cpp Object
        /// </summary>
        public IntPtr NativePointer
        {
            get
            {
                unsafe
                {
                    return (IntPtr) _nativePointer;
                }
            }
            set
            {
                unsafe
                {
                    var newNativePointer = (void*) value;
                    if (_nativePointer != newNativePointer)
                    {
                        NativePointerUpdating();
                        var oldNativePointer = _nativePointer;
                        _nativePointer = newNativePointer;
                        NativePointerUpdated((IntPtr)oldNativePointer);
                    }
                }
            }
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.CppObject"/> to <see cref="System.IntPtr"/>.
        /// </summary>
        /// <param name="cppObject">The CPP object.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator IntPtr(CppObject cppObject)
        {
            return cppObject == null ? IntPtr.Zero : cppObject.NativePointer;
        }

        /// <summary>
        /// Initializes this instance with a pointer from a temporary object and set the pointer of the temporary  
        /// object to IntPtr.Zero.
        /// </summary>
        /// <param name="temp">The instance to get the NativePointer.</param>
        protected void FromTemp(CppObject temp)
        {
            NativePointer = temp.NativePointer;
            temp.NativePointer = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes this instance with a pointer from a temporary object and set the pointer of the temporary  
        /// object to IntPtr.Zero.
        /// </summary>
        /// <param name="temp">The instance to get the NativePointer.</param>
        protected void FromTemp(IntPtr temp)
        {
            NativePointer = temp;
        }

        /// <summary>
        /// Method called when <see cref="NativePointer"/> is going to be update.
        /// </summary>
        protected virtual void NativePointerUpdating()
        {
        }

        /// <summary>
        /// Method called when the <see cref="NativePointer"/> is updated.
        /// </summary>
        protected virtual void NativePointerUpdated(IntPtr oldNativePointer)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Instantiate a ComObject from a native pointer.
        /// </summary>
        /// <typeparam name="T">The ComObject class that will be returned</typeparam>
        /// <param name="comObjectPtr">The native pointer to a com object.</param>
        /// <returns>An instance of T binded to the native pointer</returns>
        public static T FromPointer<T>(IntPtr comObjectPtr) where T : CppObject
        {
            return ObjectTracker.FindOrCreateDefaultInstance<T>(comObjectPtr);
        }

        internal static void SetDefaultInstance<T>(IntPtr comObjectPtr, T valueInstance) where T : CppObject
        {
            ObjectTracker.MakeDefaultInstance(comObjectPtr, valueInstance);
        }

        internal static T FromPointerUnsafe<T>(IntPtr comObjectPtr)
        {
            return ObjectTracker.FindOrCreateDefaultInstance<T>(comObjectPtr);
        }

        protected virtual void DisposeCachedMembers()
        {
        }
    }
}