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
using System.Windows.Forms;

using EnvDTE;

using Microsoft.VisualStudio.TemplateWizard;

namespace SharpDX.VisualStudio.ProjectWizard
{
    public class MainWizard : IWizard
    {
        private WizardForm wizardForm;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> props, WizardRunKind runKind, object[] customParams)
        {
            // Check that SharpDX is correctly installed
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SharpDXSdkDir")))
            {
                MessageBox.Show("Unable to find SharpDX installation directory. Expecting [SharpDXSdkDir] environment variable", "SharpDX Toolkit Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new WizardCancelledException("Expecting [SharpDXSdkDir] environment variable");
            }
            props.Add("$safeclassname$", props["$safeprojectname$"].Replace(".", string.Empty));

            //Call win form created in the project to accept user input
            wizardForm = new WizardForm { Properties = props };
            var result = wizardForm.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                throw new WizardCancelledException();
            }

            // Set spritebatch feature if spritetexture or spritefont is true
            if (GetKey(props, "$sharpdx_feature_spritetexture$") || GetKey(props, "$sharpdx_feature_spritefont$"))
            {
                wizardForm.Properties["$sharpdx_feature_spritebatch$"] = "true";
            }

            if (GetKey(props, "$sharpdx_feature_model3d$") || GetKey(props, "$sharpdx_feature_primitive3d$"))
            {
                wizardForm.Properties["$sharpdx_feature_3d$"] = "true";
            }
        }

        private bool GetKey(Dictionary<string, string> replacementsDictionary, string name)
        {
            string value;
            if (replacementsDictionary.TryGetValue(name, out value))
            {
                return value == "true";
            }
            return false;
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}