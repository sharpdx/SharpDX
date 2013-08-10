namespace TiledResources.Effects
{
    using SharpDX.Toolkit.Graphics;

    internal sealed class ResidencyViewerEffect : ViewerEffectBase
    {
        public ResidencyViewerEffect(GraphicsDevice device, EffectData effectData)
            : base(device, effectData)
        {
        }

        protected override SamplerState GetSampler()
        {
            return GraphicsDevice.SamplerStates.PointWrap;
        }
    }
}