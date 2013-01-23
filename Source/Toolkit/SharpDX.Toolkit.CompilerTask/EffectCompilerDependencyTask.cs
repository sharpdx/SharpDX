using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SharpDX.Toolkit
{
    public class EffectCompilerDependencyTask : Task
    {
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

        protected class TkFxcItem
        {
            public string FxName;

            public bool DynamicCompiling;

            public string LinkName;

            public string InputFilePath;

            public string OutputFilePath;

            public string OutputLink;

            public bool   OutputCs;

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
                return string.Format("FxName: {0}, DynamicCompiling: {1}, LinkName: {2}, InputFilePath: {3}, OutputFilePath: {4}, OutputCsFile: {5}, OutputNamespace: {6}, OutputClassName: {7}, OutputFieldName: {8}, OutputCs: {9}", FxName, DynamicCompiling, LinkName, InputFilePath, OutputFilePath, OutputCsFile, OutputNamespace, OutputClassName, OutputFieldName, OutputCs);
            }
        }

        public override bool Execute()
        {
            var contentFiles = new List<ITaskItem>();
            var compileFiles = new List<ITaskItem>();

            var hasErrors = false;

            foreach (ITaskItem file in Files)
            {
                var item = GetTkFxcItem(file);

                Log.LogMessage(MessageImportance.High, "Process TkFxc item {0}", item);

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

            return !hasErrors;
        }

        protected virtual bool ProcessItem(TkFxcItem item)
        {
            return true;
        }

        protected TkFxcItem GetTkFxcItem(ITaskItem item)
        {

            var data = new TkFxcItem
                           {
                               FxName = item.ItemSpec,
                               DynamicCompiling = item.GetMetadata("DynamicCompiling", DynamicCompiling),
                               LinkName = item.GetMetadata("Link", item.ItemSpec),
                               OutputNamespace = item.GetMetadata("OutputNamespace", RootNamespace),
                               OutputFieldName = item.GetMetadata("OutputFieldName", "bytecode"),
                               OutputCs =  item.GetMetadata("OutputCs", false)
                           };
            data.OutputClassName = item.GetMetadata("OutputClassName", Path.GetFileNameWithoutExtension(data.LinkName));
            data.OutputCsFile = item.GetMetadata("OutputCsFileName", Path.GetFileNameWithoutExtension(data.LinkName) + ".Generated.cs");

            var outputCsFile = item.GetMetadata<string>("LastGenOutput", null);
            if (outputCsFile != null && outputCsFile.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
            {
                data.OutputCsFile = outputCsFile;
            }

            // Fullpath to the generated Output FilePath either 
            // For fxo: $(ProjectDir)/obj/Debug/XXX/YYY.fxo 
            // For cs: $(ProjectDir)/XXX/YYY.cs
            if (data.OutputCs)
            {
                data.OutputFilePath = Path.Combine(IntermediateDirectory.ItemSpec, Path.Combine(Path.GetDirectoryName(data.FxName) ?? string.Empty, data.OutputCsFile));
            }
            else
            {
                data.OutputLink = Path.ChangeExtension(data.LinkName, "fxo");
                data.OutputFilePath = Path.Combine(IntermediateDirectory.ItemSpec, data.OutputLink);
            }

            // Prefix by ProjectDirectory
            data.OutputFilePath = Path.Combine(ProjectDirectory.ItemSpec, data.OutputFilePath);

            // Fullpath to the input file
            data.InputFilePath = Path.Combine(ProjectDirectory.ItemSpec, data.FxName);

            return data;
        }
    }
}