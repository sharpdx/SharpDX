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

namespace SharpDX.Direct3D10
{
    public partial class SkinInfo
    {
        /// <summary>	
        /// Get a list of vertices that a given bone influences and a list of the amount of influence that bone has on each vertex.	
        /// </summary>	
        /// <param name="boneIndex">An index that specifies an existing bone. Must be between 0 and the value returned by <see cref="M:SharpDX.Direct3D10.SkinInfo.GetNumBones" />. </param>
        /// <param name="offset">An offset from the top of the bone's list of influenced vertices. This must be between 0 and the value returned by <see cref="M:SharpDX.Direct3D10.SkinInfo.GetBoneInfluenceCount(System.Int32)" />. </param>
        /// <param name="count">The number of indices and weights to retrieve.  Must be between 0 and the value returned by ID3DX10SkinInfo::GetBoneInfluenceCount. </param>
        /// <param name="destIndicesRef">A list of indices into the vertex buffer, each one representing a vertex influenced by the bone. These values correspond to the values in pDestWeights, such that pDestIndices[i] corresponds to pDestWeights[i]. </param>
        /// <param name="destWeightsRef">A list of the amount of influence the bone has on each vertex. These values correspond to the values in pDestIndices, such that pDestWeights[i] corresponds to pDestIndices[i].f </param>
        /// <returns>If the method succeeds, the return value is S_OK. If the method fails, the return value can be: E_INVALIDARG or E_OUTOFMEMORY. </returns>
        /// <unmanaged>HRESULT ID3DX10SkinInfo::GetBoneInfluences([None] int BoneIndex,[None] int Offset,[None] int Count,[Out, Buffer] int* pDestIndices,[Out, Buffer] float* pDestWeights)</unmanaged>
        public void GetBoneInfluences(int boneIndex, int offset, int count, out int[] destIndicesRef, out float[] destWeightsRef)
        {
            destIndicesRef = new int[count];
            destWeightsRef = new float[count];
            GetBoneInfluences(boneIndex, offset, count, destIndicesRef, destWeightsRef);
        }
    }
}