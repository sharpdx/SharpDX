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
    /// Features supported by a <see cref="GraphicsDevice"/>.
    /// </summary>
    /// <remarks>
    /// This class gives also features for a particular format, using the operator this[dxgiFormat] on this structure.
    /// </remarks>
    public struct GraphicsDeviceFeatures
    {
        private readonly FeaturesPerFormat[] mapFeaturesPerFormat;

        /// <summary>
        /// <see cref="Format"/> to exclude from the features test.
        /// </summary>
        private readonly static List<DXGI.Format> ObsoleteFormatToExcludes = new List<DXGI.Format>() {Format.R1_UNorm, Format.B5G6R5_UNorm, Format.B5G5R5A1_UNorm};

        internal GraphicsDeviceFeatures(Direct3D11.Device device)
        {
            mapFeaturesPerFormat = new FeaturesPerFormat[256];

            // Check global features
            Level = device.FeatureLevel;
            HasComputeShaders = device.CheckFeatureSupport(Feature.ComputeShaders);
            HasDoublePrecision = device.CheckFeatureSupport(Feature.ShaderDoubles);
            device.CheckThreadingSupport(out HasMultiThreadingConcurrentResources, out HasMultiThreadingCommandLists);

            // Check features for each DXGI.Format
            foreach (var format in Enum.GetValues(typeof(DXGI.Format)))
            {
                var dxgiFormat = (DXGI.Format) format;
                int maximumMSAA = 1;
                var computeShaderFormatSupport = ComputeShaderFormatSupport.None;
                var formatSupport = FormatSupport.None;

                if (!ObsoleteFormatToExcludes.Contains(dxgiFormat))
                {
                    try
                    {
                        maximumMSAA = GetMaximumMSAASampleCount(device, dxgiFormat);
                        if (HasComputeShaders)
                            computeShaderFormatSupport = device.CheckComputeShaderFormatSupport(dxgiFormat);

                        formatSupport = device.CheckFormatSupport(dxgiFormat);
                    }
                    catch
                    {
                    }
                }

                mapFeaturesPerFormat[(int)dxgiFormat] = new FeaturesPerFormat(dxgiFormat, maximumMSAA, computeShaderFormatSupport, formatSupport);
            }
        }

        /// <summary>
        /// Features level of the current device.
        /// </summary>
        /// <msdn-id>ff476528</msdn-id>	
        /// <unmanaged>GetFeatureLevel</unmanaged>	
        /// <unmanaged-short>GetFeatureLevel</unmanaged-short>	
        /// <unmanaged>D3D_FEATURE_LEVEL ID3D11Device::GetFeatureLevel()</unmanaged>
        public FeatureLevel Level;

        /// <summary>
        /// Boolean indicating if this device supports compute shaders, unordered access on structured buffers and raw structured buffers.
        /// </summary>
        public readonly bool HasComputeShaders;

        /// <summary>
        /// Boolean indicating if this device supports shaders double precision calculations.
        /// </summary>
        public readonly bool HasDoublePrecision;

        /// <summary>
        /// Boolean indicating if this device supports concurrent resources in multithreading scenarios.
        /// </summary>
        public readonly bool HasMultiThreadingConcurrentResources;

        /// <summary>
        /// Boolean indicating if this device supports command lists in multithreading scenarios.
        /// </summary>
        public readonly bool HasMultiThreadingCommandLists;

        /// <summary>
        /// Gets the <see cref="FeaturesPerFormat" /> for the specified <see cref="SharpDX.DXGI.Format" />.
        /// </summary>
        /// <param name="dxgiFormat">The dxgi format.</param>
        /// <returns>Features for the specific format.</returns>
        public FeaturesPerFormat this[SharpDX.DXGI.Format dxgiFormat]
        {
            get { return this.mapFeaturesPerFormat[(int)dxgiFormat]; }
        }

        /// <summary>
        /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="pixelFormat">The pixelFormat.</param>
        /// <returns>The maximum multisample count for this pixel pixelFormat</returns>
        /// <msdn-id>ff476499</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CheckMultisampleQualityLevels([In] DXGI_FORMAT Format,[In] unsigned int SampleCount,[Out] unsigned int* pNumQualityLevels)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CheckMultisampleQualityLevels</unmanaged-short>
        private static int GetMaximumMSAASampleCount(Direct3D11.Device device, PixelFormat pixelFormat)
        {
            int maxCount = 1;
            for (int i = 1; i < 32; i++)
            {
                if (device.CheckMultisampleQualityLevels(pixelFormat, i) != 0)
                    maxCount = i;
            }
            return maxCount;
        }

        /// <summary>
        /// The features exposed for a particular format.
        /// </summary>
        public struct FeaturesPerFormat
        {
            internal FeaturesPerFormat(Format format, int maximumMsaa, ComputeShaderFormatSupport computeShaderFormatSupport, FormatSupport formatSupport)
            {
                Format = format;
                MaximumMSAA = maximumMsaa;
                ComputeShaderFormatSupport = computeShaderFormatSupport;
                FormatSupport = formatSupport;
            }

            /// <summary>
            /// The <see cref="SharpDX.DXGI.Format"/>.
            /// </summary>
            public readonly DXGI.Format Format;

            /// <summary>
            /// Gets the maximum MSAA sample count for a particular <see cref="PixelFormat"/>.
            /// </summary>
            public readonly int MaximumMSAA;

            /// <summary>	
            /// Gets the unordered resource support options for a compute shader resource.	
            /// </summary>	
            /// <msdn-id>ff476135</msdn-id>	
            /// <unmanaged>D3D11_FORMAT_SUPPORT2</unmanaged>	
            /// <unmanaged-short>D3D11_FORMAT_SUPPORT2</unmanaged-short>	
            public readonly ComputeShaderFormatSupport ComputeShaderFormatSupport;

            /// <summary>
            /// Support of a given format on the installed video device.
            /// </summary>
            public readonly FormatSupport FormatSupport;

            public override string ToString()
            {
                return string.Format("Format: {0}, MaximumMSAA: {1}, ComputeShaderFormatSupport: {2}, FormatSupport: {3}", Format, MaximumMSAA, ComputeShaderFormatSupport, FormatSupport);
            }
        }

        public override string ToString()
        {
            return string.Format("Level: {0}, HasComputeShaders: {1}, HasDoublePrecision: {2}, HasMultiThreadingConcurrentResources: {3}, HasMultiThreadingCommandLists: {4}", Level, HasComputeShaders, HasDoublePrecision, HasMultiThreadingConcurrentResources, HasMultiThreadingCommandLists);
        }
    }
}