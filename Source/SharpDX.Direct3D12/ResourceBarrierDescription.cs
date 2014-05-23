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
namespace SharpDX.Direct3D12
{
    public partial struct ResourceBarrierDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBarrierDescription"/> struct.
        /// </summary>
        /// <param name="transition">The transition.</param>
        public ResourceBarrierDescription(ResourceTransitionBarrierDescription transition) : this()
        {
            Type = ResourceBarrierType.Transition;
            Transition = transition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBarrierDescription"/> struct.
        /// </summary>
        /// <param name="aliasing">The aliasing.</param>
        public ResourceBarrierDescription(ResourceAliasingBarrierDescription aliasing)
            : this()
        {
            Type = ResourceBarrierType.Aliasing;
            Aliasing = aliasing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBarrierDescription"/> struct.
        /// </summary>
        /// <param name="unorderedAccessView">The unordered access view.</param>
        public ResourceBarrierDescription(ResourceUnorderedAccessViewBarrierDescription unorderedAccessView)
            : this()
        {
            Type = ResourceBarrierType.UnorderedAccessView;
            UnorderedAccessView = unorderedAccessView;
        }

        public static implicit operator ResourceBarrierDescription(ResourceTransitionBarrierDescription description)
        {
            return new ResourceBarrierDescription(description);
        }

        public static implicit operator ResourceBarrierDescription(ResourceAliasingBarrierDescription description)
        {
            return new ResourceBarrierDescription(description);
        }

        public static implicit operator ResourceBarrierDescription(ResourceUnorderedAccessViewBarrierDescription description)
        {
            return new ResourceBarrierDescription(description);
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D12_RESOURCE_BARRIER_TYPE Type</unmanaged>	
        /// <unmanaged-short>D3D12_RESOURCE_BARRIER_TYPE Type</unmanaged-short>	
        public SharpDX.Direct3D12.ResourceBarrierType Type;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D12_RESOURCE_TRANSITION_BARRIER_DESC Transition</unmanaged>	
        /// <unmanaged-short>D3D12_RESOURCE_TRANSITION_BARRIER_DESC Transition</unmanaged-short>	
        private SharpDX.Direct3D12.ResourceTransitionBarrierDescription transition;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D12_RESOURCE_TRANSITION_BARRIER_DESC Transition</unmanaged>	
        /// <unmanaged-short>D3D12_RESOURCE_TRANSITION_BARRIER_DESC Transition</unmanaged-short>	
        public SharpDX.Direct3D12.ResourceTransitionBarrierDescription Transition
        {
            get { return transition; }
            set { transition = value; }
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D12_RESOURCE_ALIASING_BARRIER_DESC Aliasing</unmanaged>	
        /// <unmanaged-short>D3D12_RESOURCE_ALIASING_BARRIER_DESC Aliasing</unmanaged-short>	
        public unsafe SharpDX.Direct3D12.ResourceAliasingBarrierDescription Aliasing
        {
            get
            {
                // CAUTION: Handle manually the union as it is not possible to use explicit layout for x86/x64 as
                // ResourceTransitionBarrierDescription contains IntPtr
                fixed(void* pTransition = &transition)
                    return *(ResourceAliasingBarrierDescription*)pTransition;
            }

            set
            {
                fixed(void* pTransition = &transition)
                    *(ResourceAliasingBarrierDescription*)pTransition = value;
            }
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>D3D12_RESOURCE_UAV_BARRIER_DESC UAV</unmanaged>	
        /// <unmanaged-short>D3D12_RESOURCE_UAV_BARRIER_DESC UAV</unmanaged-short>	
        public unsafe SharpDX.Direct3D12.ResourceUnorderedAccessViewBarrierDescription UnorderedAccessView
        {
            get
            {
                // CAUTION: Handle manually the union as it is not possible to use explicit layout for x86/x64 as
                // ResourceTransitionBarrierDescription contains IntPtr
                fixed (void* pTransition = &transition)
                    return *(ResourceUnorderedAccessViewBarrierDescription*)pTransition;
            }

            set
            {
                fixed (void* pTransition = &transition)
                    *(ResourceUnorderedAccessViewBarrierDescription*)pTransition = value;
            }
        }
    }
}