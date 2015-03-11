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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal TessellationSink Callback
    /// </summary>
    internal abstract class PixelSnappingShadow : SharpDX.ComObjectShadow
    {
        public class PixelSnappingVtbl : ComObjectVtbl
        {
            protected PixelSnappingVtbl(int nbMethods) : base(nbMethods + 3)
            {
                AddMethod(new IsPixelSnappingDisabledDelegate(IsPixelSnappingDisabledImpl));
                AddMethod(new GetCurrentTransformDelegate(GetCurrentTransformImpl));
                AddMethod(new GetPixelsPerDipDelegate(GetPixelsPerDipImpl));
            }

            /// <summary>
            /// Determines whether pixel snapping is disabled. The recommended default is FALSE,
            /// unless doing animation that requires subpixel vertical placement.
            /// </summary>
            /// <param name="thisPtr">This pointer</param>
            /// <param name="clientDrawingContext">The context passed to IDWriteTextLayout::Draw.</param>
            /// <param name="isDisabled">Output disabled</param>
            /// <returns>Receives TRUE if pixel snapping is disabled or FALSE if it not. </returns>
            /// <unmanaged>HRESULT IsPixelSnappingDisabled([None] void* clientDrawingContext,[Out] BOOL* isDisabled)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int IsPixelSnappingDisabledDelegate(IntPtr thisPtr, IntPtr clientDrawingContext, out int isDisabled);
            private static int IsPixelSnappingDisabledImpl(IntPtr thisPtr, IntPtr clientDrawingContextPtr, out int isDisabled)
            {
                isDisabled = 0;
                try
                {
                    var shadow = ToShadow<PixelSnappingShadow>(thisPtr);
                    var callback = (PixelSnapping) shadow.Callback;
                    isDisabled = callback.IsPixelSnappingDisabled(GCHandle.FromIntPtr(clientDrawingContextPtr).Target) ? 1 : 0;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            ///  Gets a transform that maps abstract coordinates to DIPs. 	
            /// </summary>
            /// <param name="thisPtr">This pointer</param>
            /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
            /// <param name="transform">Matrix transform</param>
            /// <returns>a structure which has transform information for  pixel snapping.</returns>
            /// <unmanaged>HRESULT GetCurrentTransform([None] void* clientDrawingContext,[Out] DWRITE_MATRIX* transform)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetCurrentTransformDelegate(IntPtr thisPtr, IntPtr clientDrawingContext, IntPtr transform);
            private static int GetCurrentTransformImpl(IntPtr thisPtr, IntPtr clientDrawingContextPtr, IntPtr transform)
            {
                unsafe
                {
                    RawMatrix3x2 matrix;
                    try
                    {
                        var shadow = ToShadow<PixelSnappingShadow>(thisPtr);
                        var callback = (PixelSnapping)shadow.Callback;
                        matrix = callback.GetCurrentTransform(GCHandle.FromIntPtr(clientDrawingContextPtr).Target);
                        //SharpDX.Direct2D1.LocalInterop.Write((void*)transform, ref matrix);
                        SharpDX.Utilities.Write(transform, ref matrix);
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


            /// <summary>	
            ///  Gets the number of physical pixels per DIP. 	
            /// </summary>	
            /// <remarks>	
            ///  Because a DIP (device-independent pixel) is 1/96 inch,  the pixelsPerDip value is the number of logical pixels per inch divided by 96.	
            /// </remarks>
            /// <param name="thisPtr">This pointer</param>
            /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.</param>
            /// <param name="pixelPerDip">Dip</param>
            /// <returns>the number of physical pixels per DIP</returns>
            /// <unmanaged>HRESULT GetPixelsPerDip([None] void* clientDrawingContext,[Out] FLOAT* pixelsPerDip)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetPixelsPerDipDelegate(IntPtr thisPtr, IntPtr clientDrawingContext, out float pixelPerDip);
            private static int GetPixelsPerDipImpl(IntPtr thisPtr, IntPtr clientDrawingContextPtr, out float pixelPerDip)
            {
                pixelPerDip = 0;
                try
                {
                    var shadow = ToShadow<PixelSnappingShadow>(thisPtr);
                    var callback = (PixelSnapping)shadow.Callback;
                    pixelPerDip = callback.GetPixelsPerDip(GCHandle.FromIntPtr(clientDrawingContextPtr).Target);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }
    }
}