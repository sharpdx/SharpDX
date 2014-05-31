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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Serialization
{
    public partial class BinarySerializer
    {
        /// <summary>
        /// Serializes a static <see cref="StreamOutputElement"/>.
        /// </summary>
        /// <param name="value">The <see cref="StreamOutputElement"/> to serialize</param>
        /// <param name="serializeFlags">Type of serialization, see <see cref="SerializeFlags"/>.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref StreamOutputElement value, SerializeFlags serializeFlags = SerializeFlags.Normal)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref value, out storeObjectRef, serializeFlags))
                return;

            if (Mode == SerializerMode.Read)
                value = new StreamOutputElement();

            // Store ObjectRef
            if (storeObjectRef >= 0) StoreObjectRef(value, storeObjectRef);

            Serialize(ref value.Stream);
            Serialize(ref value.SemanticName);
            Serialize(ref value.SemanticIndex);
            Serialize(ref value.StartComponent);
            Serialize(ref value.ComponentCount);
            Serialize(ref value.OutputSlot);
        }

        /// <summary>
        /// Serializes an array of static <see cref="StreamOutputElement"/>s.
        /// </summary>
        /// <param name="valueArray">An array of <see cref="StreamOutputElement"/>s to serialize</param>
        /// <param name="serializeFlags">Type of serialization, see <see cref="SerializeFlags"/>.</param>
        /// <remarks>
        /// Note that depending on the serialization <see cref="Mode"/>, this method reads or writes the value.
        /// </remarks>
        public void Serialize(ref StreamOutputElement[] valueArray, SerializeFlags serializeFlags = SerializeFlags.Normal)
        {
            int storeObjectRef;
            if (SerializeIsNull(ref valueArray, out storeObjectRef, serializeFlags))
                return;

            if (Mode == SerializerMode.Write)
            {
                // Store ObjectRef
                if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);

                WriteArrayLength(valueArray.Length);
                for (int i = 0; i < valueArray.Length; i++)
                    Serialize(ref valueArray[i], serializeFlags);
            }
            else
            {
                var count = ReadArrayLength();
                valueArray = new StreamOutputElement[count];

                // Store ObjectRef
                if (storeObjectRef >= 0) StoreObjectRef(valueArray, storeObjectRef);

                for (int index = 0; index < count; index++)
                    Serialize(ref valueArray[index], serializeFlags);
            }
        }
    }
}
