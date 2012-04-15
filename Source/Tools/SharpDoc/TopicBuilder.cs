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
using System.Collections.Generic;
using SharpCore.Logging;
using SharpDoc.Model;

namespace SharpDoc
{
    /// <summary>
    /// Build topics from assemblies and config topic.
    /// </summary>
    public class TopicBuilder
    {
        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        public List<NAssembly> Assemblies { get; set; }

        /// <summary>
        /// Gets or sets the registry.
        /// </summary>
        /// <value>The registry.</value>
        public MemberRegistry Registry { get; set; }

        /// <summary>
        /// Gets or sets the root topic.
        /// </summary>
        /// <value>The root topic.</value>
        public NTopic RootTopic { get; set; }

        /// <summary>
        /// Gets or sets the search topic.
        /// </summary>
        /// <value>The search topic.</value>
        public NTopic SearchTopic { get; set; }

        /// <summary>
        /// Build topics for Assemblies, Namespaces and Types in order to use them in the TOC
        /// </summary>
        public void Run()
        {
            // If there are any assemblies, we have to generate class library topics
            if (Assemblies.Count >= 0)
            {
                // Find if a ClassLibrary topic is referenced in the config topic
                var topicLibrary = (RootTopic != null) ? RootTopic.FindTopicById(NTopic.ClassLibraryTopicId) : null;
                if (topicLibrary == null)
                {
                    // If no class library topic found, create a new one
                    topicLibrary = NTopic.DefaultClassLibraryTopic;
                    if (RootTopic == null)
                        RootTopic = topicLibrary;
                    else
                        RootTopic.SubTopics.Add(topicLibrary);
                }

                if (topicLibrary == null)
                {
                    Logger.Fatal("Cannot find topic [{0}] for class library ", NTopic.ClassLibraryTopicId);
                    return;
                }

                foreach (var assembly in Assemblies)
                {
                    var assemblyTopic = new NTopic(assembly);
                    assemblyTopic.Name = assembly.Name + " Assembly";
                    assembly.TopicLink = assemblyTopic;
                    topicLibrary.SubTopics.Add(assemblyTopic);

                    foreach (var @namespace in assembly.Namespaces)
                    {
                        var namespaceTopic = new NTopic(@namespace);
                        namespaceTopic.Name = @namespace.Name + " Namespace";
                        @namespace.TopicLink = namespaceTopic;
                        assemblyTopic.SubTopics.Add(namespaceTopic);

                        foreach (var type in @namespace.Types)
                        {
                            var typeTopic = new NTopic(type);
                            typeTopic.Name = type.Name + " " + type.TypeName;
                            type.TopicLink = typeTopic;
                            namespaceTopic.SubTopics.Add(typeTopic);
                        }
                    }
                }
            }

            if (RootTopic == null)
            {
                Logger.Fatal("Not root topic defined");
                return;
            }

            SearchTopic = NTopic.DefaultSearchResultsTopic;

            // Add SearchTopic to the root topic
            RootTopic.SubTopics.Add(SearchTopic);

            // Associate each topic with its parent
            RootTopic.BuildParents();

            // Register root topics and all sub topics (excluding class library topics)
            Registry.Register(RootTopic);
        }
    }
}