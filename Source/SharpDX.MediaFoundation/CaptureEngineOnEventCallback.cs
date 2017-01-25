#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    [Shadow(typeof(CaptureEngineOnEventCallbackShadow))]
    internal partial interface CaptureEngineOnEventCallback
    {
        void OnEvent(MediaEvent mediaEvent);
    }
}
#endif