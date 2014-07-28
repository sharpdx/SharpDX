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

using SharpDX.Mathematics;
using SharpDX.Toolkit.Graphics;

using System;
using System.Collections.Generic;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// Manages and drives skeletal animations.
    /// </summary>
    public class AnimationSystem : GameSystem
    {
        private Dictionary<Model, AnimationState> animationStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationSystem" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public AnimationSystem(Game game)
            : base(game)
        {
            animationStates = new Dictionary<Model, AnimationState>();
            Enabled = true;
        }

        /// <summary>
        /// Starts a new animation.
        /// </summary>
        /// <param name="model">The animated model.</param>
        /// <param name="animation">The animation to play.</param>
        public void StartAnimation(Model model, ModelAnimation animation)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (animation == null)
            {
                throw new ArgumentNullException("animation");
            }

            animationStates[model] = new AnimationState
            {
                Model = model,
                Animation = animation,
                ChannelMap = BuildChannelMap(model, animation)
            };
        }

        /// <summary>
        /// Stop the animation of a model.
        /// </summary>
        /// <param name="model">The animated model.</param>
        public void StopAnimation(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            animationStates.Remove(model);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var state in animationStates.Values)
            {
                var animation = state.Animation;

                if (MathUtil.IsZero(animation.Duration))
                {
                    state.CurrentTime = 0.0f;
                }
                else
                {
                    state.CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    state.CurrentTime %= animation.Duration;
                }

                for (int i = 0; i < animation.Channels.Count; i++)
                {
                    var boneIndex = state.ChannelMap[i];
                    if (boneIndex < 0)
                    {
                        continue;
                    }

                    var bone = state.Model.Bones[boneIndex];
                    var keyFrames = animation.Channels[i].KeyFrames;

                    int nextIndex = 0;
                    int lastIndex = keyFrames.Count - 1;

                    while (nextIndex < lastIndex && state.CurrentTime >= keyFrames[nextIndex].Time)
                    {
                        nextIndex++;
                    }

                    var nextKeyFrame = keyFrames[nextIndex];

                    if (nextIndex == 0)
                    {
                        bone.Transform = (Matrix)nextKeyFrame.Value;
                    }
                    else
                    {
                        var previousKeyFrame = keyFrames[nextIndex - 1];
                        var amount = (state.CurrentTime - previousKeyFrame.Time) / (nextKeyFrame.Time - previousKeyFrame.Time);

                        bone.Transform = (Matrix)CompositeTransform.Slerp(previousKeyFrame.Value, nextKeyFrame.Value, amount);
                    }
                }
            }
        }

        private int[] BuildChannelMap(Model model, ModelAnimation animation)
        {
            var channelMap = new int[animation.Channels.Count];

            for (int i = 0; i < animation.Channels.Count; i++)
            {
                channelMap[i] = -1;
                foreach (var bone in model.Bones)
                {
                    if (bone.Name == animation.Channels[i].BoneName)
                    {
                        channelMap[i] = bone.Index;
                        break;
                    }
                }
            }

            return channelMap;
        }

        private class AnimationState
        {
            public float CurrentTime;
            public Model Model;
            public ModelAnimation Animation;
            public int[] ChannelMap;
        }
    }
}