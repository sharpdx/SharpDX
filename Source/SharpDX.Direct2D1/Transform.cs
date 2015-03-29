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

using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Represents the base interface for all of the transforms implemented by the transform author.
    /// </summary>
    /// <unmanaged>ID2D1Transform</unmanaged>	
    [ShadowAttribute(typeof(TransformShadow))]
    public partial interface Transform
    {
        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Allows a transform to state how it would map a rectangle requested on its output to a set of sample rectangles on its input.</p>	
        /// </summary>	
        /// <param name="outputRect"><dd> <p>The output rectangle to which the inputs must be mapped.</p> </dd></param>	
        /// <param name="inputRects"><dd> <p>The corresponding set of inputs. The inputs will directly correspond to the transform inputs.</p> </dd></param>	
        /// <remarks>	
        /// <p>The transform implementation must ensure that any pixel shader or software callback implementation it provides honors this calculation.</p><p>The transform implementation must regard this method as purely functional. It can base the mapped input and output rectangles on its current state as specified by the encapsulating effect properties.    However, it must not change its own state in response to this method being invoked. The DirectImage renderer implementation reserves the right to call this method at any time and in any sequence.</p>	
        /// </remarks>	
        /// <msdn-id>hh446945</msdn-id>	
        /// <unmanaged>HRESULT ID2D1Transform::MapOutputRectToInputRects([In] const RECT* outputRect,[Out, Buffer] RECT* inputRects,[In] unsigned int inputRectsCount)</unmanaged>	
        /// <unmanaged-short>ID2D1Transform::MapOutputRectToInputRects</unmanaged-short>	
        void MapOutputRectangleToInputRectangles(RawRectangle outputRect, RawRectangle[] inputRects);

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Performs the inverse mapping to <strong>MapOutputRectToInputRects</strong>.</p>	
        /// </summary>	
        /// <param name="inputRects">No documentation.</param>	
        /// <param name="inputOpaqueSubRects">No documentation.</param>
        /// <param name="outputOpaqueSubRect">No documentation.</param>
        /// <returns>No outputOpaqueSubRect.</returns>	
        /// <remarks>	
        /// <p>The transform implementation must ensure that any pixel shader or software callback implementation it provides honors this calculation.</p><p>The transform implementation must regard this method as purely functional. It can base the mapped input and output rectangles on its current state as specified by the encapsulating effect properties. However, it must not change its own state in response to this method being invoked. The Direct2D renderer implementation reserves the right to call this method at any time and in any sequence.</p>	
        /// </remarks>	
        /// <msdn-id>hh446943</msdn-id>	
        /// <unmanaged>HRESULT ID2D1Transform::MapInputRectsToOutputRect([In, Buffer] const RECT* inputRects,[In, Buffer] const RECT* inputOpaqueSubRects,[In] unsigned int inputRectCount,[Out] RECT* outputRect,[Out] RECT* outputOpaqueSubRect)</unmanaged>	
        /// <unmanaged-short>ID2D1Transform::MapInputRectsToOutputRect</unmanaged-short>	
        RawRectangle MapInputRectanglesToOutputRectangle(RawRectangle[] inputRects, RawRectangle[] inputOpaqueSubRects, out RawRectangle outputOpaqueSubRect);

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="inputIndex">No documentation.</param>	
        /// <param name="invalidInputRect">No documentation.</param>	
        /// <returns>The rectangle invalidated.</returns>	
        /// <unmanaged>HRESULT ID2D1Transform::MapInvalidRect([In] unsigned int inputIndex,[In] RECT invalidInputRect,[Out] RECT* invalidOutputRect)</unmanaged>	
        /// <unmanaged-short>ID2D1Transform::MapInvalidRect</unmanaged-short>	
        RawRectangle MapInvalidRect(int inputIndex, RawRectangle invalidInputRect);
    }
}