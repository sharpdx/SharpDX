using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// An <see cref="AudioContentReader{TAsset}"/> implementation for <see cref="SoundEffect"/> content.
    /// </summary>
    public class SoundEffectContentReader : AudioContentReader<SoundEffect>
    {
        protected override SoundEffect ReadContentInternal(AudioManager audioManager, ref ContentReaderParameters parameters)
        {
            return SoundEffect.FromStream(audioManager, parameters.Stream, parameters.AssetName);
        }
    }
}