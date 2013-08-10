namespace TiledResources.Effects
{
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    internal sealed class SamplingRendererEffect : TerrainEffectBase
    {
        private EffectConstantBuffer _pixelShaderConstantBuffer;
        private EffectParameter _encodeConstantsParameter;

        public SamplingRendererEffect(GraphicsDevice device, EffectData effectData)
            : base(device, effectData)
        {
        }

        public Vector2 EncodeConstants
        {
            get { return _encodeConstantsParameter.GetValue<Vector2>(); }
            set
            {
                _encodeConstantsParameter.SetValue(value);
                _pixelShaderConstantBuffer.IsDirty = true;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            _pixelShaderConstantBuffer = ConstantBuffers["SamplingConstants"];
            _encodeConstantsParameter = _pixelShaderConstantBuffer.Parameters["EncodeConstants"];
        }

        protected override EffectPass OnApply(EffectPass pass)
        {
            _pixelShaderConstantBuffer.Update();

            return base.OnApply(pass);
        }
    }
}