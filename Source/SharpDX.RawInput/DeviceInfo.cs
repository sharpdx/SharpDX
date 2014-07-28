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
    /// Defines the raw input data coming from any device.
    /// </summary>
    /// <unmanaged>RID_DEVICE_INFO</unmanaged>	
    public partial class DeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo"/> class.
        /// </summary>
        public DeviceInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo"/> class.
        /// </summary>
        /// <param name="rawDeviceInfo">The raw device info.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceHandle">The device handle.</param>
        internal DeviceInfo(ref RawDeviceInformation rawDeviceInfo, string deviceName, IntPtr deviceHandle)
        {
            DeviceName = deviceName;
            Handle = deviceHandle;
            DeviceType = rawDeviceInfo.Type;
        }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>
        /// The type of the device.
        /// </value>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the handle.
        /// </summary>
        /// <value>
        /// The handle.
        /// </value>
        public System.IntPtr Handle { get; set; }

        /// <summary>
        /// Converts the specified raw device info to the <see cref="DeviceInfo"/>.
        /// </summary>
        /// <param name="rawDeviceInfo">The raw device info.</param>
        /// <param name="deviceName">Name of the device.</param>
        /// <param name="deviceHandle">The device handle.</param>
        /// <returns></returns>
        internal static DeviceInfo Convert(ref RawDeviceInformation rawDeviceInfo, string deviceName, IntPtr deviceHandle)
        {
            DeviceInfo deviceInfo = null;
            switch (rawDeviceInfo.Type)
            {
                case DeviceType.HumanInputDevice:
                    deviceInfo = new HidInfo(ref rawDeviceInfo, deviceName, deviceHandle);
                    break;
                case DeviceType.Keyboard:
                    deviceInfo = new KeyboardInfo(ref rawDeviceInfo, deviceName, deviceHandle);
                    break;
                case DeviceType.Mouse:
                    deviceInfo = new MouseInfo(ref rawDeviceInfo, deviceName, deviceHandle);
                    break;
                default:
                    throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Unsupported Device Type [{0}]", (int)rawDeviceInfo.Type));
            }
            return deviceInfo;
        }
    }
}

