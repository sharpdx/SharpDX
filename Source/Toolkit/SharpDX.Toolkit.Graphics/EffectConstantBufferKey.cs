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

namespace SharpDX.Toolkit.Graphics
{
    internal class EffectConstantBufferKey : IEquatable<EffectConstantBufferKey>
    {
        public readonly EffectData.ConstantBuffer Description;
        public readonly int HashCode;

        public EffectConstantBufferKey(EffectData.ConstantBuffer description)
        {
            Description = description;
            HashCode = description.GetHashCode();
        }

        #region IEquatable<EffectConstantBufferKey> Members

        public bool Equals(EffectConstantBufferKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return HashCode == other.HashCode && Description.Equals(other.Description);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EffectConstantBufferKey) obj);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public static bool operator ==(EffectConstantBufferKey left, EffectConstantBufferKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectConstantBufferKey left, EffectConstantBufferKey right)
        {
            return !Equals(left, right);
        }
    }
}