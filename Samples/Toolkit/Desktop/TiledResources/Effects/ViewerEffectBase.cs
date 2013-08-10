namespace TiledResources.Effects
{
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    internal abstract class ViewerEffectBase : Effect
    {
        private EffectParameter _textureParameter;
        private EffectConstantBuffer _constantBuffer;

        private EffectParameter _scaleParameter;
        private EffectParameter _offsetParameter;

        protected ViewerEffectBase(GraphicsDevice device, EffectData effectData)
            : base(device, effectData, null)
        {
        }

        public Texture2DBase Texture { get { return _textureParameter.GetResource<Texture2DBase>(); } set { _textureParameter.SetResource(value); } }

        public Vector2 Scale
        {
            get { return _scaleParameter.GetValue<Vector2>(); }
            set
            {
                _scaleParameter.SetValue(value);
                _constantBuffer.IsDirty = true;
            }
        }

        public Vector2 Offset
        {
            get { return _offsetParameter.GetValue<Vector2>(); }
            set
            {
                _offsetParameter.SetValue(value);
                _constantBuffer.IsDirty = true;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            _constantBuffer = ConstantBuffers["CB0"];

            _scaleParameter = _constantBuffer.Parameters["scale"];
            _offsetParameter = _constantBuffer.Parameters["offset"];

            _textureParameter = Parameters["tex"];

            Parameters["sam"].SetResource(GetSampler());
        }

        protected override EffectPass OnApply(EffectPass pass)
        {
            _constantBuffer.Update();

            return base.OnApply(pass);
        }

        protected abstract SamplerState GetSampler();
    }
}