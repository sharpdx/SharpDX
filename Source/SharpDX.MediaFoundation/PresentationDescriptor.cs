using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class PresentationDescriptor
    {
        /// <summary>	
        /// <p> </p><p>Retrieves a stream descriptor for a stream in the presentation. The stream descriptor contains information about the stream.</p>	
        /// </summary>	
        /// <param name="dwIndex"><dd> <p>Zero-based index of the stream. To find the number of streams in the presentation, call the <strong><see cref="SharpDX.MediaFoundation.PresentationDescriptor.GetStreamDescriptorCount"/></strong> method.</p> </dd></param>	
        /// <param name="fSelectedRef"><dd> <p>Receives a Boolean value. The value is <strong>TRUE</strong> if the stream is currently selected, or <strong><see cref="SharpDX.Result.False"/></strong> if the stream is currently deselected. If a stream is selected, the media source generates data for that stream when <strong><see cref="SharpDX.MediaFoundation.MediaSource.Start"/></strong> is called. The media source will not generated data for deselected streams. To select a stream, call <strong><see cref="SharpDX.MediaFoundation.PresentationDescriptor.SelectStream"/></strong>.To deselect a stream, call <strong><see cref="SharpDX.MediaFoundation.PresentationDescriptor.DeselectStream"/></strong>.</p> </dd></param>	
        /// <param name="descriptorOut"><dd> <p>Receives a reference to the stream descriptor's <strong><see cref="SharpDX.MediaFoundation.StreamDescriptor"/></strong> interface. The caller must release the interface.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFPresentationDescriptor::GetStreamDescriptorByIndex']/*"/>	
        /// <msdn-id>ms694924</msdn-id>	
        /// <unmanaged>HRESULT IMFPresentationDescriptor::GetStreamDescriptorByIndex([In] unsigned int dwIndex,[Out] BOOL* pfSelected,[Out] IMFStreamDescriptor** ppDescriptor)</unmanaged>	
        /// <unmanaged-short>IMFPresentationDescriptor::GetStreamDescriptorByIndex</unmanaged-short>	
        public StreamDescriptor GetStreamDescriptorByIndex(int dwIndex, out SharpDX.Mathematics.Interop.RawBool fSelectedRef)
        {
            StreamDescriptor streamDescriptor;
            this.GetStreamDescriptorByIndex(dwIndex, out fSelectedRef, out streamDescriptor);
            return streamDescriptor;
        }
    }
}
