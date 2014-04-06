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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SharpDX.Collections;

namespace SharpDX.Toolkit.Content
{
    /// <summary>
    /// The content manager implementation is responsible to load and store content data (texture, songs, effects...etc.) using 
    /// several <see cref="IContentResolver"/> to resolve a stream from an asset name and several registered <see cref="IContentReader"/>
    /// to convert data from stream.
    /// </summary>
    public class ContentManager : Component, IContentManager
    {
        private readonly Dictionary<AssetKey, object> assetLockers;
        private readonly List<IContentResolver> registeredContentResolvers;
        private readonly Dictionary<Type, IContentReader> registeredContentReaders;
        private readonly List<IContentReaderFactory> registeredContentReaderFactories;
        protected readonly Dictionary<AssetKey, object> loadedAssets;

        private string rootDirectory;

        /// <summary>
        /// Initializes a new instance of ContentManager. Reference page contains code sample.
        /// </summary>
        /// <param name="serviceProvider">The service provider that the ContentManager should use to locate services.</param>
        public ContentManager(IServiceProvider serviceProvider)
            : base("ContentManager")
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            ServiceProvider = serviceProvider;

            // Content resolvers
            Resolvers = new ObservableCollection<IContentResolver>();
            Resolvers.ItemAdded += ContentResolvers_ItemAdded;
            Resolvers.ItemRemoved += ContentResolvers_ItemRemoved;
            registeredContentResolvers = new List<IContentResolver>();

            // Content readers.
            Readers = new ObservableDictionary<Type, IContentReader>();
            Readers.ItemAdded += ContentReaders_ItemAdded;
            Readers.ItemRemoved += ContentReaders_ItemRemoved;
            registeredContentReaders = new Dictionary<Type, IContentReader>();

            // Content Reader Factories
            ReaderFactories = new ObservableCollection<IContentReaderFactory>();
            ReaderFactories.ItemAdded += ReaderFactories_ItemAdded;
            ReaderFactories.ItemRemoved += ReaderFactories_ItemRemoved;
            registeredContentReaderFactories = new List<IContentReaderFactory>();

            loadedAssets = new Dictionary<AssetKey, object>();
            assetLockers = new Dictionary<AssetKey, object>();
        }

        /// <summary>
        /// Add or remove registered <see cref="IContentResolver"/> to this instance.
        /// </summary>
        public ObservableCollection<IContentResolver> Resolvers { get; private set; }

        /// <summary>
        /// Add or remove registered <see cref="IContentReader"/> to this instance.
        /// </summary>
        public ObservableDictionary<Type, IContentReader> Readers { get; private set; }

        /// <summary>
        /// Add or remove a registered <see cref="IContentReaderFactory"/> to this instance.
        /// </summary>
        /// <value>The reader factories.</value>
        public ObservableCollection<IContentReaderFactory> ReaderFactories { get; private set; }

        /// <summary>
        /// Gets the service provider associated with the ContentManager.
        /// </summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; protected set; }

        /// <summary>
        /// Gets or sets the root directory.
        /// </summary>
        public string RootDirectory
        {
            get
            {
                return rootDirectory;
            }

            set
            {
                if (loadedAssets.Count > 0)
                {
                    throw new InvalidOperationException("RootDirectory cannot be changed when a ContentManager has already assets loaded");
                }

                rootDirectory = value;
            }
        }

        /// <summary>
        /// Checks if the specified assets exists.
        /// </summary>
        /// <param name="assetName">The asset name with extension.</param>
        /// <returns><c>true</c> if the specified assets exists, <c>false</c> otherwise</returns>
        public virtual bool Exists(string assetName)
        {
            if (assetName == null) throw new ArgumentNullException("assetName");

            // First, resolve the stream for this asset.
            List<IContentResolver> resolvers;
            lock (registeredContentResolvers)
            {
                resolvers = new List<IContentResolver>(registeredContentResolvers);
            }

            if (resolvers.Count == 0)
            {
                throw new InvalidOperationException("No resolver registered to this content manager");
            }

            string assetPath = Path.Combine(rootDirectory ?? string.Empty, assetName);

            foreach (var contentResolver in resolvers)
            {
                if (contentResolver.Exists(assetPath)) return true;
            }

            return false;
        }

        /// <summary>
        /// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
        /// </summary>
        /// <typeparam name="T">Type of the asset</typeparam>
        /// <param name="assetName">The asset name </param>
        /// <param name="options">The options to pass to the content reader (null by default).</param>
        /// <returns>``0.</returns>
        /// <exception cref="SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="IContentResolver" />.</exception>
        /// <exception cref="NotSupportedException">If no content reader was suitable to decode the asset.</exception>
        public virtual T Load<T>(string assetName, object options = null)
        {
            return (T)Load(typeof(T), assetName, options);
        }

