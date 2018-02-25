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
namespace SharpDX.MediaFoundation
{
    public partial class SourceResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceResolver"/> class which is used to create a media source from a 
        /// URL or byte stream.
        /// </summary>
        /// <msdn-id>ms697433</msdn-id>	
        /// <unmanaged>HRESULT MFCreateSourceResolver([Out] IMFSourceResolver** ppISourceResolver)</unmanaged>	
        /// <unmanaged-short>MFCreateSourceResolver</unmanaged-short>	
        public SourceResolver()
        {
            MediaFactory.CreateSourceResolver(this);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Creates a media source or a byte stream from a URL. This method is synchronous. </p>	
        /// </summary>	
        /// <param name="url"><dd> <p> Null-terminated string that contains the URL to resolve. </p> </dd></param>	
        /// <param name="flags"><dd> <p> Bitwise OR of one or more flags. See <strong>Source Resolver Flags</strong>. </p> </dd></param>	
        /// <returns>A reference to the object's <strong><see cref="SharpDX.ComObject"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>The <em>dwFlags</em> parameter must contain either the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.MediaSource"/></strong> flag or the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.ByteStream"/></strong> flag, but should not contain both.</p><p>For local files, you can pass the file name in the <em>pwszURL</em> parameter; the <code>file:</code> scheme is not required.</p><p><strong>Note</strong>??This method cannot be called remotely.</p>	
        /// </remarks>	
        /// <msdn-id>ms702279</msdn-id>	
        /// <unmanaged>HRESULT IMFSourceResolver::CreateObjectFromURL([In] const wchar_t* pwszURL,[In] unsigned int dwFlags,[In] IPropertyStore* pProps,[Out] MF_OBJECT_TYPE* pObjectType,[Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFSourceResolver::CreateObjectFromURL</unmanaged-short>	
        public SharpDX.IUnknown CreateObjectFromURL(string url,
            SourceResolverFlags flags)
        {
            SharpDX.MediaFoundation.ObjectType objectType;
            return CreateObjectFromURL(url, flags, null, out objectType);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Creates a media source or a byte stream from a URL. This method is synchronous. </p>	
        /// </summary>	
        /// <param name="url"><dd> <p> Null-terminated string that contains the URL to resolve. </p> </dd></param>	
        /// <param name="flags"><dd> <p> Bitwise OR of one or more flags. See <strong>Source Resolver Flags</strong>. </p> </dd></param>	
        /// <param name="objectType"><dd> <p> Receives a member of the <strong><see cref="SharpDX.MediaFoundation.ObjectType"/></strong> enumeration, specifying the type of object that was created. </p> </dd></param>	
        /// <returns>A reference to the object's <strong><see cref="SharpDX.ComObject"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>The <em>dwFlags</em> parameter must contain either the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.MediaSource"/></strong> flag or the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.ByteStream"/></strong> flag, but should not contain both.</p><p>For local files, you can pass the file name in the <em>pwszURL</em> parameter; the <code>file:</code> scheme is not required.</p><p><strong>Note</strong>??This method cannot be called remotely.</p>	
        /// </remarks>	
        /// <msdn-id>ms702279</msdn-id>	
        /// <unmanaged>HRESULT IMFSourceResolver::CreateObjectFromURL([In] const wchar_t* pwszURL,[In] unsigned int dwFlags,[In] IPropertyStore* pProps,[Out] MF_OBJECT_TYPE* pObjectType,[Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFSourceResolver::CreateObjectFromURL</unmanaged-short>	
        public SharpDX.IUnknown CreateObjectFromURL(string url,
            SourceResolverFlags flags,
            out SharpDX.MediaFoundation.ObjectType objectType
            )
        {
            return CreateObjectFromURL(url, flags, null, out objectType);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Creates a media source or a byte stream from a URL. This method is synchronous. </p>	
        /// </summary>	
        /// <param name="url"><dd> <p> Null-terminated string that contains the URL to resolve. </p> </dd></param>	
        /// <param name="flags"><dd> <p> Bitwise OR of one or more flags. See <strong>Source Resolver Flags</strong>. </p> </dd></param>	
        /// <param name="propertyStore"><dd> <p> Pointer to the <strong><see cref="SharpDX.ComObject"/></strong> interface of a property store. The method passes the property store to the scheme handler or byte-stream handler that creates the object. The handler can use the property store to configure the object. This parameter can be <strong><c>null</c></strong>. For more information, see Configuring a Media Source. </p> </dd></param>	
        /// <param name="objectType"><dd> <p> Receives a member of the <strong><see cref="SharpDX.MediaFoundation.ObjectType"/></strong> enumeration, specifying the type of object that was created. </p> </dd></param>	
        /// <returns>A reference to the object's <strong><see cref="SharpDX.ComObject"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>The <em>dwFlags</em> parameter must contain either the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.MediaSource"/></strong> flag or the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.ByteStream"/></strong> flag, but should not contain both.</p><p>For local files, you can pass the file name in the <em>pwszURL</em> parameter; the <code>file:</code> scheme is not required.</p><p><strong>Note</strong>??This method cannot be called remotely.</p>	
        /// </remarks>	
        /// <msdn-id>ms702279</msdn-id>	
        /// <unmanaged>HRESULT IMFSourceResolver::CreateObjectFromURL([In] const wchar_t* pwszURL,[In] unsigned int dwFlags,[In] IPropertyStore* pProps,[Out] MF_OBJECT_TYPE* pObjectType,[Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFSourceResolver::CreateObjectFromURL</unmanaged-short>	
        public SharpDX.IUnknown CreateObjectFromURL(string url,
            SourceResolverFlags flags,
            SharpDX.ComObject propertyStore,
            out SharpDX.MediaFoundation.ObjectType objectType
            )
        {
            IUnknown result;
            CreateObjectFromURL(url, (int)(flags | SourceResolverFlags.MediaSource), propertyStore, out objectType, out result);
            return result;
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Creates a media source from a byte stream. This method is synchronous. </p>	
        /// </summary>	
        /// <param name="stream"><dd> <p> Pointer to the byte stream's <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface. </p> </dd></param>	
        /// <param name="url"><dd> <p> Null-terminated string that contains the URL of the byte stream. The URL is optional and can be <strong><c>null</c></strong>. See Remarks for more information. </p> </dd></param>	
        /// <param name="flags"><dd> <p> Bitwise <strong>OR</strong> of flags. See <strong>Source Resolver Flags</strong>. </p> </dd></param>	
        /// <returns>a reference to the media source's <strong><see cref="SharpDX.ComObject"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>The <em>dwFlags</em> parameter must contain the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.MediaSource"/></strong> flag and should not contain the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.ByteStream"/></strong> flag.</p><p>The source resolver attempts to find one or more byte-stream handlers for the byte stream, based on the file name extension of the URL, or the MIME type of the byte stream (or both). The URL is specified in the optional <em>pwszURL</em> parameter, and the MIME type may be specified in the <strong><see cref="SharpDX.MediaFoundation.ByteStreamAttributeKeys.ContentType"/></strong> attribute on the byte stream. Byte-stream handlers are registered by file name extension or MIME type, or both, as described in Scheme Handlers and Byte-Stream Handlers. The caller should specify at least one of these values (both if possible):</p><ul> <li> Specify the URL in the <em>pwszURL</em> parameter. </li> <li> Specify the MIME type by setting the <strong><see cref="SharpDX.MediaFoundation.ByteStreamAttributeKeys.ContentType"/></strong> attribute on the byte stream. (This attribute might be set already when you create the byte stream, depending on how the byte stream was created.) </li> </ul><p><strong>Note</strong>??This method cannot be called remotely.</p>	
        /// </remarks>	
        /// <msdn-id>ms704671</msdn-id>	
        /// <unmanaged>HRESULT IMFSourceResolver::CreateObjectFromByteStream([In] IMFByteStream* pByteStream,[In] const wchar_t* pwszURL,[In] unsigned int dwFlags,[In] IPropertyStore* pProps,[Out] MF_OBJECT_TYPE* pObjectType,[Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFSourceResolver::CreateObjectFromByteStream</unmanaged-short>	
        public SharpDX.IUnknown CreateObjectFromStream(ByteStream stream,
            string url,
            SourceResolverFlags flags)
        {
            ObjectType objectType;
            return CreateObjectFromStream(stream, url, flags, null, out objectType);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Creates a media source from a byte stream. This method is synchronous. </p>	
        /// </summary>	
        /// <param name="stream"><dd> <p> Pointer to the byte stream's <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface. </p> </dd></param>	
        /// <param name="url"><dd> <p> Null-terminated string that contains the URL of the byte stream. The URL is optional and can be <strong><c>null</c></strong>. See Remarks for more information. </p> </dd></param>	
        /// <param name="flags"><dd> <p> Bitwise <strong>OR</strong> of flags. See <strong>Source Resolver Flags</strong>. </p> </dd></param>	
        /// <param name="objectType"><dd> <p> Receives a member of the <strong><see cref="SharpDX.MediaFoundation.ObjectType"/></strong> enumeration, specifying the type of object that was created. </p> </dd></param>	
        /// <returns>a reference to the media source's <strong><see cref="SharpDX.ComObject"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>The <em>dwFlags</em> parameter must contain the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.MediaSource"/></strong> flag and should not contain the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.ByteStream"/></strong> flag.</p><p>The source resolver attempts to find one or more byte-stream handlers for the byte stream, based on the file name extension of the URL, or the MIME type of the byte stream (or both). The URL is specified in the optional <em>pwszURL</em> parameter, and the MIME type may be specified in the <strong><see cref="SharpDX.MediaFoundation.ByteStreamAttributeKeys.ContentType"/></strong> attribute on the byte stream. Byte-stream handlers are registered by file name extension or MIME type, or both, as described in Scheme Handlers and Byte-Stream Handlers. The caller should specify at least one of these values (both if possible):</p><ul> <li> Specify the URL in the <em>pwszURL</em> parameter. </li> <li> Specify the MIME type by setting the <strong><see cref="SharpDX.MediaFoundation.ByteStreamAttributeKeys.ContentType"/></strong> attribute on the byte stream. (This attribute might be set already when you create the byte stream, depending on how the byte stream was created.) </li> </ul><p><strong>Note</strong>??This method cannot be called remotely.</p>	
        /// </remarks>	
        /// <msdn-id>ms704671</msdn-id>	
        /// <unmanaged>HRESULT IMFSourceResolver::CreateObjectFromByteStream([In] IMFByteStream* pByteStream,[In] const wchar_t* pwszURL,[In] unsigned int dwFlags,[In] IPropertyStore* pProps,[Out] MF_OBJECT_TYPE* pObjectType,[Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFSourceResolver::CreateObjectFromByteStream</unmanaged-short>	
        public SharpDX.IUnknown CreateObjectFromStream(ByteStream stream,
            string url,
            SourceResolverFlags flags,
            out SharpDX.MediaFoundation.ObjectType objectType)
        {
            return CreateObjectFromStream(stream, url, flags, null, out objectType);
        }

        /// <summary>	
        /// <p><strong>Applies to: </strong>desktop apps | Metro style apps</p><p> Creates a media source from a byte stream. This method is synchronous. </p>	
        /// </summary>	
        /// <param name="stream"><dd> <p> Pointer to the byte stream's <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface. </p> </dd></param>	
        /// <param name="url"><dd> <p> Null-terminated string that contains the URL of the byte stream. The URL is optional and can be <strong><c>null</c></strong>. See Remarks for more information. </p> </dd></param>	
        /// <param name="flags"><dd> <p> Bitwise <strong>OR</strong> of flags. See <strong>Source Resolver Flags</strong>. </p> </dd></param>	
        /// <param name="propertyStore"><dd> <p> Pointer to the <strong><see cref="SharpDX.ComObject"/></strong> interface of a property store. The method passes the property store to the byte-stream handler. The byte-stream handler can use the property store to configure the media source. This parameter can be <strong><c>null</c></strong>. For more information, see Configuring a Media Source. </p> </dd></param>	
        /// <param name="objectType"><dd> <p> Receives a member of the <strong><see cref="SharpDX.MediaFoundation.ObjectType"/></strong> enumeration, specifying the type of object that was created. </p> </dd></param>	
        /// <returns>a reference to the media source's <strong><see cref="SharpDX.ComObject"/></strong> interface. The caller must release the interface.</returns>	
        /// <remarks>	
        /// <p>The <em>dwFlags</em> parameter must contain the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.MediaSource"/></strong> flag and should not contain the <strong><see cref="SharpDX.MediaFoundation.SourceResolverFlags.ByteStream"/></strong> flag.</p><p>The source resolver attempts to find one or more byte-stream handlers for the byte stream, based on the file name extension of the URL, or the MIME type of the byte stream (or both). The URL is specified in the optional <em>pwszURL</em> parameter, and the MIME type may be specified in the <strong><see cref="SharpDX.MediaFoundation.ByteStreamAttributeKeys.ContentType"/></strong> attribute on the byte stream. Byte-stream handlers are registered by file name extension or MIME type, or both, as described in Scheme Handlers and Byte-Stream Handlers. The caller should specify at least one of these values (both if possible):</p><ul> <li> Specify the URL in the <em>pwszURL</em> parameter. </li> <li> Specify the MIME type by setting the <strong><see cref="SharpDX.MediaFoundation.ByteStreamAttributeKeys.ContentType"/></strong> attribute on the byte stream. (This attribute might be set already when you create the byte stream, depending on how the byte stream was created.) </li> </ul><p><strong>Note</strong>??This method cannot be called remotely.</p>	
        /// </remarks>	
        /// <msdn-id>ms704671</msdn-id>	
        /// <unmanaged>HRESULT IMFSourceResolver::CreateObjectFromByteStream([In] IMFByteStream* pByteStream,[In] const wchar_t* pwszURL,[In] unsigned int dwFlags,[In] IPropertyStore* pProps,[Out] MF_OBJECT_TYPE* pObjectType,[Out] IUnknown** ppObject)</unmanaged>	
        /// <unmanaged-short>IMFSourceResolver::CreateObjectFromByteStream</unmanaged-short>	
        public SharpDX.IUnknown CreateObjectFromStream(ByteStream stream,
            string url,
            SourceResolverFlags flags,
            ComObject propertyStore,
            out SharpDX.MediaFoundation.ObjectType objectType)
        {
            IUnknown result;
            CreateObjectFromByteStream(stream,
                url,
                (int)(flags | SourceResolverFlags.MediaSource),
                propertyStore,
                out objectType,
                out result);
            return result;
        }
    }
}