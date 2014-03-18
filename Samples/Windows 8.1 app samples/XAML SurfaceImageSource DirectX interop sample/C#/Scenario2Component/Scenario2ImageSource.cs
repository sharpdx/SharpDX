using System.IO;
using System.Runtime.InteropServices;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;
using InputElement = SharpDX.Direct3D11.InputElement;

namespace Scenario2Component
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ModelViewProjectionConstantBuffer
    {
        public Matrix model;
        public Matrix view;
        public Matrix projection;
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct VertexPositionColor
    {
        public VertexPositionColor(Vector3 pos, Vector3 color)
        {
            this.pos = pos;
            this.color = color;
        }

        public Vector3 pos;
        public Vector3 color;
    };

    public sealed class Scenario2ImageSource : Windows.UI.Xaml.Media.Imaging.SurfaceImageSource
    {
        // Direct3D objects
        private Device d3dDevice;
        private DeviceContext d3dContext;
        private RenderTargetView renderTargetView;
        private DepthStencilView depthStencilView;
        private VertexShader vertexShader;
        private PixelShader pixelShader;
        private InputLayout inputLayout;
        private SharpDX.Direct3D11.Buffer vertexBuffer;
        private SharpDX.Direct3D11.Buffer indexBuffer;
        private SharpDX.Direct3D11.Buffer constantBuffer;

        private ModelViewProjectionConstantBuffer constantBufferData;

        private int indexCount;

        private int width;
        private int height;

        private float frameCount;

        public Scenario2ImageSource(int pixelWidth, int pixelHeight, bool isOpaque)
            : base(pixelWidth, pixelHeight, isOpaque)
        {
            width = pixelWidth;
            height = pixelHeight;

            CreateDeviceResources();

            Application.Current.Suspending += OnSuspending;
        }

        public void BeginDraw()
        {
            // Express target area as a native RECT type.
            var updateRectNative = new Rectangle(0, 0, width, height);

            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
            {
                // Begin drawing - returns a target surface and an offset to use as the top left origin when drawing.
                try
                {
                    Point offset;
                    using (var surface = sisNative.BeginDraw(updateRectNative, out offset))
                    {
                        // QI for target texture from DXGI surface.
                        using (var d3DTexture = surface.QueryInterface<Texture2D>())
                        {
                            Utilities.Dispose(ref renderTargetView);
                            renderTargetView = new RenderTargetView(d3dDevice, d3DTexture);
                        }

                        // Set viewport to the target area in the surface, taking into account the offset returned by BeginDraw.
                        var viewport = new ViewportF(offset.X, offset.Y, width, height);
                        d3dContext.Rasterizer.SetViewport(viewport);

                        // Get the surface description in order to determine its size. The size of the depth/stencil buffer and the RenderTargetView must match, so the 
                        // depth/stencil buffer must be the same size as the surface. Since the whole surface returned by BeginDraw can potentially be much larger than 
                        // the actual update rect area passed into BeginDraw, it may be preferable for some apps to include an intermediate step which instead creates a 
                        // separate smaller D3D texture and renders into it before calling BeginDraw, then simply copies that texture into the surface returned by BeginDraw. 
                        // This would prevent needing to create a depth/stencil buffer which is potentially much larger than required, thereby saving memory at the cost of 
                        // additional overhead due to the copy operation.
                        var surfaceDesc = surface.Description;

                        // Create depth/stencil buffer descriptor.
                        var depthStencilDesc = new Texture2DDescription()
                        {
                            Format = Format.D24_UNorm_S8_UInt,
                            Width = surfaceDesc.Width,
                            Height = surfaceDesc.Height,
                            ArraySize = 1,
                            MipLevels = 1,
                            BindFlags = BindFlags.DepthStencil,
                            SampleDescription = new SampleDescription(1, 0),
                            Usage = ResourceUsage.Default,
                        };

                        // Allocate a 2-D surface as the depth/stencil buffer.
                        using (var depthStencil = new Texture2D(d3dDevice, depthStencilDesc))
                        {
                            Utilities.Dispose(ref depthStencilView);
                            depthStencilView = new DepthStencilView(d3dDevice, depthStencil);
                        }
                    }
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved ||
                        ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                    {
                        // If the device has been removed or reset, attempt to recreate it and continue drawing.
                        CreateDeviceResources();
                        BeginDraw();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void EndDraw()
        {
            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
                sisNative.EndDraw();
        }

        public void Clear(Windows.UI.Color color)
        {
            // Clear render target view with given color.
            d3dContext.ClearRenderTargetView(renderTargetView, ConvertToColorF(color));
        }

        public void RenderNextAnimationFrame()
        {
            var eye = new Vector3(0.0f, 0.7f, 1.5f); // Define camera position.
            var at = new Vector3(0.0f, -0.1f, 0.0f); // Define focus position.
            var up = new Vector3(0.0f, 1.0f, 0.0f); // Define up direction.

            if (frameCount >= float.MaxValue)
            {
                frameCount = 0;
            }

            // Set view based on camera position, focal point, and up direction.
            constantBufferData.view = Matrix.Transpose(Matrix.LookAtRH(eye, at, up));

            // Rotate around Y axis by (pi/4) * 16ms per elapsed frame.
            constantBufferData.model = Matrix.Transpose(Matrix.RotationY(frameCount++*0.016f*MathUtil.PiOverFour));

            // Clear depth/stencil view.
            //d3dContext.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);

            // Set render target.
            //d3dContext.OutputMerger.SetRenderTargets(depthStencilView, renderTargetView);
            d3dContext.OutputMerger.SetRenderTargets(renderTargetView);

            // Map update to constant buffer.
            d3dContext.UpdateSubresource(ref constantBufferData, constantBuffer);

            // Set vertex buffer.
            int stride = Utilities.SizeOf<VertexPositionColor>();
            d3dContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, stride, 0));

            // Set index buffer.
            d3dContext.InputAssembler.SetIndexBuffer(indexBuffer, Format.R16_UInt, 0);

            // Set topology to triangle list.
            d3dContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            // Set input layout.
            d3dContext.InputAssembler.InputLayout = inputLayout;

            // Set vertex shader.
            d3dContext.VertexShader.Set(vertexShader);

            // Set constant buffer.
            d3dContext.VertexShader.SetConstantBuffer(0, constantBuffer);

            // Set pixel shader.
            d3dContext.PixelShader.Set(pixelShader);

            // Draw cube faces.
            d3dContext.DrawIndexed(indexCount, 0, 0);
        }

        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // Hints to the driver that the app is entering an idle state and that its memory can be used temporarily for other apps.
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device3>())
                dxgiDevice.Trim();
        }

        // Initialize hardware-dependent resources.
        private void CreateDeviceResources()
        {
            // This flag adds support for surfaces with a different color channel ordering
            // than the API default. It is required for compatibility with Direct2D.
            var creationFlags = DeviceCreationFlags.BgraSupport;

#if DEBUG
            // If the project is in a debug build, enable debugging via SDK Layers.
            creationFlags |= DeviceCreationFlags.Debug;
#endif

            // This array defines the set of DirectX hardware feature levels this app will support.
            // Note the ordering should be preserved.
            // Don't forget to declare your application's minimum required feature level in its
            // description.  All applications are assumed to support 9.1 unless otherwise stated.
            FeatureLevel[] featureLevels =
            {
                FeatureLevel.Level_11_1,
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1,
            };

            // Create the Direct3D 11 API device object.
            Utilities.Dispose(ref d3dDevice);
            d3dDevice = new Device(DriverType.Hardware, creationFlags, featureLevels);
            d3dContext = d3dDevice.ImmediateContext;

            // Get the Direct3D 11.1 API device.
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                // Query for ISurfaceImageSourceNative interface.
                using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
                {
                    sisNative.Device = dxgiDevice;
                }
            }

            // Load the vertex shader.
            var vsBytecode =
                NativeFile.ReadAllBytes(Path.Combine(Package.Current.InstalledLocation.Path,
                    "Scenario2Component\\SimpleVertexShader.cso"));
            Utilities.Dispose(ref vertexShader);
            vertexShader = new VertexShader(d3dDevice, vsBytecode);

            // Create input layout for vertex shader.
            var vertexDesc = new []
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR", 0, Format.R32G32B32_Float, 12, 0, InputClassification.PerVertexData, 0),
            };

            Utilities.Dispose(ref inputLayout);
            inputLayout = new InputLayout(d3dDevice, vsBytecode, vertexDesc);

            // Load the pixel shader.
            var psBytecode =
                NativeFile.ReadAllBytes(Path.Combine(Package.Current.InstalledLocation.Path, "Scenario2Component\\SimplePixelShader.cso"));
            Utilities.Dispose(ref pixelShader);
            pixelShader = new PixelShader(d3dDevice, psBytecode);

            // Create the constant buffer.
            var constantBufferDesc = new BufferDescription()
            {
                SizeInBytes = Utilities.SizeOf<ModelViewProjectionConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer
            };
            Utilities.Dispose(ref constantBuffer);
            constantBuffer = new Buffer(d3dDevice, constantBufferDesc);

            // Describe the vertices of the cube.
            var cubeVertices = new []
            {
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.0f, 0.0f, 0.0f)),
                new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.0f, 0.0f, 1.0f)),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.0f, 1.0f, 0.0f)),
                new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.5f), new Vector3(0.0f, 1.0f, 1.0f)),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(1.0f, 0.0f, 0.0f)),
                new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(1.0f, 0.0f, 1.0f)),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(1.0f, 1.0f, 0.0f)),
                new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1.0f, 1.0f, 1.0f)),
            };

            var vertexBufferDesc = new BufferDescription()
            {
                SizeInBytes = Utilities.SizeOf<VertexPositionColor>()*cubeVertices.Length,
                BindFlags = BindFlags.VertexBuffer
            };
            Utilities.Dispose(ref vertexBuffer);
            vertexBuffer = Buffer.Create(d3dDevice, cubeVertices, vertexBufferDesc);

            // Describe the cube indices.
            var cubeIndices = new ushort[]
            {
                0, 2, 1, // -x
                1, 2, 3,

                4, 5, 6, // +x
                5, 7, 6,

                0, 1, 5, // -y
                0, 5, 4,

                2, 6, 7, // +y
                2, 7, 3,

                0, 4, 6, // -z
                0, 6, 2,

                1, 3, 7, // +z
                1, 7, 5,
            };
            indexCount = cubeIndices.Length;

            // Create the index buffer.
            var indexBufferDesc = new BufferDescription()
            {
                SizeInBytes = sizeof(ushort) * cubeIndices.Length,
                BindFlags = BindFlags.IndexBuffer
            };
            Utilities.Dispose(ref indexBuffer);
            indexBuffer = Buffer.Create(d3dDevice, cubeIndices, indexBufferDesc);

            // Calculate the aspect ratio and field of view.
            float aspectRatio = (float) width/(float) height;

            float fovAngleY = 70.0f*MathUtil.Pi/180.0f;
            if (aspectRatio < 1.0f)
            {
                fovAngleY /= aspectRatio;
            }

            // Set right-handed perspective projection based on aspect ratio and field of view.
            constantBufferData.projection = Matrix.Transpose(
                Matrix.PerspectiveFovRH(
                    fovAngleY,
                    aspectRatio,
                    0.01f,
                    100.0f
                    )
                );

            // Start animating at frame 0.
            frameCount = 0;
        }

        private static SharpDX.Color ConvertToColorF(Windows.UI.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        private static SharpDX.RectangleF ConvertToRectF(Windows.Foundation.Rect rect)
        {
            return new RectangleF((float) rect.X, (float) rect.Y, (float) rect.Width, (float) rect.Height);
        }
    }
}
