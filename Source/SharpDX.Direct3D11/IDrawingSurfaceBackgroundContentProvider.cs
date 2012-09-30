



using System;
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
        void PrepareResources(DateTime presentTargetTime, DrawingSizeF desiredRenderTargetSize);

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
            thisComObjectPtr = CppObjectShadow.ToIntPtr<IDrawingSurfaceBackgroundContentProviderNative>(this);
        }

        public abstract void Connect(DrawingSurfaceRuntimeHost host, Device device);

        public abstract void Disconnect();

        public abstract void PrepareResources(DateTime presentTargetTime, DrawingSizeF desiredRenderTargetSize);

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
        private RenderTargetView renderTargetView;

        /// <summary>
        /// Return a pointer to the unamanged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(IDrawingSurfaceBackgroundContentProviderNative callback)
        {
            return ToIntPtr<IDrawingSurfaceBackgroundContentProviderNative>(callback);
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
                    shadow.host = new DrawingSurfaceRuntimeHost(hostPtr);
                    shadow.device = new Device(hostDevicePtr);
                    shadow.context = shadow.device.ImmediateContext;

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
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int PrepareResourcesDelegate(IntPtr thisPtr, IntPtr presentTargetTime, IntPtr desiredRenderTargetSize);
            private unsafe static int PrepareResources(IntPtr thisPtr, IntPtr presentTargetTime, IntPtr desiredRenderTargetSize)
            {
                try
                {
                    var shadow = ToShadow<DrawingSurfaceBackgroundContentProviderShadow>(thisPtr);
                    var callback = (IDrawingSurfaceBackgroundContentProviderNative)shadow.Callback;
                    callback.PrepareResources(new DateTime(*(long*)presentTargetTime), *(DrawingSizeF*)desiredRenderTargetSize);
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

                    if (shadow.device.NativePointer != hostDevice)
                        shadow.device = new Device(hostDevice);

                    if (shadow.context.NativePointer != hostDeviceContext)
                        shadow.context = new DeviceContext(hostDeviceContext);

                    if (shadow.renderTargetView == null || shadow.renderTargetView.NativePointer != hostRenderTargetView)
                        shadow.renderTargetView = new RenderTargetView(hostRenderTargetView);

                    callback.Draw(shadow.device, shadow.context, shadow.renderTargetView);
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