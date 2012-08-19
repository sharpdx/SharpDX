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
    public class SerializerContext
    {
        private int chunkCount;
        private Chunk[] chunks;
        private Chunk currentChunk;

        private Chunk CurrentChunk
        {
            get { return currentChunk; }
            set
            {
                currentChunk = value;
                currentReader = currentChunk.Reader;
                currentWriter = currentChunk.Writer;
            }
        }

        private BinaryReader currentReader;
        private BinaryWriter currentWriter;
        private Dictionary<FourCC, Dynamic> dynamicMapToType;
        private Dictionary<Type, Dynamic> dynamicMapToFourCC;

        private class Chunk
        {
            public FourCC Id;
            public BinaryReader Reader;
            public BinaryWriter Writer;
            public Stream Buffer;
            public long ChunkIndexEnd;
        }

        private class Dynamic
        {
            public FourCC Id;

            public Type Type;

            public ReadRef Reader;

            public WriteRef Writer;

            public SerializerDynamicAction DynamicSerializer;

            public object DynamicReader<T>(SerializerContext context) where T : new()
            {
                object value = new T();
                DynamicSerializer(ref value, context);
                return value;
            }

            public void DynamicWriter(object value, SerializerContext context)
            {
                DynamicSerializer(ref value, context);
            }
        }

        public delegate void SerializerPrimitiveAction<T>(ref T value);

        public delegate void SerializerAction<T>(ref T value, SerializerContext context);

        public delegate void SerializerDynamicAction(ref object value, SerializerContext context);


        public delegate object ReadRef(SerializerContext context);

        public delegate void WriteRef(object value, SerializerContext context);

        public SerializerContext(Stream stream, SerializerMode mode)
        {
            dynamicMapToType = new Dictionary<FourCC, Dynamic>();
            dynamicMapToFourCC = new Dictionary<Type, Dynamic>();

            chunks = new Chunk[8];
            CurrentChunk = new Chunk {Buffer = stream, Writer = new BinaryWriter(stream), Reader = new BinaryReader(stream)};
            chunks[chunkCount] = CurrentChunk;
            chunkCount++;

            Mode = mode;

            foreach (var defaultDynamic in DefaultDynamics)
                RegisterDynamic(defaultDynamic);
        }

        public SerializerMode Mode { get; set; }

        public void RegisterDynamic<T>(FourCC id) where T : IDataSerializer, new()
        {
            RegisterDynamic(new Dynamic { Id = id, Type = typeof (T), Reader = ReaderDataSerializer<T>, Writer = WriterDataSerializer<T>});
        }

        public void RegisterDynamicArray<T>(FourCC id) where T : IDataSerializer, new()
        {
            RegisterDynamic(new Dynamic { Id = id, Type = typeof(T[]), Reader = ReaderDataSerializerArray<T>, Writer = WriterDataSerializerArray<T> });
        }

        public void RegisterDynamicList<T>(FourCC id) where T : IDataSerializer, new()
        {
            RegisterDynamic(new Dynamic { Id = id, Type = typeof(List<T>), Reader = ReaderDataSerializerList<T>, Writer = WriterDataSerializerList<T> });
        }

        public void RegisterDynamic<T>(FourCC id, SerializerDynamicAction serializer) where T : new()
        {
            var dynamicSerializer = new Dynamic {Id = id, Type = typeof (T), DynamicSerializer = serializer};
            dynamicSerializer.Reader = dynamicSerializer.DynamicReader<T>;
            dynamicSerializer.Writer = dynamicSerializer.DynamicWriter;
            RegisterDynamic(dynamicSerializer);
        }

        private static readonly Dynamic[] DefaultDynamics = {
                new Dynamic {Id = 0,  Type = typeof (int),      Reader = ReaderInt,      Writer = WriterInt},
                new Dynamic {Id = 1,  Type = typeof (uint),     Reader = ReaderUInt,     Writer = WriterUInt},
                new Dynamic {Id = 2,  Type = typeof (short),    Reader = ReaderShort,    Writer = WriterShort},
                new Dynamic {Id = 3,  Type = typeof (ushort),   Reader = ReaderUShort,   Writer = WriterUShort},
                new Dynamic {Id = 4,  Type = typeof (long),     Reader = ReaderLong,     Writer = WriterLong},
                new Dynamic {Id = 5,  Type = typeof (ulong),    Reader = ReaderULong,    Writer = WriterULong},
                new Dynamic {Id = 6,  Type = typeof (byte),     Reader = ReaderByte,     Writer = WriterByte},
                new Dynamic {Id = 7,  Type = typeof (sbyte),    Reader = ReaderSByte,    Writer = WriterSByte},
                new Dynamic {Id = 8,  Type = typeof (bool),     Reader = ReaderBool,     Writer = WriterBool},
                new Dynamic {Id = 9,  Type = typeof (float),    Reader = ReaderFloat,    Writer = WriterFloat},
                new Dynamic {Id = 10, Type = typeof (double),   Reader = ReaderDouble,   Writer = WriterDouble},
                new Dynamic {Id = 11, Type = typeof (string),   Reader = ReaderString,   Writer = WriterString},
                new Dynamic {Id = 12, Type = typeof (char),     Reader = ReaderChar,     Writer = WriterChar},
                new Dynamic {Id = 13, Type = typeof (DateTime), Reader = ReaderDateTime, Writer = WriterDateTime},
                new Dynamic {Id = 14, Type = typeof (Guid),     Reader = ReaderGuid,     Writer = WriterGuid},

                new Dynamic {Id = 30,  Type = typeof (int[]),     Reader = ReaderIntArray,      Writer = WriterIntArray},
                new Dynamic {Id = 31,  Type = typeof (uint[]),    Reader = ReaderUIntArray,     Writer = WriterUIntArray},
                new Dynamic {Id = 32,  Type = typeof (short[]),   Reader = ReaderShortArray,    Writer = WriterShortArray},
                new Dynamic {Id = 33,  Type = typeof (ushort[]),  Reader = ReaderUShortArray,   Writer = WriterUShortArray},
                new Dynamic {Id = 34,  Type = typeof (long[]),    Reader = ReaderLongArray,     Writer = WriterLongArray},
                new Dynamic {Id = 35,  Type = typeof (ulong[]),   Reader = ReaderULongArray,    Writer = WriterULongArray},
                new Dynamic {Id = 36,  Type = typeof (byte[]),    Reader = ReaderByteArray,     Writer = WriterByteArray},
                new Dynamic {Id = 37,  Type = typeof (sbyte[]),   Reader = ReaderSByteArray,    Writer = WriterSByteArray},
                new Dynamic {Id = 38,  Type = typeof (bool[]),    Reader = ReaderBoolArray,     Writer = WriterBoolArray},
                new Dynamic {Id = 39,  Type = typeof (float[]),   Reader = ReaderFloatArray,    Writer = WriterFloatArray},
                new Dynamic {Id = 40, Type = typeof (double[]),   Reader = ReaderDoubleArray,   Writer = WriterDoubleArray},
                new Dynamic {Id = 41, Type = typeof (string[]),   Reader = ReaderStringArray,   Writer = WriterStringArray},
                new Dynamic {Id = 42, Type = typeof (char[]),     Reader = ReaderCharArray,     Writer = WriterCharArray},
                new Dynamic {Id = 43, Type = typeof (DateTime[]), Reader = ReaderDateTimeArray, Writer = WriterDateTimeArray},
                new Dynamic {Id = 44, Type = typeof (Guid[]),     Reader = ReaderGuidArray,     Writer = WriterGuidArray},

                new Dynamic {Id = 60,  Type = typeof (List<int>),     Reader = ReaderIntList,      Writer = WriterIntList},
                new Dynamic {Id = 61,  Type = typeof (List<uint>),    Reader = ReaderUIntList,     Writer = WriterUIntList},
                new Dynamic {Id = 62,  Type = typeof (List<short>),   Reader = ReaderShortList,    Writer = WriterShortList},
                new Dynamic {Id = 63,  Type = typeof (List<ushort>),  Reader = ReaderUShortList,   Writer = WriterUShortList},
                new Dynamic {Id = 64,  Type = typeof (List<long>),    Reader = ReaderLongList,     Writer = WriterLongList},
                new Dynamic {Id = 65,  Type = typeof (List<ulong>),   Reader = ReaderULongList,    Writer = WriterULongList},
                new Dynamic {Id = 66,  Type = typeof (List<byte>),    Reader = ReaderByteList,     Writer = WriterByteList},
                new Dynamic {Id = 67,  Type = typeof (List<sbyte>),   Reader = ReaderSByteList,    Writer = WriterSByteList},
                new Dynamic {Id = 68,  Type = typeof (List<bool>),    Reader = ReaderBoolList,     Writer = WriterBoolList},
                new Dynamic {Id = 69,  Type = typeof (List<float>),   Reader = ReaderFloatList,    Writer = WriterFloatList},
                new Dynamic {Id = 70, Type = typeof (List<double>),   Reader = ReaderDoubleList,   Writer = WriterDoubleList},
                new Dynamic {Id = 71, Type = typeof (List<string>),   Reader = ReaderStringList,   Writer = WriterStringList},
                new Dynamic {Id = 72, Type = typeof (List<char>),     Reader = ReaderCharList,     Writer = WriterCharList},
                new Dynamic {Id = 73, Type = typeof (List<DateTime>), Reader = ReaderDateTimeList, Writer = WriterDateTimeList},
                new Dynamic {Id = 74, Type = typeof (List<Guid>),     Reader = ReaderGuidList,     Writer = WriterGuidList},
            };

       public void BeginChunk(FourCC chunkId)
        {
            // Allocate new Chunk
            if (chunks[chunkCount] == null)
            {
                var buffer = new MemoryStream();
                CurrentChunk = new Chunk {Buffer = buffer, Writer = new BinaryWriter(buffer), Reader = chunks[chunkCount - 1].Reader};

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
                CurrentChunk.Buffer.Position = 0;

            if (chunkCount >= chunks.Length)
            {
                var temp = new Chunk[chunks.Length * 2];
                Array.Copy(chunks, temp, chunks.Length);
                chunks = temp;
            } 
            
            if (Mode == SerializerMode.Write)
            {
                // Write the chunkId to the current chunk
                chunks[chunkCount - 2].Writer.Write((int)chunkId);
            }
            else
            {
                var chunkIdRead = currentReader.ReadInt32();
                if (chunkIdRead != chunkId)
                    throw new InvalidDataException(string.Format("Chunk [{0}] read from stream doesn't match expecting chunk [{1}]", chunkIdRead, chunkId));

                CurrentChunk.ChunkIndexEnd = chunks[0].Buffer.Position + 4 + currentReader.ReadUInt32();
            }
        }

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
                currentWriter.Write((uint)previousChunk.Buffer.Length);

                // Write the whole chunk
                currentWriter.Write(((MemoryStream)previousChunk.Buffer).GetBuffer(), 0, (int)previousChunk.Buffer.Length);
            }
            else
            {
                if (previousChunk.ChunkIndexEnd != CurrentChunk.Buffer.Position)
                    throw new InvalidDataException(string.Format("Unexpected size when reading chunk [{0}]", CurrentChunk.Id));
            }
        }

        public T Load<T>() where T : IDataSerializer, new()
        {
            Mode = SerializerMode.Read;
            T value = default(T);
            Serialize(ref value);
            return value;
        }


        public void Save<T>(T value) where T : IDataSerializer, new()
        {
            Mode = SerializerMode.Write;
            Serialize(ref value);
            Flush();
        }

        public void Flush()
        {
            chunks[0].Writer.Flush();
        }

        public void SerializeDynamic<T>(ref T value)
        {
            if (SerializeIsNull(ref value))
                return;
            SerializeRawDynamic(ref value);
        }

        public void Serialize<T>(ref T value, bool allowNull = true) where T : IDataSerializer, new()
        {
            if (!typeof(T).IsValueType && SerializeIsNull(ref value, allowNull))
                return;

            if (Mode == SerializerMode.Read)
                value = new T();

            value.Serialize(this);
        }

        //public void Serialize<T>(ref T value, SerializerAction<T> serializer) where T : new()
        //{
        //    if (!typeof(T).IsValueType && SerializeIsNull(ref value))
        //        return;

        //    if (Mode == SerializerMode.Read)
        //        value = new T();

        //    serializer(ref value, this);
        //}

        public void Serialize(ref string[] valueList)
        {
            if (SerializeIsNull(ref valueList))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(valueList.Length);
                foreach (var value in valueList)
                {
                    string localValue = value;
                    Serialize(ref localValue);
                }
            }
            else
            {
                var count = currentReader.ReadInt32();
                valueList = new string[count];
                for (int index = 0; index < count; index++)
                    Serialize(ref valueList[index]);
            }
        }


        public void Serialize<T>(ref T[] valueList, SerializerPrimitiveAction<T> writer)
        {
            if (SerializeIsNull(ref valueList))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write((int)valueList.Length);
                foreach (var value in valueList)
                {
                    T localValue = value;
                    writer(ref localValue);
                }
            }
            else
            {
                var count = currentReader.ReadInt32();
                valueList = new T[count];
                for (int index = 0; index < count; index++)
                    writer(ref valueList[index]);
            }
        }

        public void Serialize<T>(ref T[] valueList) where T : IDataSerializer, new()
        {
            if (SerializeIsNull(ref valueList))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(valueList.Length);
                foreach (var value in valueList)
                {
                    T localValue = value;
                    Serialize(ref localValue);
                }
            }
            else
            {
                var count = currentReader.ReadInt32();
                valueList = new T[count];
                for (int index = 0; index < count; index++)
                    Serialize(ref valueList[index]);
            }
        }

        public void Serialize(ref byte[] valueList)
        {
            if (SerializeIsNull(ref valueList))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(valueList.Length);
                currentWriter.Write(valueList);
            }
            else
            {
                int count = currentReader.ReadInt32();
                valueList = new byte[count];
                currentReader.Read(valueList, 0, count);
            }
        }

        public void Serialize<T>(ref List<T> valueList) where T : IDataSerializer, new()
        {
            if (SerializeIsNull(ref valueList))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(valueList.Count);
                foreach (var value in valueList)
                {
                    T localValue = value;
                    Serialize(ref localValue);
                }
            }
            else
            {
                var count = currentReader.ReadInt32();
                valueList = new List<T>(count);
                for (int index = 0; index < count; index++)
                {
                    var value = default(T);
                    Serialize(ref value);
                    valueList.Add(value);
                }
            }
        }

        public void Serialize<T>(ref List<T> valueList, SerializerPrimitiveAction<T> writer)
        {
            if (SerializeIsNull(ref valueList))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(valueList.Count);
                foreach (var value in valueList)
                {
                    T localValue = value;
                    writer(ref localValue);
                }
            }
            else
            {
                var count = currentReader.ReadInt32();
                valueList = new List<T>(count);
                for (int i = 0; i < count; i++)
                {
                    var localValue = default(T);
                    writer(ref localValue);
                    valueList.Add(localValue);
                }
            }
        }

        public void Serialize(ref string value)
        {
            if (SerializeIsNull(ref value))
                return;

            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadString();
            }
        }

        public void Serialize(ref bool value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadBoolean();
            }
        }

        public void Serialize(ref byte value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadByte();
            }
        }

        public void Serialize(ref sbyte value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadSByte();
            }
        }

        public void Serialize(ref short value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadInt16();
            }
        }

        public void Serialize(ref ushort value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadUInt16();
            }
        }

        public void Serialize(ref int value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadInt32();
            }
        }

        public void Serialize(ref uint value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadUInt32();
            }
        }

        public void Serialize(ref long value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadInt64();
            }
        }

        public void Serialize(ref ulong value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadUInt64();
            }
        }

        public void Serialize(ref char value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadChar();
            }
        }

        public void Serialize(ref float value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadSingle();
            }
        }

        public void Serialize(ref double value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value);
            }
            else
            {
                value = currentReader.ReadDouble();
            }
        }

        public void Serialize(ref DateTime value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value.ToBinary());
            }
            else
            {
                value = new DateTime(currentReader.ReadInt64());
            }
        }

        public void Serialize(ref Guid value)
        {
            if (Mode == SerializerMode.Write)
            {
                currentWriter.Write(value.ToByteArray());
            }
            else
            {
                value = new Guid(currentReader.ReadBytes(16));
            }
        }

        private bool SerializeIsNull<T>(ref T value, bool allowNull = true)
        {
            if (Mode == SerializerMode.Write)
            {
                bool isNullValue = ReferenceEquals(value, null);
                if (!allowNull && isNullValue)
                    throw new InvalidDataException("Cannot serialize null");
                currentWriter.Write((byte)(isNullValue ? 0 : 1));
                return isNullValue;
            }

            value = default(T);
            return currentReader.ReadByte() == 0;
        }

        private void SerializeRawDynamic<T>(ref T value, bool noDynamic = false)
        {
            if (Mode == SerializerMode.Write)
            {
                var type = (noDynamic) ? typeof(T) : value.GetType();
                Dynamic dyn;
                if (!dynamicMapToFourCC.TryGetValue(type, out dyn))
                    throw new InvalidDataException(string.Format("Type [{0}] is not registered as dynamic", type));

                // Write the id of the object
                if (!noDynamic)
                    currentWriter.Write((int)dyn.Id);

                dyn.Writer(value, this);
            }
            else
            {
                // Gets the id for this dynamic
                Dynamic dyn;
                if (noDynamic)
                {
                    var type = typeof(T);
                    if (!dynamicMapToFourCC.TryGetValue(type, out dyn))
                        throw new InvalidDataException(string.Format("Type [{0}] is not registered as dynamic", type));
                }
                else
                {
                    var id = (FourCC)currentReader.ReadInt32();

                    if (!dynamicMapToType.TryGetValue(id, out dyn))
                        throw new InvalidDataException(string.Format("Type [{0}] is not registered as dynamic", id));
                }

                value = (T)dyn.Reader(this);
            }
        }

        private void RegisterDynamic(Dynamic dynamic)
        {
            dynamicMapToFourCC.Add(dynamic.Type, dynamic);
            dynamicMapToType.Add(dynamic.Id, dynamic);
        }


        #region Primitive Array Readers

        private static object ReaderIntArray(SerializerContext context)
        {
            int[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderUIntArray(SerializerContext context)
        {
            uint[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderShortArray(SerializerContext context)
        {
            short[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderUShortArray(SerializerContext context)
        {
            ushort[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderLongArray(SerializerContext context)
        {
            long[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderULongArray(SerializerContext context)
        {
            ulong[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderBoolArray(SerializerContext context)
        {
            bool[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderFloatArray(SerializerContext context)
        {
            float[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderDoubleArray(SerializerContext context)
        {
            double[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderDateTimeArray(SerializerContext context)
        {
            DateTime[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderGuidArray(SerializerContext context)
        {
            Guid[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderCharArray(SerializerContext context)
        {
            char[] value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderStringArray(SerializerContext context)
        {
            string[] value = null;
            context.Serialize(ref value);
            return value;
        }

        private static object ReaderByteArray(SerializerContext context)
        {
            int count = context.currentReader.ReadInt32();
            return context.currentReader.ReadBytes(count);
        }

        private static object ReaderSByteArray(SerializerContext context)
        {
            sbyte[] values = null;
            context.Serialize(ref values, context.Serialize);
            return values;
        }

        #endregion

        #region Primitive Array Writer

        private static void WriterIntArray(object value, SerializerContext context)
        {
            var valueTyped = (int[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterUIntArray(object value, SerializerContext context)
        {
            var valueTyped = (uint[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterShortArray(object value, SerializerContext context)
        {
            var valueTyped = (short[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterUShortArray(object value, SerializerContext context)
        {
            var valueTyped = (ushort[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterLongArray(object value, SerializerContext context)
        {
            var valueTyped = (long[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterULongArray(object value, SerializerContext context)
        {
            var valueTyped = (ulong[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterSByteArray(object value, SerializerContext context)
        {
            var valueTyped = (sbyte[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterStringArray(object value, SerializerContext context)
        {
            var valueTyped = (string[]) value;
            context.Serialize(ref valueTyped);
        }

        private static void WriterCharArray(object value, SerializerContext context)
        {
            var valueTyped = (char[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterBoolArray(object value, SerializerContext context)
        {
            var valueTyped = (bool[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterFloatArray(object value, SerializerContext context)
        {
            var valueTyped = (float[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterDoubleArray(object value, SerializerContext context)
        {
            var valueTyped = (double[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterDateTimeArray(object value, SerializerContext context)
        {
            var valueTyped = (DateTime[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterGuidArray(object value, SerializerContext context)
        {
            var valueTyped = (Guid[]) value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterByteArray(object value, SerializerContext context)
        {
            var valueArray = (byte[]) value;
            context.currentWriter.Write(valueArray.Length);
            context.currentWriter.Write(valueArray);
        }

        #endregion

        #region Primitive List Readers

        private static object ReaderIntList(SerializerContext context)
        {
            List<int> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderUIntList(SerializerContext context)
        {
            List<uint> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderShortList(SerializerContext context)
        {
            List<short> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderUShortList(SerializerContext context)
        {
            List<ushort> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderLongList(SerializerContext context)
        {
            List<long> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderULongList(SerializerContext context)
        {
            List<ulong> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderBoolList(SerializerContext context)
        {
            List<bool> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderFloatList(SerializerContext context)
        {
            List<float> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderDoubleList(SerializerContext context)
        {
            List<double> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderDateTimeList(SerializerContext context)
        {
            List<DateTime> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderGuidList(SerializerContext context)
        {
            List<Guid> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderCharList(SerializerContext context)
        {
            List<char> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderStringList(SerializerContext context)
        {
            List<string> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderByteList(SerializerContext context)
        {
            List<byte> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        private static object ReaderSByteList(SerializerContext context)
        {
            List<sbyte> value = null;
            context.Serialize(ref value, context.Serialize);
            return value;
        }

        #endregion

        #region Primitive List Writers

        private static void WriterIntList(object value, SerializerContext context)
        {
            var valueTyped = (List<int>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterUIntList(object value, SerializerContext context)
        {
            var valueTyped = (List<uint>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterShortList(object value, SerializerContext context)
        {
            var valueTyped = (List<short>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterUShortList(object value, SerializerContext context)
        {
            var valueTyped = (List<ushort>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterLongList(object value, SerializerContext context)
        {
            var valueTyped = (List<long>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterULongList(object value, SerializerContext context)
        {
            var valueTyped = (List<ulong>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterSByteList(object value, SerializerContext context)
        {
            var valueTyped = (List<sbyte>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterStringList(object value, SerializerContext context)
        {
            var valueTyped = (List<string>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterCharList(object value, SerializerContext context)
        {
            var valueTyped = (List<char>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterBoolList(object value, SerializerContext context)
        {
            var valueTyped = (List<bool>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterFloatList(object value, SerializerContext context)
        {
            var valueTyped = (List<float>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterDoubleList(object value, SerializerContext context)
        {
            var valueTyped = (List<double>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterDateTimeList(object value, SerializerContext context)
        {
            var valueTyped = (List<DateTime>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterGuidList(object value, SerializerContext context)
        {
            var valueTyped = (List<Guid>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        private static void WriterByteList(object value, SerializerContext context)
        {
            var valueTyped = (List<byte>)value;
            context.Serialize(ref valueTyped, context.Serialize);
        }

        #endregion

        #region Primitive Readers

        private static object ReaderInt(SerializerContext context)
        {
            return context.currentReader.ReadInt32();
        }

        private static object ReaderUInt(SerializerContext context)
        {
            return context.currentReader.ReadUInt32();
        }

        private static object ReaderShort(SerializerContext context)
        {
            return context.currentReader.ReadInt16();
        }

        private static object ReaderUShort(SerializerContext context)
        {
            return context.currentReader.ReadUInt16();
        }

        private static object ReaderLong(SerializerContext context)
        {
            return context.currentReader.ReadInt64();
        }

        private static object ReaderULong(SerializerContext context)
        {
            return context.currentReader.ReadUInt64();
        }

        private static object ReaderBool(SerializerContext context)
        {
            return context.currentReader.ReadBoolean();
        }

        private static object ReaderByte(SerializerContext context)
        {
            return context.currentReader.ReadByte();
        }

        private static object ReaderSByte(SerializerContext context)
        {
            return context.currentReader.ReadSByte();
        }

        private static object ReaderString(SerializerContext context)
        {
            return context.currentReader.ReadString();
        }

        private static object ReaderFloat(SerializerContext context)
        {
            return context.currentReader.ReadSingle();
        }

        private static object ReaderDouble(SerializerContext context)
        {
            return context.currentReader.ReadDouble();
        }

        private static object ReaderChar(SerializerContext context)
        {
            return context.currentReader.ReadChar();
        }

        private static object ReaderDateTime(SerializerContext context)
        {
            return new DateTime(context.currentReader.ReadInt64());
        }

        private static object ReaderGuid(SerializerContext context)
        {
            return new Guid(context.currentReader.ReadBytes(16));
        }

        #endregion

        #region Primitive Writers

        private static void WriterInt(object value, SerializerContext context)
        {
            context.currentWriter.Write((int) value);
        }

        private static void WriterUInt(object value, SerializerContext context)
        {
            context.currentWriter.Write((uint) value);
        }

        private static void WriterShort(object value, SerializerContext context)
        {
            context.currentWriter.Write((short) value);
        }

        private static void WriterUShort(object value, SerializerContext context)
        {
            context.currentWriter.Write((ushort) value);
        }

        private static void WriterLong(object value, SerializerContext context)
        {
            context.currentWriter.Write((long) value);
        }

        private static void WriterULong(object value, SerializerContext context)
        {
            context.currentWriter.Write((ulong) value);
        }

        private static void WriterByte(object value, SerializerContext context)
        {
            context.currentWriter.Write((byte) value);
        }

        private static void WriterSByte(object value, SerializerContext context)
        {
            context.currentWriter.Write((sbyte) value);
        }

        private static void WriterString(object value, SerializerContext context)
        {
            context.currentWriter.Write((string) value);
        }

        private static void WriterChar(object value, SerializerContext context)
        {
            context.currentWriter.Write((char) value);
        }

        private static void WriterBool(object value, SerializerContext context)
        {
            context.currentWriter.Write((bool) value);
        }

        private static void WriterFloat(object value, SerializerContext context)
        {
            context.currentWriter.Write((float) value);
        }

        private static void WriterDouble(object value, SerializerContext context)
        {
            context.currentWriter.Write((double) value);
        }

        private static void WriterDateTime(object value, SerializerContext context)
        {
            context.currentWriter.Write(((DateTime) value).ToBinary());
        }

        private static void WriterGuid(object value, SerializerContext context)
        {
            context.currentWriter.Write(((Guid) value).ToByteArray());
        }

        #endregion

        #region IDataSerializer Reader and Writer (+ Array and List)

        private static object ReaderDataSerializer<T>(SerializerContext context) where T : IDataSerializer, new()
        {
            var value = new T();
            ((IDataSerializer) value).Serialize(context);
            return value;
        }

        private static void WriterDataSerializer<T>(object value, SerializerContext context)  where T : IDataSerializer
        {
            ((IDataSerializer) value).Serialize(context);
        }

        private static object ReaderDataSerializerArray<T>(SerializerContext context) where T : IDataSerializer, new()
        {
            var value = (T[]) null;
            context.Serialize(ref value);
            return value;
        }

        private static void WriterDataSerializerArray<T>(object value, SerializerContext context) where T : IDataSerializer, new()
        {
            var valueList = (T[])value;
            context.Serialize(ref valueList);
        }

        private static object ReaderDataSerializerList<T>(SerializerContext context) where T : IDataSerializer, new()
        {
            var value = (List<T>)null;
            context.Serialize(ref value);
            return value;
        }

        private static void WriterDataSerializerList<T>(object value, SerializerContext context) where T : IDataSerializer, new()
        {
            var valueList = (List<T>)value;
            context.Serialize(ref valueList);
        }

        #endregion

    }
}