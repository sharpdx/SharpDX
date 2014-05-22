// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using SharpDX.Direct3D11;

namespace SharpDX.Direct3D12
{
    public partial class PipelineStateDescription
    {
        private ComputeShader computeShader;
        private VertexShader vertexShader;
        private PixelShader pixelShader;
        private DomainShader domainShader;
        private HullShader hullShader;
        private GeometryShader geometryShader;
        private BlendState blendState;
        private RasterizerState rasterizerState;
        private DepthStencilState depthStencilState;
        private InputLayout inputLayout;

        /// <summary>
        /// Gets or sets the compute shader.
        /// </summary>
        /// <value>The compute shader.</value>
        public ComputeShader ComputeShader
        {
            get
            {
                return computeShader;
            }
            set
            {
                computeShader = value;
                ComputeShaderPointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the vertex shader.
        /// </summary>
        /// <value>The vertex shader.</value>
        public VertexShader VertexShader
        {
            get
            {
                return vertexShader;
            }
            set
            {
                vertexShader = value;
                VertexShaderPointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the pixel shader.
        /// </summary>
        /// <value>The pixel shader.</value>
        public PixelShader PixelShader
        {
            get
            {
                return pixelShader;
            }
            set
            {
                pixelShader = value;
                PixelShaderPointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the domain shader.
        /// </summary>
        /// <value>The domain shader.</value>
        public DomainShader DomainShader
        {
            get
            {
                return domainShader;
            }
            set
            {
                domainShader = value;
                DomainShaderPointer = value == null ? IntPtr.Zero : value.NativePointer;
            } 
        }

        /// <summary>
        /// Gets or sets the hull shader.
        /// </summary>
        /// <value>The hull shader.</value>
        public HullShader HullShader
        {
            get
            {
                return hullShader;
            }
            set
            {
                hullShader = value;
                HullShaderPointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the geometry shader.
        /// </summary>
        /// <value>The geometry shader.</value>
        public GeometryShader GeometryShader
        {
            get
            {
                return geometryShader;
            }
            set
            {
                geometryShader = value;
                GeometryShaderPointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the state of the blend.
        /// </summary>
        /// <value>The state of the blend.</value>
        public BlendState BlendState
        {
            get { return blendState; }

            set
            {
                blendState = value;
                BlendStatePointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the state of the rasterizer.
        /// </summary>
        /// <value>The state of the rasterizer.</value>
        public RasterizerState RasterizerState
        {
            get { return rasterizerState; }

            set
            {
                rasterizerState = value;
                RasterizerStatePointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the state of the depth stencil.
        /// </summary>
        /// <value>The state of the depth stencil.</value>
        public DepthStencilState DepthStencilState
        {
            get { return depthStencilState; }

            set
            {
                depthStencilState = value;
                DepthStencilStatePointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }

        /// <summary>
        /// Gets or sets the input layout.
        /// </summary>
        /// <value>The input layout.</value>
        public InputLayout InputLayout
        {
            get { return inputLayout; }
            set
            {
                inputLayout = value;
                InputLayoutPointer = value == null ? IntPtr.Zero : value.NativePointer;
            }
        }
    }
}