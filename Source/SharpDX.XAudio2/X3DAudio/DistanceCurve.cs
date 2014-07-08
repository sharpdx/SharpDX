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

namespace SharpDX.X3DAudio
{
    internal partial struct DistanceCurve
    {
        public static unsafe IntPtr FromCurvePoints(CurvePoint[] points)
        {
            if (points == null || points.Length == 0)
                return IntPtr.Zero;

            var pDistanceCurve = (DistanceCurve*)Marshal.AllocHGlobal(Utilities.SizeOf<DistanceCurve>() + points.Length * Utilities.SizeOf<CurvePoint>());
            var pPoints = (CurvePoint*)&pDistanceCurve[1];
            pDistanceCurve->PointCount = points.Length;
            pDistanceCurve->PointsPointer = new IntPtr(pPoints);
            Utilities.Write(pDistanceCurve->PointsPointer, points, 0, points.Length);
            return (IntPtr) pDistanceCurve;
        }
    }
}