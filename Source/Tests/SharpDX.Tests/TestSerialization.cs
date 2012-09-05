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
using System.Xml.Serialization;
using NUnit.Framework;
using SharpDX.Serialization;

namespace SharpDX.Tests
{
    [TestFixture]
    [Description("Tests SharpDX.Serialization")]
    public class TestSerialization
    {
        public class TestSimpleDynamicData : IDataSerializable
        {
            public object A;
            public object B;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.AllowIdentity = true;
                serializer.SerializeDynamic(ref A);
                serializer.SerializeDynamic(ref B);
                serializer.AllowIdentity = false;
            }
        }

        [Test]
        public void TestSimpleDynamic()
        {
            var stream = new MemoryStream();
            var rw = new BinarySerializer(stream, SerializerMode.Write);

            var arrayValue = new byte[] { 1, 2, 3, 4 };
            var dataArray = new TestSimpleDynamicData() { A = arrayValue, B = arrayValue };
            rw.Save(dataArray);

            stream.Position = 0;

            var dataArrayLoaded = rw.Load<TestSimpleDynamicData>();

            Assert.AreEqual(dataArrayLoaded.A, dataArrayLoaded.B);
        }

        [Test]
        public void TestString()
        {
            var stream = new MemoryStream();
            var rw = new BinarySerializer(stream, SerializerMode.Write, Text.Encoding.ASCII);

            var string1 = "0123";
            var string2 = "4567";
            var string3 = "ABCDEFGH";

            rw.Serialize(ref string1, 4);       // (4) Serialize only 4 bytes
            rw.Serialize(ref string2, true);    // (5) Serialize only 4 bytes + 1 null byte
            rw.Serialize(ref string3);          // (9) Serialize all bytes with length prefix (sizeof(byte) + 8)
            
            // Check Expected total size
            Assert.AreEqual(stream.Position, 4 + 5 + 9);

            stream.Position = 0;

            // Switch to read mode
            rw.Mode = SerializerMode.Read;

            string string11 = null;
            string string22 = null;
            string string33 = null;
            rw.Serialize(ref string11, 4);       // (4) Serialize only 4 bytes
            rw.Serialize(ref string22, true);    // (5) Serialize only 4 bytes + 1 null byte
            rw.Serialize(ref string33);          // (9) Serialize all bytes with length prefix (sizeof(byte) + 8)
            Assert.AreEqual(stream.Position, 4 + 5 + 9);

            Assert.AreEqual(string1, string11);
            Assert.AreEqual(string2, string22);
            Assert.AreEqual(string3, string33);

            // Check for an end of stream exception if we are trying to read a fixed length string with not enough bytes into the buffer
            Assert.Catch<EndOfStreamException>(() =>
                                                   {
                                                       rw.Mode = SerializerMode.Read;
                                                       stream.Position = 0;
                                                       rw.Serialize(ref string1, 50);
                                                   });

            string1 = null;
            Assert.Catch<ArgumentNullException>(() =>
                                                    {
                                                        rw.Mode = SerializerMode.Write;
                                                        stream.Position = 0;
                                                        rw.Serialize(ref string1, 4);
                                                    });

        }

        private static void RegisterDynamicForTestNonDataSerializer(BinarySerializer serializer)
        {
            serializer.RegisterDynamic<TestNonDataSerializer>("TNDS", (ref object value, BinarySerializer context) =>
            {
                var valueRef = (TestNonDataSerializer)value;

                if (context.Mode == SerializerMode.Write)
                {
                    context.Serialize(ref valueRef.A);
                }
                else
                {
                    valueRef = new TestNonDataSerializer();
                    context.Serialize(ref valueRef.A);
                    value = valueRef;
                }
            });
            
        }

        [Test]
        public void TestStatic()
        {
            var stream = new MemoryStream();
            var rw = new BinarySerializer(stream, SerializerMode.Write);
            RegisterDynamicForTestNonDataSerializer(rw);

            rw.Save(dataReference);
            rw.Flush();

            stream.Position = 0;
            var dataSerialized  = rw.Load<TestData>();

            var referenceText = SerializeXml(dataReference);
            var serializedText = SerializeXml(dataSerialized);

            Assert.AreEqual(referenceText, serializedText);
        }

