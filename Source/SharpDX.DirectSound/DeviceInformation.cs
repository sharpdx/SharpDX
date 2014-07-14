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

namespace SharpDX.DirectSound
{
    /// <summary>
    /// Contains information about a DirectSound device.
    /// </summary>
    public class DeviceInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInformation"/> class.
        /// </summary>
        /// <param name="driverGuid">The driver GUID.</param>
        /// <param name="description">The description.</param>
        /// <param name="moduleName">Name of the module.</param>
        public DeviceInformation(Guid driverGuid, string description, string moduleName)
        {
            DriverGuid = driverGuid;
            Description = description;
            ModuleName = moduleName;
        }

        /// <summary>
        /// Identifies the DirectSound driver being enumerated
        /// </summary>
        public Guid DriverGuid { get; set; }
        
        /// <summary>
        /// String that provides a textual description of the DirectSound device.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// String that specifies the module name of the DirectSound driver corresponding to this device.
        /// </summary>
        public string ModuleName { get; set; }
    }
}