using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniCube
{
    public partial class MultiControlForm : Form
    {
        public MultiControlForm()
        {
            InitializeComponent();
        }

        public Control RenderControl1
        {
            get
            {
                return renderControl1;
            }
        }

        public Control RenderControl2
        {
            get
            {
                return renderControl2;
            }
        }

        public Control RenderControl3
        {
            get
            {
                return renderControl3;
            }
        }


        private void MultiControlForm_Load(object sender, EventArgs e)
        {

        }
    }
}
