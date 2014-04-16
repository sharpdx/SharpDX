// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
#if !W8CORE
using System;
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    public static partial class MediaFactory
    {
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
        /// <p><strong>Applies to: </strong>desktop apps only</p><p> </p><p>Creates an activation object for the sample grabber media sink.</p>	
        /// </summary>	
        /// <param name="iMFMediaTypeRef"><dd> <p> Pointer to the <strong><see cref="SharpDX.MediaFoundation.MediaType"/></strong> interface, defining the media type for the sample grabber's input stream. </p> </dd></param>	
        /// <param name="iMFSampleGrabberSinkCallbackRef"><dd> <p> Pointer to the <strong><see cref="SharpDX.MediaFoundation.SampleGrabberSinkCallback"/></strong> interface of a callback object. The caller must implement this interface. </p> </dd></param>	
        /// <param name="iActivateOut"><dd> <p> Receives a reference to the <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> interface. Use this interface to complete the creation of the sample grabber. The caller must release the interface. </p> </dd></param>	
        /// <returns><p>If this function succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <remarks>	
        /// <p>To create the sample grabber sink, call <strong><see cref="SharpDX.MediaFoundation.Activate.ActivateObject"/></strong> on the reference received in the <em>ppIActivate</em> parameter.</p><p>Before calling <strong>ActivateObject</strong>, you can configure the sample grabber by setting any of the following attributes on the <em>ppIActivate</em> reference:</p><ul> <li> <see cref="SharpDX.MediaFoundation.SampleGrabberSinkAttributeKeys.IgnoreClock"/> </li> <li> <strong><see cref="SharpDX.MediaFoundation.SampleGrabberSinkAttributeKeys.SampleTimeOffset"/></strong> </li> </ul>	
        /// </remarks>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='MFCreateSampleGrabberSinkActivate']/*"/>	
        /// <msdn-id>ms702068</msdn-id>	
        /// <unmanaged>HRESULT MFCreateSampleGrabberSinkActivate([In] IMFMediaType* pIMFMediaType,[In] IMFSampleGrabberSinkCallback* pIMFSampleGrabberSinkCallback,[Out] IMFActivate** ppIActivate)</unmanaged>	
        /// <unmanaged-short>MFCreateSampleGrabberSinkActivate</unmanaged-short>	
        public static void CreateSampleGrabberSinkActivate(MediaType mediaType, SampleGrabberSinkCallback callback, out Activate activate)
        {
            MediaFactory.CreateSampleGrabberSinkActivate(mediaType, SampleGrabberSinkCallbackShadow.ToIntPtr(callback), out activate);
        }
    }
}
#endif