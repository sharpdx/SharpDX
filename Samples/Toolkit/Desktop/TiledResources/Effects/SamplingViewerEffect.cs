namespace TiledResources.Effects
{
    using SharpDX.Toolkit.Graphics;

    internal sealed class SamplingViewerEffect : ViewerEffectBase
    {
        public SamplingViewerEffect(GraphicsDevice device, EffectData effectData)
            : base(device, effectData)
        {
        }

        protected override SamplerState GetSampler()
        {
            return GraphicsDevice.SamplerStates.LinearClamp;
        }
    }
}