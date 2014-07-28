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
    public partial class ObjectGuid
    {
        /// <summary>Constant XAxis.</summary>
        /// <unmanaged>GUID_XAxis</unmanaged>
        public static readonly System.Guid XAxis = new Guid(XAxisStr);

        /// <summary>Constant YAxis.</summary>
        /// <unmanaged>GUID_YAxis</unmanaged>
        public static readonly System.Guid YAxis = new Guid(YAxisStr);

        /// <summary>Constant ZAxis.</summary>
        /// <unmanaged>GUID_ZAxis</unmanaged>
        public static readonly System.Guid ZAxis = new Guid(ZAxisStr);

        /// <summary>Constant RxAxis.</summary>
        /// <unmanaged>GUID_RxAxis</unmanaged>
        public static readonly System.Guid RxAxis = new Guid(RxAxisStr);

        /// <summary>Constant RyAxis.</summary>
        /// <unmanaged>GUID_RyAxis</unmanaged>
        public static readonly System.Guid RyAxis = new Guid(RyAxisStr);

        /// <summary>Constant RzAxis.</summary>
        /// <unmanaged>GUID_RzAxis</unmanaged>
        public static readonly System.Guid RzAxis = new Guid(RzAxisStr);

        /// <summary>Constant Button.</summary>
        /// <unmanaged>GUID_Button</unmanaged>
        public static readonly System.Guid Button = new Guid(ButtonStr);

        /// <summary>Constant Key.</summary>
        /// <unmanaged>GUID_Key</unmanaged>
        public static readonly System.Guid Key = new Guid(KeyStr);

        /// <summary>Constant Slider.</summary>
        /// <unmanaged>GUID_Slider</unmanaged>
        public static readonly System.Guid Slider = new Guid(SliderStr);

        /// <summary>Constant PovController.</summary>
        /// <unmanaged>GUID_POV</unmanaged>
        public static readonly System.Guid PovController = new Guid(PovControllerStr);

        /// <summary>Constant Unknown.</summary>
        /// <unmanaged>GUID_Unknown</unmanaged>
        public static readonly System.Guid Unknown = new Guid(UnknownStr);
    }
}