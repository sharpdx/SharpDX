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
using SharpDX.Multimedia;

namespace SharpDX.Serialization
{
    /// <summary>
    /// Serializer action.
    /// </summary>
    /// <param name="value">The value to read or write.</param>
    /// <param name="serializer">The serializer.</param>
    public delegate void SerializerAction(ref object value, BinarySerializer serializer);

    /// <summary>
    /// This class provides serialization methods for types implementing the <see cref="IDataSerializer"/>.
    /// </summary>
    /// <remarks>
    /// BinarySerializer is a basic binary serializer with the following features:
    /// <ul>
    /// <li>10x times faster and smaller than default System Serialization and Xml Serialization.</li>
    /// <li>Supports for all primitive types, array/List&lt;T&gt;/Dictionary of primitive types, custom data via <see cref="IDataSerializer"/> (struct or class) and array/List/Dictionary of custom data.</li>
    /// <li>Optimized binary format, data serialized to the strict minimum.</li>
    /// <li>Should be compatible with Win8/WinRT, Desktop.</li>
    /// <li>Not reflection based serializer, but fully compile time serializer.</li>
    /// <li>Format could be readback from C/C++.</li>
    /// 
    /// </ul>
    /// </remarks>
    public class BinarySerializer
    {
        private int chunkCount;
        private Chunk[] chunks;
        private Chunk currentChunk;
        private readonly Dictionary<FourCC, Dynamic> dynamicMapToType;
        private readonly Dictionary<Type, Dynamic> dynamicMapToFourCC;
        private readonly Dictionary<object, int> objectToPosition;
        private readonly Dictionary<int, object> positionToObject;
        private int allowNullCount;
        private int allowIdentityReferenceCount;

        public delegate void SerializerPrimitiveAction<T>(ref T value);

        public delegate void SerializerTypeAction<T>(ref T value, BinarySerializer serializer);


        public delegate object ReadRef(BinarySerializer serializer);

        public delegate void WriteRef(object value, BinarySerializer serializer);

        
        /// <summary>
        /// Underlying stream this instance is reading/writing to.
        /// </summary>
        public readonly Stream Stream;

        /// <summary>
        /// Reader used to directly read from the underlying stream.
        /// </summary>
        public readonly BinaryReader Reader;

        /// <summary>
        /// Writer used to directly write to the underlying stream.
        /// </summary>
        public readonly BinaryWriter Writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializer" /> class.
        /// </summary>
        /// <param name="stream">The stream to read or write to.</param>
        /// <param name="mode">The read or write mode.</param>
        public BinarySerializer(Stream stream, SerializerMode mode)
        {
            // Allocate dictionary used for dynamic serialization.
            dynamicMapToType = new Dictionary<FourCC, Dynamic>();
            dynamicMapToFourCC = new Dictionary<Type, Dynamic>();

            // For serializing reference
            objectToPosition = new Dictionary<object, int>(new IdentityEqualityComparer<object>());
            positionToObject = new Dictionary<int, object>();

            // Allocate the chunk array and initial chunk.
            chunks = new Chunk[8];
            Stream = stream;
            Reader = new BinaryReader(stream);
            Writer = new BinaryWriter(stream);
            CurrentChunk = new Chunk { ChunkIndexStart = 0 };

            chunks[chunkCount] = CurrentChunk;
            chunkCount++;

            // Setup the mode.
            Mode = mode;

            // Register all default dynamics.
            foreach (var defaultDynamic in DefaultDynamics)
                RegisterDynamic(defaultDynamic);
        }

        /// <summary>
        /// Gets or sets the serialization mode.
        /// </summary>
        /// <value>The serialization mode.</value>
        public SerializerMode Mode { get; set; }

        /// <summary>
        /// Enables to serializing null value. Default is <strong>false</strong>.
        /// </summary>
        /// <param name="isAllowingNull">if set to <c>true</c> [is allowing null].</param>
        /// <returns>Count of AllowNull. Passing true increments the count, passing false decrement it.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <remarks>This boolean value can be turned on/off while serializing.</remarks>
        public int EnableNullReference(bool isAllowingNull)
        {
            allowNullCount += (isAllowingNull) ? 1 : -1;
            if (allowNullCount < 0)
                throw new InvalidOperationException("Invalid call to EnableNullReference. Must match true/false in pair.");
            return allowNullCount;
        }

        /// <summary>
        /// Enables to serialize an object only once using object reference. Default is <strong>false</strong>.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <returns>Count of EnableIdentityReference. Passing true increments the count, passing false decrement it.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public int EnableIdentityReference(bool isEnabled)
        {
            allowIdentityReferenceCount += (isEnabled) ? 1 : -1;
            if (allowIdentityReferenceCount < 0)
                throw new InvalidOperationException("Invalid call to EnableIdentityReference. Must match true/false in pair.");
            return allowIdentityReferenceCount;            
        }

        /// <summary>
        /// Register a dynamic serializer for a particular type implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the element to serialize.</typeparam>
        /// <param name="id">The id to use for serializing T.</param>
        public void RegisterDynamic<T>(FourCC id) where T : IDataSerializer, new()
        {
            RegisterDynamic(new Dynamic {Id = id, Type = typeof (T), Reader = ReaderDataSerializer<T>, Writer = WriterDataSerializer<T>});
        }

        /// <summary>
        /// Register a dynamic array serializer for a particular type implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the element in the array.</typeparam>
        /// <param name="id">The id to use for serializing T[].</param>
        public void RegisterDynamicArray<T>(FourCC id) where T : IDataSerializer, new()
        {
            RegisterDynamic(new Dynamic {Id = id, Type = typeof (T[]), Reader = ReaderDataSerializerArray<T>, Writer = WriterDataSerializerArray<T>});
        }

