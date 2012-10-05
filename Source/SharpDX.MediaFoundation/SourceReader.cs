using System;
using System.IO;

namespace SharpDX.MediaFoundation
{
    public partial class SourceReader
    {
        private ByteStream byteStream;

        /// <summary>	
        /// Creates the source reader from a byte stream.
        /// </summary>	
        /// <param name="buffer"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface of a byte stream. This byte stream will provide the source data for the source reader.</p> </dd></param>	
        /// <param name="attributes"><dd> <p>Pointer to the <strong><see cref="SharpDX.MediaFoundation.MediaAttributes"/></strong> interface. You can use this parameter to configure the source reader. For more information, see Source Reader Attributes. This parameter can be <strong><c>null</c></strong>.</p> </dd></param>	
        /// <remarks>	
        /// <p>Call <strong>CoInitialize(Ex)</strong> and <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Startup"/></strong> before calling this function.</p><p> Internally, the source reader calls the <strong><see cref="SharpDX.MediaFoundation.SourceResolver.CreateObjectFromByteStream_"/></strong> method to create a media source from the byte stream. Therefore, a byte-stream handler must be registered for the byte stream. For more information about byte-stream handlers, see Scheme Handlers and Byte-Stream Handlers. </p><p>This function is available on Windows?Vista if Platform Update Supplement for Windows?Vista is installed.</p>	
        /// </remarks>	
        /// <msdn-id>dd388106</msdn-id>	
        /// <unmanaged>HRESULT MFCreateSourceReaderFromByteStream([In] IMFByteStream* pByteStream,[In, Optional] IMFAttributes* pAttributes,[Out, Fast] IMFSourceReader** ppSourceReader)</unmanaged>	
        /// <unmanaged-short>MFCreateSourceReaderFromByteStream</unmanaged-short>	
        public SourceReader(byte[] buffer, MediaAttributes attributes = null)
        {
            byteStream = new ByteStream(new MemoryStream(buffer));
            MediaFactory.CreateSourceReaderFromByteStream(byteStream.NativePointer, attributes, this);
        }

        /// <summary>	
        /// Creates the source reader from a byte stream.
        /// </summary>	
        /// <param name="buffer"><dd> <p>A reference to the <strong><see cref="SharpDX.MediaFoundation.IByteStream"/></strong> interface of a byte stream. This byte stream will provide the source data for the source reader.</p> </dd></param>	
        /// <param name="attributes"><dd> <p>Pointer to the <strong><see cref="SharpDX.MediaFoundation.MediaAttributes"/></strong> interface. You can use this parameter to configure the source reader. For more information, see Source Reader Attributes. This parameter can be <strong><c>null</c></strong>.</p> </dd></param>	
        /// <remarks>	
        /// <p>Call <strong>CoInitialize(Ex)</strong> and <strong><see cref="SharpDX.MediaFoundation.MediaFactory.Startup"/></strong> before calling this function.</p><p> Internally, the source reader calls the <strong><see cref="SharpDX.MediaFoundation.SourceResolver.CreateObjectFromByteStream_"/></strong> method to create a media source from the byte stream. Therefore, a byte-stream handler must be registered for the byte stream. For more information about byte-stream handlers, see Scheme Handlers and Byte-Stream Handlers. </p><p>This function is available on Windows?Vista if Platform Update Supplement for Windows?Vista is installed.</p>	
        /// </remarks>	
        /// <msdn-id>dd388106</msdn-id>	
        /// <unmanaged>HRESULT MFCreateSourceReaderFromByteStream([In] IMFByteStream* pByteStream,[In, Optional] IMFAttributes* pAttributes,[Out, Fast] IMFSourceReader** ppSourceReader)</unmanaged>	
        /// <unmanaged-short>MFCreateSourceReaderFromByteStream</unmanaged-short>	
        public SourceReader(Stream buffer, MediaAttributes attributes = null)
        {
            byteStream = new ByteStream(buffer);
            MediaFactory.CreateSourceReaderFromByteStream(byteStream.NativePointer, attributes, this);
        }
    }
}