// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;

namespace SharpDX.Animation
{
    public partial class Manager
    {
        private static readonly Guid ManagerGuid = new Guid("4C1FC63A-695C-47E8-A339-1A194BE3D0B8");
        private ManagerEventHandlerCallback statusEventHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Manager"/> class.
        /// </summary>
        public Manager()
        {
            Utilities.CreateComInstance(ManagerGuid, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(Manager)), this);
        }

        /// <summary>
        /// A delegate to receive status changed events from the manager.
        /// </summary>
        /// <param name="newStatus">The new status.</param>
        /// <param name="previousStatus">The previous status.</param>
        public delegate void StatusChangedDelegate(ManagerStatus newStatus, ManagerStatus previousStatus);

        /// <summary>
        /// A delegate used to resolve scheduling conflicts.
        /// </summary>
        /// <param name="scheduledStoryboard">The scheduled storyboard.</param>
        /// <param name="newStoryboard">The new storyboard.</param>
        /// <param name="priorityEffect">The priority effect.</param>
        /// <returns><c>true</c> if newStoryboard has priority. <c>false</c> if scheduledStoryboard has priority</returns>
        public delegate bool PriorityComparisonDelegate(Storyboard scheduledStoryboard, Storyboard newStoryboard, PriorityEffect priorityEffect);

        /// <summary>
        /// Occurs when [status changed].
        /// </summary>
        public event StatusChangedDelegate StatusChanged
        {
            add
            {
                // Setup the Manager Event Handler delegates
                if (statusEventHandler == null)
                {
                    statusEventHandler = new ManagerEventHandlerCallback();
                    SetManagerEventHandler(statusEventHandler);
                }
                statusEventHandler.Delegates += value;
            }

            remove
            {
                if (statusEventHandler == null) return;

                statusEventHandler.Delegates -= value;

                if (statusEventHandler.Delegates.GetInvocationList().Length == 0)
                {
                    SetManagerEventHandler(null);
                    statusEventHandler.Dispose();
                    statusEventHandler = null;
                }
            }
        }

        private PriorityComparisonCallback cancelPriorityComparisonCallback;

        /// <summary>
        /// Sets the cancel priority comparison.
        /// </summary>
        /// <value>
        /// The cancel priority comparison.
        /// </value>
        public PriorityComparisonDelegate CancelPriorityComparison
        {
            set
            {
                if (value != null)
                {
                    if (cancelPriorityComparisonCallback == null)
                    {
                        cancelPriorityComparisonCallback = new PriorityComparisonCallback { Delegate = value };
                        SetCancelPriorityComparison(cancelPriorityComparisonCallback);
                    }
                    cancelPriorityComparisonCallback.Delegate = value;
                }
                else if (cancelPriorityComparisonCallback != null)
                {
                    SetCancelPriorityComparison(null);
                    cancelPriorityComparisonCallback.Dispose();
                    cancelPriorityComparisonCallback = null;
                }
            }
        }

        private PriorityComparisonCallback trimPriorityComparisonCallback;

        /// <summary>
        /// Sets the trim priority comparison.
        /// </summary>
        /// <value>
        /// The trim priority comparison.
        /// </value>
        public PriorityComparisonDelegate TrimPriorityComparison
        {
            set
            {
                if (value != null)
                {
                    if (trimPriorityComparisonCallback == null)
                    {
                        trimPriorityComparisonCallback = new PriorityComparisonCallback { Delegate = value };
                        SetTrimPriorityComparison(trimPriorityComparisonCallback);
                    }
                    trimPriorityComparisonCallback.Delegate = value;
                }
                else if (trimPriorityComparisonCallback != null)
                {
                    SetTrimPriorityComparison(null);
                    trimPriorityComparisonCallback.Dispose();
                    trimPriorityComparisonCallback = null;
                }
            }
        }

        private PriorityComparisonCallback compressPriorityComparisonCallback;