        /// <summary>
        /// Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.
        /// </summary>
        /// <param name="assetType"Asset Type</param>
        /// <param name="assetName">The asset name </param>
        /// <param name="options">The options to pass to the content reader (null by default).</param>
        /// <returns>Asset</returns>
        /// <exception cref="SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="IContentResolver" />.</exception>
        /// <exception cref="NotSupportedException">If no content reader was suitable to decode the asset.</exception>
        public virtual object Load(Type assetType, string assetName, object options = null)
        {
            if (assetName == null) throw new ArgumentNullException("assetName");
            if (assetType == null) throw new ArgumentNullException("assetType");

            object result = null;

            // Build asset key
            var assetKey = new AssetKey(assetType, assetName);

            // Lock loading by asset name, like this, we can have several loading in multithreaded // with a single instance per asset name
            lock (GetAssetLocker(assetKey, true))
            {
                // First, try to load the asset from the cache
                lock (loadedAssets)
                {
                    if (loadedAssets.TryGetValue(assetKey, out result))
                    {
                        return result;
                    }
                }

                // Else we need to load it from a content resolver disk/zip...etc.
                string assetPath = Path.Combine(rootDirectory ?? string.Empty, assetName);

                // First, resolve the stream for this asset.
                var stream = FindStream(assetPath);

                result = LoadAssetWithDynamicContentReader(assetType, assetName, stream, options);

                // Cache the loaded assets
                lock (loadedAssets)
                {
                    loadedAssets.Add(assetKey, result);
                }
            }

            // We could have an exception, but that's fine, as the user will be able to find why.
            return result;
        }


        /// <summary>
        /// Unloads all data that was loaded by this ContentManager. All data will be disposed.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="Load{T}"/> method, this method is not thread safe and must be called by a single caller at a single time.
        /// </remarks>
        public virtual void Unload()
        {
            lock (assetLockers)
            {
                lock (loadedAssets)
                {
                    foreach (var loadedAsset in loadedAssets.Values)
                    {
                        var disposable = loadedAsset as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }

                    assetLockers.Clear();
                    loadedAssets.Clear();
                }
            }
        }

        /// <summary>
        ///	Unloads and disposes an asset.
        /// </summary>
        /// <param name="assetName">The asset name</param>
        /// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
        public virtual bool Unload<T>(string assetName)
        {
            return Unload(typeof(T), assetName);
        }

        /// <summary>
        /// Unloads and disposes an asset.
        /// </summary>
        /// <param name="assetType">The asset type</param>
        /// <param name="assetName">The asset name</param>
        /// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
        public virtual bool Unload(Type assetType, string assetName)
        {
            if (assetType == null) throw new ArgumentNullException("assetType");
            if (assetName == null) throw new ArgumentNullException("assetName");
            object asset;

            // Build asset key
            var assetKey = new AssetKey(assetType, assetName);

            object assetLockerRead = GetAssetLocker(assetKey, false);
            if (assetLockerRead == null)
                return false;

            lock (assetLockerRead)
            {
                lock (loadedAssets)
                {
                    if (!loadedAssets.TryGetValue(assetKey, out asset))
                        return false;
                    loadedAssets.Remove(assetKey);
                }

                lock (assetLockers)
                    assetLockers.Remove(assetKey);
            }

            var disposable = asset as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            return true;
        }

        protected object GetAssetLocker(AssetKey assetKey, bool create)
        {
            object assetLockerRead;
            lock (assetLockers)
            {
                if (!assetLockers.TryGetValue(assetKey, out assetLockerRead) && create)
                {
                    assetLockerRead = new object();
                    assetLockers.Add(assetKey, assetLockerRead);
                }
            }
            return assetLockerRead;
        }

