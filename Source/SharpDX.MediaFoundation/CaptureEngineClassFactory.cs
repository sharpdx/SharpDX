#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CaptureEngineClassFactory
    {
        public CaptureEngineClassFactory()
        {
            Utilities.CreateComInstance(ClsidMFCaptureEngineClassFactory, Utilities.CLSCTX.ClsctxInproc, Utilities.GetGuidFromType(typeof(CaptureEngineClassFactory)), this);
        }
    }
}
#endif
