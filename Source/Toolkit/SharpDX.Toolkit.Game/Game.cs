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

using SharpDX.Collections;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{

    /// <summary>
    /// The game.
    /// </summary>
    public class Game : Component
    {
        #region Fields

        private readonly List<IDrawable> currentlyDrawingGameSystems;
        private readonly List<IUpdateable> currentlyUpdatingGameSystems;
        private readonly List<IContentable> currentlyContentGameSystems;
        private readonly List<IDrawable> drawableGameSystems;
        private readonly GameTime gameTime;
        private readonly List<IGameSystem> pendingGameSystems;
        private readonly List<IUpdateable> updateableGameSystems;
        private readonly List<IContentable> contentableGameSystems;
        private readonly int[] lastUpdateCount;
        private readonly float updateCountAverageSlowLimit;
        private GamePlatform gamePlatform;
        private IGraphicsDeviceService graphicsDeviceService;
        private IGraphicsDeviceManager graphicsDeviceManager;

        private readonly DisposeCollector contentCollector;
        private bool isEndRunRequired;
        private bool isExiting;
        private bool isFirstUpdateDone;
        private bool suppressDraw;

        private TimeSpan totalGameTime;
        private TimeSpan inactiveSleepTime;
        private TimeSpan maximumElapsedTime;
        private TimeSpan accumulatedElapsedGameTime;
        private TimeSpan lastFrameElapsedGameTime;
        private int nextLastUpdateCountIndex;
        private bool drawRunningSlowly;
        private bool forceElapsedTimeToZero;
        private bool contentLoaded = false;

        private readonly TimerTick timer;

        private bool isMouseVisible;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Game" /> class.
        /// </summary>
        public Game()
        {
            // Internals
            drawableGameSystems = new List<IDrawable>();
            currentlyContentGameSystems  = new List<IContentable>();
            currentlyDrawingGameSystems = new List<IDrawable>();
            pendingGameSystems = new List<IGameSystem>();
            updateableGameSystems = new List<IUpdateable>();
            currentlyUpdatingGameSystems = new List<IUpdateable>();
            contentableGameSystems = new List<IContentable>();
            contentCollector = new DisposeCollector();
            gameTime = new GameTime();
            totalGameTime = new TimeSpan();
            timer = new TimerTick();
            IsFixedTimeStep = false;
            maximumElapsedTime = TimeSpan.FromMilliseconds(500.0);
            TargetElapsedTime = TimeSpan.FromTicks(10000000 / 60); // target elapsed time is by default 60Hz
            lastUpdateCount = new int[4];
            nextLastUpdateCountIndex = 0;

            // Calculate the updateCountAverageSlowLimit (assuming moving average is >=3 )
            // Example for a moving average of 4:
            // updateCountAverageSlowLimit = (2 * 2 + (4 - 2)) / 4 = 1.5f
            const int BadUpdateCountTime = 2; // number of bad frame (a bad frame is a frame that has at least 2 updates)
            var maxLastCount = 2 * Math.Min(BadUpdateCountTime, lastUpdateCount.Length);
            updateCountAverageSlowLimit = (float)(maxLastCount + (lastUpdateCount.Length - maxLastCount)) / lastUpdateCount.Length;

            // Externals
            Services = new GameServiceRegistry();
            Content = new ContentManager(Services);
            LaunchParameters = new LaunchParameters();
            GameSystems = new GameSystemCollection();

            // Create Platform
            gamePlatform = GamePlatform.Create(this);
            gamePlatform.Activated += gamePlatform_Activated;
            gamePlatform.Deactivated += gamePlatform_Deactivated;
            gamePlatform.Exiting += gamePlatform_Exiting;
            gamePlatform.WindowCreated += GamePlatformOnWindowCreated;

            // By default, add a FileResolver for the ContentManager
            Content.Resolvers.Add(new FileSystemContentResolver(gamePlatform.DefaultAppDirectory));

            // Setup registry
            Services.AddService(typeof(IServiceRegistry), Services);
            Services.AddService(typeof(IContentManager), Content);
            Services.AddService(typeof(IGamePlatform), gamePlatform);

            // Register events on GameSystems.
            GameSystems.ItemAdded += GameSystems_ItemAdded;
            GameSystems.ItemRemoved += GameSystems_ItemRemoved;

            IsActive = true;
        }

        #endregion

        #region Public Events

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
        /// Occurs when [window created].
        /// </summary>
        public event EventHandler<EventArgs> WindowCreated;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="ContentManager"/>.
        /// </summary>
        /// <value>The content manager.</value>
        public ContentManager Content { get; set; }

        /// <summary>
        /// Gets the game components registered by this game.
        /// </summary>
        /// <value>The game components.</value>
        public GameSystemCollection GameSystems { get; private set; }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (graphicsDeviceService == null)
                {
                    throw new InvalidOperationException("GraphicsDeviceService is not yet initialized");
                }

                return graphicsDeviceService.GraphicsDevice;
            }
        }

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
        public bool IsMouseVisible
        {
            get
            {
                return isMouseVisible;
            }

            set
            {
                isMouseVisible = value;
                if (Window != null)
                {
                    Window.IsMouseVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets the launch parameters.
        /// </summary>
        /// <value>The launch parameters.</value>
        public LaunchParameters LaunchParameters { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is running.
        /// </summary>
        public bool IsRunning { get; private set; }

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
        public GameWindow Window
        {
            get
            {
                if (gamePlatform != null)
                {
                    return gamePlatform.MainWindow;
                }
                return null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Exits the game.
        /// </summary>
        public void Exit()
        {
            isExiting = true;
            gamePlatform.Exit();
            if (IsRunning && isEndRunRequired)
            {
                EndRun();
                IsRunning = false;
            }
        }

        /// <summary>
        /// Resets the elapsed time counter.
        /// </summary>
        public void ResetElapsedTime()
        {
            forceElapsedTimeToZero = true;
            drawRunningSlowly = false;
            Array.Clear(lastUpdateCount, 0, lastUpdateCount.Length);
            nextLastUpdateCountIndex = 0;
        }

        internal void InitializeDevice()
        {
            // Make sure that the device is already created
            graphicsDeviceManager.CreateDevice();
        }

        internal void InitializeBeforeRun()
        {
            // Gets the graphics device service
            graphicsDeviceService = Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            if (graphicsDeviceService == null)
            {
                throw new InvalidOperationException("No GraphicsDeviceService found");
            }

            // Checks the graphics device
            if (graphicsDeviceService.GraphicsDevice == null)
            {
                throw new InvalidOperationException("No GraphicsDevice found");
            }

            // Initialize this instance and all game systems
            Initialize();

            // If there were any new game systems added in initialize, we should remove them here
            InitializePendingGameSystems();

            // Load the content of the game
            LoadContent();

            // Same here, make sure that pending game systems setup in LoadContent are initialized
            InitializePendingGameSystems();

            IsRunning = true;

            BeginRun();

            timer.Reset();
            gameTime.Update(totalGameTime, TimeSpan.Zero, false);
            gameTime.FrameCount = 0;

            // Run the first time an update
            Update(gameTime);

            isFirstUpdateDone = true;
        }

        /// <summary>
        /// Call this method to initialize the game, begin running the game loop, and start processing events for the game.
        /// </summary>
        /// <param name="gameContext">The window Context for this game.</param>
        /// <exception cref="System.InvalidOperationException">Cannot run this instance while it is already running</exception>
        public void Run(GameContext gameContext = null)
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Cannot run this instance while it is already running");
            }

            // Gets the graphics device manager
            graphicsDeviceManager = Services.GetService(typeof(IGraphicsDeviceManager)) as IGraphicsDeviceManager;
            if (graphicsDeviceManager == null)
            {
                throw new InvalidOperationException("No GraphicsDeviceManager found");
            }

            if (graphicsDeviceManager == null)
            {
                throw new InvalidOperationException("No GraphicsDeviceManager found");
            }

            // Gets the GameWindow Context
            gameContext = gameContext ?? new GameContext();

            try
            {
                gamePlatform.Run(gameContext);

                if (gamePlatform.IsBlockingRun)
                {
                    // If the previous call was blocking, then we can call Endrun
                    EndRun();
                }
                else
                {
                    // EndRun will be executed on Game.Exit
                    isEndRunRequired = true;
                }
            }
            finally
            {
                if (!isEndRunRequired)
                {
                    IsRunning = false;
                }
            }
        }

        /// <summary>
        /// Call this method to switch game rendering to a different control.
        /// </summary>
        /// <param name="context">The new game context.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="context"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Is thrown when game is not running (<see cref="IsRunning"/> is false).</exception>
        public void Switch(GameContext context)
        {
            if(context == null) throw new ArgumentNullException("context");

            if (!IsRunning) throw new InvalidOperationException("Cannot switch context while not running, call Run(GameContext) instead.");

            gamePlatform.Switch(context);
        }

        /// <summary>
        /// Prevents calls to Draw until the next Update.
        /// </summary>
        public void SuppressDraw()
        {
            suppressDraw = true;
        }

        /// <summary>
        /// Updates the game's clock and calls Update and Draw.
        /// </summary>
        public void Tick()
        {
            // If this instance is existing, then don't make any further update/draw
            if (isExiting)
            {
                return;
            }

            // If this instance is not active, sleep for an inactive sleep time
            if (!IsActive)
            {
                Utilities.Sleep(inactiveSleepTime);
            }

            // Update the timer
            timer.Tick();

            var elapsedAdjustedTime = timer.ElapsedAdjustedTime;

            if (forceElapsedTimeToZero)
            {
                elapsedAdjustedTime = TimeSpan.Zero;
                forceElapsedTimeToZero = false;
            }

            if (elapsedAdjustedTime > maximumElapsedTime)
            {
                elapsedAdjustedTime = maximumElapsedTime;
            }

            bool suppressNextDraw = true;
            int updateCount = 1;
            var singleFrameElapsedTime = elapsedAdjustedTime;

            if (IsFixedTimeStep)
            {
                // If the rounded TargetElapsedTime is equivalent to current ElapsedAdjustedTime
                // then make ElapsedAdjustedTime = TargetElapsedTime. We take the same internal rules as XNA 
                if (Math.Abs(elapsedAdjustedTime.Ticks - TargetElapsedTime.Ticks) < (TargetElapsedTime.Ticks >> 6))
                {
                    elapsedAdjustedTime = TargetElapsedTime;
                }

                // Update the accumulated time
                accumulatedElapsedGameTime += elapsedAdjustedTime;

                // Calculate the number of update to issue
                updateCount = (int)(accumulatedElapsedGameTime.Ticks / TargetElapsedTime.Ticks);

                // If there is no need for update, then exit
                if (updateCount == 0)
                {
                    return;
                }

                // Calculate a moving average on updateCount
                lastUpdateCount[nextLastUpdateCountIndex] = updateCount;
                float updateCountMean = 0;
                for (int i = 0; i < lastUpdateCount.Length; i++)
                {
                    updateCountMean += lastUpdateCount[i];
                }

                updateCountMean /= lastUpdateCount.Length;
                nextLastUpdateCountIndex = (nextLastUpdateCountIndex + 1) % lastUpdateCount.Length;

                // Test when we are running slowly
                drawRunningSlowly = updateCountMean > updateCountAverageSlowLimit;

                // We are going to call Update updateCount times, so we can subtract this from accumulated elapsed game time
                accumulatedElapsedGameTime = new TimeSpan(accumulatedElapsedGameTime.Ticks - (updateCount * TargetElapsedTime.Ticks));
                singleFrameElapsedTime = TargetElapsedTime;
            }
            else
            {
                Array.Clear(lastUpdateCount, 0, lastUpdateCount.Length);
                nextLastUpdateCountIndex = 0;
                drawRunningSlowly = false;
            }

            // Reset the time of the next frame
            for (lastFrameElapsedGameTime = TimeSpan.Zero; updateCount > 0 && !isExiting; updateCount--)
            {
                gameTime.Update(totalGameTime, singleFrameElapsedTime, drawRunningSlowly);
                try
                {
                    Update(gameTime);

                    // If there is no exception, then we can draw the frame
                    suppressNextDraw &= suppressDraw;
                    suppressDraw = false;
                }
                finally
                {
                    lastFrameElapsedGameTime += singleFrameElapsedTime;
                    totalGameTime += singleFrameElapsedTime;
                }
            }

            if (!suppressNextDraw)
            {
                DrawFrame();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the drawing of a frame. This method is followed by calls to Draw and EndDraw.
        /// </summary>
        /// <returns><c>true</c> to continue drawing, false to not call <see cref="Draw"/> and <see cref="EndDraw"/></returns>
        protected virtual bool BeginDraw()
        {
            if ((graphicsDeviceManager != null) && !graphicsDeviceManager.BeginDraw())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called after all components are initialized but before the first update in the game loop.
        /// </summary>
        protected virtual void BeginRun()
        {
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                lock (this)
                {
                    var array = new IGameSystem[GameSystems.Count];
                    this.GameSystems.CopyTo(array, 0);
                    for (int i = 0; i < array.Length; i++)
                    {
                        var disposable = array[i] as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }

                    var disposableGraphicsManager = graphicsDeviceManager as IDisposable;
                    if (disposableGraphicsManager != null)
                    {
                        disposableGraphicsManager.Dispose();
                    }

                    DisposeGraphicsDeviceEvents();

                    if (gamePlatform != null)
                    {
                        gamePlatform.Dispose();
                    }
                }
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Reference page contains code sample.
        /// </summary>
        /// <param name="gameTime">
        /// Time passed since the last call to Draw.
        /// </param>
        protected virtual void Draw(GameTime gameTime)
        {
            // Just lock current drawable game systems to grab them in a temporary list.
            lock (drawableGameSystems)
            {
                for (int i = 0; i < drawableGameSystems.Count; i++)
                {
                    currentlyDrawingGameSystems.Add(drawableGameSystems[i]);
                }
            }

            for (int i = 0; i < currentlyDrawingGameSystems.Count; i++)
            {
                var drawable = currentlyDrawingGameSystems[i];
                if (drawable.Visible)
                {
                    if (drawable.BeginDraw())
                    {
                        drawable.Draw(gameTime);
                        drawable.EndDraw();
                    }
                }
            }

            currentlyDrawingGameSystems.Clear();
        }

        /// <summary>Ends the drawing of a frame. This method is preceded by calls to Draw and BeginDraw.</summary>
        protected virtual void EndDraw()
        {
            if (graphicsDeviceManager != null)
            {
                graphicsDeviceManager.EndDraw();
            }
        }

        /// <summary>Called after the game loop has stopped running before exiting.</summary>
        protected virtual void EndRun()
        {
        }

        /// <summary>Called after the Game and GraphicsDevice are created, but before LoadContent.  Reference page contains code sample.</summary>
        protected virtual void Initialize()
        {
            // Setup the graphics device if it was not already setup.
            SetupGraphicsDeviceEvents();
            InitializePendingGameSystems();
        }


        private void InitializePendingGameSystems(bool loadContent = false)
        {
            // Add all game systems that were added to this game instance before the game started.
            while (pendingGameSystems.Count != 0)
            {
                pendingGameSystems[0].Initialize();

                if (loadContent && pendingGameSystems[0] is IContentable)
                {
                    ((IContentable)pendingGameSystems[0]).LoadContent();
                }

                pendingGameSystems.RemoveAt(0);
            }
        }

        /// <summary>
        /// Adds an object to be disposed automatically when <see cref="UnloadContent"/> is called. See remarks.
        /// </summary>
        /// <typeparam name="T">Type of the object to dispose</typeparam>
        /// <param name="disposable">The disposable object.</param>
        /// <returns>The disposable object.</returns>
        /// <remarks>
        /// Use this method for any content that is not loaded through the <see cref="ContentManager"/>.
        /// </remarks>
        protected T ToDisposeContent<T>(T disposable) where T : IDisposable
        {
            return contentCollector.Collect(disposable);
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected virtual void LoadContent()
        {
            LoadContentSystems();
            contentLoaded = true;
        }

        private void LoadContentSystems()
        {
            lock (contentableGameSystems)
            {
                foreach (var contentable in contentableGameSystems)
                {
                    currentlyContentGameSystems.Add(contentable);
                }
            }

            foreach (var contentable in currentlyContentGameSystems)
            {
                contentable.LoadContent();
            }

            currentlyContentGameSystems.Clear();
        }

        /// <summary>
        /// Raises the Activated event. Override this method to add code to handle when the game gains focus.
        /// </summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Activated event.</param>
        protected virtual void OnActivated(object sender, EventArgs args)
        {
            var handler = Activated;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises the Deactivated event. Override this method to add code to handle when the game loses focus.
        /// </summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Deactivated event.</param>
        protected virtual void OnDeactivated(object sender, EventArgs args)
        {
            var handler = Deactivated;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises an Exiting event. Override this method to add code to handle when the game is exiting.
        /// </summary>
        /// <param name="sender">The Game.</param>
        /// <param name="args">Arguments for the Exiting event.</param>
        protected virtual void OnExiting(object sender, EventArgs args)
        {
            var handler = Exiting;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected virtual void OnWindowCreated()
        {
            EventHandler<EventArgs> handler = WindowCreated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void GamePlatformOnWindowCreated(object sender, EventArgs eventArgs)
        {
            OnWindowCreated();
        }


        /// <summary>
        /// This is used to display an error message if there is no suitable graphics device or sound card.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        /// <returns>The <see cref="bool" />.</returns>
        protected virtual bool ShowMissingRequirementMessage(Exception exception)
        {
            return true;
        }

        /// <summary>
        /// Called when graphics resources need to be unloaded. Override this method to unload any game-specific graphics resources.
        /// </summary>
        protected virtual void UnloadContent()
        {
            // Dispose all objects allocated from content
            contentCollector.DisposeAndClear();

            lock (contentableGameSystems)
            {
                foreach (var contentable in contentableGameSystems)
                {
                    currentlyContentGameSystems.Add(contentable);
                }
            }

            foreach (var contentable in currentlyContentGameSystems)
            {
                contentable.UnloadContent();
            }

            currentlyContentGameSystems.Clear();
        }

        /// <summary>
        /// Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">
        /// Time passed since the last call to Update.
        /// </param>
        protected virtual void Update(GameTime gameTime)
        {
            lock (updateableGameSystems)
            {
                foreach (var updateable in updateableGameSystems)
                {
                    currentlyUpdatingGameSystems.Add(updateable);
                }
            }

            foreach (var updateable in currentlyUpdatingGameSystems)
            {
                if (updateable.Enabled)
                {
                    updateable.Update(gameTime);
                }
            }

            currentlyUpdatingGameSystems.Clear();
            isFirstUpdateDone = true;
        }

        private void gamePlatform_Activated(object sender, EventArgs e)
        {
            if (!IsActive)
            {
                IsActive = true;
                OnActivated(this, EventArgs.Empty);
            }
        }

        private void gamePlatform_Deactivated(object sender, EventArgs e)
        {
            if (IsActive)
            {
                IsActive = false;
                OnDeactivated(this, EventArgs.Empty);
            }
        }

        private void gamePlatform_Exiting(object sender, EventArgs e)
        {
            OnExiting(this, EventArgs.Empty);
        }

        private static bool AddGameSystem<T>(T gameSystem, List<T> gameSystems, IComparer<T> comparer, Comparison<T> orderComparer, bool removePreviousSystem = false)
        {
            lock (gameSystems)
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
                    while ((index < gameSystems.Count) && (orderComparer(gameSystems[index], gameSystem) == 0))
                    {
                        index++;
                    }

                    gameSystems.Insert(index, gameSystem);

                    // True, the system was inserted
                    return true;
                }
            }

            // False, it is already in the list
            return false;
        }

        private void DrawFrame()
        {
            try
            {
                if (!isExiting && isFirstUpdateDone && !Window.IsMinimized && BeginDraw())
                {
                    gameTime.Update(totalGameTime, lastFrameElapsedGameTime, drawRunningSlowly);
                    gameTime.FrameCount++;

                    Draw(gameTime);

                    EndDraw();
                }
            }
            finally
            {
                lastFrameElapsedGameTime = TimeSpan.Zero;
            }
        }

        private void GameSystems_ItemAdded(object sender, ObservableCollectionEventArgs<IGameSystem> e)
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

            // Add a contentable system to a separate list
            var contentableSystem = gameSystem as IContentable;
            if (contentableSystem != null)
            {
                lock (contentableGameSystems)
                {
                    if (!contentableGameSystems.Contains(contentableSystem))
                    {
                        contentableGameSystems.Add(contentableSystem);
                    }
                }

                if (IsRunning)
                {
                    // Load the content of the system if the game is already running
                    contentableSystem.LoadContent();
                }
            }

            // Add an updateable system to the separate list
            var updateableGameSystem = gameSystem as IUpdateable;
            if (updateableGameSystem != null && AddGameSystem(updateableGameSystem, updateableGameSystems, UpdateableSearcher.Default, UpdateableComparison))
            {
                updateableGameSystem.UpdateOrderChanged += updateableGameSystem_UpdateOrderChanged;
            }

            // Add a drawable system to the separate list
            var drawableGameSystem = gameSystem as IDrawable;
            if (drawableGameSystem != null && AddGameSystem(drawableGameSystem, drawableGameSystems, DrawableSearcher.Default, DrawableComparison))
            {
                drawableGameSystem.DrawOrderChanged += drawableGameSystem_DrawOrderChanged;
            }

        }

        private static int UpdateableComparison(IUpdateable left, IUpdateable right)
        {
            return left.UpdateOrder.CompareTo(right.UpdateOrder);
        }

        private static int DrawableComparison(IDrawable left, IDrawable right)
        {
            return left.DrawOrder.CompareTo(right.DrawOrder);
        }

        private void GameSystems_ItemRemoved(object sender, ObservableCollectionEventArgs<IGameSystem> e)
        {
            var gameSystem = e.Item;

            if (!IsRunning)
            {
                pendingGameSystems.Remove(gameSystem);
            }

            var contentableSystem = gameSystem as IContentable;
            if (contentableSystem != null)
            {
                lock (contentableGameSystems)
                {
                    contentableGameSystems.Remove(contentableSystem);
                }

                if (IsRunning)
                {
                    contentableSystem.UnloadContent();
                }
            }

            var gameComponent = gameSystem as IUpdateable;
            if (gameComponent != null)
            {
                lock (updateableGameSystems)
                {
                    updateableGameSystems.Remove(gameComponent);
                }

                gameComponent.UpdateOrderChanged -= updateableGameSystem_UpdateOrderChanged;
            }

            var item = gameSystem as IDrawable;
            if (item != null)
            {
                lock (drawableGameSystems)
                {
                    drawableGameSystems.Remove(item);
                }

                item.DrawOrderChanged -= drawableGameSystem_DrawOrderChanged;
            }
        }

        private void SetupGraphicsDeviceEvents()
        {
            // Find the IGraphicsDeviceSerive.
            graphicsDeviceService = Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;

            // If there is no graphics device service, don't go further as the whole Game would not work
            if (graphicsDeviceService == null)
            {
                throw new InvalidOperationException("Unable to create find a IGraphicsDeviceService");
            }

            if (graphicsDeviceService.GraphicsDevice == null)
            {
                throw new InvalidOperationException("Unable to find a GraphicsDevice instance");
            }

            graphicsDeviceService.DeviceCreated += graphicsDeviceService_DeviceCreated;
            graphicsDeviceService.DeviceDisposing += graphicsDeviceService_DeviceDisposing;
        }

        private void DisposeGraphicsDeviceEvents()
        {
            if (graphicsDeviceService != null)
            {
                graphicsDeviceService.DeviceCreated -= graphicsDeviceService_DeviceCreated;
                graphicsDeviceService.DeviceDisposing -= graphicsDeviceService_DeviceDisposing;
            }
        }

        private void drawableGameSystem_DrawOrderChanged(object sender, EventArgs e)
        {
            AddGameSystem((IDrawable)sender, drawableGameSystems, DrawableSearcher.Default, DrawableComparison, true);
        }

        private void graphicsDeviceService_DeviceCreated(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void graphicsDeviceService_DeviceDisposing(object sender, EventArgs e)
        {
            Content.Unload();

            if (contentLoaded)
            {
                UnloadContent();
                contentLoaded = false;
            }
        }

        private void updateableGameSystem_UpdateOrderChanged(object sender, EventArgs e)
        {
            AddGameSystem((IUpdateable)sender, updateableGameSystems, UpdateableSearcher.Default, UpdateableComparison, true);
        }

        #endregion

        /// <summary>
        /// The comparer used to order <see cref="IDrawable"/> objects.
        /// </summary>
        internal struct DrawableSearcher : IComparer<IDrawable>
        {
            public static readonly DrawableSearcher Default = new DrawableSearcher();

            public int Compare(IDrawable left, IDrawable right)
            {
                if (Equals(left, right))
                {
                    return 0;
                }

                if (left == null)
                {
                    return 1;
                }

                if (right == null)
                {
                    return -1;
                }

                return (left.DrawOrder < right.DrawOrder) ? -1 : 1;
            }
        }

        /// <summary>
        /// The comparer used to order <see cref="IUpdateable"/> objects.
        /// </summary>
        internal struct UpdateableSearcher : IComparer<IUpdateable>
        {
            public static readonly UpdateableSearcher Default = new UpdateableSearcher();

            public int Compare(IUpdateable left, IUpdateable right)
            {
                if (Equals(left, right))
                {
                    return 0;
                }

                if (left == null)
                {
                    return 1;
                }

                if (right == null)
                {
                    return -1;
                }

                return (left.UpdateOrder < right.UpdateOrder) ? -1 : 1;
            }
        }
    }
}