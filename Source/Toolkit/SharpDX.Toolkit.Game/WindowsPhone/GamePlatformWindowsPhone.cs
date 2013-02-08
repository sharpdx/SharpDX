﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    internal class GamePlatformWindowsPhone : GamePlatform, IGraphicsDeviceFactory
    {
        public GamePlatformWindowsPhone(Game game) : base(game)
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
            return new GameWindow[] {new GameWindowWindowsPhoneBackgroundXaml(), new GameWindowWindowsPhoneXaml(), };
        }

        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            var gameWindowBackgroundXaml = gameWindow as GameWindowWindowsPhoneBackgroundXaml;
            if (gameWindowBackgroundXaml != null)
            {

                // Unlike Desktop and WinRT, the list of best devices are completely fixed in WP8 XAML
                // So we return a single element
                var deviceInfo = new GraphicsDeviceInformation
                    {
                        Adapter = gameWindowBackgroundXaml.GraphicsDevice.Adapter,
                        GraphicsProfile = gameWindowBackgroundXaml.GraphicsDevice.Features.Level,
                        PresentationParameters = gameWindowBackgroundXaml.GraphicsDevice.Presenter.Description
                    };

                return new List<GraphicsDeviceInformation>() {deviceInfo};
            }

            var gameWindowXaml = gameWindow as GameWindowWindowsPhoneXaml;
            if (gameWindowXaml != null)
            {

                // Unlike Desktop and WinRT, the list of best devices are completely fixed in WP8 XAML
                // So we return a single element
                var deviceInfo = new GraphicsDeviceInformation
                                 {
                                     Adapter = gameWindowXaml.GraphicsDevice.Adapter,
                                     GraphicsProfile = gameWindowXaml.GraphicsDevice.Features.Level,
                                     PresentationParameters = gameWindowXaml.GraphicsDevice.Presenter.Description
                                 };

                return new List<GraphicsDeviceInformation>() { deviceInfo };
            }

            return base.FindBestDevices(prefferedParameters);
        }

        public override GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var gameWindowBackgroundXaml = gameWindow as GameWindowWindowsPhoneBackgroundXaml;
            if (gameWindowBackgroundXaml != null)
            {
                // We don't have anything else than the GraphicsDevice created for the XAML so return it directly.
                return gameWindowBackgroundXaml.GraphicsDevice;
            }

            var gameWindowXaml = gameWindow as GameWindowWindowsPhoneXaml;
            if (gameWindowXaml != null)
            {
                // We don't have anything else than the GraphicsDevice created for the XAML so return it directly.
                return gameWindowXaml.GraphicsDevice;
            }

            return base.CreateDevice(deviceInformation);
        }
    }
}
#endif