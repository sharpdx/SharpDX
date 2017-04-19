using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.MediaFoundation
{
    public partial class MediaTypeHandler
    {
        /// <summary>	
        /// <p> </p><p>Retrieves a media type from the object's list of supported media types.</p>	
        /// </summary>	
        /// <param name="dwIndex"><dd> <p> Zero-based index of the media type to retrieve. To get the number of media types in the list, call <strong><see cref="SharpDX.MediaFoundation.MediaTypeHandler.GetMediaTypeCount"/></strong>. </p> </dd></param>	
        /// <param name="typeOut"><dd> <p> Receives a reference to the <strong><see cref="SharpDX.MediaFoundation.MediaType"/></strong> interface. The caller must release the interface. </p> </dd></param>	
        /// <returns><p>The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table.</p><table> <tr><th>Return code</th><th>Description</th></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.Result.Ok"/></strong></dt> </dl> </td><td> <p> The method succeeded. </p> </td></tr> <tr><td> <dl> <dt><strong><see cref="SharpDX.MediaFoundation.ResultCode.NoMoreTypes"/></strong></dt> </dl> </td><td> <p> The <em>dwIndex</em> parameter is out of range. </p> </td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p>Media types are returned in the approximate order of preference. The list of supported types is not guaranteed to be complete. To test whether a particular media type is supported, call <strong><see cref="SharpDX.MediaFoundation.MediaTypeHandler.IsMediaTypeSupported"/></strong>.</p><p>This interface is available on the following platforms if the Windows Media Format 11 SDK redistributable components are installed:</p><ul> <li>Windows?XP with Service Pack?2 (SP2) and later.</li> <li>Windows?XP Media Center Edition?2005 with KB900325 (Windows?XP Media Center Edition?2005) and KB925766 (October 2006 Update Rollup for Windows?XP Media Center Edition) installed.</li> </ul>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IMFMediaTypeHandler::GetMediaTypeByIndex']/*"/>	
        /// <msdn-id>bb970473</msdn-id>	
        /// <unmanaged>HRESULT IMFMediaTypeHandler::GetMediaTypeByIndex([In] unsigned int dwIndex,[Out] IMFMediaType** ppType)</unmanaged>	
        /// <unmanaged-short>IMFMediaTypeHandler::GetMediaTypeByIndex</unmanaged-short>	
        public MediaType GetMediaTypeByIndex(int dwIndex)
        {
            MediaType mediaType;
            this.GetMediaTypeByIndex(dwIndex, out mediaType);
            return mediaType;
        }
    }
}
