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
                else
                {
                    shadowEventHandler.Delegates += value;
                }
            }

            remove
            {
                shadowEventHandler.Delegates -= value;

                if (shadowEventHandler.Delegates.GetInvocationList().Length == 0)
                {
                    SetManagerEventHandler_(IntPtr.Zero);
                    shadowEventHandler.Dispose();
                    shadowEventHandler = null;
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
            var tagObjectPtr = Marshal.GetIUnknownForObject(tagObject);
            try
            {
                return GetVariableFromTag(tagObjectPtr, id);
            } 
            finally
            {
                if (tagObjectPtr != IntPtr.Zero) 
                    Marshal.Release(tagObjectPtr);
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
            var tagObjectPtr = Marshal.GetIUnknownForObject(tagObject);
            try
            {
                return GetStoryboardFromTag(tagObjectPtr, id);
            }
            finally
            {
                if (tagObjectPtr != IntPtr.Zero) 
                    Marshal.Release(tagObjectPtr);
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

            base.Dispose(disposing);
        }
    }
}