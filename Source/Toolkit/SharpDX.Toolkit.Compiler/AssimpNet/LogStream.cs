/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Runtime.InteropServices;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// Callback delegate for Assimp's LogStream.
    /// </summary>
    /// <param name="msg">Log message</param>
    /// <param name="userData">User data that is passed to the callback</param>
    internal delegate void LogCallback(String msg, String userData);

    /// <summary>
    /// Represents a log stream, which receives all log messages and
    /// streams them somewhere.
    /// </summary>
    internal class LogStream {
        private AiLogStream m_logStream;
        private LogCallback m_logCallback;

        /// <summary>
        /// User data to be passed to the callback.
        /// </summary>
        public String UserData {
            get {
                return m_logStream.UserData;
            }
            set {
                m_logStream.UserData = value;
            }
        }

        /// <summary>
        /// Constructs a new LogStream.
        /// </summary>
        protected LogStream() {
            m_logStream = new AiLogStream(OnLogstream);
        }

        /// <summary>
        /// Constructs a new LogStream.
        /// </summary>
        /// <param name="userData">User-supplied data</param>
        protected LogStream(String userData) {
            m_logStream = new AiLogStream(OnLogstream, userData);
        }

        /// <summary>
        /// Constructs a new LogStream.
        /// </summary>
        /// <param name="callback">Callback called when messages are logged.</param>
        public LogStream(LogCallback callback) {
            m_logStream = new AiLogStream(OnLogstream);
            m_logCallback = callback;
        }

        /// <summary>
        /// Constructs a new LogStream.
        /// </summary>
        /// <param name="callback">Callback called when messages are logged.</param>
        /// <param name="userData">User-supplied data</param>
        public LogStream(LogCallback callback, String userData) {
            m_logStream = new AiLogStream(OnLogstream, userData);
            m_logCallback = callback;
        }

        /// <summary>
        /// Override this method to log a message for a subclass of Logstream, if no callback
        /// was set.
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="userData">User data</param>
        protected virtual void Log(String msg, String userData) { }

        internal void OnLogstream(String msg, IntPtr userData) {
            if(m_logCallback != null) {
                m_logCallback(msg, m_logStream.UserData);
            } else {
                Log(msg, m_logStream.UserData);
            }
        }

        internal void Attach() {
            AssimpMethods.AttachLogStream(ref m_logStream);
        }

        internal void Detach() {
            AssimpMethods.DetachLogStream(ref m_logStream);
        }
    }

    /// <summary>
    /// Log stream that writes messages to the Console.
    /// </summary>
    internal sealed class ConsoleLogStream : LogStream {

        /// <summary>
        /// Constructs a new console logstream.
        /// </summary>
        public ConsoleLogStream() : base() { }

        /// <summary>
        /// Constructs a new console logstream.
        /// </summary>
        /// <param name="userData">User supplied data</param>
        public ConsoleLogStream(String userData) : base(userData) { }

        /// <summary>
        /// Log a message to the console.
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="userData">Userdata</param>
        protected override void Log(String msg, String userData) {
            if(String.IsNullOrEmpty(userData)) {
                Console.WriteLine(msg);
            } else {
                Console.WriteLine(String.Format("{0}: {1}", userData, msg));
            }
        }
    }
}
