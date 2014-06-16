// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SharpDX.XACT3
{
    /// <summary>
    /// Wave notification parameters.
    /// </summary>
    /// <unmanaged>XACT_NOTIFICATION_WAVE</unmanaged>
    public class WaveNotification : Notification
    {
        internal unsafe WaveNotification(RawNotification* rawNotification)
            : base(rawNotification)
        {
            WaveBank= CppObject.FromPointer<WaveBank>(rawNotification->Data.Wave.WaveBankPointer);
            WaveIndex = rawNotification->Data.Wave.WaveIndex;
            CueIndex = rawNotification->Data.Wave.CueIndex;
            SoundBank = CppObject.FromPointer<SoundBank>(rawNotification->Data.Wave.SoundBankPointer);
            Cue = CppObject.FromPointer<Cue>(rawNotification->Data.Wave.CuePointer);
            Wave = CppObject.FromPointer<Wave>(rawNotification->Data.Wave.WavePointer);
        }

        public WaveBank WaveBank { get; set; }

        public int WaveIndex { get; set; }

        public int CueIndex { get; set; }

        public SoundBank SoundBank { get; set; }

        public Cue Cue { get; set; }

        public Wave Wave { get; set; }
    }
}