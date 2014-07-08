// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
    /// TODO: COMMENT THIS CODE
    /// </summary>
    public class CompilerDependencyTask : Task
    {
        protected class TkItem
        {
            public string Name;

            public bool DynamicCompiling;

            public string LinkName;

            public string InputFilePath;

            public string OutputFilePath;

            public string OutputLink;

            public bool OutputCs;

            public string OutputCsFile;

            public string OutputNamespace;

            public string OutputClassName;

            public string OutputFieldName;

            public ITaskItem ParentTaskItem;

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

            public override string ToString()
            {
                return string.Format("Name: {0}, DynamicCompiling: {1}, LinkName: {2}, InputFilePath: {3}, OutputFilePath: {4}, OutputCsFile: {5}, OutputNamespace: {6}, OutputClassName: {7}, OutputFieldName: {8}, OutputCs: {9}", Name, DynamicCompiling, LinkName, InputFilePath, OutputFilePath, OutputCsFile, OutputNamespace, OutputClassName, OutputFieldName, OutputCs);
            }
        }

        [Required]
        public ITaskItem ProjectDirectory { get; set; }

        [Required]
        public ITaskItem IntermediateDirectory { get; set; }

        [Required]
        public ITaskItem[] Files { get; set; }

        [Output]
        public ITaskItem[] ContentFiles { get; set; }

        [Output]
        public ITaskItem[] CompileFiles { get; set; }

        public bool DynamicCompiling { get; set; }

        public bool Debug { get; set; }

        public string RootNamespace { get; set; }

        public string CompilerFlags { get; set; }

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

        protected virtual bool ProcessItem(TkItem item)
        {
            return true;
        }
    }
}