        /// <summary>
        /// Register a dynamic List&lt;T&gt; serializer for a particular type implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the element in the List&lt;T&gt;.</typeparam>
        /// <param name="id">The id to use for serializing List&lt;T&gt;.</param>
        public void RegisterDynamicList<T>(FourCC id) where T : IDataSerializer, new()
        {
            RegisterDynamic(new Dynamic {Id = id, Type = typeof (List<T>), Reader = ReaderDataSerializerList<T>, Writer = WriterDataSerializerList<T>});
        }

        /// <summary>
        /// Register a dynamic serializer using an external action.
        /// </summary>
        /// <typeparam name="T">Type of the element to serialize.</typeparam>
        /// <param name="id">The id to use for serializing T.</param>
        /// <param name="serializer">The serializer.</param>
        public void RegisterDynamic<T>(FourCC id, SerializerAction serializer) where T : new()
        {
            var dynamicSerializer = new Dynamic {Id = id, Type = typeof (T), DynamicSerializer = serializer};
            dynamicSerializer.Reader = dynamicSerializer.DynamicReader<T>;
            dynamicSerializer.Writer = dynamicSerializer.DynamicWriter;
            RegisterDynamic(dynamicSerializer);
        }

        /// <summary>
        /// Begin to serialize a a new chunk.
        /// </summary>
        /// <param name="chunkId">The chunk id.</param>
        /// <exception cref="SharpDX.Serialization.InvalidChunkException">If the chuck to read is not the expecting chunk.</exception>
        /// <remarks>
        /// A Chunk is an identifiable portion of data that will serialized. Chunk are usefull to encapsulate a variable 
        /// data (and check for the presence of the chunk Id). Chunk are storing a 4 bytes identifier and the length of 
        /// the chunk before reading/writing actual datas.
        /// </remarks>
        public void BeginChunk(FourCC chunkId)
        {
            // Allocate new Chunk
            if (chunks[chunkCount] == null)
            {
                CurrentChunk = new Chunk();
                chunks[chunkCount] = CurrentChunk;
            }
            else
            {
                CurrentChunk = chunks[chunkCount];
            }

            // Go to next chunk
            chunkCount++;

            // Sets the current id
            CurrentChunk.Id = chunkId;

            // Create a new chunk
            if (Mode == SerializerMode.Write)
                CurrentChunk.ChunkIndexStart = Stream.Position;

            if (chunkCount >= chunks.Length)
            {
                var temp = new Chunk[chunks.Length * 2];
                Array.Copy(chunks, temp, chunks.Length);
                chunks = temp;
            }

            if (Mode == SerializerMode.Write)
            {
                // Write the chunkId to the current chunk
                Writer.Write((int) chunkId);
                // write temporary null size
                Writer.Write(0);
            }
            else
            {
                var chunkIdRead = Reader.ReadInt32();
                if (chunkIdRead != chunkId)
                    throw new InvalidChunkException(chunkIdRead, chunkId);

                uint sizeOfChunk = (uint)Reader.ReadUInt32();
                CurrentChunk.ChunkIndexEnd = Stream.Position + sizeOfChunk;
            }
        }


        /// <summary>
        /// Ends a chunk.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">If there EndChunk is called without a previous BeginChunk.</exception>
        /// <exception cref="System.IO.InvalidDataException">If the size of data read from the chunk is different from chunk size.</exception>
        public void EndChunk()
        {
            if (chunkCount <= 1)
                throw new InvalidOperationException("EndChunk() called without BeginChunk()");

            var previousChunk = CurrentChunk;
            chunkCount--;
            CurrentChunk = chunks[chunkCount - 1];

            if (Mode == SerializerMode.Write)
            {
                // Write the size of this chunk
                long currentPosition = Stream.Position;
                Stream.Position = previousChunk.ChunkIndexStart + 4;
                Writer.Write((uint)(currentPosition - Stream.Position - 4));
                Stream.Position = currentPosition;
            }
            else
            {
                if (previousChunk.ChunkIndexEnd != Stream.Position)
                    throw new InvalidDataException(string.Format("Unexpected size when reading chunk [{0}]", CurrentChunk.Id));
            }
        }


        /// <summary>
        /// Deserialize a data from the underlying stream.
        /// </summary>
        /// <typeparam name="T">Type of the data to load.</typeparam>
        /// <returns>An instance of the loaded data.</returns>
        public T Load<T>() where T : IDataSerializer, new()
        {
            ResetStoredReference();

            Mode = SerializerMode.Read;
            T value = default(T);
            Serialize(ref value);
            return value;
        }

        /// <summary>
        /// Serializes the specified value to the underlying stream.
        /// </summary>
        /// <typeparam name="T">Type of the data to save.</typeparam>
        /// <param name="value">The value to save.</param>
        public void Save<T>(T value) where T : IDataSerializer, new()
        {
            ResetStoredReference();

            Mode = SerializerMode.Write;
            Serialize(ref value);
            Flush();
        }

        /// <summary>
        /// Flush the underlying <see cref="System.IO.Stream"/>
        /// </summary>
        public void Flush()
        {
            Writer.Flush();
        }

