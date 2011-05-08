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

namespace SharpCore.Logging
{
    /// <summary>
    /// Source code location of a logging message.
    /// </summary>
    public class LogLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogLocation"/> class.
        /// </summary>
        /// <param name="filePath">The file location.</param>
        public LogLocation(string filePath) : this(filePath, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogLocation"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="line">The line.</param>
        public LogLocation(string filePath, int line) : this(filePath, line, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogLocation"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="line">The line.</param>
        /// <param name="column">The column.</param>
        public LogLocation(string filePath, int line, int column)
        {
            File = filePath;
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Gets the file location.
        /// </summary>
        /// <value>The file location.</value>
        public string File { get; private set; }

        /// <summary>
        /// Gets the line inside the file.
        /// </summary>
        /// <value>The line.</value>
        public int Line { get; private set; }

        /// <summary>
        /// Gets the column inside the line of the file.
        /// </summary>
        /// <value>The column.</value>
        public int Column { get; private set; }
    }
}