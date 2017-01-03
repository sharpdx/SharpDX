using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CapturePhotoConfirmation
    {
        public IAsyncCallback PhotoConfirmationCallback
        {
            set
            {
                SetPhotoConfirmationCallback_(AsyncCallbackShadow.ToIntPtr(value));
            }
        }
    }
}
