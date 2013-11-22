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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace CommonDX
{
    /// <summary>
    /// A <see cref="UIElement"/> handler to add drag behavior.
    /// </summary>
    public class DragHandler
    {
        private bool moveD3dCanvas = false;
        private PointerPoint lastPos;
        private CoreCursor lastCursor;
        private UIElement uiElement;

        /// <summary>
        /// Cursor to show when mouse is over the element
        /// </summary>
        public CoreCursor CursorOver { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DragHandler"/>
        /// </summary>
        /// <param name="uiElement">The <see cref="UIElement"/> to bind this drag handler</param>
        public DragHandler(UIElement uiElement)
        {
            this.uiElement = uiElement;

            uiElement.PointerEntered += uiElement_PointerEntered;
            uiElement.PointerExited += uiElement_PointerExited;
            uiElement.PointerPressed += uiElement_PointerPressed;
            uiElement.PointerReleased += uiElement_PointerReleased;
            uiElement.PointerMoved += uiElement_PointerMoved;

        }

        protected virtual void uiElement_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            moveD3dCanvas = true;
            lastPos = e.GetCurrentPoint(null);
        }

        protected virtual void uiElement_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            moveD3dCanvas = false;
        }

        protected virtual void uiElement_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (moveD3dCanvas)
            {
                var newPosition = e.GetCurrentPoint(null);
                double deltaX = newPosition.Position.X - lastPos.Position.X;
                double deltaY = newPosition.Position.Y - lastPos.Position.Y;

                // Only support CompositeTransform and TranslateTransform
                // Is there any better way to handle this?
                if (uiElement.RenderTransform is CompositeTransform)
                {
                    var compositeTransform = (CompositeTransform)uiElement.RenderTransform;
                    compositeTransform.TranslateX += deltaX;
                    compositeTransform.TranslateY += deltaY;
                }
                else if (uiElement.RenderTransform is TranslateTransform)
                {
                    var translateTransform = (TranslateTransform)uiElement.RenderTransform;
                    translateTransform.X += deltaX;
                    translateTransform.Y += deltaY;
                }

                lastPos = newPosition;
            }
        }

        protected virtual void uiElement_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            lastCursor = Window.Current.CoreWindow.PointerCursor;
            Window.Current.CoreWindow.PointerCursor = CursorOver;
        }

        protected virtual void uiElement_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = lastCursor;
        }
    }
}
