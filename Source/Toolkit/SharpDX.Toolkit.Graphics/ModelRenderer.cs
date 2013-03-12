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
using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    public class ModelRenderer : Component
    {
        protected GraphicsDevice GraphicsDevice;

        public ModelRenderer(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }

        public Model Model { get; private set; }

        public MeshItem[] Meshes { get; private set; }

        public void Draw(GraphicsDevice graphicsDevice)
        {
        }

        public void Initialize(Model model)
        {
            Model = model;
            Meshes = new MeshItem[Model.Meshes.Count];
            for (int i = 0; i < Model.Meshes.Count; i++)
            {
                var mesh = Model.Meshes[i];
                var parts = new MeshPartItem[mesh.MeshParts.Count];
                Meshes[i] = new MeshItem(mesh, parts);

                for (int j = 0; j < mesh.MeshParts.Count; j++)
                {
                    var meshPart = mesh.MeshParts[j];
                    var effect = CreateEffect(mesh, meshPart);
                    parts[j] = new MeshPartItem(meshPart, Meshes, i) { Effect = effect };
                }
            }
        }

        protected virtual Effect CreateEffect(ModelMesh mesh, ModelMeshPart meshPart)
        {
            return null;
        }

        public struct MeshItem
        {
            public readonly List<Effect> Effects;

            public readonly ModelMesh Mesh;

            public readonly MeshPartItem[] MeshParts;

            internal MeshItem(ModelMesh mesh, MeshPartItem[] meshParts)
                : this()
            {
                Mesh = mesh;
                MeshParts = meshParts;
                Effects = new List<Effect>();
            }


            /// <summary>
            /// Draws all of the ModelMeshPart objects in this mesh, using their current Effect settings.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <exception cref="System.InvalidOperationException"></exception>
            public void Draw(GraphicsDevice context)
            {
                int count = this.MeshParts.Length;
                for (int i = 0; i < count; i++)
                {
                    var effect = MeshParts[i].Effect;
                    if (effect == null)
                    {
                        throw new InvalidOperationException("MeshPart has no effects");
                    }
                    int num3 = effect.CurrentTechnique.Passes.Count;
                    for (int j = 0; j < num3; j++)
                    {
                        effect.CurrentTechnique.Passes[j].Apply();
                        MeshParts[i].MeshPart.Draw(context);
                    }
                }
            }

        }

        public struct MeshPartItem
        {
            internal MeshPartItem(ModelMeshPart meshPart, MeshItem[] items, int parentIndex)
                : this()
            {
                MeshPart = meshPart;
                meshItems = items;
                parentRenderItem = parentIndex;
            }

            public readonly ModelMeshPart MeshPart;

            private Effect effect;

            private MeshItem[] meshItems;

            private int parentRenderItem;

            /// <summary>Gets or sets the material Effect for this mesh part.  Reference page contains code sample.</summary>
            public Effect Effect
            {
                get
                {
                    return effect;
                }
                set
                {
                    if (value != effect)
                    {
                        bool isPreviousEffectUsedByAnotherMeshPart = false;
                        bool isEffectUsedByAnotherMeshPart = false;

                        var effects = meshItems[parentRenderItem].Effects;
                        var meshPartRenderers = meshItems[parentRenderItem].MeshParts;

                        // Check that 
                        for (int i = 0; i < meshPartRenderers.Length; i++)
                        {
                            ModelMeshPart objA = meshPartRenderers[i].MeshPart;
                            if (!ReferenceEquals(objA, MeshPart))
                            {
                                var partEffect = meshPartRenderers[i].Effect;
                                if (ReferenceEquals(partEffect, effect))
                                {
                                    isPreviousEffectUsedByAnotherMeshPart = true;
                                }
                                else if (ReferenceEquals(partEffect, value))
                                {
                                    isEffectUsedByAnotherMeshPart = true;
                                }
                            }
                        }

                        if (!isPreviousEffectUsedByAnotherMeshPart && (effect != null))
                        {
                            effects.Remove(effect);
                        }
                        if (!isEffectUsedByAnotherMeshPart && (value != null))
                        {
                            effects.Add(value);
                        }
                        effect = value;
                    }
                }
            }
        }
    }


    public class BasicEffectRenderer : ModelRenderer
    {
        public BasicEffectRenderer(GraphicsDevice device)
            : base(device)
        {
        }

        protected virtual BasicEffect CreateBasicEffect()
        {
            return new BasicEffect(GraphicsDevice);
        }

        protected override Effect CreateEffect(ModelMesh mesh, ModelMeshPart meshPart)
        {
            var effect = CreateBasicEffect();

            var material = meshPart.Material;

            // Setup ColorDiffuse
            if (material.HasProperty(MaterialKeys.ColorDiffuse))
            {
                effect.DiffuseColor = material.GetProperty(MaterialKeys.ColorDiffuse);
            }

            // Setup ColorSpecular
            if (material.HasProperty(MaterialKeys.ColorSpecular))
            {
                effect.SpecularColor = material.GetProperty(MaterialKeys.ColorSpecular);
            }

            // Setup ColorEmissive
            if (material.HasProperty(MaterialKeys.ColorEmissive))
            {
                effect.EmissiveColor = material.GetProperty(MaterialKeys.ColorEmissive);
            }

            if (material.HasProperty(MaterialKeys.DiffuseTexture))
            {
                var diffuseTextureStack = material.GetProperty(MaterialKeys.DiffuseTexture);
                if (diffuseTextureStack.Count > 0)
                {
                    var diffuseTexture = diffuseTextureStack[0];

                    effect.Texture = (Texture2DBase)diffuseTexture.Texture;
                    // TODO set sampler

                    effect.TextureEnabled = true;

                    effect.EnableDefaultLighting();
                }
            }

            return effect;
        }

        public void EnableDefaultLighting()
        {
            foreach (var meshItem in Meshes)
            {
                foreach (var effect in meshItem.Effects)
                {
                    var basicEffect = effect as BasicEffect;
                    if (basicEffect != null)
                    {
                        basicEffect.EnableDefaultLighting();
                    }
                }
            }
        }

        private Matrix[] thisSharedDrawBoneMatrices;

        /// <summary>Render a model after applying the matrix transformations.</summary>
        /// <param name="world">A world transformation matrix.</param>
        /// <param name="view">A view transformation matrix.</param>
        /// <param name="projection">A projection transformation matrix.</param>
        public void Draw(GraphicsDevice context, Matrix world, Matrix view, Matrix projection)
        {
            int count = Meshes.Length;
            int num3 = Model.Bones.Count;
            Matrix[] localSharedDrawBoneMatrices = thisSharedDrawBoneMatrices;
            if ((localSharedDrawBoneMatrices == null) || (localSharedDrawBoneMatrices.Length < num3))
            {
                localSharedDrawBoneMatrices = new Matrix[num3];
                thisSharedDrawBoneMatrices = localSharedDrawBoneMatrices;
            }

            Model.CopyAbsoluteBoneTransformsTo(localSharedDrawBoneMatrices);
            for (int i = 0; i < count; i++)
            {
                var mesh = Meshes[i];
                int index = mesh.Mesh.ParentBone.Index;
                int num4 = mesh.Effects.Count;
                for (int j = 0; j < num4; j++)
                {
                    var effect = mesh.Effects[j];
                    if (effect == null)
                    {
                        throw new InvalidOperationException("Mesh has no effect");
                    }

                    var matrices = effect as IEffectMatrices;
                    if (matrices == null)
                    {
                        throw new InvalidOperationException("Effect has no IEffectMatrices");
                    }
                    matrices.World = localSharedDrawBoneMatrices[index] * world;
                    matrices.View = view;
                    matrices.Projection = projection;
                }

                mesh.Draw(context);
            }
        }
    }



}