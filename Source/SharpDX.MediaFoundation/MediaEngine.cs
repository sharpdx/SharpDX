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
#if WIN8METRO
using System;

namespace SharpDX.MediaFoundation
{
    public partial class MediaEngine
    {
        private MediaEngineNotifyImpl mediaEngineNotifyImpl;


        public delegate void MediaEngineNotifyDelegate(MediaEngineEvent mediaEvent, long param1, int param2);

        /// <summary>
        /// Initializes an instance of the <see cref="MediaEngine"/> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="attributes"></param>
        /// <param name="createFlags"> </param>
        public MediaEngine(MediaEngineClassFactory factory, MediaEngineAttributes attributes = null, MediaEngineCreateflags createFlags = MediaEngineCreateflags.None)
        {
            // Create engine attributes if null
            attributes = attributes ?? new MediaEngineAttributes();

            // Setup by default the MediaEngine notify as it is mandatory
            var notifier = new MediaEngineNotifyImpl(this);
            try
            {
                attributes.Set(MediaEngineAttributeKeys.Callback, MediaEngineNotifyShadow.ToIntPtr(notifier));
                factory.CreateInstance(createFlags, attributes, this);
                mediaEngineNotifyImpl = notifier;
            } catch
            {
                notifier.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Media engine playback event.
        /// </summary>
        public event MediaEngineNotifyDelegate PlaybackEvent;

        protected override unsafe void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mediaEngineNotifyImpl != null)
                {
                    mediaEngineNotifyImpl.Dispose();
                    mediaEngineNotifyImpl = null;
                }
            }

            base.Dispose(disposing);
        }

        private void OnPlaybackEvent(MediaEngineEvent mediaevent, long param1, int param2)
        {
            MediaEngineNotifyDelegate handler = PlaybackEvent;
            if (handler != null) handler(mediaevent, param1, param2);
        }

        private class MediaEngineNotifyImpl : CallbackBase, MediaEngineNotify
        {
            private MediaEngine MediaEngine;

            public MediaEngineNotifyImpl(MediaEngine mediaEngine)
            {
                MediaEngine = mediaEngine;
            }

            public void OnPlaybackEvent(MediaEngineEvent mediaEngineEvent, long param1, int param2)
            {
                MediaEngine.OnPlaybackEvent(mediaEngineEvent, param1, param2);
            }
        }
    }
}
#endif