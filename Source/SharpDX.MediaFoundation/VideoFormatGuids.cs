using SharpDX.Multimedia;
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

namespace SharpDX.MediaFoundation
{
    using System;

    /// <summary>
    /// Utility class which contains all Video Formats GUIDs as specified in mfapi.h
    /// </summary>
    public static class VideoFormatGuids
    {
        /// <summary>
        /// Get Video format Guid from a FourCC
        /// </summary>
        /// <param name="fourcc">FourCC required</param>
        /// <returns>Guid matching FourCC</returns>
        public static Guid FromFourCC(FourCC fourcc)
        {
            return new Guid(fourcc, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
        }

        /// <summary>Base Format</summary>
        /// <unmanaged>MFVideoFormat_Base</unmanaged>
        public static readonly System.Guid Base = new Guid(0, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>RGB32</summary>
        /// <unmanaged>MFVideoFormat_RGB32</unmanaged>
        public static readonly System.Guid RGB32 = new Guid(22, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>ARGB32</summary>
        /// <unmanaged>MFVideoFormat_ARGB32</unmanaged>
        public static readonly System.Guid ARGB32 = new Guid(21, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>RGB24</summary>
        /// <unmanaged>MFVideoFormat_RGB24</unmanaged>
        public static readonly System.Guid RGB24 = new Guid(20, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>RGB555</summary>
        /// <unmanaged>MFVideoFormat_RGB555</unmanaged>
        public static readonly System.Guid RGB555 = new Guid(24, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>RGB565</summary>
        /// <unmanaged>MFVideoFormat_RGB565</unmanaged>
        public static readonly System.Guid RGB565 = new Guid(23, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>RGB8</summary>
        /// <unmanaged>MFVideoFormat_RGB565</unmanaged>
        public static readonly System.Guid RGB8 = new Guid(41, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>AI44</summary>
        /// <unmanaged>MFVideoFormat_AI44</unmanaged>
        public static readonly System.Guid AI44 = new Guid(new FourCC("AI44"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>AYUV</summary>
        /// <unmanaged>MFVideoFormat_AYUV</unmanaged>
        public static readonly System.Guid AYUV = new Guid(new FourCC("AYUV"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>YUY2</summary>
        /// <unmanaged>MFVideoFormat_YUY2</unmanaged>
        public static readonly System.Guid YUY2 = new Guid(new FourCC("YUY2"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>YVYU</summary>
        /// <unmanaged>MFVideoFormat_YVYU</unmanaged>
        public static readonly System.Guid YVYU = new Guid(new FourCC("YVYU"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>YVU9</summary>
        /// <unmanaged>MFVideoFormat_YVU9</unmanaged>
        public static readonly System.Guid YVU9 = new Guid(new FourCC("YVU9"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>UYVY</summary>
        /// <unmanaged>MFVideoFormat_UYVY</unmanaged>
        public static readonly System.Guid UYVY = new Guid(new FourCC("UYVY"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>NV11</summary>
        /// <unmanaged>MFVideoFormat_NV11</unmanaged>
        public static readonly System.Guid NV11 = new Guid(new FourCC("NV11"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>NV12</summary>
        /// <unmanaged>MFVideoFormat_NV12</unmanaged>
        public static readonly System.Guid NV12 = new Guid(new FourCC("NV12"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>YV12</summary>
        /// <unmanaged>MFVideoFormat_YV12</unmanaged>
        public static readonly System.Guid YV12 = new Guid(new FourCC("YV12"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>I420</summary>
        /// <unmanaged>MFVideoFormat_I420</unmanaged>
        public static readonly System.Guid I420 = new Guid(new FourCC("I420"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>IYUV</summary>
        /// <unmanaged>MFVideoFormat_IYUV</unmanaged>
        public static readonly System.Guid IYUV = new Guid(new FourCC("IYUV"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y210</summary>
        /// <unmanaged>MFVideoFormat_Y210</unmanaged>
        public static readonly System.Guid Y210 = new Guid(new FourCC("Y210"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y216</summary>
        /// <unmanaged>MFVideoFormat_Y216</unmanaged>
        public static readonly System.Guid Y216 = new Guid(new FourCC("Y216"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y410</summary>
        /// <unmanaged>MFVideoFormat_Y410</unmanaged>
        public static readonly System.Guid Y410 = new Guid(new FourCC("Y410"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y416</summary>
        /// <unmanaged>MFVideoFormat_Y416</unmanaged>
        public static readonly System.Guid Y416 = new Guid(new FourCC("Y416"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y41P</summary>
        /// <unmanaged>MFVideoFormat_Y41P</unmanaged>
        public static readonly System.Guid Y41P = new Guid(new FourCC("Y41P"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y41T</summary>
        /// <unmanaged>MFVideoFormat_Y41T</unmanaged>
        public static readonly System.Guid Y41T = new Guid(new FourCC("Y41T"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y42T</summary>
        /// <unmanaged>MFVideoFormat_Y42T</unmanaged>
        public static readonly System.Guid Y42T = new Guid(new FourCC("Y42T"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>P210</summary>
        /// <unmanaged>MFVideoFormat_P210</unmanaged>
        public static readonly System.Guid P210 = new Guid(new FourCC("P210"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>P216</summary>
        /// <unmanaged>MFVideoFormat_P216</unmanaged>
        public static readonly System.Guid P216 = new Guid(new FourCC("P216"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>P010</summary>
        /// <unmanaged>MFVideoFormat_P010</unmanaged>
        public static readonly System.Guid P010 = new Guid(new FourCC("P010"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>P016</summary>
        /// <unmanaged>MFVideoFormat_P016</unmanaged>
        public static readonly System.Guid P016 = new Guid(new FourCC("P016"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>v210</summary>
        /// <unmanaged>MFVideoFormat_v210</unmanaged>
        public static readonly System.Guid v210 = new Guid(new FourCC("v210"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>v216</summary>
        /// <unmanaged>MFVideoFormat_v216</unmanaged>
        public static readonly System.Guid v216 = new Guid(new FourCC("v216"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>v410</summary>
        /// <unmanaged>MFVideoFormat_v410</unmanaged>
        public static readonly System.Guid v410 = new Guid(new FourCC("v410"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MP43</summary>
        /// <unmanaged>MFVideoFormat_MP43</unmanaged>
        public static readonly System.Guid MP43 = new Guid(new FourCC("MP43"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MP4S</summary>
        /// <unmanaged>MFVideoFormat_MP4S</unmanaged>
        public static readonly System.Guid MP4S = new Guid(new FourCC("MP4S"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>M4S2</summary>
        /// <unmanaged>MFVideoFormat_M4S2</unmanaged>
        public static readonly System.Guid M4S2 = new Guid(new FourCC("M4S2"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MP4V</summary>
        /// <unmanaged>MFVideoFormat_MP4V</unmanaged>
        public static readonly System.Guid MP4V = new Guid(new FourCC("MP4V"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>WMV1</summary>
        /// <unmanaged>MFVideoFormat_WMV1</unmanaged>
        public static readonly System.Guid WMV1 = new Guid(new FourCC("WMV1"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>WMV2</summary>
        /// <unmanaged>MFVideoFormat_WMV2</unmanaged>
        public static readonly System.Guid WMV2 = new Guid(new FourCC("WMV2"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>WMV3</summary>
        /// <unmanaged>MFVideoFormat_WMV3</unmanaged>
        public static readonly System.Guid WMV3 = new Guid(new FourCC("WMV3"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>WVC1</summary>
        /// <unmanaged>MFVideoFormat_WVC1</unmanaged>
        public static readonly System.Guid WVC1 = new Guid(new FourCC("WVC1"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MSS1</summary>
        /// <unmanaged>MFVideoFormat_MSS1</unmanaged>
        public static readonly System.Guid MSS1 = new Guid(new FourCC("MSS1"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MSS2</summary>
        /// <unmanaged>MFVideoFormat_MSS2</unmanaged>
        public static readonly System.Guid MSS2 = new Guid(new FourCC("MSS2"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MPG1</summary>
        /// <unmanaged>MFVideoFormat_MPG1</unmanaged>
        public static readonly System.Guid MPG1 = new Guid(new FourCC("MPG1"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dvsl</summary>
        /// <unmanaged>MFVideoFormat_DVSL</unmanaged>
        public static readonly System.Guid dvsl = new Guid(new FourCC("dvsl"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dvsd</summary>
        /// <unmanaged>MFVideoFormat_DVSD</unmanaged>
        public static readonly System.Guid dvsd = new Guid(new FourCC("dvsd"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dvhd</summary>
        /// <unmanaged>MFVideoFormat_DVHD</unmanaged>
        public static readonly System.Guid dvhd = new Guid(new FourCC("dvhd"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dv25</summary>
        /// <unmanaged>MFVideoFormat_DV25</unmanaged>
        public static readonly System.Guid dv25 = new Guid(new FourCC("dv25"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dv50</summary>
        /// <unmanaged>MFVideoFormat_DV50</unmanaged>
        public static readonly System.Guid dv50 = new Guid(new FourCC("dv50"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dvh1</summary>
        /// <unmanaged>MFVideoFormat_DVH1</unmanaged>
        public static readonly System.Guid dvh1 = new Guid(new FourCC("dvh1"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>dvc</summary>
        /// <unmanaged>MFVideoFormat_DVC</unmanaged>
        public static readonly System.Guid dvc = new Guid(new FourCC("dvc "), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>H264</summary>
        /// <unmanaged>MFVideoFormat_H264</unmanaged>
        public static readonly System.Guid H264 = new Guid(new FourCC("H264"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>MJPG</summary>
        /// <unmanaged>MFVideoFormat_MJPG</unmanaged>
        public static readonly System.Guid MJPG = new Guid(new FourCC("MJPG"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>Y420O</summary>
        /// <unmanaged>MFVideoFormat_420O</unmanaged>
        public static readonly System.Guid Y420O = new Guid(new FourCC("420O"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>HEVC</summary>
        /// <unmanaged>MFVideoFormat_HEVC</unmanaged>
        public static readonly System.Guid HEVC = new Guid(new FourCC("HEVC"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

        /// <summary>HEVS</summary>
        /// <unmanaged>MFVideoFormat_HEVC_ES</unmanaged>
        public static readonly System.Guid HEVS = new Guid(new FourCC("HEVS"), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

    }
}
