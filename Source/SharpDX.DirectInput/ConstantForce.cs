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
    /// This class describes a Constant force effect. 
    /// It is passed in the <see cref="EffectParameters.Parameters"/> of the <see cref="EffectParameters"/> structure.
    /// </summary>
    public class ConstantForce : TypeSpecificParameters
    {
        /// <summary>
        /// Gets or sets the magnitude.
        /// The magnitude of the effect, in the range from - 10,000 through 10,000. If an envelope is applied to this effect, the value represents the magnitude of the sustain. If no envelope is applied, the value represents the amplitude of the entire effect.
        /// </summary>
        /// <value>The magnitude.</value>
        public int Magnitude { get; set; }


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
                if (bufferSize != sizeof(RawConstantForce))
                    return null;

                Magnitude = ((RawConstantForce*) bufferPointer)->Magnitude;
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
                ((RawConstantForce*) pData)->Magnitude = Magnitude;
                return pData;
            }
        }

        /// <summary>
        /// Gets the size of this specific parameter.
        /// </summary>
        /// <value>The size.</value>
        public override int Size
        {
            get { return Utilities.SizeOf<RawConstantForce>(); }
        }
    }
}