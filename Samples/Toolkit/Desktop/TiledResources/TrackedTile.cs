namespace TiledResources
{
    using System;
    using SharpDX.Direct3D11;

    /// <summary>
    /// Represents a tracked tile information
    /// </summary>
    internal sealed class TrackedTile
    {
        public readonly ManagedTiledResource ManagedTiledResource;
        public readonly TiledResourceCoordinate Coordinate;
        public readonly short MipLevel;
        public readonly short Face;
        public int PhysicalTileOffset;
        public uint lastSeen;
        public byte[] TileData;
        public TileState State;

        /// <summary>
        /// Used for ordering of tiles that needs to be loaded from disk
        /// </summary>
        public static readonly Comparison<TrackedTile> LoadComparison = Load;

        /// <summary>
        /// Used for ordering of tiles that needs to be mapped to GPU
        /// </summary>
        public static readonly Comparison<TrackedTile> MapComparison = Map;

        /// <summary>
        /// Used for ordering of tiles that can be evicted from GPU
        /// </summary>
        public static readonly Comparison<TrackedTile> EvictComparison = Evict;

        public TrackedTile(ManagedTiledResource managedTiledResource, TiledResourceCoordinate coordinate, short mipLevel, short face)
        {
            ManagedTiledResource = managedTiledResource;
            Coordinate = coordinate;
            MipLevel = mipLevel;
            Face = face;
        }

        private static int Load(TrackedTile a, TrackedTile b)
        {
            var result = a.lastSeen.CompareTo(b.lastSeen);
            return result != 0 ? result : a.MipLevel.CompareTo(b.MipLevel);
        }

        private static int Map(TrackedTile a, TrackedTile b)
        {
            if (a.State == TileState.Loaded && b.State == TileState.Loading) return 1;
            if (a.State == TileState.Loading && b.State == TileState.Loaded) return -1;

            return Load(a, b);
        }

        private static int Evict(TrackedTile a, TrackedTile b)
        {
            return -Load(b, a);
        }
    }
}