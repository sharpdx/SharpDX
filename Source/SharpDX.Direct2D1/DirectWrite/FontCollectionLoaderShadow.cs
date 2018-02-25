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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal FontCollectionLoader Callback
    /// </summary>
    internal class FontCollectionLoaderShadow : SharpDX.ComObjectShadow
    {
        private static readonly FontCollectionLoaderVtbl Vtbl = new FontCollectionLoaderVtbl();

        private Factory _factory;

        public static IntPtr ToIntPtr(FontCollectionLoader loader)
        {
            return ToCallbackPtr<FontCollectionLoader>(loader);
        }
        
        public static void SetFactory(FontCollectionLoader loader, Factory factory)
        {
            var shadowPtr = ToIntPtr(loader);
            var shadow = ToShadow<FontCollectionLoaderShadow>(shadowPtr);
            shadow._factory = factory;
        }

        private class FontCollectionLoaderVtbl : ComObjectVtbl
        {
            public FontCollectionLoaderVtbl() : base(1)
            {
                AddMethod(new CreateEnumeratorFromKeyDelegate(CreateEnumeratorFromKeyImpl));
            }


            /// <unmanaged>HRESULT IDWriteFontCollectionLoader::CreateEnumeratorFromKey([None] IDWriteFactory* factory,[In, Buffer] const void* collectionKey,[None] int collectionKeySize,[Out] IDWriteFontFileEnumerator** fontFileEnumerator)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CreateEnumeratorFromKeyDelegate(IntPtr thisPtr, IntPtr factory, IntPtr collectionKey, int collectionKeySize, out IntPtr fontFileEnumerator);

            private static int CreateEnumeratorFromKeyImpl(IntPtr thisPtr, IntPtr factory, IntPtr collectionKey, int collectionKeySize, out IntPtr fontFileEnumerator)
            {
                fontFileEnumerator = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<FontCollectionLoaderShadow>(thisPtr);
                    var callback = (FontCollectionLoader)shadow.Callback;
                    Debug.Assert(factory == shadow._factory.NativePointer);
                    var enumerator = callback.CreateEnumeratorFromKey(shadow._factory, new DataPointer(collectionKey, collectionKeySize));
                    fontFileEnumerator = FontFileEnumeratorShadow.ToIntPtr(enumerator);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}