        /// <summary>
        /// Sets the compress priority comparison.
        /// </summary>
        /// <value>
        /// The compress priority comparison.
        /// </value>
        public PriorityComparisonDelegate CompressPriorityComparison
        {
            set
            {
                if (value != null)
                {
                    if (compressPriorityComparisonCallback == null)
                    {
                        compressPriorityComparisonCallback = new PriorityComparisonCallback { Delegate = value };
                        SetCompressPriorityComparison(compressPriorityComparisonCallback);
                    }
                    compressPriorityComparisonCallback.Delegate = value;
                }
                else if (compressPriorityComparisonCallback != null)
                {
                    SetCompressPriorityComparison(null);
                    compressPriorityComparisonCallback.Dispose();
                    compressPriorityComparisonCallback = null;
                }
            }
        }

        private PriorityComparisonCallback concludePriorityComparisonCallback;

        /// <summary>
        /// Sets the conclude priority comparison.
        /// </summary>
        /// <value>
        /// The conclude priority comparison.
        /// </value>
        public PriorityComparisonDelegate ConcludePriorityComparison
        {
            set
            {
                if (value != null)
                {
                    if (concludePriorityComparisonCallback == null)
                    {
                        concludePriorityComparisonCallback = new PriorityComparisonCallback { Delegate = value };
                        SetConcludePriorityComparison(concludePriorityComparisonCallback);
                    }
                    concludePriorityComparisonCallback.Delegate = value;
                }
                else if (concludePriorityComparisonCallback != null)
                {
                    SetConcludePriorityComparison(null);
                    concludePriorityComparisonCallback.Dispose();
                    concludePriorityComparisonCallback = null;
                }
            }
        }



        /// <summary>
        /// Gets the variable from tag.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="tagObject">The tag object. This parameter can be null.</param>
        /// <returns>A variable associated with this tag.</returns>
        /// <unmanaged>HRESULT IUIAnimationManager::GetVariableFromTag([In, Optional] void* object,[In] unsigned int id,[Out] IUIAnimationVariable** variable)</unmanaged>
        public SharpDX.Animation.Variable GetVariableFromTag(int id, object tagObject = null)
        {
            var tagObjectHandle = GCHandle.Alloc(tagObject);
            try
            {
                return GetVariableFromTag(GCHandle.ToIntPtr(tagObjectHandle), id);
            } 
            finally
            {
                tagObjectHandle.Free();
            }
        }

        /// <summary>
        /// Gets the storyboard from tag.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="tagObject">The tag object. This parameter can be null.</param>
        /// <returns>A storyboard associated with this tag.</returns>
        /// <unmanaged>HRESULT IUIAnimationManager::GetStoryboardFromTag([In, Optional] void* object,[In] unsigned int id,[Out] IUIAnimationStoryboard** storyboard)</unmanaged>
        public SharpDX.Animation.Storyboard GetStoryboardFromTag(int id, object tagObject = null)
        {
            var tagObjectHandle = GCHandle.Alloc(tagObject);
            try
            {
                return GetStoryboardFromTag(GCHandle.ToIntPtr(tagObjectHandle), id);
            }
            finally
            {
                tagObjectHandle.Free();
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (statusEventHandler != null)
            {
                SetManagerEventHandler(null);
                statusEventHandler.Dispose();
                statusEventHandler = null;
            }

            if (cancelPriorityComparisonCallback != null)
            {
                SetCancelPriorityComparison(null);
                cancelPriorityComparisonCallback.Dispose();
                cancelPriorityComparisonCallback = null;
            }

            if (concludePriorityComparisonCallback != null)
            {
                SetConcludePriorityComparison(null);
                concludePriorityComparisonCallback.Dispose();
                concludePriorityComparisonCallback = null;
            }

            if (compressPriorityComparisonCallback != null)
            {
                SetCompressPriorityComparison(null);
                compressPriorityComparisonCallback.Dispose();
                compressPriorityComparisonCallback = null;
            }

            if (concludePriorityComparisonCallback != null)
            {
                SetConcludePriorityComparison(null);
                concludePriorityComparisonCallback.Dispose();
                concludePriorityComparisonCallback = null;
            }

            base.Dispose(disposing);
        }
    }
}