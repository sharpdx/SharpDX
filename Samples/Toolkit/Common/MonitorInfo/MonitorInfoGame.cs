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
using System.Linq;
using System.Text;
using SharpDX.Toolkit.Input;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;

namespace MonitorInfo
{
    using SharpDX.DXGI;

    /// <summary>
    /// This sample demonstrates how to read monitor and GPU information and allows switching between them
    /// </summary>
    public class MonitorInfoGame : Game
    {
        /// <summary>
        /// Holds information about gathered monitor information to be able to switch it correctly
        /// </summary>
        private sealed class ModeInfo
        {
            public ModeInfo(GraphicsAdapter adapter, int outputIndex, bool isFullScreen, int width, int height)
            {
                Adapter = adapter;
                OutputIndex = outputIndex;
                IsFullScreen = isFullScreen;
                Width = width;
                Height = height;
            }

            public readonly GraphicsAdapter Adapter; // associated GPU
            public readonly int OutputIndex; // monitor index in the GPU
            public readonly bool IsFullScreen; // whether to switch to full screen or not
            public readonly int Width; // resolution width in pixels
            public readonly int Height; // resolution height in pixels

            // equality members were implemented to cover scenario when there are several GPUs and LoadContent was called
            // several times - this will avoid comparing ModeInfo by reference to ensure that correct mode was selected

            #region Equality members

            private bool Equals(ModeInfo other)
            {
                return ((Adapter)Adapter).NativePointer == ((Adapter)other.Adapter).NativePointer
                       && OutputIndex == other.OutputIndex
                       && IsFullScreen.Equals(other.IsFullScreen)
                       && Width == other.Width
                       && Height == other.Height;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ModeInfo && Equals((ModeInfo)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = (Adapter != null ? ((Adapter)Adapter).NativePointer.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ OutputIndex;
                    hashCode = (hashCode * 397) ^ IsFullScreen.GetHashCode();
                    hashCode = (hashCode * 397) ^ Width;
                    hashCode = (hashCode * 397) ^ Height;
                    return hashCode;
                }
            }

            public static bool operator ==(ModeInfo left, ModeInfo right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ModeInfo left, ModeInfo right)
            {
                return !Equals(left, right);
            }

            #endregion
        }

        private readonly GraphicsDeviceManager graphicsDeviceManager; // mandatory
        private readonly KeyboardManager keyboardManager; // we will process keyboard input here

        private KeyboardState keyboardState;

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private string text;

        private readonly List<ModeInfo> availableModes = new List<ModeInfo>();

        private ModeInfo currentMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorInfoGame" /> class.
        /// </summary>
        public MonitorInfoGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // handling of this event allows to force usage of a specific adapter
            graphicsDeviceManager.PreparingDeviceSettings += HandlePreparingDeviceSettings;

            // initial, windowed resolution
            graphicsDeviceManager.PreferredBackBufferWidth = 1024;
            graphicsDeviceManager.PreferredBackBufferHeight = 768;

            // all initial components should be created in game constructor
            keyboardManager = new KeyboardManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Monitor information";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = ToDisposeContent(new SpriteBatch(GraphicsDevice));
            font = Content.Load<SpriteFont>("CourierNew");

            var sb = new StringBuilder();

            // analyze first the current graphics adapter
            var currentAdapter = GraphicsDevice.Adapter;

            // store information about the initial mode
            currentMode = new ModeInfo(currentAdapter,
                                       graphicsDeviceManager.PreferredFullScreenOutputIndex,
                                       graphicsDeviceManager.IsFullScreen,
                                       graphicsDeviceManager.PreferredBackBufferWidth,
                                       graphicsDeviceManager.PreferredBackBufferHeight);

            availableModes.Clear(); // LoadContent can be called several times, clear the modes list
            availableModes.Add(currentMode);

            // append information about current graphics adapter
            sb.AppendLine("Current graphics adapter:");
            AppendAdapterInfo(sb, currentAdapter);

            // check if we have more graphics adapters
            var otherGraphicsAdapters = GraphicsAdapter.Adapters.Where(x => x != currentAdapter).ToList();
            if (otherGraphicsAdapters.Count > 0)
            {
                // iterate all available graphics adapters and append their information
                sb.AppendLine();
                sb.AppendLine("Other graphics adapters:");
                foreach (var adapter in otherGraphicsAdapters)
                {
                    AppendAdapterInfo(sb, adapter);
                    sb.AppendLine();
                }
            }

            // append the information about available controls
            sb.AppendLine("Press:");
            for (var i = 0; i < availableModes.Count; i++)
            {
                var mode = availableModes[i];
                sb.AppendFormat("  {0} - {1}, Monitor {2}, {3}x{4}, {5}{6}",
                                i,
                                mode.Adapter.Description.Description,
                                mode.OutputIndex,
                                mode.Width,
                                mode.Height,
                                mode.IsFullScreen ? "Fullscreen" : "Windowed",
                                Environment.NewLine);
            }

