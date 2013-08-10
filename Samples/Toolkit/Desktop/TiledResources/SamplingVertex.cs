namespace TiledResources
{
    using System.Runtime.InteropServices;
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Vertex structure used in sampling visualization rendering
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SamplingVertex
    {
        [VertexElement("POSITION")]
        public readonly Vector2 Position;

        [VertexElement("TEXCOORD")]
        public readonly Vector2 Texcoord;

        public SamplingVertex(float x, float y, float u, float v)
        {
            Position = new Vector2(x, y);
            Texcoord = new Vector2(u, v);
        }
    }
}