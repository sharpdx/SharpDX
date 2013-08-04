using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDX.VisualStudio.ProjectWizard
{
    public partial class WizardForm : Form
    {
        public Dictionary<string, string> Properties { get; set; }

        public WizardForm()
        {
            InitializeComponent();
        }

        private void features_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = ((CheckBox)sender);
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
        }
    }
}