        private Stream FindStream(string assetName)
        {
            Stream stream = null;
            // First, resolve the stream for this asset.
            List<IContentResolver> resolvers;
            lock (registeredContentResolvers)
            {
                resolvers = new List<IContentResolver>(registeredContentResolvers);
            }

            if (resolvers.Count == 0)
            {
                throw new InvalidOperationException("No resolver registered to this content manager");
            }

            Exception lastException = null;
            foreach (var contentResolver in resolvers)
            {
                try
                {
                    if (contentResolver.Exists(assetName))
                    {
                        stream = contentResolver.Resolve(assetName);
                        if (stream != null)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                    lastException = ex;
                }
            }

            if (stream == null)
                throw new AssetNotFoundException(assetName, lastException);

            return stream;
        }

        private object LoadAssetWithDynamicContentReader(Type assetType, string assetName, Stream stream, object options)
        {
            object result;


            var parameters = new ContentReaderParameters
                             {
                                 AssetName = assetName,
                                 AssetType = assetType,
                                 Stream = stream,
                                 Options = options
                             };

            try
            {
                IContentReader contentReader;
                lock (registeredContentReaders)
                {
                    if (!registeredContentReaders.TryGetValue(assetType, out contentReader))
                    {
                        // Use registered factories to handle a type
                        lock (registeredContentReaderFactories)
                        {
                            foreach (var factory in registeredContentReaderFactories)
                            {
                                contentReader = factory.TryCreate(assetType);
                                if (contentReader != null)
                                {
                                    registeredContentReaders.Add(assetType, contentReader);
                                    break;
                                }
                            }
                        }

                        // Else tries to get a ContentReaderAttribute to resolve the type
                        if (contentReader == null)
                        {
#if WIN8METRO
                            var contentReaderAttribute = Utilities.GetCustomAttribute<ContentReaderAttribute>(assetType.GetTypeInfo(), true);
#else
                            var contentReaderAttribute = Utilities.GetCustomAttribute<ContentReaderAttribute>(assetType, true);
#endif

                            if (contentReaderAttribute != null)
                            {
                                contentReader = Activator.CreateInstance(contentReaderAttribute.ContentReaderType) as IContentReader;
                                if (contentReader != null)
                                    Readers.Add(assetType, contentReader);
                            }
                        }
                    }
                }

                if (contentReader == null)
                {
                    throw new NotSupportedException(string.Format("Type [{0}] doesn't provide a ContentReaderAttribute, and there is no registered content reader/factory for it.", assetType.FullName));
                }

                result = contentReader.ReadContent(this, ref parameters);

                if (result == null)
                {
                    throw new NotSupportedException(string.Format("Registered ContentReader of type [{0}] fails to load content of type [{1}] from file [{2}].", contentReader.GetType(), assetType.FullName, assetName));
                }
            }
            finally
            {
                // If we don't need to keep the stream open, then we can close it
                // and make sure that we will close the stream even if there is an exception.
                if (!parameters.KeepStreamOpen)
                    stream.Dispose();
            }

            return result;
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                Unload();
            }

            base.Dispose(disposeManagedResources);
        }

        private void ContentResolvers_ItemAdded(object sender, ObservableCollectionEventArgs<IContentResolver> e)
        {
            if (e.Item == null)
                throw new ArgumentNullException("Cannot add a null IContentResolver", "value");

            lock (registeredContentResolvers)
            {
                registeredContentResolvers.Add(e.Item);
            }
        }

        private void ContentResolvers_ItemRemoved(object sender, ObservableCollectionEventArgs<IContentResolver> e)
        {
            lock (registeredContentResolvers)
            {
                registeredContentResolvers.Remove(e.Item);
            }
        }

        private void ContentReaders_ItemAdded(object sender, ObservableDictionaryEventArgs<Type, IContentReader> e)
        {
            if (e.Key == null || e.Value == null)
                throw new ArgumentNullException("Cannot add a null Type/IContentReader", "value");

            lock (registeredContentReaders)
            {
                registeredContentReaders.Add(e.Key, e.Value);
            }
        }

        private void ContentReaders_ItemRemoved(object sender, ObservableDictionaryEventArgs<Type, IContentReader> e)
        {
            lock (registeredContentReaders)
            {
                registeredContentReaders.Remove(e.Key);
            }
        }

        void ReaderFactories_ItemAdded(object sender, ObservableCollectionEventArgs<IContentReaderFactory> e)
        {
            if (e.Item == null)
                throw new ArgumentNullException("Cannot add a null IContentReader", "value");

            lock (registeredContentReaderFactories)
            {
                registeredContentReaderFactories.Add(e.Item);
            }
        }

        void ReaderFactories_ItemRemoved(object sender, ObservableCollectionEventArgs<IContentReaderFactory> e)
        {
            lock (registeredContentReaderFactories)
            {
                registeredContentReaderFactories.Remove(e.Item);
            }
        }

        /// <summary>
        /// Use this key to store loaded assets.
        /// </summary>
        protected struct AssetKey : IEquatable<AssetKey>
        {
            public AssetKey(Type assetType, string assetName)
            {
                AssetType = assetType;
                AssetName = assetName;
            }

            public readonly Type AssetType;

            public readonly string AssetName;

            public bool Equals(AssetKey other)
            {
                return AssetType == other.AssetType && string.Equals(AssetName, other.AssetName, StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is AssetKey && Equals((AssetKey)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (AssetType.GetHashCode() * 397) ^ AssetName.GetHashCode();
                }
            }

            public static bool operator ==(AssetKey left, AssetKey right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(AssetKey left, AssetKey right)
            {
                return !left.Equals(right);
            }
        }
    }
}
