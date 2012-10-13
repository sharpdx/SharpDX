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
using System.IO;
using SharpDX.IO;

namespace SharpDX.Toolkit.Content
{
    public class ContentManager : Component
    {
        private readonly Dictionary<string, object> assetLockers;
        private readonly Dictionary<string, object> loadedAssets;
        private readonly object registeredContentResolversLock = new object();
        private readonly object registeredContentReadersLock = new object();
        private List<IContentResolver> registeredContentResolvers;
        private List<IContentReader> registeredContentReaders;

        /// <summary>
        /// Initializes a new instance of ContentManager.  Reference page contains code sample.
        /// </summary>
        /// <param name="serviceProvider">The service provider that the ContentManager should use to locate services.</param>
        public ContentManager(IServiceProvider serviceProvider) : base("ContentManager")
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            ServiceProvider = serviceProvider;
            registeredContentReaders = new List<IContentReader>();
            registeredContentResolvers = new List<IContentResolver>();
            loadedAssets = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            assetLockers = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the service provider associated with the ContentManager.
        /// </summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetNameWithExtension">Full asset name (with its extension)</param>
        /// <returns>``0.</returns>
        /// <exception cref="SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="ContentResolver"/>.</exception>
        /// <exception cref="NotSupportedException">If no content reader was suitable to decode the asset.</exception>
        public virtual T Load<T>(string assetNameWithExtension)
        {
            assetNameWithExtension = PathUtility.GetNormalizedPath(assetNameWithExtension);

            object result = null;

            // Lock loading by asset name, like this, we can have several loading in multithreaded // with a single instance per assetname
            lock (GetAssetLocker(assetNameWithExtension))
            {
                // First, try to load the asset from the cache
                lock (loadedAssets)
                {
                    if (loadedAssets.TryGetValue(assetNameWithExtension, out result))
                    {
                        return (T) result;
                    }
                }

                // Else we need to load it from a content resolver disk/zip...etc.

                // First, resolve the stream for this asset.
                Stream stream = FindStream(assetNameWithExtension);
                if (stream == null)
                    throw new AssetNotFoundException(assetNameWithExtension);

                result = LoadAssetWithDynamicContentReader<T>(assetNameWithExtension, stream);

                // Cache the loaded assets
                lock (loadedAssets)
                {
                    loadedAssets.Add(assetNameWithExtension, result);

                    // If this asset is disposable, then add it to the list of object to dispose.
                    if (result is IDisposable)
                        ToDispose(result);
                }
            }

            // We could have an exception, but that's fine, as the user will be able to find why.
            return (T) result;
        }


        /// <summary>
        /// Registers a content reader.
        /// </summary>
        /// <param name="contentReader">The content reader.</param>
        public void RegisterContentReader(IContentReader contentReader)
        {
            lock (registeredContentReadersLock)
            {
                registeredContentReaders = new List<IContentReader>(registeredContentReaders) { contentReader };
            }
        }

        /// <summary>
        /// Unregister a content reader.
        /// </summary>
        /// <param name="contentReader">The content reader.</param>
        public void UnRegisterContentReader(IContentReader contentReader)
        {
            lock (registeredContentReadersLock)
            {
                var tempList = new List<IContentReader>(registeredContentReaders);
                tempList.Remove(contentReader);
                registeredContentReaders = tempList;
            }
        }

        public void RegisterContentResolver(IContentResolver contentResolver)
        {
            lock (registeredContentResolversLock)
            {
                registeredContentResolvers = new List<IContentResolver>(registeredContentResolvers) { contentResolver };
            }

        }

        public void UnRegisterContentResolver(IContentResolver contentResolver)
        {
            lock (registeredContentResolversLock)
            {
                var tempList = new List<IContentResolver>(registeredContentResolvers);
                tempList.Remove(contentResolver);
                registeredContentResolvers = tempList;
            }
        }

        private object GetAssetLocker(string assetNameWithExtension)
        {
            object assetLockerRead;
            lock (assetLockers)
            {
                if (!assetLockers.TryGetValue(assetNameWithExtension, out assetLockerRead))
                {
                    assetLockerRead = new object();
                    assetLockers.Add(assetNameWithExtension, assetLockerRead);
                }
            }
            return assetLockerRead;
        }

        private Stream FindStream(string assetNameWithExtension)
        {
            Stream stream = null;
            // First, resolve the stream for this asset.
            var resolvers = registeredContentResolvers;
            foreach (var contentResolver in resolvers)
            {
                stream = contentResolver.Resolve(assetNameWithExtension);
                if (stream != null)
                    break;
            }
            return stream;
        }

        private object LoadAssetWithDynamicContentReader<T>(string assetNameWithExtension, Stream stream)
        {
            object result = null;

            long startPosition = stream.Position;
            bool keepStreamOpen = false;

            // Try to load from registered content readers
            List<IContentReader> readers = registeredContentReaders;
            foreach (IContentReader registeredContentReader in readers)
            {
                // Rewind position everytime we try to load an asset
                result = registeredContentReader.ReadContent(this, assetNameWithExtension, stream, out keepStreamOpen);
                stream.Position = startPosition;
                if (result != null)
                    break;
            }

            if (result == null)
            {
                // Else try to load using a dynamic content reader attribute
                var contentReaderAttribute = Utilities.GetCustomAttribute<ContentReaderAttribute>(typeof (T), true);
                if (contentReaderAttribute == null)
                    throw new NotSupportedException("No content reader registered or found for this asset");

                object contentReaderAbstract = Activator.CreateInstance(contentReaderAttribute.ContentReaderType);
                var contentReader = contentReaderAbstract as IContentReader;
                if (contentReader == null)
                    throw new NotSupportedException(string.Format("Invalid content reader type [{0}]. Expecting an instance of IContentReader", contentReaderAbstract.GetType().FullName));

                // Rewind position everytime we try to load an asset
                stream.Position = startPosition;
                result = contentReader.ReadContent(this, assetNameWithExtension, stream, out keepStreamOpen);
                stream.Position = startPosition;
                if (result == null)
                    throw new NotSupportedException("Unable to load content");

                // If this content reader has been used successfully, then we can register it.
                RegisterContentReader(contentReader);
            }

            // If we don't need to keep the stream open, then we can close it
            if (!keepStreamOpen)
                stream.Close();

            return result;
        }
    }
}