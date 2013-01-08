namespace SharpDX.Toolkit.Audio
{
    /// <summary>
    /// Current state (playing, paused, or stopped) of a <see cref="SoundEffectInstance"/>.
    /// </summary>
    public enum SoundState
    {
        /// <summary>
        /// The <see cref="SoundEffectInstance"/> is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// The <see cref="SoundEffectInstance"/> is playing.
        /// </summary>
        Playing,

        /// <summary>
        /// The <see cref="SoundEffectInstance"/> is stopped.
        /// </summary>
        Stopped
    }
}
