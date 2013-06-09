using SharpDX;
using SharpDX.Toolkit.Graphics;
using System.Runtime.InteropServices;

namespace StreamOutParticles
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ParticleVertex
    {
        [VertexElement("POSITION")]
        public Vector3 Position;

        [VertexElement("NORMAL")]
        public Vector3 Velocity;

        [VertexElement("COLOR")]
        public Vector4 Color;

        [VertexElement("TEXCOORD0")]
        public Vector2 TimerLifetime;

        [VertexElement("TEXCOORD1")]
        public uint Flags;

        [VertexElement("TEXCOORD2")]
        public Vector2 SizeStartEnd;
    }
}