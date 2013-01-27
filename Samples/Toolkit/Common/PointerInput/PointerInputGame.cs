// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using SharpDX;
using SharpDX.Toolkit;

namespace PointerInput
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    /// <summary>
    /// Simple PointerInput application using SharpDX.Toolkit.
    /// The purpose of this application is to use PointerInput.
    /// </summary>
    /// <remarks>
    /// This application will show a number of most recent pointer events.
    /// Modify the method <see cref="PointerEventDescrption.BuildStringRepresentation"/> to show the data you need.
    /// </remarks>
    public class PointerInputGame : Game
    {
        private class PointerEventDescrption
        {
            private readonly int index;
            private readonly PointerPoint point;

            private readonly string description;

            private string cache;

            public PointerEventDescrption(int index, PointerPoint point, string description)
            {
                this.index = index;
                this.point = point;
                this.description = description;
            }

            public PointerPoint Point { get { return point; } }

            public override string ToString()
            {
                if (string.IsNullOrWhiteSpace(cache))
                    BuildStringRepresentation();
                return cache;
            }

            /// <summary>
            /// Modify this method at your taste to display needed information
            /// </summary>
            private void BuildStringRepresentation()
            {
                var sb = new StringBuilder();

                // append general point information
                sb.AppendFormat("{0} - {1}: ", index, description);
                sb.AppendFormat("Dev:{0}; ID:{1}; Pos:{2}; Kind:{3}; ", point.DeviceType, point.PointerId, point.Position, point.PointerUpdateKind);

                // append device-specific information
                switch (point.DeviceType)
                {
                    case PointerDeviceType.Touch:
                        AppendTouchProperties(sb, point);
                        break;
                    case PointerDeviceType.Pen:
                        AppendPenProperties(sb, point);
                        break;
                    case PointerDeviceType.Mouse:
                        AppendMouseProperties(sb, point);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                cache = sb.ToString();
            }

            private void AppendMouseProperties(StringBuilder sb, PointerPoint p)
            {
                sb.AppendFormat("L:{0}; R:{1}; M:{2}; d:{3}", p.IsLeftButtonPressed, p.IsRightButtonPressed, p.IsMiddleButtonPressed, p.MouseWheelDelta);
            }

            private void AppendPenProperties(StringBuilder sb, PointerPoint p)
            {
                sb.AppendFormat("Er:{0}; Rng:{1}; Inv:{2}; Or:{3}; Tw:{4}; Tx:{5}; Ty:{6}", p.IsEraser, p.IsInRange, p.IsInverted, p.Orientation, p.Twist, p.XTilt, p.YTilt);
            }

            private void AppendTouchProperties(StringBuilder sb, PointerPoint p)
            {
                sb.AppendFormat("L:{0}; C:{1}; T:{2}; R:{3}", p.IsLeftButtonPressed, p.IsCanceled, p.TouchConfidence, p.IsInRange);
            }
        }

        private readonly GraphicsDeviceManager graphicsDeviceManager;
        private readonly IPointerService pointerService;
        private SpriteBatch spriteBatch;
        private SpriteFont arial16BMFont;

        private const int maxEvents = 30;

        private int eventIndex;
        private readonly Queue<PointerEventDescrption> recentEvents = new Queue<PointerEventDescrption>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerInputGame" /> class.
        /// </summary>
        public PointerInputGame()
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Create the pointer manager
            pointerService = new PointerManager(this);

            // Force no vsync and use real timestep to print actual FPS
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            pointerService.PointerCaptureLost += p => AddEvent("Capture", p);
            pointerService.PointerEntered += p => AddEvent("Entered", p);
            pointerService.PointerExited += p => AddEvent("Exited", p);
            pointerService.PointerMoved += p => AddEvent("Moved", p);
            pointerService.PointerPressed += p => AddEvent("Pressed", p);
            pointerService.PointerReleased += p => AddEvent("Released", p);
            pointerService.PointerWheelChanged += p => AddEvent("Wheel", p);
        }

        private void AddEvent(string eventDescription, PointerPoint point)
        {
            // uncomment and edit these lines to filter-out the unneeded events
            //if (recentEvents.Count > 0)
            //{
            //    var e = recentEvents.Last();
            //    var skipEvent = e.Point.DeviceType == point.DeviceType
            //                    && e.Point.PointerId == point.PointerId
            //                    && e.Point.Properties.PointerUpdateKind == point.Properties.PointerUpdateKind;

            //    if (skipEvent) return;
            //}


            if (recentEvents.Count == maxEvents)
                recentEvents.Dequeue();

            recentEvents.Enqueue(new PointerEventDescrption(eventIndex++, point, eventDescription));
        }

        protected override void LoadContent()
        {
            Window.AllowUserResizing = true;

            // set the resolution to current window size:
            graphicsDeviceManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphicsDeviceManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphicsDeviceManager.ApplyChanges();

            // SpriteFont supports the following font file format:
            // - DirectX Toolkit MakeSpriteFont or SharpDX Toolkit tkfont
            // - BMFont from Angelcode http://www.angelcode.com/products/bmfont/
            arial16BMFont = Content.Load<SpriteFont>("Arial16.tkfnt");

            // Instantiate a SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            spriteBatch.Dispose();

            base.UnloadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "MouseInput demo";
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clears the screen with the Color.CornflowerBlue
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // print the current mouse state
            var sb = new StringBuilder();
            foreach (var e in recentEvents)
                sb.AppendLine(e.ToString());

            // Render the text
            spriteBatch.Begin();
            spriteBatch.DrawString(arial16BMFont, sb.ToString(), new Vector2(8, 32), Color.White);
            spriteBatch.End();

            // Handle base.Draw
            base.Draw(gameTime);
        }
    }
}
