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
using System.Runtime.InteropServices;
using System.Reflection;

namespace SharpDX
{
    /// <summary>
    /// A COM Interface Callback
    /// </summary>
    internal abstract class ComObjectShadow : CppObjectShadow
    {
        private int count = 1;
        private static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

#if WIN8
        HashSet<Guid> supportedGuids = new HashSet<Guid>();
#else
        List<Guid> supportedGuids = new List<Guid>();
#endif

        private void FindAllGuids(Type type)
        {
            if (type == null || type == typeof(object))
                return;

            var guid = Utilities.GetGuidFromType(type);
            if (!supportedGuids.Contains(guid))
            {
                supportedGuids.Add(guid);

#if WIN8
                var typeInfo = type.GetTypeInfo();
                foreach (var baseInterface in typeInfo.ImplementedInterfaces)
                {
                    FindAllGuids(baseInterface);
                }

#else
                var typeInfo = type;
                foreach (var baseInterface in type.GetInterfaces())
                {
                    FindAllGuids(baseInterface);
                }

#endif
                FindAllGuids(typeInfo.BaseType);
            }
        }

        protected virtual int QueryInterfaceImpl(IntPtr thisObject, ref Guid guid, out IntPtr output)
        {
            lock (supportedGuids)
            {
                if (supportedGuids.Count == 0)
                    FindAllGuids(Callback.GetType());

                // Check guid from all inherited interfaces/types
                if (supportedGuids.Contains(guid))
                {
                    AddRefImpl(thisObject);
                    output = thisObject;
                    return Result.Ok.Code;
                }
            }

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
            // TODO: Add interlocked
            count++;
            return count;
        }

        protected virtual int ReleaseImpl(IntPtr thisObject)
        {
            // TODO: Add interlocked
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
            public delegate int QueryInterfaceDelegate(IntPtr thisObject, IntPtr guid, out IntPtr output);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate int AddRefDelegate(IntPtr thisObject);

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate int ReleaseDelegate(IntPtr thisObject);

            protected static int QueryInterfaceImpl(IntPtr thisObject, IntPtr guid, out IntPtr output)
            {
                var shadow = ToShadow<ComObjectShadow>(thisObject);
                unsafe
                {
                    return shadow.QueryInterfaceImpl(thisObject, ref *((Guid*)guid), out output);
                }
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