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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Flags of a buffer.
    /// </summary>
    [Flags]
    public enum BufferFlags
    {
        /// <summary>
        /// Creates a none buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.None"/>.
        /// </remarks>
        None = 0,

        /// <summary>
        /// Creates a constant buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.ConstantBuffer"/>.
        /// </remarks>
        /// <msdn-id>ff476085</msdn-id>	
        /// <unmanaged>D3D11_BIND_CONSTANT_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D11_BIND_CONSTANT_BUFFER</unmanaged-short>	
        ConstantBuffer = 1,

        /// <summary>
        /// Creates an index buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.IndexBuffer"/>.
        /// </remarks>
        /// <msdn-id>ff476085</msdn-id>	
        /// <unmanaged>D3D11_BIND_INDEX_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D11_BIND_INDEX_BUFFER</unmanaged-short>	
        IndexBuffer = 2,

        /// <summary>
        /// Creates a vertex buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.VertexBuffer"/>.
        /// </remarks>
        /// <msdn-id>ff476085</msdn-id>	
        /// <unmanaged>D3D11_BIND_VERTEX_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D11_BIND_VERTEX_BUFFER</unmanaged-short>	
        VertexBuffer = 4,

        /// <summary>
        /// Creates a render target buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.RenderTarget"/>.
        /// </remarks>
        /// <msdn-id>ff476085</msdn-id>	
        /// <unmanaged>D3D11_BIND_RENDER_TARGET</unmanaged>	
        /// <unmanaged-short>D3D11_BIND_RENDER_TARGET</unmanaged-short>	
        RenderTarget = 8,

        /// <summary>
        /// Creates a buffer usable as a <see cref="ShaderResourceView"/>.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.ShaderResource"/>.
        /// </remarks>
        /// <msdn-id>ff476085</msdn-id>	
        /// <unmanaged>D3D11_BIND_SHADER_RESOURCE</unmanaged>	
        /// <unmanaged-short>D3D11_BIND_SHADER_RESOURCE</unmanaged-short>	
        ShaderResource = 16,

        /// <summary>
        /// Creates an unordered access buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="BindFlags.UnorderedAccess"/>.
        /// </remarks>
        /// <msdn-id>ff476085</msdn-id>	
        /// <unmanaged>D3D11_BIND_UNORDERED_ACCESS</unmanaged>	
        /// <unmanaged-short>D3D11_BIND_UNORDERED_ACCESS</unmanaged-short>	
        UnorderedAccess = 32,

        /// <summary>
        /// Creates a structured buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="ResourceOptionFlags.BufferStructured"/>.
        /// </remarks>
        /// <msdn-id>ff476203</msdn-id>	
        /// <unmanaged>D3D11_RESOURCE_MISC_BUFFER_STRUCTURED</unmanaged>	
        /// <unmanaged-short>D3D11_RESOURCE_MISC_BUFFER_STRUCTURED</unmanaged-short>	
        StructuredBuffer = 64,

        /// <summary>
        /// Creates a structured buffer that supports unordered acccess and append.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="ResourceOptionFlags.BufferStructured"/>.
        /// </remarks>
        /// <msdn-id>ff476203</msdn-id>	
        /// <unmanaged>D3D11_RESOURCE_MISC_BUFFER_STRUCTURED</unmanaged>	
        /// <unmanaged-short>D3D11_RESOURCE_MISC_BUFFER_STRUCTURED</unmanaged-short>	
        StructuredAppendBuffer = UnorderedAccess | StructuredBuffer | 128,

        /// <summary>
        /// Creates a structured buffer that supports unordered acccess and counter.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="ResourceOptionFlags.BufferStructured"/>.
        /// </remarks>
        /// <msdn-id>ff476203</msdn-id>	
        /// <unmanaged>D3D11_RESOURCE_MISC_BUFFER_STRUCTURED</unmanaged>	
        /// <unmanaged-short>D3D11_RESOURCE_MISC_BUFFER_STRUCTURED</unmanaged-short>	
        StructuredCounterBuffer = UnorderedAccess | StructuredBuffer | 256,

        /// <summary>
        /// Creates a raw buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="ResourceOptionFlags.BufferAllowRawViews"/> and <see cref="UnorderedAccessViewBufferFlags.Raw"/>.
        /// </remarks>
        /// <msdn-id>ff476203</msdn-id>	
        /// <unmanaged>D3D11_RESOURCE_MISC_BUFFER_ALLOW_RAW_VIEWS</unmanaged>	
        /// <unmanaged-short>D3D11_RESOURCE_MISC_BUFFER_ALLOW_RAW_VIEWS</unmanaged-short>	
        RawBuffer = 512,

        /// <summary>
        /// Creates an indirect arguments buffer.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="ResourceOptionFlags.DrawindirectArgs"/>.
        /// </remarks>
        /// <msdn-id>ff476203</msdn-id>	
        /// <unmanaged>D3D11_RESOURCE_MISC_DRAWINDIRECT_ARGS</unmanaged>	
        /// <unmanaged-short>D3D11_RESOURCE_MISC_DRAWINDIRECT_ARGS</unmanaged-short>	
        ArgumentBuffer = 1024
    }
}