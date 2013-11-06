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

namespace SharpDX.Direct3D9
{
    /// <summary>The device state enumeration.</summary>
    public enum DeviceState
    {
        /// <summary>The device hung.</summary>
        DeviceHung = -2005530508,

        /// <summary>The device lost.</summary>
        DeviceLost = -2005530520,

        /// <summary>The device removed.</summary>
        DeviceRemoved = -2005530512,

        /// <summary>The ok.</summary>
        Ok = 0,

        /// <summary>The out of video memory.</summary>
        OutOfVideoMemory = -2005532292,

        /// <summary>The present mode changed.</summary>
        PresentModeChanged = 0x8760877,

        /// <summary>The present occluded.</summary>
        PresentOccluded = 0x8760878
    }
}