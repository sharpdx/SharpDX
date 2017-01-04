#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(CaptureEngineOnSampleCallbackShadow))]
    public partial interface CaptureEngineOnSampleCallback
    {
        void OnSample(Sample sample);
    }
}
#endif