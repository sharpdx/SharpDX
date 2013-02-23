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
        /// <returns>Returnss an array of <strong><see cref="SharpDX.MediaFoundation.Activate"/></strong> objects. Each object represents an activation object for an MFT that matches the search criteria. The function allocates the memory for the array. The caller must release the pointers and call the Dispose for each element in the array.</returns>
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
                    activates[i] = CppObject.FromPointer<Activate>(ptr[i]);
                }
            }
            Marshal.FreeCoTaskMem(pActivatesArr);

            return activates;
        }
    }
}
#endif