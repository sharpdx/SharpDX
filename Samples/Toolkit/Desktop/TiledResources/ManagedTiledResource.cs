namespace TiledResources
{
    using SharpDX.Direct3D11;

    /// <summary>
    /// Groups the information about a tiled texture
    /// </summary>
    internal sealed class ManagedTiledResource
    {
        /// <summary>
        /// The tiled texture instance
        /// </summary>
        public SharpDX.Toolkit.Graphics.TextureCube Texture;

        /// <summary>
        /// The total number of tiles in the current texture
        /// </summary>
        public int TotalTiles;

        /// <summary>
        /// The description of the packed mip maps
        /// </summary>
        public PackedMipDescription PackedMipDescription;

        /// <summary>
        /// The shape (width/height/depth) of one tile, in texels.
        /// </summary>
        public TileShape TileShape;

        /// <summary>
        /// The information about tiled subresources
        /// </summary>
        public SubResourceTiling[] SubresourceTilings;

        /// <summary>
        /// The tile loader instance
        /// </summary>
        public TileLoader Loader;

        /// <summary>
        /// Residency information, one array for each cube face
        /// </summary>
        public byte[][] Residency = new byte[6][];

        /// <summary>
        /// The residency texture contains the information about best mip levels that are loaded
        /// </summary>
        public SharpDX.Toolkit.Graphics.Texture2D ResidencyTexture;
    }
}