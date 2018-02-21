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

namespace SharpDX.DirectWrite
{
    public partial class InlineObjectNative
    {
        /// <summary>	
        /// The application implemented rendering callback (<see cref="TextRenderer.DrawInlineObject"/>) can use this to draw the inline object without needing to cast or query the object type. The text layout does not call this method directly. 	
        /// </summary>	
        /// <param name="clientDrawingContext">The drawing context passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/>.  This parameter may be NULL. </param>
        /// <param name="renderer">The same renderer passed to <see cref="SharpDX.DirectWrite.TextLayout.Draw_"/> as the object's containing parent.  This is useful if the inline object is recursive such as a nested layout. </param>
        /// <param name="originX">The x-coordinate at the upper-left corner of the inline object. </param>
        /// <param name="originY">The y-coordinate at the upper-left corner of the inline object. </param>
        /// <param name="isSideways">A Boolean flag that indicates whether the object's baseline runs alongside the baseline axis of the line. </param>
        /// <param name="isRightToLeft">A Boolean flag that indicates whether the object is in a right-to-left context and should be drawn flipped. </param>
        /// <param name="clientDrawingEffect">The drawing effect set in <see cref="SharpDX.DirectWrite.TextLayout.SetDrawingEffect"/>.  Usually this effect is a foreground brush that  is used in glyph drawing. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteInlineObject::Draw([None] void* clientDrawingContext,[None] IDWriteTextRenderer* renderer,[None] float originX,[None] float originY,[None] BOOL isSideways,[None] BOOL isRightToLeft,[None] IUnknown* clientDrawingEffect)</unmanaged>
        public void Draw(object clientDrawingContext, TextRenderer renderer, float originX, float originY, bool isSideways, bool isRightToLeft, ComObject clientDrawingEffect)
        {
            var handle = GCHandle.Alloc(clientDrawingContext);
            IntPtr clientDrawingEffectPtr = Utilities.GetIUnknownForObject(clientDrawingEffect);
            try
            {
                this.Draw_(GCHandle.ToIntPtr(handle), renderer, originX, originY, isSideways, isRightToLeft,
                            clientDrawingEffectPtr);
            } finally
            {
                if (handle.IsAllocated) handle.Free();
                if (clientDrawingEffectPtr != IntPtr.Zero) Marshal.Release(clientDrawingEffectPtr);
            }
        }

        /// <summary>	
        /// <see cref="SharpDX.DirectWrite.TextLayout"/> calls this callback function to get the measurement of the inline object. 	
        /// </summary>	
        /// <returns>A structure describing the geometric measurement of an application-defined inline object.  These metrics are in relation to the baseline of the adjacent text. </returns>
        /// <unmanaged>HRESULT IDWriteInlineObject::GetMetrics([Out] DWRITE_INLINE_OBJECT_METRICS* metrics)</unmanaged>
        public InlineObjectMetrics Metrics
        {
            get
            {
                InlineObjectMetrics temp;
                this.GetMetrics_(out temp);
                return temp;
            }
        }

        /// <summary>	
        /// TextLayout calls this callback function to get the visible extents (in DIPs) of the inline object. In the case of a simple bitmap, with no padding and no overhang, all the overhangs will simply be zeroes.	
        /// </summary>	
        /// <returns>Overshoot of visible extents (in DIPs) outside the object. </returns>
        /// <unmanaged>HRESULT IDWriteInlineObject::GetOverhangMetrics([Out] DWRITE_OVERHANG_METRICS* overhangs)</unmanaged>
        public OverhangMetrics OverhangMetrics
        {
            get
            {
                OverhangMetrics temp;
                GetOverhangMetrics_(out temp);
                return temp;
            }
        }

        /// <summary>	
        /// Layout uses this to determine the line-breaking behavior of the inline object among the text. 	
        /// </summary>	
        /// <param name="breakConditionBefore">When this method returns, contains a value which indicates the line-breaking condition between the object and the content immediately preceding it. </param>
        /// <param name="breakConditionAfter">When this method returns, contains a value which indicates the line-breaking condition between the object and the content immediately following it. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteInlineObject::GetBreakConditions([Out] DWRITE_BREAK_CONDITION* breakConditionBefore,[Out] DWRITE_BREAK_CONDITION* breakConditionAfter)</unmanaged>
        public void GetBreakConditions(out BreakCondition breakConditionBefore, out BreakCondition breakConditionAfter)
        {
            GetBreakConditions_(out breakConditionBefore, out breakConditionAfter);
        }
    }
}