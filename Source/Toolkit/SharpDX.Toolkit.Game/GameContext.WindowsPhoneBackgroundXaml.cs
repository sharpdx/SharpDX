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
        /// On Windows Phone, the Toolkit supports the following controls:
        /// <ul>
        /// <li>XAML control inheriting <see cref="System.Windows.Controls.DrawingSurfaceBackgroundGrid"/></li>
        /// <li>XAML control inheriting <see cref="System.Windows.Controls.DrawingSurface"/></li>
        /// </ul>
        /// </remarks>
        public GameContext(object control = null, int requestedWidth = 0, int requestedHeight = 0)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control", "Does no support null control on WP8 platform");
            }
            
            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            var controlType = Control.GetType();
            if (Utilities.IsTypeInheritFrom(controlType, "System.Windows.Controls.DrawingSurfaceBackgroundGrid"))
            {
                ContextType = GameContextType.WindowsPhoneBackgroundXaml;
            }
            else if (Utilities.IsTypeInheritFrom(controlType, "System.Windows.Controls.DrawingSurface"))
            {
                ContextType = GameContextType.WindowsPhoneXaml;
            }
            else
            {
                throw new ArgumentException("Control is not supported. Must inherit from System.Windows.Forms.Control (WinForm) or System.Windows.Controls.Border (WPF hosting WinForm) or SharpDX.Toolkit.SharpDXElement (WPF)");
            }
        }
    }
}
#endif