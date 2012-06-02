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
using SharpDX.Direct2D1;
using SharpDX;
using System;

using SharpDX.IO;
using System.Runtime.InteropServices;

namespace D2DCustomPixelShaderEffect
{
    /// <summary>
    /// RippleEffect sample, port from "Direct2D custom image effects sample" C++
    /// sample from Win8 pack samples.
    /// </summary>
    /// <remarks>
    /// Unlike the C++ samples, we don't need to declare the metadata in XML but
    /// we are using here .NET attributes.
    /// </remarks>
    [CustomEffect("Adds a ripple effect that can be animated", "Stylize", "SharpDX")]
    [CustomEffectInput("Source")]
    public class RippleEffect : CustomEffectBase, DrawTransform
    {
        private static readonly Guid GUID_RipplePixelShader = Guid.NewGuid();
        private DrawInformation drawInformation;
        private RippleEffectConstantBuffer constants;

        /// <summary>
        /// Initializes a new instance of <see cref="RippleEffect"/> lcass.
        /// </summary>
        public RippleEffect()
        {
        }

        /// <summary>
        /// Gets or sets the Frequency.
        /// </summary>
        [PropertyBinding((int)RippleProperties.Frequency, "0.0", "1000.0", "0.0")]
        public float Frequency
        {
            get
            {
                return constants.Frequency;
            }
            set
            {
                constants.Frequency = MathUtil.Clamp(value, 0.0f, 1000.0f);
                UpdateConstants();
            }
        }

        /// <summary>
        /// Gets or sets the phase.
        /// </summary>
        [PropertyBinding((int)RippleProperties.Phase, "-100.0", "100.0", "0.0")]
        public float Phase
        {
            get
            {
                return constants.Phase;
            }
            set
            {
                constants.Phase = MathUtil.Clamp(value, -100.0f, 100.0f);
            }
        }

        /// <summary>
        /// Gets or sets the amplitude.
        /// </summary>
        [PropertyBinding((int)RippleProperties.Amplitude, "0.0001", "1000.0", "0.0")]
        public float Amplitude
        {
            get
            {
                return constants.Amplitude;
            }
            set
            {
                constants.Amplitude = MathUtil.Clamp(value, 0.0001f, 1000.0f);
            }
        }

        /// <summary>
        /// Gets or sets the spread.
        /// </summary>
        [PropertyBinding((int)RippleProperties.Spread, "0.0001", "1000.0", "0.0")]
        public float Spread
        {
            get
            {
                return constants.Spread;
            }
            set
            {
                constants.Spread = MathUtil.Clamp(value, 0.0001f, 1000.0f);
            }
        }

        /// <summary>
        /// Gets or sets the center of the ripple effect.
        /// </summary>
        [PropertyBinding((int)RippleProperties.Center, "(-2000.0, -2000.0)", "(2000.0, 2000.0)", "(0.0, 0.0)")]
        public DrawingPointF Center
        {
            get
            {
                return constants.Center;
            }
            set
            {
                constants.Center = value;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(EffectContext effectContext, TransformGraph transformGraph)
        {
            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            effectContext.LoadPixelShader(GUID_RipplePixelShader, NativeFile.ReadAllBytes(path + "\\Ripple.cso"));
            transformGraph.SetSingleTransformNode(this);
        }

        /// <inheritdoc/>
        public override void PrepareForRender(ChangeType changeType)
        {
            UpdateConstants();
        }

        /// <inheritdoc/>
        public override void SetGraph(TransformGraph transformGraph)
        {
            throw new NotImplementedException();
        }

        public void SetDrawInformation(DrawInformation drawInfo)
        {
            this.drawInformation = drawInfo;

            drawInformation.SetPixelShader(GUID_RipplePixelShader, PixelOptions.None);
            drawInformation.SetInputDescription(0, new InputDescription(Filter.MinimumMagLinearMipPoint, 1));
        }

        public Rectangle MapInvalidRect(int inputIndex, Rectangle invalidInputRect)
        {
            return invalidInputRect;
        }

        public Rectangle MapInputRectanglesToOutputRectangle(Rectangle[] inputRects, Rectangle[] inputOpaqueSubRects, out Rectangle outputOpaqueSubRect)
        {
            if (inputRects.Length != 1)
                throw new ArgumentException("InputRects must be length of 1", "inputRects");
            outputOpaqueSubRect = default(Rectangle);
            return inputRects[0];
        }

        public void MapOutputRectangleToInputRectangles(Rectangle outputRect, Rectangle[] inputRects)
        {
            int expansion = (int)Math.Round(constants.Amplitude);
            if (inputRects.Length != 1)
                throw new ArgumentException("InputRects must be length of 1", "inputRects");
            inputRects[0].Left = outputRect.Left - expansion;
            inputRects[0].Top = outputRect.Top - expansion;
            inputRects[0].Right = outputRect.Right + expansion;
            inputRects[0].Bottom = outputRect.Bottom + expansion;
        }

        public int InputCount
        {
            get { return 1; }
        }

        private void UpdateConstants()
        {
            if (drawInformation != null)
            {
                drawInformation.SetPixelConstantBuffer(ref constants);
            }
        }

        /// <summary>
        /// Internal structure used for the constant buffer.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RippleEffectConstantBuffer
        {
            public float Frequency;
            public float Phase;
            public float Amplitude;
            public float Spread;
            public DrawingPointF Center;
        }
    }
}
