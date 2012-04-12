// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
#if DIRECT3D11_1
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Builtin TableTransfer effect.
    /// </summary>
    public class TableTransfer : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TableTransfer"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public TableTransfer(DeviceContext context) : base(context, Effect.TableTransfer)
        {
        }

        /// <summary>
        /// The list of values used to define the transfer function for the Red channel.
        /// </summary>
        public float[] RedTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// If you set this to TRUE the effect does not apply the transfer function to the Red channel. If you set this to FALSE the effect applies the RedTableTransfer function to the Red channel. 
        /// </summary>
        public bool RedDisable
        {
            get
            {
                return GetBoolValue((int)TableTransferProperties.RedDisable);
            }
            set
            {
                SetValue((int)TableTransferProperties.RedDisable, value);
            }
        }

        /// <summary>
        /// The list of values that define the transfer function for the Green channel.
        /// </summary>
        public float[] GreenTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// If you set this to TRUE the effect does not apply the transfer function to the Green channel. If you set this to FALSE the effect applies the GreenTableTransfer function to the Green channel. 
        /// </summary>
        public bool GreenDisable
        {
            get
            {
                return GetBoolValue((int)TableTransferProperties.GreenDisable);
            }
            set
            {
                SetValue((int)TableTransferProperties.GreenDisable, value);
            }
        }

        /// <summary>
        /// The list of values that define the transfer function for the Blue channel. 
        /// </summary>
        public float[] BlueTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// If you set this to TRUE the effect does not apply the transfer function to the Blue channel. If you set this to FALSE the effect applies the BlueTableTransfer function to the Blue channel. 
        /// </summary>
        public bool BlueDisable
        {
            get
            {
                return GetBoolValue((int)TableTransferProperties.BlueDisable);
            }
            set
            {
                SetValue((int)TableTransferProperties.BlueDisable, value);
            }
        }

        /// <summary>
        /// The list of values that define the transfer function for the Alpha channel. 
        /// </summary>
        public float[] AlphaTable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// If you set this to TRUE the effect does not apply the transfer function to the Alpha channel. If you set this to FALSE the effect applies the AlphaTableTransfer function to the Alpha channel. 
        /// </summary>
        public bool AlphaDisable
        {
            get
            {
                return GetBoolValue((int)TableTransferProperties.AlphaDisable);
            }
            set
            {
                SetValue((int)TableTransferProperties.AlphaDisable, value);
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
                return GetBoolValue((int)TableTransferProperties.ClampOutput);
            }
            set
            {
                SetValue((int)TableTransferProperties.ClampOutput, value);
            }
        }
    }
}
#endif