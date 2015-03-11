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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal GeometrySink Callback
    /// </summary>
    internal class GeometrySinkShadow : SimplifiedGeometrySinkShadow
    {
        private static readonly GeometrySinkVtbl Vtbl = new GeometrySinkVtbl();
        protected override CppObjectVtbl GetVtbl { get { return Vtbl; } }

        /// <summary>
        /// Get a native callback pointer from a managed callback.
        /// </summary>
        /// <param name="geometrySink">The geometry sink.</param>
        /// <returns>A pointer to the unmanaged geometry sink counterpart</returns>
        public static IntPtr ToIntPtr(GeometrySink geometrySink)
        {
            return ToCallbackPtr<GeometrySink>(geometrySink);
        }

        private class GeometrySinkVtbl : SimplifiedGeometrySinkVtbl
        {
            public GeometrySinkVtbl() : base(5)
            {
                AddMethod(new AddLineDelegate(AddLineImpl));
                AddMethod(new AddBezierDelegate(AddBezierImpl));
                AddMethod(new AddQuadraticBezierDelegate(AddQuadraticBezierImpl));
                AddMethod(new AddQuadraticBeziersDelegate(AddQuadraticBeziersImpl));
                AddMethod(new AddArcDelegate(AddArcImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddLineDelegate(IntPtr thisPtr, RawVector2 point);
            private static unsafe void AddLineImpl(IntPtr thisPtr, RawVector2 point)
            {
                var shadow = ToShadow<GeometrySinkShadow>(thisPtr);
                var callback = (GeometrySink)shadow.Callback; 
                callback.AddLine(point);
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddBezierDelegate(IntPtr thisPtr, IntPtr bezier);
            private static unsafe void AddBezierImpl(IntPtr thisPtr, IntPtr bezier)
            {
                var shadow = ToShadow<GeometrySinkShadow>(thisPtr);
                var callback = (GeometrySink)shadow.Callback;
                callback.AddBezier(*((SharpDX.Direct2D1.BezierSegment*)bezier));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddQuadraticBezierDelegate(IntPtr thisPtr, IntPtr bezier);
            private static unsafe void AddQuadraticBezierImpl(IntPtr thisPtr, IntPtr bezier)
            {
                var shadow = ToShadow<GeometrySinkShadow>(thisPtr);
                var callback = (GeometrySink)shadow.Callback;
                callback.AddQuadraticBezier(*((SharpDX.Direct2D1.QuadraticBezierSegment*)bezier));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddQuadraticBeziersDelegate(IntPtr thisPtr, IntPtr beziers, int beziersCount);
            private static unsafe void AddQuadraticBeziersImpl(IntPtr thisPtr, IntPtr beziers, int beziersCount)
            {
                var shadow = ToShadow<GeometrySinkShadow>(thisPtr);
                var callback = (GeometrySink)shadow.Callback;
                var managedBeziers = new SharpDX.Direct2D1.QuadraticBezierSegment[beziersCount];
                Utilities.Read(beziers, managedBeziers, 0, beziersCount);
                callback.AddQuadraticBeziers(managedBeziers);
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddArcDelegate(IntPtr thisPtr, IntPtr arc);
            private static unsafe void AddArcImpl(IntPtr thisPtr, IntPtr arc)
            {
                var shadow = ToShadow<GeometrySinkShadow>(thisPtr);
                var callback = (GeometrySink)shadow.Callback;
                callback.AddArc(*((ArcSegment*)arc));
            }
        }
    }
}