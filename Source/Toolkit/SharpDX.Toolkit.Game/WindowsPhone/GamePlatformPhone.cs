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
#if WP8
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Phone.Shell;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;
using Windows.ApplicationModel;

namespace SharpDX.Toolkit
{
    internal class GamePlatformPhone : GamePlatform, IGraphicsDeviceFactory
    {
        public GamePlatformPhone(Game game) : base(game)
        {
        }

        public override string DefaultAppDirectory
        {
            get
            {
                return Package.Current.InstalledLocation.Path;
            }
        }

        internal override GameWindow[] GetSupportedGameWindows()
        {
            return new GameWindow[] {new GameWindowPhoneBackgroundXaml(), new GameWindowPhoneXaml(), };
        }

        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            var gameWindowBackgroundXaml = gameWindow as GameWindowPhoneBackgroundXaml;
            if (gameWindowBackgroundXaml != null)
            {
                // Make sure that we have the single graphics device created by the BackgroundXaml
                gameWindowBackgroundXaml.RequestDepthFormat = prefferedParameters.PreferredDepthStencilFormat;
                var graphicsDevice = gameWindowBackgroundXaml.EnsureDevice();

                // Unlike Desktop and WinRT, the list of best devices are completely fixed in WP8 XAML
                // So we return a single element
                var deviceInfo = new GraphicsDeviceInformation
                    {
                        Adapter = graphicsDevice.Adapter,
                        GraphicsProfile = graphicsDevice.Features.Level,
                        PresentationParameters = graphicsDevice.Presenter.Description
                    };

                return new List<GraphicsDeviceInformation>() { deviceInfo };
            }

            return base.FindBestDevices(prefferedParameters);
        }

        public override GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var gameWindowBackgroundXaml = gameWindow as GameWindowPhoneBackgroundXaml;
            if (gameWindowBackgroundXaml != null)
            {
                // We don't have anything else than the GraphicsDevice created for the XAML so return it directly.
                return gameWindowBackgroundXaml.EnsureDevice();
            }

            // Else this is a DrawingSruface 
            var device = GraphicsDevice.New(deviceInformation.Adapter, deviceInformation.DeviceCreationFlags, deviceInformation.GraphicsProfile);

            CreatePresenter(device, deviceInformation.PresentationParameters);

            return device;
        }

        protected override void CreatePresenter(GraphicsDevice device, PresentationParameters parameters, object newControl = null)
        {
            if(!(gameWindow is GameWindowPhoneXaml)) return;

            parameters = TryGetParameters(device, parameters);

            DisposeGraphicsPresenter(device);

            var renderTargetDesc = new Texture2DDescription
            {
                Format = Format.B8G8R8A8_UNorm,
                Width = parameters.BackBufferWidth,
                Height = parameters.BackBufferHeight,
                ArraySize = 1,
                MipLevels = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Usage = ResourceUsage.Default,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.SharedKeyedmutex | ResourceOptionFlags.SharedNthandle,
                SampleDescription = new DXGI.SampleDescription(1, 0)
            };

            var backBuffer = RenderTarget2D.New(device, renderTargetDesc);

            var graphicsPresenter = new RenderTargetGraphicsPresenter(device, backBuffer, parameters.DepthStencilFormat, true);
            device.Presenter = graphicsPresenter;

            var gameWindowXaml = (GameWindowPhoneXaml)gameWindow;
            gameWindowXaml.CreateSynchronizedTexture(backBuffer);
        }
    }
}
#endif