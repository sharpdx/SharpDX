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
using System.Threading;

namespace SharpDX
{
    /// <summary>
    /// Callback base implementation of <see cref="ICallbackable"/>.
    /// </summary>
    public abstract class CallbackBase : DisposeBase, ICallbackable
    {
        private int refCount = 1;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Release();
            }
        }
        
        public int AddReference()
        {
            var old = refCount;
            while (true)
            {
                if (old == 0)
                {
                    throw new ObjectDisposedException("Cannot add a reference to a nonreferenced item");
                }
                var current = Interlocked.CompareExchange(ref refCount, old + 1, old);
                if (current == old)
                {
                    return old + 1;
                }
                old = current;
            }
        }

        public int Release()
        {
            var old = refCount;
            while (true)
            {
                var current = Interlocked.CompareExchange(ref refCount, old - 1, old);

                if (current == old)
                {
                    if (old == 1)
                    {
                        // Dispose native resources
                        var callback = ((ICallbackable)this);
                        callback.Shadow.Dispose();
                        callback.Shadow = null;
                    }
                    return old - 1;
                }
                old = current;
            }
        }

        public Result QueryInterface(ref Guid guid, out IntPtr comObject)
        {
            var container = (ShadowContainer)((ICallbackable)this).Shadow;
            comObject = container.Find(guid);
            if (comObject == IntPtr.Zero)
            {
                return Result.NoInterface;
            }
            return Result.Ok;
        }

        IDisposable ICallbackable.Shadow { get; set; }
    }
}