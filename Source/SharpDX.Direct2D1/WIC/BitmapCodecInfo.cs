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

namespace SharpDX.WIC
{
    public partial class BitmapCodecInfo
    {
        /// <summary>
        /// Gets the pixel formats the codec supports.
        /// </summary>
        public Guid[] PixelFormats
        {
            get
            {
                int count = 0;
                GetPixelFormats(0, null, out count);
                if (count == 0)
                    return new Guid[0];

                var pixelFormats = new Guid[count];
                GetPixelFormats(count, pixelFormats, out count);

                return pixelFormats;
            }
        }

        /// <summary>
        /// Gets the color management version number the codec supports.
        /// </summary>
        public string ColorManagementVersion
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetColorManagementVersion(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetColorManagementVersion(count, (IntPtr)temp, out count);

                    return new string(temp, 0, count);
                }

            }
        }

        /// <summary>
        /// Gets the name of the device manufacture associated with the codec.
        /// </summary>
        public string DeviceManufacturer
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetDeviceManufacturer(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetDeviceManufacturer(count, (IntPtr)temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets a comma delimited list of device models associated with the codec.
        /// </summary>
        public string DeviceModels
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetDeviceModels(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetDeviceModels(count, (IntPtr)temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets a comma delimited sequence of mime types associated with the codec.
        /// </summary>
        public string MimeTypes
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetMimeTypes(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetMimeTypes(count, (IntPtr)temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }

        /// <summary>
        /// Gets a comma delimited list of the file name extensions associated with the codec.
        /// </summary>
        public string FileExtensions
        {
            get
            {
                unsafe
                {
                    int count = 0;
                    GetFileExtensions(0, IntPtr.Zero, out count);
                    if (count == 0)
                        return null;

                    var temp = stackalloc char[count];
                    GetFileExtensions(count, (IntPtr)temp, out count);

                    return new string(temp, 0, count);
                }
            }
        }
    }
}