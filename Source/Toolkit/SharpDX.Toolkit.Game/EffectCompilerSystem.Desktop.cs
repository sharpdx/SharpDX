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

using SharpDX.Toolkit.Graphics;

using System.Collections.Generic;

namespace SharpDX.Toolkit
{
    /// <summary>The effect compiler system class.</summary>
    public partial class EffectCompilerSystem
    {
        #region Fields

        private readonly Dictionary<Effect, List<FileSystemWatcher>> effectsToWatcher = new Dictionary<Effect, List<FileSystemWatcher>>();

        private readonly Dictionary<FileSystemWatcher, List<Effect>> watcherToEffects = new Dictionary<FileSystemWatcher, List<Effect>>();

        #endregion

        #region Methods

        /// <summary>Tracks the effect.</summary>
        /// <param name="effect">The effect.</param>
        protected virtual void TrackEffect(Effect effect)
        {
            var fileList = compiler.LoadDependency(effect.RawEffectData.Arguments.DependencyFilePath);

            List<FileSystemWatcher> watchers;
            if (!effectsToWatcher.TryGetValue(effect, out watchers))
            {
                watchers = new List<FileSystemWatcher>();
                effectsToWatcher.Add(effect, watchers);
            }

            var dirList = GetDirectoryList(fileList);
            var currentWatchers = new List<FileSystemWatcher>();
            foreach (var dirPath in dirList)
            {
                // Try to find an existing watcher
                var watcher = FindWatcher(watchers, dirPath) ?? FindWatcher(watcherToEffects.Keys, dirPath);

                if (watcher == null)
                {
                    watcher = new FileSystemWatcher(dirPath);
                    var effectPerWatch = new List<Effect> { effect };
                    watcherToEffects.Add(watcher, effectPerWatch);
                    watcher.Changed += watcher_Changed;
                    watcher.EnableRaisingEvents = true;
                }
                else
                {
                    var effectPerWatch = watcherToEffects[watcher];
                    if (!effectPerWatch.Contains(effect))
                    {
                        effectPerWatch.Add(effect);
                    }
                }

                if (!watchers.Contains(watcher))
                {
                    watchers.Add(watcher);
                }

                if (!currentWatchers.Contains(watcher))
                {
                    currentWatchers.Add(watcher);
                }
            }

            // Release any previous watcher allocated that are no longer needed.
            foreach (var watcher in watchers)
            {
                if (!currentWatchers.Contains(watcher))
                {
                    RemoveEffectFromWatcher(effect, watcher);
                }
            }

            // Update the list of watchers for the specified effect
            watchers.Clear();
            watchers.AddRange(currentWatchers);
        }

        /// <summary>Un-tracks the effect.</summary>
        /// <param name="effect">The effect.</param>
        protected virtual void UnTrackEffect(Effect effect)
        {
            var watchersPerEffect = effectsToWatcher[effect];

            foreach (var watcher in watchersPerEffect)
            {
                RemoveEffectFromWatcher(effect, watcher);
            }

            effectsToWatcher.Remove(effect);
        }

        private FileSystemWatcher FindWatcher(IEnumerable<FileSystemWatcher> watchers, string path)
        {
            foreach (var watcher in watchers)
            {
                if (string.Compare(watcher.Path, path, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return watcher;
                }
            }

            return null;
        }

        private void RemoveEffectFromWatcher(Effect effect, FileSystemWatcher watcher)
        {
            var listOfEffect = watcherToEffects[watcher];
            listOfEffect.Remove(effect);

            if (listOfEffect.Count == 0)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= watcher_Changed;
                watcher.Dispose();
                watcherToEffects.Remove(watcher);
            }
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                bool notityRecompiler = false;

                var effectListToCheck = watcherToEffects[(FileSystemWatcher)sender];
                lock (effectListToCheck)
                {
                    foreach (var effect in effectListToCheck)
                    {
                        var args = effect.RawEffectData.Arguments;
                        bool checkForCompiler = false;
                        lock (effectsToCheck)
                        {
                            if (!effectsToCheck.Contains(effect))
                            {
                                checkForCompiler = true;
                            }
                        }

                        if (checkForCompiler)
                        {
                            if (compiler.CheckForChanges(args.DependencyFilePath))
                            {
                                lock (effectsToCheck)
                                {
                                    if (!effectsToCheck.Contains(effect))
                                    {
                                        effectsToCheck.Add(effect);
                                        notityRecompiler = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (notityRecompiler)
                {
                    resetEvent.Set();
                }
            }
        }

        #endregion
    }
}
#endif