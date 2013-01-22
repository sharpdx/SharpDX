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

namespace SharpDX.Toolkit.Graphics
{
    public sealed class DirectionalLight
    {
        internal readonly EffectParameter directionParameter;
        internal readonly EffectParameter diffuseColorParameter;
        internal readonly EffectParameter specularColorParameter;
        private Vector3 diffuseColor;
        private Vector3 direction;
        private Vector3 specularColor;
        private bool enabled;

        public DirectionalLight(EffectParameter directionParameter, EffectParameter diffuseColorParameter, EffectParameter specularColorParameter, DirectionalLight cloneSource)
        {
            this.diffuseColorParameter = diffuseColorParameter;
            this.directionParameter = directionParameter;
            this.specularColorParameter = specularColorParameter;
            if (cloneSource != null)
            {
                this.diffuseColor = cloneSource.diffuseColor;
                this.direction = cloneSource.direction;
                this.specularColor = cloneSource.specularColor;
                this.enabled = cloneSource.enabled;
            }
            else
            {
                this.diffuseColorParameter = diffuseColorParameter;
                this.directionParameter = directionParameter;
                this.specularColorParameter = specularColorParameter;
            }
        }

        public Vector3 DiffuseColor
        {
            get
            {
                return diffuseColor;
            }
            set
            {
                diffuseColor = value;
                if (this.enabled && this.diffuseColorParameter != null)
                    diffuseColorParameter.SetValue(diffuseColor);
            }
        }

        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
                if (this.directionParameter != null)
                    directionParameter.SetValue(direction);
            }
        }

        public Vector3 SpecularColor
        {
            get
            {
                return specularColor;
            }
            set
            {
                specularColor = value;
                if (this.enabled && this.specularColorParameter != null)
                    specularColorParameter.SetValue(specularColor);
            }
        }
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;
                    if (this.enabled)
                    {
                        if (this.diffuseColorParameter != null)
                        {
                            this.diffuseColorParameter.SetValue(this.diffuseColor);
                        }
                        if (this.specularColorParameter != null)
                        {
                            this.specularColorParameter.SetValue(this.specularColor);
                        }
                    }
                    else
                    {
                        if (this.diffuseColorParameter != null)
                        {
                            this.diffuseColorParameter.SetValue(Vector3.Zero);
                        }
                        if (this.specularColorParameter != null)
                        {
                            this.specularColorParameter.SetValue(Vector3.Zero);
                        }
                    }
                }

            }
        }
    }
}