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
using System.Runtime.InteropServices;
using SharpDX.Diagnostics;

namespace SharpDX
{
    /// <summary>
    /// Root IUnknown class to interop with COM object
    /// </summary>
    public partial class ComObject : CppObject, IUnknown
    {
        /// <summary>
        /// Logs a warning of a possible memory leak when <see cref="Configuration.EnableObjectTracking" /> is enabled.
        /// Default uses <see cref="System.Diagnostics.Debug"/>.
        /// </summary>
        public static Action<string> LogMemoryLeakWarning = (warning) => System.Diagnostics.Debug.WriteLine(warning);

        /// <summary>
        /// Initializes a new instance of the <see cref="ComObject"/> class from a IUnknown object.
        /// </summary>
        /// <param name="iunknowObject">Reference to a IUnknown object</param>
        public ComObject(object iunknowObject)
        {
            NativePointer = Marshal.GetIUnknownForObject(iunknowObject);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComObject"/> class.
        /// </summary>
        protected ComObject()
        {
        }

        /// <summary>
        /// Query this instance for a particular COM GUID/interface support.
        /// </summary>
        /// <param name = "guid">GUID query interface</param>
        /// <param name = "outPtr">output object associated with this GUID, IntPtr.Zero in interface is not supported</param>
        /// <exception cref="SharpDXException">If this object doesn't support the interface</exception>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public virtual void QueryInterface(Guid guid, out IntPtr outPtr)
        {
            var result = ((IUnknown) this).QueryInterface(ref guid, out outPtr);
            result.CheckError();
        }

        /// <summary>
        ///   Query instance for a particular COM GUID/interface support.
        /// </summary>
        /// <param name = "guid">GUID query interface</param>
        /// <exception cref="SharpDXException">If this object doesn't support the interface</exception>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public virtual IntPtr QueryInterfaceOrNull(Guid guid)
        {
            var pointer = IntPtr.Zero;
            ((IUnknown)this).QueryInterface(ref guid, out pointer);
            return pointer;
        }

        /// <summary>
        /// Compares 2 COM objects and return true if the native pointer is the same.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns><c>true</c> if the native pointer is the same, <c>false</c> otherwise</returns>
        public static bool EqualsComObject<T>(T left, T right) where T : ComObject
        {
            if (Equals(left, right))
            {
                return true;
            }

            if (left == null || right == null)
            {
                return false;
            }

            return (left.NativePointer == right.NativePointer);
        }

        ///<summary>
        /// Query this instance for a particular COM interface support.
        ///</summary>
        ///<typeparam name="T">The type of the COM interface to query</typeparam>
        ///<returns>An instance of the queried interface</returns>
        /// <exception cref="SharpDXException">If this object doesn't support the interface</exception>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public virtual T QueryInterface<T>() where T : ComObject
        {
            IntPtr parentPtr;
            this.QueryInterface(Utilities.GetGuidFromType(typeof(T)), out parentPtr);
            return FromPointer<T>(parentPtr);
        }

        ///<summary>
        /// Query this instance for a particular COM interface support.
        ///</summary>
        ///<typeparam name="T">The type of the COM interface to query</typeparam>
        ///<returns>An instance of the queried interface</returns>
        /// <exception cref="SharpDXException">If this object doesn't support the interface</exception>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        internal virtual T QueryInterfaceUnsafe<T>()
        {
            IntPtr parentPtr;
            this.QueryInterface(Utilities.GetGuidFromType(typeof(T)), out parentPtr);
            return FromPointerUnsafe<T>(parentPtr);
        }

        /// <summary>
        /// Queries a managed object for a particular COM interface support (This method is a shortcut to <see cref="QueryInterface"/>)
        /// </summary>
        ///<typeparam name="T">The type of the COM interface to query</typeparam>
        /// <param name="comObject">The managed COM object.</param>
        ///<returns>An instance of the queried interface</returns>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public static T As<T>(object comObject) where T : ComObject
        {
            using (var tempObject = new ComObject(Marshal.GetIUnknownForObject(comObject)))
            {
                return tempObject.QueryInterface<T>();
            }
        }

        /// <summary>
        /// Queries a managed object for a particular COM interface support (This method is a shortcut to <see cref="QueryInterface"/>)
        /// </summary>
        ///<typeparam name="T">The type of the COM interface to query</typeparam>
        /// <param name="iunknownPtr">The managed COM object.</param>
        ///<returns>An instance of the queried interface</returns>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public static T As<T>(IntPtr iunknownPtr) where T : ComObject
        {
            using (var tempObject = new ComObject(iunknownPtr))
            {
                return tempObject.QueryInterface<T>();
            }
        }

        internal static T AsUnsafe<T>(IntPtr iunknownPtr)
        {
            using (var tempObject = new ComObject(iunknownPtr))
            {
                return tempObject.QueryInterfaceUnsafe<T>();
            }
        }

        /// <summary>
        /// Queries a managed object for a particular COM interface support.
        /// </summary>
        ///<typeparam name="T">The type of the COM interface to query</typeparam>
        /// <param name="comObject">The managed COM object.</param>
        ///<returns>An instance of the queried interface</returns>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public static T QueryInterface<T>(object comObject) where T : ComObject
        {
            using (var tempObject = new ComObject(Marshal.GetIUnknownForObject(comObject)))
            {
                return tempObject.QueryInterface<T>();
            }
        }

        /// <summary>
        /// Queries a managed object for a particular COM interface support.
        /// </summary>
        ///<typeparam name="T">The type of the COM interface to query</typeparam>
        /// <param name="comPointer">A pointer to a COM object.</param>
        ///<returns>An instance of the queried interface</returns>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public static T QueryInterfaceOrNull<T>(IntPtr comPointer) where T : ComObject
        {
            if (comPointer == IntPtr.Zero)
            {
                return null;
            }

            var guid = Utilities.GetGuidFromType(typeof(T));
            IntPtr pointerT;
            var result = (Result)Marshal.QueryInterface(comPointer, ref guid, out pointerT);
            return (result.Failure) ? null : FromPointerUnsafe<T>(pointerT);
        }

        ///<summary>
        /// Query Interface for a particular interface support.
        ///</summary>
        ///<returns>An instance of the queried interface or null if it is not supported</returns>
        ///<returns></returns>
        /// <msdn-id>ms682521</msdn-id>
        /// <unmanaged>IUnknown::QueryInterface</unmanaged>	
        /// <unmanaged-short>IUnknown::QueryInterface</unmanaged-short>
        public virtual T QueryInterfaceOrNull<T>() where T : ComObject
        {
            return FromPointer<T>(QueryInterfaceOrNull(Utilities.GetGuidFromType(typeof(T))));
        }

        ///<summary>
        /// Query Interface for a particular interface support and attach to the given instance.
        ///</summary>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        protected void QueryInterfaceFrom<T>(T fromObject) where T : ComObject
        {
            IntPtr parentPtr;
            fromObject.QueryInterface(Utilities.GetGuidFromType(this.GetType()), out parentPtr);
            NativePointer = parentPtr;
        }

        Result IUnknown.QueryInterface(ref Guid guid, out IntPtr comObject)
        {
            return Marshal.QueryInterface(NativePointer, ref guid, out comObject);              
        }

        int IUnknown.AddReference()
        {
            if (NativePointer == IntPtr.Zero) throw new InvalidOperationException("COM Object pointer is null");
            return Marshal.AddRef(NativePointer);            
        }

        int IUnknown.Release()
        {
            if (NativePointer == IntPtr.Zero) throw new InvalidOperationException("COM Object pointer is null");
            return Marshal.Release(NativePointer);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <msdn-id>ms682317</msdn-id>
        /// <unmanaged>IUnknown::Release</unmanaged>	
        /// <unmanaged-short>IUnknown::Release</unmanaged-short>
        protected unsafe override void Dispose(bool disposing)
        {
            // Only dispose non-zero object
            if (NativePointer != IntPtr.Zero)
            {
                // If object is disposed by the finalizer, emits a warning
                if(!disposing && Configuration.EnableTrackingReleaseOnFinalizer)
                {
                    if(!Configuration.EnableReleaseOnFinalizer)
                    {
                        var objectReference = ObjectTracker.Find(this);
                        LogMemoryLeakWarning?.Invoke(string.Format("Warning: Live ComObject [0x{0:X}], potential memory leak: {1}", NativePointer.ToInt64(), objectReference));
                    }
                }

                // Release the object
                if (disposing || Configuration.EnableReleaseOnFinalizer)
                    ((IUnknown)this).Release();

                // Untrack the object
                if (Configuration.EnableObjectTracking)
                    ObjectTracker.UnTrack(this);

                // Set pointer to null (using protected members in order to avoid NativePointerUpdat* callbacks.
                _nativePointer = (void*)0;
            }

            base.Dispose(disposing);
        }

        protected override void NativePointerUpdating()
        {
            if (Configuration.EnableObjectTracking)
                ObjectTracker.UnTrack(this);
        }

        protected override void NativePointerUpdated(IntPtr oldNativePointer)
        {
            if (Configuration.EnableObjectTracking)
                ObjectTracker.Track(this);
        }
    }
}