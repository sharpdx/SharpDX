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
using YamlDotNet.Serialization;

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
            serializer = new Serializer(yamlSettings);
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
            serializer = new Serializer(yamlSettings);
        }

        public override void Initialize()
        {
            base.Initialize();

            contentManager = Content as ContentManager;
            if(contentManager == null)
            {
                throw new InvalidOperationException("Unable to initialize YamlManager. Expecting IContentManager to be an instance of ContentManager");
            }

            contentManager.ReaderFactories.Add(this);
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
    }
}