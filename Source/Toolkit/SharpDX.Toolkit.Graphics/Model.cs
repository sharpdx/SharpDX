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
        /// <param name="destinationBoneTransformsPtr">The array to receive bone transforms. Length of the allocated array must be at least == sizeof(Matrix) * Bones.Count</param>
        /// <exception cref="System.ArgumentNullException">destinationBoneTransforms</exception>
        public unsafe void CopyAbsoluteBoneTransformsTo(IntPtr destinationBoneTransformsPtr)
        {
            if (destinationBoneTransformsPtr == IntPtr.Zero)
            {
                throw new ArgumentNullException("destinationBoneTransforms");
            }

            var destinationBoneTransforms = (Matrix*)destinationBoneTransformsPtr;

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

        /// <summary>
        /// Render a model after applying the matrix transformations.
        /// </summary>
        /// <param name="context">The <see cref="GraphicsDevice"/> context.</param>
        /// <param name="world">A world transformation matrix.</param>
        /// <param name="view">A view transformation matrix.</param>
        /// <param name="projection">A projection transformation matrix.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Mesh has no effect
        /// or
        /// Effect has no IEffectMatrices
        /// </exception>
        public unsafe void Draw(GraphicsDevice context, Matrix world, Matrix view, Matrix projection)
        {
            int count = Meshes.Count;
            int boneCount = Bones.Count;
            Matrix* localSharedDrawBoneMatrices = stackalloc Matrix[boneCount];

            CopyAbsoluteBoneTransformsTo(new IntPtr(localSharedDrawBoneMatrices));

            var viewProjection = Matrix.Identity;
            bool viewProjectionUsed = false;
            var worldViewProjection = Matrix.Identity;
            bool worldViewProjectionUsed = false;

            for (int i = 0; i < count; i++)
            {
                var mesh = Meshes[i];
                int index = mesh.ParentBone.Index;
                int effectCount = mesh.Effects.Count;
                for (int j = 0; j < effectCount; j++)
                {
                    var effect = mesh.Effects[j];
                    if (effect == null)
                    {
                        throw new InvalidOperationException("Mesh has no effect");
                    }

                    Matrix result;
                    Matrix.Multiply(ref localSharedDrawBoneMatrices[index], ref world, out result);

                    var matrices = effect as IEffectMatrices;
                    if (matrices == null)
                    {
                        var worldParameter = effect.Parameters["World"];
                        if (worldParameter != null)
                            worldParameter.SetValue(ref result);

                        var viewdParameter = effect.Parameters["View"];
                        if (viewdParameter != null)
                            viewdParameter.SetValue(ref view);

                        var projectiondParameter = effect.Parameters["Projection"];
                        if (projectiondParameter != null)
                            projectiondParameter.SetValue(ref projection);

                        var viewProjectiondParameter = effect.Parameters["ViewProjection"];

                        if (viewProjectiondParameter != null)
                        {
                            if (!viewProjectionUsed)
                            {
                                Matrix.Multiply(ref view, ref projection, out viewProjection);
                                viewProjectionUsed = true;
                            }
                            viewProjectiondParameter.SetValue(ref viewProjection);
                        }

                        var worldViewProjectiondParameter = effect.Parameters["WorldViewProj"];

                        if (worldViewProjectiondParameter != null)
                        {
                            if (!viewProjectionUsed)
                            {
                                Matrix.Multiply(ref view, ref projection, out viewProjection);
                                viewProjectionUsed = true;
                            }

                            Matrix.Multiply(ref result, ref viewProjection, out worldViewProjection);
                            
                            worldViewProjectiondParameter.SetValue(ref worldViewProjection);
                        }
                    }
                    else
                    {
                        matrices.World = result;
                        matrices.View = view;
                        matrices.Projection = projection;
                    }
                }

                mesh.Draw(context);
            }
        }


        /// <summary>
        /// Calculates the bounds of this model.
        /// </summary>
        /// <returns>BoundingSphere.</returns>
        public unsafe BoundingSphere CalculateBounds()
        {
            return CalculateBounds(Matrix.Identity);
        }

        /// <summary>
        /// Calculates the bounds of this model in world space.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <returns>BoundingSphere.</returns>
        public unsafe BoundingSphere CalculateBounds(Matrix world)
        {
            int count = Meshes.Count;
            int boneCount = Bones.Count;
            Matrix* localSharedDrawBoneMatrices = stackalloc Matrix[boneCount];

            CopyAbsoluteBoneTransformsTo(new IntPtr(localSharedDrawBoneMatrices));
            var defaultSphere = new BoundingSphere(Vector3.Zero, 0.0f);
            for (int i = 0; i < count; i++)
            {
                var mesh = Meshes[i];
                int index = mesh.ParentBone.Index;
                Matrix result;
                Matrix.Multiply(ref localSharedDrawBoneMatrices[index], ref world, out result);

                var meshSphere = mesh.BoundingSphere;
                Vector3.TransformCoordinate(ref meshSphere.Center, ref result, out meshSphere.Center);

                BoundingSphere.Merge(ref defaultSphere, ref meshSphere, out defaultSphere);
            }
            return defaultSphere;
        }

        /// <summary>
        /// Iterator on each <see cref="ModelMeshPart"/>.
        /// </summary>
        /// <param name="meshPartFunction">The mesh part function.</param>
        public void ForEach(Action<ModelMeshPart> meshPartFunction)
        {
            int meshCount = Meshes.Count;
            for (int i = 0; i < meshCount; i++)
            {
                Meshes[i].ForEach(meshPartFunction);
            }
        }

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

        public override string ToString()
        {
            return string.Format("{0} {1}", this.GetType().Name, Name);
        }
    }
}