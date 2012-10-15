using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace MiniTriApp
{
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
            Total = _currentTime - _startTime / 1000.0f;
            if (_lastTime == _startTime)
            {
                Delta = 1.0f/60.0f;
            }
            else
            {
                Delta = _currentTime - _lastTime/1000.0f;
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
        }
    }
}
