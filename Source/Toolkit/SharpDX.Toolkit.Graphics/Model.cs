// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

using System;
using System.IO;

using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
    public delegate Texture ModelMaterialTextureLoaderDelegate(string name);

    [ContentReader(typeof(ModelContentReader))]
    public class Model : Component
    {
        public MaterialCollection Materials;

        public ModelBoneCollection Bones;

        //// DISABLE_SKINNED_BONES
        //public ModelBoneCollection SkinnedBones;

        public ModelMeshCollection Meshes;

        public PropertyCollection Properties;

        /// <summary>
        /// Copies a transform of each bone in a model relative to all parent bones of the bone into a given array.
        /// </summary>
        /// <param name="destinationBoneTransforms">The array to receive bone transforms.</param>
        /// <exception cref="System.ArgumentNullException">destinationBoneTransforms</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">destinationBoneTransforms</exception>
        public void CopyAbsoluteBoneTransformsTo(Matrix[] destinationBoneTransforms)
        {
            if (destinationBoneTransforms == null)
            {
                throw new ArgumentNullException("destinationBoneTransforms");
            }
            if (destinationBoneTransforms.Length < Bones.Count)
            {
                throw new ArgumentOutOfRangeException("destinationBoneTransforms");
            }
            int count = Bones.Count;
            for (int i = 0; i < count; i++)
            {
                ModelBone bone = Bones[i];
                if (bone.Parent == null)
                {
                    destinationBoneTransforms[i] = bone.Transform;
                }
                else
                {
                    destinationBoneTransforms[i] = bone.Transform * destinationBoneTransforms[bone.Parent.Index];
                }
            }
        }

        /// <summary>
        /// Copies an array of transforms into each bone in the model.
        /// </summary>
        /// <param name="sourceBoneTransforms">An array containing new bone transforms.</param>
        /// <exception cref="System.ArgumentNullException">sourceBoneTransforms</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">sourceBoneTransforms</exception>
        public void CopyBoneTransformsFrom(Matrix[] sourceBoneTransforms)
        {
            if (sourceBoneTransforms == null)
            {
                throw new ArgumentNullException("sourceBoneTransforms");
            }
            if (sourceBoneTransforms.Length < Bones.Count)
            {
                throw new ArgumentOutOfRangeException("sourceBoneTransforms");
            }
            int count = Bones.Count;
            for (int i = 0; i < count; i++)
            {
                Bones[i].Transform = sourceBoneTransforms[i];
            }
        }

        /// <summary>
        /// Copies each bone transform relative only to the parent bone of the model to a given array.
        /// </summary>
        /// <param name="destinationBoneTransforms">The array to receive bone transforms.</param>
        /// <exception cref="System.ArgumentNullException">destinationBoneTransforms</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">destinationBoneTransforms</exception>
        public void CopyBoneTransformsTo(Matrix[] destinationBoneTransforms)
        {
            if (destinationBoneTransforms == null)
            {
                throw new ArgumentNullException("destinationBoneTransforms");
            }
            if (destinationBoneTransforms.Length < Bones.Count)
            {
                throw new ArgumentOutOfRangeException("destinationBoneTransforms");
            }
            int count = Bones.Count;
            for (int i = 0; i < count; i++)
            {
                destinationBoneTransforms[i] = Bones[i].Transform;
            }
        }

        ///// <summary>Render a model after applying the matrix transformations.</summary>
        ///// <param name="world">A world transformation matrix.</param>
        ///// <param name="view">A view transformation matrix.</param>
        ///// <param name="projection">A projection transformation matrix.</param>
        //public void Draw(Matrix world, Matrix view, Matrix projection)
        //{
        //    int count = Meshes.Count;
        //    int num3 = Bones.Count;
        //    Matrix[] sharedDrawBoneMatrices = Model.sharedDrawBoneMatrices;
        //    if ((sharedDrawBoneMatrices == null) || (sharedDrawBoneMatrices.Length < num3))
        //    {
        //        sharedDrawBoneMatrices = new Matrix[num3];
        //        Model.sharedDrawBoneMatrices = sharedDrawBoneMatrices;
        //    }
        //    this.CopyAbsoluteBoneTransformsTo(sharedDrawBoneMatrices);
        //    for (int i = 0; i < count; i++)
        //    {
        //        var mesh = Meshes[i];
        //        int index = mesh.ParentBone.Index;
        //        int num4 = mesh.Effects.Count;
        //        for (int j = 0; j < num4; j++)
        //        {
        //            Effect effect = mesh.Effects[j];
        //            if (effect == null)
        //            {
        //                throw new InvalidOperationException(FrameworkResources.ModelHasNoEffect);
        //            }

        //            IEffectMatrices matrices = effect as IEffectMatrices;
        //            if (matrices == null)
        //            {
        //                throw new InvalidOperationException(FrameworkResources.ModelHasNoIEffectMatrices);
        //            }
        //            matrices.World = sharedDrawBoneMatrices[index] * world;
        //            matrices.View = view;
        //            matrices.Projection = projection;
        //        }
        //        mesh.Draw();
        //    }
        //}

        /// <summary>
        /// Gets the root bone for this model.
        /// </summary>
        /// <value>The root.</value>
        public ModelBone Root
        {
            get
            {
                return Bones.Count > 0 ? Bones[0] : null;
            }
        }

        public virtual Model Clone()
        {
            var model = (Model)MemberwiseClone();
            throw new NotImplementedException();
        }

        public static Model Load(GraphicsDevice graphicsDevice, Stream stream, ModelMaterialTextureLoaderDelegate textureLoader)
        {
            using (var serializer = new ModelReader(graphicsDevice, stream, textureLoader))
            {
                return serializer.ReadModel();
            }
        }
    }
}