namespace ShaderLinking
{
    using System.Runtime.InteropServices;
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Vertex class for cube geometry
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vertex
    {
        [VertexElement("SV_POSITION")]
        public readonly Vector4 Position;

        [VertexElement("COLOR")]
        public readonly Vector4 Color;

        public Vertex(Vector3 position, Vector3 color)
        {
            Position = new Vector4(position, 1f);
            Color = new Vector4(color, 1f);
        }
    }
}