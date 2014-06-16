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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// This class describes a Custom force effect. 
    /// It is passed in the <see cref="EffectParameters.Parameters"/> of the <see cref="EffectParameters"/> structure.
    /// </summary>
    public class CustomForce : TypeSpecificParameters
    {
        /// <summary>
        /// Gets or sets the number of channels (axes) affected by this force. 
        /// The first channel is applied to the first axis associated with the effect, the second to the second, and so on. If there are fewer channels than axes, nothing is associated with the extra axes.
        /// If there is only a single channel, the effect is rotated in the direction specified by the <see cref="EffectParameters.Directions"/> of the <see cref="EffectParameters"/> structure. If there is more than one channel, rotation is not allowed.
        /// Not all devices support rotation of custom effects. 
        /// </summary>
        /// <value>The channel count.</value>
        public int ChannelCount { get; set; }

        /// <summary>
        /// Gets or sets the sample period, in microseconds.
        /// </summary>
        /// <value>The sample period.</value>
        public int SamplePeriod{ get; set; }

        /// <summary>
        /// Gets or sets the total number of samples in the <see cref="ForceData"/>. It must be an integral multiple of the <see cref="ChannelCount"/>. 
        /// </summary>
        /// <value>The sample count.</value>
        public int SampleCount { get; set; }

        /// <summary>
        /// Gets or sets  an array of force values representing the custom force. If multiple channels are provided, the values are interleaved. For example, if <see cref="ChannelCount"/> is 3, the first element of the array belongs to the first channel, the second to the second, and the third to the third. 
        /// </summary>
        /// <value>The force data.</value>
        public int[] ForceData { get; set; }

        /// <summary>
        /// Marshal this class from an unmanaged buffer.
        /// </summary>
        /// <param name="bufferSize">The size of the unmanaged buffer.</param>
        /// <param name="bufferPointer">The pointer to the unmanaged buffer.</param>
        /// <returns>An instance of TypeSpecificParameters or null</returns>
        protected override TypeSpecificParameters MarshalFrom(int bufferSize, IntPtr bufferPointer)
        {
            unsafe
            {
                if (bufferSize != sizeof(RawCustomForce))
                    return null;

                ChannelCount = ((RawCustomForce*)bufferPointer)->Channels;
                SamplePeriod = ((RawCustomForce*)bufferPointer)->SamplePeriod;
                SampleCount = ((RawCustomForce*)bufferPointer)->Samples;
                ForceData = new int[SampleCount];
                Utilities.Read(((RawCustomForce*) bufferPointer)->ForceDataPointer, ForceData, 0, ForceData.Length);
                return this;
            }
        }

        /// <summary>
        /// Marshals this class to its native/unmanaged counterpart.
        /// </summary>
        /// <returns>A pointer to an allocated buffer containing the unmanaged structure.</returns>
        internal override IntPtr MarshalTo()
        {
            unsafe
            {
                var pData = Marshal.AllocHGlobal(Size);
                ((RawCustomForce*)pData)->Channels = ChannelCount;
                ((RawCustomForce*)pData)->SamplePeriod = SamplePeriod;
                ((RawCustomForce*)pData)->Samples = SampleCount;

                var pForceDatas = Marshal.AllocHGlobal(ForceData.Length * sizeof(int));
                ((RawCustomForce*)pData)->ForceDataPointer = pForceDatas;
                Utilities.Write(pForceDatas, ForceData, 0, ForceData.Length);

                return pData;
            }
        }

        /// <summary>
        /// Free a previously allocated buffer.
        /// </summary>
        /// <param name="bufferPointer">The buffer pointer.</param>
        internal override void MarshalFree(IntPtr bufferPointer)
        {
            base.MarshalFree(bufferPointer);
            if (bufferPointer != IntPtr.Zero)
            {
                unsafe
                {
                    Marshal.FreeHGlobal(((RawCustomForce*) bufferPointer)->ForceDataPointer);
                }
            }
        }

        /// <summary>
        /// Gets the size of this specific parameter.
        /// </summary>
        /// <value>The size.</value>
        public override int Size
        {
            get { return Utilities.SizeOf<RawCustomForce>(); }
        }
    }
}