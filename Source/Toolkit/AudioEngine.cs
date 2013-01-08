using SharpDX.XAudio2;

namespace SharpDX.Toolkit.Audio
{ 
    internal static class AudioEngine
    {
        public static SharpDX.XAudio2.XAudio2 Engine;
        public static MasteringVoice MasterVoice;
        public static int SoundEffectInstanceCount;

        static AudioEngine()
        {
           Engine = new SharpDX.XAudio2.XAudio2(XAudio2Flags.None, ProcessorSpecifier.AnyProcessor);
           MasterVoice = new MasteringVoice(Engine);
           SoundEffectInstanceCount = 0;
        }

        public static void Dispose()
        {
            Engine.Dispose();
            MasterVoice.Dispose();
        }
    }
}
