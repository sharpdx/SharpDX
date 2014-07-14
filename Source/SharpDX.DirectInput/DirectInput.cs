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
using System.Collections.Generic;

namespace SharpDX.DirectInput
{
    public partial class DirectInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectInput"/> class.
        /// </summary>
        public DirectInput() : base(IntPtr.Zero)
        {
            IntPtr temp;
            DInput.DirectInput8Create(Win32Native.GetModuleHandle(null), DInput.SdkVersion, Utilities.GetGuidFromType(typeof(DirectInput)), out temp, null);
            NativePointer = temp;
        }

        /// <summary>
        /// Gets all devices.
        /// </summary>
        /// <returns>A collection of <see cref="DeviceInstance"/></returns>
        public IList<DeviceInstance> GetDevices()
        {
            return GetDevices(DeviceClass.All, DeviceEnumerationFlags.AllDevices);
        }

        /// <summary>
        /// Gets the devices for a particular <see cref="DeviceClass"/> and <see cref="DeviceEnumerationFlags"/>.
        /// </summary>
        /// <param name="deviceClass">Class of the device.</param>
        /// <param name="deviceEnumFlags">The device enum flags.</param>
        /// <returns>A collection of <see cref="DeviceInstance"/></returns>
        public IList<DeviceInstance> GetDevices(DeviceClass deviceClass, DeviceEnumerationFlags deviceEnumFlags)
        {
            var enumDevicesCallback = new EnumDevicesCallback();
            EnumDevices((int)deviceClass, enumDevicesCallback.NativePointer, IntPtr.Zero, deviceEnumFlags);
            return enumDevicesCallback.DeviceInstances;
        }

        /// <summary>
        /// Gets the devices for a particular <see cref="DeviceType"/> and <see cref="DeviceEnumerationFlags"/>.
        /// </summary>
        /// <param name="deviceType">Type of the device.</param>
        /// <param name="deviceEnumFlags">The device enum flags.</param>
        /// <returns>A collection of <see cref="DeviceInstance"/></returns>
        public IList<DeviceInstance> GetDevices(DeviceType deviceType, DeviceEnumerationFlags deviceEnumFlags)
        {
            var enumDevicesCallback = new EnumDevicesCallback();
            EnumDevices((int)deviceType, enumDevicesCallback.NativePointer, IntPtr.Zero, deviceEnumFlags);
            return enumDevicesCallback.DeviceInstances;
        }

        /// <summary>
        /// Determines whether a device with the specified Guid is attached.
        /// </summary>
        /// <param name="deviceGuid">The device Guid.</param>
        /// <returns>
        /// 	<c>true</c> if the device with the specified device Guid is attached ; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDeviceAttached(Guid deviceGuid)
        {
            return GetDeviceStatus(deviceGuid).Code == 0;
        }


        /// <summary>
        /// Runs Control Panel to enable the user to install a new input device or modify configurations.
        /// </summary>
        public void RunControlPanel()
        {
            RunControlPanel(IntPtr.Zero);
        }

        /// <summary>
        /// Runs Control Panel to enable the user to install a new input device or modify configurations.
        /// </summary>
        /// <param name="handle">Handle of the window to be used as the parent window for the subsequent user interface. If this parameter is NULL, no parent window is used.</param>
        public void RunControlPanel(IntPtr handle)
        {
            RunControlPanel(handle, 0);            
        }
    }
}