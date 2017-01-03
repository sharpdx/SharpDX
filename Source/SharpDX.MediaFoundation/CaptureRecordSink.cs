#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CaptureRecordSink
    {
        public void SetOutputByteStream(ByteStream stream, Guid containerType)
        {
            SetOutputByteStream_(stream.NativePointer, containerType);
        }

        public void SetSampleCallback(int streamSinkIndex, CaptureEngineOnSampleCallback callback)
        {
            SetSampleCallback_(streamSinkIndex, CaptureEngineOnSampleCallbackShadow.ToIntPtr(callback));
        }
    }
}

#endif