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

using System;
using SharpDX;

namespace CustomLayout
{
    /// <summary>
    /// Tells a flow layout where text is allowed to flow.
    /// </summary>
    internal class FlowLayoutSource
    {
        public FlowShape FlowShape;
        public float Width, Height;

        private float currentY;

        public FlowLayoutSource()
        {
            FlowShape = FlowShape.Circle;
            Width = 300;
            Height = 300;
            Reset();
        }

        public void Reset()
        {
            currentY = 0;
        }
        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public bool GetNextRect(float fontHeight, out RectangleF rect)
        {
            // Set defaults.
            rect = new RectangleF(0, 0, Width, Height);

            if (Width < 0 || Height < 0)
                return false;

            float halfHeight = Height / 2;
            float halfWidth = Width / 2;

            // Simple, hard-coded shape formulas.
            // You can add more shapes by adding a new enum in the header and extending
            // the switch statement.

            switch (FlowShape)
            {
                case FlowShape.Funnel:
                    float xShift = (float)(Math.Sin(currentY / Height * Math.PI * 3)) * 30;
                    // Calculate slope to determine edges.
                    rect.Top = currentY;
                    rect.Bottom = currentY + fontHeight;
                    rect.Left = xShift + (currentY / Height) * Width / 2;
                    rect.Right = Width - rect.Left;
                    break;
                case FlowShape.Circle:
                    float adjustedY = (currentY + fontHeight / 2) - halfHeight;
                    // Determine x from y using circle formula d^2 = (x^2 + y^2).
                    float x = (float)Math.Sqrt((halfHeight * halfHeight) - (adjustedY * adjustedY));
                    rect.Top = currentY;
                    rect.Bottom = currentY + fontHeight;
                    rect.Left = halfWidth - x;
                    rect.Right = halfWidth + x;
                    break;
            }

            // Advance down one line, based on font height.
            currentY += fontHeight;
            if (currentY >= Height)
            {
                // Crop any further lines.
                rect.Left = rect.Right = 0;
            }
            return true;
        }
    }

    public enum FlowShape { Circle, Funnel };


}
