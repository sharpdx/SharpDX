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
using System.Collections.Generic;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// Represents an implementation of the <see cref="GamePlatform"/> for the Desktop platform.
    /// </summary>
    internal class GamePlatformDesktop : GamePlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlatformDesktop"/> class.
        /// </summary>
        /// <param name="game">The <see cref="Game"/> instance that runs in this platform.</param>
        public GamePlatformDesktop(Game game) : base(game)
        {
        }

        /// <summary>
        /// Returns the default directory name where the current game runs.
        /// </summary>
        public override string DefaultAppDirectory
        {
            get
            {
                var assemblyUri = new Uri(game.GetType().Assembly.CodeBase);
                return Path.GetDirectoryName(assemblyUri.LocalPath);
            }
        }

        /// <summary>
        /// Gets the list of supported game window instances.
        /// </summary>
        /// <returns>The list of supported <see cref="GameWindow"/> instances.</returns>
        /// <remarks>Supports WinForms on any platform, and WPF only on DX11 (DX11.1 and up is not supported for WPF).</remarks>
        internal override GameWindow[] GetSupportedGameWindows()
        {
            return new GameWindow[] { new GameWindowDesktop()
#if !W8CORE && NET35Plus
                , new GameWindowDesktopWpf() 
#endif
            };
        }

        /// <summary>
        /// Gets the list of <see cref="GraphicsDeviceInformation"/> that correspond to the provided <see cref="GameGraphicsParameters"/>.
        /// </summary>
        /// <param name="prefferedParameters">The preferred parameters for devices to support.</param>
        /// <returns>The list of supported <see cref="GraphicsDeviceInformation"/> that match provided <see cref="GameGraphicsParameters"/>.</returns>
        /// <remarks>If <see cref="GamePlatform.FindBestDevices"/> doesn't return any devices - adds the first available <see cref="GraphicsAdapter"/>.</remarks>
        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            var graphicsDeviceInfos = base.FindBestDevices(prefferedParameters);

            // Special case where the default FindBestDevices is not working (for example via RemoteDesktop)
            if (graphicsDeviceInfos.Count == 0)
            {
                var graphicsAdapter = GraphicsAdapter.Adapters[0];

                TryFindSupportedFeatureLevel(prefferedParameters, graphicsAdapter, graphicsDeviceInfos, AddDeviceWithDefaultDisplayMode);
            }

            return graphicsDeviceInfos;
        }
    }
}
#endif