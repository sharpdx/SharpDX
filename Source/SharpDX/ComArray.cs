// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Collections;
using System.Collections.Generic;

namespace SharpDX
{
    /// <summary>A fast method to pass array of <see cref="ComObject" /> to SharpDX methods.</summary>
    public class ComArray : DisposeBase, IEnumerable
    {
        /// <summary>The values.</summary>
        protected ComObject[] values;

        /// <summary>The native buffer.</summary>
        private IntPtr nativeBuffer;

        /// <summary>Initializes a new instance of the <see cref="ComArray" /> class.</summary>
        /// <param name="array">The array.</param>
        public ComArray(params ComObject[] array)
        {
            values = array;
            nativeBuffer = IntPtr.Zero;
            if (values != null)
            {
                int length = array.Length;
                values = new ComObject[length];
                nativeBuffer = Utilities.AllocateMemory(length * Utilities.SizeOf<IntPtr>());
                for(int i = 0; i < length; i++)
                    Set(i, array[i]);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ComArray" /> class.</summary>
        /// <param name="size">The size.</param>
        public ComArray(int size)
        {
            values = new ComObject[size];
            nativeBuffer = Utilities.AllocateMemory(size * Utilities.SizeOf<IntPtr>());
        }

        /// <summary>Gets the pointer to the native array associated to this instance.</summary>
        /// <value>The native pointer.</value>
        public IntPtr NativePointer
        {
            get
            {
                return nativeBuffer;
            }
        }

        /// <summary>Gets the length.</summary>
        /// <value>The length.</value>
        public int Length
        {
            get
            {
                return values == null ? 0 : values.Length;
            }
        }

        /// <summary>Gets an object at the specified index.</summary>
        /// <param name="index">The index.</param>
        /// <returns>A <see cref="ComObject" /></returns>
        public ComObject Get(int index)
        {
            return values[index];
        }

        /// <summary>Sets from native.</summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        internal void SetFromNative(int index, ComObject value)
        {
            values[index] = value;
            unsafe
            {
                value.NativePointer = ((IntPtr*)nativeBuffer)[index];
            }
        }

        /// <summary>Sets an object at the specified index.</summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        public void Set(int index, ComObject value)
        {
            values[index] = value;
            unsafe
            {
                ((IntPtr*)nativeBuffer)[index] = value.NativePointer;
            }
        }

        /// <summary>Releases unmanaged and - optionally - managed resources</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                values = null;
            }
            Utilities.FreeMemory(nativeBuffer);
            nativeBuffer = IntPtr.Zero;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }

    /// <summary>A typed version of <see cref="ComArray" /></summary>
    /// <typeparam name="T">Type of the <see cref="ComObject" /></typeparam>
    public class ComArray<T> : ComArray, IEnumerable<T> where T : ComObject
    {
        /// <summary>Initializes a new instance of the <see cref="ComArray&lt;T&gt;" /> class.</summary>
        /// <param name="array">The array.</param>
        public ComArray(params ComObject[] array) : base(array)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ComArray&lt;T&gt;" /> class.</summary>
        /// <param name="size">The size.</param>
        public ComArray(int size) : base(size)
        {
        }

        /// <summary>Gets or sets the type T with the specified i.</summary>
        /// <param name="i">The attribute.</param>
        /// <returns>The type T from index i.</returns>
        public T this[int i]
        {
            get
            {
                return (T)Get(i);
            }
            set
            {
                Set(i, value);
            }
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public new IEnumerator<T> GetEnumerator()
        {
            return new ArrayEnumerator<T>(values.GetEnumerator());
        }

        /// <summary>The array enumerator struct.</summary>
        /// <typeparam name="T1">The type of the t1.</typeparam>
        private struct ArrayEnumerator<T1> : IEnumerator<T1> where T1 : ComObject
        {
            /// <summary>The enumerator.</summary>
            private readonly IEnumerator enumerator;

            /// <summary>Initializes a new instance of the ArrayEnumerator struct.</summary>
            /// <param name="enumerator">The enumerator.</param>
            public ArrayEnumerator(IEnumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
            }

            /// <summary>Advances the enumerator to the next element of the collection.</summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
            public void Reset()
            {
                enumerator.Reset();
            }

            /// <summary>Gets the current.</summary>
            /// <value>The current.</value>
            public T1 Current
            {
                get
                {
                    return (T1)enumerator.Current;
                }
            }

            /// <summary>Gets the current.</summary>
            /// <value>The current.</value>
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }
        }
    }
}