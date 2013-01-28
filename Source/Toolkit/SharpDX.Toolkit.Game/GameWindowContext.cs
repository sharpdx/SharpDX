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

namespace SharpDX.Toolkit
{
    /// <summary>
    /// Contains context used to render the game (NativeWindow...etc.).
    /// </summary>
    public class GameWindowContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowContext" /> class.
        /// </summary>
        public GameWindowContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowContext" /> class.
        /// </summary>
        /// <param name="windowContext">The window context.</param>
        /// <param name="requestedWidth">Requested width of the window.</param>
        /// <param name="requestedHeight">Requested height of the window.</param>
        public GameWindowContext(object windowContext, int requestedWidth = 0, int requestedHeight = 0)
        {
            WindowContext = windowContext;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;

        }

        /// <summary>
        /// The window context.
        /// </summary>
        public object WindowContext;

        /// <summary>
        /// The requested width.
        /// </summary>
        public int RequestedWidth;

        /// <summary>
        /// The requested height.
        /// </summary>
        public int RequestedHeight;

        /// <summary>
        /// The init calback.
        /// </summary>
        internal VoidAction InitCalback;

        /// <summary>
        /// The run callback.
        /// </summary>
        internal VoidAction RunCallback; 
    }
}