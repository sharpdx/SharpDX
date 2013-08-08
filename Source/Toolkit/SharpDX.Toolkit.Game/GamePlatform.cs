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

using System;
using System.Collections.Generic;

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    using Direct3D11;

    internal abstract class GamePlatform : DisposeBase, IGraphicsDeviceFactory, IGamePlatform
    {
        protected delegate void AddDeviceToListDelegate(GameGraphicsParameters prefferedParameters,
                                                        GraphicsAdapter graphicsAdapter,
                                                        GraphicsDeviceInformation deviceInfo,
                                                        List<GraphicsDeviceInformation> graphicsDeviceInfos);

        protected readonly Game game;

        protected readonly IServiceRegistry Services;

        protected GameWindow gameWindow;

        protected GamePlatform(Game game)
        {
            this.game = game;
            Services = game.Services;
            Services.AddService(typeof(IGraphicsDeviceFactory), this);
        }

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

        public abstract string DefaultAppDirectory { get; }

        public object WindowContext { get; set; }

        public event EventHandler<EventArgs> Activated;

        public event EventHandler<EventArgs> Deactivated;

        public event EventHandler<EventArgs> Exiting;

        public event EventHandler<EventArgs> Idle;

        public event EventHandler<EventArgs> Resume;

        public event EventHandler<EventArgs> Suspend;

        public event EventHandler<EventArgs> WindowCreated;

        public GameWindow MainWindow
        {
            get
            {
                return gameWindow;
            }
        }

        internal abstract GameWindow[] GetSupportedGameWindows();

        public virtual GameWindow CreateWindow(GameContext gameContext)
        {
            gameContext = gameContext ?? new GameContext();

            var windows = GetSupportedGameWindows();

            foreach (var gameWindowToTest in windows)
            {
                if (gameWindowToTest.CanHandle(gameContext))
                {
                    gameWindowToTest.Services = Services;
                    gameWindowToTest.Initialize(gameContext);
                    return gameWindowToTest;
                }
            }

            throw new ArgumentException("Game Window context not supported on this platform");
        }

        public bool IsBlockingRun { get; protected set; }

        public void Run(GameContext gameContext)
        {
            gameWindow = CreateWindow(gameContext);

            // Register on Activated 
            gameWindow.Activated += OnActivated;
            gameWindow.Deactivated += OnDeactivated;
            gameWindow.InitCallback = game.InitializeBeforeRun;
            gameWindow.RunCallback = game.Tick;
            gameWindow.ExitCallback = () => OnExiting(this, EventArgs.Empty);

            var windowCreated = WindowCreated;
            if (windowCreated != null)
            {
                windowCreated(this, EventArgs.Empty);
            }

            gameWindow.Run();
        }

        public virtual void Exit()
        {
            gameWindow.Exiting = true;
            Activated = null;
            Deactivated = null;
            Exiting = null;
            Idle = null;
            Resume = null;
            Suspend = null;
        }

        protected void OnActivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null) handler(this, e);
        }

        protected void OnDeactivated(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null) handler(this, e);
        }

        protected void OnExiting(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Exiting;
            if (handler != null) handler(this, e);
        }

        protected void OnIdle(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Idle;
            if (handler != null) handler(this, e);
        }

        protected void OnResume(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Resume;
            if (handler != null) handler(this, e);
        }

        protected void OnSuspend(object source, EventArgs e)
        {
            EventHandler<EventArgs> handler = Suspend;
            if (handler != null) handler(this, e);
        }

        protected void AddDevice(DisplayMode mode, GraphicsDeviceInformation deviceBaseInfo, GameGraphicsParameters prefferedParameters, List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            var deviceInfo = deviceBaseInfo.Clone();

            deviceInfo.PresentationParameters.RefreshRate = mode.RefreshRate;
            deviceInfo.PresentationParameters.PreferredFullScreenOutputIndex = prefferedParameters.PreferredFullScreenOutputIndex;
            deviceBaseInfo.PresentationParameters.DepthBufferShaderResource = prefferedParameters.DepthBufferShaderResource;

            if (prefferedParameters.IsFullScreen)
            {
                deviceInfo.PresentationParameters.BackBufferFormat = mode.Format;
                deviceInfo.PresentationParameters.BackBufferWidth = mode.Width;
                deviceInfo.PresentationParameters.BackBufferHeight = mode.Height;
            }
            else
            {
                deviceInfo.PresentationParameters.BackBufferFormat = prefferedParameters.PreferredBackBufferFormat;
                deviceInfo.PresentationParameters.BackBufferWidth = prefferedParameters.PreferredBackBufferWidth;
                deviceInfo.PresentationParameters.BackBufferHeight = prefferedParameters.PreferredBackBufferHeight;
            }

            // TODO: Handle multisampling / depthstencil format
            deviceInfo.PresentationParameters.DepthStencilFormat = prefferedParameters.PreferredDepthStencilFormat;
            deviceInfo.PresentationParameters.MultiSampleCount = MSAALevel.None;

            if (!graphicsDeviceInfos.Contains(deviceInfo))
            {
                graphicsDeviceInfos.Add(deviceInfo);
            }
        }

        public virtual List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            var graphicsDeviceInfos = new List<GraphicsDeviceInformation>();

            // Iterate on each adapter
            foreach (var graphicsAdapter in GraphicsAdapter.Adapters)
            {
                TryFindSupportedFeatureLevel(prefferedParameters, graphicsAdapter, graphicsDeviceInfos, TryAddDeviceWithDisplayMode);
            }

            return graphicsDeviceInfos;
        }

        public virtual GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var device = GraphicsDevice.New(deviceInformation.Adapter, deviceInformation.DeviceCreationFlags, deviceInformation.GraphicsProfile);

            var parameters = deviceInformation.PresentationParameters;

            // Give a chance to gameWindow to create desired graphics presenter, otherwise - create our own.
            var presenter = gameWindow.CreateGraphicsPresenter(device, parameters)
                            ?? new SwapChainGraphicsPresenter(device, parameters);

            device.Presenter = presenter;

            // Force to resize the gameWindow
            gameWindow.Resize(parameters.BackBufferWidth, parameters.BackBufferHeight);

            return device;
        }

        protected void TryFindSupportedFeatureLevel(GameGraphicsParameters prefferedParameters,
                                                    GraphicsAdapter graphicsAdapter,
                                                    List<GraphicsDeviceInformation> graphicsDeviceInfos,
                                                    AddDeviceToListDelegate addDelegate)
        {
            // Check if the adapter has an output with the preffered index
            if (prefferedParameters.IsFullScreen && graphicsAdapter.OutputsCount <= prefferedParameters.PreferredFullScreenOutputIndex)
                return;

            // Iterate on each preferred graphics profile
            foreach (var featureLevel in prefferedParameters.PreferredGraphicsProfile)
            {
                // Check if this profile is supported.
                if (graphicsAdapter.IsProfileSupported(featureLevel))
                {
                    var deviceInfo = CreateGraphicsDeviceInformation(prefferedParameters, graphicsAdapter, featureLevel);

                    addDelegate(prefferedParameters, graphicsAdapter, deviceInfo, graphicsDeviceInfos);

                    // If the profile is supported, we are just using the first best one
                    break;
                }
            }
        }

        private GraphicsDeviceInformation CreateGraphicsDeviceInformation(GameGraphicsParameters prefferedParameters,
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
                           IsFullScreen = prefferedParameters.IsFullScreen,
                           PreferredFullScreenOutputIndex = prefferedParameters.PreferredFullScreenOutputIndex,
                           DepthBufferShaderResource = prefferedParameters.DepthBufferShaderResource,
                           PresentationInterval = prefferedParameters.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate,
                           DeviceWindowHandle = MainWindow.NativeWindow,
                           RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput
                       }
                   };
        }

        private void TryAddDeviceWithDisplayMode(GameGraphicsParameters prefferedParameters,
                                                 GraphicsAdapter graphicsAdapter,
                                                 GraphicsDeviceInformation deviceInfo,
                                                 List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            // if we want to switch to fullscreen, try to find only needed output, otherwise check them all
            if (prefferedParameters.IsFullScreen)
            {
                if (prefferedParameters.PreferredFullScreenOutputIndex < graphicsAdapter.OutputsCount)
                {
                    var output = graphicsAdapter.GetOutputAt(prefferedParameters.PreferredFullScreenOutputIndex);
                    TryAddDeviceFromOutput(prefferedParameters, output, deviceInfo, graphicsDeviceInfos);
                }
            }
            else
            {
                for (var i = 0; i < graphicsAdapter.OutputsCount; i++)
                {
                    var output = graphicsAdapter.GetOutputAt(i);
                    TryAddDeviceFromOutput(prefferedParameters, output, deviceInfo, graphicsDeviceInfos);
                }
            }


        }

        private void TryAddDeviceFromOutput(GameGraphicsParameters prefferedParameters,
                                            GraphicsOutput output,
                                            GraphicsDeviceInformation deviceInfo,
                                            List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            if (output.CurrentDisplayMode != null)
                AddDevice(output.CurrentDisplayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);

            if (prefferedParameters.IsFullScreen)
            {
                // Get display mode for the particular width, height, pixelformat
                foreach (var displayMode in output.SupportedDisplayModes)
                    AddDevice(displayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);
            }
        }

        protected void AddDeviceWithDefaultDisplayMode(GameGraphicsParameters prefferedParameters, GraphicsAdapter graphicsAdapter, GraphicsDeviceInformation deviceInfo, List<GraphicsDeviceInformation> graphicsDeviceInfos)
        {
            var displayMode = new DisplayMode(DXGI.Format.B8G8R8A8_UNorm, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height, new Rational(60, 1));
            AddDevice(displayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (gameWindow != null)
                {
                    gameWindow.Dispose();
                    gameWindow = null;
                }
            }
        }
    }
}