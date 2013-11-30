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
using System.Reflection;
using SharpDX.Toolkit.Content;
using SharpYaml;
using SharpYaml.Serialization;

namespace SharpDX.Toolkit.Yaml
{
    /// <summary>
    /// This manager allows to read/deserialize YAML files into .NET objects using
    /// the <see cref="IContentManager"/>
    /// </summary>
    public class YamlManager : GameSystem, IContentReaderFactory, IContentReader
    {
        private ContentManager contentManager;
        private readonly SerializerSettings yamlSettings;
        private readonly IAttributeRegistry attributeRegistry;
        private readonly Serializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSystem" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public YamlManager(IServiceRegistry registry) : base(registry)
        {
            Services.AddService(this);
            yamlSettings = new SerializerSettings();            
            attributeRegistry = yamlSettings.Attributes;
            serializer = CreateSerializer(yamlSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSystem" /> class.
        /// </summary>
        /// <param name="game">The game.</param>
        public YamlManager(Game game) : base(game)
        {
            Services.AddService(this);
            yamlSettings = new SerializerSettings();
            attributeRegistry = yamlSettings.Attributes;
            serializer = CreateSerializer(yamlSettings);
        }

        public override void Initialize()
        {
            base.Initialize();

            contentManager = Content as ContentManager;
            if(contentManager == null)
            {
                throw new InvalidOperationException("Unable to initialize YamlManager. Expecting IContentManager to be an instance of ContentManager");
            }

            yamlSettings.ObjectSerializerBackend = new AssetObjectSerializerBackend(yamlSettings, contentManager);
            contentManager.ReaderFactories.Add(this);

            
        }

        protected virtual Serializer CreateSerializer(SerializerSettings settings)
        {
            return NewSerializer(settings);
        }

        /// <summary>
        /// Registers a tag mapping, an association between a type and a YAML tag, used when
        /// deserializing a YAML document, when a specific .NET type is expected.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException">
        /// tagName
        /// or
        /// type
        /// </exception>
        public void RegisterTagMapping(string tagName, Type type)
        {
            if(tagName == null) throw new ArgumentNullException("tagName");
            if(type == null) throw new ArgumentNullException("type");

            yamlSettings.RegisterTagMapping(tagName, type);
#if W8CORE
            attributeRegistry.Register(type.GetTypeInfo(), new YamlTagAttribute(tagName));
#else
            attributeRegistry.Register(type, new YamlTagAttribute(tagName));
#endif
        }

        /// <summary>
        /// Registers an attribute to the specific member, allowing to customize Yaml serialization
        /// for types that can't define Yaml attributes on their types.
        /// See <see cref="YamlTagAttribute"/>, <see cref="YamlMemberAttribute"/> and <see cref="YamlIgnoreAttribute"/>
        /// for more details.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="attribute">The attribute.</param>
        /// <exception cref="System.ArgumentNullException">
        /// memberInfo
        /// or
        /// attribute
        /// </exception>
        public void RegisterAttribute(MemberInfo memberInfo, Attribute attribute)
        {
            if(memberInfo == null) throw new ArgumentNullException("memberInfo");
            if(attribute == null) throw new ArgumentNullException("attribute");
            yamlSettings.Attributes.Register(memberInfo, attribute);
        }

        IContentReader IContentReaderFactory.TryCreate(Type type)
        {

#if W8CORE
            var attributes = attributeRegistry.GetAttributes(type.GetTypeInfo(), false);
#else
            var attributes = attributeRegistry.GetAttributes(type, false);
#endif
            if(attributes.Count > 0)
            {
                foreach (var attribute in attributes)
                {
                    if(attribute is YamlTagAttribute)
                    {
                        // Make sure that the assembly for type is registered for loading
#if W8CORE
                        yamlSettings.RegisterAssembly(type.GetTypeInfo().Assembly);
#else
                        yamlSettings.RegisterAssembly(type.Assembly);
#endif
                        return this;
                    }
                }
            }

            return null;
        }

        object IContentReader.ReadContent(IContentManager contentManagerArg, ref ContentReaderParameters parameters)
        {
            return serializer.Deserialize(parameters.Stream, parameters.AssetType);
        }

        /// <summary>
        /// Creates a new instance of the YAML serializer.
        /// </summary>
        /// <returns>The default YAML serializer used by the <see cref="YamlManager"/>.</returns>
        public static Serializer NewSerializer()
        {
            return NewSerializer(new SerializerSettings());
        }

        /// <summary>
        /// Creates a new instance of the YAML serializer.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default YAML serializer used by the <see cref="YamlManager"/>.</returns>
        /// <exception cref="System.ArgumentNullException">settings</exception>
        public static Serializer NewSerializer(SerializerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");

            RegisterDefaults(settings);
            return new Serializer(settings);
        }

        private static void RegisterDefaults(SerializerSettings settings)
        {
            settings.EmitAlias = false;

            var attributes = (AttributeRegistry)settings.Attributes;

            settings.RegisterTagMapping("!Vector2", typeof(Vector2));
            attributes.Register(GetTypeInfo<Vector2>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<Vector2>("X"), new YamlMemberAttribute(0));
            attributes.Register(GetField<Vector2>("Y"), new YamlMemberAttribute(1));

            settings.RegisterTagMapping("!Vector3", typeof(Vector3));
            attributes.Register(GetTypeInfo<Vector3>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<Vector3>("X"), new YamlMemberAttribute(0));
            attributes.Register(GetField<Vector3>("Y"), new YamlMemberAttribute(1));
            attributes.Register(GetField<Vector3>("Z"), new YamlMemberAttribute(2));

            settings.RegisterTagMapping("!Vector4", typeof(Vector4));
            attributes.Register(GetTypeInfo<Vector4>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<Vector4>("X"), new YamlMemberAttribute(0));
            attributes.Register(GetField<Vector4>("Y"), new YamlMemberAttribute(1));
            attributes.Register(GetField<Vector4>("Z"), new YamlMemberAttribute(2));
            attributes.Register(GetField<Vector4>("W"), new YamlMemberAttribute(3));

            settings.RegisterTagMapping("!Color", typeof(Color));
            attributes.Register(GetTypeInfo<Color>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<Color>("R"), new YamlMemberAttribute(0));
            attributes.Register(GetField<Color>("G"), new YamlMemberAttribute(1));
            attributes.Register(GetField<Color>("B"), new YamlMemberAttribute(2));
            attributes.Register(GetField<Color>("A"), new YamlMemberAttribute(3));

            settings.RegisterTagMapping("!ColorBGRA", typeof(ColorBGRA));
            attributes.Register(GetTypeInfo<ColorBGRA>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<ColorBGRA>("B"), new YamlMemberAttribute(0));
            attributes.Register(GetField<ColorBGRA>("G"), new YamlMemberAttribute(1));
            attributes.Register(GetField<ColorBGRA>("R"), new YamlMemberAttribute(2));
            attributes.Register(GetField<ColorBGRA>("A"), new YamlMemberAttribute(3));

            settings.RegisterTagMapping("!Color3", typeof(Color3));
            attributes.Register(GetTypeInfo<Color3>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<Color3>("Red"), new YamlMemberAttribute("R") { Order = 0 });
            attributes.Register(GetField<Color3>("Green"), new YamlMemberAttribute("G") { Order = 1 });
            attributes.Register(GetField<Color3>("Blue"), new YamlMemberAttribute("B") { Order = 2 });

            settings.RegisterTagMapping("!Color4", typeof(Color4));
            attributes.Register(GetTypeInfo<Color4>(), new YamlStyleAttribute(YamlStyle.Flow));
            attributes.Register(GetField<Color4>("Red"), new YamlMemberAttribute("R") { Order = 0 });
            attributes.Register(GetField<Color4>("Green"), new YamlMemberAttribute("G") { Order = 1 });
            attributes.Register(GetField<Color4>("Blue"), new YamlMemberAttribute("B") { Order = 2 });
            attributes.Register(GetField<Color4>("Alpha"), new YamlMemberAttribute("A") { Order = 3 });

            settings.RegisterAssembly(GetAssembly<Vector2>());
        }

        private static Assembly GetAssembly<T>()
        {
#if W8CORE
            return typeof(T).GetTypeInfo().Assembly;
#else
            return typeof(T).Assembly;
#endif
        }

        private static MemberInfo GetTypeInfo<T>()
        {
#if W8CORE
            return typeof(T).GetTypeInfo();
#else
            return typeof(T);
#endif
        }

        private static FieldInfo GetField<T>(string name)
        {
#if W8CORE
            return typeof(T).GetTypeInfo().GetDeclaredField(name);
#else
            return typeof(T).GetField(name);
#endif
        }
    }
}