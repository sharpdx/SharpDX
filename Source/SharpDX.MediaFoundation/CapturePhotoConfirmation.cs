#if DESKTOP_APP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class CapturePhotoConfirmation
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <param name="notificationCallbackRef">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFCapturePhotoConfirmation::SetPhotoConfirmationCallback']/*"/>	
        /// <unmanaged>HRESULT IMFCapturePhotoConfirmation::SetPhotoConfirmationCallback([In] IMFAsyncCallback* pNotificationCallback)</unmanaged>	
        /// <unmanaged-short>IMFCapturePhotoConfirmation::SetPhotoConfirmationCallback</unmanaged-short>	
        public IAsyncCallback PhotoConfirmationCallback
        {
            set
            {
                SetPhotoConfirmationCallback(value);
            }
        }
    }
}

#endif