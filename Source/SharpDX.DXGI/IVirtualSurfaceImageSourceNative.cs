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
using SharpDX.Mathematics.Interop;

namespace SharpDX.DXGI
{
    public partial class IVirtualSurfaceImageSourceNative 
    {
        private IVirtualSurfaceUpdatesCallbackNative callback;
        private EventHandler<EventArgs> updatesNeeded;

        /// <summary>
        /// Gets the set of regions that must be updated on the shared surface.
        /// </summary>
        public RawRectangle[] UpdateRectangles
        {
            get
            {
                int count = this.GetUpdateRectCount();
                var regionToUpdate = new RawRectangle[count];
                this.GetUpdateRects(regionToUpdate, count);
                return regionToUpdate;
            }
        }

        /// <summary>
        /// Event fired when an update is needed. Use <see cref="IVirtualSurfaceImageSourceNative.UpdateRectangles"/> to get the region to update.
        /// </summary>
        public event EventHandler<EventArgs> UpdatesNeeded
        {
            add
            {
                if (callback == null)
                {
                    callback = new VirtualSurfaceUpdatesCallbackNativeCallback(this);
                    RegisterForUpdatesNeeded(callback);
                }

                updatesNeeded = (EventHandler<EventArgs>)Delegate.Combine(updatesNeeded, value);

            }

            remove
            {

                updatesNeeded = (EventHandler<EventArgs>)Delegate.Remove(updatesNeeded, value);
            }
        }

        private void OnUpdatesNeeded()
        {
            if (updatesNeeded != null)
                updatesNeeded(this, EventArgs.Empty);
        }

        private class VirtualSurfaceUpdatesCallbackNativeCallback : CallbackBase, IVirtualSurfaceUpdatesCallbackNative
        {
            IVirtualSurfaceImageSourceNative eventCallback;

            public VirtualSurfaceUpdatesCallbackNativeCallback(IVirtualSurfaceImageSourceNative eventCallbackArg)
            {
                eventCallback = eventCallbackArg;
            }

            public void UpdatesNeeded()
            {
                eventCallback.OnUpdatesNeeded();
            }
        }
    }
}