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
using System.Reflection;
using System.Windows.Forms;

using EnvDTE;

using EnvDTE80;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using NuGet.VisualStudio;

namespace SharpDX.VisualStudio.ProjectWizard
{
    /// <summary>
    /// Main wizard class called by the SharpDX project template.
    /// </summary>
    public class MainWizard : IWizard
    {
        private WizardForm wizardForm;

        private EnvDTE._DTE dte;

        private IWizard winRTCertificateWizard;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            if (winRTCertificateWizard != null) winRTCertificateWizard.BeforeOpeningFile(projectItem);
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (winRTCertificateWizard != null) winRTCertificateWizard.ProjectFinishedGenerating(project);

            try
            {
                var services = new ServiceProvider(dte as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
                var componentModel = (IComponentModel)services.GetService(typeof(SComponentModel));
                var packageManager = componentModel.GetService<IVsPackageInstaller>();

                packageManager.InstallPackage(null, project, "SharpDX.Toolkit.Game", (string)null, false);
                packageManager.InstallPackage(null, project, "SharpDX.Toolkit.Input", (string)null, false);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to install nuget SharpDX.Toolkit.Game package. Please install it manually", "Error installing SharpDX nuget package", MessageBoxButtons.OK);
            }
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            if (winRTCertificateWizard != null) winRTCertificateWizard.ProjectItemFinishedGenerating(projectItem);
        }

        public void RunFinished()
        {
            if (winRTCertificateWizard != null) winRTCertificateWizard.RunFinished();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> props, WizardRunKind runKind, object[] customParams)
        {
            this.dte = automationObject as EnvDTE._DTE;

            props.Add("$safeclassname$", props["$safeprojectname$"].Replace(".", string.Empty));
            props["$currentVsCulture$"] = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;

            //Call win form created in the project to accept user input
            wizardForm = new WizardForm(props);
            var result = wizardForm.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                throw new WizardCancelledException();
            }

            // Hack on WinRT / WinRT XAML to run the certificate wizards as well as our own wizard
            if (GetKey(props, "$sharpdx_platform_winrt$") || GetKey(props, "$sharpdx_platform_winrt_xaml$"))
            {
                try
                {
                    var assembly = Assembly.Load("Microsoft.VisualStudio.WinRT.TemplateWizards, Version=11.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
                    var type = assembly.GetType("Microsoft.VisualStudio.WinRT.TemplateWizards.CreateProjectCertificate.Wizard");
                    winRTCertificateWizard = (IWizard)Activator.CreateInstance(type);
                }
                catch (Exception ex)
                {
                }
            }
            if (winRTCertificateWizard != null) winRTCertificateWizard.RunStarted(automationObject, props, runKind, customParams);

            // Set spritebatch feature if spritetexture or spritefont is true
            if (GetKey(props, "$sharpdx_feature_spritetexture$") || GetKey(props, "$sharpdx_feature_spritefont$"))
            {
                props["$sharpdx_feature_spritebatch$"] = "true";
            }

            if (GetKey(props, "$sharpdx_feature_model3d$") || GetKey(props, "$sharpdx_feature_primitive3d$"))
            {
                props["$sharpdx_feature_3d$"] = "true";
            }

            if (GetKey(props, "$sharpdx_platform_winrt_xaml$"))
            {
                props["$sharpdx_platform_winrt$"] = "true";
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