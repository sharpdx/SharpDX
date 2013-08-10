namespace TiledResources
{
    using System.Collections.Generic;

    internal sealed class TileKeyEqualityComparer : IEqualityComparer<TileKey>
    {
        public bool Equals(TileKey x, TileKey y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(TileKey key)
        {
            return key.GetHashCode();
        }
    }
}