// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using SharpDX.Multimedia;

#if STORE_APP
using Windows.Storage.Streams;
#endif

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Decoder from compressed audio (mp3, wma...etc.) to PCM.
    /// </summary>
    /// <remarks>
    /// This class was developed following the <a href="http://msdn.microsoft.com/en-us/library/windows/desktop/dd757929%28v=vs.85%29.aspx">"Tutorial: Decoding Audio"</a>
    /// </remarks>
    public class AudioDecoder : DisposeBase
    {
        private SourceReader sourceReader;
        private SourceReader nextSourceReader;
        private Sample currentSample;
        private MediaBuffer currentBuffer;
        private readonly object sourceReaderLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDecoder" /> class.
        /// </summary>
        public AudioDecoder()
        {
            // Make sure that the MediaEngine is initialized.
            MediaManager.Startup();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDecoder" /> class.
        /// </summary>
        /// <param name="stream">The stream to read the compressed audio.</param>
        public AudioDecoder(Stream stream)
        {
            // Make sure that the MediaEngine is initialized.
            MediaManager.Startup();
            SetSourceStream(stream);
        }

        /// <summary>
        /// Gets or sets the source stream. See remarks.
        /// </summary>
        /// <value>The source.</value>
        /// <remarks>
        /// The source must be set before calling <see cref="GetSamples()"/>
        /// </remarks>
        public void SetSourceStream(Stream value)
        {
            lock (sourceReaderLock)
            {
                // If the nextSourceReader is not null
                if (nextSourceReader != null)
                    nextSourceReader.Dispose();
                nextSourceReader = new SourceReader(value);
                Initialize(nextSourceReader);
            }
        }

#if STORE_APP
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioDecoder" /> class.
        /// </summary>
        /// <param name="stream">The stream to read the compressed audio.</param>
        public AudioDecoder(IRandomAccessStream stream)
        {
            // Make sure that the MediaEngine is initialized.
            MediaManager.Startup();
            SetSourceStream(stream);
        }

        /// <summary>
        /// Gets or sets the source stream. See remarks.
        /// </summary>
        /// <value>The source.</value>
        /// <remarks>
        /// The source must be set before calling <see cref="GetSamples()"/>
        /// </remarks>
        public void SetSourceStream(IRandomAccessStream value)
        {
            lock (sourceReaderLock)
            {
                // If the nextSourceReader is not null
                if (nextSourceReader != null)
                    nextSourceReader.Dispose();
                nextSourceReader = new SourceReader(value);
                Initialize(nextSourceReader);
            }
        }
#endif
        
        /// <summary>
        /// Gets the total duration in seconds.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Gets the PCM wave format output by this decoder.
        /// </summary>
        /// <value>The wave format.</value>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Gets the decoded PCM samples. See remarks.
        /// </summary>
        /// <returns>An enumerator of pointer to PCM decoded data with the same format as returned by <see cref="WaveFormat"/>.</returns>
        /// <remarks>
        /// This method is only working as a single enumerator at a time.
        /// </remarks>
        /// <remarks>
        /// The <see cref="Source"/> must be set before calling <see cref="GetSamples()"/>
        /// </remarks>
        public IEnumerable<DataPointer> GetSamples()
        {
            return GetSamples(new TimeSpan());
        }

        /// <summary>
        /// Gets the decoded PCM samples. See remarks.
        /// </summary>
        /// <param name="startingPositionInSeconds">The starting position in seconds.</param>
        /// <returns>An enumerator of pointer to PCM decoded data with the same format as returned by <see cref="WaveFormat"/>.</returns>
        /// <remarks>
        /// This method is only working as a single enumerator at a time.
        /// The <see cref="SetSourceStream(System.IO.Stream)"/> must be set before calling <see cref="GetSamples()"/>
        /// </remarks>
        public IEnumerable<DataPointer> GetSamples(TimeSpan startingPositionInSeconds)
        {
            // A new reader is setup, so initialize it.
            lock (sourceReaderLock)
            {
                // If the reader was changed
                if (nextSourceReader != null)
                {
                    if (sourceReader != null)
                        sourceReader.Dispose();

                    sourceReader = nextSourceReader;
                    nextSourceReader = null;
                }
            }

            // Make sure that any prior call 
            CleanupAndDispose();

            CheckIfDisposed();

            // Set the position
            sourceReader.SetCurrentPosition((long)(startingPositionInSeconds.Ticks));

            while (true)
            {
                int streamIndex;
                SourceReaderFlags flags;
                long time;

                CheckIfDisposed();

                using (currentSample = sourceReader.ReadSample(SourceReaderIndex.FirstAudioStream, SourceReaderControlFlags.None, out streamIndex, out flags, out time))
                {
                    if ((flags & SourceReaderFlags.Endofstream) != 0)
                        break;

                    CheckIfDisposed();

                    using (currentBuffer = currentSample.ConvertToContiguousBuffer())
                    {
                        int bufferMaxLength;
                        int bufferCurrentLength;

                        CheckIfDisposed();
                        
                        var ptr = currentBuffer.Lock(out bufferMaxLength, out bufferCurrentLength);

                        yield return new DataPointer(ptr, bufferCurrentLength);

                        // Warning, because the yield could never return here, currentBuffer and currentSample should be disposed when disposing this object or when
                        // calling it again on the GetSamples method.

                        // In case a Dispose occurred while decoding
                        if (currentBuffer == null)
                            break;

                        currentBuffer.Unlock();
                    }
                }
            }

            // They have been disposed, so we can just clear them
            currentBuffer = null;
            currentSample = null;
        }

        private void CleanupAndDispose()
        {
            if (currentBuffer != null)
            {
                currentBuffer.Unlock();
                currentBuffer.Dispose();
                currentBuffer = null;
            }

            if (currentSample != null)
            {
                currentSample.Dispose();
                currentSample = null;
            }
        }

        private void CheckIfDisposed()
        {
            if (IsDisposed)
                throw new InvalidOperationException("This instance is disposed while enumerating the samples.");
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            CleanupAndDispose();

            if (nextSourceReader != null)
            {
                nextSourceReader.Dispose();
                nextSourceReader = null;
            }

            if (sourceReader != null)
            {
                sourceReader.Dispose();
                sourceReader = null;
            }
        }

        private void Initialize(SourceReader reader)
        {
            // Invalidate selection for all streams
            reader.SetStreamSelection(SourceReaderIndex.AllStreams, false);

            // Select only audio stream
            reader.SetStreamSelection(SourceReaderIndex.FirstAudioStream, true);

            // Get the media type for the current stream.
            using (var mediaType = reader.GetNativeMediaType(SourceReaderIndex.FirstAudioStream, 0))
            {
                var majorType = mediaType.Get(MediaTypeAttributeKeys.MajorType);
                if (majorType != MediaTypeGuids.Audio)
                    throw new InvalidOperationException("Input stream doesn't contain an audio stream.");
            }

            // Set the type on the source reader to use PCM
            using (var partialType = new MediaType())
            {
                partialType.Set(MediaTypeAttributeKeys.MajorType, MediaTypeGuids.Audio);
                partialType.Set(MediaTypeAttributeKeys.Subtype, AudioFormatGuids.Pcm);
                reader.SetCurrentMediaType(SourceReaderIndex.FirstAudioStream, partialType);
            }

            // Retrieve back the real media type
            using (var realMediaType = reader.GetCurrentMediaType(SourceReaderIndex.FirstAudioStream))
            {
                int sizeRef;
                WaveFormat = realMediaType.ExtracttWaveFormat(out sizeRef);
            }

            Duration = new TimeSpan(reader.GetPresentationAttribute(SourceReaderIndex.MediaSource, PresentationDescriptionAttributeKeys.Duration));
        }
    }
}
