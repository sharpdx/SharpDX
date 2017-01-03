#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CapturePhotoSink
    {
        public ByteStream OutputByteStream
        {
            set
            {
                SetOutputByteStream_(value.NativePointer);
            }
        }

        public CaptureEngineOnSampleCallback SampleCallback
        {
            set
            {
                SetSampleCallback_(CaptureEngineOnSampleCallbackShadow.ToIntPtr(value));
            }
        }
    }
}
#endif
