namespace TiledResources
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using SharpDX;
    using SharpDX.Direct3D11;
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Input;
    using Keys = SharpDX.Toolkit.Input.Keys;

    /// <summary>
    /// Game class for this sample
    /// </summary>
    internal sealed class SampleGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly OrbitCamera _camera;
        private readonly KeyboardManager _keyboardManager;

        private TerrainRenderer _terrainRenderer;
        private SamplingRenderer _samplingRenderer;
        private ResidencyManager _residencyManager;

        private bool _isBorderEnabled;
        private KeyboardState _previousKeyboardState;

        private bool _shouldExit;

        public SampleGame()
        {
            // create the graphics device manager and set desired screen resolution
            _graphicsDeviceManager = new GraphicsDeviceManager(this)
                                     {
                                         PreferredBackBufferWidth = 1440,
                                         PreferredBackBufferHeight = 900
                                     };

            // give a chance to override selected graphics device settings (for example, to use a WARP adapter)
            _graphicsDeviceManager.PreparingDeviceSettings += HandlePreparingDeviceSettings;

            // initialize the camera
            _camera = new OrbitCamera
                      {
                          Radius = 2f,
                          Target = new Vector3()
                      };

            // initialize the keyboard manager
            _keyboardManager = new KeyboardManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // check tiled resources support:
            var device = (Device)_graphicsDeviceManager.GraphicsDevice;
            var features = device.CheckD3D112Feature();

            if (features.TiledResourcesTier == TiledResourcesTier.NotSupported)
            {
                System.Windows.Forms.MessageBox.Show("Tiled resources are not supported on current graphics device. Exiting...");
                _shouldExit = true;
                Exit();
            }

            // check if needed resources are present:
            var vertexBufferExists = DataFileExists(SampleSettings.VertexBufferFile);
            var indexBufferExists = DataFileExists(SampleSettings.IndexBufferFile);
            var diffuseTextureExists = DataFileExists(SampleSettings.DiffuseTextureFile);
            var normalTextureExists = DataFileExists(SampleSettings.NormalTextureFile);

            var isSomethingMissing = !(vertexBufferExists && indexBufferExists && diffuseTextureExists && normalTextureExists);
            if (isSomethingMissing)
            {
                const string tiledResourcesUrl = "http://tiledresources.codeplex.com/wikipage?title=Additional%20Dependencies&referringTitle=Home";

                var sb = new StringBuilder();
                sb.AppendLine("Some data files are missing.");
                sb.AppendLine("Full data path is: " + Path.GetFullPath(SampleSettings.DataPath));
                if (!vertexBufferExists) sb.AppendLine("Missing file: " + SampleSettings.VertexBufferFile);
                if (!indexBufferExists) sb.AppendLine("Missing file: " + SampleSettings.IndexBufferFile);
                if (!diffuseTextureExists) sb.AppendLine("Missing file: " + SampleSettings.DiffuseTextureFile);
                if (!normalTextureExists) sb.AppendLine("Missing file: " + SampleSettings.NormalTextureFile);
                sb.AppendLine("Either place these files at indicated path or fix the path in the class 'SampleSettings'.");
                sb.AppendLine("Data download instructions are at: " + tiledResourcesUrl);
                sb.AppendLine("Press YES to open the link above in browser.");

                var result = System.Windows.Forms.MessageBox.Show(sb.ToString(),
                                                                  "Requried data not found",
                                                                  System.Windows.Forms.MessageBoxButtons.YesNo,
                                                                  System.Windows.Forms.MessageBoxIcon.Error);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    System.Diagnostics.Process.Start(tiledResourcesUrl);

                _shouldExit = true;
                Exit();
            }
        }

        protected override void LoadContent()
        {
            if (_shouldExit) return;

            _camera.SetAspectRatio(GraphicsDevice.BackBuffer.Width / (float)GraphicsDevice.BackBuffer.Height);

            // create all needed components
            _terrainRenderer = ToDisposeContent(new TerrainRenderer(_graphicsDeviceManager, Content));
            _samplingRenderer = ToDisposeContent(new SamplingRenderer(_graphicsDeviceManager, Content));
            _residencyManager = ToDisposeContent(new ResidencyManager(_graphicsDeviceManager, Content));

            // schedule the loading of the structures that needs to be used right now
            var createTexturesTask = _terrainRenderer.CreateTexturesAsync();
            var loadStructuresTask = _residencyManager.LoadStructuresAsync();

            // schedule the loading of other data
            var terrainRendererTask = _terrainRenderer.LoadStructuresAsync();
            var samplingRendererTask = _samplingRenderer.LoadStructuresAsync();

            // wait for loading of critical structures that will be used below
            Task.WaitAll(createTexturesTask, loadStructuresTask);

            // prepare the tiled textures
            _terrainRenderer.DiffuseResidencyMap = _residencyManager.ManageTexture(_terrainRenderer.DiffuseTexture,
                                                                                   Path.Combine(SampleSettings.DataPath, SampleSettings.DiffuseTextureFile));

            _terrainRenderer.NormalResidencyMap = _residencyManager.ManageTexture(_terrainRenderer.NormalTexture,
                                                                                  Path.Combine(SampleSettings.DataPath, SampleSettings.NormalTextureFile));

            var initializeManagedResourcesTask = _residencyManager.InitializeManagedResourcesAsync();

            // wait for all content to load
            Task.WaitAll(terrainRendererTask, samplingRendererTask, initializeManagedResourcesTask);

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(SharpDX.Toolkit.Graphics.ClearOptions.DepthBuffer | SharpDX.Toolkit.Graphics.ClearOptions.Target, new Color4(0), 1f, 0);

            _samplingRenderer.SamplingEffect.Update(_camera);
            _terrainRenderer.TerrainEffect.Update(_camera);

            // render the sampling pass
            _samplingRenderer.SetTargetsForSampling();
            _terrainRenderer.Draw(_samplingRenderer.SamplingEffect);

            // render the actual drawing pass
            _terrainRenderer.SetTargetsForRendering(_camera);
            _terrainRenderer.Draw();

            // render the sampling visualisation
            _samplingRenderer.RenderVisualisation();

            // collect the samples to determine which tiles to load next
            var samples = _samplingRenderer.CollectSamples();
            // prepare new tiles for loading
            _residencyManager.EnqueueSamples(samples);
            // process all enqueued tiels
            _residencyManager.ProcessQueues();
            // render residency visualization
            _residencyManager.RenderVisualization();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // handle input
            var keyboardState = _keyboardManager.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) && _previousKeyboardState.IsKeyUp(Keys.Escape)) Exit();

            if (keyboardState.IsKeyDown(Keys.T) && _previousKeyboardState.IsKeyUp(Keys.T))
            {
                _isBorderEnabled = !_isBorderEnabled;
                _residencyManager.SetBorderMode(_isBorderEnabled);
                _residencyManager.Reset();
            }

            var time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.A)) _camera.Left(time);
            if (keyboardState.IsKeyDown(Keys.D)) _camera.Right(time);
            if (keyboardState.IsKeyDown(Keys.W)) _camera.Up(time);
            if (keyboardState.IsKeyDown(Keys.S)) _camera.Down(time);
            if (keyboardState.IsKeyDown(Keys.Q)) _camera.ZoomIn(time);
            if (keyboardState.IsKeyDown(Keys.E)) _camera.ZoomOut(time);

            _previousKeyboardState = keyboardState;
        }

        private void HandlePreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var info = e.GraphicsDeviceInformation;
            // uncomment this line to use the WARP adapter
            //info.Adapter = AdapterSelector.FintMatchingWarpAdapter();
            info.DeviceCreationFlags |= DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug;
        }

        private bool DataFileExists(string file)
        {
            return File.Exists(Path.Combine(SampleSettings.DataPath, file));
        }
    }
}