        [Test]
        public void TestDynamic()
        {

            var stream = new MemoryStream();
            var rw = new BinarySerializer(stream, SerializerMode.Write);
            RegisterDynamicForTestNonDataSerializer(rw);

            var dynamicData = new TestDynamicData();
            dynamicData.CopyFrom(ref dataReference);

            rw.RegisterDynamic<TestStructData>("TSDT");
            rw.RegisterDynamicArray<TestStructData>("TSDA");
            rw.RegisterDynamicList<TestStructData>("TSDL");

            rw.RegisterDynamic<TestClassData>("TCDT");
            rw.RegisterDynamicArray<TestClassData>("TCDA");
            rw.RegisterDynamicList<TestClassData>("TCDL");

            rw.Save(dynamicData);
            rw.Flush();

            stream.Position = 0;
            var dataSerialized = rw.Load<TestDynamicData>();


            var dataSerializedForXml = new TestData();
            dataSerialized.CopyTo(ref dataSerializedForXml);

            var referenceText = SerializeXml(dataReference);
            var serializedText = SerializeXml(dataSerializedForXml);

            Assert.AreEqual(referenceText, serializedText);

            // Check Identity references
            Assert.AreEqual(dataSerialized.SIdentity1, dataSerialized.SIdentity11);
            Assert.AreEqual(dataSerialized.SIdentity2, dataSerialized.SIdentity22);
            Assert.AreEqual(dataSerialized.SIdentity3, dataSerialized.SIdentity33);
            Assert.AreEqual(dataSerialized.SIdentity4, dataSerialized.SIdentity44);
            Assert.AreEqual(dataSerialized.SIdentity5, dataSerialized.SIdentity55);
        }
        private string SerializeXml<T>(T value)
        {
            var referenceSerializer = new XmlSerializer(typeof(T));
            var referenceTextWriter = new StringWriter();
            referenceSerializer.Serialize(referenceTextWriter, value);
            return referenceTextWriter.ToString();
        }

