// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit
{
    using System;
    using System.Collections.Generic;

    using SharpDX.Direct3D;
    using SharpDX.DXGI;
    using SharpDX.Toolkit.Graphics;

    /// <summary>The game platform class.</summary>
    internal abstract class GamePlatform : DisposeBase, IGraphicsDeviceFactory, IGamePlatform
    {
        /// <summary>The services.</summary>
        protected IServiceRegistry Services { get; private set; }

        /// <summary>The game.</summary>
        protected readonly Game game;

        /// <summary>Initializes a new instance of the <see cref="GamePlatform"/> class.</summary>
        /// <param name="game">The game.</param>
        protected GamePlatform(Game game)
        {
            this.game = game;
            this.Services = game.Services;
            this.Services.AddService(typeof(IGraphicsDeviceFactory), this);
        }

        /// <summary>The add device automatic list delegate delegate.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="graphicsAdapter">The graphics adapter.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="graphicsDeviceInfos">The graphics device infos.</param>
        protected delegate void AddDeviceToListDelegate(
            GameGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            GraphicsDeviceInformation deviceInfo,
            List<GraphicsDeviceInformation> graphicsDeviceInfos);

        /// <summary>Occurs when [activated].</summary>
        public event EventHandler<EventArgs> Activated;

        /// <summary>Occurs when [deactivated].</summary>
        public event EventHandler<EventArgs> Deactivated;

        /// <summary>Occurs when [exiting].</summary>
        public event EventHandler<EventArgs> Exiting;

        /// <summary>Occurs when [idle].</summary>
        public event EventHandler<EventArgs> Idle;

        /// <summary>Occurs when [resume].</summary>
        public event EventHandler<EventArgs> Resume;

        /// <summary>Occurs when [suspend].</summary>
        public event EventHandler<EventArgs> Suspend;

        /// <summary>Occurs when [window created].</summary>
        public event EventHandler<EventArgs> WindowCreated;
        /// <summary>Gets the default app directory.</summary>
        /// <value>The default app directory.</value>
        public abstract string DefaultAppDirectory { get; }
        /// <summary>Gets or sets a value indicating whether this instance is blocking run.</summary>
        /// <value><see langword="true" /> if this instance is blocking run; otherwise, <see langword="false" />.</value>
        public bool IsBlockingRun { get; protected set; }

        /// <summary>Gets the main window.</summary>
        /// <value>The main window.</value>
        public GameWindow MainWindow { get; protected set; }

        /// <summary>Gets or sets the window context.</summary>
        /// <value>The window context.</value>
        public object WindowContext { get; set; }

        /// <summary>Creates the specified game.</summary>
        /// <param name="game">The game.</param>
        /// <returns>GamePlatform.</returns>
        public static GamePlatform Create(Game game)
        {
#if WIN8METRO
            return new GamePlatformWinRT(game);
#elif WP8
            return new GamePlatformPhone(game);
#else
            return new GamePlatformDesktop(game);
#endif
        }

        /// <summary>Creates the device.</summary>
        /// <param name="deviceInformation">The device information.</param>
        /// <returns>GraphicsDevice.</returns>
        public virtual GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var device = GraphicsDevice.New(deviceInformation.Adapter, deviceInformation.DeviceCreationFlags, deviceInformation.GraphicsProfile);

            var parameters = deviceInformation.PresentationParameters;

            // Give a chance to gameWindow to create desired graphics presenter, otherwise - create our own.
            var presenter = this.MainWindow.CreateGraphicsPresenter(device, parameters) ?? new SwapChainGraphicsPresenter(device, parameters);

            device.Presenter = presenter;

            // Force to resize the gameWindow
            this.MainWindow.Resize(parameters.BackBufferWidth, parameters.BackBufferHeight);

            return device;
        }

        /// <summary>Creates the a new <see cref="GameWindow" />. See remarks.</summary>
        /// <param name="gameContext">The window context. See remarks.</param>
        /// <returns>A new game window.</returns>
        /// <exception cref="System.ArgumentException">Game Window context not supported on this platform</exception>
        /// <remarks>This is currently only supported on Windows Desktop. The window context supported on windows is a subclass of System.Windows.Forms.Control (or null and a default RenderForm will be created).</remarks>
        public virtual GameWindow CreateWindow(GameContext gameContext)
        {
            gameContext = gameContext ?? new GameContext();

            var windows = this.GetSupportedGameWindows();

            foreach(var gameWindowToTest in windows)
            {
                if(gameWindowToTest.CanHandle(gameContext))
                {
                    gameWindowToTest.Services = this.Services;
                    gameWindowToTest.Initialize(gameContext);
                    return gameWindowToTest;
                }
            }

            throw new ArgumentException("Game Window context not supported on this platform");
        }

        /// <summary>Exits this instance.</summary>
        public virtual void Exit()
        {
            this.MainWindow.Exiting = true;
            this.Activated = null;
            this.Deactivated = null;
            this.Exiting = null;
            this.Idle = null;
            this.Resume = null;
            this.Suspend = null;
        }

        /// <summary>Finds the best devices.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <returns>List{GraphicsDeviceInformation}.</returns>
        public virtual List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters preferredParameters)
        {
            var graphicsDeviceInfos = new List<GraphicsDeviceInformation>();

            // Iterate on each adapter
            foreach(var graphicsAdapter in GraphicsAdapter.Adapters) this.TryFindSupportedFeatureLevel(preferredParameters, graphicsAdapter, graphicsDeviceInfos, this.TryAddDeviceWithDisplayMode);

            return graphicsDeviceInfos;
        }

        /// <summary>Runs the specified game context.</summary>
        /// <param name="gameContext">The game context.</param>
        public void Run(GameContext gameContext)
        {
            this.MainWindow = this.CreateWindow(gameContext);

            // set the mouse visibility in case if it was set in the game constructor:
            this.MainWindow.IsMouseVisible = this.game.IsMouseVisible;

            // Register on Activated 
            this.MainWindow.Activated += this.OnActivated;
            this.MainWindow.Deactivated += this.OnDeactivated;
            this.MainWindow.InitCallback = this.game.InitializeBeforeRun;
            this.MainWindow.RunCallback = this.game.Tick;
            this.MainWindow.ExitCallback = () => this.OnExiting(this, EventArgs.Empty);

            var windowCreated = this.WindowCreated;
            if(windowCreated != null) windowCreated(this, EventArgs.Empty);

            this.MainWindow.Run();
        }

        /// <summary>Gets the supported game windows.</summary>
        /// <returns>GameWindow[][].</returns>
        internal abstract GameWindow[] GetSupportedGameWindows();

        /// <summary>Adds the device.</summary>
        /// <param name="mode">The mode.</param>
        /// <param name="deviceBaseInfo">The device base information.</param>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="graphicsDeviceInfos">The graphics device infos.</param>
        protected void AddDevice(
            DisplayMode mode,
            GraphicsDeviceInformation deviceBaseInfo,
            GameGraphicsParameters preferredParameters,
            List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            var deviceInfo = deviceBaseInfo.Clone();

            deviceInfo.PresentationParameters.RefreshRate = mode.RefreshRate;
            deviceInfo.PresentationParameters.PreferredFullScreenOutputIndex = preferredParameters.PreferredFullScreenOutputIndex;
            deviceBaseInfo.PresentationParameters.DepthBufferShaderResource = preferredParameters.DepthBufferShaderResource;

            if(preferredParameters.IsFullScreen)
            {
                deviceInfo.PresentationParameters.BackBufferFormat = mode.Format;
                deviceInfo.PresentationParameters.BackBufferWidth = mode.Width;
                deviceInfo.PresentationParameters.BackBufferHeight = mode.Height;
            }
            else
            {
                deviceInfo.PresentationParameters.BackBufferFormat = preferredParameters.PreferredBackBufferFormat;
                deviceInfo.PresentationParameters.BackBufferWidth = preferredParameters.PreferredBackBufferWidth;
                deviceInfo.PresentationParameters.BackBufferHeight = preferredParameters.PreferredBackBufferHeight;
            }

            // TODO: Handle multisampling / depthstencil format
            deviceInfo.PresentationParameters.DepthStencilFormat = preferredParameters.PreferredDepthStencilFormat;
            deviceInfo.PresentationParameters.MultiSampleCount = MSAALevel.None;

            if(!graphicsDeviceInfos.Contains(deviceInfo)) graphicsDeviceInfos.Add(deviceInfo);
        }

        /// <summary>Adds the device with default display mode.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="graphicsAdapter">The graphics adapter.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="graphicsDeviceInfos">The graphics device infos.</param>
        protected void AddDeviceWithDefaultDisplayMode(
            GameGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            GraphicsDeviceInformation deviceInfo,
            List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            var displayMode = new DisplayMode(
                Format.B8G8R8A8_UNorm,
                this.MainWindow.ClientBounds.Width,
                this.MainWindow.ClientBounds.Height,
                new Rational(60, 1));
            this.AddDevice(displayMode, deviceInfo, preferredParameters, graphicsDeviceInfos);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(this.MainWindow != null)
                {
                    this.MainWindow.Dispose();
                    this.MainWindow = null;
                }
            }
        }

        /// <summary>Called when [activated].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnActivated(object source, EventArgs e)
        {
            var handler = this.Activated;
            if(handler != null) handler(this, e);
        }

        /// <summary>Called when [deactivated].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnDeactivated(object source, EventArgs e)
        {
            var handler = this.Deactivated;
            if(handler != null) handler(this, e);
        }

        /// <summary>Called when [exiting].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnExiting(object source, EventArgs e)
        {
            var handler = this.Exiting;
            if(handler != null) handler(this, e);
        }

        /// <summary>Called when [idle].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnIdle(object source, EventArgs e)
        {
            var handler = this.Idle;
            if(handler != null) handler(this, e);
        }

        /// <summary>Called when [resume].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnResume(object source, EventArgs e)
        {
            var handler = this.Resume;
            if(handler != null) handler(this, e);
        }

        /// <summary>Called when [suspend].</summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void OnSuspend(object source, EventArgs e)
        {
            var handler = this.Suspend;
            if(handler != null) handler(this, e);
        }

        /// <summary>Tries the find supported feature level.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="graphicsAdapter">The graphics adapter.</param>
        /// <param name="graphicsDeviceInfos">The graphics device infos.</param>
        /// <param name="addDelegate">The add delegate.</param>
        protected void TryFindSupportedFeatureLevel(
            GameGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            List<GraphicsDeviceInformation> graphicsDeviceInfos,
            AddDeviceToListDelegate addDelegate)
        {
            // Check if the adapter has an output with the preferred index
            if(preferredParameters.IsFullScreen && graphicsAdapter.OutputsCount <= preferredParameters.PreferredFullScreenOutputIndex) return;

            // Iterate on each preferred graphics profile
            foreach(var featureLevel in preferredParameters.PreferredGraphicsProfile)
            {
                // Check if this profile is supported.
                if(graphicsAdapter.IsProfileSupported(featureLevel))
                {
                    var deviceInfo = this.CreateGraphicsDeviceInformation(preferredParameters, graphicsAdapter, featureLevel);

                    addDelegate(preferredParameters, graphicsAdapter, deviceInfo, graphicsDeviceInfos);

                    // If the profile is supported, we are just using the first best one
                    break;
                }
            }
        }

        /// <summary>Creates the graphics device information.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="graphicsAdapter">The graphics adapter.</param>
        /// <param name="featureLevel">The feature level.</param>
        /// <returns>GraphicsDeviceInformation.</returns>
        private GraphicsDeviceInformation CreateGraphicsDeviceInformation(
            GameGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            FeatureLevel featureLevel)
        {
            return new GraphicsDeviceInformation
                   {
                       Adapter = graphicsAdapter,
                       GraphicsProfile = featureLevel,
                       PresentationParameters =
                       {
                           MultiSampleCount = MSAALevel.None,
                           IsFullScreen = preferredParameters.IsFullScreen,
                           PreferredFullScreenOutputIndex =
                               preferredParameters.PreferredFullScreenOutputIndex,
                           DepthBufferShaderResource = preferredParameters.DepthBufferShaderResource,
                           PresentationInterval =
                               preferredParameters.SynchronizeWithVerticalRetrace
                                   ? PresentInterval.One
                                   : PresentInterval.Immediate,
                           DeviceWindowHandle = this.MainWindow.NativeWindow,
                           RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput
                       }
                   };
        }

        /// <summary>Tries the add device from output.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="output">The output.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="graphicsDeviceInfos">The graphics device infos.</param>
        private void TryAddDeviceFromOutput(
            GameGraphicsParameters preferredParameters,
            GraphicsOutput output,
            GraphicsDeviceInformation deviceInfo,
            List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            if(output.CurrentDisplayMode != null) this.AddDevice(output.CurrentDisplayMode, deviceInfo, preferredParameters, graphicsDeviceInfos);

            if(preferredParameters.IsFullScreen)
            {
                // Get display mode for the particular width, height, pixel format
                foreach(var displayMode in output.SupportedDisplayModes) this.AddDevice(displayMode, deviceInfo, preferredParameters, graphicsDeviceInfos);
            }
        }

        /// <summary>Tries the add device with display mode.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <param name="graphicsAdapter">The graphics adapter.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="graphicsDeviceInfos">The graphics device infos.</param>
        private void TryAddDeviceWithDisplayMode(
            GameGraphicsParameters preferredParameters,
            GraphicsAdapter graphicsAdapter,
            GraphicsDeviceInformation deviceInfo,
            List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            // if we want to switch to fullscreen, try to find only needed output, otherwise check them all
            if(preferredParameters.IsFullScreen)
            {
                if(preferredParameters.PreferredFullScreenOutputIndex < graphicsAdapter.OutputsCount)
                {
                    var output = graphicsAdapter.GetOutputAt(preferredParameters.PreferredFullScreenOutputIndex);
                    this.TryAddDeviceFromOutput(preferredParameters, output, deviceInfo, graphicsDeviceInfos);
                }
            }
            else
            {
                for(var i = 0; i < graphicsAdapter.OutputsCount; i++)
                {
                    var output = graphicsAdapter.GetOutputAt(i);
                    this.TryAddDeviceFromOutput(preferredParameters, output, deviceInfo, graphicsDeviceInfos);
                }
            }
        }
    }
}
