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

using System;
using System.Collections.Generic;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    public class Game : Component
    {
        private readonly List<IDrawable> drawableGameSystems;
        private readonly List<IGameSystem> pendingGameSystems;
        private readonly List<IUpdateable> updateableGameSystems;
        private readonly GamePlatform gamePlatform;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game" /> class.
        /// </summary>
        public Game()
        {
            // Internals
            drawableGameSystems = new List<IDrawable>();
            pendingGameSystems = new List<IGameSystem>();
            updateableGameSystems = new List<IUpdateable>();

            // Externals
            Services = new GameServiceRegistry();
            GameSystems = new GameSystemCollection();
            Content = new ContentManager(Services);

            // Add ContentManager as a service.
            Services.AddService(typeof (IContentManager), Content);

            // Register events on GameSystems.
            GameSystems.ItemAdded += GameSystems_ItemAdded;
            GameSystems.ItemRemoved += GameSystems_ItemRemoved;

            // Create the game platform for the current platform
            gamePlatform = GamePlatform.Create();

            IsActive = true;
        }

        /// <summary>
        /// Gets the game components registered by this game.
        /// </summary>
        /// <value>The game components.</value>
        public GameSystemCollection GameSystems { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ContentManager"/>.
        /// </summary>
        /// <value>The content manager.</value>
        public ContentManager Content { get; set; }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Gets or sets the inactive sleep time.
        /// </summary>
        /// <value>The inactive sleep time.</value>
        public TimeSpan InactiveSleepTime { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed time step.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed time step; otherwise, <c>false</c>.</value>
        public bool IsFixedTimeStep { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse should be visible.
        /// </summary>
        /// <value><c>true</c> if the mouse should be visible; otherwise, <c>false</c>.</value>
        public bool IsMouseVisible { get; set; }

        /// <summary>
        /// Gets the service container.
        /// </summary>
        /// <value>The service container.</value>
        public GameServiceRegistry Services { get; private set; }

        /// <summary>
        /// Gets or sets the target elapsed time.
        /// </summary>
        /// <value>The target elapsed time.</value>
        public TimeSpan TargetElapsedTime { get; set; }

        /// <summary>
        /// Gets the abstract window.
        /// </summary>
        /// <value>The window.</value>
        public GameWindow Window { get; private set; }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Occurs when [activated].
        /// </summary>
        public event EventHandler<EventArgs> Activated;

        /// <summary>
        /// Occurs when [deactivated].
        /// </summary>
        public event EventHandler<EventArgs> Deactivated;

        /// <summary>
        /// Occurs when [exiting].
        /// </summary>
        public event EventHandler<EventArgs> Exiting;

        /// <summary>
        /// Starts the drawing of a frame. This method is followed by calls to Draw and EndDraw.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        protected virtual bool BeginDraw()
        {
            return true;
        }

        /// <summary>Called after all components are initialized but before the first update in the game loop.</summary>
        protected virtual void BeginRun()
        {
        }

        /// <summary>  Reference page contains code sample.</summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected virtual void Draw(GameTime gameTime)
        {
        }

        /// <summary>Ends the drawing of a frame. This method is preceeded by calls to Draw and BeginDraw.</summary>
        protected virtual void EndDraw()
        {
        }

        /// <summary>Called after the game loop has stopped running before exiting.</summary>
        protected virtual void EndRun()
        {
        }

        /// <summary>Exits the game.</summary>
        public void Exit()
        {
        }

        /// <summary>Called after the Game and GraphicsDevice are created, but before LoadContent.  Reference page contains code sample.</summary>
        protected virtual void Initialize()
        {
        }

        /// <summary />
        protected virtual void LoadContent()
        {
        }

        /// <summary>Raises the Activated event. Override this method to add code to handle when the game gains focus.</summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Activated event.</param>
        protected virtual void OnActivated(object sender, EventArgs args)
        {
        }

        /// <summary>Raises the Deactivated event. Override this method to add code to handle when the game loses focus.</summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Deactivated event.</param>
        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
        }

        /// <summary>Raises an Exiting event. Override this method to add code to handle when the game is exiting.</summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Exiting event.</param>
        protected virtual void OnExiting(object sender, EventArgs args)
        {
        }

        private void Paint(object sender, EventArgs e)
        {
        }

        /// <summary>Resets the elapsed time counter.</summary>
        public void ResetElapsedTime()
        {
        }

        /// <summary>Call this method to initialize the game, begin running the game loop, and start processing events for the game.</summary>
        public void Run()
        {
        }

        /// <summary>Run the game through what would happen in a single tick of the game clock; this method is designed for debugging only.</summary>
        public void RunOneFrame()
        {
        }

        /// <summary>This is used to display an error message if there is no suitable graphics device or sound card.</summary>
        /// <param name="exception">The exception to display.</param>
        protected virtual bool ShowMissingRequirementMessage(Exception exception)
        {
            return true;
        }

        /// <summary>Prevents calls to Draw until the next Update.</summary>
        public void SuppressDraw()
        {
        }

        /// <summary>Updates the game's clock and calls Update and Draw.</summary>
        public void Tick()
        {
        }

        /// <summary>Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.</summary>
        protected virtual void UnloadContent()
        {
        }

        /// <summary> Reference page contains links to related conceptual articles.</summary>
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected virtual void Update(GameTime gameTime)
        {
        }

        private void OnActivated(EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null) handler(this, e);
        }

        private void OnDeactivated(EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null) handler(this, e);
        }

        private void OnExiting(EventArgs e)
        {
            EventHandler<EventArgs> handler = Exiting;
            if (handler != null) handler(this, e);
        }

        private void GameSystems_ItemAdded(object sender, Collections.ObservableCollectionEventArgs<IGameSystem> e)
        {
            var gameSystem = e.Item;

            // If the game is already running, then we can initialize the game system now
            if (IsRunning)
            {
                gameSystem.Initialize();
            }
            else
            {
                // else we need to initialize it later
                pendingGameSystems.Add(gameSystem);
            }

            // Add an updateable system to the separate list
            var updateableGameSystem = gameSystem as IUpdateable;
            if (updateableGameSystem != null && AddGameSystem(updateableGameSystem, updateableGameSystems, UpdateableComparer.Default))
            {
                updateableGameSystem.UpdateOrderChanged += updateableGameSystem_UpdateOrderChanged;
            }

            // Add a drawable system to the separate list
            var drawableGameSystem = gameSystem as IDrawable;
            if (drawableGameSystem != null && AddGameSystem(drawableGameSystem, drawableGameSystems, DrawableComparer.Default))
            {
                drawableGameSystem.DrawOrderChanged += drawableGameSystem_DrawOrderChanged;
            }
        }

        private void GameSystems_ItemRemoved(object sender, Collections.ObservableCollectionEventArgs<IGameSystem> e)
        {
            var gameSystem = e.Item;

            if (!IsRunning)
            {
                pendingGameSystems.Remove(gameSystem);
            }

            var gameComponent = gameSystem as IUpdateable;
            if (gameComponent != null)
            {
                updateableGameSystems.Remove(gameComponent);
                gameComponent.UpdateOrderChanged -= updateableGameSystem_UpdateOrderChanged;
            }

            var item = gameSystem as IDrawable;
            if (item != null)
            {
                drawableGameSystems.Remove(item);
                item.DrawOrderChanged -= drawableGameSystem_DrawOrderChanged;
            }
        }

        private static bool AddGameSystem<T>(T gameSystem, List<T> gameSystems, IComparer<T> comparer, bool removePreviousSystem = false)
        {
            // If we are updating the order
            if (removePreviousSystem)
            {
                gameSystems.Remove(gameSystem);
            }

            // Find this gameSystem
            int index = gameSystems.BinarySearch(gameSystem, comparer);
            if (index < 0)
            {
                // If index is negative, that is the bitwise complement of the index of the next element that is larger than item 
                // or, if there is no larger element, the bitwise complement of Count.
                index = ~index;

                // Iterate until the order is different or we are at the end of the list
                while ((index < gameSystems.Count) && (comparer.Compare(gameSystems[index], gameSystem) == 0))
                {
                    index++;
                }

                gameSystems.Insert(index, gameSystem);

                // True, the system was inserted
                return true;
            }

            // False, it is already in the list
            return false;
        }

        private void updateableGameSystem_UpdateOrderChanged(object sender, EventArgs e)
        {
            AddGameSystem((IUpdateable)sender, updateableGameSystems, UpdateableComparer.Default, true);
        }

        private void drawableGameSystem_DrawOrderChanged(object sender, EventArgs e)
        {
            AddGameSystem((IDrawable)sender, drawableGameSystems, DrawableComparer.Default, true);
        }

        #region Nested type: DrawableComparer

        internal class DrawableComparer : IComparer<IDrawable>
        {
            public static readonly DrawableComparer Default = new DrawableComparer();

            #region IComparer<IDrawable> Members

            public int Compare(IDrawable left, IDrawable right)
            {
                if (Equals(left, right))
                    return 0;
                if (left == null)
                    return 1;
                if (right == null)
                    return -1;
                return (left.DrawOrder < right.DrawOrder) ? -1 : 1;
            }

            #endregion
        }

        #endregion

        #region Nested type: UpdateableComparer

        internal class UpdateableComparer : IComparer<IUpdateable>
        {
            public static readonly UpdateableComparer Default = new UpdateableComparer();

            #region IComparer<IUpdateable> Members

            public int Compare(IUpdateable left, IUpdateable right)
            {
                if (Equals(left, right))
                    return 0;
                if (left == null)
                    return 1;
                if (right == null)
                    return -1;
                return (left.UpdateOrder < right.UpdateOrder) ? -1 : 1;
            }

            #endregion
        }

        #endregion
    }
}