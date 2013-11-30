using SharpDX.Toolkit.Content;
using SharpYaml.Events;
using SharpYaml.Serialization;
using SharpYaml.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpDX.Toolkit.Yaml
{
    /// <summary>
    /// Allows the <see cref="YamlManager"/> to load related assets by name.
    /// </summary>
    internal class AssetObjectSerializerBackend: DefaultObjectSerializerBackend
    {
        private readonly SerializerSettings settings;
        private readonly IContentManager contentManager;

        public AssetObjectSerializerBackend(SerializerSettings settings, IContentManager contentManager)
        {
            this.settings = settings;
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
            if (value is AssetName)
                return contentManager.Load(expectedType, ((AssetName)value).Name);

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
