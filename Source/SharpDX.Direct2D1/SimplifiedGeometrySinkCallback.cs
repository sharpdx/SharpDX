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
using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal SimplifiedGeometrySink Callback
    /// </summary>
    internal class SimplifiedGeometrySinkCallback : SharpDX.ComObjectCallbackNative
    {
        private SimplifiedGeometrySink Callback { get; set; }

        public static IntPtr CallbackToPtr(SimplifiedGeometrySink callback)
        {
            return CallbackToPtr<SimplifiedGeometrySink, SimplifiedGeometrySinkCallback>(callback);
        }

        public override void Attach<T>(T callback)
        {
            Attach(callback, 0);
        }

        protected override void Attach<T>(T callback, int nbMethods)
        {
            base.Attach(callback, nbMethods + 7);
            Callback = (SimplifiedGeometrySink)callback;
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
        private void SetFillModeImpl(IntPtr thisPtr, SharpDX.Direct2D1.FillMode fillMode)
        {
            Callback.SetFillMode(fillMode);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void SetSegmentFlagsDelegate(IntPtr thisPtr, SharpDX.Direct2D1.PathSegment vertexFlags);
        private void SetSegmentFlagsImpl(IntPtr thisPtr, SharpDX.Direct2D1.PathSegment vertexFlags)
        {
            Callback.SetSegmentFlags(vertexFlags);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void BeginFigureDelegate(IntPtr thisPtr, System.Drawing.PointF startPoint, SharpDX.Direct2D1.FigureBegin figureBegin);
        private void BeginFigureImpl(IntPtr thisPtr, System.Drawing.PointF startPoint, SharpDX.Direct2D1.FigureBegin figureBegin)
        {
            Callback.BeginFigure(startPoint,figureBegin);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void AddLinesDelegate(IntPtr thisPtr, IntPtr points, int pointsCount);
        private void AddLinesImpl(IntPtr thisPtr, IntPtr points, int pointsCount)
        {
            unsafe
            {
                System.Drawing.PointF[] managedPoints = new PointF[pointsCount];
                Utilities.Read(points, managedPoints, 0, pointsCount);
                Callback.AddLines(managedPoints);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void AddBeziersDelegate(IntPtr thisPtr, IntPtr beziers, int beziersCount);
        private void AddBeziersImpl(IntPtr thisPtr, IntPtr beziers, int beziersCount)
        {
            unsafe
            {
                SharpDX.Direct2D1.BezierSegment[] managedBeziers = new SharpDX.Direct2D1.BezierSegment[beziersCount];
                Utilities.Read(beziers, managedBeziers, 0, beziersCount);
                Callback.AddBeziers(managedBeziers);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void EndFigureDelegate(IntPtr thisPtr, SharpDX.Direct2D1.FigureEnd figureEnd);
        private void EndFigureImpl(IntPtr thisPtr, SharpDX.Direct2D1.FigureEnd figureEnd)
        {
            Callback.EndFigure(figureEnd);
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CloseDelegate(IntPtr thisPtr);
        private int CloseImpl(IntPtr thisPtr)
        {
            try
            {
                Callback.Close();
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }
    }
}