namespace TiledResources
{
    using System.Collections.Generic;
    using SharpDX.Direct3D11;

    /// <summary>
    /// Groups all needed information about tile mapping update operations
    /// </summary>
    internal sealed class TileMappingUpdateArguments
    {
        public readonly List<TiledResourceCoordinate> Coordinates = new List<TiledResourceCoordinate>();
        public readonly List<TileRangeFlags> RangeFlags = new List<TileRangeFlags>();
        public readonly List<int> PhysicalOffsets = new List<int>();
        public readonly List<TrackedTile> TilesToMap = new List<TrackedTile>();
    }
}