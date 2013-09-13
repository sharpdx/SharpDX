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

using System;

namespace SharpDX.Toolkit.Content
{
    /// <summary>
    /// The content manager interface provides a service to load and store content data (texture, songs, effects...etc.).
    /// </summary>
    public interface IContentManager
    {
        /// <summary>
        /// Gets the service provider associated with the ContentManager.
        /// </summary>
        /// <value>The service provider.</value>
        /// <remarks>
        /// The service provider can be used by some <see cref="IContentReader"/> when for example a <see cref="SharpDX.Toolkit.Graphics.GraphicsDevice"/> needs to be 
        /// used to instantiate a content.
        /// </remarks>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Checks if the specified assets exists.
        /// </summary>
        /// <param name="assetName">The asset name with extension.</param>
        /// <returns><c>true</c> if the specified assets exists, <c>false</c> otherwise</returns>
        bool Exists(string assetName);

        /// <summary>
        /// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName">Full asset name (with its extension)</param>
        /// <param name="options">The options to pass to the content reader (null by default).</param>
        /// <returns>``0.</returns>
        /// <exception cref="SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="IContentResolver" />.</exception>
        /// <exception cref="System.NotSupportedException">If no content reader was suitable to decode the asset.</exception>
        T Load<T>(string assetName, object options = null);

        /// <summary>
        /// Associates a type with a reader.  If a default reader is specified for a type, this will override it.
        /// </summary>
        /// <typeparam name="T">The type that this <see cref="IContentReader"/> is able to read</typeparam>
        /// <param name="reader">The <see cref="IContentReader" /> to use when reading this type</param>
        void Register<T>(IContentReader reader);

        /// <summary>
        /// Dissociates a type from the reader if one exists. If the type is loaded after this call, then
        /// the IContentManager should attempt to find the relevant reader for the type using ContentReaderAttribute
        /// </summary>
        /// <typeparam name="T">The type to Unregister</typeparam>
        void Unregister<T>();

        /// <summary>
        /// Unloads all data that was loaded by this ContentManager. All data will be disposed.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="ContentManager.Load{T}"/> method, this method is not thread safe and must be called by a single caller at a single time.
        /// </remarks>
        void Unload();

        /// <summary>
        ///	Unloads and disposes an asset.
        /// </summary>
        /// <param name="assetName">The asset name</param>
        /// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
        bool Unload(string assetName);
    }
}