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
    /// Properties associated to an Object.
    /// </summary>
    public class ObjectProperties : PropertyAccessor
    {
        internal ObjectProperties(Device device, int code, PropertyHowType howType) : base(device, code, howType)
        {
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
        /// Gets or sets the dead zone of a joystick, in the range from 0 through 10,000, where 0 indicates that there is no dead zone, 5,000 indicates that the dead zone extends over 50 percent of the physical range of the axis on both sides of center, and 10,000 indicates that the entire physical range of the axis is dead. When the axis is within the dead zone, it is reported as being at the center of its range.
        /// </summary>
        /// <value>The dead zone.</value>
        public int DeadZone
        {
            get { return GetInt(PropertyGuids.Deadzone); }

            set { Set(PropertyGuids.Deadzone, value); }
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
        /// Gets the range of the raw data returned for axes on a human interface device. Devices can return negative values.
        /// </summary>
        /// <value>The logical range.</value>
        public InputRange LogicalRange
        {
            get { return GetRange(PropertyGuids.LogicalRange); }
        }

        /// <summary>
        /// Gets Retrieves the range of data for axes as suggested by the manufacturer of a human interface device. Values can be negative. Normally DirectInput returns values from 0 through 0xFFFF, but the range can be made to conform to the manufacturer's suggested range by using <see cref="Range"/>.
        /// </summary>
        /// <value>The physical range.</value>
        public InputRange PhysicalRange
        {
            get { return GetRange(PropertyGuids.Physicalrange); }
        }

        /// <summary>
        /// Gets the range of values an object can possibly report. For some devices, this is a read-only property.
        /// </summary>
        /// <value>The range.</value>
        public InputRange Range
        {
            get { return GetRange(PropertyGuids.Range); }

            set { Set(PropertyGuids.Range, value); }
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
    }
}