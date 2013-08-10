namespace TiledResources
{
    using System;
    using SharpDX.Direct3D11;

    /// <summary>
    /// Uniquely indentifies a tile at the specified coordinate for the specific texture resource
    /// </summary>
    internal struct TileKey : IEquatable<TileKey>
    {
        public readonly TiledResourceCoordinate Coordinate;
        public readonly SharpDX.Toolkit.Graphics.Texture2DBase Resource;

        public TileKey(TiledResourceCoordinate coordinate, SharpDX.Toolkit.Graphics.Texture2DBase resource)
        {
            Coordinate = coordinate;
            Resource = resource;
        }

        public bool Equals(TileKey other)
        {
            return Equals(Resource, other.Resource)
                   && Coordinate.X == other.Coordinate.X
                   && Coordinate.Y == other.Coordinate.Y
                   && Coordinate.Z == other.Coordinate.Z
                   && Coordinate.Subresource == other.Coordinate.Subresource;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TileKey && Equals((TileKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Resource != null ? Resource.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Coordinate.X;
                hashCode = (hashCode * 397) ^ Coordinate.Y;
                hashCode = (hashCode * 397) ^ Coordinate.Z;
                hashCode = (hashCode * 397) ^ Coordinate.Subresource;
                return hashCode;
            }
        }

        public static bool operator ==(TileKey left, TileKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileKey left, TileKey right)
        {
            return !left.Equals(right);
        }
    }
}