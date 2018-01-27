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

using System;

namespace SharpDX.MediaFoundation
{
    public partial class Transform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpDX.MediaFoundation.Transform"/> class.
        /// </summary>
        /// <param name="guid">Guid of the Media Foundation Transform.</param>
        public Transform(Guid guid)
        {
            Utilities.CreateComInstance(guid, Utilities.CLSCTX.ClsctxInprocServer, Utilities.GetGuidFromType(typeof(Transform)), this);
        }

        /// <summary>
        /// Gets the stream identifiers for the input and output streams on this Media Foundation transform (MFT).
        /// </summary>
        /// <param name="dwInputIDsRef">An array allocated by the caller. The method fills the array with the input stream identifiers. The array size must be at least equal to the number of input streams. To get the number of input streams, call <strong><see cref="SharpDX.MediaFoundation.Transform.GetStreamCount" /></strong>.<para>If the caller passes an array that is larger than the number of input streams, the MFT must not write values into the extra array entries.</para></param>
        /// <param name="dwOutputIDsRef">An array allocated by the caller. The method fills the array with the output stream identifiers. The array size must be at least equal to the number of output streams. To get the number of output streams, call <strong><see cref="SharpDX.MediaFoundation.Transform.GetStreamCount" /></strong>.<para>If the caller passes an array that is larger than the number of output streams, the MFT must not write values into the extra array entries.</para></param>
        /// <returns><c>true</c> if Both streams IDs for input and output are valid, <c>false</c> otherwise</returns>
        /// <msdn-id>ms693988</msdn-id>
        ///   <unmanaged>HRESULT IMFTransform::GetStreamIDs([In] unsigned int dwInputIDArraySize,[Out, Buffer] unsigned int* pdwInputIDs,[In] unsigned int dwOutputIDArraySize,[Out, Buffer] unsigned int* pdwOutputIDs)</unmanaged>
        ///   <unmanaged-short>IMFTransform::GetStreamIDs</unmanaged-short>
        public bool TryGetStreamIDs(int[] dwInputIDsRef, int[] dwOutputIDsRef)
        {
            bool isStreamIDsValid = true;
            var result = GetStreamIDs(dwInputIDsRef.Length, dwInputIDsRef, dwOutputIDsRef.Length, dwOutputIDsRef);

            //if not implemented
            if (result == Result.NotImplemented)
            {
                isStreamIDsValid = false;
            }
            else
            {
                result.CheckError();
            }

            return isStreamIDsValid;
        }

        /// <summary>
        /// Gets an available media type for an output stream on this Media Foundation transform (MFT).
        /// </summary>
        /// <param name="dwOutputStreamID">Output stream identifier. To get the list of stream identifiers, call <strong><see cref="SharpDX.MediaFoundation.Transform.GetStreamIDs" /></strong>.</param>
        /// <param name="dwTypeIndex">Index of the media type to retrieve. Media types are indexed from zero and returned in approximate order of preference.</param>
        /// <param name="typeOut">Receives a pointer to the <strong><see cref="SharpDX.MediaFoundation.MediaType" /></strong> interface. The caller must release the interface.</param>
        /// <returns><c>true</c> if A media type for an output stream is available, <c>false</c> otherwise</returns>
        /// <msdn-id>ms703812</msdn-id>	
        /// <unmanaged>HRESULT IMFTransform::GetOutputAvailableType([In] unsigned int dwOutputStreamID,[In] unsigned int dwTypeIndex,[Out] IMFMediaType** ppType)</unmanaged>	
        /// <unmanaged-short>IMFTransform::GetOutputAvailableType</unmanaged-short>	
        public bool TryGetOutputAvailableType(int dwOutputStreamID, int dwTypeIndex, out MediaType typeOut)
        {
            bool mediaTypeAvailable = true;
            var result = GetOutputAvailableType(dwOutputStreamID, dwTypeIndex, out typeOut);

            //An object ran out of media types
            if (result == ResultCode.NoMoreTypes)
            {
                mediaTypeAvailable = false;
            }
            else
            {
                result.CheckError();
            }

            return mediaTypeAvailable;
        }

        /// <summary>
        /// Generates output from the current input data.
        /// </summary>
        /// <param name="dwFlags">Bitwise OR of zero or more flags from the <strong><see cref="SharpDX.MediaFoundation.TransformProcessOutputFlags" /></strong> enumeration.</param>
        /// <param name="outputSamplesRef">Pointer to an array of <strong><see cref="SharpDX.MediaFoundation.TOutputDataBuffer" /></strong> structures, allocated by the caller. The MFT uses this array to return output data to the caller.</param>
        /// <param name="dwStatusRef">Receives a bitwise OR of zero or more flags from the <strong><see cref="SharpDX.MediaFoundation.TransformProcessOutputStatus" /></strong> enumeration.</param>
        /// <returns><c>true</c> if the transform cannot produce output data until it receives more input data, <c>false</c> otherwise</returns>
        /// <msdn-id>ms704014</msdn-id>	
        /// <unmanaged>HRESULT IMFTransform::ProcessOutput([In] _MFT_PROCESS_OUTPUT_FLAGS dwFlags,[In] unsigned int cOutputBufferCount,[In] MFT_OUTPUT_DATA_BUFFER* pOutputSamples,[Out] _MFT_PROCESS_OUTPUT_STATUS* pdwStatus)</unmanaged>	
        /// <unmanaged-short>IMFTransform::ProcessOutput</unmanaged-short>	
        public bool ProcessOutput(TransformProcessOutputFlags dwFlags, TOutputDataBuffer[] outputSamplesRef, out TransformProcessOutputStatus dwStatusRef)
        {
            bool needMoreInput = false;
            var result = ProcessOutput(dwFlags, outputSamplesRef.Length, ref outputSamplesRef[0], out dwStatusRef);

            // Check if it needs more input
            if (result== ResultCode.TransformNeedMoreInput)
            {
                needMoreInput = true;
            }
            else
            {
                result.CheckError();
            }

            return !needMoreInput;
        }
    }
}