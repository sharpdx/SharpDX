// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

namespace WPFHost
{
    public static class Disposer
    {
        /// <summary>
        /// Dispose an object instance and set the reference to null
        /// </summary>
        /// <typeparam name="TypeName">The type of object to dispose</typeparam>
        /// <param name="resource">A reference to the instance for disposal</param>
        /// <remarks>This method hides any thrown exceptions that might occur during disposal of the object (by design)</remarks>
        public static void SafeDispose<TypeName>(ref TypeName resource) where TypeName : class
        {
            if (resource == null)
                return;

            IDisposable disposer = resource as IDisposable;
            if (disposer != null)
            {
                try
                {
                    disposer.Dispose();
                }
                catch
                {
                }
            }

            resource = null;
        }
    }
}