        /// <summary>
        /// Serializes a dynamic value.
        /// </summary>
        /// <typeparam name="T">Known type of the value to serialize. The known type is not the runtime type that will be actually serialized.</typeparam>
        /// <param name="value">The value to serialize based on its runtime type.</param>
        public void SerializeDynamic<T>(ref T value)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref value, out storeObjectRef, true))
                return;

            SerializeRawDynamic(ref value);
        }

        /// <summary>
        /// Serializes a static value implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<T>(ref T value) where T : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref value, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Read)
                value = new T();

            value.Serialize(this);

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(value, storeObjectRef);
        }

        /// <summary>
        /// Serializes an enum value.
        /// </summary>
        /// <typeparam name="T">Type of the enum to serialize.</typeparam>
        /// <param name="value">The value to serialize</param>
        /// <exception cref="ArgumentException">If type T is not an enum.</exception>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public unsafe void SerializeEnum<T>(ref T value) where T : struct, IComparable, IFormattable
        {
            if (!Utilities.IsEnum(typeof(T)))
                throw new ArgumentException("T generic parameter must be a valid enum", "value");

            var pValue = Interop.Fixed(ref value);

            switch (Utilities.SizeOf<T>())
            {
                case 1:
                    {
                        Serialize(ref *(byte*)pValue);
                        break;
                    }
                case 2:
                    {
                        Serialize(ref *(short*)pValue);
                        break;
                    }
                case 4:
                    {
                        Serialize(ref *(int*)pValue);
                        break;
                    }
                case 8:
                    {
                        Serialize(ref *(long*)pValue);
                        break;
                    }
            }
        }

        /// <summary>
        /// Serializes an array of primitives using serialization methods implemented by this instance for each item in the array.
        /// </summary>
        /// <typeparam name="T">Type of the primitive data to serialize.</typeparam>
        /// <param name="valueArray">An array of primitive value to serialize</param>
        /// <param name="serializer">The serializer to user to serialize the T values.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<T>(ref T[] valueArray, SerializerPrimitiveAction<T> serializer)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write((int) valueArray.Length);
                for (int i = 0; i < valueArray.Length; i++)
                    serializer(ref valueArray[i]);
            }
            else
            {
                var count = Reader.ReadInt32();
                valueArray = new T[count];
                for (int index = 0; index < count; index++)
                    serializer(ref valueArray[index]);
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);
        }

        /// <summary>
        /// Serializes count elements in an array of primitives using serialization methods implemented by this instance for each item in the array. 
        /// </summary>
        /// <typeparam name="T">Type of the primitive data to serialize.</typeparam>
        /// <param name="valueArray">An array of primitive value to serialize</param>
        /// <param name="count">Count elements to serialize. See remarks.</param>
        /// <param name="serializer">The serializer to user to serialize the T values.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.<br/>
        /// <strong>Caution</strong>: Also unlike the plain array version, the count is not serialized. This method is usefull
        /// when we want to serialize the count of an array separately from the array.
        /// </remarks>
        public void Serialize<T>(ref T[] valueArray, int count, SerializerPrimitiveAction<T> serializer)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                for (int i = 0; i < count; i++)
                    serializer(ref valueArray[i]);
            }
            else
            {
                valueArray = new T[count];
                for (int index = 0; index < count; index++)
                    serializer(ref valueArray[index]);
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);
        }

        /// <summary>
        /// Serializes an array of static values that are implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="valueArray">An array of value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<T>(ref T[] valueArray) where T : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef)) 
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(valueArray.Length);
                for (int i = 0; i < valueArray.Length; i++)
                    Serialize(ref valueArray[i]);
            }
            else
            {
                var count = Reader.ReadInt32();
                valueArray = new T[count];
                for (int index = 0; index < count; index++)
                    Serialize(ref valueArray[index]);
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);
        }

        /// <summary>
        /// Serializes count elements in an array of static values that are implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="valueArray">An array of value to serialize</param>
        /// <param name="count">Count elements to serialize. See remarks.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.<br/>
        /// <strong>Caution</strong>: Also unlike the plain array version, the count is not serialized. This method is usefull
        /// when we want to serialize the count of an array separately from the array.
        /// </remarks>
        public void Serialize<T>(ref T[] valueArray, int count) where T : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                for (int i = 0; i < count; i++)
                    Serialize(ref valueArray[i]);
            }
            else
            {
                valueArray = new T[count];
                for (int index = 0; index < count; index++)
                    Serialize(ref valueArray[index]);
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);
        }

        /// <summary>
        /// Serializes an array of bytes.
        /// </summary>
        /// <param name="valueArray">An array of bytes to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref byte[] valueArray)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(valueArray.Length);
                Writer.Write(valueArray);
            }
            else
            {
                int count = Reader.ReadInt32();
                valueArray = new byte[count];
                Reader.Read(valueArray, 0, count);
            }


            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);
        }

        /// <summary>
        /// Serializes an array of bytes.
        /// </summary>
        /// <param name="valueArray">An array of bytes to serialize</param>
        /// <param name="count">Count elements to serialize. See remarks.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.<br/>
        /// <strong>Caution</strong>: Also unlike the plain array version, the count is not serialized. This method is usefull
        /// when we want to serialize the count of an array separately from the array.
        /// </remarks>
        public void Serialize(ref byte[] valueArray, int count)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(valueArray, 0, count);
            }
            else
            {
                valueArray = new byte[count];
                Reader.Read(valueArray, 0, count);
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);
        }

        /// <summary>
        /// Serializes a list of static values that are implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="valueList">A list of value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<T>(ref List<T> valueList) where T : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueList, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(valueList.Count);
                foreach (var value in valueList)
                {
                    T localValue = value;
                    Serialize(ref localValue);
                }
            }
            else
            {
                var count = Reader.ReadInt32();
                valueList = new List<T>(count);
                for (int index = 0; index < count; index++)
                {
                    var value = default(T);
                    Serialize(ref value);
                    valueList.Add(value);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueList, storeObjectRef);
        }

        /// <summary>
        /// Serializes a list of primitive values using a specific serializer method from this instance.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="valueList">A list of value to serialize</param>
        /// <param name="serializerMethod">A method of this instance to serialize the primitive T type</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<T>(ref List<T> valueList, SerializerPrimitiveAction<T> serializerMethod)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueList, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(valueList.Count);
                foreach (var value in valueList)
                {
                    T localValue = value;
                    serializerMethod(ref localValue);
                }
            }
            else
            {
                var count = Reader.ReadInt32();
                valueList = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    var localValue = default(T);
                    serializerMethod(ref localValue);
                    valueList.Add(localValue);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueList, storeObjectRef);
        }

        /// <summary>
        /// Serializes count elements from a list of static values that are implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="valueList">A list of value to serialize</param>
        /// <param name="count">Count elements to serialize. See remarks.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.<br/>
        /// <strong>Caution</strong>: Also unlike the plain array version, the count is not serialized. This method is usefull
        /// when we want to serialize the count of an array separately from the array.
        /// </remarks>
        public void Serialize<T>(ref List<T> valueList, int count) where T : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueList, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                for (int i = 0; i < count; i++)
                {
                    T localValue = valueList[i];
                    Serialize(ref localValue);
                }
            }
            else
            {
                valueList = new List<T>(count);
                for (int index = 0; index < count; index++)
                {
                    var value = default(T);
                    Serialize(ref value);
                    valueList.Add(value);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueList, storeObjectRef);
        }

        /// <summary>
        /// Serializes count  elements from a list of primitive values using a specific serializer method from this instance.
        /// </summary>
        /// <typeparam name="T">Type of the data to serialize.</typeparam>
        /// <param name="valueList">A list of value to serialize</param>
        /// <param name="serializerMethod">A method of this instance to serialize the primitive T type</param>
        /// <param name="count">Count elements to serialize. See remarks.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.<br/>
        /// <strong>Caution</strong>: Also unlike the plain array version, the count is not serialized. This method is usefull
        /// when we want to serialize the count of an array separately from the array.
        /// </remarks>
        public void Serialize<T>(ref List<T> valueList, int count, SerializerPrimitiveAction<T> serializerMethod)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueList, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                for (int i = 0; i < count; i++)
                {
                    T localValue = valueList[i];
                    serializerMethod(ref localValue);
                }
            }
            else
            {
                valueList = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    var localValue = default(T);
                    serializerMethod(ref localValue);
                    valueList.Add(localValue);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(valueList, storeObjectRef);
        }

        /// <summary>
        /// Serializes a dictionary of key/values that are both implementing the <see cref="IDataSerializer"/> interface.
        /// </summary>
        /// <typeparam name="TKey">Type of key to serialize.</typeparam>
        /// <typeparam name="TValue">Type of value to serialize.</typeparam>
        /// <param name="dictionary">A dictionary to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary)
            where TKey : IDataSerializer, new()
            where TValue : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref dictionary, out storeObjectRef))
                return; 

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(dictionary.Count);
                foreach (var value in dictionary)
                {
                    TKey localKey = value.Key;
                    TValue localValue = value.Value;
                    localKey.Serialize(this);
                    localValue.Serialize(this);
                }
            }
            else
            {
                var count = Reader.ReadInt32();
                dictionary = new Dictionary<TKey, TValue>(count);
                for (int i = 0; i < count; i++)
                {
                    TKey localKey = default(TKey);
                    TValue localValue = default(TValue);
                    localKey.Serialize(this);
                    localValue.Serialize(this);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(dictionary, storeObjectRef);
        }

        /// <summary>
        /// Serializes a dictionary of key/values.
        /// </summary>
        /// <typeparam name="TKey">Type of key to serialize that is implementing the <see cref="IDataSerializer"/> interface.</typeparam>
        /// <typeparam name="TValue">Type of primitive value with its associated serializer.</typeparam>
        /// <param name="dictionary">A dictionary to serialize</param>
        /// <param name="valueSerializer">Serializer used for the TValue.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, SerializerPrimitiveAction<TValue> valueSerializer) where TKey : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref dictionary, out storeObjectRef))
                return; 

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(dictionary.Count);
                foreach (var value in dictionary)
                {
                    TKey localKey = value.Key;
                    TValue localValue = value.Value;
                    localKey.Serialize(this);
                    valueSerializer(ref localValue);
                }
            }
            else
            {
                var count = Reader.ReadInt32();
                dictionary = new Dictionary<TKey, TValue>(count);
                for (int i = 0; i < count; i++)
                {
                    TKey localKey = default(TKey);
                    TValue localValue = default(TValue);
                    localKey.Serialize(this);
                    valueSerializer(ref localValue);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(dictionary, storeObjectRef);
        }

        /// <summary>
        /// Serializes a dictionary of key/values.
        /// </summary>
        /// <typeparam name="TKey">Type of primitive value with its associated serializer.</typeparam>
        /// <typeparam name="TValue">Type of value to serialize that is implementing the <see cref="IDataSerializer"/> interface.</typeparam>
        /// <param name="dictionary">A dictionary to serialize</param>
        /// <param name="keySerializer">Serializer used for the TKey.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, SerializerPrimitiveAction<TKey> keySerializer) where TValue : IDataSerializer, new()
        {
            int storeObjectRef;
            if (SerializeIsNull(ref dictionary, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(dictionary.Count);
                foreach (var value in dictionary)
                {
                    TKey localKey = value.Key;
                    TValue localValue = value.Value;
                    keySerializer(ref localKey);
                    localValue.Serialize(this);
                }
            }
            else
            {
                var count = Reader.ReadInt32();
                dictionary = new Dictionary<TKey, TValue>(count);
                for (int i = 0; i < count; i++)
                {
                    TKey localKey = default(TKey);
                    TValue localValue = default(TValue);
                    keySerializer(ref localKey);
                    localValue.Serialize(this);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(dictionary, storeObjectRef);
        }

        /// <summary>
        /// Serializes a dictionary of key/values.
        /// </summary>
        /// <typeparam name="TKey">Type of primitive key with its associated serializer.</typeparam>
        /// <typeparam name="TValue">Type of primitive value with its associated serializer.</typeparam>
        /// <param name="dictionary">A dictionary to serialize</param>
        /// <param name="keySerializer">Serializer used for the TKey.</param>
        /// <param name="valueSerializer">Serializer used for the TValue.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize<TKey, TValue>(ref Dictionary<TKey, TValue> dictionary, SerializerPrimitiveAction<TKey> keySerializer, SerializerPrimitiveAction<TValue> valueSerializer)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref dictionary, out storeObjectRef))
                return;
            
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(dictionary.Count);
                foreach (var value in dictionary)
                {
                    TKey localKey = value.Key;
                    TValue localValue = value.Value;
                    keySerializer(ref localKey);
                    valueSerializer(ref localValue);
                }
            }
            else
            {
                var count = Reader.ReadInt32();
                dictionary = new Dictionary<TKey, TValue>(count);
                for (int i = 0; i < count; i++)
                {
                    TKey localKey = default(TKey);
                    TValue localValue = default(TValue);
                    keySerializer(ref localKey);
                    valueSerializer(ref localValue);
                }
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(dictionary, storeObjectRef);
        }

        /// <summary>
        /// Serializes a single <strong>string</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref string value)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref value, out storeObjectRef))
                return;

            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadString();
            }

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(value, storeObjectRef);
        }

        /// <summary>
        /// Serializes a single <strong>boolean</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref bool value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadBoolean();
            }
        }

        /// <summary>
        /// Serializes a single <strong>byte</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref byte value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadByte();
            }
        }

        /// <summary>
        /// Serializes a single <strong>sbyte</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref sbyte value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadSByte();
            }
        }

        /// <summary>
        /// Serializes a single <strong>short</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref short value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadInt16();
            }
        }

        /// <summary>
        /// Serializes a single <strong>ushort</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref ushort value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadUInt16();
            }
        }

        /// <summary>
        /// Serializes a single <strong>int</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref int value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadInt32();
            }
        }

        /// <summary>
        /// Serializes a single <strong>uint</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref uint value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadUInt32();
            }
        }

        /// <summary>
        /// Serializes a single <strong>long</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref long value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadInt64();
            }
        }

        /// <summary>
        /// Serializes a single <strong>ulong</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref ulong value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadUInt64();
            }
        }

        /// <summary>
        /// Serializes a single <strong>char</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref char value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadChar();
            }
        }

        /// <summary>
        /// Serializes a single <strong>float</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref float value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadSingle();
            }
        }

        /// <summary>
        /// Serializes a single <strong>double</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref double value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value);
            }
            else
            {
                value = Reader.ReadDouble();
            }
        }

        /// <summary>
        /// Serializes a single <strong>DateTime</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref DateTime value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.ToBinary());
            }
            else
            {
                value = new DateTime(Reader.ReadInt64());
            }
        }

        /// <summary>
        /// Serializes a single <strong>Guid</strong> value.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref Guid value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.ToByteArray());
            }
            else
            {
                value = new Guid(Reader.ReadBytes(16));
            }
        }


        private bool SerializeIsNull<T>(ref T value, out int storeObjectReference, bool isDynamic = false)
        {
            storeObjectReference = -1;

            // If value type, no null possible
            if (Utilities.IsValueType(typeof(T)))
                return false;

            bool isNullValue = ReferenceEquals(value, null);
            if (allowNullCount > 0 || allowIdentityReferenceCount > 0)
            {
                // Handle write
                if (Mode == SerializerMode.Write)
                {
                    // Handle reference
                    if (!isNullValue && allowIdentityReferenceCount > 0 && !isDynamic)
                    {
                        int position;
                        if (objectToPosition.TryGetValue(value, out position))
                        {
                            Writer.Write((byte)2);
                            Writer.Write(position);
                            return true;
                        }

                        // Register reference
                        objectToPosition.Add(value, (int)Stream.Position);
                    }

                    Writer.Write((byte)(isNullValue ? 0 : 1));

                    return isNullValue;
                }
                else
                {
                    // Handle read
                    value = default(T);
                    int objectPosition = (int)Stream.Position;
                    int objectReferenceHeader = Reader.ReadByte();
                    switch (objectReferenceHeader)
                    {
                        case 1:
                            {
                                if (allowIdentityReferenceCount > 0 && !isDynamic)
                                    storeObjectReference = objectPosition;
                            }
                            break;

                        case 2:
                            {
                                if (allowIdentityReferenceCount == 0)
                                    throw new InvalidOperationException("Can't read serialized reference when SerializeReference is off");

                                // Read object position from stream
                                objectPosition = Reader.ReadInt32();
                                object localValue;
                                if (!positionToObject.TryGetValue(objectPosition, out localValue))
                                    throw new InvalidOperationException(string.Format("Can't find serialized reference at position [{0}]", objectPosition));

                                // Set Object Reference
                                value = (T) localValue;

                                // Set it as null
                                objectReferenceHeader = 0;
                            }
                            break;
                    }

                    return objectReferenceHeader == 0;
                }
            }

            // Null is not allowed
            if (isNullValue && Mode == SerializerMode.Write)
            {
                throw new ArgumentNullException("value");
            }

            return false;
        }

        private void SerializeRawDynamic<T>(ref T value, bool noDynamic = false)
        {
            if (Mode == SerializerMode.Write)
            {
                var type = (noDynamic) ? typeof (T) : value.GetType();
                Dynamic dyn;
                if (!dynamicMapToFourCC.TryGetValue(type, out dyn))
                    throw new InvalidDataException(string.Format("Type [{0}] is not registered as dynamic", type));

                // Write the id of the object
                if (!noDynamic)
                    Writer.Write((int) dyn.Id);

                dyn.Writer(value, this);
            }
            else
            {
                // Gets the id for this dynamic
                Dynamic dyn;
                if (noDynamic)
                {
                    var type = typeof (T);
                    if (!dynamicMapToFourCC.TryGetValue(type, out dyn))
                        throw new InvalidDataException(string.Format("Type [{0}] is not registered as dynamic", type));
                }
                else
                {
                    var id = (FourCC) Reader.ReadInt32();

                    if (!dynamicMapToType.TryGetValue(id, out dyn))
                        throw new InvalidDataException(string.Format("Type [{0}] is not registered as dynamic", id));
                }

                value = (T) dyn.Reader(this);
            }
        }

        private void RegisterDynamic(Dynamic dynamic)
        {
            dynamicMapToFourCC.Add(dynamic.Type, dynamic);
            dynamicMapToType.Add(dynamic.Id, dynamic);
        }


        #region Primitive Array Readers

        private static object ReaderIntArray(BinarySerializer serializer)
        {
            int[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderUIntArray(BinarySerializer serializer)
        {
            uint[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderShortArray(BinarySerializer serializer)
        {
            short[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderUShortArray(BinarySerializer serializer)
        {
            ushort[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderLongArray(BinarySerializer serializer)
        {
            long[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderULongArray(BinarySerializer serializer)
        {
            ulong[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderBoolArray(BinarySerializer serializer)
        {
            bool[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderFloatArray(BinarySerializer serializer)
        {
            float[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderDoubleArray(BinarySerializer serializer)
        {
            double[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderDateTimeArray(BinarySerializer serializer)
        {
            DateTime[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderGuidArray(BinarySerializer serializer)
        {
            Guid[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderCharArray(BinarySerializer serializer)
        {
            char[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderStringArray(BinarySerializer serializer)
        {
            string[] value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderByteArray(BinarySerializer serializer)
        {
            byte[] value = null;
            serializer.Serialize(ref value);
            return value;
        }

        private static object ReaderSByteArray(BinarySerializer serializer)
        {
            sbyte[] values = null;
            serializer.Serialize(ref values, serializer.Serialize);
            return values;
        }

        #endregion

        #region Primitive Array Writer

        private static void WriterIntArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (int[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterUIntArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (uint[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterShortArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (short[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterUShortArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (ushort[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterLongArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (long[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterULongArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (ulong[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterSByteArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (sbyte[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterStringArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (string[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterCharArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (char[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterBoolArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (bool[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterFloatArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (float[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterDoubleArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (double[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterDateTimeArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (DateTime[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterGuidArray(object value, BinarySerializer serializer)
        {
            var valueTyped = (Guid[]) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterByteArray(object value, BinarySerializer serializer)
        {
            var valueArray = (byte[]) value;
            serializer.Serialize(ref valueArray);
        }

        #endregion

        #region Primitive List Readers

        private static object ReaderIntList(BinarySerializer serializer)
        {
            List<int> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderUIntList(BinarySerializer serializer)
        {
            List<uint> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderShortList(BinarySerializer serializer)
        {
            List<short> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderUShortList(BinarySerializer serializer)
        {
            List<ushort> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderLongList(BinarySerializer serializer)
        {
            List<long> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderULongList(BinarySerializer serializer)
        {
            List<ulong> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderBoolList(BinarySerializer serializer)
        {
            List<bool> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderFloatList(BinarySerializer serializer)
        {
            List<float> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderDoubleList(BinarySerializer serializer)
        {
            List<double> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderDateTimeList(BinarySerializer serializer)
        {
            List<DateTime> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderGuidList(BinarySerializer serializer)
        {
            List<Guid> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderCharList(BinarySerializer serializer)
        {
            List<char> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderStringList(BinarySerializer serializer)
        {
            List<string> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderByteList(BinarySerializer serializer)
        {
            List<byte> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        private static object ReaderSByteList(BinarySerializer serializer)
        {
            List<sbyte> value = null;
            serializer.Serialize(ref value, serializer.Serialize);
            return value;
        }

        #endregion

        #region Primitive List Writers

        private static void WriterIntList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<int>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterUIntList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<uint>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterShortList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<short>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterUShortList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<ushort>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterLongList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<long>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterULongList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<ulong>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterSByteList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<sbyte>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterStringList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<string>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterCharList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<char>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterBoolList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<bool>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterFloatList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<float>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterDoubleList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<double>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterDateTimeList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<DateTime>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterGuidList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<Guid>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        private static void WriterByteList(object value, BinarySerializer serializer)
        {
            var valueTyped = (List<byte>) value;
            serializer.Serialize(ref valueTyped, serializer.Serialize);
        }

        #endregion

        #region Primitive Readers

        private static object ReaderInt(BinarySerializer serializer)
        {
            return serializer.Reader.ReadInt32();
        }

        private static object ReaderUInt(BinarySerializer serializer)
        {
            return serializer.Reader.ReadUInt32();
        }

        private static object ReaderShort(BinarySerializer serializer)
        {
            return serializer.Reader.ReadInt16();
        }

        private static object ReaderUShort(BinarySerializer serializer)
        {
            return serializer.Reader.ReadUInt16();
        }

        private static object ReaderLong(BinarySerializer serializer)
        {
            return serializer.Reader.ReadInt64();
        }

        private static object ReaderULong(BinarySerializer serializer)
        {
            return serializer.Reader.ReadUInt64();
        }

        private static object ReaderBool(BinarySerializer serializer)
        {
            return serializer.Reader.ReadBoolean();
        }

        private static object ReaderByte(BinarySerializer serializer)
        {
            return serializer.Reader.ReadByte();
        }

        private static object ReaderSByte(BinarySerializer serializer)
        {
            return serializer.Reader.ReadSByte();
        }

        private static object ReaderString(BinarySerializer serializer)
        {
            var value = (string) null;
            serializer.Serialize(ref value);
            return value;
        }

        private static object ReaderFloat(BinarySerializer serializer)
        {
            return serializer.Reader.ReadSingle();
        }

        private static object ReaderDouble(BinarySerializer serializer)
        {
            return serializer.Reader.ReadDouble();
        }

        private static object ReaderChar(BinarySerializer serializer)
        {
            return serializer.Reader.ReadChar();
        }

        private static object ReaderDateTime(BinarySerializer serializer)
        {
            return new DateTime(serializer.Reader.ReadInt64());
        }

        private static object ReaderGuid(BinarySerializer serializer)
        {
            return new Guid(serializer.Reader.ReadBytes(16));
        }

        #endregion

        #region Primitive Writers

        private static void WriterInt(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((int) value);
        }

        private static void WriterUInt(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((uint) value);
        }

        private static void WriterShort(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((short) value);
        }

        private static void WriterUShort(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((ushort) value);
        }

        private static void WriterLong(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((long) value);
        }

        private static void WriterULong(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((ulong) value);
        }

        private static void WriterByte(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((byte) value);
        }

        private static void WriterSByte(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((sbyte) value);
        }

        private static void WriterString(object value, BinarySerializer serializer)
        {
            var str = (string) value;
            serializer.Serialize(ref str);
        }

        private static void WriterChar(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((char) value);
        }

        private static void WriterBool(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((bool) value);
        }

        private static void WriterFloat(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((float) value);
        }

        private static void WriterDouble(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write((double) value);
        }

        private static void WriterDateTime(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write(((DateTime) value).ToBinary());
        }

        private static void WriterGuid(object value, BinarySerializer serializer)
        {
            serializer.Writer.Write(((Guid) value).ToByteArray());
        }

        #endregion

        #region IDataSerializer Reader and Writer (+ Array and List)

        private static object ReaderDataSerializer<T>(BinarySerializer serializer) where T : IDataSerializer, new()
        {
            var value = default(T);
            serializer.Serialize(ref value);
            return value;
        }

        private static void WriterDataSerializer<T>(object value, BinarySerializer serializer) where T : IDataSerializer, new()
        {
            var valueTyped = (T)value;
            serializer.Serialize(ref valueTyped);
        }

        private static object ReaderDataSerializerArray<T>(BinarySerializer serializer) where T : IDataSerializer, new()
        {
            var value = (T[]) null;
            serializer.Serialize(ref value);
            return value;
        }

        private static void WriterDataSerializerArray<T>(object value, BinarySerializer serializer) where T : IDataSerializer, new()
        {
            var valueList = (T[]) value;
            serializer.Serialize(ref valueList);
        }

        private static object ReaderDataSerializerList<T>(BinarySerializer serializer) where T : IDataSerializer, new()
        {
            var value = (List<T>) null;
            serializer.Serialize(ref value);
            return value;
        }

        private static void WriterDataSerializerList<T>(object value, BinarySerializer serializer) where T : IDataSerializer, new()
        {
            var valueList = (List<T>) value;
            serializer.Serialize(ref valueList);
        }

        #endregion


        private static readonly Dynamic[] DefaultDynamics =
            {
                new Dynamic {Id = 0, Type = typeof (int), Reader = ReaderInt, Writer = WriterInt},
                new Dynamic {Id = 1, Type = typeof (uint), Reader = ReaderUInt, Writer = WriterUInt},
                new Dynamic {Id = 2, Type = typeof (short), Reader = ReaderShort, Writer = WriterShort},
                new Dynamic {Id = 3, Type = typeof (ushort), Reader = ReaderUShort, Writer = WriterUShort},
                new Dynamic {Id = 4, Type = typeof (long), Reader = ReaderLong, Writer = WriterLong},
                new Dynamic {Id = 5, Type = typeof (ulong), Reader = ReaderULong, Writer = WriterULong},
                new Dynamic {Id = 6, Type = typeof (byte), Reader = ReaderByte, Writer = WriterByte},
                new Dynamic {Id = 7, Type = typeof (sbyte), Reader = ReaderSByte, Writer = WriterSByte},
                new Dynamic {Id = 8, Type = typeof (bool), Reader = ReaderBool, Writer = WriterBool},
                new Dynamic {Id = 9, Type = typeof (float), Reader = ReaderFloat, Writer = WriterFloat},
                new Dynamic {Id = 10, Type = typeof (double), Reader = ReaderDouble, Writer = WriterDouble},
                new Dynamic {Id = 11, Type = typeof (string), Reader = ReaderString, Writer = WriterString},
                new Dynamic {Id = 12, Type = typeof (char), Reader = ReaderChar, Writer = WriterChar},
                new Dynamic {Id = 13, Type = typeof (DateTime), Reader = ReaderDateTime, Writer = WriterDateTime},
                new Dynamic {Id = 14, Type = typeof (Guid), Reader = ReaderGuid, Writer = WriterGuid},

                new Dynamic {Id = 30, Type = typeof (int[]), Reader = ReaderIntArray, Writer = WriterIntArray},
                new Dynamic {Id = 31, Type = typeof (uint[]), Reader = ReaderUIntArray, Writer = WriterUIntArray},
                new Dynamic {Id = 32, Type = typeof (short[]), Reader = ReaderShortArray, Writer = WriterShortArray},
                new Dynamic {Id = 33, Type = typeof (ushort[]), Reader = ReaderUShortArray, Writer = WriterUShortArray},
                new Dynamic {Id = 34, Type = typeof (long[]), Reader = ReaderLongArray, Writer = WriterLongArray},
                new Dynamic {Id = 35, Type = typeof (ulong[]), Reader = ReaderULongArray, Writer = WriterULongArray},
                new Dynamic {Id = 36, Type = typeof (byte[]), Reader = ReaderByteArray, Writer = WriterByteArray},
                new Dynamic {Id = 37, Type = typeof (sbyte[]), Reader = ReaderSByteArray, Writer = WriterSByteArray},
                new Dynamic {Id = 38, Type = typeof (bool[]), Reader = ReaderBoolArray, Writer = WriterBoolArray},
                new Dynamic {Id = 39, Type = typeof (float[]), Reader = ReaderFloatArray, Writer = WriterFloatArray},
                new Dynamic {Id = 40, Type = typeof (double[]), Reader = ReaderDoubleArray, Writer = WriterDoubleArray},
                new Dynamic {Id = 41, Type = typeof (string[]), Reader = ReaderStringArray, Writer = WriterStringArray},
                new Dynamic {Id = 42, Type = typeof (char[]), Reader = ReaderCharArray, Writer = WriterCharArray},
                new Dynamic {Id = 43, Type = typeof (DateTime[]), Reader = ReaderDateTimeArray, Writer = WriterDateTimeArray},
                new Dynamic {Id = 44, Type = typeof (Guid[]), Reader = ReaderGuidArray, Writer = WriterGuidArray},

                new Dynamic {Id = 60, Type = typeof (List<int>), Reader = ReaderIntList, Writer = WriterIntList},
                new Dynamic {Id = 61, Type = typeof (List<uint>), Reader = ReaderUIntList, Writer = WriterUIntList},
                new Dynamic {Id = 62, Type = typeof (List<short>), Reader = ReaderShortList, Writer = WriterShortList},
                new Dynamic {Id = 63, Type = typeof (List<ushort>), Reader = ReaderUShortList, Writer = WriterUShortList},
                new Dynamic {Id = 64, Type = typeof (List<long>), Reader = ReaderLongList, Writer = WriterLongList},
                new Dynamic {Id = 65, Type = typeof (List<ulong>), Reader = ReaderULongList, Writer = WriterULongList},
                new Dynamic {Id = 66, Type = typeof (List<byte>), Reader = ReaderByteList, Writer = WriterByteList},
                new Dynamic {Id = 67, Type = typeof (List<sbyte>), Reader = ReaderSByteList, Writer = WriterSByteList},
                new Dynamic {Id = 68, Type = typeof (List<bool>), Reader = ReaderBoolList, Writer = WriterBoolList},
                new Dynamic {Id = 69, Type = typeof (List<float>), Reader = ReaderFloatList, Writer = WriterFloatList},
                new Dynamic {Id = 70, Type = typeof (List<double>), Reader = ReaderDoubleList, Writer = WriterDoubleList},
                new Dynamic {Id = 71, Type = typeof (List<string>), Reader = ReaderStringList, Writer = WriterStringList},
                new Dynamic {Id = 72, Type = typeof (List<char>), Reader = ReaderCharList, Writer = WriterCharList},
                new Dynamic {Id = 73, Type = typeof (List<DateTime>), Reader = ReaderDateTimeList, Writer = WriterDateTimeList},
                new Dynamic {Id = 74, Type = typeof (List<Guid>), Reader = ReaderGuidList, Writer = WriterGuidList},
            };

        private Chunk CurrentChunk
        {
            get { return currentChunk; }
            set { currentChunk = value; }
        }

        private class Chunk
        {
            public FourCC Id;
            public long ChunkIndexStart;
            public long ChunkIndexEnd;
        }

        private void StoreObjectRef(object value, int position)
        {
            objectToPosition.Add(value, position);
            positionToObject.Add(position, value);
        }

        private void ResetStoredReference()
        {
            positionToObject.Clear();
            objectToPosition.Clear();
        }

        private class Dynamic
        {
            public FourCC Id;

            public Type Type;

            public ReadRef Reader;

            public WriteRef Writer;

            public SerializerAction DynamicSerializer;

            public object DynamicReader<T>(BinarySerializer serializer) where T : new()
            {
                object value = new T();
                DynamicSerializer(ref value, serializer);
                return value;
            }

            public void DynamicWriter(object value, BinarySerializer serializer)
            {
                DynamicSerializer(ref value, serializer);
            }
        }
    }
}