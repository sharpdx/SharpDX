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
using System.Collections.Generic;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// The compiler dependency task class.
    /// TODO: COMMENT THIS CODE
    /// </summary>
    public class CompilerDependencyTask : Task
    {
        /// <summary>The task item class.</summary>
        protected class TkItem
        {
            /// <summary>The name.</summary>
            public string Name;

            /// <summary>The dynamic compiling.</summary>
            public bool DynamicCompiling;

            /// <summary>The link name.</summary>
            public string LinkName;

            /// <summary>The input file path.</summary>
            public string InputFilePath;

            /// <summary>The output file path.</summary>
            public string OutputFilePath;

            /// <summary>The output link.</summary>
            public string OutputLink;

            /// <summary>The output cs.</summary>
            public bool OutputCs;

            /// <summary>The output cs file.</summary>
            public string OutputCsFile;

            /// <summary>The output namespace.</summary>
            public string OutputNamespace;

            /// <summary>The output class name.</summary>
            public string OutputClassName;

            /// <summary>The output field name.</summary>
            public string OutputFieldName;

            /// <summary>The parent task item.</summary>
            public ITaskItem ParentTaskItem;

            /// <summary>Automatics the task item.</summary>
            /// <returns>TaskItem.</returns>
            public TaskItem ToTaskItem()
            {
                var item = new TaskItem(OutputFilePath);

                // For Fxo we output Link used by <Content> Item
                if (!OutputCs)
                {
                    item.SetMetadata("CopyToOutputDirectory", "PreserveNewest");
                    item.SetMetadata("Link", OutputLink);
                }
                return item;
            }

            /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return string.Format("Name: {0}, DynamicCompiling: {1}, LinkName: {2}, InputFilePath: {3}, OutputFilePath: {4}, OutputCsFile: {5}, OutputNamespace: {6}, OutputClassName: {7}, OutputFieldName: {8}, OutputCs: {9}", Name, DynamicCompiling, LinkName, InputFilePath, OutputFilePath, OutputCsFile, OutputNamespace, OutputClassName, OutputFieldName, OutputCs);
            }
        }

        /// <summary>Gets or sets the project directory.</summary>
        /// <value>The project directory.</value>
        [Required]
        public ITaskItem ProjectDirectory { get; set; }

        /// <summary>Gets or sets the intermediate directory.</summary>
        /// <value>The intermediate directory.</value>
        [Required]
        public ITaskItem IntermediateDirectory { get; set; }

        /// <summary>Gets or sets the files.</summary>
        /// <value>The files.</value>
        [Required]
        public ITaskItem[] Files { get; set; }

        /// <summary>Gets or sets the content files.</summary>
        /// <value>The content files.</value>
        [Output]
        public ITaskItem[] ContentFiles { get; set; }

        /// <summary>Gets or sets the compile files.</summary>
        /// <value>The compile files.</value>
        [Output]
        public ITaskItem[] CompileFiles { get; set; }

        /// <summary>Gets or sets a value indicating whether [dynamic compiling].</summary>
        /// <value><see langword="true" /> if [dynamic compiling]; otherwise, <see langword="false" />.</value>
        public bool DynamicCompiling { get; set; }

        /// <summary>Gets or sets a value indicating whether this <see cref="CompilerDependencyTask"/> is debug.</summary>
        /// <value><see langword="true" /> if debug; otherwise, <see langword="false" />.</value>
        public bool Debug { get; set; }

        /// <summary>Gets or sets the root namespace.</summary>
        /// <value>The root namespace.</value>
        public string RootNamespace { get; set; }

        /// <summary>Gets or sets the compiler flags.</summary>
        /// <value>The compiler flags.</value>
        public string CompilerFlags { get; set; }

        /// <summary>When overridden in a derived class, executes the task.</summary>
        /// <returns>true if the task successfully executed; otherwise, false.</returns>
        public sealed override bool Execute()
        {
            var hasErrors = false;
            try
            {
                Initialize();

                var contentFiles = new List<ITaskItem>();
                var compileFiles = new List<ITaskItem>();

                foreach (ITaskItem file in Files)
                {
                    var item = GetTkItem(file);

                    Log.LogMessage(MessageImportance.Low, "Process item {0}", item);

                    if (!ProcessItem(item))
                    {
                        hasErrors = true;
                    }

                    var returnedItem = item.ToTaskItem();

                    // Fxo are copied to output
                    if (item.OutputCs)
                    {
                        compileFiles.Add(returnedItem);
                    }
                    else
                    {
                        contentFiles.Add(returnedItem);
                    }
                }

                ContentFiles = contentFiles.ToArray();
                CompileFiles = compileFiles.ToArray();
            }
            catch (Exception ex)
            {
                Log.LogError("Unexpected exception: {0}", ex);
                hasErrors = true;
            }

            return !hasErrors;
        }

        /// <summary>Initializes this instance.</summary>
        protected virtual void Initialize() { }

        private TkItem GetTkItem(ITaskItem item)
        {

            var data = new TkItem
                       {
                           Name = item.ItemSpec,
                           DynamicCompiling = item.GetMetadata("DynamicCompiling", DynamicCompiling),
                           LinkName = item.GetMetadata("Link", item.ItemSpec),
                           OutputNamespace = item.GetMetadata("OutputNamespace", RootNamespace),
                           OutputFieldName = item.GetMetadata("OutputFieldName", "bytecode"),
                           OutputCs = item.GetMetadata("OutputCs", false),
                           ParentTaskItem = item
                       };

            data.OutputClassName = item.GetMetadata("OutputClassName", Path.GetFileNameWithoutExtension(data.LinkName));
            data.OutputCsFile = item.GetMetadata("OutputCsFileName", Path.GetFileNameWithoutExtension(data.LinkName) + ".Generated.cs");

            var outputCsFile = item.GetMetadata<string>("LastGenOutput", null);
            if (outputCsFile != null && outputCsFile.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
            {
                data.OutputCsFile = outputCsFile;
            }

            // Full path to the generated Output FilePath either 
            // For fxo: $(ProjectDir)/obj/Debug/XXX/YYY.fxo 
            // For cs: $(ProjectDir)/XXX/YYY.cs
            if (data.OutputCs)
            {
                data.OutputFilePath = Path.Combine(IntermediateDirectory.ItemSpec, Path.Combine(Path.GetDirectoryName(data.Name) ?? string.Empty, data.OutputCsFile));
            }
            else
            {
                data.OutputLink = Path.ChangeExtension(data.LinkName, "tkb");
                data.OutputFilePath = Path.Combine(IntermediateDirectory.ItemSpec, data.OutputLink);
            }

            // Prefix by ProjectDirectory
            data.OutputFilePath = Path.Combine(ProjectDirectory.ItemSpec, data.OutputFilePath);

            // Full path to the input file
            data.InputFilePath = Path.Combine(ProjectDirectory.ItemSpec, data.Name);

            return data;
        }

        /// <summary>Processes the item.</summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ProcessItem(TkItem item)
        {
            return true;
        }
    }
}