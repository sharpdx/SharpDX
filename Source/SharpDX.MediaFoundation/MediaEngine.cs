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

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Delegate MediaEngineNotifyDelegate {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    /// <param name="mediaEvent">The media event.</param>
    /// <param name="param1">The param1.</param>
    /// <param name="param2">The param2.</param>
    public delegate void MediaEngineNotifyDelegate(MediaEngineEvent mediaEvent, long param1, int param2);
    
    public partial class MediaEngine
    {
        private MediaEngineNotifyImpl mediaEngineNotifyImpl;

        /// <summary>
        /// Initializes an instance of the <see cref="MediaEngine"/> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="attributes"></param>
        /// <param name="createFlags"> </param>
        /// <param name="playbackCallback"></param>
        /// <msdn-id>hh447921</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEngineClassFactory::CreateInstance([In] MF_MEDIA_ENGINE_CREATEFLAGS dwFlags,[In] IMFAttributes* pAttr,[Out, Fast] IMFMediaEngine** ppPlayer)</unmanaged>	
        /// <unmanaged-short>IMFMediaEngineClassFactory::CreateInstance</unmanaged-short>	
        public MediaEngine(MediaEngineClassFactory factory, MediaEngineAttributes attributes = null, MediaEngineCreateFlags createFlags = MediaEngineCreateFlags.None, MediaEngineNotifyDelegate playbackCallback = null)
        {
            // Create engine attributes if null
            attributes = attributes ?? new MediaEngineAttributes();

            if (playbackCallback != null)
            {
                PlaybackEvent += playbackCallback;
            }

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
            private readonly MediaEngine MediaEngine;

            public MediaEngineNotifyImpl(MediaEngine mediaEngine)
            {
                MediaEngine = mediaEngine;
            }

            public void OnPlaybackEvent(MediaEngineEvent mediaEngineEvent, long param1, int param2)
            {
                MediaEngine.OnPlaybackEvent(mediaEngineEvent, param1, param2);
            }
        }

        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Sets the URL of a media resource.</p>	
        /// </summary>	
        /// <param name="urlRef"><dd> <p>The URL of the media resource.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>This method corresponds to setting the <strong>src</strong> attribute of the <strong>HTMLMediaElement</strong> interface in HTML5.</p><p>The URL specified by this method takes precedence over media resources specified in the <strong><see cref="SharpDX.MediaFoundation.MediaEngine.SetSourceElements"/></strong> method. To load the URL, call <strong><see cref="SharpDX.MediaFoundation.MediaEngine.Load"/></strong>.</p><p>This method asynchronously loads the URL. When the operation starts, the Media Engine sends an <strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent.LoadStart"/></strong> event. If no errors occur during the <strong>Load</strong> operation, several other events are generated, including the following.</p><ul> <li><strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent.LoadedMetadata"/></strong></li> <li><strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent.LoadedData"/></strong></li> <li><strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent.CanPlay"/></strong></li> <li><strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent.CanPlayThrough"/></strong></li> </ul><p>If the Media Engine is unable to load the URL, the Media Engine sends an <strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent.Error"/></strong> event. </p><p>For more information about event handling in the Media Engine, see <strong><see cref="SharpDX.MediaFoundation.MediaEngineNotify"/></strong>.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFMediaEngine::SetSource']/*"/>	
        /// <msdn-id>hh448017</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEngine::SetSource([In] wchar_t* pUrl)</unmanaged>	
        /// <unmanaged-short>IMFMediaEngine::SetSource</unmanaged-short>	
        public string Source
        {
            set
            {
                SetSource(Utilities.StringToCoTaskMemUni(value));
            }
        }
    }
}