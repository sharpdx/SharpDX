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

using System.Collections.Generic;

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public sealed partial class ModelData
    {
        /// <summary>
        /// Class Animation
        /// </summary>
        public class Animation : IDataSerializable
        {
            /// <summary>
            /// The name of this animation.
            /// </summary>
            public string Name;

            /// <summary>
            /// Total total animation duration.
            /// </summary>
            public float Duration;

            /// <summary>
            /// The children node indices.
            /// </summary>
            public List<AnimationChannel> Channels;

            /// <summary>
            /// Initializes a new instance of the <see cref="Animation" /> class.
            /// </summary>
            public Animation()
            {
                Channels = new List<AnimationChannel>();
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.Serialize(ref Duration);
                serializer.Serialize(ref Channels);
            }
        }

        /// <summary>
        /// Class AnimationChannel
        /// </summary>
        public class AnimationChannel : IDataSerializable
        {
            /// <summary>
            /// The name of the animated bone.
            /// </summary>
            public string BoneName;

            /// <summary>
            /// The key frames of this animation.
            /// </summary>
            public List<KeyFrame> KeyFrames;

            /// <summary>
            /// Initializes a new instance of the <see cref="AnimationChannel" /> class.
            /// </summary>
            public AnimationChannel()
            {
                KeyFrames = new List<KeyFrame>();
            }

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref BoneName);
                serializer.Serialize(ref KeyFrames);
            }
        }

        /// <summary>
        /// Class KeyFrame
        /// </summary>
        public class KeyFrame : IDataSerializable
        {
            /// <summary>
            /// The key time.
            /// </summary>
            public float Time;

            /// <summary>
            /// The bone transform.
            /// </summary>
            public Matrix Value;

            /// <inheritdoc/>
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Time);
                serializer.Serialize(ref Value);
            }
        }
    }
}