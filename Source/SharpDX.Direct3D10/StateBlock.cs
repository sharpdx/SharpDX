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
    public partial class StateBlock
    {

        /// <summary>	
        /// Create a state block.	
        /// </summary>	
        /// <remarks>	
        /// A state block is a collection of device state, and is used for saving and restoring device state. Use a state-block mask to enable subsets of state for saving and restoring. The <see cref="SharpDX.Direct3D10.StateBlockMask"/> structure can be filled manually or by using any of the D3D10StateBlockMaskXXX APIs. A state block mask can also be obtained by calling <see cref="SharpDX.Direct3D10.EffectTechnique.ComputeStateBlockMask"/> or <see cref="SharpDX.Direct3D10.EffectPass.ComputeStateBlockMask"/>.   Differences between Direct3D 9 and Direct3D 10: In Direct3D 10, a state block object does not contain any valid information about the state of the device until <see cref="SharpDX.Direct3D10.StateBlock.Capture"/> is called. In Direct3D 9, state is saved in a state block object, when it is created.   ? 	
        /// </remarks>	
        /// <param name="device">The device for which the state block will be created. </param>
        /// <param name="mask">Indicates which parts of the device state will be captured when calling <see cref="SharpDX.Direct3D10.StateBlock.Capture"/> and reapplied when calling <see cref="SharpDX.Direct3D10.StateBlock.Apply"/>. See remarks. </param>
        /// <unmanaged>HRESULT D3D10CreateStateBlock([None] ID3D10Device* pDevice,[None] D3D10_STATE_BLOCK_MASK* pStateBlockMask,[None] ID3D10StateBlock** ppStateBlock)</unmanaged>
        public StateBlock(Device device, StateBlockMask mask)
        {
            D3D10.CreateStateBlock(device, ref mask, this);
        }
    }
}