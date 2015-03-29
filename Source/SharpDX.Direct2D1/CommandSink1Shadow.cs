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

namespace SharpDX.Direct2D1
{
    internal class CommandSink1Shadow : SharpDX.ComObjectShadow
    {
        private unsafe static readonly CommandSink1Vtbl Vtbl = new CommandSink1Vtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(CommandSink1 callback)
        {
            return ToCallbackPtr<CommandSink1>(callback);
        }

        public class CommandSink1Vtbl : CommandSinkShadow.CommandSinkVtbl
        {
            public CommandSink1Vtbl()
                : base(1)
            {
                AddMethod(new SetPrimitiveBlend1Delegate(SetPrimitiveBlend1Impl));
            }

            /// <summary>	
            /// Sets the blending for primitives.
            /// </summary>	
            /// <unmanaged>HRESULT ID2D1CommandSink1::SetPrimitiveBlend1([In] D2D1_PRIMITIVE_BLEND primitiveBlend)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetPrimitiveBlend1Delegate(IntPtr thisPtr, SharpDX.Direct2D1.PrimitiveBlend primitiveBlend);
            private unsafe static int SetPrimitiveBlend1Impl(IntPtr thisPtr, SharpDX.Direct2D1.PrimitiveBlend primitiveBlend)
            {
                try
                {
                    var shadow = ToShadow<CommandSink1Shadow>(thisPtr);
                    var callback = (CommandSink1)shadow.Callback;
                    callback.PrimitiveBlend1 = primitiveBlend;
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