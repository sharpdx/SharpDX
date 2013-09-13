﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Threading;

namespace SharpDX.Threading
{
    /// <summary>
    /// Task utility for threading.
    /// </summary>
    public class TaskUtil
    {
        /// <summary>
        /// Runs the specified action in a thread.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="taskName">Name of the task.</param>
        public static void Run(Action action, string taskName = "SharpDXTask")
        {
#if W8CORE
            System.Threading.Tasks.Task.Factory.StartNew(() => action(), System.Threading.Tasks.TaskCreationOptions.LongRunning);
#else
            var thread = new System.Threading.Thread(() => action()) { IsBackground = true, Name = taskName };
            thread.Start();
#endif
        }
    }
}