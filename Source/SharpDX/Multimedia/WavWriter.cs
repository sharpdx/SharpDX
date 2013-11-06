using System;
using System.Collections.Generic;
using System.IO;

using SharpDX.Serialization;

namespace SharpDX.Multimedia
{
    /// <summary>The wav file writer class.</summary>
    public class WavWriter
    {
        /// <summary>The serializer.</summary>
        private readonly BinarySerializer serializer;
        /// <summary>The is begin.</summary>
        private bool isBegin = false;

        /// <summary>Initializes a new instance of the <see cref="WavWriter"/> class.</summary>
        /// <param name="outputStream">The output stream.</param>
        public WavWriter(Stream outputStream)
        {
            serializer = new BinarySerializer(outputStream, SerializerMode.Write);
        }

        /// <summary>Begins the specified wave format.</summary>
        /// <param name="waveFormat">The wave format.</param>
        /// <exception cref="System.InvalidOperationException">Cannot begin a new WAV while another begin has not been closed</exception>
        public void Begin(WaveFormat waveFormat)
        {
            if (isBegin)
                throw new InvalidOperationException("Cannot begin a new WAV while another begin has not been closed");

            serializer.BeginChunk("RIFF");
            var fourCC = new FourCC("WAVE");
            serializer.Serialize(ref fourCC);

            serializer.BeginChunk("fmt ");
            serializer.Serialize(ref waveFormat);
            serializer.EndChunk();

            serializer.BeginChunk("data");
            isBegin = true;
        }

        /// <summary>Appends the data.</summary>
        /// <param name="dataPointers">The data pointers.</param>
        public void AppendData(IEnumerable<DataPointer> dataPointers)
        {
            CheckBegin();
            foreach (var dataPointer in dataPointers)
                AppendData(dataPointer);
        }

        /// <summary>Appends the data.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="dataBuffers">The data buffers.</param>
        public unsafe void AppendData<T>(IEnumerable<T[]> dataBuffers) where T : struct
        {
            CheckBegin();
            foreach (var dataPointer in dataBuffers)
            {
                AppendData(dataPointer);
            }
        }

        /// <summary>Appends the data.</summary>
        /// <param name="dataPointer">The data pointer.</param>
        public void AppendData(DataPointer dataPointer)
        {
            CheckBegin();
            serializer.SerializeMemoryRegion(dataPointer);
        }

        /// <summary>Appends the data.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of attribute.</typeparam>
        /// <param name="buffer">The buffer.</param>
        public unsafe void AppendData<T>(T[] buffer) where T : struct
        {
            CheckBegin();
            AppendData(new DataPointer((IntPtr)Interop.Fixed(buffer), Utilities.SizeOf<T>(buffer)));
        }

        /// <summary>Ends this instance.</summary>
        public void End()
        {
            CheckBegin();
            serializer.EndChunk();
            serializer.EndChunk();
            isBegin = false;
        }

        /// <summary>Checks the begin.</summary>
        /// <exception cref="System.InvalidOperationException">Begin was not called</exception>
        private void CheckBegin()
        {
            if (!isBegin)
                throw new InvalidOperationException("Begin was not called");
        }
    }
}
