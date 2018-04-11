using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.MediaFoundation
{
    public partial class SinkWriter
    {
        /// <summary>	
        /// <p>Gets statistics about the performance of the sink writer.</p>	
        /// </summary>	
        /// <param name="dwStreamIndex"><dd> <p>The zero-based index of a stream to query, or <strong><see cref="SharpDX.MediaFoundation.SinkWriterIndex.AllStreams"/> </strong> to query the media sink itself.</p> </dd></param>	
        /// <param name="statsRef"><dd> <p>A reference to an <strong><see cref="SharpDX.MediaFoundation.SinkWriterStatistics"/></strong> structure. Before calling the method, set the <strong>cb</strong> member to the size of the structure in bytes. The method fills the structure with statistics from the sink writer.</p> </dd></param>	
        /// <returns><p>This method can return one of these values.</p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p>Success.</p> </td></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.MediaFoundation.ResultCode.InvalidStreamNumber"/></strong></dt> </dl> </td><td> <p>Invalid stream number.</p> </td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p>This interface is available on Windows?Vista if Platform Update Supplement for Windows?Vista is installed.</p>	
        /// </remarks>	
        /// <include file='.\..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IMFSinkWriter::GetStatistics']/*"/>	
        /// <msdn-id>dd374650</msdn-id>	
        /// <unmanaged>HRESULT IMFSinkWriter::GetStatistics([In] unsigned int dwStreamIndex,[Out] MF_SINK_WRITER_STATISTICS* pStats)</unmanaged>	
        /// <unmanaged-short>IMFSinkWriter::GetStatistics</unmanaged-short>	
        public void GetStatistics(int dwStreamIndex, out SharpDX.MediaFoundation.SinkWriterStatistics statsRef)
        {
            unsafe
            {
                statsRef = new SharpDX.MediaFoundation.SinkWriterStatistics();
                statsRef.Cb = sizeof(SharpDX.MediaFoundation.SinkWriterStatistics);
                fixed (void* statsRefPtr = &statsRef)
                {
                    GetStatistics_(dwStreamIndex, new IntPtr(statsRefPtr));
                }
            }
        }
    }
}
