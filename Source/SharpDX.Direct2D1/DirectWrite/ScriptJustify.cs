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


namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Defines glyph characteristic information that an application needs to implement justification.
    /// </summary>
    public enum ScriptJustify
    {
        /// <summary>
        /// Justification cannot be applied at the glyph.
        /// </summary>
        None = 0,
        /// <summary>
        /// The glyph represents a blank in an Arabic run.
        /// </summary>
        ArabicBlank = 1,
        /// <summary>
        /// An inter-character justification point follows the glyph.
        /// </summary>
        Character = 2,

        /// <summary>
        /// The glyph represents a blank outside an Arabic run.
        /// </summary>
        Blank = 4,

        /// <summary>
        /// Normal middle-of-word glyph that connects to the right (begin).
        /// </summary>
        ArabicNormal = 7,
        /// <summary>
        /// Kashida (U+0640) in the middle of the word.
        /// </summary>
        ArabicKashida = 8,
        /// <summary>
        /// Final form of an alef-like (U+0627, U+0625, U+0623, U+0622).
        /// </summary>
        ArabicAlef = 9,
        /// <summary>
        /// Final form of Ha (U+0647).
        /// </summary>
        ArabicHa = 10,
        /// <summary>
        /// Final form of Ra (U+0631).
        /// </summary>
        ArabicRa = 11,
        /// <summary>
        /// Final form of Ba (U+0628).
        /// </summary>
        ArabicBa = 12,
        /// <summary>
        /// Ligature of alike (U+0628,U+0631).
        /// </summary>
        ArabicBaRa = 13,
        /// <summary>
        /// Highest priority: initial shape of Seen class (U+0633).
        /// </summary>
        ArabicSeen = 14,
        /// <summary>
        /// Highest priority: medial shape of Seen class (U+0633).
        /// </summary>
        ArabicSeenMedial = 15
    }

}