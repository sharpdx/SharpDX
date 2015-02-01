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
#if WP8
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D11
{
    [ShadowAttribute(typeof(DrawingSurfaceBackgroundContentProviderShadow))]
    public partial interface IDrawingSurfaceBackgroundContentProviderNative
    {
        // IDrawingSurfaceRuntimeHostNative *
        // ID3D11Device1 *hostDevice
        void Connect(DrawingSurfaceRuntimeHost host, Device device);

        void Disconnect();

        // _In_  const LARGE_INTEGER *presentTargetTime,
        // DrawingSurfaceSizeF *desiredRenderTargetSize
        void PrepareResources(DateTime presentTargetTime, ref Size2F desiredRenderTargetSize);

        //_In_  ID3D11Device1 *hostDevice,
        //_In_  ID3D11DeviceContext1 *hostDeviceContext,
        //_In_  ID3D11RenderTargetView *hostRenderTargetView) = 0;
        void Draw(Device device, DeviceContext context, RenderTargetView renderTargetView);
    }

    public abstract class DrawingSurfaceBackgroundContentProviderNativeBase : CallbackBase, IDrawingSurfaceBackgroundContentProviderNative, IInspectable, ICustomQueryInterface
    {
        private IntPtr thisComObjectPtr;

        protected DrawingSurfaceBackgroundContentProviderNativeBase()
        {
            thisComObjectPtr = CppObject.ToCallbackPtr<IDrawingSurfaceBackgroundContentProviderNative>(this);
        }

        public abstract void Connect(DrawingSurfaceRuntimeHost host, Device device);

        public abstract void Disconnect();

        public abstract void PrepareResources(DateTime presentTargetTime, ref Size2F desiredRenderTargetSize);

        public abstract void Draw(Device device, DeviceContext context, RenderTargetView renderTargetView);

        CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
        {
            ppv = thisComObjectPtr;
            return CustomQueryInterfaceResult.Handled;
        }
    }

    /// <summary>
    /// Internal DrawingSurfaceBackgroundContentProvider Callback
    /// </summary>
    internal class DrawingSurfaceBackgroundContentProviderShadow : ComObjectShadow
    {
        private static readonly DrawingSurfaceBackgroundContentProviderVtbl Vtbl = new DrawingSurfaceBackgroundContentProviderVtbl();

        private DrawingSurfaceRuntimeHost host;
        private Device device;
        private DeviceContext context;
        private RenderTargetView[] renderTargetViewCache = new RenderTargetView[3];
        int renderTargetViewCacheIdx = 0;


        private void ClearPointers()
        {
            CleanPointer(ref host);
            CleanPointer(ref device);
            CleanPointer(ref context);
            for (int i = 0; i < renderTargetViewCache.Length; i++)
                CleanPointer(ref renderTargetViewCache[i]);
        }

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(IDrawingSurfaceBackgroundContentProviderNative callback)
        {
            return ToCallbackPtr<IDrawingSurfaceBackgroundContentProviderNative>(callback);
        }

        public class DrawingSurfaceBackgroundContentProviderVtbl : ComObjectVtbl
        {
            public DrawingSurfaceBackgroundContentProviderVtbl()
                : base(4)
            {
                AddMethod(new ConnectDelegate(Connect));
                AddMethod(new DisconnectDelegate(Disconnect));
                AddMethod(new PrepareResourcesDelegate(PrepareResources));
                AddMethod(new DrawDelegate(Draw));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int ConnectDelegate(IntPtr thisPtr, IntPtr host, IntPtr hostDevice);
            private static int Connect(IntPtr thisPtr, IntPtr hostPtr, IntPtr hostDevicePtr)
            {
                try
                {
                    var shadow = ToShadow<DrawingSurfaceBackgroundContentProviderShadow>(thisPtr);
                    var callback = (IDrawingSurfaceBackgroundContentProviderNative)shadow.Callback;

                    // Precache values.
                    shadow.ClearPointers();
                    shadow.host = new DrawingSurfaceRuntimeHost(hostPtr);
                    shadow.device = new Device(hostDevicePtr);

                    callback.Connect(shadow.host, shadow.device);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1DrawingSurfaceBackgroundContentProvider::SetComputeInfo([In] ID2D1ComputeInfo* computeInfo)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void DisconnectDelegate(IntPtr thisPtr);
            private static void Disconnect(IntPtr thisPtr)
            {
                var shadow = ToShadow<DrawingSurfaceBackgroundContentProviderShadow>(thisPtr);
                var callback = (IDrawingSurfaceBackgroundContentProviderNative)shadow.Callback;
                callback.Disconnect();
                shadow.ClearPointers();
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int PrepareResourcesDelegate(IntPtr thisPtr, IntPtr presentTargetTime, IntPtr desiredRenderTargetSize);
            private unsafe static int PrepareResources(IntPtr thisPtr, IntPtr presentTargetTime, IntPtr desiredRenderTargetSize)
            {
                try
                {
                    var shadow = ToShadow<DrawingSurfaceBackgroundContentProviderShadow>(thisPtr);
                    var callback = (IDrawingSurfaceBackgroundContentProviderNative)shadow.Callback;
                    callback.PrepareResources(new DateTime(*(long*)presentTargetTime), ref *(Size2F*)desiredRenderTargetSize);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawDelegate(IntPtr thisPtr, IntPtr hostDevice, IntPtr hostDeviceContext, IntPtr hostRenderTargetView);
            private static int Draw(IntPtr thisPtr, IntPtr hostDevice, IntPtr hostDeviceContext, IntPtr hostRenderTargetView)
            {
                try
                {
                    var shadow = ToShadow<DrawingSurfaceBackgroundContentProviderShadow>(thisPtr);
                    var callback = (IDrawingSurfaceBackgroundContentProviderNative)shadow.Callback;

                    if (hostDevice == IntPtr.Zero || hostDeviceContext == IntPtr.Zero || hostRenderTargetView == IntPtr.Zero)
                        throw new ArgumentException();

                    if(shadow.device == null || shadow.device.NativePointer != hostDevice)
                    {
                        CleanPointer(ref shadow.device);
                        shadow.device = new Device(hostDevice);
                    }

                    if(shadow.context == null || shadow.context.NativePointer != hostDeviceContext)
                    {
                        CleanPointer(ref shadow.context);
                        shadow.context = new DeviceContext(hostDeviceContext);
                    }

                    var cache = shadow.renderTargetViewCache;
                    RenderTargetView renderTargetView = null;
                    // circle through the queue and search for cached hostRenderTargetView
                    for (int i = 0; i < cache.Length; i++)
                    {
                        renderTargetView = cache[shadow.renderTargetViewCacheIdx];
                        shadow.renderTargetViewCacheIdx = (++shadow.renderTargetViewCacheIdx) % 3;
                        if (renderTargetView!=null && renderTargetView.NativePointer == hostRenderTargetView) break;
                        renderTargetView = null;
                    }
                    if (renderTargetView == null)
                    {
                        CleanPointer(ref cache[shadow.renderTargetViewCacheIdx]);
                        renderTargetView = new RenderTargetView(hostRenderTargetView);
                        cache[shadow.renderTargetViewCacheIdx] = renderTargetView;
                        shadow.renderTargetViewCacheIdx = (++shadow.renderTargetViewCacheIdx) % 3;
                    }

                    callback.Draw(shadow.device, shadow.context, renderTargetView);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

        }

        private static void CleanPointer<T>(ref T comObject) where T : ComObject
        {
            if (comObject != null)
            {
                comObject.NativePointer = IntPtr.Zero;
            }
            comObject = null;
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}
#endif