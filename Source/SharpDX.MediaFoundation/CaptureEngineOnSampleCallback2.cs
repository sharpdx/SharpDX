#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(CaptureEngineOnSampleCallback2Shadow))]
    public partial interface CaptureEngineOnSampleCallback2
    {
        void OnSynchronizedEvent(MediaEvent mediaEvent);
    }
}
#endif