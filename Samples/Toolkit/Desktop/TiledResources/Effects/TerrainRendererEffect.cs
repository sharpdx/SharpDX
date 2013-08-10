namespace TiledResources.Effects
{
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    internal sealed class TerrainRendererEffect : TerrainEffectBase
    {
        private EffectParameter _colorTextureParameter;
        private EffectParameter _colorResidencyParameter;
        private EffectParameter _normalTextureParameter;
        private EffectParameter _normalResidencyParameter;

        private EffectConstantBuffer _pixelShaderConstantBuffer;
        private EffectParameter _sunPositionParameter;

        public TerrainRendererEffect(GraphicsDevice device, EffectData effectData)
            : base(device, effectData)
        {
        }

        public Texture2DBase ColorTexture { get { return _colorTextureParameter.GetResource<Texture2DBase>(); } set { _colorTextureParameter.SetResource(value); } }
        public Texture2DBase ColorResidency { get { return _colorResidencyParameter.GetResource<Texture2DBase>(); } set { _colorResidencyParameter.SetResource(value); } }
        public Texture2DBase NormalTexture { get { return _normalTextureParameter.GetResource<Texture2DBase>(); } set { _normalTextureParameter.SetResource(value); } }
        public Texture2DBase NormalResidency { get { return _normalResidencyParameter.GetResource<Texture2DBase>(); } set { _normalResidencyParameter.SetResource(value); } }

        public Vector4 SunPosition
        {
            get { return _sunPositionParameter.GetValue<Vector4>(); }
            set
            {
                _sunPositionParameter.SetValue(value);
                _pixelShaderConstantBuffer.IsDirty = true;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            _colorTextureParameter = Parameters["ColorTexture"];
            _colorResidencyParameter = Parameters["ColorResidency"];
            _normalTextureParameter = Parameters["NormalTexture"];
            _normalResidencyParameter = Parameters["NormalResidency"];

            Parameters["Trilinear"].SetResource(GraphicsDevice.SamplerStates.AnisotropicWrap);
            Parameters["MaxFilter"].SetResource(GraphicsDevice.SamplerStates.LinearWrap);

            _pixelShaderConstantBuffer = ConstantBuffers["PixelShaderConstants"];

            _sunPositionParameter = _pixelShaderConstantBuffer.Parameters["SunPosition"];
        }

        protected override EffectPass OnApply(EffectPass pass)
        {
            _pixelShaderConstantBuffer.Update();

            return base.OnApply(pass);
        }
    }
}