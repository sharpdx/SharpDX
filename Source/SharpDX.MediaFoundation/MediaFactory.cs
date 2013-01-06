using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
        /// <returns>Returnss an array of <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> objects. Each object represents an activation object for an MFT that matches the search criteria. The function allocates the memory for the array. The caller must release the pointers and call the Dispose for each element in the array.</returns>
        public static Activate[] TEnumEx(Guid guidCategory, TransformEnumFlag enumFlags, TRegisterTypeInformation? inputTypeRef, TRegisterTypeInformation? outputTypeRef)
        {
            Activate[] activateArray;
            int numActivate;

            TRegisterTypeInformation inputTypeRef_;
            if (inputTypeRef.HasValue)
                inputTypeRef_ = inputTypeRef.Value;
            TRegisterTypeInformation outputTypeRef_;
            if (outputTypeRef.HasValue)
                outputTypeRef_ = outputTypeRef.Value;
            Result __result__;
            unsafe
            {
                void* numActivateRef = &numActivate;
                IntPtr* pMFTActivateOut_ = stackalloc IntPtr[1];
                __result__ = MFTEnumEx_(guidCategory, (int)enumFlags, (inputTypeRef.HasValue) ? &inputTypeRef_ : (void*)IntPtr.Zero, (outputTypeRef.HasValue) ? &outputTypeRef_ : (void*)IntPtr.Zero, pMFTActivateOut_, numActivateRef);

                activateArray = new Activate[numActivate];
                IntPtr* ptr = (IntPtr*)(*pMFTActivateOut_);
                for (int i = 0; i < numActivate; i++)
                    activateArray[i] = (ptr[i] == IntPtr.Zero) ? null : new Activate(ptr[i]);
            }

            __result__.CheckError();
            return activateArray;
        }
    }
}
