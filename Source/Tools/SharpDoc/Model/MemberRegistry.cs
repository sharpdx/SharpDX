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
using System.Collections.Generic;
using SharpCore.Logging;

namespace SharpDoc.Model
{
    /// <summary>
    /// A class that associate a xml id to a <see cref="IModelReference"/>.
    /// </summary>
    public class MemberRegistry
    {
        private readonly Dictionary<string, IModelReference> _mapIdToModelElement;
        private const string TopicContainer = "__topics__";

        private readonly List<NNamespace> namespaces = new List<NNamespace>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRegistry"/> class.
        /// </summary>
        public MemberRegistry()
        {
            _mapIdToModelElement = new Dictionary<string, IModelReference>();
        }

        public List<NNamespace> Namespaces
        {
            get
            {
                return namespaces;
            }
        }

        /// <summary>
        /// Finds the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="context">The context. If context is null, look inside all registered assemblies</param>
        /// <returns></returns>
        public IModelReference FindById(string id)
        {
            IModelReference refFound = null;
            _mapIdToModelElement.TryGetValue(id, out refFound);
            return refFound;
        }

        /// <summary>
        /// Registers the specified topic and all subtopics.
        /// </summary>
        public void Register(NTopic topic)
        {
            if (topic == null)
                return;

            var previousTopic = FindById(topic.Id);
            if (previousTopic is NTopic)
            {
                Logger.Error("The topic [{0}] is already declared", previousTopic.Id);
                return;
            }

            // Otherwise, the previous topic is probably a class/member definition, so we
            // don't need to register it
            if (previousTopic != null)
                return;

            // If this is a real topic, we can register it and all its children
            Register((IModelReference)topic);
            foreach (var subTopic in topic.SubTopics)
                Register(subTopic);
        }

        /// <summary>
        /// Registers the specified model element with the specified id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="modelReference">The model element.</param>
        public bool Register(IModelReference modelReference)
        {
            if (_mapIdToModelElement.ContainsKey(modelReference.Id))
            {
                return false;
            }
            _mapIdToModelElement.Add(modelReference.Id, modelReference);

            if (modelReference is NNamespace)
            {
                namespaces.Add((NNamespace)modelReference);
            }

            return true;
        }
    }
}