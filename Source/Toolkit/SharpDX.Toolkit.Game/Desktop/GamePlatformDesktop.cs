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
#if !W8CORE
using System;
using System.IO;
using System.Reflection;

namespace SharpDX.Toolkit
{
    using System.Collections.Generic;
    using DXGI;
    using Graphics;

    /// <summary>The game platform desktop class.</summary>
    internal class GamePlatformDesktop : GamePlatform
    {
        /// <summary>Initializes a new instance of the <see cref="GamePlatform" /> class.</summary>
        /// <param name="game">The game.</param>
        public GamePlatformDesktop(Game game) : base(game)
        {
            IsBlockingRun = true;
        }

        /// <summary>Gets the default app directory.</summary>
        /// <value>The default app directory.</value>
        public override string DefaultAppDirectory
        {
            get
            {
                var assemblyUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                return Path.GetDirectoryName(assemblyUri.LocalPath);
            }
        }

        /// <summary>Gets the supported game windows.</summary>
        /// <returns>GameWindow[][].</returns>
        internal override GameWindow[] GetSupportedGameWindows()
        {
            return new GameWindow[] { new GameWindowDesktop()
#if !W8CORE && NET35Plus && !DIRECTX11_1
                , new GameWindowDesktopWpf() 
#endif
            };
        }

        /// <summary>Finds the best devices.</summary>
        /// <param name="preferredParameters">The preferred parameters.</param>
        /// <returns>List{GraphicsDeviceInformation}.</returns>
        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters preferredParameters)
        {
            var graphicsDeviceInfos = base.FindBestDevices(preferredParameters);

            // Special case where the default FindBestDevices is not working
            if (graphicsDeviceInfos.Count == 0)
            {
                var graphicsAdapter = GraphicsAdapter.Adapters[0];

                TryFindSupportedFeatureLevel(preferredParameters, graphicsAdapter, graphicsDeviceInfos, AddDeviceWithDefaultDisplayMode);
            }

            return graphicsDeviceInfos;
        }
    }
}
#endif