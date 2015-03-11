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

using SharpDX.Direct2D1;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal TextRenderer Callback
    /// </summary>
    internal class TextRendererShadow : PixelSnappingShadow
    {
        private static readonly TextRendererVtbl Vtbl = new TextRendererVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(TextRenderer callback)
        {
            return ToCallbackPtr<TextRenderer>(callback);
        }

        private class TextRendererVtbl : PixelSnappingVtbl
        {
            public TextRendererVtbl() : base(4)
            {
                unsafe
                {
                    AddMethod(new DrawGlyphRunDelegate(DrawGlyphRunImpl));
                    AddMethod(new DrawUnderlineDelegate(DrawUnderlineImpl));
                    AddMethod(new DrawStrikethroughDelegate(DrawStrikethroughImpl));
                    AddMethod(new DrawInlineObjectDelegate(DrawInlineObjectImpl));
                }
            }

            /// <unmanaged>HRESULT DrawGlyphRun([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[None] DWRITE_MEASURING_MODE measuringMode,[In] const DWRITE_GLYPH_RUN* glyphRun,[In] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[None] IUnknown* clientDrawingEffect)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawGlyphRunDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode,
                                                      IntPtr glyphRunPtr, IntPtr glyphRunDescription, IntPtr clientDrawingEffect);
            private static int DrawGlyphRunImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode,
                                         IntPtr glyphRunPtr, IntPtr glyphRunDescriptionPtr, IntPtr clientDrawingEffectPtr)
            {
                unsafe
                {
                    var shadow = ToShadow<TextRendererShadow>(thisObject);
                    var callback = (TextRenderer) shadow.Callback;

                    // Read GlyphRun
                    var glyphRunData = default(GlyphRun.__Native);
                    Utilities.Read(glyphRunPtr, ref glyphRunData);
                    using (var glyphRun = new GlyphRun())
                    {
                        glyphRun.__MarshalFrom(ref glyphRunData);

                        // Read GlyphRunDescription
                        var glyphRunDescriptionData = default(GlyphRunDescription.__Native);
                        Utilities.Read(glyphRunDescriptionPtr, ref glyphRunDescriptionData);
                        var glyphRunDescription = new GlyphRunDescription();
                        glyphRunDescription.__MarshalFrom(ref glyphRunDescriptionData);

                        return
                            callback.DrawGlyphRun(GCHandle.FromIntPtr(clientDrawingContextPtr).Target, baselineOriginX,
                                                  baselineOriginY, measuringMode, glyphRun, glyphRunDescription,
                                                  (ComObject) Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
                    }
                }
            }

            /// <unmanaged>HRESULT DrawUnderline([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[In] const DWRITE_UNDERLINE* underline,[None] IUnknown* clientDrawingEffect)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int DrawUnderlineDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, Underline.__Native* underline, IntPtr clientDrawingEffect);
            private unsafe static int DrawUnderlineImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float baselineOriginX, float baselineOriginY, Underline.__Native* underline, IntPtr clientDrawingEffectPtr)
            {
                unsafe
                {
                    var shadow = ToShadow<TextRendererShadow>(thisObject);
                    var callback = (TextRenderer)shadow.Callback;

                    var underlineData = new Underline();
                    underlineData.__MarshalFrom(ref *underline);
                    return callback.DrawUnderline(GCHandle.FromIntPtr(clientDrawingContextPtr).Target, baselineOriginX, baselineOriginY, ref underlineData, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
                }
            }

            /// <unmanaged>HRESULT DrawStrikethrough([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[In] const DWRITE_STRIKETHROUGH* strikethrough,[None] IUnknown* clientDrawingEffect)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int DrawStrikethroughDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, Strikethrough.__Native* strikethrough,
                                                           IntPtr clientDrawingEffect);
            private unsafe static int DrawStrikethroughImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float baselineOriginX, float baselineOriginY, Strikethrough.__Native* strikethrough, IntPtr clientDrawingEffectPtr)
            {
                unsafe
                {
                    var shadow = ToShadow<TextRendererShadow>(thisObject);
                    var callback = (TextRenderer)shadow.Callback;

                    var strikethroughData = new Strikethrough();
                    strikethroughData.__MarshalFrom(ref *strikethrough);
                    return callback.DrawStrikethrough(GCHandle.FromIntPtr(clientDrawingContextPtr).Target, baselineOriginX, baselineOriginY, ref strikethroughData, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
                }
            }

            /// <unmanaged>HRESULT DrawInlineObject([None] void* clientDrawingContext,[None] FLOAT originX,[None] FLOAT originY,[None] IDWriteInlineObject* inlineObject,[None] BOOL isSideways,[None] BOOL isRightToLeft,[None] IUnknown* clientDrawingEffect)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawInlineObjectDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float originX, float originY, IntPtr inlineObject, int isSideways, int isRightToLeft, IntPtr clientDrawingEffect);
            private static int DrawInlineObjectImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float originX, float originY, IntPtr inlineObject, int isSideways, int isRightToLeft, IntPtr clientDrawingEffectPtr)
            {
                var shadow = ToShadow<TextRendererShadow>(thisObject);
                var callback = (TextRenderer)shadow.Callback;
                return callback.DrawInlineObject(GCHandle.FromIntPtr(clientDrawingContextPtr).Target, originX, originY, new InlineObjectNative(inlineObject), isSideways != 0, isRightToLeft != 0, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}