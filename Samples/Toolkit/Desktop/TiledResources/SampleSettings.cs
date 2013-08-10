namespace TiledResources
{
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Contains the global settings
    /// </summary>
    internal static class SampleSettings
    {
        /// <summary>
        /// Contains the settings for the tile residency
        /// </summary>
        public static class TileResidency
        {
            public const int PoolSizeInTiles = 512;
            public const int MaxSimultaneousFileLoadTasks = 16;
            public const int MaxTilesLoadedPerFrame = 100;
        }

        /// <summary>
        /// Contains the information about textures
        /// </summary>
        public static class TerrainAssets
        {
            public static class Diffuse
            {
                public const int DimensionSize = 16384;
                public static readonly PixelFormat Format = PixelFormat.BC1.UNorm;
                public const int UnpackedMipCount = 6;
            }

            public static class Normal
            {
                public const int DimensionSize = 16384;
                public static readonly PixelFormat Format = PixelFormat.BC5.SNorm;
                public const int UnpackedMipCount = 7;
            }
        }

        /// <summary>
        /// Contains the settings related to tile sampling
        /// </summary>
        public static class Sampling
        {
            public const float Ratio = 8f;
            public const int SamplesPerFrame = 100;
        }

        public const int TileSizeInBytes = 0x10000;

        public const string DataPath = @"..\..\..\..\..\..\..\SharpDXTilingResourcesSampleData\";
        public const string VertexBufferFile = @"geometry.vb.bin";
        public const string IndexBufferFile = @"geometry.ib.bin";
        public const string DiffuseTextureFile = @"diffuse.bin";
        public const string NormalTextureFile = @"normal.bin";
    }
}