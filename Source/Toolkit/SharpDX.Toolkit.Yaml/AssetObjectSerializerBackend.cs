// Copyright (c) 2010-2014 SharpDX - SharpDX Team
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
using SharpDX.Toolkit.Content;
using SharpYaml.Events;
using SharpYaml.Serialization;
using SharpYaml.Serialization.Serializers;
using System;
using System.Collections.Generic;

namespace SharpDX.Toolkit.Yaml
{
    /// <summary>
    /// Allows the <see cref="YamlManager"/> to load related assets by name.
    /// </summary>
    internal class AssetObjectSerializerBackend: DefaultObjectSerializerBackend
    {
        private readonly IContentManager contentManager;

        public AssetObjectSerializerBackend(SerializerSettings settings, IContentManager contentManager)
        {
            settings.RegisterTagMapping("!asset", typeof(AssetName));
            settings.RegisterSerializerFactory(new AssetNameFactory());
            this.contentManager = contentManager;
        }

        public override object ReadMemberValue(ref ObjectContext objectContext, IMemberDescriptor memberDescriptor, object memberValue, Type memberType)
        {
            return LoadAssetIfRequired(memberType, base.ReadMemberValue(ref objectContext, memberDescriptor, memberValue, memberType));
        }

        public override object ReadCollectionItem(ref ObjectContext objectContext, Type itemType)
        {
            return LoadAssetIfRequired(itemType, base.ReadCollectionItem(ref objectContext, itemType));
        }

        public override KeyValuePair<object, object> ReadDictionaryItem(ref ObjectContext objectContext, KeyValuePair<Type, Type> keyValueType)
        {
            var pair = base.ReadDictionaryItem(ref objectContext, keyValueType);
            var contentValue = LoadAssetIfRequired(keyValueType.Value, pair.Value);
            return new KeyValuePair<object, object>(pair.Key, contentValue);
        }

        private object LoadAssetIfRequired(Type expectedType, object value)
        {
            var assetName = value as AssetName;
            if (assetName != null)
                return contentManager.Load(expectedType, assetName.Name);

            return value;
        }

        private class AssetName
        {
            public string Name { get; set; }
        }

        private class AssetNameFactory : ScalarSerializerBase, IYamlSerializableFactory
        {
            public IYamlSerializable TryCreate(SerializerContext context, ITypeDescriptor typeDescriptor)
            {
                return typeDescriptor.Type == typeof(AssetName) ? this : null;
            }

            public override object ConvertFrom(ref ObjectContext context, Scalar fromScalar)
            {
                return new AssetName() { Name = fromScalar.Value };
            }

            public override string ConvertTo(ref ObjectContext objectContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
