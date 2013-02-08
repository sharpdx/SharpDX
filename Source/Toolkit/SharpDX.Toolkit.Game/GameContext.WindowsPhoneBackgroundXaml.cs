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
#if WP8
using System;
using System.Windows.Controls;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// A <see cref="GameContext"/> to use for rendering to an existing WinForm <see cref="Control"/>.
    /// </summary>
    public partial class GameContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext"/> class.
        /// </summary>
        /// <exception cref="System.NotSupportedException">Expecting a DrawingSurfaceBackgroundGrid or DrawingSurface to run the GameContext</exception>
        public GameContext()
        {
            throw new NotSupportedException("Expecting a DrawingSurfaceBackgroundGrid or DrawingSurface to run the GameContext");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public GameContext(DrawingSurfaceBackgroundGrid control, int requestedWidth = 0, int requestedHeight = 0)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            ContextType = GameContextType.WindowsPhoneBackgroundXaml;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        public GameContext(DrawingSurface control, int requestedWidth = 0, int requestedHeight = 0)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            ContextType = GameContextType.WindowsPhoneXaml;
        }

        /// <summary>
        /// The control used as a GameWindow context (either an instance of <see cref="DrawingSurfaceBackgroundGrid"/> or <see cref="DrawingSurface"/>.
        /// </summary>
        public readonly object Control;

        /// <summary>
        /// Performs an implicit conversion from <see cref="DrawingSurfaceBackgroundGrid"/> to <see cref="GameContext"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GameContext(DrawingSurfaceBackgroundGrid control)
        {
            return new GameContext(control);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DrawingSurface"/> to <see cref="GameContext"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator GameContext(DrawingSurface control)
        {
            return new GameContext(control);
        }
    }
}
#endif