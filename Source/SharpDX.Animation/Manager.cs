// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
        private ManagerEventHandlerShadow shadowEventHandler;

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
                if (shadowEventHandler == null)
                {
                    shadowEventHandler = new ManagerEventHandlerShadow();
                    SetManagerEventHandler_(ManagerEventHandlerShadow.ToIntPtr(shadowEventHandler));
                }
                shadowEventHandler.Delegates += value;
            }

            remove
            {
                if (shadowEventHandler == null) return;

                shadowEventHandler.Delegates -= value;

                if (shadowEventHandler.Delegates.GetInvocationList().Length == 0)
                {
                    SetManagerEventHandler_(IntPtr.Zero);
                    shadowEventHandler.Dispose();
                    shadowEventHandler = null;
                }
            }
        }

        private PriorityComparisonShadow cancelPriorityComparisonShadow;

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
                    if (cancelPriorityComparisonShadow == null)
                    {
                        cancelPriorityComparisonShadow = new PriorityComparisonShadow { Delegate = value };
                        SetCancelPriorityComparison_(PriorityComparisonShadow.ToIntPtr(cancelPriorityComparisonShadow));
                    }
                    cancelPriorityComparisonShadow.Delegate = value;
                }
                else if (cancelPriorityComparisonShadow != null)
                {
                    SetCancelPriorityComparison_(IntPtr.Zero);
                    cancelPriorityComparisonShadow.Dispose();
                    cancelPriorityComparisonShadow = null;
                }
            }
        }

        private PriorityComparisonShadow trimPriorityComparisonShadow;

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
                    if (trimPriorityComparisonShadow == null)
                    {
                        trimPriorityComparisonShadow = new PriorityComparisonShadow { Delegate = value };
                        SetTrimPriorityComparison_(PriorityComparisonShadow.ToIntPtr(trimPriorityComparisonShadow));
                    }
                    trimPriorityComparisonShadow.Delegate = value;
                }
                else if (trimPriorityComparisonShadow != null)
                {
                    SetTrimPriorityComparison_(IntPtr.Zero);
                    trimPriorityComparisonShadow.Dispose();
                    trimPriorityComparisonShadow = null;
                }
            }
        }

        private PriorityComparisonShadow compressPriorityComparisonShadow;

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
                    if (compressPriorityComparisonShadow == null)
                    {
                        compressPriorityComparisonShadow = new PriorityComparisonShadow { Delegate = value };
                        SetCompressPriorityComparison_(PriorityComparisonShadow.ToIntPtr(compressPriorityComparisonShadow));
                    }
                    compressPriorityComparisonShadow.Delegate = value;
                }
                else if (compressPriorityComparisonShadow != null)
                {
                    SetCompressPriorityComparison_(IntPtr.Zero);
                    compressPriorityComparisonShadow.Dispose();
                    compressPriorityComparisonShadow = null;
                }
            }
        }

        private PriorityComparisonShadow concludePriorityComparisonShadow;

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
                    if (trimPriorityComparisonShadow == null)
                    {
                        trimPriorityComparisonShadow = new PriorityComparisonShadow { Delegate = value };
                        SetConcludePriorityComparison_(PriorityComparisonShadow.ToIntPtr(trimPriorityComparisonShadow));
                    }
                    trimPriorityComparisonShadow.Delegate = value;
                }
                else if (trimPriorityComparisonShadow != null)
                {
                    SetConcludePriorityComparison_(IntPtr.Zero);
                    trimPriorityComparisonShadow.Dispose();
                    trimPriorityComparisonShadow = null;
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
            if (shadowEventHandler != null)
            {
                SetManagerEventHandler_(IntPtr.Zero);
                shadowEventHandler.Dispose();
                shadowEventHandler = null;
            }

            if (cancelPriorityComparisonShadow != null)
            {
                SetCancelPriorityComparison_(IntPtr.Zero);
                cancelPriorityComparisonShadow.Dispose();
                cancelPriorityComparisonShadow = null;
            }

            if (trimPriorityComparisonShadow != null)
            {
                SetConcludePriorityComparison_(IntPtr.Zero);
                trimPriorityComparisonShadow.Dispose();
                trimPriorityComparisonShadow = null;
            }

            if (compressPriorityComparisonShadow != null)
            {
                SetCompressPriorityComparison_(IntPtr.Zero);
                compressPriorityComparisonShadow.Dispose();
                compressPriorityComparisonShadow = null;
            }

            if (trimPriorityComparisonShadow != null)
            {
                SetConcludePriorityComparison_(IntPtr.Zero);
                trimPriorityComparisonShadow.Dispose();
                trimPriorityComparisonShadow = null;
            }

            base.Dispose(disposing);
        }
    }
}