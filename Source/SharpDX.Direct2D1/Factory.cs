// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.Direct2D1
{
    public partial class Factory
    {

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory" />.
        /// </summary>
        public Factory()
            : this(FactoryType.SingleThreaded)
        {
        }

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory" />.
        /// </summary>
        public Factory(FactoryType factoryType)
            : this(factoryType, DebugLevel.None)
        {
        }

        /// <summary>
        /// Default Constructor for a <see cref = "SharpDX.Direct2D1.Factory" />.
        /// </summary>
        public Factory(FactoryType factoryType, DebugLevel debugLevel)
            : base(IntPtr.Zero)
        {
            FactoryOptions? options = null;

            if (debugLevel != DebugLevel.None)
                options = new FactoryOptions() { DebugLevel = debugLevel };

            IntPtr temp;
            D2D1.CreateFactory(factoryType, Utilities.GetGuidFromType(GetType()), options, out temp);
            FromTemp(temp);
        }

        /// <summary>	
        /// Retrieves the current desktop dots per inch (DPI). To refresh this value, call {{ReloadSystemMetrics}}.	
        /// </summary>	
        /// <remarks>	
        /// Use this method to obtain the system DPI when setting physical pixel values, such as when you specify the size of a window. 	
        /// </remarks>	
        public Size2F DesktopDpi
        {
            get
            {
                float y;
                float x;
                GetDesktopDpi(out x, out y);
                return new Size2F(x, y);
            }
        }
    }
}
