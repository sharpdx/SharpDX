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
using System.Runtime.InteropServices;

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// A Work Queue Identifier
    /// </summary>
    /// <msdn-id>ms703102</msdn-id>	
    /// <unmanaged>Work Queue Identifiers</unmanaged>	
    /// <unmanaged-short>Work Queue Identifiers</unmanaged-short>	
    [StructLayout(LayoutKind.Sequential)]
    public struct WorkQueueId : IEquatable<WorkQueueId>
    {
        /// <summary>
        /// The default queue associated to the <see cref="WorkQueueType.Standard"/>.
        /// </summary>
        public static readonly WorkQueueId Standard = new WorkQueueId(WorkQueueType.Standard);

        /// <summary>
        /// The identifier.
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueueId"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        public WorkQueueId(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkQueueId"/> struct.
        /// </summary>
        /// <param name="id">The id.</param>
        public WorkQueueId(WorkQueueType id)
        {
            this.Id = (int)id;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(WorkQueueId other)
        {
            return Id == other.Id;
        }

        /// <summary>Determines whether the specified <see cref="System.Object" /> is equal to this instance.</summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return obj is WorkQueueId && Equals((WorkQueueId)obj);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>Implements the ==.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(WorkQueueId left, WorkQueueId right)
        {
            return left.Equals(right);
        }

        /// <summary>Implements the !=.</summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(WorkQueueId left, WorkQueueId right)
        {
            return !left.Equals(right);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0} (Type: {1})", Id, (WorkQueueType)Id);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="WorkQueueId"/>.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WorkQueueId(int id)
        {
            return new WorkQueueId(id);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="WorkQueueType"/> to <see cref="WorkQueueId"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WorkQueueId(WorkQueueType type)
        {
            return new WorkQueueId(type);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="WorkQueueId"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="workQueueId">The work queue Id.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator int(WorkQueueId workQueueId)
        {
            return workQueueId.Id;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="WorkQueueId"/> to <see cref="WorkQueueType"/>.
        /// </summary>
        /// <param name="workQueueId">The work queue Id.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator WorkQueueType(WorkQueueId workQueueId)
        {
            return (WorkQueueType)workQueueId.Id;
        }
    }
}