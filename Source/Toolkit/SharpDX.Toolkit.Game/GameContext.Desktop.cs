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
#if !W8CORE
using System;
using SharpDX.Windows;
using System.Windows.Forms;

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
        /// <param name="control">The control, platform dependent. See remarks for supported controls.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        /// <exception cref="System.ArgumentException">Control is not supported. Must inherit from System.Windows.Forms.Control (WinForm) or System.Windows.Controls.Border (WPF hosting WinForm) or SharpDX.Toolkit.SharpDXElement (WPF)</exception>
        /// <remarks>
        /// On Windows Desktop, the Toolkit supports the following controls:
        /// <ul>
        /// <li>WinForm control inheriting <see cref="System.Windows.Forms.Control"/></li>
        /// <li>WPF control inheriting <see cref="System.Windows.Controls.Border"/> for Hwnd hosting</li>
        /// <li>WPF D3DImage control <see cref="SharpDX.Toolkit.SharpDXElement"/></li>
        /// </ul>
        /// </remarks>
        public GameContext(object control = null, int requestedWidth = 0, int requestedHeight = 0)
        {
            Control = control ?? CreateDefaultControl();
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            var controlType = Control.GetType();
            if (Utilities.IsTypeInheritFrom(controlType, "System.Windows.Forms.Control"))
            {
                ContextType = GameContextType.Desktop;
            }
            else if (Utilities.IsTypeInheritFrom(controlType, "System.Windows.Controls.Border"))
            {
                ContextType = GameContextType.DesktopHwndWpf;
            }
            else if (Utilities.IsTypeInheritFrom(controlType, "SharpDX.Toolkit.SharpDXElement"))
            {
                ContextType = GameContextType.DesktopWpf;
            }
            else
            {
                throw new ArgumentException("Control is not supported. Must inherit from System.Windows.Forms.Control (WinForm) or System.Windows.Controls.Border (WPF hosting WinForm) or SharpDX.Toolkit.SharpDXElement (WPF)");
            }
        }

        private static object CreateDefaultControl()
        {
            return new RenderForm("Toolkit Game");
        }

        /// <summary>
        /// Gets or sets a value indicating whether the render loop should use the default <see cref="Application.DoEvents"/> instead of a custom window message loop lightweight for GC. Default is false
        /// </summary>
        /// <value><c>true</c> if use the default <see cref="Application.DoEvents"/>; otherwise, <c>false</c>.</value>
        public bool UseApplicationDoEvents { get; set; }
    }
}
#endif