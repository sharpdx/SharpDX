namespace ForegroundSwapChains
{
    using System;
    using SharpDX;
    using SharpDX.DXGI;
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    internal sealed class ForegroundSwapChainGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly KeyboardManager _keyboardManager;

        private BasicEffect _basicEffect;
        private Buffer<VertexPositionColor> _vertices;
        private VertexInputLayout _inputLayout;
        private SwapChain _foregroundChain;

        private RenderTarget2D _foregroundRenderTarget;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private float _scale; // the current scale of the 3D swap chain
        private ViewportF _currentViewport; // the current viewport of the 3D swap chain

        public ForegroundSwapChainGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.DeviceCreationFlags |= SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport;

            // we will create and destroy the foreground swap chain in these event handlers
            _graphicsDeviceManager.DeviceChangeBegin += HandleDeviceChangeBegin;
            _graphicsDeviceManager.DeviceChangeEnd += HandleDeviceChangeEnd;

            // initialize the keyboard manager
            _keyboardManager = new KeyboardManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphicsDeviceManager.ApplyChanges();

            base.Initialize();
        }

        private void HandleDeviceChangeBegin(object sender, EventArgs e)
        {
            // before device changes we need to destroy the old foreground swap chain
            Utilities.Dispose(ref _foregroundRenderTarget);
            Utilities.Dispose(ref _foregroundChain);
        }

        private void HandleDeviceChangeEnd(object sender, EventArgs e)
        {
            var device = _graphicsDeviceManager.GraphicsDevice;

            var swapChain = (SwapChain)device.Presenter.NativePresenter;
            var d = swapChain.Description;

            var d3DDevice = (SharpDX.Direct3D11.Device)device;
            using (var dxgiDevice = d3DDevice.QueryInterface<Device3>())
            {
                var adapter = dxgiDevice.Adapter;
                var factory = adapter.GetParent<Factory2>();

                var description = new SwapChainDescription1
                                  {
                                      Width = d.ModeDescription.Width,
                                      Height = d.ModeDescription.Height,
                                      Format = d.ModeDescription.Format,
                                      Stereo = false,
                                      SampleDescription = new SampleDescription(1, 0),
                                      Usage = Usage.RenderTargetOutput,
                                      BufferCount = 2,
                                      SwapEffect = SwapEffect.FlipSequential,
                                      Flags = SwapChainFlags.ForegroundLayer,
                                      Scaling = Scaling.None,
                                      AlphaMode = AlphaMode.Premultiplied
                                  };

                // create the foreground swap chain for the core window
                using (var comWindow = new ComObject(Window.NativeWindow))
                    _foregroundChain = new SwapChain1(factory, (SharpDX.Direct3D11.Device)device, comWindow, ref description);

                // recreate the foreground render target
                using (var backBuffer = _foregroundChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0))
                    _foregroundRenderTarget = RenderTarget2D.New(device, backBuffer);
            }
        }

        protected override void LoadContent()
        {
            _basicEffect = ToDisposeContent(new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                View = Matrix.LookAtRH(new Vector3(0, 0, 5), new Vector3(0, 0, 0), Vector3.UnitY),
                Projection = Matrix.PerspectiveFovRH(MathUtil.PiOverFour, (float)GraphicsDevice.BackBuffer.Width / GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f),
                World = Matrix.Identity
            });

            _vertices = ToDisposeContent(SharpDX.Toolkit.Graphics.Buffer.Vertex.New(
                GraphicsDevice,
                new[]
                    {
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange), // Back
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.Orange),

                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange), // Front
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.Orange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.Orange),

                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.OrangeRed), // Top
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.OrangeRed),

                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.OrangeRed), // Bottom
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.OrangeRed),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.OrangeRed),

                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.DarkOrange), // Left
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, -1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(-1.0f, 1.0f, 1.0f), Color.DarkOrange),

                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.DarkOrange), // Right
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, -1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, -1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, 1.0f), Color.DarkOrange),
                        new VertexPositionColor(new Vector3(1.0f, 1.0f, -1.0f), Color.DarkOrange),
                    }));
            ToDisposeContent(_vertices);

            _font = Content.Load<SpriteFont>("Font");
            _spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));

            _inputLayout = VertexInputLayout.FromBuffer(0, _vertices);

            SetScaling(1f);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var time = (float)gameTime.TotalGameTime.TotalSeconds;
            _basicEffect.World = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f);

            var ks = _keyboardManager.GetState();

            // increase scaling
            if (ks.IsKeyDown(Keys.Up)) SetScaling(Math.Min(_scale + 0.01f, 1f));
            // decrease scaling
            if (ks.IsKeyDown(Keys.Down)) SetScaling(Math.Max(_scale - 0.01f, 0.1f));

            base.Update(gameTime);
        }

        private void SetScaling(float currentScale)
        {
            var sc = (SwapChain1)GraphicsDevice.Presenter.NativePresenter;
            // query DX11.2 swap chain
            using (var sc2 = sc.QueryInterface<SwapChain2>())
            {
                var w = (int)(GraphicsDevice.BackBuffer.Width * currentScale);
                var h = (int)(GraphicsDevice.BackBuffer.Height * currentScale);
                // set the swap chain scaling (new in DX11.2)
                sc2.SourceSize = new Size2(w, h);
                // we need to set the viewport to the same size as the swap chain scaling region
                _currentViewport = new ViewportF(0, 0, w, h);
                _scale = currentScale;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            // set the view port
            GraphicsDevice.SetViewport(_currentViewport);
            // clear the backbuffer
            GraphicsDevice.Clear(Color.DarkBlue);

            // draw the geometry
            GraphicsDevice.SetVertexBuffer(_vertices);
            GraphicsDevice.SetVertexInputLayout(_inputLayout);

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.Draw(PrimitiveType.TriangleList, _vertices.ElementCount);

            const string format = @"Current render size: {0}x{1}
Controls:
 Up - increase render size
 Down - decrease render size
";

            var text = string.Format(format, _currentViewport.Width, _currentViewport.Height);

            // set the foreground render target
            GraphicsDevice.SetRenderTargets(_foregroundRenderTarget);
            GraphicsDevice.Clear(new Color4(0f, 0f, 0f, 0f));

            // draw the HUD
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, text, new Vector2(20f, 20f), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            base.EndDraw();
            // present the foreground swapchain
            _foregroundChain.Present(1, PresentFlags.None);
        }
    }
}