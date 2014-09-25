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

using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a front end to <see cref="SharpDX.Direct3D11.Device"/> and <see cref="SharpDX.Direct3D11.DeviceContext"/>.
    /// </summary>
    public class GraphicsDevice : Component
    {
        private readonly Dictionary<object, object> sharedDataPerDevice;
        private readonly Dictionary<object, object> sharedDataPerDeviceContext = new Dictionary<object, object>();
        internal Device Device;
        internal DeviceContext Context;
        internal CommonShaderStage[] ShaderStages;
        private readonly ViewportF[] viewports = new ViewportF[16];
        internal IntPtr ResetSlotsPointers { get; private set; }
        private int maxSlotCountForVertexBuffer;

        private const int SimultaneousRenderTargetCount = OutputMergerStage.SimultaneousRenderTargetCount;
        private readonly RenderTargetView[] currentRenderTargetViews = new RenderTargetView[SimultaneousRenderTargetCount];
        private RenderTargetView currentRenderTargetView;
        private int actualRenderTargetViewCount;
        private DepthStencilView currentDepthStencilView;

        private readonly PrimitiveQuad primitiveQuad;

        private VertexInputLayout currentVertexInputLayout;
        internal EffectPass CurrentPass;

        private readonly Dictionary<InputSignatureKey, InputSignatureManager> inputSignatureCache;

        /// <summary>
        /// Gets the features supported by this <see cref="GraphicsDevice"/>.
        /// </summary>
        public readonly GraphicsDeviceFeatures Features;

        /// <summary>
        /// Default effect pool shared between all deferred GraphicsDevice instances.
        /// </summary>
        public readonly EffectPool DefaultEffectPool;

        /// <summary>
        /// Gets the <see cref="GraphicsDevice"/> for immediate rendering.
        /// </summary>
        public readonly GraphicsDevice MainDevice;

        /// <summary>
        /// Gets whether this <see cref="GraphicsDevice"/> is running in debug.
        /// </summary>
        public readonly bool IsDebugMode;

        /// <summary>
        /// Gets whether this <see cref="GraphicsDevice"/> is a deferred context.
        /// </summary>
        public readonly bool IsDeferred;

        /// <summary>
        /// Gets the registered <see cref="BlendState"/> for this graphics device.
        /// </summary>
        public readonly BlendStateCollection BlendStates;

        /// <summary>
        /// Gets the registered <see cref="DepthStencilState"/> for this graphics device.
        /// </summary>
        public readonly DepthStencilStateCollection DepthStencilStates;

        /// <summary>
        /// Gets the registered <see cref="SamplerState"/> for this graphics device.
        /// </summary>
        public readonly SamplerStateCollection SamplerStates;

        /// <summary>
        /// Gets the registered <see cref="RasterizerState"/> for this graphics device.
        /// </summary>
        public readonly RasterizerStateCollection RasterizerStates;

        internal InputAssemblerStage InputAssemblerStage;
        internal VertexShaderStage VertexShaderStage;
        internal DomainShaderStage DomainShaderStage;
        internal HullShaderStage HullShaderStage;
        internal GeometryShaderStage GeometryShaderStage;
        internal RasterizerStage RasterizerStage;
        internal PixelShaderStage PixelShaderStage;
        internal OutputMergerStage OutputMergerStage;
        internal ComputeShaderStage ComputeShaderStage;

        internal readonly bool needWorkAroundForUpdateSubResource;

        protected GraphicsDevice(DriverType type, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
            : this((featureLevels != null && featureLevels.Length > 0) ? new Device(type, flags, featureLevels) : new Device(type, flags))
        {
        }

        protected GraphicsDevice(GraphicsAdapter adapter, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
            : this((featureLevels != null && featureLevels.Length > 0) ? new Device(adapter, flags, featureLevels) : new Device(adapter, flags), adapter)
        {
        }

        protected GraphicsDevice(SharpDX.Direct3D11.Device existingDevice, GraphicsAdapter adapter = null)
        {
            Device = ToDispose(existingDevice);
            Adapter = adapter;

            // If the adapter is null, then try to locate back the adapter
            if (adapter == null)
            {
                try
                {
                    using (var dxgiDevice = Device.QueryInterface<DXGI.Device>())
                    {
                        using (var dxgiAdapter = dxgiDevice.Adapter)
                        {
                            var deviceId = dxgiAdapter.Description.DeviceId;

                            foreach (var graphicsAdapter in GraphicsAdapter.Adapters)
                            {
                                if (deviceId == graphicsAdapter.Description.DeviceId)
                                {
                                    Adapter = graphicsAdapter;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            EffectPools = new SharpDX.Collections.ObservableCollection<EffectPool>();

            IsDebugMode = (Device.CreationFlags & DeviceCreationFlags.Debug) != 0;
            MainDevice = this;
            Context = Device.ImmediateContext;
            IsDeferred = false;
            Features = new GraphicsDeviceFeatures(Device);
            AutoViewportFromRenderTargets = true; // By default

            // Global cache for all input signatures inside a GraphicsDevice.
            inputSignatureCache = new Dictionary<InputSignatureKey, InputSignatureManager>();
            sharedDataPerDevice = new Dictionary<object, object>();

            // Create default Effect pool
            DefaultEffectPool = EffectPool.New(this, "Default");

            // Create all default states
            BlendStates = ToDispose(new BlendStateCollection(this));
            DepthStencilStates = ToDispose(new DepthStencilStateCollection(this));
            SamplerStates = ToDispose(new SamplerStateCollection(this));
            RasterizerStates = ToDispose(new RasterizerStateCollection(this));

            Initialize();

            // Create Internal Effect
            primitiveQuad = ToDispose(new PrimitiveQuad(this));
        }

        protected GraphicsDevice(GraphicsDevice mainDevice, DeviceContext deferredContext)
        {
            Device = mainDevice.Device;
            Adapter = mainDevice.Adapter;
            IsDebugMode = (Device.CreationFlags & DeviceCreationFlags.Debug) != 0;
            MainDevice = mainDevice;
            Context = deferredContext;
            IsDeferred = true;
            Features = mainDevice.Features;

            // Create default Effect pool
            EffectPools = mainDevice.EffectPools;
            DefaultEffectPool = mainDevice.DefaultEffectPool;

            // Copy the Global cache for all input signatures inside a GraphicsDevice.
            inputSignatureCache = mainDevice.inputSignatureCache;
            sharedDataPerDevice = mainDevice.sharedDataPerDevice;

            // Copy the reset vertex buffer
            ResetSlotsPointers = mainDevice.ResetSlotsPointers;

            // Create all default states
            BlendStates = mainDevice.BlendStates;
            DepthStencilStates = mainDevice.DepthStencilStates;
            SamplerStates = mainDevice.SamplerStates;
            RasterizerStates = mainDevice.RasterizerStates;

            // Setup the workaround flag
            needWorkAroundForUpdateSubResource = IsDeferred && !Features.HasDriverCommandLists;
            Initialize();

            // Create Internal Effect
            primitiveQuad = ToDispose(new PrimitiveQuad(this));
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            // Default null VertexBuffers used to reset
            if (ResetSlotsPointers == IntPtr.Zero)
            {
                // CommonShaderStage.InputResourceSlotCount is the maximum of resources bindable in the whole pipeline
                ResetSlotsPointers = ToDispose(Utilities.AllocateClearedMemory(Utilities.SizeOf<IntPtr>() * CommonShaderStage.InputResourceSlotCount));
            }

            InputAssemblerStage = Context.InputAssembler;
            VertexShaderStage = Context.VertexShader;
            DomainShaderStage = Context.DomainShader;
            HullShaderStage = Context.HullShader;
            GeometryShaderStage = Context.GeometryShader;
            RasterizerStage = Context.Rasterizer;
            PixelShaderStage = Context.PixelShader;
            OutputMergerStage = Context.OutputMerger;
            ComputeShaderStage = Context.ComputeShader;

            // Precompute shader stages
            ShaderStages = new CommonShaderStage[]
                               {
                                   Context.VertexShader,
                                   Context.HullShader,
                                   Context.DomainShader,
                                   Context.GeometryShader,
                                   Context.PixelShader,
                                   Context.ComputeShader
                               };

            Performance = new GraphicsPerformance(this);
        }

        /// <summary>
        /// Check if a feature level is supported by a primary adapter.
        /// </summary>
        /// <param name="featureLevel">The feature level.</param>
        /// <returns><c>true</c> if the primary adapter is supporting this feature level; otherwise, <c>false</c>.</returns>
        public static bool IsProfileSupported(FeatureLevel featureLevel)
        {
            return SharpDX.Direct3D11.Device.IsSupportedFeatureLevel(featureLevel);
        }

        /// <summary>
        /// Gets the adapter associated with this device.
        /// </summary>
        public readonly GraphicsAdapter Adapter;

        /// <summary>
        /// Gets the effect pools.
        /// </summary>
        /// <value>The effect pools.</value>
        public SharpDX.Collections.ObservableCollection<EffectPool> EffectPools { get; private set; }

        /// <summary>
        /// Gets the back buffer sets by the current <see cref="Presenter" /> setup on this device.
        /// </summary>
        /// <value>The back buffer. The returned value may be null if no <see cref="GraphicsPresenter"/> are setup on this device.</value>
        public RenderTarget2D BackBuffer
        {
            get { return Presenter != null ? Presenter.BackBuffer : null; }
        }

        /// <summary>
        /// Gets the depth stencil buffer sets by the current <see cref="Presenter" /> setup on this device.
        /// </summary>
        /// <value>The depth stencil buffer. The returned value may be null if no <see cref="GraphicsPresenter"/> are setup on this device or no depth buffer was allocated.</value>
        public DepthStencilBuffer DepthStencilBuffer
        {
            get { return Presenter != null ? Presenter.DepthStencilBuffer : null; }
        }

        /// <summary>
        /// Gets or sets the current presenter use by the <see cref="Present"/> method.
        /// </summary>
        /// <value>The current presenter.</value>
        public GraphicsPresenter Presenter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the viewport is automatically calculated and set when a render target is set. Default is true.
        /// </summary>
        /// <value><c>true</c> if the viewport is automatically calculated and set when a render target is set; otherwise, <c>false</c>.</value>
        public bool AutoViewportFromRenderTargets { get; set; }

        /// <summary>
        /// Gets the status of this device.
        /// </summary>
        /// <msdn-id>ff476526</msdn-id>	
        /// <unmanaged>GetDeviceRemovedReason</unmanaged>	
        /// <unmanaged-short>GetDeviceRemovedReason</unmanaged-short>	
        /// <unmanaged>HRESULT ID3D11Device::GetDeviceRemovedReason()</unmanaged>
        public GraphicsDeviceStatus GraphicsDeviceStatus
        {
            get
            {
                var result = ((Device)MainDevice).DeviceRemovedReason;
                if (result == DXGI.ResultCode.DeviceRemoved)
                {
                    return GraphicsDeviceStatus.Removed;
                }

                if (result == DXGI.ResultCode.DeviceReset)
                {
                    return GraphicsDeviceStatus.Reset;
                }

                if (result == DXGI.ResultCode.DeviceHung)
                {
                    return GraphicsDeviceStatus.Hung;
                }

                if (result == DXGI.ResultCode.DriverInternalError)
                {
                    return GraphicsDeviceStatus.InternalError;
                }

                if (result == DXGI.ResultCode.InvalidCall)
                {
                    return GraphicsDeviceStatus.InvalidCall;
                }

                if (result.Code < 0)
                {
                    return GraphicsDeviceStatus.Reset;
                }

                return GraphicsDeviceStatus.Normal;
            }
        }

        /// <summary>
        /// Gets the access to performance profiler.
        /// </summary>
        /// <value>The access to performance profiler.</value>
        public GraphicsPerformance Performance { get; private set; }

        /// <summary>
        /// Gets the default quad primitive to issue draw commands.
        /// </summary>
        /// <value>The default quad primitive to issue draw commands.</value>
        public PrimitiveQuad Quad
        {
            get
            {
                return primitiveQuad;
            }
        }

        /// <summary>
        /// Clears the default render target and depth stencil buffer attached to the current <see cref="Presenter"/>.
        /// </summary>
        /// <param name="color">Set this color value in all buffers.</param>
        /// <exception cref="System.InvalidOperationException">Cannot clear without a Presenter set on this instance</exception>
        public void Clear(Color4 color)
        {
            var options = currentRenderTargetView != null ? ClearOptions.Target : (ClearOptions)0;

            if (currentDepthStencilView != null)
            {
                var textureView = currentDepthStencilView.Tag as TextureView;
                DepthStencilBuffer depthStencilBuffer;

                if (textureView == null || (depthStencilBuffer = textureView.Texture as DepthStencilBuffer) == null)
                {
                    throw new InvalidOperationException("Clear on a custom DepthStencilView is not supported by this method. Use Clear(DepthStencilView) directly");
                }

                options |= depthStencilBuffer.HasStencil ? ClearOptions.DepthBuffer | ClearOptions.Stencil : ClearOptions.DepthBuffer;

            }

            Clear(options, color, 1f, 0);
        }

        /// <summary>
        /// Clears the default render target and depth stencil buffer attached to the current <see cref="Presenter"/>.
        /// </summary>
        /// <param name="options">Options for clearing a buffer.</param>
        /// <param name="color">Set this four-component color value in the buffer.</param>
        /// <param name="depth">Set this depth value in the buffer.</param>
        /// <param name="stencil">Set this stencil value in the buffer.</param>
        public void Clear(ClearOptions options, Color4 color, float depth, int stencil)
        {
            if ((options & ClearOptions.Target) != 0)
            {
                if (currentRenderTargetView == null)
                {
                    throw new InvalidOperationException("No default render target view setup. Call SetRenderTargets before calling this method.");
                }
                Clear(currentRenderTargetView, color);
            }

            if ((options & (ClearOptions.Stencil | ClearOptions.DepthBuffer)) != 0)
            {
                if (currentDepthStencilView == null)
                {
                    throw new InvalidOperationException("No default depth stencil view setup. Call SetRenderTargets before calling this method.");
                }

                var flags = (options & ClearOptions.DepthBuffer) != 0 ? DepthStencilClearFlags.Depth : 0;
                if ((options & ClearOptions.Stencil) != 0)
                {
                    flags |= DepthStencilClearFlags.Stencil;
                }

                Clear(currentDepthStencilView, flags, depth, (byte)stencil);
            }
        }

        /// <summary>
        /// Clears the default render target and depth stencil buffer attached to the current <see cref="Presenter"/>.
        /// </summary>
        /// <param name="options">Options for clearing a buffer.</param>
        /// <param name="color">Set this four-component color value in the buffer.</param>
        /// <param name="depth">Set this depth value in the buffer.</param>
        /// <param name="stencil">Set this stencil value in the buffer.</param>
        public void Clear(ClearOptions options, Vector4 color, float depth, int stencil)
        {
            Clear(options, (Color4)color, depth, stencil);
        }

        /// <summary>
        /// Clears a render target view by setting all the elements in a render target to one value.
        /// </summary>
        /// <param name="renderTargetView">The render target view.</param>
        /// <param name="colorRGBA">A 4-component array that represents the color to fill the render target with.</param>
        /// <remarks><p>Applications that wish to clear a render target to a specific integer value bit pattern should render a screen-aligned quad instead of using this method.  The reason for this is because this method accepts as input a floating point value, which may not have the same bit pattern as the original integer.</p><table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 11/10:</p> <p>Unlike Direct3D 9, the full extent of the resource view is always cleared. Viewport and scissor settings are not applied.</p> </td></tr> </table><p>?</p></remarks>
        /// <msdn-id>ff476388</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::ClearRenderTargetView([In] ID3D11RenderTargetView* pRenderTargetView,[In] const SHARPDX_COLOR4* ColorRGBA)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::ClearRenderTargetView</unmanaged-short>
        public void Clear(SharpDX.Direct3D11.RenderTargetView renderTargetView, Color4 colorRGBA)
        {
            Context.ClearRenderTargetView(renderTargetView, colorRGBA);
        }

        /// <summary>	
        /// Clears the depth-stencil resource.
        /// </summary>	
        /// <param name="depthStencilView"><dd>  <p>Pointer to the depth stencil to be cleared.</p> </dd></param>	
        /// <param name="clearFlags"><dd>  <p>Identify the type of data to clear (see <strong><see cref="SharpDX.Direct3D11.DepthStencilClearFlags"/></strong>).</p> </dd></param>	
        /// <param name="depth"><dd>  <p>Clear the depth buffer with this value. This value will be clamped between 0 and 1.</p> </dd></param>	
        /// <param name="stencil"><dd>  <p>Clear the stencil buffer with this value.</p> </dd></param>	
        /// <remarks>	
        /// <table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 11/10:</p> <p>Unlike Direct3D 9, the full extent of the resource view is always cleared. Viewport and scissor settings are not applied.</p> </td></tr> </table><p>?</p>	
        /// </remarks>	
        /// <msdn-id>ff476387</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearDepthStencilView([In] ID3D11DepthStencilView* pDepthStencilView,[In] D3D11_CLEAR_FLAG ClearFlags,[In] float Depth,[In] unsigned char Stencil)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearDepthStencilView</unmanaged-short>	
        public void Clear(SharpDX.Direct3D11.DepthStencilView depthStencilView, SharpDX.Direct3D11.DepthStencilClearFlags clearFlags, float depth, byte stencil)
        {
            Context.ClearDepthStencilView(depthStencilView, clearFlags, depth, stencil);
        }

        /// <summary>	
        /// Clears an unordered access resource with bit-precise values.	
        /// </summary>	
        /// <param name="view">The buffer to clear.</param>	
        /// <param name="value">The value used to clear.</param>	
        /// <remarks>	
        /// <p>This API copies the lower ni bits from each array element i to the corresponding channel, where ni is the number of bits in the ith channel of the resource format (for example, R8G8B8_FLOAT has 8 bits for the first 3 channels). This works on any UAV with no format conversion.  For a raw or structured buffer view, only the first array element value is used.</p>	
        /// </remarks>	
        /// <msdn-id>ff476391</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearUnorderedAccessViewUint([In] ID3D11UnorderedAccessView* pUnorderedAccessView,[In] const unsigned int* Values)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearUnorderedAccessViewUint</unmanaged-short>	
        public void Clear(UnorderedAccessView view, Int4 value)
        {
            Context.ClearUnorderedAccessView(view, value);
        }

        /// <summary>	
        /// Clears an unordered access resource with a float value.
        /// </summary>	
        /// <param name="view">The buffer to clear.</param>	
        /// <param name="value">The value used to clear.</param>	
        /// <remarks>	
        /// <p>This API works on FLOAT, UNORM, and SNORM unordered access views (UAVs), with format conversion from FLOAT to *NORM where appropriate. On other UAVs, the operation is invalid and the call will not reach the driver.</p>	
        /// </remarks>	
        /// <msdn-id>ff476390</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearUnorderedAccessViewFloat([In] ID3D11UnorderedAccessView* pUnorderedAccessView,[In] const float* Values)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearUnorderedAccessViewFloat</unmanaged-short>	
        public void Clear(UnorderedAccessView view, Vector4 value)
        {
            Context.ClearUnorderedAccessView(view, value);
        }

        /// <summary>
        /// Copies the content of this resource to another <see cref="GraphicsResource" />.
        /// </summary>
        /// <param name="fromResource">The resource to copy from.</param>
        /// <param name="toResource">The resource to copy to.</param>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476392</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopyResource</unmanaged-short>
        public void Copy(Direct3D11.Resource fromResource, Direct3D11.Resource toResource)
        {
            Context.CopyResource(fromResource, toResource);
        }

        /// <summary>	
        /// Copy a region from a source resource to a destination resource.	
        /// </summary>	
        /// <remarks>	
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a region (10,20),(90,140) in a destination texture. 	
        /// <code> D3D11_BOX sourceRegion;	
        /// sourceRegion.left = 120;	
        /// sourceRegion.right = 200;	
        /// sourceRegion.top = 100;	
        /// sourceRegion.bottom = 220;	
        /// sourceRegion.front = 0;	
        /// sourceRegion.back = 1; pd3dDeviceContext-&gt;CopySubresourceRegion( pDestTexture, 0, 10, 20, 0, pSourceTexture, 0, &amp;sourceRegion ); </code>	
        /// 	
        ///  Notice, that for a 2D texture, front and back are set to 0 and 1 respectively. 	
        /// </remarks>	
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="sourceSubresource">Source subresource index. </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destinationSubResource">Destination subresource index. </param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region. For a 1D subresource, this must be zero. </param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <msdn-id>ff476394</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopySubresourceRegion([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In] unsigned int DstX,[In] unsigned int DstY,[In] unsigned int DstZ,[In] ID3D11Resource* pSrcResource,[In] unsigned int SrcSubresource,[In, Optional] const D3D11_BOX* pSrcBox)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopySubresourceRegion</unmanaged-short>	
        public void Copy(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.Resource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
        {
            Context.CopySubresourceRegion(source, sourceSubresource, null, destination, destinationSubResource, dstX, dstY, dstZ);
        }

        /// <summary>	
        /// Copy a region from a source resource to a destination resource.	
        /// </summary>	
        /// <remarks>	
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a region (10,20),(90,140) in a destination texture. 	
        /// <code> D3D11_BOX sourceRegion;	
        /// sourceRegion.left = 120;	
        /// sourceRegion.right = 200;	
        /// sourceRegion.top = 100;	
        /// sourceRegion.bottom = 220;	
        /// sourceRegion.front = 0;	
        /// sourceRegion.back = 1; pd3dDeviceContext-&gt;CopySubresourceRegion( pDestTexture, 0, 10, 20, 0, pSourceTexture, 0, &amp;sourceRegion ); </code>	
        /// 	
        ///  Notice, that for a 2D texture, front and back are set to 0 and 1 respectively. 	
        /// </remarks>	
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="sourceSubresource">Source subresource index. </param>
        /// <param name="sourceRegion">A reference to a 3D box (see <see cref="SharpDX.Direct3D11.ResourceRegion"/>) that defines the source subresources that can be copied. If NULL, the entire source subresource is copied. The box must fit within the source resource. </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destinationSubResource">Destination subresource index. </param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region. For a 1D subresource, this must be zero. </param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <msdn-id>ff476394</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopySubresourceRegion([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In] unsigned int DstX,[In] unsigned int DstY,[In] unsigned int DstZ,[In] ID3D11Resource* pSrcResource,[In] unsigned int SrcSubresource,[In, Optional] const D3D11_BOX* pSrcBox)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopySubresourceRegion</unmanaged-short>	
        public void Copy(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.ResourceRegion sourceRegion, SharpDX.Direct3D11.Resource destination, int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
        {
            Context.CopySubresourceRegion(source, sourceSubresource, sourceRegion, destination, destinationSubResource, dstX, dstY, dstZ);
        }

        /// <summary>	
        /// Copy a multisampled resource into a non-multisampled resource.	
        /// </summary>	
        /// <remarks>	
        /// This API is most useful when re-using the resulting render target of one render pass as an input to a second render pass. The source and destination resources must be the same resource type and have the same dimensions. In addition, they must have compatible formats. There are three scenarios for this:  ScenarioRequirements Source and destination are prestructured and typedBoth the source and destination must have identical formats and that format must be specified in the Format parameter. One resource is prestructured and typed and the other is prestructured and typelessThe typed resource must have a format that is compatible with the typeless resource (i.e. the typed resource is DXGI_FORMAT_R32_FLOAT and the typeless resource is DXGI_FORMAT_R32_TYPELESS). The format of the typed resource must be specified in the Format parameter. Source and destination are prestructured and typelessBoth the source and destination must have the same typeless format (i.e. both must have DXGI_FORMAT_R32_TYPELESS), and the Format parameter must specify a format that is compatible with the source and destination (i.e. if both are DXGI_FORMAT_R32_TYPELESS then DXGI_FORMAT_R32_FLOAT could be specified in the Format parameter). For example, given the DXGI_FORMAT_R16G16B16A16_TYPELESS format:  The source (or dest) format could be DXGI_FORMAT_R16G16B16A16_UNORM The dest (or source) format could be DXGI_FORMAT_R16G16B16A16_FLOAT    ? 	
        /// </remarks>	
        /// <param name="source">Source resource. Must be multisampled. </param>
        /// <param name="sourceSubresource">&gt;The source subresource of the source resource. </param>
        /// <param name="destination">Destination resource. Must be a created with the <see cref="SharpDX.Direct3D11.ResourceUsage.Default"/> flag and be single-sampled. See <see cref="SharpDX.Direct3D11.Resource"/>. </param>
        /// <param name="destinationSubresource">A zero-based index, that identifies the destination subresource. Use {{D3D11CalcSubresource}} to calculate the index. </param>
        /// <param name="format">A <see cref="SharpDX.DXGI.Format"/> that indicates how the multisampled resource will be resolved to a single-sampled resource.  See remarks. </param>
        /// <unmanaged>void ID3D11DeviceContext::ResolveSubresource([In] ID3D11Resource* pDstResource,[In] int DstSubresource,[In] ID3D11Resource* pSrcResource,[In] int SrcSubresource,[In] DXGI_FORMAT Format)</unmanaged>
        public void Copy(SharpDX.Direct3D11.Resource source, int sourceSubresource, SharpDX.Direct3D11.Resource destination, int destinationSubresource, SharpDX.DXGI.Format format)
        {
            Context.ResolveSubresource(source, sourceSubresource, destination, destinationSubresource, format);
        }

        /// <summary>	
        /// <p>Copies data from a buffer holding variable length data.</p>	
        /// </summary>	
        /// <param name="sourceView"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D11.UnorderedAccessView"/></strong> of a Structured Buffer resource created with either  <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Append"/></strong> or <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Counter"/></strong> specified  when the UAV was created.   These types of resources have hidden counters tracking "how many" records have  been written.</p> </dd></param>	
        /// <param name="destinationBuffer"><dd>  <p>Pointer to <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>.  This can be any buffer resource that other copy commands,  such as <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/></strong> or <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopySubresourceRegion_"/></strong>, are able to write to.</p> </dd></param>	
        /// <param name="offsetInBytes"><dd>  <p>Offset from the start of <em>pDstBuffer</em> to write 32-bit UINT structure (vertex) count from <em>pSrcView</em>.</p> </dd></param>	
        /// <msdn-id>ff476393</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::CopyStructureCount([In] ID3D11Buffer* pDstBuffer,[In] unsigned int DstAlignedByteOffset,[In] ID3D11UnorderedAccessView* pSrcView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::CopyStructureCount</unmanaged-short>	
        public void CopyCount(SharpDX.Direct3D11.UnorderedAccessView sourceView, SharpDX.Direct3D11.Buffer destinationBuffer, int offsetInBytes = 0)
        {
            Context.CopyStructureCount(destinationBuffer, offsetInBytes, sourceView);
        }

        /// <summary>	
        /// <p>Restore all default settings.</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>This method resets any device context to the default settings. This sets all input/output resource slots, shaders, input layouts, predications, scissor rectangles, depth-stencil state, rasterizer state, blend state, sampler state, and viewports to <strong><c>null</c></strong>. The primitive topology is set to UNDEFINED.</p><p>For a scenario where you would like to clear a list of commands recorded so far, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.FinishCommandListInternal"/></strong> and throw away the resulting <strong><see cref="SharpDX.Direct3D11.CommandList"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>ff476389</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::ClearState()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::ClearState</unmanaged-short>	
        public void ClearState()
        {
            Context.ClearState();
        }

        private PrimitiveTopology PrimitiveType
        {
            set
            {
                InputAssemblerStage.PrimitiveTopology = value;
            }
        }

        /// <summary>	
        /// <p>Draw indexed, non-instanced primitives.</p>	
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="indexCount"><dd>  <p>Number of indices to draw.</p> </dd></param>	
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>	
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>If the sum of both indices is negative, the result of the function call is undefined.</p>	
        /// </remarks>	
        /// <msdn-id>ff476409</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexed([In] unsigned int IndexCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexed</unmanaged-short>	
        public void DrawIndexed(PrimitiveType primitiveType, int indexCount, int startIndexLocation = 0, int baseVertexLocation = 0)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        /// <summary>	
        /// <p>Draw non-indexed, non-instanced primitives.</p>	
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="vertexCount"><dd>  <p>Number of vertices to draw.</p> </dd></param>	
        /// <param name="startVertexLocation"><dd>  <p>Index of the first vertex, which is usually an offset in a vertex buffer; it could also be used as the first vertex id generated for a shader parameter marked with the <strong>SV_TargetId</strong> system-value semantic.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>The vertex data for a draw call normally comes from a vertex buffer that is bound to the pipeline. However, you could also provide the vertex data from a shader that has vertex data marked with the <strong>SV_VertexId</strong> system-value semantic.</p>	
        /// </remarks>	
        /// <msdn-id>ff476407</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Draw([In] unsigned int VertexCount,[In] unsigned int StartVertexLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Draw</unmanaged-short>	
        public void Draw(PrimitiveType primitiveType, int vertexCount, int startVertexLocation = 0)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.Draw(vertexCount, startVertexLocation);
        }

        /// <summary>	
        /// <p>Draw indexed, instanced primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="indexCountPerInstance"><dd>  <p>Number of indices read from the index buffer for each instance.</p> </dd></param>	
        /// <param name="instanceCount"><dd>  <p>Number of instances to draw.</p> </dd></param>	
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>	
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>	
        /// <param name="startInstanceLocation"><dd>  <p>A value added to each index before reading per-instance data from a vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>Instancing may extend performance by reusing the same geometry to draw multiple objects in a scene. One example of instancing could be  to draw the same object with different positions and colors. Indexing requires multiple vertex buffers: at least one for per-vertex data  and a second buffer for per-instance data.</p>	
        /// </remarks>	
        /// <msdn-id>ff476410</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexedInstanced([In] unsigned int IndexCountPerInstance,[In] unsigned int InstanceCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation,[In] unsigned int StartInstanceLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexedInstanced</unmanaged-short>	
        public void DrawIndexedInstanced(PrimitiveType primitiveType, int indexCountPerInstance, int instanceCount, int startIndexLocation = 0, int baseVertexLocation = 0, int startInstanceLocation = 0)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
        }

        /// <summary>	
        /// <p>Draw non-indexed, instanced primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="vertexCountPerInstance"><dd>  <p>Number of vertices to draw.</p> </dd></param>	
        /// <param name="instanceCount"><dd>  <p>Number of instances to draw.</p> </dd></param>	
        /// <param name="startVertexLocation"><dd>  <p>Index of the first vertex.</p> </dd></param>	
        /// <param name="startInstanceLocation"><dd>  <p>A value added to each index before reading per-instance data from a vertex buffer.</p> </dd></param>	
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline.</p><p>Instancing may extend performance by reusing the same geometry to draw multiple objects in a scene. One example of instancing could be  to draw the same object with different positions and colors.</p><p>The vertex data for an instanced draw call normally comes from a vertex buffer that is bound to the pipeline.  However, you could also provide the vertex data from a shader that has instanced data identified with a system-value semantic (SV_InstanceID).</p>	
        /// </remarks>	
        /// <msdn-id>ff476412</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawInstanced([In] unsigned int VertexCountPerInstance,[In] unsigned int InstanceCount,[In] unsigned int StartVertexLocation,[In] unsigned int StartInstanceLocation)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawInstanced</unmanaged-short>	
        public void DrawInstanced(PrimitiveType primitiveType, int vertexCountPerInstance, int instanceCount, int startVertexLocation = 0, int startInstanceLocation = 0)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
        }

        /// <summary>	
        /// <p>Draw geometry of an unknown size.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <remarks>	
        /// <p>A draw API submits work to the rendering pipeline. This API submits work of an unknown size that was processed by the input assembler, vertex shader, and stream-output stages;  the work may or may not have gone through the geometry-shader stage.</p><p>After data has been streamed out to stream-output stage buffers, those buffers can be again bound to the Input Assembler stage at input slot 0 and DrawAuto will draw them without the application needing to know the amount of data that was written to the buffers. A measurement of the amount of data written to the SO stage buffers is maintained internally when the data is streamed out. This means that the CPU does not need to fetch the measurement before re-binding the data that was streamed as input data. Although this amount is tracked internally, it is still the responsibility of applications to use input layouts to describe the format of the data in the SO stage buffers so that the layouts are available when the buffers are again bound to the input assembler.</p><p>The following diagram shows the DrawAuto process.</p><p></p><p>Calling DrawAuto does not change the state of the streaming-output buffers that were bound again as inputs.</p><p>DrawAuto only works when drawing with one input buffer bound as an input to the IA stage at slot 0. Applications must create the SO buffer resource with both binding flags, <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> and <strong><see cref="SharpDX.Direct3D11.BindFlags.StreamOutput"/></strong>.</p><p>This API does not support indexing or instancing.</p><p>If an application needs to retrieve the size of the streaming-output buffer, it can query for statistics on streaming output by using <strong><see cref="SharpDX.Direct3D11.QueryType.StreamOutputStatistics"/></strong>.</p>	
        /// </remarks>	
        /// <msdn-id>ff476408</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawAuto()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawAuto</unmanaged-short>	
        public void DrawAuto(PrimitiveType primitiveType)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.DrawAuto();
        }

        /// <summary>	
        /// <p>Draw indexed, instanced, GPU-generated primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which is a buffer containing the GPU generated primitives.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>Offset in <em>pBufferForArgs</em> to the start of the GPU generated primitives.</p> </dd></param>	
        /// <remarks>	
        /// <p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476411</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexedInstancedIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexedInstancedIndirect</unmanaged-short>	
        public void DrawIndexedInstanced(PrimitiveType primitiveType, SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.DrawIndexedInstancedIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Draw instanced, GPU-generated primitives.</p>	
        /// </summary>	
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which is a buffer containing the GPU generated primitives.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>Offset in <em>pBufferForArgs</em> to the start of the GPU generated primitives.</p> </dd></param>	
        /// <remarks>	
        /// <p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476413</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DrawInstancedIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DrawInstancedIndirect</unmanaged-short>	
        public void DrawInstanced(PrimitiveType primitiveType, SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            SetupInputLayout();

            PrimitiveType = primitiveType;
            Context.DrawInstancedIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Execute a command list from a thread group.</p>	
        /// </summary>	
        /// <param name="threadGroupCountX"><dd>  <p>The number of groups dispatched in the x direction. <em>ThreadGroupCountX</em> must be less than <see cref="SharpDX.Direct3D11.ComputeShaderStage.DispatchMaximumThreadGroupsPerDimension"/> (65535).</p> </dd></param>	
        /// <param name="threadGroupCountY"><dd>  <p>The number of groups dispatched in the y direction. <em>ThreadGroupCountY</em> must be less than <see cref="SharpDX.Direct3D11.ComputeShaderStage.DispatchMaximumThreadGroupsPerDimension"/> (65535).</p> </dd></param>	
        /// <param name="threadGroupCountZ"><dd>  <p>The number of groups dispatched in the z direction. <em>ThreadGroupCountZ</em> must be less than <see cref="SharpDX.Direct3D11.ComputeShaderStage.DispatchMaximumThreadGroupsPerDimension"/> (65535).  In feature level 10 the value for <em>ThreadGroupCountZ</em> must be 1.</p> </dd></param>	
        /// <remarks>	
        /// <p>You call the <strong>Dispatch</strong> method to execute commands in a compute shader. A compute shader can be run on many threads in parallel, within a thread group. Index a particular thread, within a thread group using a 3D vector  given by (x,y,z).</p><p>In the following illustration, assume a thread group with 50 threads where the size of the group is given by (5,5,2). A single thread is identified from a  thread group with 50 threads in it, using the vector (4,1,1).</p><p></p><p>The following illustration shows the relationship between the parameters passed to <strong><see cref="SharpDX.Direct3D11.DeviceContext.Dispatch"/></strong>, Dispatch(5,3,2), the values specified in the numthreads attribute, numthreads(10,8,3), and values that will passed to the compute shader for the thread-related system values 	
        /// (SV_GroupIndex,SV_DispatchThreadID,SV_GroupThreadID,SV_GroupID).</p><p></p>	
        /// </remarks>	
        /// <msdn-id>ff476405</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Dispatch([In] unsigned int ThreadGroupCountX,[In] unsigned int ThreadGroupCountY,[In] unsigned int ThreadGroupCountZ)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Dispatch</unmanaged-short>	
        public void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
        {
            Context.Dispatch(threadGroupCountX, threadGroupCountY, threadGroupCountZ);
        }

        /// <summary>	
        /// <p>Execute a command list over one or more thread groups.</p>	
        /// </summary>	
        /// <param name="argumentsBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>, which must be loaded with data that matches the argument list for <strong><see cref="SharpDX.Direct3D11.DeviceContext.Dispatch"/></strong>.</p> </dd></param>	
        /// <param name="alignedByteOffsetForArgs"><dd>  <p>A byte-aligned offset between the start of the buffer and the arguments.</p> </dd></param>	
        /// <remarks>	
        /// <p>You call the <strong>DispatchIndirect</strong> method to execute commands in a compute shader.</p><p>When an application creates a buffer that is associated with the <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> interface that  <em>pBufferForArgs</em> points to, the application must set the <strong><see cref="SharpDX.Direct3D11.ResourceOptionFlags.DrawindirectArgs"/></strong> flag in the <strong>MiscFlags</strong> member of the <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> structure that describes the buffer. To create the buffer, the application calls the <strong><see cref="SharpDX.Direct3D11.Device.CreateBuffer"/></strong> method and in this call passes a reference to <strong><see cref="SharpDX.Direct3D11.BufferDescription"/></strong> in the <em>pDesc</em> parameter.</p>	
        /// </remarks>	
        /// <msdn-id>ff476406</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::DispatchIndirect([In] ID3D11Buffer* pBufferForArgs,[In] unsigned int AlignedByteOffsetForArgs)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::DispatchIndirect</unmanaged-short>	
        public void Dispatch(SharpDX.Direct3D11.Buffer argumentsBuffer, int alignedByteOffsetForArgs = 0)
        {
            Context.DispatchIndirect(argumentsBuffer, alignedByteOffsetForArgs);
        }

        /// <summary>	
        /// <p>Sends queued-up commands in the command buffer to the graphics processing unit (GPU).</p>	
        /// </summary>	
        /// <remarks>	
        /// <p>Most applications don't need to call this method. If an application calls this method when not necessary, it incurs a performance penalty. Each call to <strong>Flush</strong> incurs a significant amount of overhead.</p><p>When Microsoft Direct3D state-setting, present, or draw commands are called by an application, those commands are queued into an internal command buffer.  <strong>Flush</strong> sends those commands to the GPU for processing. Typically, the Direct3D runtime sends these commands to the GPU automatically whenever the runtime determines that  they need to be sent, such as when the command buffer is full or when an application maps a resource. <strong>Flush</strong> sends the commands manually.</p><p>We recommend that you use <strong>Flush</strong> when the CPU waits for an arbitrary amount of time (such as when  you call the <strong>Sleep</strong> function).</p><p>Because <strong>Flush</strong> operates asynchronously,  it can return either before or after the GPU finishes executing the queued graphics commands. However, the graphics commands eventually always complete. You can call the <strong><see cref="SharpDX.Direct3D11.Device.CreateQuery"/></strong> method with the <strong><see cref="SharpDX.Direct3D11.QueryType.Event"/></strong> value to create an event query; you can then use that event query in a call to the <strong><see cref="SharpDX.Direct3D11.DeviceContext.GetDataInternal"/></strong> method to determine when the GPU is finished processing the graphics commands.	
        /// </p><p>Microsoft Direct3D?11 defers the destruction of objects. Therefore, an application can't rely upon objects immediately being destroyed. By calling <strong>Flush</strong>, you destroy any  objects whose destruction was deferred.  If an application requires synchronous destruction of an object, we recommend that the application release all its references, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.ClearState"/></strong>, and then call <strong>Flush</strong>.</p>Deferred Destruction Issues with Flip Presentation Swap Chains<p>Direct3D?11 defers the destruction of objects like views and resources until it can efficiently destroy them. This deferred destruction can cause problems with flip presentation model swap chains. Flip presentation model swap chains have the <strong>DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL</strong> flag set. When you create a flip presentation model swap chain, you can associate only one swap chain at a time with an <strong><see cref="System.IntPtr"/></strong>, <strong>IWindow</strong>, or composition surface. If an application attempts to destroy a flip presentation model swap chain and replace it with another swap chain, the original swap chain is not destroyed when the application immediately frees all of the original swap chain's references.</p><p>Most applications typically use the <strong><see cref="SharpDX.DXGI.SwapChain.ResizeBuffers"/></strong> method for the majority of scenarios where they replace new swap chain buffers for old swap chain buffers. However, if an application must actually destroy an old swap chain and create a new swap chain, the application must force the destruction of all objects that the application freed. To force the destruction, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.ClearState"/></strong> (or otherwise ensure no views are bound to pipeline state), and then call <strong>Flush</strong> on the immediate context. You must force destruction before you call <strong>IDXGIFactory2::CreateSwapChainForHwnd</strong>, <strong>IDXGIFactory2::CreateSwapChainForImmersiveWindow</strong>, or <strong>IDXGIFactory2::CreateSwapChainForCompositionSurface</strong> again to create a new swap chain.</p>	
        /// </remarks>	
        /// <msdn-id>ff476425</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::Flush()</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::Flush</unmanaged-short>	
        public void Flush()
        {
            Context.Flush();
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/> from an existing <see cref="SharpDX.Direct3D11.Device"/>.
        /// </summary>
        /// <param name="existingDevice">An existing device.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice New(SharpDX.Direct3D11.Device existingDevice)
        {
            return new GraphicsDevice(existingDevice);
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/> using <see cref="DriverType.Hardware"/>.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/></returns>
        public static GraphicsDevice New(DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return New(DriverType.Hardware, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice New(DriverType type, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            if (type == DriverType.Hardware)
                return new GraphicsDevice(GraphicsAdapter.Default, flags, featureLevels);

            return new GraphicsDevice(type, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <param name="adapter">The graphics adapter to use.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="GraphicsDevice"/>.</returns>
        public static GraphicsDevice New(GraphicsAdapter adapter, DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return new GraphicsDevice(adapter, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new deferred <see cref="GraphicsDevice"/>.
        /// </summary>
        /// <returns>A deferred <see cref="GraphicsDevice"/>.</returns>
        public GraphicsDevice NewDeferred()
        {
            return new GraphicsDevice(this, new DeviceContext(Device));
        }

        /// <summary>	
        /// <p>Sets the blend state of the output-merger stage.</p>	
        /// </summary>	
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>	
        /// <remarks>	
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476462</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>	
        public void SetBlendState(BlendState blendState)
        {
            if (blendState == null)
            {
                OutputMergerStage.SetBlendState(null, Color.White, -1);
            }
            else
            {
                OutputMergerStage.SetBlendState(blendState, blendState.BlendFactor, blendState.MultiSampleMask);
            }
        }

        /// <summary>	
        /// <p>Sets the blend state of the output-merger stage.</p>	
        /// </summary>	
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <param name="blendFactor"><dd>  <p>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <strong><see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/></strong> option.</p> </dd></param>	
        /// <param name="multiSampleMask"><dd>  <p>32-bit sample coverage. The default value is 0xffffffff. See remarks.</p> </dd></param>	
        /// <remarks>	
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476462</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>	
        public void SetBlendState(BlendState blendState, Color4 blendFactor, int multiSampleMask = -1)
        {
            if (blendState == null)
            {
                OutputMergerStage.SetBlendState(null, blendFactor, multiSampleMask);
            }
            else
            {
                OutputMergerStage.SetBlendState(blendState, blendFactor, multiSampleMask);
            }
        }

        /// <summary>	
        /// <p>Sets the blend state of the output-merger stage.</p>	
        /// </summary>	
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <param name="blendFactor"><dd>  <p>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <strong><see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/></strong> option.</p> </dd></param>	
        /// <param name="multiSampleMask"><dd>  <p>32-bit sample coverage. The default value is 0xffffffff. See remarks.</p> </dd></param>	
        /// <remarks>	
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476462</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>	
        public void SetBlendState(BlendState blendState, Color4 blendFactor, uint multiSampleMask = 0xFFFFFFFF)
        {
            SetBlendState(blendState, blendFactor, unchecked((int)multiSampleMask));
        }

        /// <summary>	
        /// Sets the depth-stencil state of the output-merger stage.
        /// </summary>	
        /// <param name="depthStencilState"><dd>  <p>Pointer to a depth-stencil state interface (see <strong><see cref="SharpDX.Direct3D11.DepthStencilState"/></strong>) to bind to the device. Set this to <strong><c>null</c></strong> to use the default state listed in <strong><see cref="SharpDX.Direct3D11.DepthStencilStateDescription"/></strong>.</p> </dd></param>	
        /// <param name="stencilReference"><dd>  <p>Reference value to perform against when doing a depth-stencil test. See remarks.</p> </dd></param>	
        /// <remarks>	
        /// <p>To create a depth-stencil state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateDepthStencilState"/></strong>.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476463</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetDepthStencilState([In, Optional] ID3D11DepthStencilState* pDepthStencilState,[In] unsigned int StencilRef)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetDepthStencilState</unmanaged-short>	
        public void SetDepthStencilState(DepthStencilState depthStencilState, int stencilReference = 0)
        {
            OutputMergerStage.SetDepthStencilState(depthStencilState, stencilReference);
        }

        /// <summary>	
        /// <p>Sets the <strong>rasterizer state</strong> for the rasterizer stage of the pipeline.</p>	
        /// </summary>	
        /// <param name="rasterizerState">The rasterizer state to set on this device.</param>	
        /// <msdn-id>ff476479</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetState([In, Optional] ID3D11RasterizerState* pRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetState</unmanaged-short>	
        public void SetRasterizerState(RasterizerState rasterizerState)
        {
            RasterizerStage.State = rasterizerState;
        }

        /// <summary>
        /// Binds a single scissor rectangle to the rasterizer stage.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <remarks>	
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>	
        /// </remarks>	
        /// <msdn-id>ff476478</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(int left, int top, int right, int bottom)
        {
            RasterizerStage.SetScissorRectangle(left, top, right, bottom);
        }

        /// <summary>
        /// Binds a set of scissor rectangles to the rasterizer stage.
        /// </summary>
        /// <param name = "scissorRectangles">The set of scissor rectangles to bind.</param>
        /// <remarks>	
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>	
        /// </remarks>	
        /// <msdn-id>ff476478</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>	
        public void SetScissorRectangles(params Rectangle[] scissorRectangles)
        {
            RasterizerStage.SetScissorRectangles(scissorRectangles);
        }

        /// <summary>
        /// Gets the main viewport.
        /// </summary>
        /// <value>The main viewport.</value>
        public ViewportF Viewport
        {
            get
            {
                RasterizerStage.GetViewports(viewports);
                return viewports[0];
            }

            set
            {
                SetViewport(value);
            }
        }

        /// <summary>
        /// Gets the viewport.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Returns a viewport bound to a specified render target</returns>
        public ViewportF GetViewport(int index)
        {
            RasterizerStage.GetViewports(viewports);
            return viewports[index];
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="x">The x coordinate of the viewport.</param>
        /// <param name="y">The y coordinate of the viewport.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="minZ">The min Z.</param>
        /// <param name="maxZ">The max Z.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewport(float x, float y, float width, float height, float minZ = 0.0f, float maxZ = 1.0f)
        {
            viewports[0] = new ViewportF(x, y, width, height, minZ, maxZ);
            RasterizerStage.SetViewport(x, y, width, height, minZ, maxZ);
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewport(ViewportF viewport)
        {
            viewports[0] = viewport;
            RasterizerStage.SetViewport(viewport);
        }

        /// <summary>
        /// Binds a set of viewports to the rasterizer stage.
        /// </summary>
        /// <param name = "viewports">The set of viewports to bind.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewports(params ViewportF[] viewports)
        {
            for (int i = 0; i < viewports.Length; i++)
                this.viewports[i] = viewports[i];

            RasterizerStage.SetViewports(this.viewports, viewports.Length);
        }

        /// <summary>
        /// Unbinds all depth-stencil buffer and render targets from the output-merger stage.
        /// </summary>
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void ResetTargets()
        {
            for (int i = 0; i < currentRenderTargetViews.Length; i++)
                currentRenderTargetViews[i] = null;
            actualRenderTargetViewCount = 0;
            currentRenderTargetView = null;
            currentDepthStencilView = null;
            OutputMergerStage.ResetTargets();
        }

        /// <summary>
        /// Gets the render targets currently bound to the <see cref="OutputMergerStage"/> through this <see cref="GraphicsDevice"/>instance.
        /// </summary>
        /// <param name="depthStencilViewRef">The depth stencil view, may ne null.</param>
        /// <returns>An array of <see cref="RenderTargetView"/>.</returns>
        public RenderTargetView[] GetRenderTargets(out DepthStencilView depthStencilViewRef)
        {
            var renderTargets = new RenderTargetView[actualRenderTargetViewCount];
            for (int i = 0; i < actualRenderTargetViewCount; i++)
                renderTargets[i] = currentRenderTargetViews[i];
            depthStencilViewRef = currentDepthStencilView;
            return renderTargets;
        }

        /// <summary>	
        /// <p>Bind one or more render targets atomically and the depth-stencil buffer to the output-merger stage.</p>	
        /// </summary>	
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(params RenderTargetView[] renderTargetViews)
        {
            if (renderTargetViews == null)
            {
                throw new ArgumentNullException("renderTargetViews");
            }

            CommonSetRenderTargets(renderTargetViews);
            currentDepthStencilView = null;
            OutputMergerStage.SetTargets(renderTargetViews);
        }

        /// <summary>	
        /// Binds a single render target to the output-merger stage.
        /// </summary>	
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(RenderTargetView renderTargetView)
        {
            CommonSetRenderTargets(renderTargetView);
            currentDepthStencilView = null;
            OutputMergerStage.SetTargets(renderTargetView);
        }

        /// <summary>
        /// Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
        {
            if (renderTargetViews == null)
            {
                throw new ArgumentNullException("renderTargetViews");
            }

            CommonSetRenderTargets(renderTargetViews);
            currentDepthStencilView = depthStencilView;
            OutputMergerStage.SetTargets(depthStencilView, renderTargetViews);
        }

        /// <summary>
        /// Binds a depth-stencil buffer and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>	
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>	
        /// </remarks>	
        /// <msdn-id>ff476464</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>	
        public void SetRenderTargets(DepthStencilView depthStencilView, RenderTargetView renderTargetView)
        {
            CommonSetRenderTargets(renderTargetView);
            currentDepthStencilView = depthStencilView;
            OutputMergerStage.SetTargets(depthStencilView, renderTargetView);
        }

        /// <summary>
        /// Resets the stream output targets bound to the StreamOutput stage.
        /// </summary>
        /// <msdn-id>ff476484</msdn-id>
        ///   <unmanaged>void ID3D11DeviceContext::SOSetTargets([In] unsigned int NumBuffers,[In, Buffer, Optional] const ID3D11Buffer** ppSOTargets,[In, Buffer, Optional] const unsigned int* pOffsets)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::SOSetTargets</unmanaged-short>
        public void ResetStreamOutputTargets()
        {
            Context.StreamOutput.SetTargets(0, null, null);
        }

        /// <summary>
        /// Sets the stream output targets bound to the StreamOutput stage.
        /// </summary>
        /// <param name="buffer">The buffer to bind on the first stream output slot.</param>
        /// <param name="offsets">The offsets in bytes of the buffer. An offset of -1 will cause the stream output buffer to be appended, continuing after the last location written to the buffer in a previous stream output pass.</param>
        /// <msdn-id>ff476484</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::SOSetTargets([In] unsigned int NumBuffers,[In, Buffer, Optional] const ID3D11Buffer** ppSOTargets,[In, Buffer, Optional] const unsigned int* pOffsets)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::SOSetTargets</unmanaged-short>
        public unsafe void SetStreamOutputTarget(Buffer buffer, int offsets = -1)
        {
            Context.StreamOutput.SetTarget(buffer, offsets);
        }

        /// <summary>
        /// Sets the stream output targets bound to the StreamOutput stage.
        /// </summary>
        /// <param name="buffers">The buffers.</param>
        /// <msdn-id>ff476484</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::SOSetTargets([In] unsigned int NumBuffers,[In, Buffer, Optional] const ID3D11Buffer** ppSOTargets,[In, Buffer, Optional] const unsigned int* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::SOSetTargets</unmanaged-short>	
        public void SetStreamOutputTargets(params StreamOutputBufferBinding[] buffers)
        {
            Context.StreamOutput.SetTargets(buffers);
        }

        /// <summary>	
        /// <p>Bind an index buffer to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="indexBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> object, that contains indices. The index buffer must have been created with  the <strong><see cref="SharpDX.Direct3D11.BindFlags.IndexBuffer"/></strong> flag.</p> </dd></param>	
        /// <param name="is32Bit">Set to true if indices are 32-bit values (integer size) or false if they are 16-bit values (short size)</param>	
        /// <param name="offset">Offset (in bytes) from the start of the index buffer to the first index to use. Default to 0</param>	
        /// <remarks>	
        /// <p>For information about creating index buffers, see How to: Create an Index Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind  <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will  not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.</p>	
        /// </remarks>	
        /// <msdn-id>ff476453</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetIndexBuffer([In, Optional] ID3D11Buffer* pIndexBuffer,[In] DXGI_FORMAT Format,[In] unsigned int Offset)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetIndexBuffer</unmanaged-short>	
        public void SetIndexBuffer(Buffer indexBuffer, bool is32Bit, int offset = 0)
        {
            InputAssemblerStage.SetIndexBuffer(indexBuffer, is32Bit ? DXGI.Format.R32_UInt : DXGI.Format.R16_UInt, offset);
        }

        /// <summary>
        /// Sets the vertex input layout.
        /// </summary>
        /// <param name="inputLayout">The input layout.</param>
        /// <msdn-id>ff476454</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetInputLayout([In, Optional] ID3D11InputLayout* pInputLayout)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetInputLayout</unmanaged-short>	
        public void SetVertexInputLayout(VertexInputLayout inputLayout)
        {
            currentVertexInputLayout = inputLayout;
        }

        /// <summary>
        /// Bind a vertex buffer on the slot #0 of the input-assembler stage.
        /// </summary>	
        /// <param name="vertexBuffer">The vertex buffer to bind to this slot. This vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</param>	
        /// <param name="vertexIndex">The index is the number of vertex element between the first element of a vertex buffer and the first element that will be used.</param>	
        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void SetVertexBuffer<T>(Buffer<T> vertexBuffer, int vertexIndex = 0) where T : struct
        {
            SetVertexBuffer(0, vertexBuffer, vertexIndex);
        }

        /// <summary>
        /// Bind a vertex buffer to the input-assembler stage.
        /// </summary>	
        /// <param name="slot">The first input slot for binding.</param>	
        /// <param name="vertexBuffer">The vertex buffer to bind to this slot. This vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</param>	
        /// <param name="vertexIndex">The index is the number of vertex element between the first element of a vertex buffer and the first element that will be used.</param>	
        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.</p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public unsafe void SetVertexBuffer<T>(int slot, Buffer<T> vertexBuffer, int vertexIndex = 0) where T : struct
        {
            IntPtr vertexBufferPtr = IntPtr.Zero;
            int stride = Utilities.SizeOf<T>();
            int offset = vertexIndex * stride;
            if (vertexBuffer != null)
            {
                vertexBufferPtr = ((Direct3D11.Buffer)vertexBuffer).NativePointer;

                // Update the index of the last slot buffer bounded, used by ResetVertexBuffers
                if ((slot + 1) > maxSlotCountForVertexBuffer)
                    maxSlotCountForVertexBuffer = slot + 1;
            }
            InputAssemblerStage.SetVertexBuffers(slot, 1, new IntPtr(&vertexBufferPtr), new IntPtr(&stride), new IntPtr(&offset));
        }

        /// <summary>
        /// <p>Bind a vertex buffer to the input-assembler stage.</p>	
        /// </summary>	
        /// <param name="slot">The first input slot for binding.</param>	
        /// <param name="vertexBuffer">The vertex buffer to bind to this slot. This vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</param>	
        /// <param name="vertexStride">The vertexStride is the size (in bytes) of the elements that are to be used from that vertex buffer.</param>	
        /// <param name="offsetInBytes">The offset is the number of bytes between the first element of a vertex buffer and the first element that will be used.</param>	
        /// <remarks>	
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.</p>	
        /// </remarks>	
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public unsafe void SetVertexBuffer(int slot, SharpDX.Direct3D11.Buffer vertexBuffer, int vertexStride, int offsetInBytes = 0)
        {
            IntPtr vertexBufferPtr = IntPtr.Zero;
            if (vertexBuffer != null)
            {
                vertexBufferPtr = vertexBuffer.NativePointer;

                // Update the index of the last slot buffer bounded, used by ResetVertexBuffers
                if ((slot + 1) > maxSlotCountForVertexBuffer)
                    maxSlotCountForVertexBuffer = slot + 1;
            }
            InputAssemblerStage.SetVertexBuffers(slot, 1, new IntPtr(&vertexBufferPtr), new IntPtr(&vertexStride), new IntPtr(&offsetInBytes));
        }

        /// <summary>
        /// Resets all vertex buffers bounded to a slot range. By default, It clears all the bounded buffers. See remarks.
        /// </summary>	
        /// <remarks>
        /// This is sometimes required to unding explicitly vertex buffers bounding to the input shader assembly, when a
        /// vertex buffer is used as the output of the pipeline.
        /// </remarks>
        /// <msdn-id>ff476456</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>	
        public void ResetVertexBuffers()
        {
            if (maxSlotCountForVertexBuffer == 0)
                return;

            InputAssemblerStage.SetVertexBuffers(0, maxSlotCountForVertexBuffer, ResetSlotsPointers, ResetSlotsPointers, ResetSlotsPointers);

            maxSlotCountForVertexBuffer = 0;
        }

        /// <summary>
        /// Presents the Backbuffer to the screen.
        /// </summary>
        /// <remarks>
        /// This method is only working if a <see cref="GraphicsPresenter"/> is set on this device using <see cref="Presenter"/> property.
        /// </remarks>
        /// <msdn-id>bb174576</msdn-id>	
        /// <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>	
        /// <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>	
        public void Present()
        {
            if (IsDeferred)
                throw new InvalidOperationException("Cannot use Present on a deferred context");

            if (Presenter != null)
            {
                try
                {
                    Presenter.Present();
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode == DXGI.ResultCode.DeviceReset || ex.ResultCode == DXGI.ResultCode.DeviceRemoved)
                    {
                        // TODO: Implement device reset / removed
                    }
                    throw;
                }

            }
        }

        /// <summary>
        /// Remove all shaders bounded to each stage.
        /// </summary>
        public void ResetShaderStages()
        {
            foreach (var commonShaderStage in ShaderStages)
            {
                commonShaderStage.SetShader(null, null, 0);
            }
        }

        public static implicit operator Device(GraphicsDevice from)
        {
            return from == null ? null : from.Device;
        }

        public static implicit operator DeviceContext(GraphicsDevice from)
        {
            return from == null ? null : from.Context;
        }

        private void SetupInputLayout()
        {
            if (CurrentPass == null)
                throw new InvalidOperationException("Cannot perform a Draw/Dispatch operation without an EffectPass applied.");

            var inputLayout = CurrentPass.GetInputLayout(currentVertexInputLayout);
            InputAssemblerStage.SetInputLayout(inputLayout);
        }

        /// <summary>
        /// A delegate called to create shareable data. See remarks.
        /// </summary>
        /// <typeparam name="T">Type of the data to create.</typeparam>
        /// <returns>A new instance of the data to share.</returns>
        /// <remarks>
        /// Because this method is being called from a lock region, this method should not be time consuming.
        /// </remarks>
        public delegate T CreateSharedData<out T>() where T : IDisposable;

        /// <summary>
        /// Gets a shared data for this device context with a delegate to create the shared data if it is not present.
        /// </summary>
        /// <typeparam name="T">Type of the shared data to get/create.</typeparam>
        /// <param name="type">Type of the data to share.</param>
        /// <param name="key">The key of the shared data.</param>
        /// <param name="sharedDataCreator">The shared data creator.</param>
        /// <returns>An instance of the shared data. The shared data will be disposed by this <see cref="GraphicsDevice"/> instance.</returns>
        public T GetOrCreateSharedData<T>(SharedDataType type, object key, CreateSharedData<T> sharedDataCreator) where T : IDisposable
        {
            var dictionary = (type == SharedDataType.PerDevice) ? sharedDataPerDevice : sharedDataPerDeviceContext;

            lock (dictionary)
            {
                object localValue;
                if (!dictionary.TryGetValue(key, out localValue))
                {
                    localValue = ToDispose(sharedDataCreator());
                    dictionary.Add(key, localValue);
                }
                return (T)localValue;
            }
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (Presenter != null)
                {

                    // Invalid for WinRT - throwing a "Value does not fall within the expected range" Exception
#if !WIN8METRO
                    // Make sure that the Presenter is reverted to window before shutting down
                    // otherwise the Direct3D11.Device will generate an exception on Dispose()
                    Presenter.IsFullScreen = false;
#endif
                    Presenter.Dispose();
                    Presenter = null;
                }

                // effect pools will be disposed only by the master graphics device
                if(!IsDeferred)
                {
                    // dispose EffectPools in reverse order as they will remove themselves from the list
                    for(var i = EffectPools.Count - 1; i >= 0; i--)
                    {
                        EffectPools[i].Dispose();
                    }

                    EffectPools = null;
                }
            }

            base.Dispose(disposeManagedResources);
        }


        /// <summary>
        /// Gets or create an input signature manager for a particular signature.
        /// </summary>
        /// <param name="signatureBytecode">The signature bytecode.</param>
        /// <param name="signatureHashcode">The signature hashcode.</param>
        /// <returns></returns>
        internal InputSignatureManager GetOrCreateInputSignatureManager(byte[] signatureBytecode, int signatureHashcode)
        {
            var key = new InputSignatureKey(signatureBytecode, signatureHashcode);

            InputSignatureManager signatureManager;

            // Lock all input signatures, as they are shared between all shaders/graphics device instances.
            lock (inputSignatureCache)
            {
                if (!inputSignatureCache.TryGetValue(key, out signatureManager))
                {
                    signatureManager = ToDispose(new InputSignatureManager(this, signatureBytecode));
                    inputSignatureCache.Add(key, signatureManager);
                }
            }

            return signatureManager;
        }

        private void CommonSetRenderTargets(RenderTargetView rtv)
        {
            currentRenderTargetViews[0] = rtv;
            for (int i = 1; i < actualRenderTargetViewCount; i++)
                currentRenderTargetViews[i] = null;
            actualRenderTargetViewCount = 1;
            currentRenderTargetView = rtv;

            // Setup the viewport from the rendertarget view
            TextureView textureView;
            if (AutoViewportFromRenderTargets && rtv != null && (textureView = rtv.Tag as TextureView) != null)
            {
                SetViewport(new ViewportF(0, 0, textureView.Width, textureView.Height));
            }
        }

        private void CommonSetRenderTargets(RenderTargetView[] rtvs)
        {
            var rtv0 = rtvs.Length > 0 ? rtvs[0] : null;
            for (int i = 0; i < rtvs.Length; i++)
                currentRenderTargetViews[i] = rtvs[i];
            for (int i = rtvs.Length; i < actualRenderTargetViewCount; i++)
                currentRenderTargetViews[i] = null;
            actualRenderTargetViewCount = rtvs.Length;
            currentRenderTargetView = rtv0;

            // Setup the viewport from the rendertarget view
            TextureView textureView;
            if (AutoViewportFromRenderTargets && rtv0 != null && (textureView = rtv0.Tag as TextureView) != null)
            {
                SetViewport(new ViewportF(0, 0, textureView.Width, textureView.Height));
            }
        }
    }
}
