using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public static partial class VideoFormatGuids
    {
        /// <summary>
        /// Returns a standard Media foundation GUID format from a FourCC input
        /// </summary>
        /// <param name="fourCC">FourCC input</param>
        /// <returns>Media foundation unique ID</returns>
        public static Guid FromFourCC(SharpDX.Multimedia.FourCC fourCC)
        {
            return new Guid(string.Concat(fourCC.ToString("I", null), "-0000-0010-8000-00aa00389b71"));
        }

        public static readonly Guid Wmv1 = new Guid("31564d57-0000-0010-8000-00aa00389b71");
        public static readonly Guid Wmv2 = new Guid("32564d57-0000-0010-8000-00aa00389b71");
        public static readonly Guid Wmv3 = new Guid("33564d57-0000-0010-8000-00aa00389b71");

        public static readonly Guid Dvc = new Guid("20637664-0000-0010-8000-00aa00389b71");
        public static readonly Guid Dv50 = new Guid("30357664-0000-0010-8000-00aa00389b71");
        public static readonly Guid Dv25 = new Guid("35327664-0000-0010-8000-00aa00389b71");

        public static readonly Guid H263 = new Guid("33363248-0000-0010-8000-00aa00389b71");
        public static readonly Guid H264 = new Guid("34363248-0000-0010-8000-00aa00389b71");
        public static readonly Guid H265 = new Guid("35363248-0000-0010-8000-00aa00389b71");

        public static readonly Guid Hevc = new Guid("43564548-0000-0010-8000-00aa00389b71");
        public static readonly Guid HevcEs = new Guid("53564548-0000-0010-8000-00aa00389b71");

        public static readonly Guid Vp80 = new Guid("30385056-0000-0010-8000-00aa00389b71");
        public static readonly Guid Vp90 = new Guid("30395056-0000-0010-8000-00aa00389b71");

        public static readonly Guid MultisampledS2 = new Guid("3253534d-0000-0010-8000-00aa00389b71");
        public static readonly Guid M4S2 = new Guid("3253344d-0000-0010-8000-00aa00389b71");
        public static readonly Guid Wvc1 = new Guid("31435657-0000-0010-8000-00aa00389b71");

        public static readonly Guid P010 = new Guid("30313050-0000-0010-8000-00aa00389b71");
        public static readonly Guid AI44 = new Guid("34344941-0000-0010-8000-00aa00389b71");

        public static readonly Guid Dvh1 = new Guid("31687664-0000-0010-8000-00aa00389b71");
        public static readonly Guid Dvhd = new Guid("64687664-0000-0010-8000-00aa00389b71");

        public static readonly Guid MultisampledS1 = new Guid("3153534d-0000-0010-8000-00aa00389b71");

        public static readonly Guid Mp43 = new Guid("3334504d-0000-0010-8000-00aa00389b71");
        public static readonly Guid Mp4s = new Guid("5334504d-0000-0010-8000-00aa00389b71");
        public static readonly Guid Mp4v = new Guid("5634504d-0000-0010-8000-00aa00389b71");
        public static readonly Guid Mpg1 = new Guid("3147504d-0000-0010-8000-00aa00389b71");
        public static readonly Guid Mjpg = new Guid("47504a4d-0000-0010-8000-00aa00389b71");

        public static readonly Guid Dvsl = new Guid("6c737664-0000-0010-8000-00aa00389b71");
        public static readonly Guid YUY2 = new Guid("32595559-0000-0010-8000-00aa00389b71");

        public static readonly Guid Yv12 = new Guid("32315659-0000-0010-8000-00aa00389b71");
        public static readonly Guid P016 = new Guid("36313050-0000-0010-8000-00aa00389b71");

        public static readonly Guid P210 = new Guid("30313250-0000-0010-8000-00aa00389b71");
        public static readonly Guid P216 = new Guid("36313250-0000-0010-8000-00aa00389b71");
        public static readonly Guid I420 = new Guid("30323449-0000-0010-8000-00aa00389b71");
        public static readonly Guid Dvsd = new Guid("64737664-0000-0010-8000-00aa00389b71");

        public static readonly Guid Y42T = new Guid("54323459-0000-0010-8000-00aa00389b71");
        public static readonly Guid NV12 = new Guid("3231564e-0000-0010-8000-00aa00389b71");
        public static readonly Guid NV11 = new Guid("3131564e-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y210 = new Guid("30313259-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y216 = new Guid("36313259-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y410 = new Guid("30313459-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y416 = new Guid("36313459-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y41P = new Guid("50313459-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y41T = new Guid("54313459-0000-0010-8000-00aa00389b71");
        public static readonly Guid Yvu9 = new Guid("39555659-0000-0010-8000-00aa00389b71");
        public static readonly Guid Yvyu = new Guid("55595659-0000-0010-8000-00aa00389b71");
        public static readonly Guid Iyuv = new Guid("56555949-0000-0010-8000-00aa00389b71");
        public static readonly Guid Uyvy = new Guid("59565955-0000-0010-8000-00aa00389b71");

        public static readonly Guid AYUV = new Guid("56555941-0000-0010-8000-00aa00389b71");
        public static readonly Guid Y420O = new Guid("4f303234-0000-0010-8000-00aa00389b71");
    }
}
