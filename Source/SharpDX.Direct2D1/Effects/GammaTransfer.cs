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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in GammaTransfer effect.
    /// </summary>
    public class GammaTransfer : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GammaTransfer"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public GammaTransfer(DeviceContext context) : base(context, Effect.GammaTransfer)
        {
        }

        /// <summary>
        /// The amplitude of the gamma transfer function for the Red channel.
        /// </summary>
        public float RedAmplitude
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.RedAmplitude);
            }
            set
            {
                SetValue((int)GammaTransferProperties.RedAmplitude, value);
            }
        }

        /// <summary>
        /// The exponent of the gamma transfer function for the Red channel.
        /// </summary>
        public float RedExponent
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.RedExponent);
            }
            set
            {
                SetValue((int)GammaTransferProperties.RedExponent, value);
            }
        }

        /// <summary>
        /// The offset of the gamma transfer function for the Red channel.
        /// </summary>
        public float RedOffset
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.RedOffset);
            }
            set
            {
                SetValue((int)GammaTransferProperties.RedOffset, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Red channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Red channel. 
        /// </summary>
        public bool RedDisable
        {

            get
            {
                return GetBoolValue((int)GammaTransferProperties.RedDisable);
            }
            set
            {
                SetValue((int)GammaTransferProperties.RedDisable, value);
            }
        }

        /// <summary>
        /// The amplitude of the gamma transfer function for the Green channel.
        /// </summary>
        public float GreenAmplitude
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.GreenAmplitude);
            }
            set
            {
                SetValue((int)GammaTransferProperties.GreenAmplitude, value);
            }
        }

        /// <summary>
        /// The exponent of the gamma transfer function for the Green channel.
        /// </summary>
        public float GreenExponent
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.GreenExponent);
            }
            set
            {
                SetValue((int)GammaTransferProperties.GreenExponent, value);
            }
        }

        /// <summary>
        /// The offset of the gamma transfer function for the Green channel.
        /// </summary>
        public float GreenOffset
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.GreenOffset);
            }
            set
            {
                SetValue((int)GammaTransferProperties.GreenOffset, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Green channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Green channel. 
        /// </summary>
        public bool GreenDisable
        {

            get
            {
                return GetBoolValue((int)GammaTransferProperties.GreenDisable);
            }
            set
            {
                SetValue((int)GammaTransferProperties.GreenDisable, value);
            }
        }

        /// <summary>
        /// The amplitude of the gamma transfer function for the Blue channel.
        /// </summary>
        public float BlueAmplitude
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.BlueAmplitude);
            }
            set
            {
                SetValue((int)GammaTransferProperties.BlueAmplitude, value);
            }
        }

        /// <summary>
        /// The exponent of the gamma transfer function for the Blue channel.
        /// </summary>
        public float BlueExponent
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.BlueExponent);
            }
            set
            {
                SetValue((int)GammaTransferProperties.BlueExponent, value);
            }
        }

        /// <summary>
        /// The offset of the gamma transfer function for the Blue channel.
        /// </summary>
        public float BlueOffset
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.BlueOffset);
            }
            set
            {
                SetValue((int)GammaTransferProperties.BlueOffset, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Blue channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Blue channel. 
        /// </summary>
        public bool BlueDisable
        {

            get
            {
                return GetBoolValue((int)GammaTransferProperties.BlueDisable);
            }
            set
            {
                SetValue((int)GammaTransferProperties.BlueDisable, value);
            }
        }

        /// <summary>
        /// The amplitude of the gamma transfer function for the Alpha channel.
        /// </summary>
        public float AlphaAmplitude
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.AlphaAmplitude);
            }
            set
            {
                SetValue((int)GammaTransferProperties.AlphaAmplitude, value);
            }
        }

        /// <summary>
        /// The exponent of the gamma transfer function for the Alpha channel.
        /// </summary>
        public float AlphaExponent
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.AlphaExponent);
            }
            set
            {
                SetValue((int)GammaTransferProperties.AlphaExponent, value);
            }
        }

        /// <summary>
        /// The offset of the gamma transfer function for the Alpha channel.
        /// </summary>
        public float AlphaOffset
        {
            get
            {
                return GetFloatValue((int)GammaTransferProperties.AlphaOffset);
            }
            set
            {
                SetValue((int)GammaTransferProperties.AlphaOffset, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Alpha channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Alpha channel. 
        /// </summary>
        public bool AlphaDisable
        {

            get
            {
                return GetBoolValue((int)GammaTransferProperties.AlphaDisable);
            }
            set
            {
                SetValue((int)GammaTransferProperties.AlphaDisable, value);
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
                return GetBoolValue((int)GammaTransferProperties.ClampOutput);
            }
            set
            {
                SetValue((int)GammaTransferProperties.ClampOutput, value);
            }
        }
    }
}