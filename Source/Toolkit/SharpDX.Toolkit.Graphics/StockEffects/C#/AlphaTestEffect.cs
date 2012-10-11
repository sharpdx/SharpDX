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
// AlphaTestEffect.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Built-in effect that supports alpha testing.
    /// </summary>
    public partial class AlphaTestEffect : Effect, IEffectMatrices, IEffectFog
    {
        #region Effect Parameters

        EffectParameter textureParam;
        EffectParameter diffuseColorParam;
        EffectParameter alphaTestParam;
        EffectParameter fogColorParam;
        EffectParameter fogVectorParam;
        EffectParameter worldViewProjParam;
        EffectPass shaderPass;
        #endregion

        #region Fields

        bool fogEnabled;
        bool vertexColorEnabled;

        Matrix world = Matrix.Identity;
        Matrix view = Matrix.Identity;
        Matrix projection = Matrix.Identity;

        Matrix worldView;

        Vector3 diffuseColor = Vector3.One;

        float alpha = 1;

        float fogStart = 0;
        float fogEnd = 1;

        SharpDX.Direct3D11.Comparison alphaFunction = Direct3D11.Comparison.Greater;
        int referenceAlpha;
        bool isEqNe;

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
                dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
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
                dirtyFlags |= EffectDirtyFlags.WorldViewProj | EffectDirtyFlags.Fog;
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
        public Vector3 DiffuseColor
        {
            get { return diffuseColor; }
            
            set
            {
                diffuseColor = value;
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
        /// Gets or sets whether vertex color is enabled.
        /// </summary>
        public bool VertexColorEnabled
        {
            get { return vertexColorEnabled; }
            
            set
            {
                if (vertexColorEnabled != value)
                {
                    vertexColorEnabled = value;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }
        }


        /// <summary>
        /// Gets or sets the alpha compare function (default Greater).
        /// </summary>
        public Direct3D11.Comparison AlphaFunction
        {
            get { return alphaFunction; }
            
            set
            {
                alphaFunction = value;
                dirtyFlags |= EffectDirtyFlags.AlphaTest;
            }
        }


        /// <summary>
        /// Gets or sets the reference alpha value (default 0).
        /// </summary>
        public int ReferenceAlpha
        {
            get { return referenceAlpha; }
            
            set
            {
                referenceAlpha = value;
                dirtyFlags |= EffectDirtyFlags.AlphaTest;
            }
        }


        #endregion

        #region Methods

        private const string AlphaTestEffectName = "Toolkit::AlphaTestEffect";

        /// <summary>
        /// Creates a new AlphaTestEffect with default parameter settings.
        /// </summary>
        public AlphaTestEffect(GraphicsDevice device) : this(device, device.DefaultEffectPool)
        {
        }

        /// <summary>
        /// Creates a new AlphaTestEffect with default parameter settings from a specified <see cref="EffectPool"/>.
        /// </summary>
        public AlphaTestEffect(GraphicsDevice device, EffectPool pool)
            : base(device, pool, AlphaTestEffectName)
        {
            CacheEffectParameters();
        }

        protected override void Initialize()
        {
            Pool.RegisterBytecode(effectBytecode);
            base.Initialize();
        }

        ///// <summary>
        ///// Creates a new AlphaTestEffect by cloning parameter settings from an existing instance.
        ///// </summary>
        //protected AlphaTestEffect(AlphaTestEffect cloneSource)
        //    : base(cloneSource)
        //{
        //    CacheEffectParameters();

        //    fogEnabled = cloneSource.fogEnabled;
        //    vertexColorEnabled = cloneSource.vertexColorEnabled;

        //    world = cloneSource.world;
        //    view = cloneSource.view;
        //    projection = cloneSource.projection;

        //    diffuseColor = cloneSource.diffuseColor;

        //    alpha = cloneSource.alpha;

        //    fogStart = cloneSource.fogStart;
        //    fogEnd = cloneSource.fogEnd;
            
        //    alphaFunction = cloneSource.alphaFunction;
        //    referenceAlpha = cloneSource.referenceAlpha;
        //}


        ///// <summary>
        ///// Creates a clone of the current AlphaTestEffect instance.
        ///// </summary>
        //public override Effect Clone()
        //{
        //    return new AlphaTestEffect(this);
        //}

        /// <summary>
        /// Looks up shortcut references to our effect parameters.
        /// </summary>
        void CacheEffectParameters()
        {
            textureParam        = Parameters["Texture"];
            diffuseColorParam   = Parameters["DiffuseColor"];
            alphaTestParam      = Parameters["AlphaTest"];
            fogColorParam       = Parameters["FogColor"];
            fogVectorParam      = Parameters["FogVector"];
            worldViewProjParam  = Parameters["WorldViewProj"];
        }

        /// <summary>
        /// Lazily computes derived parameter values immediately before applying the effect.
        /// </summary>
        protected internal override EffectPass OnApply(EffectPass pass)
        {
            // Recompute the world+view+projection matrix or fog vector?
            dirtyFlags = EffectHelpers.SetWorldViewProjAndFog(dirtyFlags, ref world, ref view, ref projection, ref worldView, fogEnabled, fogStart, fogEnd, worldViewProjParam, fogVectorParam);

            // Recompute the diffuse/alpha material color parameter?
            if ((dirtyFlags & EffectDirtyFlags.MaterialColor) != 0)
            {
                diffuseColorParam.SetValue(new Vector4(diffuseColor * alpha, alpha));

                dirtyFlags &= ~EffectDirtyFlags.MaterialColor;
            }

            // Recompute the alpha test settings?
            if ((dirtyFlags & EffectDirtyFlags.AlphaTest) != 0)
            {
                var alphaTest = new Vector4();
                bool eqNe = false;
                
                // Convert reference alpha from 8 bit integer to 0-1 float format.
                float reference = (float)referenceAlpha / 255f;
                
                // Comparison tolerance of half the 8 bit integer precision.
                const float threshold = 0.5f / 255f;
                
                switch (alphaFunction)
                {
                    case Direct3D11.Comparison.Less:
                        // Shader will evaluate: clip((a < x) ? z : w)
                        alphaTest.X = reference - threshold;
                        alphaTest.Z = 1;
                        alphaTest.W = -1;
                        break;

                    case Direct3D11.Comparison.LessEqual:
                        // Shader will evaluate: clip((a < x) ? z : w)
                        alphaTest.X = reference + threshold;
                        alphaTest.Z = 1;
                        alphaTest.W = -1;
                        break;

                    case Direct3D11.Comparison.GreaterEqual:
                        // Shader will evaluate: clip((a < x) ? z : w)
                        alphaTest.X = reference - threshold;
                        alphaTest.Z = -1;
                        alphaTest.W = 1;
                        break;

                    case Direct3D11.Comparison.Greater:
                        // Shader will evaluate: clip((a < x) ? z : w)
                        alphaTest.X = reference + threshold;
                        alphaTest.Z = -1;
                        alphaTest.W = 1;
                        break;

                    case Direct3D11.Comparison.Equal:
                        // Shader will evaluate: clip((abs(a - x) < Y) ? z : w)
                        alphaTest.X = reference;
                        alphaTest.Y = threshold;
                        alphaTest.Z = 1;
                        alphaTest.W = -1;
                        eqNe = true;
                        break;

                    case Direct3D11.Comparison.NotEqual:
                        // Shader will evaluate: clip((abs(a - x) < Y) ? z : w)
                        alphaTest.X = reference;
                        alphaTest.Y = threshold;
                        alphaTest.Z = -1;
                        alphaTest.W = 1;
                        eqNe = true;
                        break;

                    case Direct3D11.Comparison.Never:
                        // Shader will evaluate: clip((a < x) ? z : w)
                        alphaTest.Z = -1;
                        alphaTest.W = -1;
                        break;

                    case Direct3D11.Comparison.Always:
                    default:
                        // Shader will evaluate: clip((a < x) ? z : w)
                        alphaTest.Z = 1;
                        alphaTest.W = 1;
                        break;
                }
                
                alphaTestParam.SetValue(alphaTest);

                dirtyFlags &= ~EffectDirtyFlags.AlphaTest;
                
                // If we changed between less/greater vs. equal/notequal
                // compare modes, we must also update the shader index.
                if (isEqNe != eqNe)
                {
                    isEqNe = eqNe;
                    dirtyFlags |= EffectDirtyFlags.ShaderIndex;
                }
            }

            // Recompute the shader index?
            if ((dirtyFlags & EffectDirtyFlags.ShaderIndex) != 0)
            {
                int shaderIndex = 0;
                
                if (!fogEnabled)
                    shaderIndex += 1;
                
                if (vertexColorEnabled)
                    shaderIndex += 2;
                
                if (isEqNe)
                    shaderIndex += 4;

                shaderPass = pass.SubPasses[shaderIndex];

                dirtyFlags &= ~EffectDirtyFlags.ShaderIndex;
            }

            return base.OnApply(shaderPass);
        }

        #endregion
    }
}
