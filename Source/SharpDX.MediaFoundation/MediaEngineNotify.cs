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
namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(MediaEngineNotifyShadow))]
    internal partial interface MediaEngineNotify
    {
        /// <summary>	
        /// <p>[This documentation is preliminary and is subject to change.]</p><p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p>Notifies the application when a playback event occurs.</p>	
        /// </summary>	
        /// <param name="mediaEngineEvent"><dd> <p>A member of the <strong><see cref="SharpDX.MediaFoundation.MediaEngineEvent"/></strong> enumeration that specifies the event.</p> </dd></param>	
        /// <param name="param1"><dd> <p>The first event parameter. The meaning of this parameter depends on the event code.</p> </dd></param>	
        /// <param name="param2"><dd> <p>The second event parameter. The meaning of this parameter depends on the event code.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <msdn-id>hh447963</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaEngineNotify::EventNotify([In] unsigned int event,[In] ULONG_PTR param1,[In] unsigned int param2)</unmanaged>	
        /// <unmanaged-short>IMFMediaEngineNotify::EventNotify</unmanaged-short>	
        void OnPlaybackEvent(MediaEngineEvent mediaEngineEvent, long param1, int param2);
    }
}