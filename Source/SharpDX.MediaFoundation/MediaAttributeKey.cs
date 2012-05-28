// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using SharpDX.Win32;

namespace SharpDX.MediaFoundation
{
    /// <summary>
    /// Associate an attribute key with a type used to retreive keys from a <see cref="MediaAttributes"/> instance.
    /// </summary>
    public class MediaAttributeKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey"/> struct.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="type">The type.</param>
        public MediaAttributeKey(Guid guid, Type type)
        {
            Guid = guid;
            Type = type;
        }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; private set; }


        internal static object GetValueFromVariant(ref SharpDX.Win32.Variant variant)
        {
            var value = variant.Value;
            if (value == null)
                return null;

            if (value is uint || value is int)
            {
                return (int)Convert.ChangeType(value, typeof(int));
            }

            if (value is ulong || value is long)
            {
                return (long)Convert.ChangeType(value, typeof(long));
            }

            if (value is double)
            {
                return (int)Convert.ChangeType(value, typeof(double));
            }
            
            return value;
        }

        internal static T GetValueFromVariant<T>(ref SharpDX.Win32.Variant variant)
        {
            var value = variant.Value;
            if (value == null)
                return default(T);

            if (value is uint || value is ulong || value is double)
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (variant.Type == VariantType.Default && variant.ElementType == VariantElementType.ComUnknown && typeof(T).IsSubclassOf(typeof(ComObject)) )
            {
                return (T)value;
            }

            if (value.GetType() == typeof(T))
            {
                return (T)value;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Variant value type [{0}] is not handled", value.GetType().Name));
        }


        internal static void SetValueToVariant<T>(ref SharpDX.Win32.Variant variant, T value)
        {
            if (value is int || value is bool || value.GetType().IsEnum || value is byte || value is uint)
            {
                variant.Value = Convert.ToUInt32(value);
                return;
            }

            if (value is ulong || value is IntPtr || value is long)
            {
                variant.Value = Convert.ToUInt64(value);
                return;
            }

            if (value is string || value is byte[] || value is ComObject)
            {
                variant.Value = value;
                return;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Variant value type [{0}] is not handled", value.GetType().Name));
        }

        internal static void SetValueToVariant(ref SharpDX.Win32.Variant variant, object value)
        {
            if (value is int || value is bool || value.GetType().IsEnum || value is byte || value is uint)
            {
                variant.Value = Convert.ToUInt32(value);
                return;
            }

            if (value is ulong || value is IntPtr || value is long)
            {
                variant.Value = Convert.ToUInt64(value);
                return;
            }

            if (value is string || value is byte[] || value is ComObject)
            {
                variant.Value = value;
                return;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Variant value type [{0}] is not handled", value.GetType().Name));
        }
    }


    /// <summary>
    /// Generic version of <see cref="MediaAttributeKey"/>
    /// </summary>
    /// <typeparam name="T">Type of the value of this key</typeparam>
    public class MediaAttributeKey<T> : MediaAttributeKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public MediaAttributeKey(string guid)
            : base(new Guid(guid), typeof(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAttributeKey&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public MediaAttributeKey(Guid guid) : base(guid, typeof(T))
        {
        }

        internal T GetValue(ref SharpDX.Win32.Variant variant)
        {
            return GetValueFromVariant<T>(ref variant);
        }

        internal void SetValue<T>(ref SharpDX.Win32.Variant variant, T value)
        {
            SetValueToVariant(ref variant, value);
        }
    }
}