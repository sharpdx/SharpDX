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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace MiniTriApp
{
    /// <summary>
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// The preferred way to access Direct3D on WP8 is by using SharpDX.Toolkit.
    /// </summary>
    class BasicTimer
    {
        readonly Stopwatch _watch = new Stopwatch();
        private long _currentTime;
        private long _startTime;
        private long _lastTime;

        public BasicTimer()
        {
            Reset();
        }

        public float Delta { get; set; }

        public float Total { get; set; }

        internal void Update()
        {
             _currentTime = _watch.ElapsedMilliseconds;           
            Total = (_currentTime - _startTime) / 1000.0f;
            if (_lastTime == _startTime)
            {
                Delta = 1.0f/60.0f;
            }
            else
            {
                Delta = (_currentTime - _lastTime)/1000.0f;
            }
            _lastTime = _currentTime;
        }

        internal void Reset()
        {
            _watch.Reset();
            Update();
            _startTime = _currentTime;
            Total = 0.0f;
            Delta = 1.0f / 60.0f;
            _watch.Start();
        }
    }
}
