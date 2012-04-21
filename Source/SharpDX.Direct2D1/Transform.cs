// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
#if DIRECTX11_1
using System;
using System.Collections.Generic;
using System.Text;

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
        /// Sets the input rectangles for this rendering pass into the transform.
        /// </summary>	
        /// <remarks>	
        /// Unlike the MapInputRectsToOutputRect and MapOutputRectToInputRects methods, this method is explicitly called by the renderer at a determined place in its rendering algorithm. The transform implementation may change its state based on the input rectangles and uses this information to control its rendering information.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1Transform::SetInputRects([In, Buffer] const RECT* inputRects,[In] unsigned int inputRectsCount)</unmanaged>	
        SharpDX.Rectangle[] InputRectangles { set; }

        /// <summary>	
        /// Allows a transform to state how it would map a rectangle requested on its output to a set of sample rectangles on its input.
        /// </summary>	
        /// <param name="outputRect"><para>The output rectangle to which the inputs must be mapped.</para></param>	
        /// <param name="inputRects"><para>The corresponding set of inputs. The inputs will directly correspond to the transform inputs.</para></param>	
        /// <remarks>	
        /// The transform implementation must ensure that any pixel shader or software callback implementation it provides honors this calculation.The transform implementation must regard this method as purely functional. It can base the mapped input and output rectangles on its current state as specified by the encapsulating effect properties.    However, it must not change its own state in response to this method being invoked. The DirectImage renderer implementation reserves the right to call this method at any time and in any sequence.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID2D1Transform::MapOutputRectToInputRects([In] const RECT* outputRect,[Out, Buffer] RECT* inputRects,[In] unsigned int inputRectsCount)</unmanaged>	
        void MapOutputRectangleToInputRectangles(SharpDX.Rectangle outputRect, SharpDX.Rectangle[] inputRects);

        /// <summary>	
        /// Performs the inverse mapping to <see cref="MapOutputRectToInputRects"/>.
        /// </summary>	
        /// <param name="inputRects">An array of input rectangles to be mapped to an output rectangle.</param>	
        /// <param name="inputRectsCount">No documentation.</param>	
        /// <param name="outputRect">No documentation.</param>	
        /// <returns>If the method succeeds, it returns <see cref="SharpDX.Result.Ok"/>. If it fails, it returns an <see cref="SharpDX.Result"/> error code.</returns>	
        /// <remarks>	
        /// The transform implementation must ensure that any pixel shader or software callback implementation it provides honors this calculation.The transform implementation must regard this method as purely functional. It can base the mapped input and output rectangles on its current state as specified by the encapsulating effect properties. However, it must not change its own state in response to this method being invoked. The Direct2D renderer implementation reserves the right to call this method at any time and in any sequence.	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1Transform::MapInputRectsToOutputRect']/*"/>	
        /// <unmanaged>HRESULT ID2D1Transform::MapInputRectsToOutputRect([In, Buffer] const RECT* inputRects,[In] unsigned int inputRectsCount,[Out] RECT* outputRect)</unmanaged>	
        SharpDX.Rectangle MapInputRectanglesToOutputRectangle(SharpDX.Rectangle[] inputRects);
    }
}
#endif