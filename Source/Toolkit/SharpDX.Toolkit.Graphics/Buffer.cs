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
using System.Collections.Generic;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// All-in-One Buffer class linked <see cref="SharpDX.Direct3D11.Buffer"/>.
    /// </summary>
    /// <remarks>
    /// This class is able to create constant buffers, index buffers, vertex buffers, structured buffer, raw buffers, argument buffers.
    /// </remarks>
    /// <msdn-id>ff476351</msdn-id>	
    /// <unmanaged>ID3D11Buffer</unmanaged>	
    /// <unmanaged-short>ID3D11Buffer</unmanaged-short>	
    public partial class Buffer : GraphicsResource
    {
        private readonly Dictionary<DXGI.Format, ShaderResourceView> shaderResourceViews = new Dictionary<Format, ShaderResourceView>();

        private readonly Dictionary<RenderTargetKey, RenderTargetView> renderTargetViews = new Dictionary<RenderTargetKey, RenderTargetView>();

        private ShaderResourceView shaderResourceView;

        private UnorderedAccessView unorderedAccessView;

        /// <summary>
        /// Gets the description of this buffer.
        /// </summary>
        public readonly BufferDescription Description;

        /// <summary>
        /// Gets the number of elements.
        /// </summary>
        /// <remarks>
        /// This value is valid for structured buffers, raw buffers and index buffers that are used as a SharedResourceView.
        /// </remarks>
        public readonly int Count;

        /// <summary>
        /// Gets the type of this buffer.
        /// </summary>
        public readonly BufferFlags BufferFlags;

        /// <summary>
        /// Gets the default view format of this buffer.
        /// </summary>
        public readonly PixelFormat ViewFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffer" /> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="bufferFlags">Type of the buffer.</param>
        /// <param name="viewFormat">The view format.</param>
        /// <param name="dataPointer">The data pointer.</param>
        private Buffer(BufferDescription description, BufferFlags bufferFlags, PixelFormat viewFormat, IntPtr dataPointer)
            : this(GraphicsDevice.Current, description, bufferFlags, viewFormat, dataPointer )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffer" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        /// <param name="bufferFlags">Type of the buffer.</param>
        /// <param name="viewFormat">The view format.</param>
        /// <param name="dataPointer">The data pointer.</param>
        private Buffer(GraphicsDevice deviceLocal, BufferDescription description, BufferFlags bufferFlags, PixelFormat viewFormat, IntPtr dataPointer)
        {
            Description = description;
            BufferFlags = bufferFlags;
            ViewFormat = viewFormat;
            InitCountAndViewFormat(out Count, ref ViewFormat);
            Initialize(deviceLocal, new Direct3D11.Buffer(deviceLocal, dataPointer, Description));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buffer" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeBuffer">The native buffer.</param>
        /// <param name="bufferFlags">Type of the buffer.</param>
        /// <param name="viewFormat">The view format.</param>
        private Buffer(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer, BufferFlags bufferFlags, PixelFormat viewFormat)
        {
            Description = nativeBuffer.Description;
            BufferFlags = bufferFlags;
            ViewFormat = viewFormat;
            InitCountAndViewFormat(out Count, ref ViewFormat);
            Initialize(deviceLocal, nativeBuffer);
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns>A new instance of this buffer as a staging resource</returns>
        public Buffer ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read | CpuAccessFlags.Write;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Buffer(GraphicsDevice, stagingDesc, BufferFlags, ViewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance</returns>
        /// <remarks>
        /// This method will not copy the content of the buffer to the clone
        /// </remarks>
        public Buffer Clone()
        {
            return new Buffer(GraphicsDevice, Description, BufferFlags, ViewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Gets a <see cref="ShaderResourceView"/> for a particular <see cref="PixelFormat"/>.
        /// </summary>
        /// <param name="viewFormat">The view format.</param>
        /// <returns>A <see cref="ShaderResourceView"/> for the particular view format.</returns>
        /// <remarks>
        /// The buffer must have been declared with <see cref="Graphics.BufferFlags.ShaderResource"/>. 
        /// The ShaderResourceView instance is kept by this buffer and will be disposed when this buffer is disposed.
        /// </remarks>
        /// <msdn-id>ff476519</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateShaderResourceView([In] ID3D11Resource* pResource,[In, Optional] const D3D11_SHADER_RESOURCE_VIEW_DESC* pDesc,[Out, Fast] ID3D11ShaderResourceView** ppSRView)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateShaderResourceView</unmanaged-short>	
        public ShaderResourceView GetShaderResourceView(PixelFormat viewFormat)
        {
            ShaderResourceView srv = null;
            if ((Description.BindFlags & BindFlags.ShaderResource) != 0)
            {
                lock (shaderResourceViews)
                {
                    if (!shaderResourceViews.TryGetValue(viewFormat, out srv))
                    {
                        var description = new ShaderResourceViewDescription
                        {
                            Format = viewFormat,
                            Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                            BufferEx =
                            {
                                ElementCount = Count,
                                FirstElement = 0,
                                Flags = ShaderResourceViewExtendedBufferFlags.None
                            }
                        };

                        if (((BufferFlags & BufferFlags.RawBuffer) == BufferFlags.RawBuffer))
                            description.BufferEx.Flags |= ShaderResourceViewExtendedBufferFlags.Raw;

                        srv = ToDispose(new ShaderResourceView(this.GraphicsDevice, this.Resource, description));
                    }
                }
            }
            return srv;
        }

        /// <summary>
        /// Gets a <see cref="RenderTargetView" /> for a particular <see cref="PixelFormat" />.
        /// </summary>
        /// <param name="pixelFormat">The view format.</param>
        /// <param name="width">The width in pixels of the render target.</param>
        /// <returns>A <see cref="RenderTargetView" /> for the particular view format.</returns>
        /// <remarks>The buffer must have been declared with <see cref="Graphics.BufferFlags.RenderTarget" />.
        /// The RenderTargetView instance is kept by this buffer and will be disposed when this buffer is disposed.</remarks>
        public RenderTargetView GetRenderTargetView(PixelFormat pixelFormat, int width)
        {
            RenderTargetView srv = null;
            if ((Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                lock (renderTargetViews)
                {
                    var renderTargetKey = new RenderTargetKey(pixelFormat, width);

                    if (!renderTargetViews.TryGetValue(renderTargetKey, out srv))
                    {
                        var description = new RenderTargetViewDescription()
                        {
                            Format = pixelFormat,
                            Dimension = RenderTargetViewDimension.Buffer,
                            Buffer =
                            {
                                ElementWidth = pixelFormat.SizeInBytes * width,
                                ElementOffset = 0
                            }
                        };

                        srv = ToDispose(new RenderTargetView(this.GraphicsDevice, this.Resource, description));
                    }
                }
            }
            return srv;
        }

        /// <summary>
        /// Creates a new <see cref="Buffer"/> instance.
        /// </summary>
        /// <param name="buffer">The original buffer to duplicate the definition from.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <returns>An instance of a new <see cref="Buffer"/></returns>
        /// <msdn-id>ff476501</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>	
        public static Buffer New(Buffer buffer, DXGI.Format viewFormat = SharpDX.DXGI.Format.Unknown)
        {
            var bufferType = GetBufferFlagsFromDescription(buffer.Description);
            return new Buffer(GraphicsDevice.Current, buffer, bufferType, viewFormat);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="description">The description of the buffer.</param>
        /// <param name="viewFormat">View format used if the buffer is used as a shared resource view.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(BufferDescription description, DXGI.Format viewFormat = SharpDX.DXGI.Format.Unknown)
        {
            var bufferType = GetBufferFlagsFromDescription(description);
            return new Buffer(description, bufferType, viewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(int bufferSize, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(bufferSize, 0, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(int bufferSize, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(bufferSize, 0, bufferFlags, viewFormat, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="elementSize">Size of an element in the buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(int bufferSize, int elementSize, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(bufferSize, elementSize, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer in bytes.</param>
        /// <param name="elementSize">Size of an element in the buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(int bufferSize, int elementSize, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default)
        {
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);
            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(description, bufferFlags, viewFormat, IntPtr.Zero);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="value">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New<T>(ref T value, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            return New(ref value, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="value">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public unsafe static Buffer New<T>(ref T value, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            int bufferSize = Utilities.SizeOf<T>();
            int elementSize = ((bufferFlags & BufferFlags.StructuredBuffer) != 0) ? Utilities.SizeOf<T>() : 0;

            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);

            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(description, bufferFlags, viewFormat, (IntPtr)Interop.Fixed(ref value));
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="initialValue">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New<T>(T[] initialValue, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            return New(initialValue, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <typeparam name="T">Type of the buffer, to get the sizeof from.</typeparam>
        /// <param name="initialValue">The initial value of this buffer.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public unsafe static Buffer New<T>(T[] initialValue, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default) where T : struct
        {
            int bufferSize = Utilities.SizeOf<T>() * initialValue.Length;
            int elementSize = Utilities.SizeOf<T>();
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);

            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(description, bufferFlags, viewFormat, (IntPtr)Interop.Fixed(initialValue));
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="elementSize">Size of the element.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(DataPointer dataPointer, int elementSize, BufferFlags bufferFlags, ResourceUsage usage = ResourceUsage.Default)
        {
            return New(dataPointer, elementSize, bufferFlags, PixelFormat.Unknown, usage);
        }

        /// <summary>
        /// Creates a new <see cref="Buffer" /> instance.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        /// <param name="elementSize">Size of the element.</param>
        /// <param name="bufferFlags">The buffer flags to specify the type of buffer.</param>
        /// <param name="viewFormat">The view format must be specified if the buffer is declared as a shared resource view.</param>
        /// <param name="usage">The usage.</param>
        /// <returns>An instance of a new <see cref="Buffer" /></returns>
        /// <msdn-id>ff476501</msdn-id>
        /// <unmanaged>HRESULT ID3D11Device::CreateBuffer([In] const D3D11_BUFFER_DESC* pDesc,[In, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Buffer** ppBuffer)</unmanaged>
        /// <unmanaged-short>ID3D11Device::CreateBuffer</unmanaged-short>
        public static Buffer New(DataPointer dataPointer, int elementSize, BufferFlags bufferFlags, DXGI.Format viewFormat, ResourceUsage usage = ResourceUsage.Default)
        {
            int bufferSize = dataPointer.Size;
            viewFormat = CheckPixelFormat(bufferFlags, elementSize, viewFormat);
            var description = NewDescription(bufferSize, elementSize, bufferFlags, usage);
            return new Buffer(description, bufferFlags, viewFormat, dataPointer.Pointer);
        }

        protected override void OnNameChanged()
        {
            base.OnNameChanged();
            if (GraphicsDevice.IsDebugMode)
            {
                if (shaderResourceView != null)
                    shaderResourceView.DebugName = Name == null ? null : string.Format("{0} SRV", Name);

                if (unorderedAccessView != null)
                    unorderedAccessView.DebugName = Name == null ? null : string.Format("{0} UAV", Name);
            }
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            if (disposeManagedResources)
            {
                // Clear all views
                shaderResourceView = null;
                unorderedAccessView = null;
                renderTargetViews.Clear();
                shaderResourceViews.Clear();
            }
        }

        /// <summary>
        /// Initializes the specified device arg.
        /// </summary>
        /// <param name="deviceArg">The device arg.</param>
        /// <param name="resource">The resource.</param>
        private void Initialize(GraphicsDevice deviceArg, Direct3D11.Buffer resource)
        {
            base.Initialize(deviceArg, resource);

            // Staging resource don't have any views
            if (Description.Usage != ResourceUsage.Staging)
                this.InitializeViews();
        }

        private void InitCountAndViewFormat(out int count, ref PixelFormat viewFormat)
        {
            if (Description.StructureByteStride == 0)
            {
                if ((BufferFlags & BufferFlags.RawBuffer) != 0)
                    count = Description.SizeInBytes / sizeof(int);
                else if ((BufferFlags & BufferFlags.IndexBuffer) != 0)
                {
                    count = (BufferFlags & BufferFlags.ShaderResource) != 0 ? Description.SizeInBytes / ViewFormat.SizeInBytes : 0;
                }
                else
                    count = 1;
            }
            else
            {
                // For structured buffer
                count = Description.SizeInBytes / Description.StructureByteStride;
                viewFormat = PixelFormat.Unknown;
            }
        }

        private static BufferFlags GetBufferFlagsFromDescription(BufferDescription description)
        {
            var bufferType = (BufferFlags)0;

            if ((description.BindFlags & BindFlags.ConstantBuffer) != 0)
                bufferType |= BufferFlags.ConstantBuffer;

            if ((description.BindFlags & BindFlags.IndexBuffer) != 0)
                bufferType |= BufferFlags.IndexBuffer;

            if ((description.BindFlags & BindFlags.VertexBuffer) != 0)
                bufferType |= BufferFlags.VertexBuffer;

            if ((description.BindFlags & BindFlags.UnorderedAccess) != 0)
                bufferType |= BufferFlags.UnorderedAccess;

            if ((description.BindFlags & BindFlags.RenderTarget) != 0)
                bufferType |= BufferFlags.RenderTarget;

            if ((description.OptionFlags & ResourceOptionFlags.BufferStructured) != 0)
                bufferType |= BufferFlags.StructuredBuffer;

            if ((description.OptionFlags & ResourceOptionFlags.BufferAllowRawViews) != 0)
                bufferType |= BufferFlags.RawBuffer;

            if ((description.OptionFlags & ResourceOptionFlags.DrawindirectArgs) != 0)
                bufferType |= BufferFlags.ArgumentBuffer;

            return bufferType;
        }
        
        private static PixelFormat CheckPixelFormat(BufferFlags bufferFlags, int elementSize, PixelFormat viewFormat)
        {
            if ((bufferFlags & BufferFlags.IndexBuffer) != 0 && (bufferFlags & BufferFlags.ShaderResource) != 0)
            {
                if (elementSize != 2 && elementSize != 4)
                    throw new ArgumentException("Element size must be set to sizeof(short) = 2 or sizeof(int) = 4 for index buffer if index buffer is bound to a ShaderResource", "elementSize");

                viewFormat = elementSize == 2 ? PixelFormat.R16.UInt: PixelFormat.R32.UInt;
            }
            return viewFormat;
        }

        private static BufferDescription NewDescription(int bufferSize, int elementSize, BufferFlags bufferFlags, ResourceUsage usage)
        {
            var desc = new BufferDescription()
            {
                SizeInBytes = bufferSize,
                StructureByteStride = (bufferFlags & BufferFlags.StructuredBuffer) != 0 ? elementSize : 0,
                CpuAccessFlags = GetCputAccessFlagsFromUsage(usage),
                BindFlags = BindFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                Usage = usage,
            };

            if ((bufferFlags & BufferFlags.ConstantBuffer) != 0)
                desc.BindFlags |= BindFlags.ConstantBuffer;

            if ((bufferFlags & BufferFlags.IndexBuffer) != 0)
                desc.BindFlags |= BindFlags.IndexBuffer;

            if ((bufferFlags & BufferFlags.VertexBuffer) != 0)
                desc.BindFlags |= BindFlags.VertexBuffer;

            if ((bufferFlags & BufferFlags.RenderTarget) != 0)
                desc.BindFlags |= BindFlags.RenderTarget;

            if ((bufferFlags & BufferFlags.ShaderResource) != 0)
                desc.BindFlags |= BindFlags.ShaderResource;

            if ((bufferFlags & BufferFlags.UnorderedAccess) != 0)
                desc.BindFlags |= BindFlags.UnorderedAccess;

            if ((bufferFlags & BufferFlags.StructuredBuffer) != 0)
            {
                desc.OptionFlags |= ResourceOptionFlags.BufferStructured;
                if (elementSize == 0)
                    throw new ArgumentException("Element size cannot be set to 0 for structured buffer");
            }

            if ((bufferFlags & BufferFlags.RawBuffer) == BufferFlags.RawBuffer)
                desc.OptionFlags |= ResourceOptionFlags.BufferAllowRawViews;

            if ((bufferFlags & BufferFlags.ArgumentBuffer) == BufferFlags.ArgumentBuffer)
                desc.OptionFlags |= ResourceOptionFlags.DrawindirectArgs;

            return desc;
        }

        /// <summary>
        /// Initializes the views.
        /// </summary>
        private void InitializeViews()
        {
            var bindFlags = Description.BindFlags;

            var srvFormat = ViewFormat;
            var uavFormat = ViewFormat;
            
            if (((BufferFlags & BufferFlags.RawBuffer) != 0))
            {
                srvFormat = PixelFormat.R32.Typeless;
                uavFormat = PixelFormat.R32.Typeless;
            }

            if ((bindFlags & BindFlags.ShaderResource) != 0)
            {
                this.shaderResourceView = GetShaderResourceView(srvFormat);
            }

            if ((bindFlags & BindFlags.UnorderedAccess) != 0)
            {
                var description = new UnorderedAccessViewDescription()
                {
                    Format = uavFormat,
                    Dimension = UnorderedAccessViewDimension.Buffer,
                    Buffer =
                    {
                        ElementCount = Count,
                        FirstElement = 0,
                        Flags = UnorderedAccessViewBufferFlags.None
                    },
                };

                if (((BufferFlags & BufferFlags.RawBuffer) == BufferFlags.RawBuffer))
                    description.Buffer.Flags |= UnorderedAccessViewBufferFlags.Raw;

                if (((BufferFlags & BufferFlags.StructuredAppendBuffer) == BufferFlags.StructuredAppendBuffer))
                    description.Buffer.Flags |= UnorderedAccessViewBufferFlags.Append;

                if (((BufferFlags & BufferFlags.StructuredCounterBuffer) == BufferFlags.StructuredCounterBuffer))
                    description.Buffer.Flags |= UnorderedAccessViewBufferFlags.Counter;

                this.unorderedAccessView = ToDispose(new UnorderedAccessView(this.GraphicsDevice, this.Resource, description));
            }
        }
        
        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Direct3D11.Resource(Buffer from)
        {
            return from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator Direct3D11.Buffer(Buffer from)
        {
            return (Direct3D11.Buffer)from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator ShaderResourceView(Buffer from)
        {
            return from.shaderResourceView;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator UnorderedAccessView(Buffer from)
        {
            return from.unorderedAccessView;
        }        

        private struct RenderTargetKey : IEquatable<RenderTargetKey>
        {
            public RenderTargetKey(Format pixelFormat, int width)
            {
                PixelFormat = pixelFormat;
                Width = width;
            }

            public DXGI.Format PixelFormat;

            public int Width;

            public bool Equals(RenderTargetKey other)
            {
                return PixelFormat.Equals(other.PixelFormat) && Width == other.Width;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is RenderTargetKey && Equals((RenderTargetKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (PixelFormat.GetHashCode() * 397) ^ Width;
                }
            }

            public static bool operator ==(RenderTargetKey left, RenderTargetKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(RenderTargetKey left, RenderTargetKey right)
            {
                return !left.Equals(right);
            }
        }
    }
}