            sb.AppendLine("  ESC - to quit");

            text = sb.ToString();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, null);
            spriteBatch.DrawString(font, text, new Vector2(20, 20), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // update keyboard state
            keyboardState = keyboardManager.GetState();

            // if Esc is pressed - quit program
            if (keyboardState.IsKeyPressed(Keys.Escape))
            {
                Exit();
                return;
            }

            // numer keys (NOT numpad ones) have name like D0, D1, etc...
            // associate available modes each with its key
            for (int i = 0; i < availableModes.Count; i++)
            {
                var key = (Keys)Enum.Parse(typeof(Keys), "D" + i);
                if (keyboardState.IsKeyPressed(key))
                {
                    ApplyMode(availableModes[i]);
                    return;
                }
            }
        }

        private void HandlePreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            // override the current graphics adapter
            if (currentMode != null)
                e.GraphicsDeviceInformation.Adapter = currentMode.Adapter;
        }

        /// <summary>
        /// Applies the current render mode - fullscreen/windowed, resolution and monitor index
        /// </summary>
        /// <param name="mode"></param>
        private void ApplyMode(ModeInfo mode)
        {
            if (currentMode == mode) return;

            currentMode = mode;

            graphicsDeviceManager.PreferredBackBufferWidth = mode.Width;
            graphicsDeviceManager.PreferredBackBufferHeight = mode.Height;
            graphicsDeviceManager.IsFullScreen = mode.IsFullScreen;
            graphicsDeviceManager.PreferredFullScreenOutputIndex = mode.OutputIndex;

            // graphics adapter will be set during this call in a handler of the 'GraphicsDeviceManager.PreparingDeviceSettings' event.
            graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Appends the information about current graphics adapter and its output and adds it to the list of available graphics modes
        /// </summary>
        /// <param name="sb">Where to write the information</param>
        /// <param name="adapter">The adapter whose information needs to be analyzed</param>
        private void AppendAdapterInfo(StringBuilder sb, GraphicsAdapter adapter)
        {
            var adapterDescription = adapter.Description;

            // general adapter information
            sb.AppendLine(adapterDescription.Description);
            // onboard video RAM
            sb.AppendFormat("VRAM             : {0}MiB{1}", ToMB(adapterDescription.DedicatedVideoMemory), Environment.NewLine);
            // OS RAM dedicated to the adapter (typical for integrated GPUs)
            sb.AppendFormat("Dedicated OS RAM : {0}MiB{1}", ToMB(adapterDescription.DedicatedSystemMemory), Environment.NewLine);
            // OS RAM that can be shared with the adapter (for example, 'Turbo Cache' for NVidia GPUs)
            sb.AppendFormat("Shared OS RAM    : {0}MiB{1}", ToMB(adapterDescription.SharedSystemMemory), Environment.NewLine);

            // iterate trough all outputs attached to this adapter
            for (var i = 0; i < adapter.OutputsCount; i++)
            {
                // write its information
                var output = adapter.GetOutputAt(i);
                sb.AppendFormat("Output {0}; ", i);

                var description = ((Output)output).Description;
                var desktopBounds = description.DesktopBounds;

                sb.AppendFormat("{0}; Attached to desktop: {1}; Desktop bounds: ({2},{3}; {4},{5}); ",
                                description.DeviceName,
                                description.IsAttachedToDesktop,
                                desktopBounds.Left,
                                desktopBounds.Top,
                                desktopBounds.Right,
                                desktopBounds.Bottom);

                sb.AppendLine();
                sb.Append("\tCurrent display mode: ");

                // if there is a display mode - write its information and add it in the list of available modes
                var currentDisplayMode = output.CurrentDisplayMode;
                if (currentDisplayMode != null)
                {
                    if (availableModes.Count < 10)
                    {
                        var modeInfo = new ModeInfo(adapter, i, true, desktopBounds.Width, desktopBounds.Height);
                        availableModes.Add(modeInfo);
                    }

                    sb.AppendFormat("{0}x{1}@{2}, {3}",
                                    currentDisplayMode.Width,
                                    currentDisplayMode.Height,
                                    currentDisplayMode.RefreshRate.Numerator / currentDisplayMode.RefreshRate.Denominator,
                                    currentDisplayMode.Format);
                }
                else
                {
                    sb.Append("null");
                }

                sb.AppendLine();
            }
        }

        /// <summary>
        /// Converts the pointer size to MiB
        /// </summary>
        /// <param name="size">Size in bytes</param>
        /// <returns>Size in MiBs</returns>
        private PointerSize ToMB(PointerSize size)
        {
            return size / (1024 * 1024);
        }
    }
}
