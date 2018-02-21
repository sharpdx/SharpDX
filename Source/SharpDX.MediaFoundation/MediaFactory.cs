// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#if DESKTOP_APP
using System;
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    public static partial class MediaFactory
    {
        /// <summary>	
        /// <p>Enumerates a list of audio or video capture devices.</p>	
        /// </summary>	
        /// <param name="attributesRef"><dd> <p>Pointer to an attribute store that contains search criteria. To create the attribute store, call <strong><see cref="SharpDX.MediaFoundation.MediaFactory.CreateAttributes"/></strong>. Set one or more of the following attributes on the attribute store:</p> <table> <tr><th>Value</th><th>Meaning</th></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceType"/></strong></dt> </dl> </td><td> <p>Specifies whether to enumerate audio or video devices. (Required.)</p> </td></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceTypeAudcapRole"/></strong></dt> </dl> </td><td> <p>For audio capture devices, specifies the device role. (Optional.)</p> </td></tr> <tr><td><dl> <dt><strong><see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceTypeVidcapCategory"/></strong></dt> </dl> </td><td> <p>For video capture devices, specifies the device category. (Optional.)</p> </td></tr> </table> <p>?</p> </dd></param>	
        /// <param name="pSourceActivateOut"><dd> <p>Receives an array of <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> interface references. Each reference represents an activation object for a media source. The function allocates the memory for the array. The caller must release the references in the array and call <strong>CoTaskMemFree</strong> to free the memory for the array.</p> </dd></param>	
        /// <param name="cSourceActivateRef"><dd> <p>Receives the number of elements in the <em>pppSourceActivate</em> array. If no capture devices match the search criteria, this parameter receives the value 0.</p> </dd></param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>Each returned <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> reference represents a capture device, and can be used to create a media source for that device. You can also use the <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> reference to query for attributes that describe the device. The following attributes might be set:</p><table> <tr><th>Attribute</th><th>Description</th></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.FriendlyName"/> </td><td>The display name of the device.</td></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.MediaType"/> </td><td>The major type and subtype GUIDs that describe the device's output format.</td></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceType"/> </td><td>The type of capture device (audio or video).</td></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceTypeAudcapEndpointId"/> </td><td>The audio endpoint ID string. (Audio devices only.)</td></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceTypeVidcapCategory"/> </td><td>The device category. (Video devices only.)</td></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceTypeVidcapHwSource"/> </td><td> Whether a device is a hardware or software device. (Video devices only.)</td></tr> <tr><td> <see cref="SharpDX.MediaFoundation.CaptureDeviceAttributeKeys.SourceTypeVidcapSymbolicLink"/> </td><td>The symbolic link for the device driver. (Video devices only.)</td></tr> </table><p>?</p><p>To create a media source from an <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> reference, call the <strong><see cref="SharpDX.MediaFoundation.Activate.ActivateObject"/></strong> method.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='MFEnumDeviceSources']/*"/>	
        /// <msdn-id>dd388503</msdn-id>	
        /// <unmanaged>HRESULT MFEnumDeviceSources([In] IMFAttributes* pAttributes,[Out, Buffer] IMFActivate*** pppSourceActivate,[Out] unsigned int* pcSourceActivate)</unmanaged>	
        /// <unmanaged-short>MFEnumDeviceSources</unmanaged-short>	
        public static Activate[] EnumDeviceSources(MediaAttributes attributesRef)
        {

            IntPtr devicePtr;
            int devicesCount;

            EnumDeviceSources(attributesRef, out devicePtr, out devicesCount);

            var result = new Activate[devicesCount];

            unsafe
            {
                var address = (void**)devicePtr;
                for (var i = 0; i < devicesCount; i++)
                    result[i] = new Activate(new IntPtr(address[i]));
            }

            return result;
        }

        /// <summary>
        /// Gets a list of Microsoft Media Foundation transforms (MFTs) that match specified search criteria. This function extends the <strong><see cref="SharpDX.MediaFoundation.MediaFactory.TEnum"/></strong> function.
        /// </summary>
        /// <param name="guidCategory">A GUID that specifies the category of MFTs to enumerate. For a list of MFT categories, see <strong><see cref="SharpDX.MediaFoundation.TransformCategoryGuids"/></strong>.</param>
        /// <param name="enumFlags">The bitwise OR of zero or more flags from the <strong><see cref="SharpDX.MediaFoundation.TransformEnumFlag"/></strong> enumeration.</param>
        /// <param name="inputTypeRef">A pointer to an <strong><see cref="SharpDX.MediaFoundation.TRegisterTypeInformation"/></strong> structure that specifies an input media type to match.<para>This parameter can be NULL. If NULL, all input types are matched.</para></param>
        /// <param name="outputTypeRef">A pointer to an <strong><see cref="SharpDX.MediaFoundation.TRegisterTypeInformation"/></strong> structure that specifies an output media type to match.<para>This parameter can be NULL. If NULL, all output types are matched.</para></param>
        /// <returns>Returns an array of <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> objects. Each object represents an activation object for an MFT that matches the search criteria. The function allocates the memory for the array. The caller must release the pointers and call the Dispose for each element in the array.</returns>
        /// <msdn-id>dd388652</msdn-id>	
        /// <unmanaged>HRESULT MFTEnumEx([In] GUID guidCategory,[In] unsigned int Flags,[In, Optional] const MFT_REGISTER_TYPE_INFO* pInputType,[In, Optional] const MFT_REGISTER_TYPE_INFO* pOutputType,[Out, Buffer] IMFActivate*** pppMFTActivate,[Out] unsigned int* pnumMFTActivate)</unmanaged>	
        /// <unmanaged-short>MFTEnumEx</unmanaged-short>	
        public static Activate[] FindTransform(Guid guidCategory, TransformEnumFlag enumFlags, TRegisterTypeInformation? inputTypeRef = null, TRegisterTypeInformation? outputTypeRef = null)
        {
            IntPtr pActivatesArr;
            int pNumActivates;
            TEnumEx(guidCategory, (int)enumFlags, inputTypeRef, outputTypeRef, out pActivatesArr, out pNumActivates);

            var activates = new Activate[pNumActivates];
            unsafe
            {
                var ptr = (IntPtr*)(pActivatesArr);
                for (int i = 0; i < pNumActivates; i++)
                {
                    activates[i] = new Activate(ptr[i]);
                }
            }
            Marshal.FreeCoTaskMem(pActivatesArr);

            return activates;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Copies an image or image plane from one buffer to another. </p>	
        /// </summary>	
        /// <param name="destRef"><dd> <p> Pointer to the start of the first row of pixels in the destination buffer. </p> </dd></param>	
        /// <param name="lDestStride"><dd> <p> Stride of the destination buffer, in bytes. </p> </dd></param>	
        /// <param name="srcRef"><dd> <p> Pointer to the start of the first row of pixels in the source image. </p> </dd></param>	
        /// <param name="lSrcStride"><dd> <p> Stride of the source image, in bytes. </p> </dd></param>	
        /// <param name="dwWidthInBytes"><dd> <p> Width of the image, in bytes. </p> </dd></param>	
        /// <param name="dwLines"><dd> <p> Number of rows of pixels to copy. </p> </dd></param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p> This function copies a single plane of the image. For planar YUV formats, you must call the function once for each plane. In this case, <em>pDest</em> and <em>pSrc</em> must point to the start of each plane. </p><p> This function is optimized if the MMX, SSE, or SSE2 instruction sets are available on the processor. The function performs a non-temporal store (the data is written to memory directly without polluting the cache). </p><p><strong>Note</strong>??Prior to Windows?7, this function was exported from evr.dll. Starting in Windows?7, this function is exported from mfplat.dll, and evr.dll exports a stub function that calls into mfplat.dll. For more information, see Library Changes in Windows?7.</p>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='MFCopyImage']/*"/>	
        /// <msdn-id>bb970554</msdn-id>	
        /// <unmanaged>HRESULT MFCopyImage([Out, Buffer] unsigned char* pDest,[In] int lDestStride,[In, Buffer] const unsigned char* pSrc,[In] int lSrcStride,[In] unsigned int dwWidthInBytes,[In] unsigned int dwLines)</unmanaged>	
        /// <unmanaged-short>MFCopyImage</unmanaged-short>	
        public static void CopyImage(IntPtr destRef, int lDestStride, IntPtr srcRef, int lSrcStride, int dwWidthInBytes, int dwLines)
        {
            unsafe
            {
                SharpDX.Result __result__;
                __result__ = MFCopyImage_(destRef.ToPointer(), lDestStride, srcRef.ToPointer(), lSrcStride, dwWidthInBytes, dwLines);
                __result__.CheckError();
            }
        }
    }
}
#endif