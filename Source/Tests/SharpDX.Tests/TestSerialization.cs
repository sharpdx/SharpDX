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
        [Test]
        public void TestStatic()
        {
            var stream = new MemoryStream();
            var rw = new BinarySerializer(stream, SerializerMode.Write);

            rw.RegisterDynamic<TestNonDataSerializer>("TNDS", (ref object value, BinarySerializer context) =>
                                                                  {
                                                                      var valueRef = (TestNonDataSerializer) value;

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

            var dynamicData = new TestDynamicData();
            dynamicData.CopyFrom(dataReference);

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
            dataSerialized.CopyTo(dataSerializedForXml);

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

        public class TestSimpleDynamicData : IDataSerializer
        {
            public object A;
            public object B;

            void IDataSerializer.Serialize(BinarySerializer binarySerializer)
            {
                binarySerializer.EnableIdentityReference(true);
                binarySerializer.SerializeDynamic(ref A);
                binarySerializer.SerializeDynamic(ref B);
                binarySerializer.EnableIdentityReference(false);
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


        private string SerializeXml(TestData value)
        {
            var referenceSerializer = new XmlSerializer(typeof(TestData));
            var referenceTextWriter = new StringWriter();
            referenceSerializer.Serialize(referenceTextWriter, value);
            return value.ToString();
        }

        [Serializable]
        public class TestClassData : IDataSerializer
        {
            public byte[] A;

            void IDataSerializer.Serialize(BinarySerializer binarySerializer)
            {
                binarySerializer.BeginChunk("ABCD");
                binarySerializer.Serialize(ref A);
                binarySerializer.EndChunk();
            }
        }

        [Serializable]
        public struct TestStructData : IDataSerializer
        {
            public int A;

            public int B;

            public int C;

            public int D;

            void IDataSerializer.Serialize(BinarySerializer binarySerializer)
            {
                binarySerializer.Serialize(ref A);
                binarySerializer.Serialize(ref B);
                binarySerializer.Serialize(ref C);
                binarySerializer.Serialize(ref D);
            }
        }

        [Serializable]
        public struct TestNonDataSerializer
        {
            public int A;
        }

        [Serializable]
        public struct TestData : IDataSerializer
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

            void IDataSerializer.Serialize(BinarySerializer binarySerializer)
            {
                binarySerializer.Serialize(ref A);
                binarySerializer.Serialize(ref AArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref AList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref B);
                binarySerializer.Serialize(ref BArray);
                binarySerializer.Serialize(ref BList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref C);
                binarySerializer.Serialize(ref CArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref CList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref D);
                binarySerializer.Serialize(ref DArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref DList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref E);
                binarySerializer.Serialize(ref EArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref EList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref F);
                binarySerializer.Serialize(ref FArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref FList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref G);
                binarySerializer.Serialize(ref GArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref GList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref H);
                binarySerializer.Serialize(ref HArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref HList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref I);
                binarySerializer.Serialize(ref IArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref IList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref J);
                binarySerializer.Serialize(ref JArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref JList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref K);
                binarySerializer.Serialize(ref KArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref KList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref L);
                binarySerializer.Serialize(ref LArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref LList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref M);
                binarySerializer.Serialize(ref MArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref MList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref N);
                binarySerializer.Serialize(ref NArray, binarySerializer.Serialize);
                binarySerializer.Serialize(ref NList, binarySerializer.Serialize);

                binarySerializer.Serialize(ref O);
                binarySerializer.Serialize(ref OArray);
                binarySerializer.Serialize(ref OList);

                // Allow null values here.
                binarySerializer.EnableNullReference(true);
                binarySerializer.Serialize(ref ONull);
                binarySerializer.Serialize(ref OArrayNull);
                binarySerializer.Serialize(ref OListNull);
                binarySerializer.EnableNullReference(false);

                binarySerializer.Serialize(ref Q);
                binarySerializer.Serialize(ref QArray);
                binarySerializer.Serialize(ref QList);

                binarySerializer.SerializeDynamic(ref R);

                // Test for identity objects
                binarySerializer.EnableIdentityReference(true);
                binarySerializer.Serialize(ref SIdentity1);
                binarySerializer.Serialize(ref SIdentity2);
                binarySerializer.Serialize(ref SIdentity3);
                binarySerializer.Serialize(ref SIdentity4);
                binarySerializer.Serialize(ref SIdentity5, binarySerializer.Serialize);

                binarySerializer.Serialize(ref SIdentity11);
                binarySerializer.Serialize(ref SIdentity22);
                binarySerializer.Serialize(ref SIdentity33);
                binarySerializer.Serialize(ref SIdentity44);
                binarySerializer.Serialize(ref SIdentity55, binarySerializer.Serialize);
                binarySerializer.EnableIdentityReference(false);
            }
        }

        [Serializable]
        public struct TestDynamicData : IDataSerializer
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
            
            void IDataSerializer.Serialize(BinarySerializer binarySerializer)
            {
                binarySerializer.EnableNullReference(true);
                binarySerializer.SerializeDynamic(ref A);
                binarySerializer.SerializeDynamic(ref AArray);
                binarySerializer.SerializeDynamic(ref AList);

                binarySerializer.SerializeDynamic(ref B);
                binarySerializer.SerializeDynamic(ref BArray);
                binarySerializer.SerializeDynamic(ref BList);

                binarySerializer.SerializeDynamic(ref C);
                binarySerializer.SerializeDynamic(ref CArray);
                binarySerializer.SerializeDynamic(ref CList);

                binarySerializer.SerializeDynamic(ref D);
                binarySerializer.SerializeDynamic(ref DArray);
                binarySerializer.SerializeDynamic(ref DList);

                binarySerializer.SerializeDynamic(ref E);
                binarySerializer.SerializeDynamic(ref EArray);
                binarySerializer.SerializeDynamic(ref EList);

                binarySerializer.SerializeDynamic(ref F);
                binarySerializer.SerializeDynamic(ref FArray);
                binarySerializer.SerializeDynamic(ref FList);

                binarySerializer.SerializeDynamic(ref G);
                binarySerializer.SerializeDynamic(ref GArray);
                binarySerializer.SerializeDynamic(ref GList);

                binarySerializer.SerializeDynamic(ref H);
                binarySerializer.SerializeDynamic(ref HArray);
                binarySerializer.SerializeDynamic(ref HList);

                binarySerializer.SerializeDynamic(ref I);
                binarySerializer.SerializeDynamic(ref IArray);
                binarySerializer.SerializeDynamic(ref IList);

                binarySerializer.SerializeDynamic(ref J);
                binarySerializer.SerializeDynamic(ref JArray);
                binarySerializer.SerializeDynamic(ref JList);

                binarySerializer.SerializeDynamic(ref K);
                binarySerializer.SerializeDynamic(ref KArray);
                binarySerializer.SerializeDynamic(ref KList);

                binarySerializer.SerializeDynamic(ref L);
                binarySerializer.SerializeDynamic(ref LArray);
                binarySerializer.SerializeDynamic(ref LList);

                binarySerializer.SerializeDynamic(ref M);
                binarySerializer.SerializeDynamic(ref MArray);
                binarySerializer.SerializeDynamic(ref MList);

                binarySerializer.SerializeDynamic(ref N);
                binarySerializer.SerializeDynamic(ref NArray);
                binarySerializer.SerializeDynamic(ref NList);

                binarySerializer.SerializeDynamic(ref O);
                binarySerializer.SerializeDynamic(ref OArray);
                binarySerializer.SerializeDynamic(ref OList);

                binarySerializer.SerializeDynamic(ref ONull);
                binarySerializer.SerializeDynamic(ref OArrayNull);
                binarySerializer.SerializeDynamic(ref OListNull);

                binarySerializer.SerializeDynamic(ref Q);
                binarySerializer.SerializeDynamic(ref QArray);
                binarySerializer.SerializeDynamic(ref QList);

                // Test for identity objects
                binarySerializer.EnableIdentityReference(true);
                binarySerializer.SerializeDynamic(ref SIdentity1);
                binarySerializer.SerializeDynamic(ref SIdentity2);
                binarySerializer.SerializeDynamic(ref SIdentity3);
                binarySerializer.SerializeDynamic(ref SIdentity4);
                binarySerializer.SerializeDynamic(ref SIdentity5);

                binarySerializer.SerializeDynamic(ref SIdentity11);
                binarySerializer.SerializeDynamic(ref SIdentity22);
                binarySerializer.SerializeDynamic(ref SIdentity33);
                binarySerializer.SerializeDynamic(ref SIdentity44);
                binarySerializer.SerializeDynamic(ref SIdentity55);
                binarySerializer.EnableIdentityReference(false);

                binarySerializer.EnableNullReference(false);
            }

            public void CopyTo(TestData dest)
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

            public void CopyFrom(TestData dest)
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

                this.SIdentity1 = (TestClassData) dest.SIdentity1;
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

            Q = new TestStructData() { A = 1, B = 2, C = 3, D = 4 },
            QArray = new TestStructData[] { new TestStructData() { A = 1, B = 2, C = 3, D = 4 }, new TestStructData() { A = 5, B = 6, C = 7, D = 8 }, },
            QList = new List<TestStructData> { new TestStructData() { A = 1, B = 2, C = 3, D = 4 }, new TestStructData() { A = 5, B = 6, C = 7, D = 8 }, },

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