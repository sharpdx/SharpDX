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
using System.Globalization;
using System.Runtime.InteropServices;

using SharpDX.Serialization;

namespace SharpDX
{
    /// <summary>
    /// Defines the viewport dimensions using float coordinates for (X,Y,Width,Height).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ViewportF : IEquatable<ViewportF>, IDataSerializable
    {
        /// <summary>
        /// Position of the pixel coordinate of the upper-left corner of the viewport.
        /// </summary>
        public float X;

        /// <summary>
        /// Position of the pixel coordinate of the upper-left corner of the viewport.
        /// </summary>
        public float Y;

        /// <summary>
        /// Width dimension of the viewport.
        /// </summary>
        public float Width;

        /// <summary>
        /// Height dimension of the viewport.
        /// </summary>
        public float Height;

        /// <summary>
        /// Gets or sets the minimum depth of the clip volume.
        /// </summary>
        public float MinDepth;

        /// <summary>
        /// Gets or sets the maximum depth of the clip volume.
        /// </summary>
        public float MaxDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportF"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left corner of the viewport in pixels.</param>
        /// <param name="y">The y coordinate of the upper-left corner of the viewport in pixels.</param>
        /// <param name="width">The width of the viewport in pixels.</param>
        /// <param name="height">The height of the viewport in pixels.</param>
        public ViewportF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            MinDepth = 0f;
            MaxDepth = 1f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportF"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left corner of the viewport in pixels.</param>
        /// <param name="y">The y coordinate of the upper-left corner of the viewport in pixels.</param>
        /// <param name="width">The width of the viewport in pixels.</param>
        /// <param name="height">The height of the viewport in pixels.</param>
        /// <param name="minDepth">The minimum depth of the clip volume.</param>
        /// <param name="maxDepth">The maximum depth of the clip volume.</param>
        public ViewportF(float x, float y, float width, float height, float minDepth, float maxDepth)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            MinDepth = minDepth;
            MaxDepth = maxDepth;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportF"/> struct.
        /// </summary>
        /// <param name="bounds">A bounding box that defines the location and size of the viewport in a render target.</param>
        public ViewportF(RectangleF bounds)
        {
            X = bounds.X;
            Y = bounds.Y;
            Width = bounds.Width;
            Height = bounds.Height;
            MinDepth = 0f;
            MaxDepth = 1f;
        }

        /// <summary>
        /// Gets the size of this resource.
        /// </summary>
        /// <value>The bounds.</value>
        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X, Y, Width, Height);
            }

            set
            {
                X = value.X;
                Y = value.Y;
                Width = value.Width;
                Height = value.Height;
            }
        }

    	/// <summary>
        /// Determines whether the specified <see cref="SharpDX.ViewportF"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="SharpDX.ViewportF"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="SharpDX.ViewportF"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ViewportF other)
        {
            return MathUtil.NearEqual(X, other.X) && MathUtil.NearEqual(Y, other.Y) && MathUtil.NearEqual(Width, other.Width) && MathUtil.NearEqual(Height, other.Height) && MathUtil.NearEqual(MinDepth, other.MinDepth)
                   && MathUtil.NearEqual(MaxDepth, other.MaxDepth);
        }
	
        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is ViewportF && Equals((ViewportF)obj);
        }
	
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                hashCode = (hashCode * 397) ^ MinDepth.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxDepth.GetHashCode();
                return hashCode;
            }
        }

	/// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ViewportF left, ViewportF right)
        {
            return left.Equals(right);
        }

	/// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ViewportF left, ViewportF right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Retrieves a string representation of this object.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3} MinDepth:{4} MaxDepth:{5}}}", X, Y, Width, Height, MinDepth, MaxDepth);
        }

        /// <summary>
        /// Projects a 3D vector from object space into screen space.
        /// </summary>
        /// <param name="source">The vector to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <returns>Vector3.</returns>
        public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            var matrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
            var vector = (Vector3)Vector3.Transform(source, matrix);
            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;

            if (!MathUtil.IsOne(a))
            {
                vector = (vector / a);
            }

            vector.X = (((vector.X + 1f) * 0.5f) * Width) + X;
            vector.Y = (((-vector.Y + 1f) * 0.5f) * Height) + Y;
            vector.Z = (vector.Z * (MaxDepth - MinDepth)) + MinDepth;

            return vector;
        }

        /// <summary>
        /// Converts a screen space point into a corresponding point in world space.
        /// </summary>
        /// <param name="source">The vector to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <returns>Vector3.</returns>
        public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            var matrix = Matrix.Invert(Matrix.Multiply(Matrix.Multiply(world, view), projection));
            source.X = (((source.X - X) / (Width)) * 2f) - 1f;
            source.Y = -((((source.Y - Y) / (Height)) * 2f) - 1f);
            source.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);
            var vector = (Vector3)Vector3.Transform(source, matrix);

            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!MathUtil.IsOne(a))
            {
                vector = (vector / a);
            }

            return vector;
        }

        /// <summary>
        /// Gets the aspect ratio used by the viewport.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public float AspectRatio
        {
            get
            {
                if (!MathUtil.IsZero(Height))
                {
                    return Width / Height;
                }
                return 0f;
            }
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpDX.Viewport"/> to <see cref="SharpDX.ViewportF"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ViewportF(Viewport value)
        {
            return new ViewportF(value.X, value.Y, value.Width, value.Height, value.MinDepth, value.MaxDepth);
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(X);
                serializer.Writer.Write(Y);
                serializer.Writer.Write(Width);
                serializer.Writer.Write(Height);
                serializer.Writer.Write(MinDepth);
                serializer.Writer.Write(MaxDepth);
            }
            else
            {
                X = serializer.Reader.ReadSingle();
                Y = serializer.Reader.ReadSingle();
                Width = serializer.Reader.ReadSingle();
                Height = serializer.Reader.ReadSingle();
                MinDepth = serializer.Reader.ReadSingle();
                MaxDepth = serializer.Reader.ReadSingle();
            }
        }
    }
}
