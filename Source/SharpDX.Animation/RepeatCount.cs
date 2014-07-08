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

namespace SharpDX.Animation
{
    /// <summary>
    /// Repeat count used for <see cref="Storyboard.RepeatBetweenKeyframes(SharpDX.Animation.KeyFrame,SharpDX.Animation.KeyFrame,SharpDX.Animation.RepeatCount)"/>
    /// </summary>
    public enum RepeatCount : int
    {
        /// <summary>
        /// Indicates that the interval between two keyframes in a storyboard should repeat indefinitely until the <see cref="Storyboard.Conclude"/> method is called.
        /// </summary>
        /// <unmanaged>UI_ANIMATION_REPEAT_INDEFINITELY</unmanaged>
        Indefinitely = -1,


        /// <summary>
        /// Indicates that the interval between two keyframes in a storyboard should repeat indefinitely until the keyframe loop terminates on the ending keyframe when the <see cref="Storyboard.Conclude"/> method is called.
        /// </summary>
        /// <unmanaged>UI_ANIMATION_REPEAT_INDEFINITELY_CONCLUDE_AT_END</unmanaged>
        IndefinitelyConcludeAtEnd = -1,
        
        /// <summary>
        /// Indicates that the interval between two keyframes in a storyboard should repeat indefinitely until the keyframe loop terminates on the starting keyframe when the <see cref="Storyboard.Conclude"/> method is called.
        /// </summary>
        /// <unmanaged>UI_ANIMATION_REPEAT_INDEFINITELY_CONCLUDE_AT_START</unmanaged>
        IndefinitelyConcludeAtStart = -2,
    }
}