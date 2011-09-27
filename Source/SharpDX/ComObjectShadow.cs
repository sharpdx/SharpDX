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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// A COM Interface Callback
    /// </summary>
    internal abstract class ComObjectShadow : CppObjectShadow
    {
        private int count = 1;
        private static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

        protected virtual int QueryInterfaceImpl(IntPtr thisObject, ref Guid guid, out IntPtr output)
        {
            if (guid == Utilities.GetGuidFromType(GetType()))
            {
                AddRefImpl(thisObject);
                output = thisObject;
                return Result.Ok.Code;
            }

            if (guid == Utilities.GetGuidFromType(Callback.GetType()))
            {
                AddRefImpl(thisObject);
                output = thisObject;
                return Result.Ok.Code;
            }

            // TODO add resolving GUID from inherited interface?
            if (guid == IID_IUnknown)
            {
                AddRefImpl(thisObject);
                output = thisObject;
                return Result.Ok.Code;
            }
            output = IntPtr.Zero;
            return Result.NoInterface.Code;
        }

        protected virtual int AddRefImpl(IntPtr thisObject)
        {
            count++;
            return count;
        }

        protected virtual int ReleaseImpl(IntPtr thisObject)
        {
            count--;
            return count;
        }

        internal abstract class ComObjectVtbl : CppObjectVtbl
        {
            protected ComObjectVtbl(int numberOfCallbackMethods)
                : base(numberOfCallbackMethods + 3)
            {
                unsafe
                {
                    AddMethod(new QueryInterfaceDelegate(QueryInterfaceImpl));
                    AddMethod(new AddRefDelegate(AddRefImpl));
                    AddMethod(new ReleaseDelegate(ReleaseImpl));
                }
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public unsafe delegate int QueryInterfaceDelegate(IntPtr thisObject, Guid* guid, out IntPtr output);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate int AddRefDelegate(IntPtr thisObject);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate int ReleaseDelegate(IntPtr thisObject);

            protected unsafe static int QueryInterfaceImpl(IntPtr thisObject, Guid* guid, out IntPtr output)
            {
                var shadow = ToShadow<ComObjectShadow>(thisObject);
                return shadow.QueryInterfaceImpl(thisObject, ref *guid, out output);
            }

            protected static int AddRefImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<ComObjectShadow>(thisObject);
                return shadow.AddRefImpl(thisObject);
            }

            protected static int ReleaseImpl(IntPtr thisObject)
            {
                var shadow = ToShadow<ComObjectShadow>(thisObject);
                return shadow.ReleaseImpl(thisObject);
            }
        }
    }
}