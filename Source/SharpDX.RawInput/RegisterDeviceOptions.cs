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

using System.Windows.Forms;

namespace SharpDX.RawInput
{
    /// <summary>
    /// Options used when using <see cref="Device.RegisterDevice(SharpDX.Multimedia.UsagePage,SharpDX.Multimedia.UsageId,SharpDX.RawInput.DeviceFlags)"/>
    /// </summary>
    public enum RegisterDeviceOptions
    {
        /// <summary>
        /// Default register using <see cref="Application.AddMessageFilter"/> for RawInput message filtering.
        /// </summary>
        Default = 0,

        /// <summary>
        /// To disable message filtering
        /// </summary>
        NoFiltering = 1,

        /// <summary>
        /// Use custom message filtering instead of <see cref="Application.AddMessageFilter"/>
        /// </summary>
        CustomFiltering = 2,
    }
}