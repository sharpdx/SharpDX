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
    /// Custom data stored in <see cref="EffectParameters"/>.
    /// </summary>
    public class TypeSpecificParameters
    {
        private int _bufferSize;
        private byte[] _buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSpecificParameters"/> class.
        /// </summary>
        protected TypeSpecificParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSpecificParameters"/> class.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="bufferPointer">The buffer pointer.</param>
        internal TypeSpecificParameters(int bufferSize, IntPtr bufferPointer)
        {
            Init(bufferSize, bufferPointer);
        }

        /// <summary>
        /// Initializes this instance from the specified buffer.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="bufferPointer">The buffer pointer.</param>
        private void Init(int bufferSize, IntPtr bufferPointer)
        {
            this._bufferSize = bufferSize;

            // By default, copy as-is previous data
            if (_bufferSize > 0 && bufferPointer != IntPtr.Zero)
            {
                this._buffer = new byte[bufferSize];
                Utilities.Read(bufferPointer, _buffer, 0, _bufferSize);
            }            
        }

        /// <summary>
        /// Marshal this class from an unmanaged buffer.
        /// </summary>
        /// <param name="bufferSize">The size of the unmanaged buffer.</param>
        /// <param name="bufferPointer">The pointer to the unmanaged buffer.</param>
        /// <returns>An instance of TypeSpecificParameters or null</returns>
        protected virtual TypeSpecificParameters MarshalFrom(int bufferSize, IntPtr bufferPointer)
        {
            Init(bufferSize, bufferPointer);
            return this;
        }

        /// <summary>
        /// Free a previously allocated buffer.
        /// </summary>
        /// <param name="bufferPointer">The buffer pointer.</param>
        internal virtual void MarshalFree(IntPtr bufferPointer)
        {
            if (bufferPointer != IntPtr.Zero)
                Marshal.FreeHGlobal(bufferPointer);
        }

        /// <summary>
        /// Marshals this class to its native/unmanaged counterpart.
        /// </summary>
        /// <returns>A pointer to an allocated buffer containing the unmanaged structure.</returns>
        internal virtual IntPtr MarshalTo()
        {
            // By default, copy as-is previous data
            IntPtr copyData = IntPtr.Zero;
            if (_bufferSize > 0 && _buffer != null)
            {
                copyData = Marshal.AllocHGlobal(_bufferSize);
                Utilities.Write(copyData, _buffer, 0, _bufferSize);
            }
            
            return copyData;
        }

        /// <summary>
        /// Convert this instance to another typed instance: <see cref="ConditionSet"/>, <see cref="ConstantForce"/>, <see cref="CustomForce"/>, <see cref="PeriodicForce"/> <see cref="RampForce"/>.
        /// </summary>
        /// <typeparam name="T">A class <see cref="TypeSpecificParameters"/></typeparam>
        /// <returns>An instance of the T class</returns>
        public T As<T>() where T : TypeSpecificParameters, new()
        {
            // If As of same type, than return this
            if (this.GetType() == typeof(T))
                return (T)this;

            // If AsOf from base class, than return subclass
            if (this.GetType() == typeof(TypeSpecificParameters))
            {
                unsafe
                {
                    fixed (void* pBuffer = _buffer)
                        return (T) new T().MarshalFrom(_bufferSize, (IntPtr)pBuffer);
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the size of this specific parameter.
        /// </summary>
        /// <value>The size.</value>
        public virtual int Size { get { return _bufferSize; } }
    }
}