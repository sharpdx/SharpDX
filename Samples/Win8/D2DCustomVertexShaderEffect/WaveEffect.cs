using SharpDX.Direct2D1;
using SharpDX;
using System;

using SharpDX.IO;
using System.Runtime.InteropServices;

namespace D2DCustomVertexShaderEffect
{
    [CustomEffect("Adds a wave effect that can be animated", "Stylize", "SharpDX")]
    [CustomEffectInput("Source")]
    public class WaveEffect : CustomEffectBase, DrawTransform
    {
        private const float waveMarginLeft = 0.25f;
        private const float waveMarginTop = 0.20f;
        private const float waveMarginRight = 0.25f;
        private const float waveMarginBottom = 0.40f;
        private const float waveHeight = 60;
        private const float waveAngleOffset = -0.4f;

        private const int TESSELLATION_AMOUNT = 32;

        private static readonly Guid GUID_WaveVertexShader = Guid.NewGuid();
        private static readonly Guid GUID_WaveVertexBuffer = Guid.NewGuid();
        private DrawInformation drawInformation;
        private WaveEffectConstantBuffer constants;

        private VertexBuffer   vertexBuffer;
        private int numberOfVertices;


        public WaveEffect()
        {
        }

        [PropertyBinding((int)WaveProperties.WaveOffset, "0.0", "1.0", "0.0")]
        public float WaveOffset
        {
            get
            {
                return constants.WaveOffset;
            }
            set
            {
                constants.WaveOffset = value;
                UpdateConstants();
            }
        }

        [PropertyBinding((int)WaveProperties.AngleX, "0.0", "1.0", "0.0")]
        public float AngleX
        {
            get
            {
                return constants.AngleX;
            }
            set
            {
                constants.AngleX = MathUtil.Clamp(value, -100.0f, 100.0f);
            }
        }

        [PropertyBinding((int)WaveProperties.AngleY, "0.0", "1.0", "0.0")]
        public float AngleY
        {
            get
            {
                return constants.AngleY;
            }
            set
            {
                constants.AngleY = MathUtil.Clamp(value, 0.0f, 1.0f);
            }
        }

        private Rectangle inputRectangle;
        private float SizeX { get; set; }
        private float SizeY { get; set; }

        //EffectContext _effectContext;
        public override void Initialize(EffectContext effectContext, TransformGraph transformGraph)
        {
            //WARNING : as soon as TransformGraph.SetSingleTransformNode is called it chain calls the 
            //SetDrawInformation via a callbac. Unfortunately this is too early because the code below
            //within this method is used to populate stateful data needed by the SetDrawInformation call. 
            //transformGraph.SetSingleTransformNode(this);

            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            byte[] vertexShaderBytecode = NativeFile.ReadAllBytes(path + "\\WaveEffect.cso");
            effectContext.LoadVertexShader(GUID_WaveVertexShader, vertexShaderBytecode);

            // Only generate the vertex buffer if it has not already been initialized.
            vertexBuffer = effectContext.FindVertexBuffer(GUID_WaveVertexBuffer);
            
            if (vertexBuffer == null)
            {
                //InitializeVertexBuffer(effectContext);

                var mesh = GenerateMesh();

                // Updating geometry every time the effect is rendered can be costly, so it is 
                // recommended that vertex buffer remain static if possible (which it is in this
                // sample effect).
                using (var stream = DataStream.Create(mesh, true, true))
                {
                    var vbProp = new VertexBufferProperties(1, VertexUsage.Static, stream);

                    var cvbProp = new CustomVertexBufferProperties(vertexShaderBytecode, new[] {
                        new InputElement("MESH_POSITION", 0, SharpDX.DXGI.Format.R32G32_Float,0,0),
                    }, Utilities.SizeOf<Vector2>());

                    // The GUID is optional, and is provided here to register the geometry globally.
                    // As mentioned above, this avoids duplication if multiple versions of the effect
                    // are created. 
                    vertexBuffer = new VertexBuffer(effectContext, GUID_WaveVertexBuffer, vbProp, cvbProp);
                }
            }

            PrepareForRender(ChangeType.Properties|ChangeType.Context);
            transformGraph.SetSingleTransformNode(this);
        }

        private Vector2[] GenerateMesh()
        {
            numberOfVertices = 6 * TESSELLATION_AMOUNT * TESSELLATION_AMOUNT;

            var mesh = new Vector2[numberOfVertices]; 

            float offset = 1.0f / TESSELLATION_AMOUNT;

            for (int i = 0; i < TESSELLATION_AMOUNT; i++)
            {
                for (int j = TESSELLATION_AMOUNT - 1; j >= 0; j--)
                {
                    int index = (i * TESSELLATION_AMOUNT + j) * 6;

                    // This mesh consists of pairs of triangles forming squares. Each square contains
                    // six vertices, three for each triangle. 'offset' represents the distance between each vertice
                    // in the triangles. In this mesh, we only set the x and y coordinates of the vertices, since
                    // they are the only variable part of the geometry. In the vertex shader, z is generated
                    // based on x, and w is defined to be '1'. The actual coordinates here range from 0 to 1,
                    // these values are scaled up based on the size of the image in the vertex shader.
                    mesh[index].X = i * offset;
                    mesh[index].Y = j * offset;
                    mesh[index + 1].X = i * offset;
                    mesh[index + 1].Y = j * offset + offset;
                    mesh[index + 2].X = i * offset + offset;
                    mesh[index + 2].Y = j * offset;
                    mesh[index + 3].X = i * offset + offset;
                    mesh[index + 3].Y = j * offset;
                    mesh[index + 4].X = i * offset;
                    mesh[index + 4].Y = j * offset + offset;
                    mesh[index + 5].X = i * offset + offset;
                    mesh[index + 5].Y = j * offset + offset;
                }
            }

            return mesh;
        }

