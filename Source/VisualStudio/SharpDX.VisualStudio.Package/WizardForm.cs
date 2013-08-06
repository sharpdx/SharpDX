using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpDX.VisualStudio.ProjectWizard
{
    public partial class WizardForm : Form
    {
        public Dictionary<string, string> Properties { get; private set; }

        private WizardForm()
        {
            InitializeComponent();
        }

        public WizardForm(Dictionary<string, string> properties) : this()
        {
            Properties = properties;
            radioButtonPlatformDesktop.Checked = true;
        }

        private void features_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;

            Properties[(string)checkbox.Tag] = checkbox.Checked.ToString().ToLowerInvariant();

            // If We are using the bloom effect, It requires to have at least one basic effect setup
            if (checkBloomEffect.Checked &&
                !checkModel3d.Checked &&
                !checkPrimitive3d.Checked &&
                !checkSpriteBatch.Checked &&
                !checkSpriteFont.Checked)
            {
                // By default 
                checkModel3d.Checked = true;
            }

            if ((checkInputKeyboard.Checked || checkInputMouse.Checked || checkInputTouch.Checked) &&
                !checkSpriteFont.Checked)
            {
                // By default 
                checkSpriteFont.Checked = true;
            }
        }

        private void platform_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = (RadioButton)sender;

            var platform = (string)radioButton.Tag;
            Properties[platform] = radioButton.Checked.ToString().ToLowerInvariant();

            var isNotPlatformWP8 = !platform.Contains("wp8");
            if (!isNotPlatformWP8)
            {
                checkInputKeyboard.Checked = false;
                checkInputMouse.Checked = false;
            }
            checkInputMouse.Enabled = isNotPlatformWP8;
            checkInputKeyboard.Enabled = isNotPlatformWP8;            
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            CheckUncheckAll(true);
        }

        private void buttonUncheckAll_Click(object sender, EventArgs e)
        {
            CheckUncheckAll(false);
        }

        private void CheckUncheckAll(bool state)
        {
            checkBloomEffect.Checked = state;
            checkInputKeyboard.Checked = state;
            checkInputMouse.Checked = state;
            checkInputTouch.Checked = state;

            checkSpriteFont.Checked = state;
            checkSpriteBatch.Checked = state;
            checkPrimitive3d.Checked = state;
            checkModel3d.Checked = state;
        }
    }
}
