// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Collections.Generic;

namespace SharpDX
{
    internal class CallbackContainer : DisposeBase
    {
        private struct Entry
        {
            public Entry(Type type, ComObjectCallbackNative nativeCallback)
            {
                Type = type;
                NativeCallback = nativeCallback;
            }

            public Type Type;
            public ComObjectCallbackNative NativeCallback;
        }

        private readonly List<Entry> _entries;

        public CallbackContainer()
        {
            _entries = new List<Entry>(5);
        }

        internal ComObjectCallbackNative Find(Type type)
        {
            for (int i = 0; i < _entries.Count; i++)
                if (_entries[i].Type == type)
                    return _entries[i].NativeCallback;
            return null;
        }

        internal void Add(Type type, ComObjectCallbackNative callbackNative)
        {
            _entries.Add(new Entry(type, callbackNative));
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var comObjectCallbackNative in _entries)
                    comObjectCallbackNative.NativeCallback.Dispose();
                _entries.Clear();
            }
        }
    }
}