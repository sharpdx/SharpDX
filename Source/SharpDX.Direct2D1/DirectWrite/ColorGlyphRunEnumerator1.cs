using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.DirectWrite
{
    public partial class ColorGlyphRunEnumerator1
    {
        public new unsafe ColorGlyphRun1 CurrentRun
        {
            get
            {
                GetCurrentRun(out IntPtr ptr);
                var run = new ColorGlyphRun1();
                run.__MarshalFrom(ref *((ColorGlyphRun1.__Native*)ptr));
                return run;
            }
        }
    }
}
