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
using System.IO;
using System.Threading;

using SharpDX.Threading;
using SharpDX.Toolkit.Graphics;

using System.Collections.Generic;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// Allows to dynamically recompile Effects at runtime when original source code changed, without having to recompile the application. See remarks.
    /// </summary>
    /// <remarks>
    /// The effect must have been compiled with tkfxc and option /Re. This features is only available from Windows Desktop and doesn't work
    /// on other platforms.
    /// </remarks>
    public partial class EffectCompilerSystem : GameSystem
    {
        #region Fields

        private readonly IEffectCompiler compiler;

        private readonly List<Effect> effectsCompilable;

        private readonly List<Effect> effectsToAlwaysCheck;

        private readonly List<Effect> effectsToCheck;

        private readonly List<Effect> effectsToRecompile;

        private readonly List<EffectToRebind> effectsToReset;

        private readonly List<EffectToRebind> effectsToResetTemp;

        private readonly AutoResetEvent endThreadCompilerEvent;

        private readonly AutoResetEvent resetEvent;

        private bool isThreadRunning;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectCompilerSystem" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public EffectCompilerSystem(Game game)
            : base(game)
        {
            effectsCompilable = new List<Effect>();
            effectsToAlwaysCheck = new List<Effect>();
            effectsToCheck = new List<Effect>();
            effectsToRecompile = new List<Effect>();
            effectsToReset = new List<EffectToRebind>();
            effectsToResetTemp = new List<EffectToRebind>();
            resetEvent = new AutoResetEvent(false);
            endThreadCompilerEvent = new AutoResetEvent(false);
            Enabled = true;
#if !W8CORE
            compiler = new EffectCompiler();
#endif
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when a compilation ended for an effect.
        /// </summary>
        public event EventHandler<EffectCompilerEventArgs> CompilationEnded;

        /// <summary>
        /// Occurs when a compilation error occured for an effect.
        /// </summary>
        public event EventHandler<EffectCompilerEventArgs> CompilationError;

        /// <summary>
        /// Occurs when a compilation started for an effect.
        /// </summary>
        public event EventHandler<EffectCompilerEventArgs> CompilationStarted;

        #endregion

        #region Public Methods and Operators

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (compiler == null)
            {
                return;
            }

            // Compute the list of effects that have been recompiled and their bytecode needs to be updated
            effectsToResetTemp.Clear();
            lock (effectsToReset)
            {
                effectsToResetTemp.AddRange(effectsToReset);
                effectsToReset.Clear();
            }

            // Initialize all effects with their new bytecode
            foreach (var effectToRebind in effectsToResetTemp)
            {
                var effect = effectToRebind.Effect;
                var effectData = effectToRebind.EffectData;

                // First register the bytecode to the pool in order to reuse existing shaders
                effect.Pool.RegisterBytecode(effectData, true);

                // Then re-instantiate this effect.
                effect.InitializeFrom(effectData.Effects[0]);
            }

            base.Update(gameTime);
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposeManagedResources)
        {
            if (isThreadRunning)
            {
                isThreadRunning = false;
                endThreadCompilerEvent.WaitOne();
            }

            base.Dispose(disposeManagedResources);
        }

        protected override void LoadContent()
        {
            if (compiler == null)
            {
                return;
            }

            foreach (var effectPool in GraphicsDevice.EffectPools)
            {
                AddEffectPool(effectPool);
            }
            GraphicsDevice.EffectPools.ItemAdded += EffectPools_ItemAdded;
            GraphicsDevice.EffectPools.ItemRemoved += EffectPools_ItemRemoved;

            base.LoadContent();

            isThreadRunning = true;
            TaskUtil.Run(ThreadCompiler);
        }

        protected virtual void OnCompilationEnded(EffectCompilerEventArgs e)
        {
            EventHandler<EffectCompilerEventArgs> handler = CompilationEnded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCompilationError(EffectCompilerEventArgs e)
        {
            EventHandler<EffectCompilerEventArgs> handler = CompilationError;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCompilationStarted(EffectCompilerEventArgs e)
        {
            EventHandler<EffectCompilerEventArgs> handler = CompilationStarted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void UnloadContent()
        {
            if (compiler == null)
            {
                return;
            }

            isThreadRunning = false;

            GraphicsDevice.EffectPools.ItemAdded -= EffectPools_ItemAdded;
            GraphicsDevice.EffectPools.ItemRemoved -= EffectPools_ItemRemoved;

            foreach (var effectPool in GraphicsDevice.EffectPools)
            {
                RemoveEffectPool(effectPool);
            }

            base.UnloadContent();
        }

        private static List<string> GetDirectoryList(IEnumerable<string> filePathList)
        {
            var directories = new List<string>();
            foreach (var filePath in filePathList)
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (directoryPath != null)
                {
                    directoryPath = directoryPath.ToLower();
                    if (!directories.Contains(directoryPath))
                    {
                        directories.Add(directoryPath);
                    }
                }
            }
            return directories;
        }

        private void AddEffect(Effect effect)
        {
            if (effect.IsSupportingDynamicCompilation)
            {
                lock (effectsCompilable)
                {
                    effectsCompilable.Add(effect);
                    TrackEffect(effect);
                }
            }
        }

        private void AddEffectPool(EffectPool effectPool)
        {
            foreach (var registeredEffect in effectPool.RegisteredEffects)
            {
                AddEffect(registeredEffect);
            }

            effectPool.EffectAdded += effectPool_EffectAdded;
            effectPool.EffectRemoved += effectPool_EffectRemoved;
        }

        private void EffectPools_ItemAdded(object sender, Collections.ObservableCollectionEventArgs<EffectPool> e)
        {
            AddEffectPool(e.Item);
        }

        private void EffectPools_ItemRemoved(object sender, Collections.ObservableCollectionEventArgs<EffectPool> e)
        {
            RemoveEffectPool(e.Item);
        }

        private void RemoveEffect(Effect effect)
        {
            if (effect.IsSupportingDynamicCompilation)
            {
                lock (effectsCompilable)
                {
                    effectsCompilable.Remove(effect);
                    UnTrackEffect(effect);
                }
            }
        }

        private void RemoveEffectPool(EffectPool effectPool)
        {
            effectPool.EffectAdded -= effectPool_EffectAdded;
            effectPool.EffectRemoved -= effectPool_EffectRemoved;

            foreach (var registeredEffect in effectPool.RegisteredEffects)
            {
                RemoveEffect(registeredEffect);
            }
        }

        private void ThreadCompiler()
        {
            try
            {
                // Loop until the end of this system
                while (isThreadRunning)
                {
                    // On a system that supports FileSystemWatcher (Desktop), process request every 250ms
                    // otherwise, process files every 1000ms.
                    if (!resetEvent.WaitOne(250 * effectsToAlwaysCheck.Count == 0 ? 1 : 5))
                    {
                        if (effectsToAlwaysCheck.Count == 0)
                        {
                            continue;
                        }
                    }

                    // Clears the list of effects to recompile
                    effectsToRecompile.Clear();

                    // Copy the list of effcts to check to the list of effects to recompile
                    lock (effectsToCheck)
                    {
                        effectsToRecompile.AddRange(effectsToCheck);
                        effectsToCheck.Clear();
                    }

                    // Add effects to always check
                    effectsToRecompile.AddRange(effectsToAlwaysCheck);

                    // Try to recompile each effect.
                    foreach (var effect in effectsToRecompile)
                    {
                        var args = effect.RawEffectData.Arguments;

                        // Check that and effect really needs to be recompiled (check all include dependencies)
                        if (compiler.CheckForChanges(args.DependencyFilePath))
                        {
                            OnCompilationStarted(new EffectCompilerEventArgs(effect));

                            // Then recompile it with same flags, macros and include directories.
                            var compilerResult = compiler.CompileFromFile(args.FilePath, args.CompilerFlags, args.Macros, args.IncludeDirectoryList, true, args.DependencyFilePath);

                            // If there are errors notify them
                            if (compilerResult.HasErrors || compilerResult.EffectData.Effects.Count == 0)
                            {
                                OnCompilationError(new EffectCompilerEventArgs(effect, compilerResult.Logger.Messages));
                            }
                            else
                            {
                                OnCompilationEnded(new EffectCompilerEventArgs(effect, compilerResult.Logger.Messages));

                                lock (effectsToReset)
                                {
                                    effectsToReset.Add(new EffectToRebind(effect, compilerResult.EffectData));
                                }

                                // Effect has been updated. We need to update its dependency.
                                lock (effectsCompilable)
                                {
                                    TrackEffect(effect);
                                }

                            }
                        }
                    }
                }
            }
            finally
            {
                // Notify end of thread
                endThreadCompilerEvent.Set();
            }
        }

        private void effectPool_EffectAdded(object sender, Collections.ObservableCollectionEventArgs<Effect> e)
        {
            AddEffect(e.Item);
        }

        private void effectPool_EffectRemoved(object sender, Collections.ObservableCollectionEventArgs<Effect> e)
        {
            RemoveEffect(e.Item);
        }

        #endregion

        private struct EffectToRebind
        {
            #region Fields

            public readonly Effect Effect;

            public readonly EffectData EffectData;

            #endregion

            #region Constructors and Destructors

            public EffectToRebind(Effect effect, EffectData effectData)
            {
                Effect = effect;
                EffectData = effectData;
            }

            #endregion
        }
    }
}