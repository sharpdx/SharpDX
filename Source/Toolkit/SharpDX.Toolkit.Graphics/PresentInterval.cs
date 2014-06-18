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
namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Defines flags that describe the relationship between the adapter refresh rate and the rate at which Present operations are completed. 
    /// </summary>
    public enum PresentInterval
    {
        /// <summary>
        /// The runtime updates the window client area immediately, and might do so more than once during the adapter refresh period. Present operations might be affected immediately. This option is always available for both windowed and full-screen swap chains.
        /// </summary>
        Immediate = 0,

        /// <summary>
        /// The driver waits for the vertical retrace period (the runtime will beam trace to prevent tearing). Present operations are not affected more frequently than the screen refresh rate; the runtime completes one Present operation per adapter refresh period, at most. This option is always available for both windowed and full-screen swap chains.
        /// </summary>
        One = 1,

        /// <summary>
        /// The driver waits for the vertical retrace period. Present operations are not affected more frequently than every second screen refresh. 
        /// </summary>
        Two = 2,

        /// <summary>
        /// Equivalent to setting <see cref="One"/>.
        /// </summary>
        Default = One,
    }
}