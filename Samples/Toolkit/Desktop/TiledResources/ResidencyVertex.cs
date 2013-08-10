namespace TiledResources
{
    using System.Runtime.InteropServices;
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Vertex structure used to render residency visualization
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ResidencyVertex
    {
        /// <summary>
        /// Vertex position
        /// </summary>
        [VertexElement("POSITION")]
        public readonly Vector2 Position;

        /// <summary>
        /// Texture coordinate
        /// </summary>
        [VertexElement("TEXCOORD")]
        public readonly Vector4 Texcoord;

        public ResidencyVertex(float x, float y, float u, float v, float w, float i)
        {
            Position = new Vector2(x, y);
            Texcoord = new Vector4(u, v, w, i);
        }
    }
}