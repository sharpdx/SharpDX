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
namespace SharpDX.Direct3D12
{
    public partial struct StaticSamplerDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticSamplerDescription"/> struct.
        /// </summary>
        /// <param name="shaderVisibility">The shader visibility.</param>
        /// <param name="shaderRegister">The shader register.</param>
        /// <param name="registerSpace">The register space.</param>
        public StaticSamplerDescription(ShaderVisibility shaderVisibility, int shaderRegister, int registerSpace) : this()
        {
            ShaderVisibility = shaderVisibility;
            ShaderRegister = shaderRegister;
            RegisterSpace = registerSpace;

            Filter = Filter.MinMagMipLinear;
            AddressU = TextureAddressMode.Clamp;
            AddressV = TextureAddressMode.Clamp;
            AddressW = TextureAddressMode.Clamp;
            MinLOD = -float.MaxValue;
            MaxLOD = float.MaxValue;
            MipLODBias = 0.0f;
            MaxAnisotropy = 16;
            ComparisonFunc = Comparison.Never;
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticSamplerDescription"/> struct.
        /// </summary>
        /// <param name="samplerStateDescription">Sampler state description</param>
        /// <param name="shaderVisibility">The shader visibility.</param>
        /// <param name="shaderRegister">The shader register.</param>
        /// <param name="registerSpace">The register space.</param>
        public StaticSamplerDescription(SamplerStateDescription samplerStateDescription, ShaderVisibility shaderVisibility, int shaderRegister, int registerSpace) : this()
        {
            ShaderVisibility = shaderVisibility;
            ShaderRegister = shaderRegister;
            RegisterSpace = registerSpace;
            BorderColor = StaticBorderColor.TransparentBlack;

            Filter = samplerStateDescription.Filter;
            AddressU = samplerStateDescription.AddressU;
            AddressV = samplerStateDescription.AddressV;
            AddressW = samplerStateDescription.AddressW;
            MinLOD = samplerStateDescription.MinimumLod;
            MaxLOD = samplerStateDescription.MaximumLod;
            MipLODBias = samplerStateDescription.MipLodBias;
            MaxAnisotropy = samplerStateDescription.MaximumAnisotropy;
            ComparisonFunc = samplerStateDescription.ComparisonFunction;
        }

        /// <summary>
        /// Sets the (u,v,w) addressing mode with the same value.
        /// </summary>
        /// <value>The (u,v,w) addressing mode with the same value.</value>
        public TextureAddressMode AddressUVW
        {
            set
            {
                AddressU = value;
                AddressV = value;
                AddressW = value;
            }
        }
    }
}