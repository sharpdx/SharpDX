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
    /// This class describes a set of <see cref="Condition"/> effect.
    /// It is passed in the <see cref="EffectParameters.Parameters"/> of the <see cref="EffectParameters"/> structure.
    /// </summary>
    public class ConditionSet : TypeSpecificParameters
    {
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
                // BufferSize must be a multiple of the size of Condition structure
                if (bufferSize <= 0 || (bufferSize % sizeof(Condition)) != 0)
                    return null;

                // Allocate a set of Condition and marshal from unmanaged memory
                int numberOfConditions = bufferSize/sizeof (Condition);
                Conditions = new Condition[numberOfConditions];
                Utilities.Read(bufferPointer, Conditions, 0, Conditions.Length);
                return this;
            }
        }

        /// <summary>
        /// Marshals this class to its native/unmanaged counterpart.
        /// </summary>
        /// <returns>A pointer to an allocated buffer containing the unmanaged structure.</returns>
        internal override IntPtr MarshalTo()
        {
            if (Size == 0)
                return IntPtr.Zero;

            var pData = Marshal.AllocHGlobal(Size);
            Utilities.Write(pData, Conditions, 0, Conditions.Length);
            return pData;
        }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>The conditions.</value>
        public Condition[] Conditions { get; set; }

        /// <summary>
        /// Gets the size of this specific parameter.
        /// </summary>
        /// <value>The size.</value>
        public override int Size
        {
            get
            {
                unsafe
                {
                    return (Conditions != null) ? Conditions.Length*sizeof (Condition) : 0;
                }
            }
        }
    }
}