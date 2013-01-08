using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.MediaFoundation
{
    public partial class Transform
    {
        public const uint E_NotImplemented = 0x80004001;
        public const uint MF_E_NO_MORE_TYPES = 0xC00D36B9;
        public const uint MF_E_TRANSFORM_NEED_MORE_INPUT = 0xC00D6D72;

        /// <summary>
        /// Gets the stream identifiers for the input and output streams on this Media Foundation transform (MFT).
        /// </summary>
        /// <param name="dwInputIDsRef">An array allocated by the caller. The method fills the array with the input stream identifiers. The array size must be at least equal to the number of input streams. To get the number of input streams, call <strong><see cref="SharpDX.MediaFoundation.Transform.GetStreamCount"/></strong>.<para>If the caller passes an array that is larger than the number of input streams, the MFT must not write values into the extra array entries.</para></param>
        /// <param name="dwOutputIDsRef">An array allocated by the caller. The method fills the array with the output stream identifiers. The array size must be at least equal to the number of output streams. To get the number of output streams, call <strong><see cref="SharpDX.MediaFoundation.Transform.GetStreamCount"/></strong>.<para>If the caller passes an array that is larger than the number of output streams, the MFT must not write values into the extra array entries.</para></param>
        /// <param name="isImplemented">If true, Both streams IDs for input and output assumed 0.</param>
        public void GetStreamIDs(int[] dwInputIDsRef, int[] dwOutputIDsRef, out bool isImplemented)
        {
            isImplemented = true;
            unsafe
            {
                SharpDX.Result __result__;
                fixed (void* dwInputIDsRef_ = dwInputIDsRef)
                fixed (void* dwOutputIDsRef_ = dwOutputIDsRef)
                    __result__ =
                    SharpDX.MediaFoundation.LocalInterop.Calliint(_nativePointer, dwInputIDsRef.Length, dwInputIDsRef_, dwOutputIDsRef.Length, dwOutputIDsRef_, ((void**)(*(void**)_nativePointer))[5]);

                //if not implemented
                if ((uint)__result__.Code == E_NotImplemented)
                {
                    isImplemented = false;
                }
                else
                    __result__.CheckError();
            }
        }

        /// <summary>
        /// Gets an available media type for an output stream on this Media Foundation transform (MFT).
        /// </summary>
        /// <param name="dwOutputStreamID">Output stream identifier. To get the list of stream identifiers, call <strong><see cref="SharpDX.MediaFoundation.Transform.GetStreamIDs"/></strong>.</param>
        /// <param name="dwTypeIndex">Index of the media type to retrieve. Media types are indexed from zero and returned in approximate order of preference.</param>
        /// <param name="typeOut">Receives a pointer to the <strong><see cref="SharpDX.MediaFoundation.MediaType"/></strong> interface. The caller must release the interface.</param>
        /// <param name="objectRanOutOfMediaTypes">Returns true if no more output media types is available</param>
        public void GetOutputAvailableType(int dwOutputStreamID, int dwTypeIndex, out MediaType typeOut, out bool objectRanOutOfMediaTypes)
        {
            objectRanOutOfMediaTypes = false;
            unsafe
            {
                IntPtr typeOut_ = IntPtr.Zero;
                SharpDX.Result __result__;
                __result__ =
                SharpDX.MediaFoundation.LocalInterop.Calliint(_nativePointer, dwOutputStreamID, dwTypeIndex, &typeOut_, ((void**)(*(void**)_nativePointer))[14]);
                typeOut = (typeOut_ == IntPtr.Zero) ? null : new SharpDX.MediaFoundation.MediaType(typeOut_);

                //An object ran out of media types
                if ((uint)__result__.Code == MF_E_NO_MORE_TYPES)
                {
                    objectRanOutOfMediaTypes = true;
                }
                else
                {
                    __result__.CheckError();
                }
            }
        }

        /// <summary>
        /// Generates output from the current input data.
        /// </summary>
        /// <param name="dwFlags">Bitwise OR of zero or more flags from the <strong><see cref="SharpDX.MediaFoundation.TransformProcessOutputFlags"/></strong> enumeration.</param>
        /// <param name="outputSamplesRef">Pointer to an array of <strong><see cref="SharpDX.MediaFoundation.TOutputDataBuffer"/></strong> structures, allocated by the caller. The MFT uses this array to return output data to the caller.</param>
        /// <param name="dwStatusRef">Receives a bitwise OR of zero or more flags from the <strong><see cref="SharpDX.MediaFoundation.TransformProcessOutputStatus"/></strong> enumeration.</param>
        /// <param name="needMoreInput">Returns true if the transform cannot produce output data until it receives more input data.</param>
        public void ProcessOutput(TransformProcessOutputFlags dwFlags, TOutputDataBuffer[] outputSamplesRef, out TransformProcessOutputStatus dwStatusRef, out bool needMoreInput)
        {
            needMoreInput = false;
            unsafe
            {
                SharpDX.Result __result__;
                fixed (void* outputSamplesRef_ = &outputSamplesRef[0])
                fixed (void* dwStatusRef_ = &dwStatusRef)
                    __result__ = SharpDX.MediaFoundation.LocalInterop.Calliint(_nativePointer, (int)dwFlags, outputSamplesRef.Length, outputSamplesRef_, dwStatusRef_, ((void**)(*(void**)_nativePointer))[25]);
                if ((uint)__result__.Code == MF_E_TRANSFORM_NEED_MORE_INPUT)
                {
                    needMoreInput = true;
                }
                else
                    __result__.CheckError();
            }
        }

    }
}
