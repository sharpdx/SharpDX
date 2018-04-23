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
    public partial class StreamOutputStage
    {
        /// <summary>	
        /// <p>Set the target output buffers for the stream-output stage of the pipeline.</p>	
        /// </summary>	
        /// <param name="numBuffers"><dd>  <p>The number of buffer to bind to the device. A maximum of four output buffers can be set. If less than four are defined by the call, the remaining buffer slots are set to <strong><c>null</c></strong>. See Remarks.</p> </dd></param>	
        /// <param name="sOTargetsOut"><dd>  <p>The array of output buffers (see <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>) to bind to the device. The buffers must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.StreamOutput"/></strong> flag.</p> </dd></param>	
        /// <param name="offsetsRef"><dd>  <p>Array of offsets to the output buffers from <em>ppSOTargets</em>, one offset for each buffer. The offset values must be in bytes.</p> </dd></param>	
        /// <remarks>	
        /// <p>An offset of -1 will cause the stream output buffer to be appended, continuing after the last location written to the buffer in a previous stream output pass.</p><p>Calling this method using a buffer that is currently bound for writing will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476484</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::SOSetTargets([In] unsigned int NumBuffers,[In, Buffer, Optional] const ID3D11Buffer** ppSOTargets,[In, Buffer, Optional] const unsigned int* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::SOSetTargets</unmanaged-short>	
        internal void SetTargets(int numBuffers, SharpDX.Direct3D11.Buffer[] sOTargetsOut, int[] offsetsRef)
        {
            unsafe
            {
                var sOTargetsOut_ = (IntPtr*)0;
                if (sOTargetsOut != null)
                {
                    IntPtr* sOTargetsOut__ = stackalloc IntPtr[sOTargetsOut.Length];
                    sOTargetsOut_ = sOTargetsOut__;
                    for (int i = 0; i < sOTargetsOut.Length; i++)
                        sOTargetsOut_[i] = (sOTargetsOut[i] == null) ? IntPtr.Zero : sOTargetsOut[i].NativePointer;
                }
                int[] offsetsRef__ = offsetsRef;
                fixed (void* offsetsRef_ = offsetsRef__)
                    SetTargets(numBuffers, new IntPtr(sOTargetsOut_), new IntPtr(offsetsRef_));
            }
        }

        /// <summary>
        /// Sets the stream output targets bound to the StreamOutput stage.
        /// </summary>
        /// <param name="buffer">The buffer to bind on the first stream output slot.</param>
        /// <param name="offsets">An offset of -1 will cause the stream output buffer to be appended, continuing after the last location written to the buffer in a previous stream output pass.</param>
        /// <msdn-id>ff476484</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::SOSetTargets([In] unsigned int NumBuffers,[In, Buffer, Optional] const ID3D11Buffer** ppSOTargets,[In, Buffer, Optional] const unsigned int* pOffsets)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::SOSetTargets</unmanaged-short>
        public unsafe void SetTarget(Buffer buffer, int offsets)
        {
            var ptr = buffer != null ? buffer.NativePointer : IntPtr.Zero;

            SetTargets(1, new IntPtr(&ptr), new IntPtr(&offsets));
        }

        /// <summary>	
        /// Set the target output {{buffers}} for the {{StreamOutput}} stage, which enables/disables the pipeline to stream-out data.	
        /// </summary>	
        /// <remarks>	
        /// Call ID3D10Device::SOSetTargets (before any draw calls) to stream data out; call SOSetTargets with NULL to stop streaming data out. For an example, see Exercise 01 from the GDC 2007 workshop, which sets the stream output render targets before calling draw methods in the RenderInstanceToStream function. An offset of -1 will cause the stream output buffer to be appended, continuing after the last location written to the buffer in a previous stream output pass. Calling this method using a buffer that is currently bound for writing will effectively bind NULL instead because a buffer cannot be bound as both an input and an output at the same time. The {{DeviceDebug Layer}} will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime. The method will not hold a reference to the interfaces passed in. For that reason, applications should be careful not to release an interface currently in use by the device. 	
        /// </remarks>	
        /// <param name="bufferBindings">an array of output buffers (see <see cref="SharpDX.Direct3D11.StreamOutputBufferBinding"/>) to bind to the device. The buffers must have been created with the <see cref="SharpDX.Direct3D11.BindFlags.StreamOutput"/> flag. </param>
        /// <unmanaged>void SOSetTargets([In] int NumBuffers,[In, Buffer, Optional] const ID3D10Buffer** ppSOTargets,[In, Buffer, Optional] const int* pOffsets)</unmanaged>
        /// <msdn-id>ff476484</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::SOSetTargets([In] unsigned int NumBuffers,[In, Buffer, Optional] const ID3D11Buffer** ppSOTargets,[In, Buffer, Optional] const unsigned int* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::SOSetTargets</unmanaged-short>	
        public void SetTargets(StreamOutputBufferBinding[] bufferBindings)
        {
            if (bufferBindings == null)
            {
                SetTargets(0, null, null);
            }
            else
            {
                var buffers = new Buffer[bufferBindings.Length];
                var offsets = new int[bufferBindings.Length];
                for (int i = 0; i < bufferBindings.Length; i++)
                {
                    buffers[i] = bufferBindings[i].Buffer;
                    offsets[i] = bufferBindings[i].Offset;
                }

                SetTargets(bufferBindings.Length, buffers, offsets);
            }
        }

        /// <summary>	
        /// Get the target output {{buffers}} for the {{StreamOutput}} stage of the pipeline.	
        /// </summary>	
        /// <remarks>	
        /// Any returned interfaces will have their reference count incremented by one. Applications should call {{IUnknown::Release}} on the returned interfaces when they are no longer needed to avoid memory leaks. 	
        /// </remarks>	
        /// <param name="numBuffers">Number of buffers to get. A maximum of four output buffers can be retrieved. </param>
        /// <returns>an array of output buffers (see <see cref="SharpDX.Direct3D11.Buffer"/>) to bind to the device.</returns>
        /// <unmanaged>void SOGetTargets([In] int NumBuffers,[Out, Buffer, Optional] ID3D10Buffer** ppSOTargets,[Out, Buffer, Optional] int* pOffsets)</unmanaged>
        public Buffer[] GetTargets(int numBuffers)
        {
            var buffers = new Buffer[numBuffers];
            GetTargets(numBuffers, buffers);
            return buffers;
        }
    }
}