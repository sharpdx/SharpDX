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
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal TextRenderer Callback
    /// </summary>
    internal class TextRendererCallback : PixelSnappingCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        public TextRenderer Callback { get; private set; }

        public static IntPtr CallbackToPtr(TextRenderer callback)
        {
            return CallbackToPtr<TextRenderer, TextRendererCallback>(callback);
        }

        public override void Attach<T>(T callback)
        {
            Attach(callback, 4);
            Callback = (TextRenderer)callback;
            AddMethod(new DrawGlyphRunDelegate(DrawGlyphRunImpl));
            AddMethod(new DrawUnderlineDelegate(DrawUnderlineImpl));
            AddMethod(new DrawStrikethroughDelegate(DrawStrikethroughImpl));
            AddMethod(new DrawInlineObjectDelegate(DrawInlineObjectImpl));
        }

        /// <unmanaged>HRESULT DrawGlyphRun([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[None] DWRITE_MEASURING_MODE measuringMode,[In] const DWRITE_GLYPH_RUN* glyphRun,[In] const DWRITE_GLYPH_RUN_DESCRIPTION* glyphRunDescription,[None] IUnknown* clientDrawingEffect)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DrawGlyphRunDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, SharpDX.DirectWrite.MeasuringMode measuringMode,
                                                  IntPtr glyphRunPtr, IntPtr glyphRunDescription, IntPtr clientDrawingEffect);
        private int DrawGlyphRunImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float baselineOriginX, float baselineOriginY, SharpDX.DirectWrite.MeasuringMode measuringMode,
                                     IntPtr glyphRunPtr, IntPtr glyphRunDescriptionPtr, IntPtr clientDrawingEffectPtr)
        {
            unsafe
            {                
                // Read GlyphRun
                var glyphRunData = default(GlyphRun.__Native);
                Utilities.Read(glyphRunPtr, ref glyphRunData);
                var glyphRun = new GlyphRun();
                glyphRun.__MarshalFrom(ref glyphRunData);

                // Read GlyphRunDescription
                var glyphRunDescriptionData = default(GlyphRunDescription.__Native);
                Utilities.Read(glyphRunDescriptionPtr, ref glyphRunDescriptionData);
                var glyphRunDescription = new GlyphRunDescription();
                glyphRunDescription.__MarshalFrom(ref glyphRunDescriptionData);

                return Callback.DrawGlyphRun(Utilities.GetObjectForIUnknown(clientDrawingContextPtr), baselineOriginX, baselineOriginY, measuringMode, glyphRun, glyphRunDescription,
                    (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
            }
        }

        /// <unmanaged>HRESULT DrawUnderline([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[In] const DWRITE_UNDERLINE* underline,[None] IUnknown* clientDrawingEffect)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DrawUnderlineDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, IntPtr underline, IntPtr clientDrawingEffect);
        private int DrawUnderlineImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float baselineOriginX, float baselineOriginY, IntPtr underline, IntPtr clientDrawingEffectPtr)
        {
            unsafe
            {
                Underline underlineData = default(Underline);
                Utilities.Read(underline, ref underlineData);
                return Callback.DrawUnderline(Utilities.GetObjectForIUnknown(clientDrawingContextPtr), baselineOriginX, baselineOriginY, ref underlineData, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
            }
        }

        /// <unmanaged>HRESULT DrawStrikethrough([None] void* clientDrawingContext,[None] FLOAT baselineOriginX,[None] FLOAT baselineOriginY,[In] const DWRITE_STRIKETHROUGH* strikethrough,[None] IUnknown* clientDrawingEffect)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DrawStrikethroughDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float baselineOriginX, float baselineOriginY, IntPtr strikethrough,
                                                       IntPtr clientDrawingEffect);
        private int DrawStrikethroughImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float baselineOriginX, float baselineOriginY, IntPtr strikethrough, IntPtr clientDrawingEffectPtr )
        {
            unsafe
            {
                Strikethrough strikethroughData = default(Strikethrough);
                Utilities.Read(strikethrough, ref strikethroughData);
                return Callback.DrawStrikethrough(Utilities.GetObjectForIUnknown(clientDrawingContextPtr), baselineOriginX, baselineOriginY, ref strikethroughData, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
            }            
        }

        /// <unmanaged>HRESULT DrawInlineObject([None] void* clientDrawingContext,[None] FLOAT originX,[None] FLOAT originY,[None] IDWriteInlineObject* inlineObject,[None] BOOL isSideways,[None] BOOL isRightToLeft,[None] IUnknown* clientDrawingEffect)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DrawInlineObjectDelegate(IntPtr thisObject, IntPtr clientDrawingContext, float originX, float originY, IntPtr inlineObject, int isSideways, int isRightToLeft, IntPtr clientDrawingEffect);
        private int DrawInlineObjectImpl(IntPtr thisObject, IntPtr clientDrawingContextPtr, float originX, float originY, IntPtr inlineObject, int isSideways, int isRightToLeft, IntPtr clientDrawingEffectPtr)
        {
            return Callback.DrawInlineObject(Utilities.GetObjectForIUnknown(clientDrawingContextPtr), originX, originY, new InlineObjectNative(inlineObject), isSideways != 0, isRightToLeft != 0, (ComObject)Utilities.GetObjectForIUnknown(clientDrawingEffectPtr)).Code;
        }
    }
}