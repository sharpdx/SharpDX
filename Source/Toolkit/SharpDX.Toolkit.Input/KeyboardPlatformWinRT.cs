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

#if WIN8METRO

using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SharpDX.Toolkit.Input
{
    using System.Collections.Generic;

    /// <summary>
    /// A specific implementation of <see cref="KeyboardPlatform"/> for WinRT platform
    /// </summary>
    internal sealed class KeyboardPlatformWinRT : KeyboardPlatform
    {
        // mapping between WinRT keys and toolkit keys
        private static readonly IDictionary<VirtualKey, Keys> _keysDictionary;

        static KeyboardPlatformWinRT()
        {
            _keysDictionary = new Dictionary<VirtualKey, Keys>();
            // this dictionary was built from Desktop version.
            // some keys were removed (like OEM and Media buttons) as they don't have mapping in WinRT
            _keysDictionary[VirtualKey.None] = Keys.None;
            _keysDictionary[VirtualKey.Back] = Keys.Back;
            _keysDictionary[VirtualKey.Tab] = Keys.Tab;
            _keysDictionary[VirtualKey.Enter] = Keys.Enter;
            _keysDictionary[VirtualKey.Pause] = Keys.Pause;
            _keysDictionary[VirtualKey.CapitalLock] = Keys.CapsLock;
            _keysDictionary[VirtualKey.Kana] = Keys.Kana;
            _keysDictionary[VirtualKey.Kanji] = Keys.Kanji;
            _keysDictionary[VirtualKey.Escape] = Keys.Escape;
            _keysDictionary[VirtualKey.Convert] = Keys.ImeConvert;
            _keysDictionary[VirtualKey.NonConvert] = Keys.ImeNoConvert;
            _keysDictionary[VirtualKey.Space] = Keys.Space;
            _keysDictionary[VirtualKey.PageUp] = Keys.PageUp;
            _keysDictionary[VirtualKey.PageDown] = Keys.PageDown;
            _keysDictionary[VirtualKey.End] = Keys.End;
            _keysDictionary[VirtualKey.Home] = Keys.Home;
            _keysDictionary[VirtualKey.Left] = Keys.Left;
            _keysDictionary[VirtualKey.Up] = Keys.Up;
            _keysDictionary[VirtualKey.Right] = Keys.Right;
            _keysDictionary[VirtualKey.Down] = Keys.Down;
            _keysDictionary[VirtualKey.Select] = Keys.Select;
            _keysDictionary[VirtualKey.Print] = Keys.Print;
            _keysDictionary[VirtualKey.Execute] = Keys.Execute;
            _keysDictionary[VirtualKey.Print] = Keys.PrintScreen;
            _keysDictionary[VirtualKey.Insert] = Keys.Insert;
            _keysDictionary[VirtualKey.Delete] = Keys.Delete;
            _keysDictionary[VirtualKey.Help] = Keys.Help;
            _keysDictionary[VirtualKey.Number0] = Keys.D0;
            _keysDictionary[VirtualKey.Number1] = Keys.D1;
            _keysDictionary[VirtualKey.Number2] = Keys.D2;
            _keysDictionary[VirtualKey.Number3] = Keys.D3;
            _keysDictionary[VirtualKey.Number4] = Keys.D4;
            _keysDictionary[VirtualKey.Number5] = Keys.D5;
            _keysDictionary[VirtualKey.Number6] = Keys.D6;
            _keysDictionary[VirtualKey.Number7] = Keys.D7;
            _keysDictionary[VirtualKey.Number8] = Keys.D8;
            _keysDictionary[VirtualKey.Number9] = Keys.D9;
            _keysDictionary[VirtualKey.A] = Keys.A;
            _keysDictionary[VirtualKey.B] = Keys.B;
            _keysDictionary[VirtualKey.C] = Keys.C;
            _keysDictionary[VirtualKey.D] = Keys.D;
            _keysDictionary[VirtualKey.E] = Keys.E;
            _keysDictionary[VirtualKey.F] = Keys.F;
            _keysDictionary[VirtualKey.G] = Keys.G;
            _keysDictionary[VirtualKey.H] = Keys.H;
            _keysDictionary[VirtualKey.I] = Keys.I;
            _keysDictionary[VirtualKey.J] = Keys.J;
            _keysDictionary[VirtualKey.K] = Keys.K;
            _keysDictionary[VirtualKey.L] = Keys.L;
            _keysDictionary[VirtualKey.M] = Keys.M;
            _keysDictionary[VirtualKey.N] = Keys.N;
            _keysDictionary[VirtualKey.O] = Keys.O;
            _keysDictionary[VirtualKey.P] = Keys.P;
            _keysDictionary[VirtualKey.Q] = Keys.Q;
            _keysDictionary[VirtualKey.R] = Keys.R;
            _keysDictionary[VirtualKey.S] = Keys.S;
            _keysDictionary[VirtualKey.T] = Keys.T;
            _keysDictionary[VirtualKey.U] = Keys.U;
            _keysDictionary[VirtualKey.V] = Keys.V;
            _keysDictionary[VirtualKey.W] = Keys.W;
            _keysDictionary[VirtualKey.X] = Keys.X;
            _keysDictionary[VirtualKey.Y] = Keys.Y;
            _keysDictionary[VirtualKey.Z] = Keys.Z;
            _keysDictionary[VirtualKey.LeftWindows] = Keys.LeftWindows;
            _keysDictionary[VirtualKey.RightWindows] = Keys.RightWindows;
            _keysDictionary[VirtualKey.Application] = Keys.Apps;
            _keysDictionary[VirtualKey.Sleep] = Keys.Sleep;
            _keysDictionary[VirtualKey.NumberPad0] = Keys.NumPad0;
            _keysDictionary[VirtualKey.NumberPad1] = Keys.NumPad1;
            _keysDictionary[VirtualKey.NumberPad2] = Keys.NumPad2;
            _keysDictionary[VirtualKey.NumberPad3] = Keys.NumPad3;
            _keysDictionary[VirtualKey.NumberPad4] = Keys.NumPad4;
            _keysDictionary[VirtualKey.NumberPad5] = Keys.NumPad5;
            _keysDictionary[VirtualKey.NumberPad6] = Keys.NumPad6;
            _keysDictionary[VirtualKey.NumberPad7] = Keys.NumPad7;
            _keysDictionary[VirtualKey.NumberPad8] = Keys.NumPad8;
            _keysDictionary[VirtualKey.NumberPad9] = Keys.NumPad9;
            _keysDictionary[VirtualKey.Multiply] = Keys.Multiply;
            _keysDictionary[VirtualKey.Add] = Keys.Add;
            _keysDictionary[VirtualKey.Separator] = Keys.Separator;
            _keysDictionary[VirtualKey.Subtract] = Keys.Subtract;
            _keysDictionary[VirtualKey.Decimal] = Keys.Decimal;
            _keysDictionary[VirtualKey.Divide] = Keys.Divide;
            _keysDictionary[VirtualKey.F1] = Keys.F1;
            _keysDictionary[VirtualKey.F2] = Keys.F2;
            _keysDictionary[VirtualKey.F3] = Keys.F3;
            _keysDictionary[VirtualKey.F4] = Keys.F4;
            _keysDictionary[VirtualKey.F5] = Keys.F5;
            _keysDictionary[VirtualKey.F6] = Keys.F6;
            _keysDictionary[VirtualKey.F7] = Keys.F7;
            _keysDictionary[VirtualKey.F8] = Keys.F8;
            _keysDictionary[VirtualKey.F9] = Keys.F9;
            _keysDictionary[VirtualKey.F10] = Keys.F10;
            _keysDictionary[VirtualKey.F11] = Keys.F11;
            _keysDictionary[VirtualKey.F12] = Keys.F12;
            _keysDictionary[VirtualKey.F13] = Keys.F13;
            _keysDictionary[VirtualKey.F14] = Keys.F14;
            _keysDictionary[VirtualKey.F15] = Keys.F15;
            _keysDictionary[VirtualKey.F16] = Keys.F16;
            _keysDictionary[VirtualKey.F17] = Keys.F17;
            _keysDictionary[VirtualKey.F18] = Keys.F18;
            _keysDictionary[VirtualKey.F19] = Keys.F19;
            _keysDictionary[VirtualKey.F20] = Keys.F20;
            _keysDictionary[VirtualKey.F21] = Keys.F21;
            _keysDictionary[VirtualKey.F22] = Keys.F22;
            _keysDictionary[VirtualKey.F23] = Keys.F23;
            _keysDictionary[VirtualKey.F24] = Keys.F24;
            _keysDictionary[VirtualKey.NumberKeyLock] = Keys.NumLock;
            _keysDictionary[VirtualKey.Scroll] = Keys.Scroll;
            _keysDictionary[VirtualKey.LeftShift] = Keys.LeftShift;
            _keysDictionary[VirtualKey.RightShift] = Keys.RightShift;
            _keysDictionary[VirtualKey.LeftControl] = Keys.LeftControl;
            _keysDictionary[VirtualKey.RightControl] = Keys.RightControl;
            _keysDictionary[VirtualKey.LeftMenu] = Keys.LeftAlt;
            _keysDictionary[VirtualKey.RightMenu] = Keys.RightAlt;
            _keysDictionary[VirtualKey.Back] = Keys.BrowserBack;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MousePlatformWinRT"/> class.
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="CoreWindow"/> or <see cref="UIElement"/> class.</param>
        public KeyboardPlatformWinRT(object nativeWindow) : base(nativeWindow) { }

        /// <summary>
        /// Binds to specific events of the provided CoreWindow
        /// </summary>
        /// <param name="nativeWindow">A reference to <see cref="CoreWindow"/> or <see cref="UIElement"/> class.</param>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="nativeWindow"/> is null.</exception>
        /// <exception cref="ArgumentException">Is thrown when <paramref name="nativeWindow"/> is not a <see cref="CoreWindow"/> and not an <see cref="UIElement"/></exception>
        protected override void BindWindow(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            var window = nativeWindow as CoreWindow;
            if (window != null)
            {
                window.KeyDown += HandleWindowKeyDown;
                window.KeyUp += HandleWindowKeyUp;
                return;
            }

            var uiElement = nativeWindow as UIElement;
            if (uiElement != null)
            {
                uiElement.KeyDown += HandleUIElementKeyDown;
                uiElement.KeyUp += HandleUIElementKeyUp;
                return;
            }

            throw new ArgumentException("Should be an instance of either CoreWindow or UIElement", "nativeWindow");
        }

        /// <summary>
        /// Handles the <see cref="CoreWindow.KeyDown"/> event
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="args">Provides the key value from <see cref="KeyEventArgs.VirtualKey"/> property</param>
        private void HandleWindowKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            RaiseKeyPressed(TranslateKey(args.VirtualKey));
        }

        /// <summary>
        /// Handles the <see cref="CoreWindow.KeyUp"/> event
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="args">Provides the key value from <see cref="KeyEventArgs.VirtualKey"/> property</param>
        private void HandleWindowKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            RaiseKeyReleased(TranslateKey(args.VirtualKey));
        }

        /// <summary>
        /// Handles the <see cref="UIElement.KeyDown"/> event
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="args">Provides the key value from <see cref="KeyRoutedEventArgs.Key"/> property</param>
        private void HandleUIElementKeyDown(object sender, KeyRoutedEventArgs args)
        {
            RaiseKeyPressed(TranslateKey(args.Key));
        }

        /// <summary>
        /// Handles the <see cref="UIElement.KeyUp"/> event
        /// </summary>
        /// <param name="sender">Ignored</param>
        /// <param name="args">Provides the key value from <see cref="KeyRoutedEventArgs.Key"/> property</param>
        private void HandleUIElementKeyUp(object sender, KeyRoutedEventArgs args)
        {
            RaiseKeyReleased(TranslateKey(args.Key));
        }

        /// <summary>
        /// Translates the <see cref="System.Windows.Forms.Keys"/> value to <see cref="Keys"/>
        /// </summary>
        /// <param name="key">The WinForms key value</param>
        /// <returns>toolkit key value or <see cref="Keys.None"/> for unknown values</returns>
        private static Keys TranslateKey(VirtualKey key)
        {
            Keys translatedKey;
            // ignore unknown keys
            return _keysDictionary.TryGetValue(key, out translatedKey) ? translatedKey : Keys.None;
        }
    }
}

#endif