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
    /// Internal CommandSink Callback
    /// </summary>
    internal class CommandSinkShadow : SharpDX.ComObjectShadow
    {
        private unsafe static readonly CommandSinkVtbl Vtbl = new CommandSinkVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(CommandSink callback)
        {
            return ToCallbackPtr<CommandSink>(callback);
        }

        public class CommandSinkVtbl : ComObjectVtbl
        {
            public CommandSinkVtbl()
                : this(0)
            {
            }

            public CommandSinkVtbl(int numMethods)
                : base(numMethods + 28)
            {
                AddMethod(new CallNoParams(BeginDrawImpl));
                AddMethod(new CallNoParams(EndDrawImpl));
                AddMethod(new SetAntialiasModeDelegate(SetAntialiasModeImpl));

                AddMethod(new SetTagsDelegate(SetTagsImpl));
                AddMethod(new SetTextAntialiasModeDelegate(SetTextAntialiasModeImpl));
                AddMethod(new SetTextRenderingParamsDelegate(SetTextRenderingParamsImpl));

                AddMethod(new SetTransformDelegate(SetTransformImpl));
                AddMethod(new SetPrimitiveBlendDelegate(SetPrimitiveBlendImpl));
                AddMethod(new SetUnitModeDelegate(SetUnitModeImpl));

                AddMethod(new ClearDelegate(ClearImpl));
                AddMethod(new DrawGlyphRunDelegate(DrawGlyphRunImpl));
                AddMethod(new DrawLineDelegate(DrawLineImpl));

                AddMethod(new DrawGeometryDelegate(DrawGeometryImpl));
                AddMethod(new DrawRectangleDelegate(DrawRectangleImpl));
                AddMethod(new DrawBitmapDelegate(DrawBitmapImpl));

                AddMethod(new DrawImageDelegate(DrawImageImpl));
                AddMethod(new DrawGdiMetafileDelegate(DrawGdiMetafileImpl));
                AddMethod(new FillMeshDelegate(FillMeshImpl));

                AddMethod(new FillOpacityMaskDelegate(FillOpacityMaskImpl));
                AddMethod(new FillGeometryDelegate(FillGeometryImpl));
                AddMethod(new FillRectangleDelegate(FillRectangleImpl));

                AddMethod(new PushAxisAlignedClipDelegate(PushAxisAlignedClipImpl));
                AddMethod(new PushLayerDelegate(PushLayerImpl));
                AddMethod(new CallNoParams(PopAxisAlignedClipImpl));

                AddMethod(new CallNoParams(PopLayerImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CallNoParams(IntPtr thisPtr);

            /// <unmanaged>HRESULT ID2D1CommandSink::BeginDraw()</unmanaged>	
            private unsafe static int BeginDrawImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.BeginDraw();
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::EndDraw()</unmanaged>	
            private unsafe static int EndDrawImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.EndDraw();
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::SetAntialiasMode([In] D2D1_ANTIALIAS_MODE antialiasMode)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetAntialiasModeDelegate(IntPtr thisPtr, SharpDX.Direct2D1.AntialiasMode antialiasMode);
            private unsafe static int SetAntialiasModeImpl(IntPtr thisPtr, SharpDX.Direct2D1.AntialiasMode antialiasMode)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.AntialiasMode = antialiasMode;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::SetTags([In] unsigned longlong tag1,[In] unsigned longlong tag2)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetTagsDelegate(IntPtr thisPtr, long tag1, long tag2);
            private unsafe static int SetTagsImpl(IntPtr thisPtr, long tag1, long tag2)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.SetTags(tag1, tag2);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::SetTextAntialiasMode([In] D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetTextAntialiasModeDelegate(IntPtr thisPtr, SharpDX.Direct2D1.TextAntialiasMode textAntialiasMode);
            private unsafe static int SetTextAntialiasModeImpl(IntPtr thisPtr, SharpDX.Direct2D1.TextAntialiasMode textAntialiasMode)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.TextAntialiasMode = textAntialiasMode;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::SetTextRenderingParams([In, Optional] IDWriteRenderingParams* textRenderingParams)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetTextRenderingParamsDelegate(IntPtr thisPtr, IntPtr textRenderingParams);
            private unsafe static int SetTextRenderingParamsImpl(IntPtr thisPtr, IntPtr textRenderingParams)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.TextRenderingParams = new DirectWrite.RenderingParams(textRenderingParams);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::SetTransform([In] const D2D_MATRIX_3X2_F* transform)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetTransformDelegate(IntPtr thisPtr, IntPtr transform);
            private unsafe static int SetTransformImpl(IntPtr thisPtr, IntPtr transform)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.Transform = *(RawMatrix3x2*)transform;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// Sets the blending for primitives.
            /// </summary>	
            /// <unmanaged>HRESULT ID2D1CommandSink::SetPrimitiveBlend([In] D2D1_PRIMITIVE_BLEND primitiveBlend)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetPrimitiveBlendDelegate(IntPtr thisPtr, SharpDX.Direct2D1.PrimitiveBlend primitiveBlend);
            private unsafe static int SetPrimitiveBlendImpl(IntPtr thisPtr, SharpDX.Direct2D1.PrimitiveBlend primitiveBlend)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.PrimitiveBlend = primitiveBlend;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// Sets the unit mode
            /// </summary>	
            /// <unmanaged>HRESULT ID2D1CommandSink::SetUnitMode([In] D2D1_UNIT_MODE unitMode)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetUnitModeDelegate(IntPtr thisPtr, SharpDX.Direct2D1.UnitMode unitMode);
            private unsafe static int SetUnitModeImpl(IntPtr thisPtr, SharpDX.Direct2D1.UnitMode unitMode)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.UnitMode = unitMode;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::Clear([In, Optional] const D2D_COLOR_F* color)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int ClearDelegate(IntPtr thisPtr, IntPtr color);
            private unsafe static int ClearImpl(IntPtr thisPtr, IntPtr color)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.Clear(color == IntPtr.Zero ? (RawColor4?)null : *(RawColor4*)color);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawGlyphRun([In] D2D_POINT_2F baselineOrigin,[In] const DWRITE_GLYPH_RUN* glyphRun,[In, Optional] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[In] ID2D1Brush* foregroundBrush,[In] DWRITE_MEASURING_MODE measuringMode)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawGlyphRunDelegate(IntPtr thisPtr, RawVector2 baselineOrigin, IntPtr glyphRun, IntPtr glyphRunDescriptionPtr, IntPtr foregroundBrush, SharpDX.Direct2D1.MeasuringMode measuringMode);
            private unsafe static int DrawGlyphRunImpl(IntPtr thisPtr, RawVector2 baselineOrigin, IntPtr glyphRunNative, IntPtr glyphRunDescriptionPtr, IntPtr foregroundBrush, SharpDX.Direct2D1.MeasuringMode measuringMode)
            {
                var glyphRun = new DirectWrite.GlyphRun();
                try
                {
                    var glyphRunDescription = new SharpDX.DirectWrite.GlyphRunDescription();
                    glyphRunDescription.__MarshalFrom(ref *(SharpDX.DirectWrite.GlyphRunDescription.__Native*)glyphRunDescriptionPtr);

                    glyphRun.__MarshalFrom(ref *(DirectWrite.GlyphRun.__Native*)glyphRunNative);

                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawGlyphRun(baselineOrigin, glyphRun, glyphRunDescription, new Brush(foregroundBrush), measuringMode);

                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                finally
                {
                    glyphRun.__MarshalFree(ref *(DirectWrite.GlyphRun.__Native*)glyphRunNative);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawLine([In] D2D_POINT_2F point0,[In] D2D_POINT_2F point1,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawLineDelegate(IntPtr thisPtr, RawVector2 point0, RawVector2 point1, IntPtr brush, float strokeWidth, IntPtr strokeStyle);
            private unsafe static int DrawLineImpl(IntPtr thisPtr, RawVector2 point0, RawVector2 point1, IntPtr brush, float strokeWidth, IntPtr strokeStyle)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawLine(point0, point1, new Brush(brush), strokeWidth, new StrokeStyle(strokeStyle));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawGeometryDelegate(IntPtr thisPtr, IntPtr geometry, IntPtr brush, float strokeWidth, IntPtr strokeStyle);
            private unsafe static int DrawGeometryImpl(IntPtr thisPtr, IntPtr geometry, IntPtr brush, float strokeWidth, IntPtr strokeStyle)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawGeometry(new Geometry(geometry), new Brush(brush), strokeWidth, new StrokeStyle(strokeStyle));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawRectangle([In] const D2D_RECT_F* rect,[In] ID2D1Brush* brush,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawRectangleDelegate(IntPtr thisPtr, IntPtr rect, IntPtr brush, float strokeWidth, IntPtr strokeStyle);
            private unsafe static int DrawRectangleImpl(IntPtr thisPtr, IntPtr rect, IntPtr brush, float strokeWidth, IntPtr strokeStyle)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawRectangle(*(RawRectangleF*)rect, new Brush(brush), strokeWidth, new StrokeStyle(strokeStyle));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawBitmap([In] ID2D1Bitmap* bitmap,[In, Optional] const D2D_RECT_F* destinationRectangle,[In] float opacity,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In, Optional] const D2D_RECT_F* sourceRectangle,[In, Optional] const D2D_MATRIX_4X4_F* perspectiveTransform)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawBitmapDelegate(IntPtr thisPtr, IntPtr bitmap, IntPtr destinationRectangle, float opacity, SharpDX.Direct2D1.InterpolationMode interpolationMode, IntPtr sourceRectangle, IntPtr erspectiveTransformRef);
            private unsafe static int DrawBitmapImpl(IntPtr thisPtr, IntPtr bitmap, IntPtr destinationRectangle, float opacity, SharpDX.Direct2D1.InterpolationMode interpolationMode, IntPtr sourceRectangle, IntPtr erspectiveTransformRef)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawBitmap(new Bitmap(bitmap),
                        destinationRectangle == IntPtr.Zero ? (RawRectangleF?)null : *(RawRectangleF*)destinationRectangle,
                        opacity,
                        interpolationMode,
                        sourceRectangle == IntPtr.Zero ? (RawRectangleF?)null : *(RawRectangleF*)sourceRectangle,
                        erspectiveTransformRef == IntPtr.Zero ? (RawMatrix?)null : *(RawMatrix*)erspectiveTransformRef);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawImage([In] ID2D1Image* image,[In, Optional] const D2D_POINT_2F* targetOffset,[In, Optional] const D2D_RECT_F* imageRectangle,[In] D2D1_INTERPOLATION_MODE interpolationMode,[In] D2D1_COMPOSITE_MODE compositeMode)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawImageDelegate(IntPtr thisPtr, IntPtr image, IntPtr targetOffset, IntPtr imageRectangle, SharpDX.Direct2D1.InterpolationMode interpolationMode, SharpDX.Direct2D1.CompositeMode compositeMode);
            private unsafe static int DrawImageImpl(IntPtr thisPtr, IntPtr image, IntPtr targetOffset, IntPtr imageRectangle, SharpDX.Direct2D1.InterpolationMode interpolationMode, SharpDX.Direct2D1.CompositeMode compositeMode)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawImage(new Image(image),
                        targetOffset == IntPtr.Zero ? (RawVector2?)null : *(RawVector2*)targetOffset,
                        imageRectangle == IntPtr.Zero ? (RawRectangleF?)null : *(RawRectangleF*)imageRectangle,
                        interpolationMode,
                        compositeMode);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::DrawGdiMetafile([In] ID2D1GdiMetafile* gdiMetafile,[In, Optional] const D2D_POINT_2F* targetOffset)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawGdiMetafileDelegate(IntPtr thisPtr, IntPtr gdiMetafile, IntPtr targetOffset);
            private unsafe static int DrawGdiMetafileImpl(IntPtr thisPtr, IntPtr gdiMetafile, IntPtr targetOffset)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.DrawGdiMetafile(new GdiMetafile(gdiMetafile),
                        targetOffset == IntPtr.Zero ? (RawVector2?)null : *(RawVector2*)targetOffset);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// [This documentation is preliminary and is subject to change.]	
            /// </summary>	
            /// <param name="mesh"><para>The mesh object to be filled.</para></param>	
            /// <param name="brush"><para>The brush with which to fill the mesh.</para></param>	
            /// <unmanaged>HRESULT ID2D1CommandSink::FillMesh([In] ID2D1Mesh* mesh,[In] ID2D1Brush* brush)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int FillMeshDelegate(IntPtr thisPtr, IntPtr mesh, IntPtr brush);
            private unsafe static int FillMeshImpl(IntPtr thisPtr, IntPtr mesh, IntPtr brush)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.FillMesh(new Mesh(mesh), new Brush(brush));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::FillOpacityMask([In] ID2D1Bitmap* opacityMask,[In] ID2D1Brush* brush,[In, Optional] const D2D_RECT_F* destinationRectangle,[In, Optional] const D2D_RECT_F* sourceRectangle)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int FillOpacityMaskDelegate(IntPtr thisPtr, IntPtr opacityMask, IntPtr brush, IntPtr destinationRectangle, IntPtr sourceRectangle);
            private unsafe static int FillOpacityMaskImpl(IntPtr thisPtr, IntPtr opacityMask, IntPtr brush, IntPtr destinationRectangle, IntPtr sourceRectangle)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.FillOpacityMask(new Bitmap(opacityMask), new Brush(brush),
                        destinationRectangle == IntPtr.Zero ? (RawRectangleF?)null : *(RawRectangleF*)destinationRectangle,
                        sourceRectangle == IntPtr.Zero ? (RawRectangleF?)null : *(RawRectangleF*)sourceRectangle);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::FillGeometry([In] ID2D1Geometry* geometry,[In] ID2D1Brush* brush,[In, Optional] ID2D1Brush* opacityBrush)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int FillGeometryDelegate(IntPtr thisPtr, IntPtr geometry, IntPtr brush, IntPtr opacityBrush);
            private unsafe static int FillGeometryImpl(IntPtr thisPtr, IntPtr geometry, IntPtr brush, IntPtr opacityBrush)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.FillGeometry(new Geometry(geometry), new Brush(brush), new Brush(opacityBrush));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1CommandSink::FillRectangle([In] const D2D_RECT_F* rect,[In] ID2D1Brush* brush)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int FillRectangleDelegate(IntPtr thisPtr, IntPtr rect, IntPtr brush);
            private unsafe static int FillRectangleImpl(IntPtr thisPtr, IntPtr rect, IntPtr brush)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.FillRectangle(*(RawRectangleF*)rect, new Brush(brush));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// [This documentation is preliminary and is subject to change.]	
            /// </summary>	
            /// <param name="clipRect"><para>The rectangle that defines the clip.</para></param>	
            /// <param name="antialiasMode"><para>Whether the given clip should be antialiased.</para></param>	
            /// <remarks>	
            /// If the current world transform is not preserving the axis, clipRectangle is transformed and the bounds of the transformed rectangle are used instead.	
            /// </remarks>	
            /// <unmanaged>HRESULT ID2D1CommandSink::PushAxisAlignedClip([In] const D2D_RECT_F* clipRect,[In] D2D1_ANTIALIAS_MODE antialiasMode)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int PushAxisAlignedClipDelegate(IntPtr thisPtr, IntPtr clipRect, SharpDX.Direct2D1.AntialiasMode antialiasMode);
            private unsafe static int PushAxisAlignedClipImpl(IntPtr thisPtr, IntPtr clipRect, SharpDX.Direct2D1.AntialiasMode antialiasMode)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.PushAxisAlignedClip(*(RawRectangleF*)clipRect, antialiasMode);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <param name="layerParameters1">No documentation.</param>	
            /// <param name="layer">No documentation.</param>	
            /// <unmanaged>HRESULT ID2D1CommandSink::PushLayer([In] const D2D1_LAYER_PARAMETERS1* layerParameters1,[In, Optional] ID2D1Layer* layer)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int PushLayerDelegate(IntPtr thisPtr, IntPtr layerParameters1, IntPtr layer);
            private unsafe static int PushLayerImpl(IntPtr thisPtr, IntPtr layerParameters1, IntPtr layer)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    var layerParameters = new LayerParameters1();
                    layerParameters.__MarshalFrom(ref *(LayerParameters1.__Native*)layerParameters1);
                    callback.PushLayer(ref layerParameters, layer == IntPtr.Zero ? null : new Layer(layer));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// [This documentation is preliminary and is subject to change.]	
            /// </summary>	
            /// <unmanaged>HRESULT ID2D1CommandSink::PopAxisAlignedClip()</unmanaged>	
            private unsafe static int PopAxisAlignedClipImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.PopAxisAlignedClip();
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <summary>	
            /// No documentation.	
            /// </summary>	
            /// <unmanaged>HRESULT ID2D1CommandSink::PopLayer()</unmanaged>	
            private unsafe static int PopLayerImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<CommandSinkShadow>(thisPtr);
                    var callback = (CommandSink)shadow.Callback;
                    callback.PopLayer();
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
