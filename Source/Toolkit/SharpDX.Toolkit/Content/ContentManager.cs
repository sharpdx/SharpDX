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
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
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
        /// <summary>The asset lockers.</summary>
        private readonly Dictionary<string, object> assetLockers;

        /// <summary>The loaded assets.</summary>
        private readonly Dictionary<string, object> loadedAssets;

        /// <summary>The registered content resolvers.</summary>
        private readonly List<IContentResolver> registeredContentResolvers;

        /// <summary>The registered content readers.</summary>
        private readonly Dictionary<Type, IContentReader> registeredContentReaders;

        /// <summary>The root directory.</summary>
        private string rootDirectory;

        /// <summary>The full directory.</summary>
        private string fullDirectory;

        /// <summary>Initializes a new instance of ContentManager. Reference page contains code sample.</summary>
        /// <param name="serviceProvider">The service provider that the ContentManager should use to locate services.</param>
        /// <param name="rootDirectory">The root directory.</param>
        /// <exception cref="System.ArgumentNullException">serviceProvider</exception>
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory = "")
            : base("ContentManager")
        {
            if(serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            
            if(rootDirectory == null)
            {
                throw new ArgumentNullException("rootDirectory");
            }
            
            this.RootDirectory = rootDirectory;
            this.ServiceProvider = serviceProvider;

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

            loadedAssets = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            assetLockers = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>Add or remove registered <see cref="IContentResolver" /> to this instance.</summary>
        /// <value>The resolvers.</value>
        public ObservableCollection<IContentResolver> Resolvers { get; private set; }

        /// <summary>Add or remove registered <see cref="IContentReader" /> to this instance.</summary>
        /// <value>The readers.</value>
        public ObservableDictionary<Type, IContentReader> Readers { get; private set; }

        /// <summary>Gets the service provider associated with the ContentManager.</summary>
        /// <value>The service provider.</value>
        /// <remarks>The service provider can be used by some <see cref="IContentReader" /> when for example a <see cref="SharpDX.Toolkit.Graphics.GraphicsDevice" /> needs to be
        /// used to instantiate a content.</remarks>
        public IServiceProvider ServiceProvider { get; protected set; }

        /// <summary>Gets or sets the root directory.</summary>
        /// <value>The root directory.</value>
        /// <exception cref="System.InvalidOperationException">RootDirectory cannot be changed when a ContentManager has already assets loaded.</exception>
        public string RootDirectory
        {
            get
            {
                return rootDirectory;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                
                if (loadedAssets.Count > 0)
                {
                    throw new InvalidOperationException("RootDirectory cannot be changed when a ContentManager has already assets loaded");
                }

                this.rootDirectory = value;

                this.fullDirectory = Path.Combine(GameLocationPath, this.rootDirectory);
            }
        }

        /// <summary>Gets the game location path.</summary>
        /// <value>The game location path.</value>
        public static string GameLocationPath
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                if(assembly == null)
                {
                    assembly = Assembly.GetCallingAssembly();
                }

                return Path.GetDirectoryName(assembly.Location);
            }
        }

        /// <summary>Checks if the specified assets exists.</summary>
        /// <param name="assetName">The asset name with extension.</param>
        /// <returns><c>true</c> if the specified assets exists, <c>false</c> otherwise</returns>
        /// <exception cref="System.InvalidOperationException">No resolver registered to this content manager</exception>
        public virtual bool Exists(string assetName)
        {
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

        /// <summary>Loads an asset that has been processed by the Content Pipeline.  Reference page contains code sample.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="assetName">The asset name</param>
        /// <param name="options">The options to pass to the content reader (null by default).</param>
        /// <returns>the loaded asset.</returns>
        /// <exception cref="AssetNotFoundException"></exception>
        /// <exception cref="SharpDX.Toolkit.Content.AssetNotFoundException">If the asset was not found from all <see cref="IContentResolver" />.</exception>
        /// <exception cref="NotSupportedException">If no content reader was suitable to decode the asset.</exception>
        public virtual T Load<T>(string assetName, object options = null)
        {
            if (this.loadedAssets == null)
                throw new ObjectDisposedException(this.ToString());
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException("assetName");

            object result;

            // Lock loading by asset name, like this, we can have several loading in multi threaded with a single instance per asset name.
            lock(GetAssetLocker(assetName, true))
            {
                // First, try to load the asset from the cache
                lock(this.loadedAssets)
                {
                    if(this.loadedAssets.TryGetValue(assetName, out result))
                    {
                        return (T)result;
                    }
                }

                // Else we need to load it from a content resolver disk/zip...etc.
                string assetPath = GetCleanPath(Path.Combine(this.fullDirectory, assetName));

                // First, resolve the stream for this asset.
                Stream stream = FindStream(assetPath);
                if(stream == null)
                {
                    throw new AssetNotFoundException("Unable to find " + assetPath + ".");
                }

                result = LoadAssetWithDynamicContentReader<T>(assetName, stream, options);

                // Cache the loaded assets
                lock (this.loadedAssets)
                {
                    this.loadedAssets.Add(assetName, result);
                }
            }

            // We could have an exception, but that's fine, as the user will be able to find why.
            return (T)result;
        }

        /// <summary>Unloads all data that was loaded by this ContentManager. All data will be disposed.</summary>
        /// <remarks>Unlike <see cref="ContentManager.Load{T}" /> method, this method is not thread safe and must be called by a single caller at a single time.</remarks>
        public virtual void Unload()
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

        /// <summary>Unloads and disposes an asset.</summary>
        /// <param name="assetName">The asset name</param>
        /// <returns><c>true</c> if the asset exists and was unloaded, <c>false</c> otherwise.</returns>
        public virtual bool Unload(string assetName)
        {
            object asset;

            object assetLockerRead = GetAssetLocker(assetName, false);
            if (assetLockerRead == null)
                return false;

            lock (assetLockerRead)
            {
                lock (loadedAssets)
                {
                    if (!loadedAssets.TryGetValue(assetName, out asset))
                        return false;
                    loadedAssets.Remove(assetName);
                }

                lock (assetLockers)
                    assetLockers.Remove(assetName);
            }

            var disposable = asset as IDisposable;
            if (disposable != null)
                disposable.Dispose();

            return true;
        }

        /// <summary>Gets the clean path.</summary>
        /// <param name="path">The path.</param>
        /// <returns>System.String.</returns>
        internal static string GetCleanPath(string path)
        {
            string result = path.Replace('/', '\\');
            result = result.Replace("\\.\\", "\\");
            while (result.StartsWith(".\\", StringComparison.Ordinal))
            {
                result = result.Substring(".\\".Length);
            }
            
            while(result.EndsWith("\\.", StringComparison.Ordinal))
            {
                result = result.Length <= "\\.".Length ? "\\" : result.Substring(0, result.Length - "\\.".Length);
            }
            
            int position;
            for (int startIndex = 1; startIndex < result.Length; startIndex = CollapseParentDirectory(ref result, position, "\\..\\".Length) )
            {
                position = result.IndexOf("\\..\\", startIndex, StringComparison.Ordinal);
                if(position < 0)
                {
                    break;
                }
            }
            
            if (result.EndsWith("\\..", StringComparison.Ordinal))
            {
                position = result.Length - "\\..".Length;
                if(position > 0)
                {
                    CollapseParentDirectory(ref result, position, "\\..".Length);
                }
            }
            
            if(result == ".")
            {
                result = string.Empty;
            }
            
            return result;
        }

        /// <summary>Collapses the parent directory.</summary>
        /// <param name="path">The path.</param>
        /// <param name="position">The position.</param>
        /// <param name="removeLength">Length of the remove.</param>
        /// <returns>System.Int32.</returns>
        private static int CollapseParentDirectory(ref string path, int position, int removeLength)
        {
            int startIndex = path.LastIndexOf('\\', position - 1) + 1;
            path = path.Remove(startIndex, position - startIndex + removeLength);
            return Math.Max(startIndex - 1, 1);
        }

        /// <summary>Gets the asset locker.</summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="create">if set to <see langword="true" /> [create].</param>
        /// <returns>System.Object.</returns>
        private object GetAssetLocker(string assetName, bool create)
        {
            object assetLockerRead;
            lock (assetLockers)
            {
                if (!assetLockers.TryGetValue(assetName, out assetLockerRead) && create)
                {
                    assetLockerRead = new object();
                    assetLockers.Add(assetName, assetLockerRead);
                }
            }
            return assetLockerRead;
        }

        /// <summary>Finds the stream.</summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns>Stream.</returns>
        /// <exception cref="System.InvalidOperationException">No resolver registered to this content manager</exception>
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

            foreach (var contentResolver in resolvers)
            {
                stream = contentResolver.Resolve(assetName);
                if (stream != null)
                    break;
            }
            return stream;
        }

        /// <summary>Loads the asset with dynamic content reader.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotSupportedException">Type T doesn't provide a ContentReaderAttribute, and there is no registered content reader for it or fails.</exception>
        private object LoadAssetWithDynamicContentReader<T>(string assetName, Stream stream, object options)
        {
            object result;
            var type = typeof(T);

            var parameters = new ContentReaderParameters
                             {
                                     AssetName = assetName,
                                     AssetType = type,
                                     Stream = stream,
                                     Options = options
                                 };

            try
            {
                IContentReader contentReader;
                lock (registeredContentReaders)
                {
                    if (!registeredContentReaders.TryGetValue(type, out contentReader))
                    {
#if WIN8METRO
                        var contentReaderAttribute = Utilities.GetCustomAttribute<ContentReaderAttribute>(type.GetTypeInfo(), true);
#else
                        var contentReaderAttribute = Utilities.GetCustomAttribute<ContentReaderAttribute>(type, true);
#endif

                        if (contentReaderAttribute != null)
                        {
                            contentReader = Activator.CreateInstance(contentReaderAttribute.ContentReaderType) as IContentReader;
                            if(contentReader != null)
                            {
                                Readers.Add(typeof(T), contentReader);
                            }
                        }
                    }
                }

                if (contentReader == null)
                {
                    throw new NotSupportedException(string.Format("Type [{0}] doesn't provide a ContentReaderAttribute, and there is no registered content reader for it.", type.FullName));
                }

                result = contentReader.ReadContent(this, ref parameters);

                if (result == null)
                {
                    throw new NotSupportedException(string.Format("Registered ContentReader of type [{0}] fails to load content of type [{1}] from file [{2}].", contentReader.GetType(), type.FullName, assetName));
                }
            }
            finally
            {
                // If we don't need to keep the stream open, then we can close it
                // and make sure that we will close the stream even if there is an exception.
                if(!parameters.KeepStreamOpen)
                {
                    stream.Dispose();
                }
            }

            return result;
        }

        /// <summary>Disposes of object resources.</summary>
        /// <param name="disposeManagedResources">If true, managed resources should be
        /// disposed of in addition to unmanaged resources.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                Unload();
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>ContentResolvers item added.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The decimal.</param>
        private void ContentResolvers_ItemAdded(object sender, ObservableCollectionEventArgs<IContentResolver> e)
        {
            lock (registeredContentResolvers)
            {
                registeredContentResolvers.Add(e.Item);
            }
        }

        /// <summary>ContentResolvers item removed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The decimal.</param>
        private void ContentResolvers_ItemRemoved(object sender, ObservableCollectionEventArgs<IContentResolver> e)
        {
            lock (registeredContentResolvers)
            {
                registeredContentResolvers.Remove(e.Item);
            }
        }

        /// <summary>ContentResolvers item added.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The decimal.</param>
        private void ContentReaders_ItemAdded(object sender, ObservableDictionaryEventArgs<Type, IContentReader> e)
        {
            lock (registeredContentReaders)
            {
                registeredContentReaders.Add(e.Key, e.Value);
            }
        }

        /// <summary>ContentResolvers item removed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The decimal.</param>
        private void ContentReaders_ItemRemoved(object sender, ObservableDictionaryEventArgs<Type, IContentReader> e)
        {
            lock (registeredContentReaders)
            {
                registeredContentReaders.Remove(e.Key);
            }
        }
    }
}
