// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Text;
using System.Text.RegularExpressions;

namespace SharpGen.Logging
{
    /// <summary>
    /// <see cref="ILogger"/> base implementation.
    /// </summary>
    public abstract class LoggerBase : ILogger
    {
        private static Regex regex = new Regex(@"^\s*at\s+([^\)]+)\)\s+in\s+(.*):line\s+(\d+)");

        /// <summary>
        /// Exits the process with the specified reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="exitCode">The exit code</param>
        public abstract void Exit(string reason, int exitCode);

        /// <summary>
        /// Logs the specified log message.
        /// </summary>
        /// <param name="logLevel">The log level</param>
        /// <param name="logLocation">The log location.</param>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        public abstract void Log(LogLevel logLevel, LogLocation logLocation, string context, string message, Exception exception, params object[] parameters);


        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="logLocation">The log location.</param>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string FormatMessage(LogLevel logLevel, LogLocation logLocation, string context, string message, Exception exception, params object[] parameters)
        {
            var lineMessage = new StringBuilder();

            if (logLocation != null)
                lineMessage.AppendFormat("{0}({1},{2}): ", logLocation.File, logLocation.Line, logLocation.Column);

            // Write log parsable by Visual Studio
            var levelName = Enum.GetName(typeof (LogLevel), logLevel).ToLower();
            lineMessage.AppendFormat("{0}:{1}", levelName == "fatal" ? "error:fatal":levelName , FormatMessage(context, message, parameters));

            return lineMessage.ToString();
        }

        /// <summary>
        /// Formats the message.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string FormatMessage(string context, string message, params object[] parameters)
        {
            var lineMessage = new StringBuilder();

            // Write log parsable by Visual Studio
            lineMessage.AppendFormat("{0}{1}", (context != null) ? " in " + context + " " : "", message != null ? string.Format(message, parameters) : "");

            return lineMessage.ToString();
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="logLocation">The log location.</param>
        /// <param name="ex">The exception to log.</param>
        protected void LogException(LogLocation logLocation, Exception ex)
        {
            // Print friendly error parsable by Visual Studio in order to display them in the Error List
            var reader = new StringReader(ex.ToString());

            // And write the exception parsable by Visual Studio
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    string methodLocation = match.Groups[1].Value;
                    string fileName = match.Groups[2].Value;
                    int lineNumber;
                    int.TryParse(match.Groups[3].Value, out lineNumber);
                    Log( LogLevel.Error, new LogLocation(fileName, lineNumber, 1), methodLocation, "Exception", null);
                }
                else
                {
                    // Escape a line
                    Log(LogLevel.Error, logLocation, null, line.Replace("{", "{{").Replace("}", "}}"), null);
                }
            }
        }
    }
}