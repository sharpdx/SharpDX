using System;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Base class for reading audio content.
    /// </summary>
    /// <typeparam name="TAsset">The audio asset type.</typeparam>
    public abstract class AudioContentReader<TAsset> : IContentReader
    {
        /// <summary>
        /// Reads the content and performs any common precondition checks.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="parameters">The content reader parameters.</param>
        /// <returns>The loaded audio content object.</returns>
        /// <exception cref="ArgumentNullException">Is thrown when <paramref name="contentManager"/> is null.</exception>
        /// <exception cref="ArgumentException">Is thrown when <paramref name="parameters"/> <see cref="ContentReaderParameters.AssetType"/> is not <typeparamref name="TAsset"/></exception>
        /// <exception cref="InvalidOperationException">Is thrown when <paramref name="contentManager"/> <see cref="IContentManager.ServiceProvider"/> doesn't contain an <see cref="AudioManager"/> service.</exception>
        public object ReadContent(IContentManager contentManager, ref ContentReaderParameters parameters)
        {
            if (contentManager == null) throw new ArgumentNullException("contentManager");

            if (parameters.AssetType != typeof(TAsset))
            {
                var message = string.Format("Invalid asset type, expected {0} but got {1}", typeof(TAsset), parameters.AssetType);
                throw new ArgumentException(message, "parameters");
            }

            var audioManager = contentManager.ServiceProvider.GetService(typeof(AudioManager)) as AudioManager;
            if (audioManager == null)
                throw new InvalidOperationException("Cannot read WaveBank without AudioManager.");

            return ReadContentInternal(audioManager, ref parameters);
        }

        /// <summary>
        /// Derived class must implement this method to perform the actual asset reading.
        /// </summary>
        /// <param name="audioManager">The associated audio manager.</param>
        /// <param name="parameters">The content reader parameters containing the asset data stream.</param>
        /// <returns>The loaded asset.</returns>
        protected abstract TAsset ReadContentInternal(AudioManager audioManager, ref ContentReaderParameters parameters);
    }
}