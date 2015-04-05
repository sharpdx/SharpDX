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
using System.Runtime.InteropServices;

namespace SharpDX.XAPO
{
    /// <summary>
    /// AudioProcessor interface for XAudio27.
    /// </summary>
    [Guid("a90bc001-e897-e897-55e4-9e4700000001")]
    [Shadow(typeof(ParameterProviderShadow))]
    internal interface ParameterProvider27
    {
    }

    [Shadow(typeof(ParameterProviderShadow))]
    internal partial interface ParameterProvider : ParameterProvider27
    {
        /// <summary>	
        /// Sets effect-specific parameters.	
        /// </summary>	
        /// <param name="parameters"> Effect-specific parameter block. </param>
        /// <unmanaged>void IXAPOParameters::SetParameters([In, Buffer] const void* pParameters,[None] UINT32 ParameterByteSize)</unmanaged>
        /* public void SetParameters(IntPtr arametersRef, int parameterByteSize) */
        void SetParameters(DataStream parameters);

        /// <summary>	
        /// Gets the current values for any effect-specific parameters.	
        /// </summary>	
        /// <param name="parameters">[in, out]  Receives an effect-specific parameter block. </param>
        /// <unmanaged>void IXAPOParameters::GetParameters([Out, Buffer] void* pParameters,[None] UINT32 ParameterByteSize)</unmanaged>
        void GetParameters(DataStream parameters);
    }

    internal partial class ParameterProviderNative
    {
        public void SetParameters(DataStream parameters)
        {
            SetParameters_(parameters.PositionPointer, (int)(parameters.Length - parameters.Position));
        }

        public void GetParameters(DataStream parameters)
        {
            GetParameters_(parameters.DataPointer, (int)(parameters.Length - parameters.Position));
        }
    }
}