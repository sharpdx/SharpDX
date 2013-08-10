namespace TiledResources
{
    using System.Runtime.InteropServices;
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    [StructLayout(LayoutKind.Sequential)]
    internal struct TerrainVertex
    {
        [VertexElement("POSITION")]
        public readonly Vector3 Position;

        public TerrainVertex(Vector3 position)
        {
            Position = position;
        }
    }
}