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
// -----------------------------------------------------------------------------
// The following code is a port of XNA StockEffects http://xbox.create.msdn.com/en-US/education/catalog/sample/stock_effects
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.
//-----------------------------------------------------------------------------
// EnvironmentMapEffect.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Built-in effect that supports environment mapping.
    /// </summary>
    public partial class EnvironmentMapEffect : Effect, IEffectMatrices, IEffectLights, IEffectFog
    {
        #region Effect Parameters

        EffectParameter textureParam;
        EffectParameter environmentMapParam;
        EffectParameter environmentMapAmountParam;
        EffectParameter environmentMapSpecularParam;
        EffectParameter fresnelFactorParam;
        EffectParameter diffuseColorParam;
        EffectParameter emissiveColorParam;
        EffectParameter eyePositionParam;
        EffectParameter fogColorParam;
        EffectParameter fogVectorParam;
        EffectParameter worldParam;
        EffectParameter worldInverseTransposeParam;
        EffectParameter worldViewProjParam;
        EffectPass shaderPass;

        #endregion

        #region Fields

        bool oneLight;
        bool fogEnabled;
        bool fresnelEnabled;
        bool specularEnabled;

        Matrix world = Matrix.Identity;
        Matrix view = Matrix.Identity;
        Matrix projection = Matrix.Identity;

        Matrix worldView;

        Vector4 diffuseColor = Vector4.One;
        Vector3 emissiveColor = Vector3.Zero;
        Vector3 ambientLightColor = Vector3.Zero;

        float alpha = 1;

        DirectionalLight light0;
        DirectionalLight light1;
        DirectionalLight light2;

        float fogStart = 0;
        float fogEnd = 1;

        EffectDirtyFlags dirtyFlags = EffectDirtyFlags.All;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the world matrix.
        /// </summary>
        public Matrix World
        {
            get { return world; }
            
            set
            {
                world = value;
                dirtyFlags |= EffectDirtyFlags.World | EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
            }
        }

        /// <summary>
        /// Gets or sets the view matrix.
        /// </summary>
        public Matrix View
        {
            get { return view; }
            
            set
            {
                view = value;
                dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.EyePosition | EffectDirtyFlags.Fog;
            }
        }

        /// <summary>
        /// Gets or sets the projection matrix.
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
            
            set
            {
                projection = value;
                dirtyFlags |= EffectDirtyFlags.WorldViewProj;
            }
        }

        /// <summary>
        /// Gets or sets the material diffuse color (range 0 to 1).
        /// </summary>
        public Vector4 DiffuseColor
        {
            get { return diffuseColor; }
            
            set
            {
                diffuseColor = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }

        /// <summary>
        /// Gets or sets the material emissive color (range 0 to 1).
        /// </summary>
        public Vector3 EmissiveColor
        {
            get { return emissiveColor; }
            
            set
            {
                emissiveColor = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }

        /// <summary>
        /// Gets or sets the material alpha.
        /// </summary>
        public float Alpha
        {
            get { return alpha; }
            
            set
            {
                alpha = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }

        /// <summary>
        /// Gets or sets the ambient light color (range 0 to 1).
        /// </summary>
        public Vector3 AmbientLightColor
        {
            get { return ambientLightColor; }
            
            set
            {
                ambientLightColor = value;
                dirtyFlags |= EffectDirtyFlags.MaterialColor;
            }
        }

        /// <summary>
        /// Gets the first directional light.
        /// </summary>
        public DirectionalLight DirectionalLight0 { get { return light0; } }

        /// <summary>
        /// Gets the second directional light.
        /// </summary>
        public DirectionalLight DirectionalLight1 { get { return light1; } }

        /// <summary>
        /// Gets the third directional light.
        /// </summary>
        public DirectionalLight DirectionalLight2 { get { return light2; } }

        /// <summary>
        /// Gets or sets the fog enable flag.
        /// </summary>
        public bool FogEnabled
        {
            get { return fogEnabled; }
            
            set
            {
                if (fogEnabled != value)
                {
                    fogEnabled = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex | EffectDirtyFlags.FogEnable;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fog start distance.
        /// </summary>
        public float FogStart
        {
            get { return fogStart; }
            
            set
            {
                fogStart = value;
                dirtyFlags |= EffectDirtyFlags.Fog;
            }
        }

        /// <summary>
        /// Gets or sets the fog end distance.
        /// </summary>
        public float FogEnd
        {
            get { return fogEnd; }
            
            set
            {
                fogEnd = value;
                dirtyFlags |= EffectDirtyFlags.Fog;
            }
        }

        /// <summary>
        /// Gets or sets the fog color.
        /// </summary>
        public Vector3 FogColor
        {
            get { return fogColorParam.GetValue<Vector3>(); }
            set { fogColorParam.SetValue(value); }
        }


        /// <summary>
        /// Gets or sets the current texture.
        /// </summary>
        public Texture2D Texture
        {
            get { return textureParam.GetResource<Texture2D>(); }
            set { textureParam.SetResource(value); }
        }

        /// <summary>
        /// Gets or sets the current environment map texture.
        /// </summary>
        public TextureCube EnvironmentMap
        {
            get { return environmentMapParam.GetResource<TextureCube>(); }
            set { environmentMapParam.SetResource(value); }
        }
             
        /// <summary>
        /// Gets or sets the amount of the environment map RGB that will be blended over 
        /// the base texture. Range 0 to 1, default 1. If set to zero, the RGB channels 
        /// of the environment map will completely ignored (but the environment map alpha 
        /// may still be visible if EnvironmentMapSpecular is greater than zero).
        /// </summary>
        public float EnvironmentMapAmount
        {
            get { return environmentMapAmountParam.GetValue<float>(); }
            set { environmentMapAmountParam.SetValue(value); }
        }

        /// <summary>
        /// Gets or sets the amount of the environment map alpha channel that will 
        /// be added to the base texture. Range 0 to 1, default 0. This can be used 
        /// to implement cheap specular lighting, by encoding one or more specular 
        /// highlight patterns into the environment map alpha channel, then setting 
        /// EnvironmentMapSpecular to the desired specular light color.
        /// </summary>
        public Vector3 EnvironmentMapSpecular
        {
            get { return environmentMapSpecularParam.GetValue<Vector3>(); }

            set
            {
                environmentMapSpecularParam.SetValue(value);
                
                bool enabled = (value != Vector3.Zero);
                
                if (specularEnabled != enabled)
                {
                    specularEnabled = enabled;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the Fresnel factor used for the environment map blending. 
        /// Higher values make the environment map only visible around the silhouette 
        /// edges of the object, while lower values make it visible everywhere. 
        /// Setting this property to 0 disables Fresnel entirely, making the 
        /// environment map equally visible regardless of view angle. The default is 
        /// 1. Fresnel only affects the environment map RGB (the intensity of which is 
        /// controlled by EnvironmentMapAmount). The alpha contribution (controlled by 
        /// EnvironmentMapSpecular) is not affected by the Fresnel setting.
        /// </summary>
        public float FresnelFactor
        {
            get { return fresnelFactorParam.GetValue<float>(); }

            set
            {
                fresnelFactorParam.SetValue(value);
                
                bool enabled = (value != 0);
                
                if (fresnelEnabled != enabled)
                {
                    fresnelEnabled = enabled;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }
        }

        /// <summary>
        /// This effect requires lighting, so we explicitly implement
        /// IEffectLights.LightingEnabled, and do not allow turning it off.
        /// </summary>
        bool IEffectLights.LightingEnabled
        {
            get { return true; }
            set { if (!value) throw new NotSupportedException("EnvironmentMapEffect does not support setting LightingEnabled to false."); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new EnvironmentMapEffect with default parameter settings.
        /// </summary>
        public EnvironmentMapEffect(GraphicsDevice device) : this(device, device.DefaultEffectPool)
        {
        }

        /// <summary>
        /// Creates a new EnvironmentMapEffect with default parameter settings from a specified <see cref="EffectPool"/>.
        /// </summary>
        public EnvironmentMapEffect(GraphicsDevice device, EffectPool pool)
            : base(device, effectBytecode, pool)
        {
            CacheEffectParameters(null);

            DirectionalLight0.Enabled = true;

            EnvironmentMapAmount = 1;
            EnvironmentMapSpecular = Vector3.Zero;
            FresnelFactor = 1;
        }

        /// <summary>Initializes this instance.</summary>
        protected override void Initialize()
        {
            Pool.RegisterBytecode(effectBytecode);
            base.Initialize();
        }

        ///// <summary>
        ///// Creates a new EnvironmentMapEffect by cloning parameter settings from an existing instance.
        ///// </summary>
        //protected EnvironmentMapEffect(EnvironmentMapEffect cloneSource)
        //    : base(cloneSource)
        //{
        //    CacheEffectParameters(cloneSource);

        //    fogEnabled = cloneSource.fogEnabled;
        //    fresnelEnabled = cloneSource.fresnelEnabled;
        //    specularEnabled = cloneSource.specularEnabled;

        //    world = cloneSource.world;
        //    view = cloneSource.view;
        //    projection = cloneSource.projection;

        //    diffuseColor = cloneSource.diffuseColor;
        //    emissiveColor = cloneSource.emissiveColor;
        //    ambientLightColor = cloneSource.ambientLightColor;

        //    alpha = cloneSource.alpha;

        //    fogStart = cloneSource.fogStart;
        //    fogEnd = cloneSource.fogEnd;
        //}


        ///// <summary>
        ///// Creates a clone of the current EnvironmentMapEffect instance.
        ///// </summary>
        //public override Effect Clone()
        //{
        //    return new EnvironmentMapEffect(this);
        //}


        /// <summary>
        /// Sets up the standard key/fill/back lighting rig.
        /// </summary>
        public void EnableDefaultLighting()
        {
            AmbientLightColor = EffectHelpers.EnableDefaultLighting(light0, light1, light2);
        }


        /// <summary>
        /// Looks up shortcut references to our effect parameters.
        /// </summary>
        void CacheEffectParameters(EnvironmentMapEffect cloneSource)
        {
            textureParam                = Parameters["Texture"];
            environmentMapParam         = Parameters["EnvironmentMap"];
            environmentMapAmountParam   = Parameters["EnvironmentMapAmount"];
            environmentMapSpecularParam = Parameters["EnvironmentMapSpecular"];
            fresnelFactorParam          = Parameters["FresnelFactor"];
            diffuseColorParam           = Parameters["DiffuseColor"];
            emissiveColorParam          = Parameters["EmissiveColor"];
            eyePositionParam            = Parameters["EyePosition"];
            fogColorParam               = Parameters["FogColor"];
            fogVectorParam              = Parameters["FogVector"];
            worldParam                  = Parameters["World"];
            worldInverseTransposeParam  = Parameters["WorldInverseTranspose"];
            worldViewProjParam          = Parameters["WorldViewProj"];

            light0 = new DirectionalLight(Parameters["DirLight0Direction"],
                                          Parameters["DirLight0DiffuseColor"],
                                          null,
                                          (cloneSource != null) ? cloneSource.light0 : null);

            light1 = new DirectionalLight(Parameters["DirLight1Direction"],
                                          Parameters["DirLight1DiffuseColor"],
                                          null,
                                          (cloneSource != null) ? cloneSource.light1 : null);

            light2 = new DirectionalLight(Parameters["DirLight2Direction"],
                                          Parameters["DirLight2DiffuseColor"],
                                          null,
                                          (cloneSource != null) ? cloneSource.light2 : null);
        }

        /// <summary>
        /// Lazily computes derived parameter values immediately before applying the effect.
        /// </summary>
        protected internal override EffectPass OnApply(EffectPass pass)
        {
            // Recompute the world+view+projection matrix or fog vector?
            dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(dirtyFlags, ref world, ref view, ref projection, ref worldView, fogEnabled, fogStart, fogEnd, worldViewProjParam, fogVectorParam);

            // Recompute the world inverse transpose and eye position?
            dirtyFlags = EffectHelpers.SetLightingMatrices(dirtyFlags, ref world, ref view, worldParam, worldInverseTransposeParam, eyePositionParam);
            
            // Recompute the diffuse/emissive/alpha material color parameters?
            if ((dirtyFlags & EffectDirtyFlags.MaterialColor) != 0)
            {
                EffectHelpers.SetMaterialColor(true, alpha, ref diffuseColor, ref emissiveColor, ref ambientLightColor, diffuseColorParam, emissiveColorParam);

                dirtyFlags &= ~EffectDirtyFlags.MaterialColor;
            }

            // Check if we can use the only-bother-with-the-first-light shader optimization.
            bool newOneLight = !light1.Enabled && !light2.Enabled;
            
            if (oneLight != newOneLight)
            {
                oneLight = newOneLight;
                dirtyFlags |= EffectDirtyFlags.ShaderIndex;
            }

            // Recompute the shader index?
            if ((dirtyFlags & EffectDirtyFlags.ShaderIndex) != 0)
            {
                int shaderIndex = 0;
                
                if (!fogEnabled)
                    shaderIndex += 1;
                
                if (fresnelEnabled)
                    shaderIndex += 2;
                
                if (specularEnabled)
                    shaderIndex += 4;

                if (oneLight)
                    shaderIndex += 8;

                shaderPass = pass.SubPasses[shaderIndex];

                dirtyFlags &= ~EffectDirtyFlags.ShaderIndex;
            }

            return base.OnApply(shaderPass);
        }

        #endregion
    }
}
