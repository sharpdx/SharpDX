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
using SharpCore.Logging;
using SharpDoc.Model;

namespace SharpDoc
{
    /// <summary>
    /// A processor to add information to the model after It has been loaded.
    /// </summary>
    public class ModelProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProcessor"/> class.
        /// </summary>
        public ModelProcessor()
        {
            Assemblies = new List<NAssembly>();
            Registry = new MemberRegistry();
        }

        /// <summary>
        /// Gets the registry.
        /// </summary>
        /// <value>The registry.</value>
        public MemberRegistry Registry { get; private set;}

        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>The assemblies.</value>
        public List<NAssembly> Assemblies { get; private set; }

        /// <summary>
        /// Gets or sets the assembly manager.
        /// </summary>
        /// <value>The assembly manager.</value>
        public IAssemblyManager AssemblyManager { get; set; }

        /// <summary>
        /// Gets or sets the model builder.
        /// </summary>
        /// <value>The model builder.</value>
        public IModelBuilder ModelBuilder { get; set; }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run(Config config)
        {
            if (AssemblyManager == null)
                throw new InvalidOperationException("AssemblyManager must be set");

            if (ModelBuilder == null)
                throw new InvalidOperationException("ModelBuilder must be set");

            var assemblySources = AssemblyManager.Load(config);

            // Process all assemblies
            foreach (var assemblySource in assemblySources)
            {
                var assembly = ModelBuilder.LoadFrom(assemblySource, Registry);
                Assemblies.Add(assembly);
            }

            // Sort assemblies
            Assemblies.Sort((from, to) => from.Name.CompareTo(to.Name));

            // Perform additionnal step by adding direct descendants for each class);
            foreach (var assembly in Assemblies)
            {
                // Sort namespaces
                assembly.Namespaces.Sort((from, to) => from.Name.CompareTo(to.Name));

                foreach (var @namespace in assembly.Namespaces)
                {
                    foreach (var type in @namespace.Types)
                    {
                        if (type is NClass)
                        {
                            var directParent = type.Bases[0];
                            var baseModel = Registry.FindById(directParent.Id, assembly) as NType;
                            // If not found from current assembly, find from other assemblies
                            // TODO this is not correct. Correct behavior requires to iterate
                            // on assembly dependencies.
                            if (baseModel == null)
                                baseModel = Registry.FindById(directParent.Id) as NType;
                            if (baseModel != null)
                                baseModel.Descendants.Add(type);
                        }
                    }
                }
            }

            // Compute a flatten hierarchy and sort it
            foreach (var assembly in Assemblies)
            {
                foreach (var @namespace in assembly.Namespaces)
                {
                    foreach (var type in @namespace.Types)
                    {
                        if (type is NClass)
                            FlattenHierarchy(type, type);
                    }
                }
            }
        }

        /// <summary>
        /// Flattens the hierarchy for a class.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        /// <param name="currentType">Type of the current.</param>
        private void FlattenHierarchy(NType baseType, NType currentType)
        {
            // Sort the descendants in alphabetical order
            currentType.Descendants.Sort(CompareByDisplayName);

            // Iterate on all descendants
            foreach (var descendantRef in currentType.Descendants)
            {
                var descendant = Registry.FindById(descendantRef.Id) as NType;

                if (descendant != null)
                {
                    int level = 0;
                    int index = 0;
                    for (int i = 0; i < baseType.FlattenedHierarchy.Count; i++)
                    {
                        var flattenItem = baseType.FlattenedHierarchy[i];
                        if (flattenItem.Item2.Id == descendant.Bases[0].Id)
                        {
                            level = flattenItem.Item1;
                            index = i;
                            break;
                        }
                    }

                    // Insert the descendant at the right position in the flattened view
                    baseType.FlattenedHierarchy.Insert(index + 1, new Tuple<int, INMemberReference>(level + 1, descendant));

                    // Flatten the hierarchy for the descendant
                    // note: recursive call here!
                    FlattenHierarchy(baseType, descendant);
                }
            }
        }

        /// <summary>
        /// Compares the display name of the by two <see cref="INMemberReference"/>.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        private int CompareByDisplayName(INMemberReference from, INMemberReference to)
        {
            return to.Name.CompareTo(from.Name);
        }
    }
}