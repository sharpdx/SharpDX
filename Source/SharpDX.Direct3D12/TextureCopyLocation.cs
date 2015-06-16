using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.Direct3D12
{
    /// <summary>	
    /// No documentation for Direct3D12	
    /// </summary>	
    /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION']/*"/>	
    /// <unmanaged>D3D12_TEXTURE_COPY_LOCATION</unmanaged>	
    /// <unmanaged-short>D3D12_TEXTURE_COPY_LOCATION</unmanaged-short>	
    public partial struct TextureCopyLocation
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::pResource']/*"/>	
        /// <unmanaged>ID3D12Resource* pResource</unmanaged>	
        /// <unmanaged-short>ID3D12Resource pResource</unmanaged-short>	
        public System.IntPtr PResource;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::Type']/*"/>	
        /// <unmanaged>D3D12_TEXTURE_COPY_TYPE Type</unmanaged>	
        /// <unmanaged-short>D3D12_TEXTURE_COPY_TYPE Type</unmanaged-short>	
        public SharpDX.Direct3D12.TextureCopyType Type;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::PlacedFootprint']/*"/>	
        /// <unmanaged>D3D12_PLACED_SUBRESOURCE_FOOTPRINT PlacedFootprint</unmanaged>	
        /// <unmanaged-short>D3D12_PLACED_SUBRESOURCE_FOOTPRINT PlacedFootprint</unmanaged-short>	
        public SharpDX.Direct3D12.PlacedSubResourceFootprint PlacedFootprint;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::SubresourceIndex']/*"/>	
        /// <unmanaged>unsigned int SubresourceIndex</unmanaged>	
        /// <unmanaged-short>unsigned int SubresourceIndex</unmanaged-short>	
        public int SubresourceIndex;
    }
}
