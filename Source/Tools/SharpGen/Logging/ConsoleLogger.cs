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
using System.IO;

namespace SharpGen.Logging
{
    /// <summary>
    /// Default logger to Console.Out.
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        public ConsoleLogger()
        {
            Output = Console.Out;
        }

        /// <summary>
        /// Gets or sets the output <see cref="TextWriter"/>. Default is set to <see cref="Console.Out"/>.
        /// </summary>
        /// <value>The output <see cref="TextWriter"/>.</value>
        public TextWriter Output { get; set; }

        /// <summary>
        /// Exits the process with the specified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="exitCode">The exit code</param>
        public override void Exit(string reason, int exitCode)
        {
            if (Output == null)
                return;

            Logger.Error("Process stopped. " + reason);
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Logs the specified log message.
        /// </summary>
        /// <param name="logLevel">The log level</param>
        /// <param name="logLocation">The log location.</param>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        public override void Log(LogLevel logLevel, LogLocation logLocation, string context, string message, Exception exception, params object[] parameters)
        {
            lock (this)
            {
                if (Output == null)
                    return;

                string lineMessage = FormatMessage(logLevel, logLocation, context, message, exception, parameters);

                Output.WriteLine(lineMessage);
                Output.Flush();

                if (exception != null)
                    LogException(logLocation, exception);
            }
        }
    }
}