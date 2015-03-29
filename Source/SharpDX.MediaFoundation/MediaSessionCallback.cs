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
#if DESKTOP_APP
using System;

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Specialized callback for <see cref="MediaSession"/>. This callback automatically starts the callback on the session,
    /// handles <see cref="MediaEventGenerator.EndGetEvent"/> on invoke, dispatch the asynchronous event to an external action to
    /// be implemented by the client and stops the callback when the event <see cref="MediaEventTypes.SessionClosed"/> is received.
    /// </summary>
    public class MediaSessionCallback : AsyncCallbackBase
    {
        private readonly MediaSession session;
        private readonly Action<MediaEvent> eventCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaSessionCallback"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="eventCallback">The event callback. The object <see cref="MediaEvent"/> must be disposed by the callback when finished with it.</param>
        /// <exception cref="System.ArgumentNullException">session or eventCallback</exception>
        public MediaSessionCallback(MediaSession session, Action<MediaEvent> eventCallback)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            if (eventCallback == null)
                throw new ArgumentNullException("eventCallback");

            this.session = session;
            this.eventCallback = eventCallback;

            // Subscribe to next events automatically
            session.BeginGetEvent(this, null);
        }

        public override void Invoke(AsyncResult asyncResult)
        {
            // EndGetEvent mandatory
            var evt = session.EndGetEvent(asyncResult);

            var typeInfo = evt.TypeInfo;
            if (typeInfo != MediaEventTypes.SessionClosed)
            {
                // If not closed, continnue to subscribe to next events
                session.BeginGetEvent(this, null);
            }

            // Call the callback
            eventCallback(evt);
        }
    }
}
#endif