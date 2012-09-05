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

namespace SharpDX.Toolkit.Diagnostics
{
    /// <summary>
    /// Class used to log warning, error, info messages.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// An action to log a message.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message">The message.</param>
        public delegate void LogAction(Logger logger, LogMessage message);

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        public Logger()
        {
            Messages = new List<LogMessage>();
        }

        /// <summary>
        /// List of logged messages.
        /// </summary>
        public readonly List<LogMessage> Messages;

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; set; }

        /// <summary>
        /// Occurs when a new message is logged.
        /// </summary>
        public event LogAction NewMessageLogged;

        /// <summary>
        /// Logs an Error with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public void Error(string errorMessage)
        {
            LogMessage(new LogMessage(LogMessageType.Error, errorMessage));
        }

        /// <summary>
        /// Logs an Error with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Error(string errorMessage, params object[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            Error(string.Format(errorMessage, parameters));
        }

        /// <summary>
        /// Logs a warning with the specified warning message.
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        public void Warning(string warningMessage)
        {
            LogMessage(new LogMessage(LogMessageType.Warning, warningMessage));
        }

        /// <summary>
        /// Logs a warning with the specified warning message.
        /// </summary>
        /// <param name="warningMessage">The warning message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Warning(string warningMessage, params object[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            Warning(string.Format(warningMessage, parameters));
        }

        /// <summary>
        /// Logs a info with the specified info message.
        /// </summary>
        /// <param name="infoMessage">The info message.</param>
        public void Info(string infoMessage)
        {
            LogMessage(new LogMessage(LogMessageType.Info, infoMessage));
        }

        /// <summary>
        /// Logs a warning with the specified info message.
        /// </summary>
        /// <param name="infoMessage">The info message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Info(string infoMessage, params object[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            Info(string.Format(infoMessage, parameters));
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void LogMessage(LogMessage message)
        {
            if (message.Type == LogMessageType.Error)
                HasErrors = true;

            Messages.Add(message);

            var handler = NewMessageLogged;
            if (handler != null) handler(this, message);
        }
    }
}