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
using System.Collections.ObjectModel;
using System.Windows.Forms;
using SharpDX.Multimedia;
using SharpDX.Win32;

namespace SharpDX.RawInput
{
    /// <summary>
    /// Provides access to RawInput methods.
    /// </summary>
    public partial class Device
    {
        private static RawInputMessageFilter rawInputMessageFilter;

        /// <summary>
        /// Occurs when [keyboard input].
        /// </summary>
        public static event EventHandler<KeyboardInputEventArgs> KeyboardInput;

        /// <summary>
        /// Occurs when [mouse input].
        /// </summary>
        public static event EventHandler<MouseInputEventArgs> MouseInput;

        /// <summary>
        /// Occurs when [raw input].
        /// </summary>
        public static event EventHandler<RawInputEventArgs> RawInput;

        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <returns></returns>
        public static unsafe List<DeviceInfo> GetDevices()
        {
            // Get the number of input device
            int deviceCount = 0;
            RawInputFunctions.GetRawInputDeviceList(null, ref deviceCount, Utilities.SizeOf<RawInputDevicelist>());
            if (deviceCount == 0)
                return null;

            // Get the raw input device list
            var rawInputDeviceList = new RawInputDevicelist[deviceCount];
            RawInputFunctions.GetRawInputDeviceList(rawInputDeviceList, ref deviceCount, Utilities.SizeOf<RawInputDevicelist>());

            var deviceInfoList = new List<DeviceInfo>();
            // Iterate on all input device
            for (int i = 0; i < deviceCount; i++)
            {
                var deviceHandle = rawInputDeviceList[i].Device;

                // Get the DeviceName
                int countDeviceNameChars = 0;
                RawInputFunctions.GetRawInputDeviceInfo(deviceHandle, RawInputDeviceInfoType.DeviceName, IntPtr.Zero, ref countDeviceNameChars);
                char* deviceNamePtr = stackalloc char[countDeviceNameChars];
                RawInputFunctions.GetRawInputDeviceInfo(deviceHandle, RawInputDeviceInfoType.DeviceName, (IntPtr) deviceNamePtr, ref countDeviceNameChars);
                var deviceName = new string(deviceNamePtr, 0, countDeviceNameChars);

                // Get the DeviceInfo
                int sizeOfDeviceInfo = 0;
                RawInputFunctions.GetRawInputDeviceInfo(deviceHandle, RawInputDeviceInfoType.DeviceInfo, IntPtr.Zero, ref sizeOfDeviceInfo);
                byte* deviceInfoPtr = stackalloc byte[sizeOfDeviceInfo];
                RawInputFunctions.GetRawInputDeviceInfo(deviceHandle, RawInputDeviceInfoType.DeviceInfo, (IntPtr) deviceInfoPtr, ref sizeOfDeviceInfo);

                deviceInfoList.Add(DeviceInfo.Convert(ref *(RawDeviceInformation*) deviceInfoPtr, deviceName, deviceHandle));
            }

            return deviceInfoList;
        }

        /// <summary>
        /// Registers the devices that supply the raw input data.
        /// </summary>
        /// <param name="usagePage">The usage page.</param>
        /// <param name="usageId">The usage id.</param>
        /// <param name="flags">The flags.</param>
        public static void RegisterDevice(UsagePage usagePage, UsageId usageId, DeviceFlags flags)
        {
            RegisterDevice(usagePage, usageId, flags, IntPtr.Zero);
        }

        /// <summary>
        /// Registers the devices that supply the raw input data.
        /// </summary>
        /// <param name="usagePage">The usage page.</param>
        /// <param name="usageId">The usage id.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="target">The target.</param>
        /// <param name="addMessageFilter">if set to <c>true</c> register message filter to Application.AddMessageFilter.</param>
        public static void RegisterDevice(UsagePage usagePage, UsageId usageId, DeviceFlags flags, IntPtr target, bool addMessageFilter = true)
        {
            var rawInputDevices = new RawInputDevice[1];
            rawInputDevices[0].UsagePage = (short) usagePage;
            rawInputDevices[0].Usage = (short) usageId;
            rawInputDevices[0].Flags = (int) flags;
            rawInputDevices[0].Target = target;

            // Register this device
            RawInputFunctions.RegisterRawInputDevices(rawInputDevices, 1, Utilities.SizeOf<RawInputDevice>());

            if (rawInputMessageFilter == null && addMessageFilter)
            {
                rawInputMessageFilter = new RawInputMessageFilter();
                MessageFilterHook.AddMessageFilter(target, rawInputMessageFilter);
            }
        }

        /// <summary>
        /// Handles a RawInput message manually.
        /// </summary>
        /// <param name="rawInputMessagePointer">A pointer to a RawInput message.</param>
        /// <remarks>
        /// This method can be used directly when handling RawInput messages from non-WinForms application.
        /// </remarks>
        public static void HandleMessage(IntPtr rawInputMessagePointer)
        {
            unsafe
            {
                // Get the size of the RawInput structure
                int sizeOfRawInputData = 0;
                RawInputFunctions.GetRawInputData(rawInputMessagePointer, RawInputDataType.Input, IntPtr.Zero, ref sizeOfRawInputData, Utilities.SizeOf<RawInputHeader>());

                if (sizeOfRawInputData == 0)
                    return;

                // Get the RawInput data structure
                var rawInputDataPtr = stackalloc byte[sizeOfRawInputData];
                RawInputFunctions.GetRawInputData(rawInputMessagePointer, RawInputDataType.Input, (IntPtr)rawInputDataPtr, ref sizeOfRawInputData, Utilities.SizeOf<RawInputHeader>());

                var rawInput = (RawInput*)rawInputDataPtr;

                switch (rawInput->Header.Type)
                {
                    case DeviceType.HumanInputDevice:
                        if (RawInput != null)
                            RawInput(null, new HidInputEventArgs(ref *rawInput));
                        break;
                    case DeviceType.Keyboard:
                        if (KeyboardInput != null)
                            KeyboardInput(null, new KeyboardInputEventArgs(ref *rawInput));
                        break;
                    case DeviceType.Mouse:
                        if (MouseInput != null)
                            MouseInput(null, new MouseInputEventArgs(ref *rawInput));
                        break;
                }
            }
        }

        /// <summary>
        /// Internal RawInput message filtering
        /// </summary>
        internal class RawInputMessageFilter : IMessageFilter
        {
            /// <summary>
            /// WM_INPUT
            /// </summary>
            private const int WmInput = 0x00FF;

            public virtual bool PreFilterMessage(ref Message m)
            {
                // Handle only WM_INPUT messages
                if (m.Msg == WmInput)
                    HandleMessage(m.LParam);
                return false;
            }
        }
    }
}

