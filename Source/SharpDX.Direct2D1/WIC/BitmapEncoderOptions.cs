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
using SharpDX.Win32;

namespace SharpDX.WIC
{
    /// <summary>
    /// BitmapEncoderOptions used for encoding.
    /// </summary>
    public class BitmapEncoderOptions : PropertyBag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapEncoderOptions"/> class.
        /// </summary>
        /// <param name="propertyBagPointer">The property bag pointer.</param>
        public BitmapEncoderOptions(IntPtr propertyBagPointer)
            : base(propertyBagPointer)
        {
        }

        /// <summary>
        /// Gets or sets the image quality.
        /// </summary>
        /// <value>
        /// The image quality.
        /// </value>
        /// <remarks>
        /// Range value: 0-1.0f
        /// Applicable Codecs: JPEG, HDPhoto
        /// </remarks>
        public float ImageQuality
        {
            get { return Get(ImageQualityKey); }
            set { Set(ImageQualityKey, value);}
        }
        private static readonly PropertyBagKey<float, float> ImageQualityKey = new PropertyBagKey<float, float>("ImageQuality");

        /// <summary>
        /// Gets or sets the compression quality.
        /// </summary>
        /// <value>
        /// The compression quality.
        /// </value>
        /// <remarks>
        /// Range value: 0-1.0f 
        /// Applicable Codecs: TIFF
        /// </remarks>
        public float CompressionQuality
        {
            get { return Get(CompressionQualityKey); }
            set { Set(CompressionQualityKey, value); }
        }
        private static readonly PropertyBagKey<float, float> CompressionQualityKey = new PropertyBagKey<float, float>("CompressionQuality");

        /// <summary>
        /// Gets or sets a value indicating whether loss less compression is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [loss less]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Range value: true-false
        /// Applicable Codecs: HDPhoto
        /// </remarks>
        public bool LossLess
        {
            get { return Get(LosslessKey); }
            set { Set(LosslessKey, value); }
        }
        private static readonly PropertyBagKey<bool, bool> LosslessKey = new PropertyBagKey<bool, bool>("Lossless");

        /// <summary>
        /// Gets or sets the bitmap transform.
        /// </summary>
        /// <value>
        /// The bitmap transform.
        /// </value>
        /// <remarks>
        /// Range value: <see cref="BitmapTransformOptions"/>
        /// Applicable Codecs: JPEG
        /// </remarks>
        public BitmapTransformOptions BitmapTransform
        {
            get { return Get(BitmapTransformKey); }
            set { Set(BitmapTransformKey, value); }
        }
        private static readonly PropertyBagKey<BitmapTransformOptions, byte> BitmapTransformKey = new PropertyBagKey<BitmapTransformOptions, byte>("BitmapTransform");

        /// <summary>
        /// Gets or sets a value indicating whether [interlace option].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [interlace option]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Range value: true-false
        /// Applicable Codecs: PNG
        /// </remarks>
        public bool InterlaceOption
        {
            get { return Get(InterlaceOptionKey); }
            set { Set(InterlaceOptionKey, value); }
        }
        private static readonly PropertyBagKey<bool, bool> InterlaceOptionKey = new PropertyBagKey<bool, bool>("InterlaceOption");

        /// <summary>
        /// Gets or sets the filter option.
        /// </summary>
        /// <value>
        /// The filter option.
        /// </value>
        /// <remarks>
        /// Range value: <see cref="PngFilterOption"/>
        /// Applicable Codecs: PNG
        /// </remarks>
        public PngFilterOption FilterOption
        {
            get { return Get(FilterOptionKey); }
            set { Set(FilterOptionKey, value); }
        }
        private static readonly PropertyBagKey<PngFilterOption, byte> FilterOptionKey = new PropertyBagKey<PngFilterOption, byte>("FilterOption");

        /// <summary>
        /// Gets or sets the TIFF compression method.
        /// </summary>
        /// <value>
        /// The TIFF compression method.
        /// </value>
        /// <remarks>
        /// Range value: <see cref="TiffCompressionOption"/>
        /// Applicable Codecs: TIFF
        /// </remarks>
        public TiffCompressionOption TiffCompressionMethod
        {
            get { return Get(TiffCompressionMethodKey); }
            set { Set(TiffCompressionMethodKey, value); }
        }
        private static readonly PropertyBagKey<TiffCompressionOption, bool> TiffCompressionMethodKey = new PropertyBagKey<TiffCompressionOption, bool>("TiffCompressionMethod");

        /// <summary>
        /// Gets or sets the luminance.
        /// </summary>
        /// <value>
        /// The luminance.
        /// </value>
        /// <remarks>
        /// Range value: 64 Entries (DCT)
        /// Applicable Codecs: JPEG
        /// </remarks>
        public uint[] Luminance
        {
            get { return Get(LuminanceKey); }
            set { Set(LuminanceKey, value); }
        }
        private static readonly PropertyBagKey<uint[], uint[]> LuminanceKey = new PropertyBagKey<uint[], uint[]>("Luminance");

        /// <summary>
        /// Gets or sets the chrominance.
        /// </summary>
        /// <value>
        /// The chrominance.
        /// </value>
        /// <remarks>
        /// Range value: 64 Entries (DCT)
        /// Applicable Codecs: JPEG
        /// </remarks>
        public uint[] Chrominance
        {
            get { return Get(ChrominanceKey); }
            set { Set(ChrominanceKey, value); }
        }
        private static readonly PropertyBagKey<uint[], uint[]> ChrominanceKey = new PropertyBagKey<uint[], uint[]>("Chrominance");

        /// <summary>
        /// Gets or sets the JPEG Y Cr Cb subsampling.
        /// </summary>
        /// <value>
        /// The JPEG Y Cr Cb subsampling.
        /// </value>
        /// <remarks>
        /// Range value: <see cref="JpegYCrCbSubsamplingOption"/>
        /// Applicable Codecs: JPEG
        /// </remarks>
        public JpegYCrCbSubsamplingOption JpegYCrCbSubsampling
        {
            get { return Get(JpegYCrCbSubsamplingKey); }
            set { Set(JpegYCrCbSubsamplingKey, value); }
        }
        private static readonly PropertyBagKey<JpegYCrCbSubsamplingOption, byte> JpegYCrCbSubsamplingKey = new PropertyBagKey<JpegYCrCbSubsamplingOption, byte>("JpegYCrCbSubsampling");

        /// <summary>
        /// Gets or sets a value indicating whether [suppress app0].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [suppress app0]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Range value: true-false
        /// Applicable Codecs: JPEG
        /// </remarks>
        public bool SuppressApp0
        {
            get { return Get(SuppressApp0Key); }
            set { Set(SuppressApp0Key, value); }
        }
        private static readonly PropertyBagKey<bool, bool> SuppressApp0Key = new PropertyBagKey<bool, bool>("SuppressApp0");
    }
}