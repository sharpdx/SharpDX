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
    }
}
