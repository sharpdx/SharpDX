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

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in ConvolveMatrix effect.
    /// </summary>
    public class ConvolveMatrix : Effect
    {
        private float[] kernelMatrix;
        /// <summary>
        /// Initializes a new instance of <see cref="ConvolveMatrixEffect"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public ConvolveMatrix(DeviceContext context) : base(context, Effect.ConvolveMatrix)
        {
        }

        /// <summary>
        /// The size of one unit in the kernel. The units are in (DIPs/kernel unit), where a kernel unit is the size of the element in the convolution kernel. A value of 1 (DIP/kernel unit) corresponds to one pixel in a image at 96 DPI.
        /// </summary>
        public float KernelUnitLength
        {
            get
            {
                return GetFloatValue((int)ConvoleMatrixProperties.KernelUnitLength);
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.KernelUnitLength, value);
            }
        }

        /// <summary>
        /// The interpolation mode the effect uses to scale the image to the corresponding kernel unit length. There are six scale modes that range in quality and speed. If you don't select a mode, the effect uses the interpolation mode of the device context. See Scale modes for more info
        /// </summary>
        public ConvoleMatrixScaleMode ScaleMode
        {
            get
            {
                return GetEnumValue<ConvoleMatrixScaleMode>((int)ConvoleMatrixProperties.ScaleMode);
            }
            set
            {
                SetEnumValue((int)ConvoleMatrixProperties.ScaleMode, value);
            }
        }

        /// <summary>
        /// The width of the kernel matrix. The units are specified in kernel units.       
        /// </summary>
        public int KernelSizeX
        {
            get
            {
                return unchecked((int)GetUIntValue((int)ConvoleMatrixProperties.KernelSizeX));
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.KernelSizeX, unchecked((uint)value));
            }
        }

        /// <summary>
        /// The height of the kernel matrix. The units are specified in kernel units.        
        /// </summary>
        public int KernelSizeY
        {
            get
            {
                return unchecked((int)GetUIntValue((int)ConvoleMatrixProperties.KernelSizeY));
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.KernelSizeY, unchecked((uint)value));
            }
        }

        /// <summary>
        /// The kernel matrix to be applied to the image. The kernel elements aren't bounded and are specified as floats.
        /// The first set of KernelSizeX numbers in the FLOAT[] corresponds to the first row in the kernel. 
        /// The second set of KernelSizeX numbers correspond to the second row, and so on up to KernelSizeY rows.
        /// </summary>
        public unsafe float[] KernelMatrix
        {
            get
            {
                if (kernelMatrix == null)
                {
                    kernelMatrix = new float[KernelSizeX * KernelSizeY];
                }

                if (kernelMatrix.Length > 0)
                {
                    fixed (void* pKernelMatrix = kernelMatrix)
                        GetValue((int)ConvoleMatrixProperties.KernelMatrix, PropertyType.Blob, (IntPtr)pKernelMatrix, sizeof(float) * kernelMatrix.Length);
                }

                return kernelMatrix;
            }
            set
            {
                if (value.Length != (KernelSizeX * KernelSizeY))
                {
                    throw new ArgumentException("Size of the array doesn't match KernelSizeX * KernelSizeY");
                }
                kernelMatrix = value;

                fixed (void* pKernelMatrix = kernelMatrix)
                    SetValue((int)ConvoleMatrixProperties.KernelMatrix, PropertyType.Blob, (IntPtr)pKernelMatrix, sizeof(float) * kernelMatrix.Length);
            }
        }

        /// <summary>
        /// The kernel matrix is applied to a pixel and then the result is divided by this value. 0 behaves as a value of float epsilon.
        /// </summary>
        public float Divisor
        {
            get
            {
                return GetFloatValue((int)ConvoleMatrixProperties.Divisor);
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.Divisor, value);
            }
        }

        /// <summary>
        /// The effect applies the kernel matrix, the divisor, and then the bias is added to the result. The bias is unbounded and unitless.
        /// </summary>
        public float Bias
        {
            get
            {
                return GetFloatValue((int)ConvoleMatrixProperties.Bias);
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.Bias, value);
            }
        }

        /// <summary>
        /// Shifts the convolution kernel from a centered position on the output pixel to a position you specify left/right and up/down. The offset is defined in kernel units.
        /// With some offsets and kernel sizes, the convolution kernel’s samples won't land on a pixel image center. The pixel values for the kernel sample are computed by bilinear interpolation.
        /// </summary>
        public RawVector2 KernelOffset
        {
            get
            {
                return GetVector2Value((int)ConvoleMatrixProperties.KernelOffset);
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.KernelOffset, value);
            }
        }

        /// <summary>
        /// Specifies whether the convolution kernel is applied to the alpha channel or only the color channels.
        /// If you set this to TRUE the convolution kernel is applied only to the color channels.
        /// If you set this to FALSE the convolution kernel is applied to all channels.
        /// </summary>
        public bool PreserveAlpha
        {
            get
            {
                return GetBoolValue((int)ConvoleMatrixProperties.PreserveAlpha);
            }
            set
            {
                SetValue((int)ConvoleMatrixProperties.PreserveAlpha, value);
            }
        }

        /// <summary>
        /// The mode used to calculate the border of the image, soft or hard. See <see cref="BorderMode"/> modes for more info.
        /// </summary>
        public BorderMode BorderMode
        {
            get
            {
                return GetEnumValue<BorderMode>((int)ConvoleMatrixProperties.BorderMode);
            }
            set
            {
                SetEnumValue((int)ConvoleMatrixProperties.BorderMode, value);
            }
        }

        /// <summary>
        /// Whether the effect clamps color values to between 0 and 1 before the effect passes the values to the next effect in the graph. The effect clamps the values before it premultiplies the alpha .
        /// if you set this to TRUE the effect will clamp the values. 
        /// If you set this to FALSE, the effect will not clamp the color values, but other effects and the output surface may clamp the values if they are not of high enough precision.
        /// </summary>
        public bool ClampOutput
        {
            get
            {
                return GetBoolValue((int)ColorMatrixProperties.ClampOutput);
            }
            set
            {
                SetValue((int)ColorMatrixProperties.ClampOutput, value);
            }
        }
    }
}