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
#if WIN8METRO
using System.Collections.Generic;

using SharpDX.DXGI;
using SharpDX.Toolkit.Graphics;
using Windows.ApplicationModel;

namespace SharpDX.Toolkit
{
    internal class GamePlatformWinRT : GamePlatform, IGraphicsDeviceFactory
    {
        public GamePlatformWinRT(Game game) : base(game)
        {
            Services.AddService(typeof(IGraphicsDeviceFactory), this);
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
            return new GameWindow[] { new GameWindowWinRT() };
        }

        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            var graphicsDeviceInfos = base.FindBestDevices(prefferedParameters);

            // Special case where the default FindBestDevices is not working
            if (graphicsDeviceInfos.Count == 0)
            {
                var graphicsAdapter = GraphicsAdapter.Adapters[0];

                // Iterate on each preferred graphics profile
                foreach (var featureLevel in prefferedParameters.PreferredGraphicsProfile)
                {
                    // Check if this profile is supported.
                    if (graphicsAdapter.IsProfileSupported(featureLevel))
                    {
                        var deviceInfo = new GraphicsDeviceInformation
                                             {
                                                 Adapter = graphicsAdapter,
                                                 GraphicsProfile = featureLevel,
                                                 PresentationParameters =
                                                     {
                                                         MultiSampleCount = MSAALevel.None,
                                                         IsFullScreen = prefferedParameters.IsFullScreen,
                                                         PresentationInterval = prefferedParameters.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate,
                                                         DeviceWindowHandle = MainWindow.NativeWindow,
                                                         RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput
                                                     }
                                             };

                        // Hardcoded format and refresh rate...
                        // This is a workaround to allow this code to work inside the emulator
                        // but this is not really robust
                        // TODO: Check how to handle this case properly
                        var displayMode = new DisplayMode(DXGI.Format.B8G8R8A8_UNorm, gameWindow.ClientBounds.Width, gameWindow.ClientBounds.Height, new Rational(60, 1));
                        AddDevice(graphicsAdapter, displayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);

                        // If the profile is supported, we are just using the first best one
                        break;
                    }
                }
            }

            return graphicsDeviceInfos;
        }
    }
}
#endif