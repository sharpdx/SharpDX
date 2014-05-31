// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Serialization
{
    public partial class BinarySerializer
    {
        partial void RegisterPartialDynamics()
        {
            RegisterDynamic<Color>("TKC1", SerializeColor);
            RegisterDynamic<Color3>("TKC3", SerializeColor3);
            RegisterDynamic<Color4>("TKC4", SerializeColor4);
            RegisterDynamic<Vector3>("TKV3", SerializeVector3);
            RegisterDynamic<Vector2>("TKV2", SerializeVector2);
            RegisterDynamic<Vector4>("TKV4", SerializeVector4);
        }

        private static void SerializeColor(ref Color value, BinarySerializer serializer) { serializer.Serialize(ref value); }
        private static void SerializeColor3(ref Color3 value, BinarySerializer serializer) { serializer.Serialize(ref value); }
        private static void SerializeColor4(ref Color4 value, BinarySerializer serializer) { serializer.Serialize(ref value); }
        private static void SerializeVector2(ref Vector2 value, BinarySerializer serializer) { serializer.Serialize(ref value); }
        private static void SerializeVector3(ref Vector3 value, BinarySerializer serializer) { serializer.Serialize(ref value); }
        private static void SerializeVector4(ref Vector4 value, BinarySerializer serializer) { serializer.Serialize(ref value); }

        public void Serialize(ref AngleSingle value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.Radians);
            }
            else
            {
                value.Radians = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Bool4 value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.iX);
                Writer.Write(value.iY);
                Writer.Write(value.iZ);
                Writer.Write(value.iW);
            }
            else
            {
                value.iX = Reader.ReadInt32();
                value.iY = Reader.ReadInt32();
                value.iZ = Reader.ReadInt32();
                value.iW = Reader.ReadInt32();
            }
        }

        public void Serialize(ref BoundingBox value)
        {
            Serialize(ref value.Minimum);
            Serialize(ref value.Maximum);
        }

        public void Serialize(ref BoundingFrustum value)
        {
            Matrix matrix = value.Matrix;
            Serialize(ref matrix);
            if (Mode == SerializerMode.Read)
                value.Matrix = matrix;

            //Including planes for compatibility purposes, they are always inferred from the matrix, so there's no reason to serialize them.:
            Plane dummyPlane = new Plane();
            Serialize(ref dummyPlane);//Near
            Serialize(ref dummyPlane);//Far
            Serialize(ref dummyPlane);//Left
            Serialize(ref dummyPlane);//Right
            Serialize(ref dummyPlane);//Top
            Serialize(ref dummyPlane);//Bottom
        }

        public void Serialize(ref BoundingSphere value)
        {
            Serialize(ref value.Center);
            Serialize(ref value.Radius);
        }

        public void Serialize(ref Color value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.ToRgba());
            }
            else
            {
                value = Color.FromRgba(Reader.ReadInt32());
            }
        }

        public void Serialize(ref Color3 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.Red);
                Writer.Write(value.Green);
                Writer.Write(value.Blue);
            }
            else
            {
                value.Red = Reader.ReadSingle();
                value.Green = Reader.ReadSingle();
                value.Blue = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Color4 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.Red);
                Writer.Write(value.Green);
                Writer.Write(value.Blue);
                Writer.Write(value.Alpha);
            }
            else
            {
                value.Red = Reader.ReadSingle();
                value.Green = Reader.ReadSingle();
                value.Blue = Reader.ReadSingle();
                value.Alpha = Reader.ReadSingle();
            }
        }

        public void Serialize(ref ColorBGRA value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.ToBgra());
            }
            else
            {
                value = ColorBGRA.FromBgra(Reader.ReadInt32());
            }
        }

        public void Serialize(ref Half value)
        {
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.RawValue);
            }
            else
            {
                value.RawValue = Reader.ReadUInt16();
            }
        }

        public void Serialize(ref Half2 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X.RawValue);
                Writer.Write(value.Y.RawValue);
            }
            else
            {
                value.X.RawValue = Reader.ReadUInt16();
                value.Y.RawValue = Reader.ReadUInt16();
            }
        }

        public void Serialize(ref Half3 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X.RawValue);
                Writer.Write(value.Y.RawValue);
                Writer.Write(value.Z.RawValue);
            }
            else
            {
                value.X.RawValue = Reader.ReadUInt16();
                value.Y.RawValue = Reader.ReadUInt16();
                value.Z.RawValue = Reader.ReadUInt16();
            }
        }

        public void Serialize(ref Half4 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X.RawValue);
                Writer.Write(value.Y.RawValue);
                Writer.Write(value.Z.RawValue);
                Writer.Write(value.W.RawValue);
            }
            else
            {
                value.X.RawValue = Reader.ReadUInt16();
                value.Y.RawValue = Reader.ReadUInt16();
                value.Z.RawValue = Reader.ReadUInt16();
                value.W.RawValue = Reader.ReadUInt16();
            }
        }

        public void Serialize(ref Int3 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Z);
            }
            else
            {
                value.X = Reader.ReadInt32();
                value.Y = Reader.ReadInt32();
                value.Z = Reader.ReadInt32();
            }
        }

        public void Serialize(ref Int4 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Z);
                Writer.Write(value.W);
            }
            else
            {
                value.X = Reader.ReadInt32();
                value.Y = Reader.ReadInt32();
                value.Z = Reader.ReadInt32();
                value.W = Reader.ReadInt32();
            }
        }

        public void Serialize(ref Matrix value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.M11);
                Writer.Write(value.M12);
                Writer.Write(value.M13);
                Writer.Write(value.M14);
                Writer.Write(value.M21);
                Writer.Write(value.M22);
                Writer.Write(value.M23);
                Writer.Write(value.M24);
                Writer.Write(value.M31);
                Writer.Write(value.M32);
                Writer.Write(value.M33);
                Writer.Write(value.M34);
                Writer.Write(value.M41);
                Writer.Write(value.M42);
                Writer.Write(value.M43);
                Writer.Write(value.M44);
            }
            else
            {
                value.M11 = Reader.ReadSingle();
                value.M12 = Reader.ReadSingle();
                value.M13 = Reader.ReadSingle();
                value.M14 = Reader.ReadSingle();
                value.M21 = Reader.ReadSingle();
                value.M22 = Reader.ReadSingle();
                value.M23 = Reader.ReadSingle();
                value.M24 = Reader.ReadSingle();
                value.M31 = Reader.ReadSingle();
                value.M32 = Reader.ReadSingle();
                value.M33 = Reader.ReadSingle();
                value.M34 = Reader.ReadSingle();
                value.M41 = Reader.ReadSingle();
                value.M42 = Reader.ReadSingle();
                value.M43 = Reader.ReadSingle();
                value.M44 = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Matrix3x2 value)
        {
            // Write optivalue.Mized version without using Serialize value.Methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.M11);
                Writer.Write(value.M12);
                Writer.Write(value.M21);
                Writer.Write(value.M22);
                Writer.Write(value.M31);
                Writer.Write(value.M32);
            }
            else
            {
                value.M11 = Reader.ReadSingle();
                value.M12 = Reader.ReadSingle();
                value.M21 = Reader.ReadSingle();
                value.M22 = Reader.ReadSingle();
                value.M31 = Reader.ReadSingle();
                value.M32 = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Matrix3x3 value)
        {
            // Write optivalue.Mized version without using Serialize value.Methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.M11);
                Writer.Write(value.M12);
                Writer.Write(value.M13);
                Writer.Write(value.M21);
                Writer.Write(value.M22);
                Writer.Write(value.M23);
                Writer.Write(value.M31);
                Writer.Write(value.M32);
                Writer.Write(value.M33);
            }
            else
            {
                value.M11 = Reader.ReadSingle();
                value.M12 = Reader.ReadSingle();
                value.M13 = Reader.ReadSingle();
                value.M21 = Reader.ReadSingle();
                value.M22 = Reader.ReadSingle();
                value.M23 = Reader.ReadSingle();
                value.M31 = Reader.ReadSingle();
                value.M32 = Reader.ReadSingle();
                value.M33 = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Matrix5x4 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.M11);
                Writer.Write(value.M12);
                Writer.Write(value.M13);
                Writer.Write(value.M14);
                Writer.Write(value.M21);
                Writer.Write(value.M22);
                Writer.Write(value.M23);
                Writer.Write(value.M24);
                Writer.Write(value.M31);
                Writer.Write(value.M32);
                Writer.Write(value.M33);
                Writer.Write(value.M34);
                Writer.Write(value.M41);
                Writer.Write(value.M42);
                Writer.Write(value.M43);
                Writer.Write(value.M44);
                Writer.Write(value.M51);
                Writer.Write(value.M52);
                Writer.Write(value.M53);
                Writer.Write(value.M54);
            }
            else
            {
                value.M11 = Reader.ReadSingle();
                value.M12 = Reader.ReadSingle();
                value.M13 = Reader.ReadSingle();
                value.M14 = Reader.ReadSingle();
                value.M21 = Reader.ReadSingle();
                value.M22 = Reader.ReadSingle();
                value.M23 = Reader.ReadSingle();
                value.M24 = Reader.ReadSingle();
                value.M31 = Reader.ReadSingle();
                value.M32 = Reader.ReadSingle();
                value.M33 = Reader.ReadSingle();
                value.M34 = Reader.ReadSingle();
                value.M41 = Reader.ReadSingle();
                value.M42 = Reader.ReadSingle();
                value.M43 = Reader.ReadSingle();
                value.M44 = Reader.ReadSingle();
                value.M51 = Reader.ReadSingle();
                value.M52 = Reader.ReadSingle();
                value.M53 = Reader.ReadSingle();
                value.M54 = Reader.ReadSingle();
            }
        }

        public void Serialize(ref OrientedBoundingBox value)
        {
            Serialize(ref value.Extents);
            Serialize(ref value.Transformation);
        }

        public void Serialize(ref Plane value)
        {
            Serialize(ref value.Normal);
            Serialize(ref value.D);
        }

        public void Serialize(ref Point value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
            }
            else
            {
                value.X = Reader.ReadInt32();
                value.Y = Reader.ReadInt32();
            }
        }

        public void Serialize(ref Quaternion value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Z);
                Writer.Write(value.W);
            }
            else
            {
                value.X = Reader.ReadSingle();
                value.Y = Reader.ReadSingle();
                value.Z = Reader.ReadSingle();
                value.W = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Ray value)
        {
            Serialize(ref value.Position);
            Serialize(ref value.Direction);
        }

        public void Serialize(ref Rectangle value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.Left);
                Writer.Write(value.Top);
                Writer.Write(value.Right);
                Writer.Write(value.Bottom);
            }
            else
            {
                value.Left = Reader.ReadInt32();
                value.Top = Reader.ReadInt32();
                value.Right = Reader.ReadInt32();
                value.Bottom = Reader.ReadInt32();
            }
        }

        public void Serialize(ref RectangleF value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.Left);
                Writer.Write(value.Top);
                Writer.Write(value.Right);
                Writer.Write(value.Bottom);
            }
            else
            {
                value.Left = Reader.ReadSingle();
                value.Top = Reader.ReadSingle();
                value.Right = Reader.ReadSingle();
                value.Bottom = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Vector2 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
            }
            else
            {
                value.X = Reader.ReadSingle();
                value.Y = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Vector3 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Z);
            }
            else
            {
                value.X = Reader.ReadSingle();
                value.Y = Reader.ReadSingle();
                value.Z = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Vector4 value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Z);
                Writer.Write(value.W);
            }
            else
            {
                value.X = Reader.ReadSingle();
                value.Y = Reader.ReadSingle();
                value.Z = Reader.ReadSingle();
                value.W = Reader.ReadSingle();
            }
        }

        public void Serialize(ref Viewport value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Width);
                Writer.Write(value.Height);
                Writer.Write(value.MinDepth);
                Writer.Write(value.MaxDepth);
            }
            else
            {
                value.X = Reader.ReadInt32();
                value.Y = Reader.ReadInt32();
                value.Width = Reader.ReadInt32();
                value.Height = Reader.ReadInt32();
                value.MinDepth = Reader.ReadSingle();
                value.MaxDepth = Reader.ReadSingle();
            }
        }

        public void Serialize(ref ViewportF value)
        {
            // Write optimized version without using Serialize methods
            if (Mode == SerializerMode.Write)
            {
                Writer.Write(value.X);
                Writer.Write(value.Y);
                Writer.Write(value.Width);
                Writer.Write(value.Height);
                Writer.Write(value.MinDepth);
                Writer.Write(value.MaxDepth);
            }
            else
            {
                value.X = Reader.ReadSingle();
                value.Y = Reader.ReadSingle();
                value.Width = Reader.ReadSingle();
                value.Height = Reader.ReadSingle();
                value.MinDepth = Reader.ReadSingle();
                value.MaxDepth = Reader.ReadSingle();
            }
        }
    }
}
