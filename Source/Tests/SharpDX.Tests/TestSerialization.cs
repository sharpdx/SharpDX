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
        [Serializable]
        public class TestClassData : IDataSerializer
        {
            public byte[] A;

            void IDataSerializer.Serialize(SerializerContext context)
            {
                context.BeginChunk("ABCD");
                context.Serialize(ref A);
                context.EndChunk();
            }
        }

        [Serializable]
        public struct TestStructData : IDataSerializer
        {
            public int A;

            public int B;

            public int C;

            public int D;

            void IDataSerializer.Serialize(SerializerContext context)
            {
                context.Serialize(ref A);
                context.Serialize(ref B);
                context.Serialize(ref C);
                context.Serialize(ref D);
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

            void IDataSerializer.Serialize(SerializerContext context)
            {
                context.Serialize(ref A);
                context.Serialize(ref AArray, context.Serialize);
                context.Serialize(ref AList, context.Serialize);

                context.Serialize(ref B);
                context.Serialize(ref BArray);
                context.Serialize(ref BList, context.Serialize);

                context.Serialize(ref C);
                context.Serialize(ref CArray, context.Serialize);
                context.Serialize(ref CList, context.Serialize);

                context.Serialize(ref D);
                context.Serialize(ref DArray, context.Serialize);
                context.Serialize(ref DList, context.Serialize);

                context.Serialize(ref E);
                context.Serialize(ref EArray, context.Serialize);
                context.Serialize(ref EList, context.Serialize);
               
                context.Serialize(ref F);
                context.Serialize(ref FArray, context.Serialize);
                context.Serialize(ref FList, context.Serialize);
               
                context.Serialize(ref G);
                context.Serialize(ref GArray, context.Serialize);
                context.Serialize(ref GList, context.Serialize);
               
                context.Serialize(ref H);
                context.Serialize(ref HArray, context.Serialize);
                context.Serialize(ref HList, context.Serialize);

                context.Serialize(ref I);
                context.Serialize(ref IArray, context.Serialize);
                context.Serialize(ref IList, context.Serialize);

                context.Serialize(ref J);
                context.Serialize(ref JArray, context.Serialize);
                context.Serialize(ref JList, context.Serialize);

                context.Serialize(ref K);
                context.Serialize(ref KArray, context.Serialize);
                context.Serialize(ref KList, context.Serialize);

                context.Serialize(ref L);
                context.Serialize(ref LArray, context.Serialize);
                context.Serialize(ref LList, context.Serialize);

                context.Serialize(ref M);
                context.Serialize(ref MArray, context.Serialize);
                context.Serialize(ref MList, context.Serialize);

                context.Serialize(ref N);
                context.Serialize(ref NArray);
                context.Serialize(ref NList, context.Serialize);

                context.Serialize(ref O);
                context.Serialize(ref OArray);
                context.Serialize(ref OList);

                context.Serialize(ref ONull);
                context.Serialize(ref OArrayNull);
                context.Serialize(ref OListNull);

                context.Serialize(ref Q);
                context.Serialize(ref QArray);
                context.Serialize(ref QList);

                context.SerializeDynamic(ref R);
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

            void IDataSerializer.Serialize(SerializerContext context)
            {
                context.SerializeDynamic(ref A);
                context.SerializeDynamic(ref AArray);
                context.SerializeDynamic(ref AList);

                context.SerializeDynamic(ref B);
                context.SerializeDynamic(ref BArray);
                context.SerializeDynamic(ref BList);

                context.SerializeDynamic(ref C);
                context.SerializeDynamic(ref CArray);
                context.SerializeDynamic(ref CList);

                context.SerializeDynamic(ref D);
                context.SerializeDynamic(ref DArray);
                context.SerializeDynamic(ref DList);

                context.SerializeDynamic(ref E);
                context.SerializeDynamic(ref EArray);
                context.SerializeDynamic(ref EList);

                context.SerializeDynamic(ref F);
                context.SerializeDynamic(ref FArray);
                context.SerializeDynamic(ref FList);

                context.SerializeDynamic(ref G);
                context.SerializeDynamic(ref GArray);
                context.SerializeDynamic(ref GList);

                context.SerializeDynamic(ref H);
                context.SerializeDynamic(ref HArray);
                context.SerializeDynamic(ref HList);

                context.SerializeDynamic(ref I);
                context.SerializeDynamic(ref IArray);
                context.SerializeDynamic(ref IList);

                context.SerializeDynamic(ref J);
                context.SerializeDynamic(ref JArray);
                context.SerializeDynamic(ref JList);

                context.SerializeDynamic(ref K);
                context.SerializeDynamic(ref KArray);
                context.SerializeDynamic(ref KList);

                context.SerializeDynamic(ref L);
                context.SerializeDynamic(ref LArray);
                context.SerializeDynamic(ref LList);

                context.SerializeDynamic(ref M);
                context.SerializeDynamic(ref MArray);
                context.SerializeDynamic(ref MList);

                context.SerializeDynamic(ref N);
                context.SerializeDynamic(ref NArray);
                context.SerializeDynamic(ref NList);

                context.SerializeDynamic(ref O);
                context.SerializeDynamic(ref OArray);
                context.SerializeDynamic(ref OList);

                context.SerializeDynamic(ref ONull);
                context.SerializeDynamic(ref OArrayNull);
                context.SerializeDynamic(ref OListNull);

                context.SerializeDynamic(ref Q);
                context.SerializeDynamic(ref QArray);
                context.SerializeDynamic(ref QList);
            }
        }



        [Test]
        public void TestBasicTypes()
        {

            var stream = new MemoryStream();
            var rw = new SerializerContext(stream, SerializerMode.Write);

            var dataReference = new TestData()
                           {
                               A = (sbyte) 1,
                               AArray = new sbyte[] {1, 2, 3, 4},
                               AList = new List<sbyte>() {5, 6, 7, 8},

                               B = (byte) 1,
                               BArray = new byte[] {1, 2, 3, 4},
                               BList = new List<byte> {5, 6, 7, 8},

                               C = (int) 1,
                               CArray = new int[] {1, 2, 3, 4},
                               CList = new List<int> {5, 6, 7, 8},

                               D = (uint) 1,
                               DArray = new uint[] {1, 2, 3, 4},
                               DList = new List<uint> {5, 6, 7, 8},

                               E = (short) 1,
                               EArray = new short[] {1, 2, 3, 4},
                               EList = new List<short> {5, 6, 7, 8},

                               F = (ushort) 1,
                               FArray = new ushort[] {1, 2, 3, 4},
                               FList = new List<ushort> {5, 6, 7, 8},

                               G = (long) 1,
                               GArray = new long[] {1, 2, 3, 4},
                               GList = new List<long> {5, 6, 7, 8},

                               H = (ulong) 1,
                               HArray = new ulong[] {1, 2, 3, 4},
                               HList = new List<ulong> {5, 6, 7, 8},

                               I = (float) 1,
                               IArray = new float[] {1, 2, 3, 4},
                               IList = new List<float> {5, 6, 7, 8},

                               J = (double) 1,
                               JArray = new double[] {1, 2, 3, 4},
                               JList = new List<double> {5, 6, 7, 8},

                               K = (bool) true,
                               KArray = new bool[] {true, false, true, true},
                               KList = new List<bool> {true, false, true, true},

                               L = new DateTime(1),
                               LArray = new DateTime[] {new DateTime(1), new DateTime(2), new DateTime(3), new DateTime(4)},
                               LList = new List<DateTime> {new DateTime(5), new DateTime(6), new DateTime(7), new DateTime(8)},

                               M = new Guid(),
                               MArray = new Guid[] {new Guid(), new Guid()},
                               MList = new List<Guid> {new Guid(), new Guid()},

                               N = "a",
                               NArray = new string[] {"a", "b", "c", "d"},
                               NList = new List<string> {"e", "f", "g", "h"},

                               NNull = (string) null,
                               NArrayNull = null,
                               NListNull = null,

                               O = new TestClassData() {A = new byte[] {1, 2, 3, 4}},
                               OArray = new TestClassData[] {new TestClassData() {A = new byte[] {1, 2, 3, 4}}, new TestClassData() {A = new byte[] {5, 6, 7, 8}},},
                               OList = new List<TestClassData> {new TestClassData() {A = new byte[] {1, 2, 3, 4}}, new TestClassData() {A = new byte[] {5, 6, 7, 8}},},

                               ONull = null,
                               OArrayNull = null,
                               OListNull = null,

                               Q = new TestStructData() {A = 1, B = 2, C = 3, D = 4},
                               QArray = new TestStructData[] {new TestStructData() {A = 1, B = 2, C = 3, D = 4}, new TestStructData() {A = 5, B = 6, C = 7, D = 8},},
                               QList = new List<TestStructData> {new TestStructData() {A = 1, B = 2, C = 3, D = 4}, new TestStructData() {A = 5, B = 6, C = 7, D = 8},},

                               R = new TestNonDataSerializer() { A = 1 },
                           };

            rw.RegisterDynamic<TestNonDataSerializer>("TNDS", (ref object value, SerializerContext context) =>
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

            var referenceSerializer = new XmlSerializer(typeof (TestData));
            var referenceTextWriter = new StringWriter();
            referenceSerializer.Serialize(referenceTextWriter, dataReference);

            var serializedSerializer = new XmlSerializer(typeof(TestData));
            var serializedTextWriter = new StringWriter();
            serializedSerializer.Serialize(serializedTextWriter, dataSerialized);

            var referenceText = referenceTextWriter.ToString();
            var serializedText = serializedTextWriter.ToString();

            Assert.AreEqual(referenceText, serializedText);
        }

        [Test]
        public void TestDynamic()
        {

            var stream = new MemoryStream();
            var rw = new SerializerContext(stream, SerializerMode.Write);

            var dataReference = new TestDynamicData()
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
            };

            rw.RegisterDynamic<TestStructData>("TSDT");
            rw.RegisterDynamicArray<TestStructData>("TSDA");
            rw.RegisterDynamicList<TestStructData>("TSDL");

            rw.RegisterDynamic<TestClassData>("TCDT");
            rw.RegisterDynamicArray<TestClassData>("TCDA");
            rw.RegisterDynamicList<TestClassData>("TCDL");

            rw.Save(dataReference);
            rw.Flush();

            stream.Position = 0;
            var dataSerialized = rw.Load<TestDynamicData>();

            bool isEqual = dataReference.A == dataSerialized.A;
            Console.WriteLine(isEqual);
        }



    }
}