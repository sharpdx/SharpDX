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

namespace SharpDX.Toolkit
{
    internal abstract class GamePlatform
    {
        public static GamePlatform Create()
        {
            throw new NotImplementedException();
        }

        internal abstract GameWindow Window { get; }

        internal event EventHandler<EventArgs> Activated;

        internal event EventHandler<EventArgs> Deactivated;

        internal event EventHandler<EventArgs> Exiting;

        internal event EventHandler<EventArgs> Idle;

        internal event EventHandler<EventArgs> Resume;

        internal event EventHandler<EventArgs> Suspend;

        protected void OnActivated(EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null) handler(this, e);
        }

        protected void OnDeactivated(EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null) handler(this, e);
        }

        protected void OnExiting(EventArgs e)
        {
            EventHandler<EventArgs> handler = Exiting;
            if (handler != null) handler(this, e);
        }

        protected void OnIdle(EventArgs e)
        {
            EventHandler<EventArgs> handler = Idle;
            if (handler != null) handler(this, e);
        }

        protected void OnResume(EventArgs e)
        {
            EventHandler<EventArgs> handler = Resume;
            if (handler != null) handler(this, e);
        }

        protected void OnSuspend(EventArgs e)
        {
            EventHandler<EventArgs> handler = Suspend;
            if (handler != null) handler(this, e);
        }
    }
}