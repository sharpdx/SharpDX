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
#if WIN8METRO

using System;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// A <see cref="GameContext"/> to use for rendering to an existing WinForm <see cref="Control"/>.
    /// </summary>
    public partial class GameContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameContext" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="requestedWidth">Width of the requested.</param>
        /// <param name="requestedHeight">Height of the requested.</param>
        /// <exception cref="NotSupportedException">Is thrown when <paramref name="control"/> is not supported.</exception>
        public GameContext(global::Windows.UI.Xaml.Controls.Grid control, int requestedWidth = 0, int requestedHeight = 0)
        {
            if(control == null)
                throw new ArgumentNullException("control");

            ValidateControl(control);

            Control = control;
            RequestedWidth = requestedWidth;
            RequestedHeight = requestedHeight;
            ContextType = GameContextType.WinRTBackgroundXaml;
        }

        /// <summary>
        /// The control used as a GameWindow context (either an instance of <see cref="System.Windows.Forms.Control"/> or <see cref="System.Windows.Controls.Control"/>.
        /// </summary>
        public readonly object Control;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Control"/> to <see cref="GameContext"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The result of the conversion.</returns>
        /// <exception cref="NotSupportedException">Is thrown when <paramref name="control"/> is not supported.</exception>
        public static implicit operator GameContext(global::Windows.UI.Xaml.Controls.Grid control)
        {
            ValidateControl(control);

            return new GameContext(control);
        }

        /// <summary>
        /// Checks if the provided control supports any of the native interfaces for Direct3D interop.
        /// </summary>
        /// <param name="control">The control to check.</param>
        /// <exception cref="NotSupportedException">Is thrown when <paramref name="control"/> is not supported.</exception>
        private static void ValidateControl(global::Windows.UI.Xaml.FrameworkElement control)
        {
            using (var comObject = new ComObject(control))
            {
                if (SupportsInterface<DXGI.ISwapChainBackgroundPanelNative>(comObject)) return;

#if DIRECTX11_2
                if (SupportsInterface<DXGI.ISwapChainPanelNative>(comObject)) return;
#endif
            }

            throw new NotSupportedException("Expected a control supporting native Direct3D interop");
        }

        /// <summary>
        /// Checks if the provided <see cref="ComObject"/> instance supports a certain interface.
        /// </summary>
        /// <typeparam name="T">The interface type to check the support for.</typeparam>
        /// <param name="comObject">The <see cref="ComObject"/> instance to check for support.</param>
        /// <returns>true - if the interface <typeparamref name="T"/> is supported, false - otherwise.</returns>
        private static bool SupportsInterface<T>(ComObject comObject)
            where T : ComObject
        {
            var suppotedInterface = comObject.QueryInterfaceOrNull<T>();
            if (suppotedInterface != null)
            {
                suppotedInterface.Dispose();
                return true;
            }

            return false;
        }
    }
}
#endif