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
#if W8CORE
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
        public MediaEngine(MediaEngineClassFactory factory, MediaEngineAttributes attributes = null, MediaEngineCreateFlags createFlags = MediaEngineCreateFlags.None, MediaEngineNotifyDelegate playbackCallback = null)
        {
            // Create engine attributes if null
            attributes = attributes ?? new MediaEngineAttributes();

            PlaybackEvent = playbackCallback;

            // Setup by default the MediaEngine notify as it is mandatory
            mediaEngineNotifyImpl = new MediaEngineNotifyImpl(this);
            try
            {
                attributes.Set(MediaEngineAttributeKeys.Callback, new ComObject(MediaEngineNotifyShadow.ToIntPtr(mediaEngineNotifyImpl)));
                factory.CreateInstance(createFlags, attributes, this);
            } catch
            {
                mediaEngineNotifyImpl.Dispose();
                mediaEngineNotifyImpl = null;
                throw;
            }
        }

        /// <summary>
        /// Media engine playback event.
        /// </summary>
        public event MediaEngineNotifyDelegate PlaybackEvent;


        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Queries the Media Engine to find out whether a new video frame is ready.</p>	
        /// </summary>	
        /// <param name="ptsRef"><dd> <p>If a new frame is ready, receives the presentation time of the frame.</p> </dd></param>	
        /// <returns>true if new video frame is ready for display.</returns>	
        /// <remarks>	
        /// <p>In frame-server mode, the application should call this method whenever a vertical blank occurs in the display device. If the method returns <strong><see cref="SharpDX.Result.Ok"/></strong>, call <strong><see cref="SharpDX.MediaFoundation.MediaEngine.TransferVideoFrame"/></strong> to blit the frame to the render target. If the method returns <strong>S_FALSE</strong>, wait for the next vertical blank and call the method again.</p><p>Do not call this method in rendering mode or audio-only mode. </p>	
        /// </remarks>	
        /// <msdn-id>hh448006</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEngine::OnVideoStreamTick([Out] longlong* pPts)</unmanaged>	
        /// <unmanaged-short>IMFMediaEngine::OnVideoStreamTick</unmanaged-short>	
        public bool OnVideoStreamTick(out long ptsRef)
        {
            return OnVideoStreamTick_(out ptsRef).Success;
        }

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