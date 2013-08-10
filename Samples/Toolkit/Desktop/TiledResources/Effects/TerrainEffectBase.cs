namespace TiledResources.Effects
{
    using SharpDX;
    using SharpDX.Toolkit.Graphics;

    internal abstract class TerrainEffectBase : Effect
    {
        public const float DefaultScaleFactor = 10f;

        private EffectConstantBuffer _vertexShaderConstantBuffer;

        private EffectParameter _viewMatrixParameter;
        private EffectParameter _projectionMatrixParameter;
        private EffectParameter _modelMatrixParameter;
        private EffectParameter _scaleFactorParameter;

        protected TerrainEffectBase(GraphicsDevice device, EffectData effectData)
            : base(device, effectData, null)
        {
        }

        public Matrix View
        {
            get { return _viewMatrixParameter.GetValue<Matrix>(); }
            set
            {
                _viewMatrixParameter.SetValue(value);
                _vertexShaderConstantBuffer.IsDirty = true;
            }
        }

        public Matrix Projection
        {
            get { return _projectionMatrixParameter.GetValue<Matrix>(); }
            set
            {
                _projectionMatrixParameter.SetValue(value);
                _vertexShaderConstantBuffer.IsDirty = true;
            }
        }

        public Matrix Model
        {
            get { return _modelMatrixParameter.GetValue<Matrix>(); }
            set
            {
                _modelMatrixParameter.SetValue(value);
                _vertexShaderConstantBuffer.IsDirty = true;
            }
        }

        public float ScaleFactor
        {
            get { return _scaleFactorParameter.GetValue<float>(); }
            set
            {
                _scaleFactorParameter.SetValue(value);
                _vertexShaderConstantBuffer.IsDirty = true;
            }
        }

        public void Update(Camera camera)
        {
            View = camera.View;
            Projection = camera.Projection;
            Model = Matrix.Identity;
            ScaleFactor = DefaultScaleFactor;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _vertexShaderConstantBuffer = ConstantBuffers["VertexShaderConstants"];

            _viewMatrixParameter = _vertexShaderConstantBuffer.Parameters["ViewMatrix"];
            _projectionMatrixParameter = _vertexShaderConstantBuffer.Parameters["ProjectionMatrix"];
            _modelMatrixParameter = _vertexShaderConstantBuffer.Parameters["ModelMatrix"];
            _scaleFactorParameter = _vertexShaderConstantBuffer.Parameters["scaleFactor"];
        }

        protected override EffectPass OnApply(EffectPass pass)
        {
            _vertexShaderConstantBuffer.Update();

            return base.OnApply(pass);
        }
    }
}