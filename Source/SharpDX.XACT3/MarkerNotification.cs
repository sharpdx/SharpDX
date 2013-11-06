// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    /// Marker notification parameters.
    /// </summary>
    /// <unmanaged>XACT_NOTIFICATION_MARKER</unmanaged>
    public class MarkerNotification : Notification
    {
        /// <summary>Initializes a new instance of the <see cref="MarkerNotification"/> class.</summary>
        /// <param name="rawNotification">The raw notification.</param>
        internal unsafe MarkerNotification(RawNotification* rawNotification)
            : base(rawNotification)
        {
            CueIndex = rawNotification->Data.Marker.CueIndex;
            SoundBank = CppObject.FromPointer<SoundBank>(rawNotification->Data.Marker.SoundBankPointer);
            Cue = CppObject.FromPointer<Cue>(rawNotification->Data.Marker.CuePointer);
            Marker = rawNotification->Data.Marker.Marker;
        }

        /// <summary>Gets or sets the index of the cue.</summary>
        /// <value>The index of the cue.</value>
        public int CueIndex { get; set; }

        /// <summary>Gets or sets the sound bank.</summary>
        /// <value>The sound bank.</value>
        public SoundBank SoundBank { get; set; }

        /// <summary>Gets or sets the cue.</summary>
        /// <value>The cue.</value>
        public Cue Cue { get; set; }

        /// <summary>The marker.</summary>
        public int Marker;
    }
}