        public override void PrepareForRender(ChangeType changeType)
        {
            UpdateConstants();
        }

        public override void SetGraph(TransformGraph transformGraph)
        {
            // TODO: Map NotImplementedException to this SharpDXException
            throw new NotImplementedException();
        }

        public void SetDrawInformation(DrawInformation drawInfo)
        {
            this.drawInformation = drawInfo;

            drawInformation.SetVertexProcessing(
               vertexBuffer,
               VertexOptions.UseDepthBuffer,
               null,
               new VertexRange(0, numberOfVertices),
               GUID_WaveVertexShader
               );
        }

        public Rectangle MapInvalidRect(int inputIndex, Rectangle invalidInputRect)
        {
            return invalidInputRect;
        }

        public Rectangle MapInputRectanglesToOutputRectangle(Rectangle[] inputRects, Rectangle[] inputOpaqueSubRects, out Rectangle outputOpaqueSubRect)
        {
            if (inputRects.Length != 1)
                throw new ArgumentException("InputRects must be length of 1", "inputRects");

            inputRectangle = inputRects[0];

            // Store the size of the rect so we can pass it into the vertex shader later.
            int newSizeX = inputRectangle.Right - inputRectangle.Left;
            int newSizeY = inputRectangle.Bottom - inputRectangle.Top;

            if (SizeX != newSizeX || SizeY != newSizeY)
            {
                SizeX = newSizeX;
                SizeY = newSizeY;

                UpdateConstants();
            }

            long inputRectHeight = inputRects[0].Bottom - inputRects[0].Top;
            long inputRectWidth = inputRects[0].Right - inputRects[0].Left;

            var outputRect = new Rectangle();

            // waveAngleOffset is the point where the wave is perpendicular to the screen.
            if (AngleY > waveAngleOffset)
            {
                outputRect.Top = (int)(inputRects[0].Top + (inputRectHeight * waveMarginTop) - waveHeight);
                outputRect.Bottom = (int)(inputRects[0].Bottom - (inputRectHeight * waveMarginBottom) + waveHeight + (inputRectHeight * AngleY));
            }
            else
            {
                outputRect.Top = (int)(inputRects[0].Top + (inputRectHeight * waveMarginTop) - waveHeight + (inputRectHeight * (AngleY - waveAngleOffset)));
                outputRect.Bottom = (int)(inputRects[0].Top + (inputRectHeight * waveMarginTop) + waveHeight);
            }

            if (AngleX > 0)
            {
                outputRect.Left = (int)(inputRects[0].Left + (inputRectWidth * waveMarginLeft));
                outputRect.Right = (int)(inputRects[0].Right - (inputRectWidth * waveMarginRight) + (inputRectWidth * AngleX));
            }
            else
            {
                outputRect.Left = (int)(inputRects[0].Left + (inputRectWidth * waveMarginLeft) + (inputRectWidth * AngleX));
                outputRect.Right = (int)(inputRects[0].Right - (inputRectWidth * waveMarginRight));
            }

            outputOpaqueSubRect = default(Rectangle);

            return outputRect;
        }

        public void MapOutputRectangleToInputRectangles(Rectangle outputRect, Rectangle[] inputRects)
        {
            //int expansion = (int)Math.Round(constants.Amplitude);
            if (inputRects.Length != 1)
                throw new ArgumentException("InputRects must be length of 1", "inputRects");

            inputRects[0] = inputRectangle;
        }

        public int InputCount
        {
            get { return 1; }
        }

        private void UpdateConstants()
        {
            // Update constant buffer 1 (the first constant buffer available to the effect)
            // with the progress, angle, and size values.

            constants.Matrix = Matrix.RotationX(3); 
            constants.SizeX = SizeX;
            constants.SizeY = SizeY;
            constants.WaveOffset = WaveOffset;
            constants.AngleX = AngleX;
            constants.AngleY = AngleY;

            // Only update the constant buffer if the vertex buffer has been initialized.
            if (drawInformation != null)
            {
                drawInformation.SetVertexConstantBuffer(ref constants);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct WaveEffectConstantBuffer
        {
            public Matrix Matrix;
            public float SizeX;
            public float SizeY;
            public float WaveOffset;
            public float AngleX;
            public float AngleY;
        }
    }
}
