using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpDX.Direct3D12
{
    /// <summary>	
    /// <p>Describes a portion of a texture for the purpose of texture copies. </p>	
    /// </summary>	
    /// <remarks>	
    /// <p> Use this structure with <strong>CopyTextureRegion</strong>. </p>	
    /// </remarks>	
    /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION']/*"/>	
    /// <msdn-id>dn903818</msdn-id>	
    /// <unmanaged>D3D12_TEXTURE_COPY_LOCATION</unmanaged>	
    /// <unmanaged-short>D3D12_TEXTURE_COPY_LOCATION</unmanaged-short>	
    public partial struct TextureCopyLocation
    {
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::pResource']/*"/>	
        /// <unmanaged>ID3D12Resource* pResource</unmanaged>	
        /// <unmanaged-short>ID3D12Resource pResource</unmanaged-short>	
        private System.IntPtr PResource;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::Type']/*"/>	
        /// <unmanaged>D3D12_TEXTURE_COPY_TYPE Type</unmanaged>	
        /// <unmanaged-short>D3D12_TEXTURE_COPY_TYPE Type</unmanaged-short>	
        public SharpDX.Direct3D12.TextureCopyType Type;

        private Union union;

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::PlacedFootprint']/*"/>	
        /// <unmanaged>D3D12_PLACED_SUBRESOURCE_FOOTPRINT PlacedFootprint</unmanaged>	
        /// <unmanaged-short>D3D12_PLACED_SUBRESOURCE_FOOTPRINT PlacedFootprint</unmanaged-short>	
        public SharpDX.Direct3D12.PlacedSubResourceFootprint PlacedFootprint
        {
            get { return union.PlacedFootprint; }
            set { union.PlacedFootprint = value; }
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='D3D12_TEXTURE_COPY_LOCATION::SubresourceIndex']/*"/>	
        /// <unmanaged>unsigned int SubresourceIndex</unmanaged>	
        /// <unmanaged-short>unsigned int SubresourceIndex</unmanaged-short>	
        public int SubresourceIndex
        {
            get { return union.SubResourceIndex; }
            set { union.SubResourceIndex = value; }
        }

        /// <summary>
        /// Creates a new resource copy that is represented by a subresource index
        /// </summary>
        /// <param name="resource">A valid Direct3D12 Resource</param>
        /// <param name="subResourceIndex">Sub resource index</param>
        public TextureCopyLocation(SharpDX.Direct3D12.Resource resource, int subResourceIndex) : this()
        {
            Type = TextureCopyType.SubResourceIndex;
            PResource = resource != null ? resource.NativePointer : IntPtr.Zero;
            SubresourceIndex = subResourceIndex;
        }

        /// <summary>
        /// Creates a new resource copy that is represented by a resource footprint
        /// </summary>
        /// <param name="resource">A valid Direct3D12 Resource</param>
        /// <param name="placedFootprint">Placement footprint for the resource copy operation</param>
        public TextureCopyLocation(SharpDX.Direct3D12.Resource resource, PlacedSubResourceFootprint placedFootprint) : this()
        {
            Type = TextureCopyType.PlacedFootprint;
            PResource = resource != null ? resource.NativePointer : IntPtr.Zero;
            PlacedFootprint = placedFootprint;
        }

        /// <summary>
        /// Because this union contains pointers, it is aligned on 8 bytes boundary, making the field ResourceBarrier.Type 
        /// to be aligned on 8 bytes instead of 4 bytes, so we can't use directly Explicit layout on ResourceBarrier
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private partial struct Union
        {
            [FieldOffset(0)]
            public SharpDX.Direct3D12.PlacedSubResourceFootprint PlacedFootprint;

            [FieldOffset(0)]
            public int SubResourceIndex;
        }
    }
}
