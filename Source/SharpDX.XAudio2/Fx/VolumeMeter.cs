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
using SharpDX.XAPO;

namespace SharpDX.XAudio2.Fx
{
    /// <summary>
    /// A VolumeMeter XAudio2 AudioProcessor.
    /// </summary>
    public partial class VolumeMeter : AudioProcessorParamNative<VolumeMeterLevels>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeMeter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public VolumeMeter(XAudio2 device)
            : this(device, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeMeter" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="isUsingDebug">if set to <c>true</c> [is using debug].</param>
        /// <exception cref="System.InvalidOperationException">XAudio2 must be initialized before calling this constructor</exception>
        public VolumeMeter(XAudio2 device, bool isUsingDebug)
            : base(device)
        {
#if !WINDOWS_UWP
            if (device.Version == XAudio2Version.Version27)
            {
                Guid clsid = (isUsingDebug) ? XAudio2FxContants.CLSID_AudioVolumeMeter_Debug : XAudio2FxContants.CLSID_AudioVolumeMeter;
                Utilities.CreateComInstance(clsid, Utilities.CLSCTX.ClsctxInprocServer, XAudio2FxContants.CLSID_IAudioProcessor, this);
                return;
            }
#endif
            if (device.Version == XAudio2Version.Version28)
            {
                XAudio28Functions.CreateAudioVolumeMeter(this);
            }
#if WINDOWS_UWP
            else if (device.Version == XAudio2Version.Version29)
            {
                XAudio29Functions.CreateAudioVolumeMeter(this);
            }
#endif
            else
            {
                throw new InvalidOperationException("XAudio2 must be initialized before calling this constructor");
            }
        }
    }
}