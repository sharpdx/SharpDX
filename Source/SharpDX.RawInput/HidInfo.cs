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
    /// Defines the raw input data coming from the specified Human Interface Device (HID). 
    /// </summary>
    public class HidInfo : DeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HidInfo"/> class.
        /// </summary>
        public HidInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HidInfo"/> class.
        /// </summary>
        /// <param name="rawDeviceInfo">The raw device info.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceHandle">The device handle.</param>
        internal HidInfo(ref RawDeviceInformation rawDeviceInfo, string deviceName, IntPtr deviceHandle) : base(ref rawDeviceInfo, deviceName, deviceHandle)
        {
            VendorId = rawDeviceInfo.Hid.VendorId;
            ProductId = rawDeviceInfo.Hid.ProductId;
            VersionNumber = rawDeviceInfo.Hid.VersionNumber;
            UsagePage = rawDeviceInfo.Hid.UsagePage;
            Usage = rawDeviceInfo.Hid.Usage;
        }

        /// <summary>
        /// Gets or sets the vendor id.
        /// </summary>
        /// <value>
        /// The vendor id.
        /// </value>
        /// <unmanaged>unsigned int dwVendorId</unmanaged>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        /// <value>
        /// The product id.
        /// </value>
        /// <unmanaged>unsigned int dwProductId</unmanaged>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        /// <value>
        /// The version number.
        /// </value>
        /// <unmanaged>unsigned int dwVersionNumber</unmanaged>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the usage page.
        /// </summary>
        /// <value>
        /// The usage page.
        /// </value>
        /// <unmanaged>HID_USAGE_PAGE usUsagePage</unmanaged>
        public SharpDX.Multimedia.UsagePage UsagePage { get; set; }

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        /// <unmanaged>HID_USAGE_ID usUsage</unmanaged>
        public SharpDX.Multimedia.UsageId Usage { get; set; }
    }
}