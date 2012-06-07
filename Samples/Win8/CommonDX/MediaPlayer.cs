// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using SharpDX;
using SharpDX.MediaFoundation;

namespace CommonDX
{
    /// <summary>
    /// A Media Player. This is a port of the C++ "Media engine native C++ video playback sample".
    /// </summary>
    public class MediaPlayer
    {
        private DXGIDeviceManager dxgiDeviceManager;
        private MediaEngine mediaEngineEx;

        public MediaPlayer()
        {
        }
        
        public virtual void Initialize(DeviceManager devices)
        {
            // Startup MediaManager
            MediaManager.Startup();

            // Create a DXGI Device Manager
            dxgiDeviceManager = new DXGIDeviceManager();
            dxgiDeviceManager.ResetDevice(devices.DeviceDirect3D);

            // Setup Media Engine attributes
            var attributes = new MediaEngineAttributes
                                 {
                                     DxgiManager = dxgiDeviceManager,
                                     VideoOutputFormat = (int) SharpDX.DXGI.Format.B8G8R8A8_UNorm
                                 };

            using (var factory = new MediaEngineClassFactory())
            using (var mediaEngine = new MediaEngine(factory, attributes, MediaEngineCreateflags.None))
                mediaEngineEx = mediaEngine.QueryInterface<MediaEngineEx>();

            // Register for playback notification from MediaEngine
            mediaEngineEx.PlaybackEvent += MediaEngineExOnPlaybackEvent;
        }

        private void MediaEngineExOnPlaybackEvent(MediaEngineEvent mediaEvent, long param1, int param2)
        {
            //throw new NotImplementedException();
        }
    }
}