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

namespace SharpDX.Direct3D11
{
    public partial class ComputeShaderStage
    {
        /// <summary>	
        /// Gets an array of views for an unordered resource.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call IUnknown::Release on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to return (ranges from 0 to D3D11_PS_CS_UAV_REGISTER_COUNT - 1). </param>
        /// <param name="count">Number of views to get (ranges from 0 to D3D11_PS_CS_UAV_REGISTER_COUNT - StartSlot). </param>
        /// <unmanaged>void CSGetUnorderedAccessViews([In] int StartSlot,[In] int NumUAVs,[Out, Buffer] ID3D11UnorderedAccessView** ppUnorderedAccessViews)</unmanaged>
        public UnorderedAccessView[] GetUnorderedAccessViews(int startSlot, int count)
        {
            var temp = new UnorderedAccessView[count];
            GetUnorderedAccessViews(startSlot, count, temp);
            return temp;
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource.	
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessView">A reference to an <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <unmanaged>void CSSetUnorderedAccessViews([In] int StartSlot,[In] int NumUAVs,[In, Buffer] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer] const int* pUAVInitialCounts)</unmanaged>
        public void SetUnorderedAccessView(int startSlot, SharpDX.Direct3D11.UnorderedAccessView unorderedAccessView)
        {
            SetUnorderedAccessView(startSlot, unorderedAccessView, -1);
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource.	
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessView">A reference to an <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <param name="uavInitialCount">An Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept.   Any other values set the hidden counter for that Appendable/Consumable UAV. uAVInitialCount is only relevant for UAVs which have the <see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags"/> flag,  otherwise the argument is ignored. </param>
        /// <unmanaged>void CSSetUnorderedAccessViews([In] int StartSlot,[In] int NumUAVs,[In, Buffer] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer] const int* pUAVInitialCounts)</unmanaged>
        public void SetUnorderedAccessView(int startSlot, SharpDX.Direct3D11.UnorderedAccessView unorderedAccessView, int uavInitialCount)
        {
            SetUnorderedAccessViews(startSlot, new[] { unorderedAccessView }, new[] { uavInitialCount });
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource.	
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessViews">A reference to an array of <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <unmanaged>void CSSetUnorderedAccessViews([In] int StartSlot,[In] int NumUAVs,[In, Buffer] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer] const int* pUAVInitialCounts)</unmanaged>
        public void SetUnorderedAccessViews(int startSlot, params SharpDX.Direct3D11.UnorderedAccessView[] unorderedAccessViews)
        {
            var uavInitialCounts = new int[unorderedAccessViews.Length];
            for (int i = 0; i < unorderedAccessViews.Length; i++)
                uavInitialCounts[i] = -1;
            SetUnorderedAccessViews(startSlot, unorderedAccessViews, uavInitialCounts);
        }

        /// <summary>	
        /// Sets an array of views for an unordered resource.	
        /// </summary>	
        /// <remarks>	
        /// </remarks>	
        /// <param name="startSlot">Index of the first element in the zero-based array to begin setting. </param>
        /// <param name="unorderedAccessViews">A reference to an array of <see cref="SharpDX.Direct3D11.UnorderedAccessView"/> references to be set by the method. </param>
        /// <param name="uavInitialCounts">An array of Append/Consume buffer offsets. A value of -1 indicates the current offset should be kept.   Any other values set the hidden counter for that Appendable/Consumable UAV.  pUAVInitialCounts is only relevant for UAVs which have the <see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags"/> flag,  otherwise the argument is ignored. </param>
        /// <unmanaged>void CSSetUnorderedAccessViews([In] int StartSlot,[In] int NumUAVs,[In, Buffer] const ID3D11UnorderedAccessView** ppUnorderedAccessViews,[In, Buffer] const int* pUAVInitialCounts)</unmanaged>
        public unsafe void SetUnorderedAccessViews(int startSlot, SharpDX.Direct3D11.UnorderedAccessView[] unorderedAccessViews, int[] uavInitialCounts)
        {
            var unorderedAccessViewsOut_ = (IntPtr*)0;
            if (unorderedAccessViews != null)
            {
                IntPtr* unorderedAccessViewsOut__ = stackalloc IntPtr[unorderedAccessViews.Length];
                unorderedAccessViewsOut_ = unorderedAccessViewsOut__;
                for (int i = 0; i < unorderedAccessViews.Length; i++)
                    unorderedAccessViewsOut_[i] = (unorderedAccessViews[i] == null) ? IntPtr.Zero : unorderedAccessViews[i].NativePointer;
            }
            fixed (void* puav = uavInitialCounts)
                SetUnorderedAccessViews(startSlot, unorderedAccessViews != null ? unorderedAccessViews.Length : 0, (IntPtr)unorderedAccessViewsOut_, (IntPtr)puav);
        }
    }
}