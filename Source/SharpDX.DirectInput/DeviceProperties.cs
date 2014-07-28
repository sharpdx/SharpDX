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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Properties for a <see cref="Device"/>.
    /// </summary>
    public partial class DeviceProperties : PropertyAccessor
    {
        internal DeviceProperties(Device device) : base(device, 0, PropertyHowType.Device)
        {
        }

        /// <summary>
        /// Gets the key code for a keyboard key. An exception is raised if the property cannot resolve specialized keys on USB keyboards because they do not exist in scan code form. For all other failures, an exception is also returned.
        /// </summary>
        /// <param name="key">The key id.</param>
        /// <returns>The key code</returns>
        public int GetKeyCode(Key key)
        {
            // TODO check BYid
            return GetInt(PropertyGuids.Scancode, (int) key);
        }

        /// <summary>
        /// Gets the localized key name for a keyboard key. Using this property on devices other than a keyboard will return unexpected names. 
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetKeyName(Key key)
        {
            // TODO check BYid
            return GetString(PropertyGuids.Keyname, (int)key);
        }

        // ApplicationData are not working, seems to be a bug in DirectInput
        ///// <summary>
        ///// Gets or sets the application-defined value associated with an in-game action.
        ///// </summary>
        ///// <value>The application data.</value>
        //public object ApplicationData
        //{
        //    get { return GetObject(PropertyGuids.Appdata); }

        //    set { SetObject(PropertyGuids.Appdata, value); }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether device objects are self centering.
        /// </summary>
        /// <value><c>true</c> if device objects are self centering; otherwise, <c>false</c>.</value>
        public bool AutoCenter
        {
            get { return GetInt(PropertyGuids.Autocenter) != 0; }

            set { Set(PropertyGuids.Autocenter, value ? 1 : 0); }
        }

        /// <summary>
        /// Gets or sets the axis mode.
        /// </summary>
        /// <value>The axis mode.</value>
        public DeviceAxisMode AxisMode
        {
            get { return (DeviceAxisMode) GetInt(PropertyGuids.Axismode); }

            set { Set(PropertyGuids.Axismode, (int) value); }
        }

        /// <summary>
        /// Gets or sets the input buffer size. The buffer size determines the amount of data that the buffer can hold between calls to the <see cref="Device.GetDeviceData"/> method before data is lost. You can set this value to 0 to indicate that the application does not read buffered data from the device. If the buffer size is too large for the device to support it, then the largest possible buffer size is set. However, this property always returns the buffer size set using the <see cref="BufferSize"/> property, even if the buffer cannot be supported because it is too large.
        /// </summary>
        /// <value>The size of the buffer.</value>
        public int BufferSize
        {
            get { return GetInt(PropertyGuids.BufferSize); }

            set { Set(PropertyGuids.BufferSize, value); }
        }

        /// <summary>
        /// Gets the class GUID for the device. This property lets advanced applications perform operations on a human interface device that are not supported by DirectInput. 
        /// </summary>
        /// <value>The class GUID.</value>
        public Guid ClassGuid
        {
            get { return GetGuid(PropertyGuids.Guidandpath); }
        }

        /// <summary>
        /// Gets or sets the dead zone of a joystick, in the range from 0 through 10,000, where 0 indicates that there is no dead zone, 5,000 indicates that the dead zone extends over 50 percent of the physical range of the axis on both sides of center, and 10,000 indicates that the entire physical range of the axis is dead. When the axis is within the dead zone, it is reported as being at the center of its range.
        /// </summary>
        /// <value>The dead zone.</value>
        public int DeadZone
        {
            get { return GetInt(PropertyGuids.Deadzone); }

            set { Set(PropertyGuids.Deadzone, value); }
        }

        /// <summary>
        /// Gets or sets the force feedback gain of the device. 
        /// The gain value is applied to all effects created on the device. The value is an integer in the range from 0 through 10,000, specifying the amount by which effect magnitudes should be scaled for the device. For example, a value of 10,000 indicates that all effect magnitudes are to be taken at face value. A value of 9,000 indicates that all effect magnitudes are to be reduced to 90 percent of their nominal magnitudes.
        /// DirectInput always checks the gain value before setting the gain property. If the gain is outside of the range (less than zero or greater than 10,000), setting this property will raise an exception. Otherwise, no exception if successful, even if the device does not support force feedback.
        /// Setting a gain value is useful when an application wants to scale down the strength of all force-feedback effects uniformly, based on user preferences.
        /// Unlike other properties, the gain can be set when the device is in an acquired state. 
        /// </summary>
        /// <value>The force feedback gain.</value>
        public int ForceFeedbackGain
        {
            get { return GetInt(PropertyGuids.Ffgain); }

            set { Set(PropertyGuids.Ffgain, value); }
        }

        /// <summary>
        /// Gets the input granularity. Granularity represents the smallest distance over which the object reports movement. Most axis objects have a granularity of one; that is, all values are possible. Some axes have a larger granularity. For example, the wheel axis on a mouse can have a granularity of 20; that is, all reported changes in position are multiples of 20. In other words, when the user turns the wheel slowly, the device reports a position of 0, then 20, then 40, and so on. This is a read-only property.
        /// </summary>
        /// <value>The granularity.</value>
        public int Granularity
        {
            get { return GetInt(PropertyGuids.Granularity); }
        }

        /// <summary>
        /// Gets or sets the friendly instance name of the device. 
        /// This property exists for advanced applications that want to change the friendly instance name of a device (as returned in the tszInstanceName member of the <see cref="DeviceInstance"/> structure) to distinguish it from similar devices that are plugged in simultaneously. Most applications should have no need to change the friendly name.
        /// </summary>
        /// <value>The name of the instance.</value>
        public string InstanceName
        {
            get { return GetString(PropertyGuids.InstanceName); }
            set { Set(PropertyGuids.InstanceName, value); }
        }

        /// <summary>
        /// Gets the device interface path for the device. This property lets advanced applications perform operations on a human interface device that are not supported by DirectInput. 
        /// </summary>
        /// <value>The interface path.</value>
        public string InterfacePath
        {
            get { return GetPath(PropertyGuids.Guidandpath); }
        }

        /// <summary>
        /// Gets the instance number of a joystick. This property is not implemented for the mouse or keyboard.
        /// </summary>
        /// <value>The joystick id.</value>
        public int JoystickId
        {
            get { return GetInt(PropertyGuids.Joystickid); }
        }

        /// <summary>
        /// Gets the memory load for the device. This setting applies to the entire device, rather than to any particular object. The retrieved value is in the range from 0 through 100, indicating the percentage of device memory in use. The device must be acquired in exclusive mode. If it is not, the method will fail with an exception.
        /// </summary>
        /// <value>The memory load.</value>
        public int MemoryLoad
        {
            get { return GetInt(PropertyGuids.Ffload); }
        }

        /// <summary>
        /// Gets the human-readable display name of the port to which this device is connected. Not generally used by applications. 
        /// </summary>
        /// <value>The  human-readable display name of the port to which this device is connected.</value>
        public string PortDisplayName
        {
            get { return GetPath(PropertyGuids.GetPortdisplayname); }
        }

        /// <summary>
        /// Gets the vendor identity (ID) and product ID of a HID device. This property is of type int and contains both values. These two short values are combined. This property applies to the entire device, rather than to any particular object.
        /// </summary>
        /// <value>The product id.</value>
        public int ProductId
        {
            get { return (GetInt(PropertyGuids.Vidpid) >> 16) & 0xFFFF; }
        }

        /// <summary>
        /// Gets or sets the friendly product name of the device.
        /// This property exists for advanced applications that want to change the friendly product name of a device (as returned in the tszProductName member of the <see cref="DeviceInstance"/> structure) to distinguish it from similar devices which are plugged in simultaneously. Most applications should have no need to change the friendly name.
        /// This setting applies to the entire device.
        /// Setting the product name is only useful for changing the user-defined name of an analog joystick on Microsoft Windows 98, Windows 2000, and Windows Millennium Edition (Windows Me) computers. In other cases, attempting to set this property will still return ok. However, the name is not stored in a location used by the getter of this property.
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName
        {
            get { return GetString(PropertyGuids.Productname); }
            set { Set(PropertyGuids.Productname, value); }
        }

        /// <summary>
        /// Gets or sets the range of values an object can possibly report.
        /// </summary>
        /// <value>The range.</value>
        /// <remarks>For some devices, this is a read-only property.</remarks>
        public InputRange Range
        {
            get { return GetRange(PropertyGuids.Range); }
            set { Set(PropertyGuids.Range, value);}
        }

        /// <summary>
        /// Gets or sets the saturation zones of a joystick, in the range from 0 through 10,000. The saturation level is the point at which the axis is considered to be at its most extreme position. For example, if the saturation level is set to 9,500, the axis reaches the extreme of its range when it has moved 95 percent of the physical distance from its center position (or from the dead zone).
        /// </summary>
        /// <value>The saturation.</value>
        public int Saturation
        {
            get { return GetInt(PropertyGuids.Saturation); }

            set { Set(PropertyGuids.Saturation, value); }
        }

        /// <summary>
        /// Gets the type name of a device. For most game controllers, this is the registry key name under REGSTR_PATH_JOYOEM from which static device settings can be retrieved, but predefined joystick types have special names consisting of a number sign (&Sharp;) followed by a character dependent on the type. This value might not be available for all devices.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName
        {
            get { return GetPath(PropertyGuids.Typename); }
        }

        /// <summary>
        /// Gets the user name for a user currently assigned to a device. User names are set by calling <see cref="Device.SetActionMap"/>. If no user name is set, the method throws an exception.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return GetPath(PropertyGuids.Username); }
        }

        /// <summary>
        /// Gets the vendor identity (ID) and product ID of a HID device. This property is of type int and contains both values. These two short values are combined. This property applies to the entire device, rather than to any particular object.
        /// </summary>
        /// <value>The product id.</value>
        public int VendorId
        {
            get { return GetInt(PropertyGuids.Vidpid) & 0xFFFF; }
        }
    }
}