        [Serializable]
        public class TestClassData : IDataSerializable
        {
            public byte[] A;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.BeginChunk("ABCD");
                serializer.Serialize(ref A);
                serializer.EndChunk();
            }
        }

        public enum MyEnum : byte
        {
            X = 0,
            Y = 1,
            Z = 2,
            W = 3
        }

        [Serializable]
        public struct TestStructData : IDataSerializable
        {
            public int A;

            public int B;

            public int C;

            public int D;

            public MyEnum E;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref A);
                serializer.Serialize(ref B);
                serializer.Serialize(ref C);
                serializer.Serialize(ref D);
                serializer.SerializeEnum(ref E);
            }
        }

        [Serializable]
        public struct TestNonDataSerializer
        {
            public int A;
        }

        [Serializable]
        public struct TestData : IDataSerializable
        {
            public sbyte A;
            public sbyte[] AArray;
            public List<sbyte> AList;

            public byte B;
            public byte[] BArray;
            public List<byte> BList;

            public int C;
            public int[] CArray;
            public List<int> CList;

            public uint D;
            public uint[] DArray;
            public List<uint> DList;

            public short E;
            public short[] EArray;
            public List<short> EList;

            public ushort F;
            public ushort[] FArray;
            public List<ushort> FList;

            public long G;
            public long[] GArray;
            public List<long> GList;

            public ulong H;
            public ulong[] HArray;
            public List<ulong> HList;

            public float I;
            public float[] IArray;
            public List<float> IList;

            public double J;
            public double[] JArray;
            public List<double> JList;

            public bool K;
            public bool[] KArray;
            public List<bool> KList;

            public DateTime L;
            public DateTime[] LArray;
            public List<DateTime> LList;

            public Guid M;
            public Guid[] MArray;
            public List<Guid> MList;

            public string N;
            public string[] NArray;
            public List<string> NList;

            public string NNull;
            public string[] NArrayNull;
            public List<string> NListNull;

            public TestClassData O;
            public TestClassData[] OArray;
            public List<TestClassData> OList;

            public TestClassData ONull;
            public TestClassData[] OArrayNull;
            public List<TestClassData> OListNull;

            public TestStructData Q;
            public TestStructData[] QArray;
            public List<TestStructData> QList;

            public TestNonDataSerializer R;

            public TestClassData SIdentity1;
            public string SIdentity2;
            public byte[] SIdentity3;
            public TestClassData[] SIdentity4;
            public int[] SIdentity5;

            public TestClassData SIdentity11;
            public string SIdentity22;
            public byte[] SIdentity33;
            public TestClassData[] SIdentity44;
            public int[] SIdentity55;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref A);
                serializer.Serialize(ref AArray, serializer.Serialize);
                serializer.Serialize(ref AList, serializer.Serialize);

                serializer.Serialize(ref B);
                serializer.Serialize(ref BArray);
                serializer.Serialize(ref BList, serializer.Serialize);

                serializer.Serialize(ref C);
                serializer.Serialize(ref CArray, serializer.Serialize);
                serializer.Serialize(ref CList, serializer.Serialize);

                serializer.Serialize(ref D);
                serializer.Serialize(ref DArray, serializer.Serialize);
                serializer.Serialize(ref DList, serializer.Serialize);

                serializer.Serialize(ref E);
                serializer.Serialize(ref EArray, serializer.Serialize);
                serializer.Serialize(ref EList, serializer.Serialize);

                serializer.Serialize(ref F);
                serializer.Serialize(ref FArray, serializer.Serialize);
                serializer.Serialize(ref FList, serializer.Serialize);

                serializer.Serialize(ref G);
                serializer.Serialize(ref GArray, serializer.Serialize);
                serializer.Serialize(ref GList, serializer.Serialize);

                serializer.Serialize(ref H);
                serializer.Serialize(ref HArray, serializer.Serialize);
                serializer.Serialize(ref HList, serializer.Serialize);

                serializer.Serialize(ref I);
                serializer.Serialize(ref IArray, serializer.Serialize);
                serializer.Serialize(ref IList, serializer.Serialize);

                serializer.Serialize(ref J);
                serializer.Serialize(ref JArray, serializer.Serialize);
                serializer.Serialize(ref JList, serializer.Serialize);

                serializer.Serialize(ref K);
                serializer.Serialize(ref KArray, serializer.Serialize);
                serializer.Serialize(ref KList, serializer.Serialize);

                serializer.Serialize(ref L);
                serializer.Serialize(ref LArray, serializer.Serialize);
                serializer.Serialize(ref LList, serializer.Serialize);

                serializer.Serialize(ref M);
                serializer.Serialize(ref MArray, serializer.Serialize);
                serializer.Serialize(ref MList, serializer.Serialize);

                serializer.Serialize(ref N);
                serializer.Serialize(ref NArray, serializer.Serialize);
                serializer.Serialize(ref NList, serializer.Serialize);

                serializer.Serialize(ref O);
                serializer.Serialize(ref OArray);
                serializer.Serialize(ref OList);

                // Allow null values here.
                serializer.AllowNull = true;
                serializer.Serialize(ref ONull);
                serializer.Serialize(ref OArrayNull);
                serializer.Serialize(ref OListNull);
                serializer.AllowNull = false;

                serializer.Serialize(ref Q);
                serializer.Serialize(ref QArray);
                serializer.Serialize(ref QList);

                serializer.SerializeDynamic(ref R);

                // Test for identity objects
                serializer.AllowIdentity = true;
                serializer.Serialize(ref SIdentity1);
                serializer.Serialize(ref SIdentity2);
                serializer.Serialize(ref SIdentity3);
                serializer.Serialize(ref SIdentity4);
                serializer.Serialize(ref SIdentity5, serializer.Serialize);

                serializer.Serialize(ref SIdentity11);
                serializer.Serialize(ref SIdentity22);
                serializer.Serialize(ref SIdentity33);
                serializer.Serialize(ref SIdentity44);
                serializer.Serialize(ref SIdentity55, serializer.Serialize);
                serializer.AllowIdentity = false;
            }
        }

        [Serializable]
        public struct TestDynamicData : IDataSerializable
        {
            public object A;
            public object AArray;
            public object AList;

            public object B;
            public object BArray;
            public object BList;

            public object C;
            public object CArray;
            public object CList;

            public object D;
            public object DArray;
            public object DList;

            public object E;
            public object EArray;
            public object EList;

            public object F;
            public object FArray;
            public object FList;

            public object G;
            public object GArray;
            public object GList;

            public object H;
            public object HArray;
            public object HList;

            public object I;
            public object IArray;
            public object IList;

            public object J;
            public object JArray;
            public object JList;

            public object K;
            public object KArray;
            public object KList;

            public object L;
            public object LArray;
            public object LList;

            public object M;
            public object MArray;
            public object MList;

            public object N;
            public object NArray;
            public object NList;

            public object NNull;
            public object NArrayNull;
            public object NListNull;

            public object O;
            public object OArray;
            public object OList;

            public object ONull;
            public object OArrayNull;
            public object OListNull;

            public object Q;
            public object QArray;
            public object QList;

            public object R;

            public object SIdentity1;
            public object SIdentity2;
            public object SIdentity3;
            public object SIdentity4;
            public object SIdentity5;

            public object SIdentity11;
            public object SIdentity22;
            public object SIdentity33;
            public object SIdentity44;
            public object SIdentity55;
            
            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.AllowNull = true;
                serializer.SerializeDynamic(ref A);
                serializer.SerializeDynamic(ref AArray);
                serializer.SerializeDynamic(ref AList);

                serializer.SerializeDynamic(ref B);
                serializer.SerializeDynamic(ref BArray);
                serializer.SerializeDynamic(ref BList);

                serializer.SerializeDynamic(ref C);
                serializer.SerializeDynamic(ref CArray);
                serializer.SerializeDynamic(ref CList);

                serializer.SerializeDynamic(ref D);
                serializer.SerializeDynamic(ref DArray);
                serializer.SerializeDynamic(ref DList);

                serializer.SerializeDynamic(ref E);
                serializer.SerializeDynamic(ref EArray);
                serializer.SerializeDynamic(ref EList);

                serializer.SerializeDynamic(ref F);
                serializer.SerializeDynamic(ref FArray);
                serializer.SerializeDynamic(ref FList);

                serializer.SerializeDynamic(ref G);
                serializer.SerializeDynamic(ref GArray);
                serializer.SerializeDynamic(ref GList);

                serializer.SerializeDynamic(ref H);
                serializer.SerializeDynamic(ref HArray);
                serializer.SerializeDynamic(ref HList);

                serializer.SerializeDynamic(ref I);
                serializer.SerializeDynamic(ref IArray);
                serializer.SerializeDynamic(ref IList);

                serializer.SerializeDynamic(ref J);
                serializer.SerializeDynamic(ref JArray);
                serializer.SerializeDynamic(ref JList);

                serializer.SerializeDynamic(ref K);
                serializer.SerializeDynamic(ref KArray);
                serializer.SerializeDynamic(ref KList);

                serializer.SerializeDynamic(ref L);
                serializer.SerializeDynamic(ref LArray);
                serializer.SerializeDynamic(ref LList);

                serializer.SerializeDynamic(ref M);
                serializer.SerializeDynamic(ref MArray);
                serializer.SerializeDynamic(ref MList);

                serializer.SerializeDynamic(ref N);
                serializer.SerializeDynamic(ref NArray);
                serializer.SerializeDynamic(ref NList);

                serializer.SerializeDynamic(ref O);
                serializer.SerializeDynamic(ref OArray);
                serializer.SerializeDynamic(ref OList);

                serializer.SerializeDynamic(ref ONull);
                serializer.SerializeDynamic(ref OArrayNull);
                serializer.SerializeDynamic(ref OListNull);

                serializer.SerializeDynamic(ref Q);
                serializer.SerializeDynamic(ref QArray);
                serializer.SerializeDynamic(ref QList);

                serializer.SerializeDynamic(ref R);

                // Test for identity objects
                serializer.AllowIdentity = true;
                serializer.SerializeDynamic(ref SIdentity1);
                serializer.SerializeDynamic(ref SIdentity2);
                serializer.SerializeDynamic(ref SIdentity3);
                serializer.SerializeDynamic(ref SIdentity4);
                serializer.SerializeDynamic(ref SIdentity5);

                serializer.SerializeDynamic(ref SIdentity11);
                serializer.SerializeDynamic(ref SIdentity22);
                serializer.SerializeDynamic(ref SIdentity33);
                serializer.SerializeDynamic(ref SIdentity44);
                serializer.SerializeDynamic(ref SIdentity55);
                serializer.AllowIdentity = false;

                serializer.AllowNull = false;
            }

            public void CopyTo(ref TestData dest)
            {
                dest.A = (sbyte)this.A;
                dest.AArray = (sbyte[])this.AArray;
                dest.AList = (List<sbyte>)this.AList;
                dest.B = (byte)this.B;
                dest.BArray = (byte[])this.BArray;
                dest.BList = (List<byte>)this.BList;
                dest.C = (int)this.C;
                dest.CArray = (int[])this.CArray;
                dest.CList = (List<int>)this.CList;
                dest.D = (uint)this.D;
                dest.DArray = (uint[])this.DArray;
                dest.DList = (List<uint>)this.DList;
                dest.E = (short)this.E;
                dest.EArray = (short[])this.EArray;
                dest.EList = (List<short>)this.EList;
                dest.F = (ushort)this.F;
                dest.FArray = (ushort[])this.FArray;
                dest.FList = (List<ushort>)this.FList;
                dest.G = (long)this.G;
                dest.GArray = (long[])this.GArray;
                dest.GList = (List<long>)this.GList;
                dest.H = (ulong)this.H;
                dest.HArray = (ulong[])this.HArray;
                dest.HList = (List<ulong>)this.HList;
                dest.I = (float)this.I;
                dest.IArray = (float[])this.IArray;
                dest.IList = (List<float>)this.IList;
                dest.J = (double)this.J;
                dest.JArray = (double[])this.JArray;
                dest.JList = (List<double>)this.JList;
                dest.K = (bool)this.K;
                dest.KArray = (bool[])this.KArray;
                dest.KList = (List<bool>)this.KList;
                dest.L = (DateTime)this.L;
                dest.LArray = (DateTime[])this.LArray;
                dest.LList = (List<DateTime>)this.LList;
                dest.M = (Guid)this.M;
                dest.MArray = (Guid[])this.MArray;
                dest.MList = (List<Guid>)this.MList;
                dest.N = (string)this.N;
                dest.NArray = (string[])this.NArray;
                dest.NList = (List<string>)this.NList;
                dest.NNull = (string)this.NNull;
                dest.NArrayNull = (string[])this.NArrayNull;
                dest.NListNull = (List<string>)this.NListNull;
                dest.O = (TestClassData)this.O;
                dest.OArray = (TestClassData[])this.OArray;
                dest.OList = (List<TestClassData>)this.OList;
                dest.ONull = (TestClassData)this.ONull;
                dest.OArrayNull = (TestClassData[])this.OArrayNull;
                dest.OListNull = (List<TestClassData>)this.OListNull;
                dest.Q = (TestStructData)this.Q;
                dest.QArray = (TestStructData[])this.QArray;
                dest.QList = (List<TestStructData>)this.QList;

                dest.R = (TestNonDataSerializer)this.R;

                dest.SIdentity1 = (TestClassData)this.SIdentity1;
                dest.SIdentity2 = (string)this.SIdentity2;
                dest.SIdentity3 = (byte[])this.SIdentity3;
                dest.SIdentity4 = (TestClassData[])this.SIdentity4;
                dest.SIdentity5 = (int[])this.SIdentity5;

                dest.SIdentity11 = (TestClassData)this.SIdentity11;
                dest.SIdentity22 = (string)this.SIdentity22;
                dest.SIdentity33 = (byte[])this.SIdentity33;
                dest.SIdentity44 = (TestClassData[])this.SIdentity44;
                dest.SIdentity55 = (int[])this.SIdentity55;
            }

            public void CopyFrom(ref TestData dest)
            {
                this.A = (sbyte)dest.A;
                this.AArray = (sbyte[])dest.AArray;
                this.AList = (List<sbyte>)dest.AList;
                this.B = (byte)dest.B;
                this.BArray = (byte[])dest.BArray;
                this.BList = (List<byte>)dest.BList;
                this.C = (int)dest.C;
                this.CArray = (int[])dest.CArray;
                this.CList = (List<int>)dest.CList;
                this.D = (uint)dest.D;
                this.DArray = (uint[])dest.DArray;
                this.DList = (List<uint>)dest.DList;
                this.E = (short)dest.E;
                this.EArray = (short[])dest.EArray;
                this.EList = (List<short>)dest.EList;
                this.F = (ushort)dest.F;
                this.FArray = (ushort[])dest.FArray;
                this.FList = (List<ushort>)dest.FList;
                this.G = (long)dest.G;
                this.GArray = (long[])dest.GArray;
                this.GList = (List<long>)dest.GList;
                this.H = (ulong)dest.H;
                this.HArray = (ulong[])dest.HArray;
                this.HList = (List<ulong>)dest.HList;
                this.I = (float)dest.I;
                this.IArray = (float[])dest.IArray;
                this.IList = (List<float>)dest.IList;
                this.J = (double)dest.J;
                this.JArray = (double[])dest.JArray;
                this.JList = (List<double>)dest.JList;
                this.K = (bool)dest.K;
                this.KArray = (bool[])dest.KArray;
                this.KList = (List<bool>)dest.KList;
                this.L = (DateTime)dest.L;
                this.LArray = (DateTime[])dest.LArray;
                this.LList = (List<DateTime>)dest.LList;
                this.M = (Guid)dest.M;
                this.MArray = (Guid[])dest.MArray;
                this.MList = (List<Guid>)dest.MList;
                this.N = (string)dest.N;
                this.NArray = (string[])dest.NArray;
                this.NList = (List<string>)dest.NList;
                this.NNull = (string)dest.NNull;
                this.NArrayNull = (string[])dest.NArrayNull;
                this.NListNull = (List<string>)dest.NListNull;
                this.O = (TestClassData)dest.O;
                this.OArray = (TestClassData[])dest.OArray;
                this.OList = (List<TestClassData>)dest.OList;
                this.ONull = (TestClassData)dest.ONull;
                this.OArrayNull = (TestClassData[])dest.OArrayNull;
                this.OListNull = (List<TestClassData>)dest.OListNull;
                this.Q = (TestStructData)dest.Q;
                this.QArray = (TestStructData[])dest.QArray;
                this.QList = (List<TestStructData>)dest.QList;

                this.R = (TestNonDataSerializer)dest.R;

                this.SIdentity1 = (TestClassData)dest.SIdentity1;
                this.SIdentity2 = (string) dest.SIdentity2;
                this.SIdentity3 = (byte[]) dest.SIdentity3;
                this.SIdentity4 = (TestClassData[]) dest.SIdentity4;
                this.SIdentity5 = (int[]) dest.SIdentity5;

                this.SIdentity11 = (TestClassData)dest.SIdentity11;
                this.SIdentity22 = (string)dest.SIdentity22;
                this.SIdentity33 = (byte[])dest.SIdentity33;
                this.SIdentity44 = (TestClassData[])dest.SIdentity44;
                this.SIdentity55 = (int[])dest.SIdentity55;
            }
        }

        private static readonly TestClassData SIdentity1 = new TestClassData();
        private static readonly string SIdentity2 = new string('a', 'b');
        private static readonly byte[] SIdentity3 = new byte[] {1, 2, 3, 4};
        private static readonly TestClassData[] SIdentity4 = new[] { SIdentity1, SIdentity1, SIdentity1, SIdentity1 };
        private static readonly int[] SIdentity5 = new[] {1, 2, 3, 4};

        private TestData dataReference = new TestData()
        {
            A = (sbyte)1,
            AArray = new sbyte[] { 1, 2, 3, 4 },
            AList = new List<sbyte>() { 5, 6, 7, 8 },

            B = (byte)1,
            BArray = new byte[] { 1, 2, 3, 4 },
            BList = new List<byte> { 5, 6, 7, 8 },

            C = (int)1,
            CArray = new int[] { 1, 2, 3, 4 },
            CList = new List<int> { 5, 6, 7, 8 },

            D = (uint)1,
            DArray = new uint[] { 1, 2, 3, 4 },
            DList = new List<uint> { 5, 6, 7, 8 },

            E = (short)1,
            EArray = new short[] { 1, 2, 3, 4 },
            EList = new List<short> { 5, 6, 7, 8 },

            F = (ushort)1,
            FArray = new ushort[] { 1, 2, 3, 4 },
            FList = new List<ushort> { 5, 6, 7, 8 },

            G = (long)1,
            GArray = new long[] { 1, 2, 3, 4 },
            GList = new List<long> { 5, 6, 7, 8 },

            H = (ulong)1,
            HArray = new ulong[] { 1, 2, 3, 4 },
            HList = new List<ulong> { 5, 6, 7, 8 },

            I = (float)1,
            IArray = new float[] { 1, 2, 3, 4 },
            IList = new List<float> { 5, 6, 7, 8 },

            J = (double)1,
            JArray = new double[] { 1, 2, 3, 4 },
            JList = new List<double> { 5, 6, 7, 8 },

            K = (bool)true,
            KArray = new bool[] { true, false, true, true },
            KList = new List<bool> { true, false, true, true },

            L = new DateTime(1),
            LArray = new DateTime[] { new DateTime(1), new DateTime(2), new DateTime(3), new DateTime(4) },
            LList = new List<DateTime> { new DateTime(5), new DateTime(6), new DateTime(7), new DateTime(8) },

            M = new Guid(),
            MArray = new Guid[] { new Guid(), new Guid() },
            MList = new List<Guid> { new Guid(), new Guid() },

            N = "a",
            NArray = new string[] { "a", "b", "c", "d" },
            NList = new List<string> { "e", "f", "g", "h" },

            NNull = (string)null,
            NArrayNull = null,
            NListNull = null,

            O = new TestClassData() { A = new byte[] { 1, 2, 3, 4 } },
            OArray = new TestClassData[] { new TestClassData() { A = new byte[] { 1, 2, 3, 4 } }, new TestClassData() { A = new byte[] { 5, 6, 7, 8 } }, },
            OList = new List<TestClassData> { new TestClassData() { A = new byte[] { 1, 2, 3, 4 } }, new TestClassData() { A = new byte[] { 5, 6, 7, 8 } }, },

            ONull = null,
            OArrayNull = null,
            OListNull = null,

            Q = new TestStructData() { A = 1, B = 2, C = 3, D = 4, E = MyEnum.X },
            QArray = new TestStructData[] { new TestStructData() { A = 1, B = 2, C = 3, D = 4, E = MyEnum.X }, new TestStructData() { A = 5, B = 6, C = 7, D = 8, E = MyEnum.X }, },
            QList = new List<TestStructData> { new TestStructData() { A = 1, B = 2, C = 3, D = 4, E = MyEnum.X }, new TestStructData() { A = 5, B = 6, C = 7, D = 8, E = MyEnum.X }, },

            R = new TestNonDataSerializer() { A = 1 },

            SIdentity1 = SIdentity1,
            SIdentity2 = SIdentity2,
            SIdentity3 = SIdentity3,
            SIdentity4 = SIdentity4,
            SIdentity5 = SIdentity5,

            SIdentity11 = SIdentity1,
            SIdentity22 = SIdentity2,
            SIdentity33 = SIdentity3,
            SIdentity44 = SIdentity4,
            SIdentity55 = SIdentity5,
        };
    }
}