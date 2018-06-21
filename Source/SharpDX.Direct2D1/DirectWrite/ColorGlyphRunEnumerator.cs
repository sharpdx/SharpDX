using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectWrite
{
    public partial class ColorGlyphRunEnumerator
    {
        public unsafe ColorGlyphRun GetCurrent()
        {
            GetCurrentRun(out IntPtr ptr);
            var run = new ColorGlyphRun();
            run.__MarshalFrom(ref *((ColorGlyphRun.__Native*)ptr));
            return run;
        }
    }
}
