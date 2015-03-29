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
//using System.Runtime.InteropServices.WindowsRuntime; // TODO: TO fix

namespace SharpDX
{
    /// <summary>
    /// Internal IInspectable Callback
    /// </summary>
    internal class InspectableShadow : ComObjectShadow
    {
        private static readonly InspectableProviderVtbl Vtbl = new InspectableProviderVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(IInspectable callback)
        {
            return ToCallbackPtr<IInspectable>(callback);
        }

        public class InspectableProviderVtbl : ComObjectVtbl
        {
            public InspectableProviderVtbl()
                : base(3)
            {
                unsafe
                {
                    AddMethod(new GetIidsDelegate(GetIids));
                    AddMethod(new GetRuntimeClassNameDelegate(GetRuntimeClassName));
                    AddMethod(new GetTrustLevelDelegate(GetTrustLevel));
                }
            }

            //        virtual HRESULT STDMETHODCALLTYPE GetIids( 
            ///* [out] */ __RPC__out ULONG *iidCount,
            ///* [size_is][size_is][out] */ __RPC__deref_out_ecount_full_opt(*iidCount) IID **iids) = 0;

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int GetIidsDelegate(IntPtr thisPtr, int* iidCount, IntPtr* iids);
            private unsafe static int GetIids(IntPtr thisPtr, int* iidCount, IntPtr* iids)
            {
                try
                {
                    var shadow = ToShadow<InspectableShadow>(thisPtr);
                    var callback = (IInspectable)shadow.Callback;

                    var container = (ShadowContainer) callback.Shadow;

                    int countGuids = container.Guids.Length;

                    // Copy GUIDs deduced from Callback
                    iids = (IntPtr*)Marshal.AllocCoTaskMem(IntPtr.Size * countGuids);
                    *iidCount = countGuids;

                    for (int i = 0; i < countGuids; i++)
                        iids[i] = container.Guids[i];
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            //virtual HRESULT STDMETHODCALLTYPE GetRuntimeClassName( 
            //    /* [out] */ __RPC__deref_out_opt HSTRING *className) = 0;

            /// <unmanaged>HRESULT ID2D1InspectableProvider::SetComputeInfo([In] ID2D1ComputeInfo* computeInfo)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetRuntimeClassNameDelegate(IntPtr thisPtr, IntPtr className);
            private static int GetRuntimeClassName(IntPtr thisPtr, IntPtr className)
            {
                try
                {
                    var shadow = ToShadow<InspectableShadow>(thisPtr);
                    var callback = (IInspectable)shadow.Callback;
                    // Use the name of the callback class
                    
                    // TODO: TO FIX FOR Windows Runtime
                    //var hstring = WindowsRuntimeMarshal.StringToHString(callback.GetType().FullName);
                    //Marshal.WriteIntPtr(className, hstring);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            //virtual HRESULT STDMETHODCALLTYPE GetTrustLevel( 
            //    /* [out] */ __RPC__out TrustLevel *trustLevel) = 0;
            enum TrustLevel
            {
                BaseTrust = 0,
                PartialTrust = (BaseTrust + 1),
                FullTrust = (PartialTrust + 1)
            };

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetTrustLevelDelegate(IntPtr thisPtr, IntPtr trustLevel);
            private static int GetTrustLevel(IntPtr thisPtr, IntPtr trustLevel)
            {
                try
                {
                    var shadow = ToShadow<InspectableShadow>(thisPtr);
                    var callback = (IInspectable)shadow.Callback;
                    // Write full trust
                    Marshal.WriteInt32(trustLevel, (int)TrustLevel.FullTrust);
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