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
            IntPtr pActivatesArr;
            int pNumActivates;
            TEnumEx(guidCategory, (int)enumFlags, inputTypeRef, outputTypeRef, out pActivatesArr, out pNumActivates);

            var activates = new Activate[pNumActivates];
            unsafe
            {
                IntPtr* ptr = (IntPtr*)(pActivatesArr);
                for (int i = 0; i < pNumActivates; i++)
                {
                    activates[i] = new Activate(*ptr);
                    ptr++;
                }
            }
            Marshal.FreeCoTaskMem(pActivatesArr);

            return activates;
        }
    }
}
