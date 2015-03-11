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
    /// Internal SimplifiedGeometrySink Callback
    /// </summary>
    internal class SimplifiedGeometrySinkShadow : SharpDX.ComObjectShadow
    {
        private static readonly SimplifiedGeometrySinkVtbl Vtbl = new SimplifiedGeometrySinkVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(SimplifiedGeometrySink callback)
        {
            return ToCallbackPtr<SimplifiedGeometrySink>(callback);
        }
        public class SimplifiedGeometrySinkVtbl : ComObjectVtbl
        {
            public SimplifiedGeometrySinkVtbl(int nbMethods) : base(nbMethods + 7)
            {
                AddMethod(new SetFillModeDelegate(SetFillModeImpl));
                AddMethod(new SetSegmentFlagsDelegate(SetSegmentFlagsImpl));
                AddMethod(new BeginFigureDelegate(BeginFigureImpl));
                AddMethod(new AddLinesDelegate(AddLinesImpl));
                AddMethod(new AddBeziersDelegate(AddBeziersImpl));
                AddMethod(new EndFigureDelegate(EndFigureImpl));
                AddMethod(new CloseDelegate(CloseImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void SetFillModeDelegate(IntPtr thisPtr, SharpDX.Direct2D1.FillMode fillMode);
            private static void SetFillModeImpl(IntPtr thisPtr, SharpDX.Direct2D1.FillMode fillMode)
            {
                var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                var callback = (SimplifiedGeometrySink)shadow.Callback;
                callback.SetFillMode(fillMode);
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void SetSegmentFlagsDelegate(IntPtr thisPtr, SharpDX.Direct2D1.PathSegment vertexFlags);
            private static void SetSegmentFlagsImpl(IntPtr thisPtr, SharpDX.Direct2D1.PathSegment vertexFlags)
            {
                var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                var callback = (SimplifiedGeometrySink)shadow.Callback;
                callback.SetSegmentFlags(vertexFlags);
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void BeginFigureDelegate(IntPtr thisPtr, RawVector2 startPoint, SharpDX.Direct2D1.FigureBegin figureBegin);
            private static void BeginFigureImpl(IntPtr thisPtr, RawVector2 startPoint, SharpDX.Direct2D1.FigureBegin figureBegin)
            {
                var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                var callback = (SimplifiedGeometrySink)shadow.Callback;
                callback.BeginFigure(startPoint, figureBegin);
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddLinesDelegate(IntPtr thisPtr, IntPtr points, int pointsCount);
            private static void AddLinesImpl(IntPtr thisPtr, IntPtr points, int pointsCount)
            {
                unsafe
                {
                    var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                    var callback = (SimplifiedGeometrySink)shadow.Callback;
                    var managedPoints = new RawVector2[pointsCount];
                    Utilities.Read(points, managedPoints, 0, pointsCount);
                    callback.AddLines(managedPoints);
                }
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddBeziersDelegate(IntPtr thisPtr, IntPtr beziers, int beziersCount);
            private static void AddBeziersImpl(IntPtr thisPtr, IntPtr beziers, int beziersCount)
            {
                unsafe
                {
                    var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                    var callback = (SimplifiedGeometrySink)shadow.Callback;
                    var managedBeziers = new SharpDX.Direct2D1.BezierSegment[beziersCount];
                    Utilities.Read(beziers, managedBeziers, 0, beziersCount);
                    callback.AddBeziers(managedBeziers);
                }
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void EndFigureDelegate(IntPtr thisPtr, SharpDX.Direct2D1.FigureEnd figureEnd);
            private static void EndFigureImpl(IntPtr thisPtr, SharpDX.Direct2D1.FigureEnd figureEnd)
            {
                var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                var callback = (SimplifiedGeometrySink)shadow.Callback;
                callback.EndFigure(figureEnd);
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CloseDelegate(IntPtr thisPtr);
            private static int CloseImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<SimplifiedGeometrySinkShadow>(thisPtr);
                    var callback = (SimplifiedGeometrySink)shadow.Callback;
                    callback.Close();
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}