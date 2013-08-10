namespace TiledResources
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Effects;
    using SharpDX;
    using SharpDX.DXGI;
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Content;

    /// <summary>
    /// Renders the terrain from this sample project
    /// </summary>
    internal sealed class TerrainRenderer : Component
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly IContentManager _content;

        private float _sunx = 0f;
        private float _suny = 0f;

        private SharpDX.Toolkit.Graphics.GeometricPrimitive<TerrainVertex> _terrainGeometry;
        private TerrainRendererEffect _terrainRendererEffect;

        private SharpDX.Toolkit.Graphics.TextureCube _diffuseTexture;
        private SharpDX.Toolkit.Graphics.TextureCube _normalTexture;

        private SharpDX.Toolkit.Graphics.Texture2DBase _diffuseResidency;
        private SharpDX.Toolkit.Graphics.Texture2DBase _normalResidency;

        public TerrainRenderer(GraphicsDeviceManager graphicsDeviceManager, IContentManager content)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            _content = content;
        }

        public SharpDX.Toolkit.Graphics.TextureCube DiffuseTexture { get { return _diffuseTexture; } }
        public SharpDX.Toolkit.Graphics.TextureCube NormalTexture { get { return _normalTexture; } }

        public SharpDX.Toolkit.Graphics.Texture2DBase DiffuseResidencyMap { set { _diffuseResidency = value; } }
        public SharpDX.Toolkit.Graphics.Texture2DBase NormalResidencyMap { set { _normalResidency = value; } }

        public TerrainEffectBase TerrainEffect { get { return _terrainRendererEffect; } }

        /// <summary>
        /// Prepares the render targets and pixel shader for rendering
        /// </summary>
        /// <param name="camera">A camera providing the transformation matrices</param>
        public void SetTargetsForRendering(Camera camera)
        {
            // update sun position and the displacement scale factor
            var data = new Vector4(Vector3.Normalize(new Vector3((float)Math.Sin(_sunx), (float)Math.Cos(_sunx), _suny)),
                                   TerrainEffectBase.DefaultScaleFactor);

            var device = _graphicsDeviceManager.GraphicsDevice;
            device.SetRenderTargets(device.DepthStencilBuffer, device.BackBuffer);

            device.Clear(SharpDX.Toolkit.Graphics.ClearOptions.DepthBuffer | SharpDX.Toolkit.Graphics.ClearOptions.Target, new Color4(0), 1f, 0);

            var context = _graphicsDeviceManager.GetContext2();
            context.Rasterizer.SetViewport(device.Viewport);

            _terrainRendererEffect.ColorTexture = _diffuseTexture;
            _terrainRendererEffect.ColorResidency = _diffuseResidency;
            _terrainRendererEffect.NormalTexture = _normalTexture;
            _terrainRendererEffect.NormalResidency = _normalResidency;
            _terrainRendererEffect.SunPosition = data;
        }

        /// <summary>
        /// Draw the prepared geometry
        /// </summary>
        public void Draw(SharpDX.Toolkit.Graphics.Effect effect = null)
        {
            _terrainGeometry.Draw(effect ?? _terrainRendererEffect);
        }

        public Task CreateTexturesAsync()
        {
            return Task.Factory.StartNew(CreateTextures);
        }

        public Task LoadStructuresAsync()
        {
            return Task.Factory.StartNew(LoadStructures);
        }

        private void CreateTextures()
        {
            _diffuseTexture = CreateCubeTexture(SampleSettings.TerrainAssets.Diffuse.DimensionSize,
                                                SampleSettings.TerrainAssets.Diffuse.UnpackedMipCount,
                                                SampleSettings.TerrainAssets.Diffuse.Format);

            _normalTexture = CreateCubeTexture(SampleSettings.TerrainAssets.Normal.DimensionSize,
                                               SampleSettings.TerrainAssets.Normal.UnpackedMipCount,
                                               SampleSettings.TerrainAssets.Normal.Format);
        }

        private void LoadStructures()
        {
            _terrainRendererEffect = _content.Load<TerrainRendererEffect>("TerrainRenderer_Tier1");

            var vertices = ReadFile(Path.Combine(SampleSettings.DataPath, SampleSettings.VertexBufferFile),
                                    r => new TerrainVertex(new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle())));

            var indices = ReadFile(Path.Combine(SampleSettings.DataPath, SampleSettings.IndexBufferFile),
                                   r => r.ReadInt32());

            _terrainGeometry = new SharpDX.Toolkit.Graphics.GeometricPrimitive<TerrainVertex>(_graphicsDeviceManager.GraphicsDevice,
                                                                                              vertices,
                                                                                              indices,
                                                                                              false);
        }

        private static T[] ReadFile<T>(string filepath, Func<BinaryReader, T> factory)
            where T : struct
        {
            using (var stream = File.OpenRead(filepath))
            using (var r = new BinaryReader(stream))
            {
                var dataLength = stream.Length / Marshal.SizeOf(typeof(T));
                var data = new T[dataLength];

                for (var i = 0; i < dataLength; i++)
                    data[i] = factory(r);

                return data;
            }
        }

        private SharpDX.Toolkit.Graphics.TextureCube CreateCubeTexture(int size, int mipLevels, Format format)
        {
            var description = new SharpDX.Toolkit.Graphics.TextureDescription
                              {
                                  Width = size,
                                  Height = size,
                                  MipLevels = mipLevels,
                                  Format = format,
                                  ArraySize = 6,
                                  SampleDescription = new SampleDescription(1, 0),
                                  Usage = SharpDX.Direct3D11.ResourceUsage.Default,
                                  BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                                  OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.TextureCube | SharpDX.Direct3D11.ResourceOptionFlags.Tiled
                              };

            return ToDispose(SharpDX.Toolkit.Graphics.TextureCube.New(_graphicsDeviceManager.GraphicsDevice, description));
        }
    }
}