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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes the material data attached to a <see cref="ModelMeshPart"/>. The material data is not bound to a particular effect.
    /// </summary>
    public class Material : ComponentBase
    {
        /// <summary>
        /// Gets the properties attached to this material. A list of standard keys are accessible from <see cref="TextureKeys"/>.
        /// </summary>
        public PropertyCollection Properties;

        /// <summary>
        /// Sets a property attached to this material. A list of standard keys are accessible from <see cref="TextureKeys"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetProperty<T>(PropertyKey<T> key, T value)
        {
            Properties.SetProperty(key, value);
        }

        /// <summary>
        /// Determines whether the specified key has property value. A list of standard keys are accessible from <see cref="TextureKeys"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key has property; otherwise, <c>false</c>.</returns>
        public bool HasProperty<T>(PropertyKey<T> key)
        {
            return Properties.ContainsKey(key);
        }

        /// <summary>
        /// Gets the property value for the specified key. A list of standard keys are accessible from <see cref="TextureKeys"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>``0.</returns>
        public T GetProperty<T>(PropertyKey<T> key)
        {
            return Properties.GetProperty(key);
        }

        /// <summary>
        /// Deep clone of this instance.
        /// </summary>
        /// <returns>A new copy of this Material.</returns>
        public virtual Material Clone()
        {
            var material = (Material)MemberwiseClone();

            if (Properties != null)
            {
                material.Properties = Properties.Clone();

                // Duplicate texture stacks
                foreach (var property in Properties)
                {
                    if (property.Value is MaterialTextureStack)
                    {
                        material.Properties[property.Key] = ((MaterialTextureStack)(property.Value)).Clone();
                    }
                }
            }

            return material;
        }
    }
}