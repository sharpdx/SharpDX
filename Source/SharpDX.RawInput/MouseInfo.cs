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

namespace SharpDX.RawInput
{
    /// <summary>
    /// Defines the raw input data coming from the specified mouse.
    /// </summary>
    /// <unmanaged>RID_DEVICE_INFO_MOUSE</unmanaged>	
    public class MouseInfo : DeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInfo"/> class.
        /// </summary>
        public MouseInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseInfo"/> class.
        /// </summary>
        /// <param name="rawDeviceInfo">The raw device info.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceHandle">The device handle.</param>
        internal MouseInfo(ref RawDeviceInformation rawDeviceInfo, string deviceName, IntPtr deviceHandle) : base(ref rawDeviceInfo, deviceName, deviceHandle)
        {
            Id = rawDeviceInfo.Mouse.Id;
            ButtonCount = rawDeviceInfo.Mouse.NumberOfButtons;
            SampleRate = rawDeviceInfo.Mouse.SampleRate;
            HasHorizontalWheel = rawDeviceInfo.Mouse.HasHorizontalWheel;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        /// <unmanaged>unsigned int dwId</unmanaged>	
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the button count.
        /// </summary>
        /// <value>
        /// The button count.
        /// </value>
        /// <unmanaged>unsigned int dwNumberOfButtons</unmanaged>	
        public int ButtonCount { get; set; }

        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        /// <value>
        /// The sample rate.
        /// </value>
        /// <unmanaged>unsigned int dwSampleRate</unmanaged>	
        public int SampleRate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has horizontal wheel.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has horizontal wheel; otherwise, <c>false</c>.
        /// </value>
        /// <unmanaged>BOOL fHasHorizontalWheel</unmanaged>	
        public bool HasHorizontalWheel { get; set; }
    }
}