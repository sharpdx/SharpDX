using System;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// <see cref="IContentReaderFactory"/> implementation for audio content readers.
    /// </summary>
    public class AudioContentReaderFactory : IContentReaderFactory
    {
        /// <summary>
        /// Creates an appropiate content reader for the provided type.
        /// </summary>
        /// <param name="type">The content type.</param>
        /// <returns>The content reader instance that can read <paramref name="type"/> content, or null if content type is not supported.</returns>
        public IContentReader TryCreate(Type type)
        {
            if (type == typeof(WaveBank))
                return new WaveBankContentReader();

            if (type == typeof(SoundEffect))
                return new SoundEffectContentReader();

            return null;
        }
    }
}