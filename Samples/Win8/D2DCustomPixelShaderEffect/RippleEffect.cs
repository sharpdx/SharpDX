using SharpDX.Direct2D1;
using SharpDX;
using System;
using SharpDX.D3DCompiler;
using SharpDX.IO;
using System.Runtime.InteropServices;

namespace D2DCustomPixelShaderEffect
{
    [CustomEffect("Adds a ripple effect that can be animated", "Stylize", "SharpDX")]
    [CustomEffectInput("Source")]
    public class RippleEffect : CustomEffectBase, DrawTransform
    {
        private static readonly Guid GUID_RipplePixelShader = Guid.NewGuid();
        private DrawInformation drawInformation;
        private RippleEffectConstantBuffer constants;

        public RippleEffect()
        {
        }

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

        public override void Initialize(EffectContext effectContext, TransformGraph transformGraph)
        {
            transformGraph.SetSingleTransformNode(this);

            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            effectContext.LoadPixelShader(GUID_RipplePixelShader, NativeFile.ReadAllBytes(path + "\\Ripple.cso"));
        }

        public override void PrepareForRender(ChangeType changeType)
        {
            UpdateConstants();
        }

        public override void SetGraph(TransformGraph transformGraph)
        {
            // TODO: Map NotImplementedException to this SharpDXException
            throw new SharpDXException(Result.NotImplemented);
        }

        public void SetDrawInformation(DrawInformation drawInfo)
        {
            this.drawInformation = drawInfo;

            drawInformation.SetPixelShader(GUID_RipplePixelShader, PixelOptions.None);
            drawInformation.SetInputDescription(0, new InputDescription(Filter.MinimumMagLinearMipPoint, 1));
        }

        public Rectangle[] InputRectangles
        {
            set {}
        }

        public Rectangle MapInputRectanglesToOutputRectangle(Rectangle[] inputRects)
        {
            if (inputRects.Length != 1)
                throw new SharpDXException(Result.InvalidArg);
            return inputRects[0];
        }

        public void MapOutputRectangleToInputRectangles(Rectangle outputRect, Rectangle[] inputRects)
        {
            int expansion = (int)Math.Round(constants.Amplitude);
            if (inputRects.Length != 1)
                throw new SharpDXException(Result.InvalidArg);
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
