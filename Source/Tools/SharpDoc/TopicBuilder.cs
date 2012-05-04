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
using System.Collections.Generic;
using System.IO;

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
        public void Run(Config config, Func<IModelReference, string> pageIdFunction)
        {
            RootTopic = config.RootTopic;

            // Load an existing root topic
            if (RootTopic != null)
                RootTopic.Init(Path.GetDirectoryName(config.FilePath), pageIdFunction);

            NTopic topicLibrary = null;

            // If there are any assemblies, we have to generate class library topics
            if (Assemblies.Count >= 0)
            {
                // Find if a ClassLibrary topic is referenced in the config topic
                topicLibrary = (RootTopic != null) ? RootTopic.FindTopicById(NTopic.ClassLibraryTopicId) : null;
                if (topicLibrary == null)
                {
                    // If no class library topic found, create a new one
                    topicLibrary = NTopic.DefaultClassLibraryTopic;
                    if (RootTopic == null)
                        RootTopic = topicLibrary;
                    else
                        RootTopic.SubTopics.Add(topicLibrary);
                }
            }

            if (RootTopic == null)
            {
                Logger.Fatal("No root topic assigned/ no topic for class library ");
                return;
            }

            // Calculate starting index for class library based on index from topics
            int index = 0;
            var indices = new HashSet<int>();
            var topicToReindex = new List<NTopic>();
            RootTopic.ForEachTopic(
                topic =>
                    {
                        if (indices.Contains(topic.Index))
                        {
                            Logger.Warning("Index [{0}] for Topic [{1}] is already used. Need to reassign a new index.", topic.Index, topic.Name);
                            topicToReindex.Add(topic);
                        }
                        else
                        {
                            indices.Add(topic.Index);
                            index = Math.Max(index, topic.Index);
                        }
                    });
            index++;
            foreach (var topicToIndex in topicToReindex)
            {
                topicToIndex.Index = index++;
            }

            // Process aseemblies
            foreach (var assembly in Assemblies)
            {
                // Affect new Index based on previous topics
                assembly.Index = index++;
                var assemblyTopic = new NTopic(assembly) { Name = assembly.Name + " Assembly", AttachedClassNode = assembly };
                assembly.TopicLink = assemblyTopic;
                topicLibrary.SubTopics.Add(assemblyTopic);
                assembly.SeeAlsos.Add(new NSeeAlso(topicLibrary));

                foreach (var @namespace in assembly.Namespaces)
                {
                    // Affect new Index based on previous topics
                    @namespace.Index = index++;
                    var namespaceTopic = new NTopic(@namespace) { Name = @namespace.Name + " Namespace", AttachedClassNode = @namespace };
                    @namespace.TopicLink = namespaceTopic;
                    assemblyTopic.SubTopics.Add(namespaceTopic);
                    @namespace.SeeAlsos.Add(new NSeeAlso(topicLibrary));

                    foreach (var type in @namespace.Types)
                    {
                        // Affect new Index based on previous topics
                        type.Index = index++;
                        var typeTopic = new NTopic(type) { Name = type.Name + " " + type.Category, AttachedClassNode = type };
                        type.TopicLink = typeTopic;
                        namespaceTopic.SubTopics.Add(typeTopic);
                        type.SeeAlsos.Add(new NSeeAlso(topicLibrary));

                        // We don't process fields for enums
                        if (!(type is NEnum))
                        {
                            foreach (var member in type.Members)
                            {
                                // Affect new Index based on previous topics
                                member.Index = index++;
                                member.SeeAlsos.Add(new NSeeAlso(topicLibrary));
                            }
                        }
                    }
                }
            }

            SearchTopic = NTopic.DefaultSearchResultsTopic;
            SearchTopic.Index = index++;

            // Add SearchTopic to the root topic
            RootTopic.SubTopics.Add(SearchTopic);

            // Associate each topic with its parent
            RootTopic.BuildParents();

            // Register root topics and all sub topics (excluding class library topics)
            Registry.Register(RootTopic);
        }
    }
}