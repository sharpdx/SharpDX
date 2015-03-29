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
    /// Built in LinearTransfer effect.
    /// </summary>
    public class LinearTransfer : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LinearTransfer"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public LinearTransfer(DeviceContext context) : base(context, Effect.LinearTransfer)
        {
        }

        /// <summary>
        /// TThe Y-intercept of the linear function for the Red channel. 
        /// </summary>
        public float RedYIntercept
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.RedYIntercept);
            }
            set
            {
                SetValue((int)LinearTransferProperties.RedYIntercept, value);
            }
        }

        /// <summary>
        /// The slope of the linear function for the Red channel. 
        /// </summary>
        public float RedSlope
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.RedSlope);
            }
            set
            {
                SetValue((int)LinearTransferProperties.RedSlope, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Red channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Red channel. 
        /// </summary>
        public bool RedDisable
        {

            get
            {
                return GetBoolValue((int)LinearTransferProperties.RedDisable);
            }
            set
            {
                SetValue((int)LinearTransferProperties.RedDisable, value);
            }
        }

        /// <summary>
        /// The Y-intercept of the linear function for the Green channel. 
        /// </summary>
        public float GreenYIntercept
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.GreenYIntercept);
            }
            set
            {
                SetValue((int)LinearTransferProperties.GreenYIntercept, value);
            }
        }

        /// <summary>
        /// The slope of the linear function for the Green channel.
        /// </summary>
        public float GreenSlope
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.GreenSlope);
            }
            set
            {
                SetValue((int)LinearTransferProperties.GreenSlope, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Green channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Green channel. 
        /// </summary>
        public bool GreenDisable
        {

            get
            {
                return GetBoolValue((int)LinearTransferProperties.GreenDisable);
            }
            set
            {
                SetValue((int)LinearTransferProperties.GreenDisable, value);
            }
        }

        /// <summary>
        /// The Y-intercept of the linear function for the Blue channel. 
        /// </summary>
        public float BlueYIntercept
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.BlueYIntercept);
            }
            set
            {
                SetValue((int)LinearTransferProperties.BlueYIntercept, value);
            }
        }

        /// <summary>
        /// The slope of the linear function for the Blue channel.
        /// </summary>
        public float BlueSlope
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.BlueSlope);
            }
            set
            {
                SetValue((int)LinearTransferProperties.BlueSlope, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Blue channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Blue channel. 
        /// </summary>
        public bool BlueDisable
        {

            get
            {
                return GetBoolValue((int)LinearTransferProperties.BlueDisable);
            }
            set
            {
                SetValue((int)LinearTransferProperties.BlueDisable, value);
            }
        }

        /// <summary>
        /// The Y-intercept of the linear function for the Alpha channel.
        /// </summary>
        public float AlphaYIntercept
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.AlphaYIntercept);
            }
            set
            {
                SetValue((int)LinearTransferProperties.AlphaYIntercept, value);
            }
        }

        /// <summary>
        /// The slope of the linear function for the Alpha channel. 
        /// </summary>
        public float AlphaSlope
        {
            get
            {
                return GetFloatValue((int)LinearTransferProperties.AlphaSlope);
            }
            set
            {
                SetValue((int)LinearTransferProperties.AlphaSlope, value);
            }
        }

        /// <summary>
        /// If you set this to TRUE it does not apply the transfer function to the Alpha channel. An identity transfer function is used. If you set this to FALSE it applies the gamma transfer function to the Alpha channel. 
        /// </summary>
        public bool AlphaDisable
        {

            get
            {
                return GetBoolValue((int)LinearTransferProperties.AlphaDisable);
            }
            set
            {
                SetValue((int)LinearTransferProperties.AlphaDisable, value);
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
                return GetBoolValue((int)LinearTransferProperties.ClampOutput);
            }
            set
            {
                SetValue((int)LinearTransferProperties.ClampOutput, value);
            }
        }
    }
}