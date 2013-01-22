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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>	
    /// <p>Values that identify various data, texture, and buffer types that can be assigned to a shader variable.</p>	
    /// </summary>	
    /// <remarks>	
    /// <p>A call to the <strong><see cref="SharpDX.D3DCompiler.ShaderReflectionType.GetDescription"/></strong> method returns a <strong><see cref="SharpDX.D3DCompiler.ShaderVariableType"/></strong> value in the  <strong>Type</strong> member of a  <strong><see cref="SharpDX.D3DCompiler.ShaderTypeDescription"/></strong> structure.</p><p>The types in a structured buffer describe the structure of the elements in the buffer. The layout of these types generally match their C++ struct counterparts. The following examples show structured buffers:</p><pre><code>struct mystruct {float4 val; uint ind;}; RWStructuredBuffer&lt;mystruct&gt; rwbuf;	
    /// RWStructuredBuffer&lt;float3&gt; rwbuf2;</code></pre>	
    /// </remarks>	
    /// <msdn-id>ff728735</msdn-id>	
    /// <unmanaged>D3D_SHADER_VARIABLE_TYPE</unmanaged>	
    /// <unmanaged-short>D3D_SHADER_VARIABLE_TYPE</unmanaged-short>	
    public enum EffectParameterType : byte
    {
        /// <summary>	
        /// <dd> <p>The variable is a void reference.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_VOID</unmanaged>	
        /// <unmanaged-short>D3D_SVT_VOID</unmanaged-short>	
        Void = unchecked((int)0),			
        
        /// <summary>	
        /// <dd> <p>The variable is a boolean.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_BOOL</unmanaged>	
        /// <unmanaged-short>D3D_SVT_BOOL</unmanaged-short>	
        Bool = unchecked((int)1),			
        
        /// <summary>	
        /// <dd> <p>The variable is an integer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_INT</unmanaged>	
        /// <unmanaged-short>D3D_SVT_INT</unmanaged-short>	
        Int = unchecked((int)2),			
        
        /// <summary>	
        /// <dd> <p>The variable is a floating-point number.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_FLOAT</unmanaged>	
        /// <unmanaged-short>D3D_SVT_FLOAT</unmanaged-short>	
        Float = unchecked((int)3),			
        
        /// <summary>	
        /// <dd> <p>The variable is a string.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_STRING</unmanaged>	
        /// <unmanaged-short>D3D_SVT_STRING</unmanaged-short>	
        String = unchecked((int)4),			
        
        /// <summary>	
        /// <dd> <p>The variable is a texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE</unmanaged-short>	
        Texture = unchecked((int)5),			
        
        /// <summary>	
        /// <dd> <p>The variable is a 1D texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE1D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE1D</unmanaged-short>	
        Texture1D = unchecked((int)6),			
        
        /// <summary>	
        /// <dd> <p>The variable is a 2D texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE2D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE2D</unmanaged-short>	
        Texture2D = unchecked((int)7),			
        
        /// <summary>	
        /// <dd> <p>The variable is a 3D texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE3D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE3D</unmanaged-short>	
        Texture3D = unchecked((int)8),			
        
        /// <summary>	
        /// <dd> <p>The variable is a texture cube.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURECUBE</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURECUBE</unmanaged-short>	
        TextureCube = unchecked((int)9),			
        
        /// <summary>	
        /// <dd> <p>The variable is a sampler.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_SAMPLER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_SAMPLER</unmanaged-short>	
        Sampler = unchecked((int)10),

        /// <summary>	
        /// <dd> <p>The variable is a sampler.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_SAMPLER1D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_SAMPLER1D</unmanaged-short>	
        Sampler1D = unchecked((int)11),

        /// <summary>	
        /// <dd> <p>The variable is a sampler.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_SAMPLER2D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_SAMPLER2D</unmanaged-short>	
        Sampler2D = unchecked((int)12),

        /// <summary>	
        /// <dd> <p>The variable is a sampler.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_SAMPLER3D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_SAMPLER3D</unmanaged-short>	
        Sampler3D = unchecked((int)13),

        /// <summary>	
        /// <dd> <p>The variable is a sampler.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_SAMPLERCUBE</unmanaged>	
        /// <unmanaged-short>D3D_SVT_SAMPLERCUBE</unmanaged-short>	
        SamplerCube = unchecked((int)14),

        /// <summary>	
        /// <dd> <p>The variable is a pixel shader.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_PIXELSHADER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_PIXELSHADER</unmanaged-short>	
        Pixelshader = unchecked((int)15),

        /// <summary>	
        /// <dd> <p>The variable is a vertex shader.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_VERTEXSHADER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_VERTEXSHADER</unmanaged-short>	
        Vertexshader = unchecked((int)16),

        /// <summary>	
        /// <dd> <p>The variable is a pixel shader.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_PIXELFRAGMENT</unmanaged>	
        /// <unmanaged-short>D3D_SVT_PIXELFRAGMENT</unmanaged-short>	
        Pixelfragment = unchecked((int)17),

        /// <summary>	
        /// <dd> <p>The variable is a vertex shader.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_VERTEXFRAGMENT</unmanaged>	
        /// <unmanaged-short>D3D_SVT_VERTEXFRAGMENT</unmanaged-short>	
        Vertexfragment = unchecked((int)18),

        /// <summary>	
        /// <dd> <p>The variable is an unsigned integer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_UINT</unmanaged>	
        /// <unmanaged-short>D3D_SVT_UINT</unmanaged-short>	
        UInt = unchecked((int)19),

        /// <summary>	
        /// <dd> <p>The variable is an 8-bit unsigned integer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_UINT8</unmanaged>	
        /// <unmanaged-short>D3D_SVT_UINT8</unmanaged-short>	
        UInt8 = unchecked((int)20),

        /// <summary>	
        /// <dd> <p>The variable is a geometry shader.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_GEOMETRYSHADER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_GEOMETRYSHADER</unmanaged-short>	
        Geometryshader = unchecked((int)21),

        /// <summary>	
        /// <dd> <p>The variable is a rasterizer-state object.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RASTERIZER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RASTERIZER</unmanaged-short>	
        Rasterizer = unchecked((int)22),

        /// <summary>	
        /// <dd> <p>The variable is a depth-stencil-state object.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_DEPTHSTENCIL</unmanaged>	
        /// <unmanaged-short>D3D_SVT_DEPTHSTENCIL</unmanaged-short>	
        Depthstencil = unchecked((int)23),

        /// <summary>	
        /// <dd> <p>The variable is a blend-state object.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_BLEND</unmanaged>	
        /// <unmanaged-short>D3D_SVT_BLEND</unmanaged-short>	
        Blend = unchecked((int)24),

        /// <summary>	
        /// <dd> <p>The variable is a buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_BUFFER</unmanaged-short>	
        Buffer = unchecked((int)25),

        /// <summary>	
        /// <dd> <p>The variable is a constant buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_CBUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_CBUFFER</unmanaged-short>	
        ConstantBuffer = unchecked((int)26),

        /// <summary>	
        /// <dd> <p>The variable is a texture buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TBUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TBUFFER</unmanaged-short>	
        TextureBuffer = unchecked((int)27),

        /// <summary>	
        /// <dd> <p>The variable is a 1D-texture array.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE1DARRAY</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE1DARRAY</unmanaged-short>	
        Texture1DArray = unchecked((int)28),

        /// <summary>	
        /// <dd> <p>The variable is a 2D-texture array.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE2DARRAY</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE2DARRAY</unmanaged-short>	
        Texture2DArray = unchecked((int)29),

        /// <summary>	
        /// <dd> <p>The variable is a render-target view.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RENDERTARGETVIEW</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RENDERTARGETVIEW</unmanaged-short>	
        Rendertargetview = unchecked((int)30),

        /// <summary>	
        /// <dd> <p>The variable is a depth-stencil view.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_DEPTHSTENCILVIEW</unmanaged>	
        /// <unmanaged-short>D3D_SVT_DEPTHSTENCILVIEW</unmanaged-short>	
        Depthstencilview = unchecked((int)31),

        /// <summary>	
        /// <dd> <p>The variable is a 2D-multisampled texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE2DMS</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE2DMS</unmanaged-short>	
        Texture2DMultisampled = unchecked((int)32),

        /// <summary>	
        /// <dd> <p>The variable is a 2D-multisampled-texture array.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURE2DMSARRAY</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURE2DMSARRAY</unmanaged-short>	
        Texture2DMultisampledArray = unchecked((int)33),

        /// <summary>	
        /// <dd> <p>The variable is a texture-cube array.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_TEXTURECUBEARRAY</unmanaged>	
        /// <unmanaged-short>D3D_SVT_TEXTURECUBEARRAY</unmanaged-short>	
        TextureCubeArray = unchecked((int)34),

        /// <summary>	
        /// <dd> <p>The variable holds a compiled hull-shader binary.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_HULLSHADER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_HULLSHADER</unmanaged-short>	
        Hullshader = unchecked((int)35),

        /// <summary>	
        /// <dd> <p>The variable holds a compiled domain-shader binary.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_DOMAINSHADER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_DOMAINSHADER</unmanaged-short>	
        Domainshader = unchecked((int)36),

        /// <summary>	
        /// <dd> <p>The variable is an interface.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_INTERFACE_POINTER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_INTERFACE_POINTER</unmanaged-short>	
        InterfacePointer = unchecked((int)37),

        /// <summary>	
        /// <dd> <p>The variable holds a compiled compute-shader binary.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_COMPUTESHADER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_COMPUTESHADER</unmanaged-short>	
        Computeshader = unchecked((int)38),

        /// <summary>	
        /// <dd> <p>The variable is a double precision (64-bit) floating-point number.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_DOUBLE</unmanaged>	
        /// <unmanaged-short>D3D_SVT_DOUBLE</unmanaged-short>	
        Double = unchecked((int)39),

        /// <summary>	
        /// <dd> <p>The variable is a 1D read-and-write texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWTEXTURE1D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWTEXTURE1D</unmanaged-short>	
        RWTexture1D = unchecked((int)40),

        /// <summary>	
        /// <dd> <p>The variable is an array of 1D read-and-write textures.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWTEXTURE1DARRAY</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWTEXTURE1DARRAY</unmanaged-short>	
        RWTexture1DArray = unchecked((int)41),

        /// <summary>	
        /// <dd> <p>The variable is a 2D read-and-write texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWTEXTURE2D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWTEXTURE2D</unmanaged-short>	
        RWTexture2D = unchecked((int)42),

        /// <summary>	
        /// <dd> <p>The variable is an array of 2D read-and-write textures.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWTEXTURE2DARRAY</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWTEXTURE2DARRAY</unmanaged-short>	
        RWTexture2DArray = unchecked((int)43),

        /// <summary>	
        /// <dd> <p>The variable is a 3D read-and-write texture.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWTEXTURE3D</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWTEXTURE3D</unmanaged-short>	
        RWTexture3D = unchecked((int)44),

        /// <summary>	
        /// <dd> <p>The variable is a read-and-write buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWBUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWBUFFER</unmanaged-short>	
        RWBuffer = unchecked((int)45),

        /// <summary>	
        /// <dd> <p>The variable is a byte-address buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_BYTEADDRESS_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_BYTEADDRESS_BUFFER</unmanaged-short>	
        ByteAddressBuffer = unchecked((int)46),

        /// <summary>	
        /// <dd> <p>The variable is a read-and-write byte-address buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWBYTEADDRESS_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWBYTEADDRESS_BUFFER</unmanaged-short>	
        RWByteAddressBuffer = unchecked((int)47),

        /// <summary>	
        /// <dd> <p>The variable is a structured buffer. </p> <p>For more information about structured buffer, see the <strong>Remarks</strong> section.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_STRUCTURED_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_STRUCTURED_BUFFER</unmanaged-short>	
        StructuredBuffer = unchecked((int)48),

        /// <summary>	
        /// <dd> <p>The variable is a read-and-write structured buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_RWSTRUCTURED_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_RWSTRUCTURED_BUFFER</unmanaged-short>	
        RWStructuredBuffer = unchecked((int)49),

        /// <summary>	
        /// <dd> <p>The variable is an append structured buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_APPEND_STRUCTURED_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_APPEND_STRUCTURED_BUFFER</unmanaged-short>	
        AppendStructuredBuffer = unchecked((int)50),

        /// <summary>	
        /// <dd> <p>The variable is a consume structured buffer.</p> </dd>	
        /// </summary>	
        /// <msdn-id>ff728735</msdn-id>	
        /// <unmanaged>D3D_SVT_CONSUME_STRUCTURED_BUFFER</unmanaged>	
        /// <unmanaged-short>D3D_SVT_CONSUME_STRUCTURED_BUFFER</unmanaged-short>	
        ConsumeStructuredBuffer = unchecked((int)51),
    }
}