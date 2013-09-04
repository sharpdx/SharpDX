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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Represents a collection of effects associated with a model.
    /// </summary>
    public sealed class ModelEffectCollection : ReadOnlyCollection<Effect>
    {
        private List<Effect> wrappedList;

        internal ModelEffectCollection()
            : base(new List<Effect>())
        {
            this.wrappedList = (List<Effect>)base.Items;
        }

        internal void Add(Effect effect)
        {
            base.Items.Add(effect);
        }

        /// <summary>
        /// Returns a ModelEffectCollection.Enumerator that can iterate through a ModelEffectCollection.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public new Enumerator GetEnumerator()
        {
            return new Enumerator(this.wrappedList);
        }

        internal void Remove(Effect effect)
        {
            base.Items.Remove(effect);
        }

        /// <summary>
        /// Provides the ability to iterate through the bones in an ModelEffectCollection.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator<Effect>, IDisposable, IEnumerator
        {
            private List<Effect>.Enumerator internalEnumerator;
            internal Enumerator(List<Effect> wrappedList)
            {
                this.internalEnumerator = wrappedList.GetEnumerator();
            }

            /// <summary>Gets the current element in the ModelEffectCollection.</summary>
            public Effect Current
            {
                get
                {
                    return this.internalEnumerator.Current;
                }
            }
            /// <summary>Advances the enumerator to the next element of the ModelEffectCollection.</summary>
            public bool MoveNext()
            {
                return this.internalEnumerator.MoveNext();
            }

            /// <summary>Sets the enumerator to its initial position, which is before the first element in the ModelEffectCollection.</summary>
            void IEnumerator.Reset()
            {
                IEnumerator internalEnumerator = this.internalEnumerator;
                internalEnumerator.Reset();
                this.internalEnumerator = (List<Effect>.Enumerator)internalEnumerator;
            }

            /// <summary>Immediately releases the unmanaged resources used by this object.</summary>
            public void Dispose()
            {
                this.internalEnumerator.Dispose();
            }

            /// <summary>Gets the current element in the ModelEffectCollection as a Object.</summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
        }
    }
}