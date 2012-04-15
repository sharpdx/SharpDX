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
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using SharpCore.Logging;
using SharpCore.MTPS;

namespace SharpCore
{
    /// <summary>
    /// Client to MTPS service.
    /// </summary>
    public class MsdnRegistry
    {
        private Dictionary<string,string> _mapAssetIdToContentId = new Dictionary<string, string>();
        private ContentServicePortTypeClient msdnClient;
        private const string MsdnUrlFormat = "http://msdn2.microsoft.com/{0}/library/{1}";
        private const string MtpsService = "http://services.msdn.microsoft.com/ContentServices/ContentService.asmx";
        private string MsdnRegistryCachingFile;
        private const string MsdnRegistryCachingFilePostfix = "msdn.cache";
        public const string DefaultLocale = "en-us";

        /// <summary>
        /// Initializes a new instance of the <see cref="MsdnRegistry"/> class.
        /// </summary>
        public MsdnRegistry()
        {
            // Use caching file per application using it
            AppId = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            string directory = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SharpDX"), AppId);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            MsdnRegistryCachingFile = Path.Combine(directory, MsdnRegistryCachingFilePostfix);

            Locale = DefaultLocale;
            var endpointAddress = new EndpointAddress(MtpsService);
            var binding = new BasicHttpBinding();
            msdnClient = new ContentServicePortTypeClient(binding, endpointAddress);
            LoadCacheFromDisk();
        }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <value>The locale.</value>
        public string Locale { get; set;}

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        /// <value>The app id.</value>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Finds an URL from MSDN documentation.
        /// </summary>
        /// <param name="assetId">The asset id in the form "T:System.String"</param>
        /// <returns>An url to MSDN or null if no url was found</returns>
        public string FindUrl(string assetId)
        {
            // Only look for T:System. 
            if (assetId.Length < 3 || !assetId.Substring(2).StartsWith("System.") || IsDisabled)
                return null;

            string contentId;
            if (!_mapAssetIdToContentId.TryGetValue(assetId, out contentId))
            {
                try
                {
                    Logger.Message("Get MSDN URL for id [{0}]", assetId);
                    var request = new getContentRequest { contentIdentifier = "AssetId:" + HttpUtility.UrlEncode(assetId), locale = Locale };

                    var appId = new appId() { value = AppId };
                    var response = msdnClient.GetContent(appId, request);
                    contentId = response.contentId;
                }
                catch (EndpointNotFoundException endpointNotFound)
                {
                    Logger.Warning("Unable to connect to MTPS service. Msnd resolver disabled : {0}", endpointNotFound.Message);
                    IsDisabled = true;
                }
                catch (Exception ex)
                {
                    Logger.Warning("Error while getting msdn url [{0}] : {1}", assetId, ex.Message);
                }
                _mapAssetIdToContentId[assetId] = contentId;

                SaveCacheToDisk();
            }

            if (string.IsNullOrEmpty(contentId))
                return null;

            return string.Format(MsdnUrlFormat, Locale, contentId);
        }

        /// <summary>
        /// Loads the cache.
        /// </summary>
        private void LoadCacheFromDisk()
        {
            FileStream cachingFile = null;
            try
            {
                cachingFile = new FileStream(MsdnRegistryCachingFile, FileMode.Open);
                var datacontractSerializer = new DataContractSerializer(typeof(Dictionary<string, string>));
                _mapAssetIdToContentId = (Dictionary<string,string>) datacontractSerializer.ReadObject(cachingFile);
            } catch (Exception ex)
            {
                // Don't log any exceptions
            } finally
            {
                if (cachingFile != null)
                    cachingFile.Close();
            }
            if (_mapAssetIdToContentId == null)
                _mapAssetIdToContentId = new Dictionary<string, string>();             
        }

        /// <summary>
        /// Writes the cache.
        /// </summary>
        private void SaveCacheToDisk()
        {
            FileStream cachingFile = null;
            try
            {
                cachingFile = new FileStream(MsdnRegistryCachingFile, FileMode.Create);
                var datacontractSerializer = new DataContractSerializer(typeof (Dictionary<string, string>));
                datacontractSerializer.WriteObject(cachingFile, _mapAssetIdToContentId);   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Don't log any exceptions
            }
            finally
            {
                if (cachingFile != null)
                    cachingFile.Close();
            }
        }
    }
}