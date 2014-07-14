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

using System;
using System.Diagnostics;
using System.IO;
using SharpDX.Toolkit.Content;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Internal class to load a SpriteFont.
    /// </summary>
    internal class ModelContentReader : GraphicsResourceContentReaderBase<Model>
    {
        protected override Model ReadContent(IContentManager readerManager, GraphicsDevice device, ref ContentReaderParameters parameters)
        {
            var readerOptions = parameters.Options as ModelContentReaderOptions;
            if (parameters.Options != null && readerOptions == null)
            {
                throw new ArgumentException("Invalid options. Must be instance of ModelContentReaderOptions", "options");
            }

            var assetPath = Path.GetDirectoryName(parameters.AssetName);

            // Loads the model.
            var model = Model.Load(device, parameters.Stream, name =>
                {
                    // Try to load the texture with its texture path as is
                    // otherwise try to load with tkb extension
                    var texturePath = Path.Combine(assetPath ?? string.Empty, name);
                    if (!readerManager.Exists(texturePath))
                    {
                        // Use the extension tkb to load a texture
                        texturePath = Path.ChangeExtension(texturePath, "tkb");
                    }

                    // If the texture exists, return it, otherwise return null without throwing an exception.
                    return readerManager.Exists(texturePath) ? readerManager.Load<Texture>(texturePath) : null;
                });

            if (model == null)
            {
                return null;
            }

            // If the model has no name, use filename
            if (model.Name == null)
            {
                model.Name = Path.GetFileName(parameters.AssetName);
            }

            // Applies the Effect installer on the model.
            bool disableInstaller = readerOptions != null && readerOptions.DisableEffectInstaller;
            if (!disableInstaller)
            {
                IEffectInstaller effectInstaller;
                if (readerOptions != null && readerOptions.EffectInstaller != null)
                {
                    effectInstaller = readerOptions.EffectInstaller;
                }
                else
                {
                    effectInstaller = new BasicEffectInstaller(device);
                }

                effectInstaller.Apply(model);
            }

            return model;
        }
    }
}