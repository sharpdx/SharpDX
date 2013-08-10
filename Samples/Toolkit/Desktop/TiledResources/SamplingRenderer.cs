namespace TiledResources
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Effects;
    using SharpDX;
    using SharpDX.Toolkit;
    using SharpDX.Toolkit.Content;

    /// <summary>
    /// Performs the sampling of the tiles that need to be loaded and renders the corresponding visualization
    /// </summary>
    internal sealed class SamplingRenderer : Component
    {
        private readonly Random _rand = new Random();

        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly IContentManager _contentManager;

        private SharpDX.Toolkit.Graphics.RenderTarget2D _colorTextureRenderTarget;

        private SharpDX.Toolkit.Graphics.DepthStencilBuffer _depthStencilBuffer;

        private SamplingViewerEffect _viewerEffect;
        private SamplingRendererEffect _samplingEffect;
        private SharpDX.Toolkit.Graphics.GeometricPrimitive<SamplingVertex> _viewerGeometry;

        private ViewportF _viewport;

        private int _sampleCount;

        private uint[] _samplingDataBuffer;

        private readonly List<DecodedSample> _decodedSamplesBuffer = new List<DecodedSample>();

        public SamplingRenderer(GraphicsDeviceManager graphicsDeviceManager, IContentManager contentManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
            _contentManager = contentManager;
        }

        public TerrainEffectBase SamplingEffect { get { return _samplingEffect; } }

        /// <summary>
        /// Prepares the render targets and pixel shaders for sampling
        /// </summary>
        public void SetTargetsForSampling()
        {
            var device = _graphicsDeviceManager.GraphicsDevice;
            device.SetRenderTargets(_depthStencilBuffer, _colorTextureRenderTarget);
            device.Clear(new Color4(0));
            device.SetViewport(_viewport);
        }

        /// <summary>
        /// Renders the sampling visualization
        /// </summary>
        public void RenderVisualisation()
        {
            var device = _graphicsDeviceManager.GraphicsDevice;
            device.SetRenderTargets(device.BackBuffer);

            _viewerEffect.Texture = _colorTextureRenderTarget;

            _viewerGeometry.Draw(_viewerEffect);
        }

        /// <summary>
        /// Collects the samples for tile loading
        /// </summary>
        /// <returns>The list of decoded samples</returns>
        public List<DecodedSample> CollectSamples()
        {
            int row;
            int column;
            _sampleCount = 0;

            // allocate the bufer only once
            if (_samplingDataBuffer == null)
                _samplingDataBuffer = _colorTextureRenderTarget.GetData<uint>();
            else
                _colorTextureRenderTarget.GetData(_samplingDataBuffer);

            // calculate the width of one row from the texture data
            var width = _colorTextureRenderTarget.CalculateWidth<uint>();

            // reuse the same list to avoid allocations
            _decodedSamplesBuffer.Clear();

            // pick a random sample position and decode it
            while (GetNextSamplePosition(out row, out column))
            {
                var sample = _samplingDataBuffer[row * width + column];

                // if the sample is not empty - decode it
                if (sample != 0)
                    _decodedSamplesBuffer.Add(DecodeSample(sample));
            }

            return _decodedSamplesBuffer;
        }

        public Task LoadStructuresAsync()
        {
            return Task.Factory.StartNew(LoadStructures);
        }

        /// <summary>
        /// Gets a random sample position
        /// </summary>
        /// <param name="row">The sample row.</param>
        /// <param name="column">The sample column.</param>
        /// <returns>True if the sampling should continue.</returns>
        private bool GetNextSamplePosition(out int row, out int column)
        {
            row = _rand.Next((int)_viewport.Height);
            column = _rand.Next((int)_viewport.Width);

            return ++_sampleCount <= SampleSettings.Sampling.SamplesPerFrame;
        }

        /// <summary>
        /// Decodes the sample information from a texture pixel.
        /// </summary>
        /// <param name="encodedSample">The texture pixel value.</param>
        /// <returns>The decoded sample.</returns>
        private DecodedSample DecodeSample(uint encodedSample)
        {
            var sampleB = (byte)(encodedSample & 0xFF);
            var sampleG = (byte)((encodedSample >> 8) & 0xFF);
            var sampleR = (byte)((encodedSample >> 16) & 0xFF);
            var sampleA = (byte)((encodedSample >> 24) & 0xFF);
            var x = 2.0f * sampleR / 255.0f - 1.0f;
            var y = 2.0f * sampleG / 255.0f - 1.0f;
            var z = 2.0f * sampleB / 255.0f - 1.0f;
            var lod = sampleA / 255.0f * 16.0f;
            var mip = lod < 0.0f ? (short)0 : lod > 14.0f ? (short)14 : (short)lod;
            short face;
            float u;
            float v;
            if (Math.Abs(x) > Math.Abs(y) && Math.Abs(x) > Math.Abs(z))
            {
                if (x > 0) // +X
                {
                    face = 0;
                    u = (1.0f - z / x) / 2.0f;
                    v = (1.0f - y / x) / 2.0f;
                }
                else // -X
                {
                    face = 1;
                    u = (z / -x + 1.0f) / 2.0f;
                    v = (1.0f - y / -x) / 2.0f;
                }
            }
            else if (Math.Abs(y) > Math.Abs(x) && Math.Abs(y) > Math.Abs(z))
            {
                if (y > 0) // +Y
                {
                    face = 2;
                    u = (x / y + 1.0f) / 2.0f;
                    v = (z / y + 1.0f) / 2.0f;
                }
                else // -Y
                {
                    face = 3;
                    u = (x / -y + 1.0f) / 2.0f;
                    v = (1.0f - z / -y) / 2.0f;
                }
            }
            else
            {
                if (z > 0) // +Z
                {
                    face = 4;
                    u = (x / z + 1.0f) / 2.0f;
                    v = (1.0f - y / z) / 2.0f;
                }
                else // -Z
                {
                    face = 5;
                    u = (1.0f - x / -z) / 2.0f;
                    v = (1.0f - y / -z) / 2.0f;
                }
            }

            return new DecodedSample(u, v, mip, face);
        }

        private void LoadStructures()
        {
            var device = _graphicsDeviceManager.GraphicsDevice;

            var vertexBufferData = new[]
                                   {
                                       new SamplingVertex(-1.0f, 1.0f, 0.0f, 0.0f),
                                       new SamplingVertex(1.0f, 1.0f, 1.0f, 0.0f),
                                       new SamplingVertex(-1.0f, -1.0f, 0.0f, 1.0f),
                                       new SamplingVertex(1.0f, -1.0f, 1.0f, 1.0f)
                                   };

            var indexBufferData = new[] { 0, 1, 2, 3, 2, 1 };

            _viewerGeometry = ToDispose(new SharpDX.Toolkit.Graphics.GeometricPrimitive<SamplingVertex>(device, vertexBufferData, indexBufferData, false));

            var screenViewport = new ViewportF(0f, 0f, device.BackBuffer.Width, device.BackBuffer.Height);
            var targetWidth = (int)(screenViewport.Width / SampleSettings.Sampling.Ratio);
            var targetHeight = (int)(screenViewport.Height / SampleSettings.Sampling.Ratio);

            _viewport = new ViewportF(0f, 0f, targetWidth, targetHeight, 0f, 1f);

            _colorTextureRenderTarget = ToDispose(SharpDX.Toolkit.Graphics.RenderTarget2D.New(device,
                                                                                              targetWidth,
                                                                                              targetHeight,
                                                                                              SharpDX.Toolkit.Graphics.PixelFormat.B8G8R8A8.UNorm));

            _depthStencilBuffer = ToDispose(SharpDX.Toolkit.Graphics.DepthStencilBuffer.New(device,
                                                                                            targetWidth,
                                                                                            targetHeight,
                                                                                            SharpDX.Toolkit.Graphics.DepthFormat.Depth24Stencil8));

            _viewerEffect = _contentManager.Load<SamplingViewerEffect>("SamplingViewer");

            _viewerEffect.Scale = new Vector2(_viewport.Width / screenViewport.Width,
                                              _viewport.Height / screenViewport.Height);

            _viewerEffect.Offset = new Vector2(2f * (48.0f + _viewport.Width / 2f) / screenViewport.Width - 1f,
                                               1f - 2f * (screenViewport.Height - _viewport.Height - 48.0f + _viewport.Height / 2f) / screenViewport.Height);

            _samplingEffect = _contentManager.Load<SamplingRendererEffect>("SamplingRenderer");

            _samplingEffect.EncodeConstants = new Vector2(SampleSettings.TileSizeInBytes / SampleSettings.Sampling.Ratio, 24f);
        }
    }
}