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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    public partial class EffectContext
    {

        /// <summary>
        /// Gets the DPI.
        /// </summary>
        public RawVector2 Dpi
        {
            get
            {
                RawVector2 dpi;
                GetDpi(out dpi.X, out dpi.Y);
                return dpi;
            }
            /// <unmanaged>HRESULT ID2D1EffectContext::GetMaximumSupportedFeatureLevel([In, Buffer] const D3D_FEATURE_LEVEL* featureLevels,[In] unsigned int featureLevelsCount,[Out] D3D_FEATURE_LEVEL* maximumSupportedFeatureLevel)</unmanaged>	
        }

        /// <summary>
        /// Gets the maximum feature level supported by this instance.
        /// </summary>
        /// <param name="featureLevels">An array of feature levels</param>
        /// <returns>The maximum feature level selected from the array</returns>
        public SharpDX.Direct3D.FeatureLevel GetMaximumSupportedFeatureLevel(SharpDX.Direct3D.FeatureLevel[] featureLevels)
        {
            return GetMaximumSupportedFeatureLevel(featureLevels, featureLevels.Length);
        }

        /// <summary>	
        /// Loads a pixel shader.
        /// </summary>	
        /// <param name="shaderId">An unique identifier associated with the shader bytecode.</param>	
        /// <param name="shaderBytecode">The bytecode of the shader.</param>	
        /// <unmanaged>HRESULT ID2D1EffectContext::LoadPixelShader([In] const GUID&amp; shaderId,[In, Buffer] const unsigned char* shaderBuffer,[In] unsigned int shaderBufferCount)</unmanaged>	
        public void LoadPixelShader(System.Guid shaderId, byte[] shaderBytecode)
        {
            LoadPixelShader(shaderId, shaderBytecode, shaderBytecode.Length);
        }

        /// <summary>	
        /// Loads a vertex shader.
        /// </summary>	
        /// <param name="shaderId">An unique identifier associated with the shader bytecode.</param>	
        /// <param name="shaderBytecode">The bytecode of the shader.</param>	
        /// <unmanaged>HRESULT ID2D1EffectContext::LoadVertexShader([In] const GUID&amp; resourceId,[In, Buffer] const unsigned char* shaderBuffer,[In] unsigned int shaderBufferCount)</unmanaged>	
        public void LoadVertexShader(System.Guid shaderId, byte[] shaderBytecode)
        {
            LoadVertexShader(shaderId, shaderBytecode, shaderBytecode.Length);
        }

        /// <summary>	
        /// Loads a compute shader.
        /// </summary>	
        /// <param name="shaderId">An unique identifier associated with the shader bytecode.</param>	
        /// <param name="shaderBytecode">The bytecode of the shader.</param>	
        /// <unmanaged>HRESULT ID2D1EffectContext::LoadComputeShader([In] const GUID&amp; resourceId,[In, Buffer] const unsigned char* shaderBuffer,[In] unsigned int shaderBufferCount)</unmanaged>	
        public void LoadComputeShader(System.Guid shaderId, byte[] shaderBytecode)
        {
            LoadComputeShader(shaderId, shaderBytecode, shaderBytecode.Length);
        }


        /// <summary>
        /// Check if this device is supporting a feature.
        /// </summary>
        /// <param name="feature">The feature to check.</param>
        /// <returns>
        /// Returns true if this device supports this feature, otherwise false.
        /// </returns>
        public bool CheckFeatureSupport(Feature feature)
        {
            unsafe
            {
                switch (feature)
                {
                    case Feature.Doubles:
                        {
                            FeatureDataDoubles support;

                            if (CheckFeatureSupport(Feature.Doubles, new IntPtr(&support), Utilities.SizeOf<FeatureDataDoubles>()).Failure)
                                return false;
                            return support.DoublePrecisionFloatShaderOps;
                        }
                    case Feature.D3D10XHardwareOptions:
                        {
                            FeatureDataD3D10XHardwareOptions support;
                            if (CheckFeatureSupport(Feature.D3D10XHardwareOptions, new IntPtr(&support), Utilities.SizeOf<FeatureDataD3D10XHardwareOptions>()).Failure)
                                return false;
                            return support.ComputeShadersPlusRawAndStructuredBuffersViaShader4X;
                        }
                    default:
                        throw new SharpDXException("Unsupported Feature. Use specialized CheckXXX methods");
                }
            }
        }

    }
}