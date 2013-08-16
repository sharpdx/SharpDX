namespace ShaderLinking
{
    using System;
    using System.Windows.Forms;
    using SharpDX.Windows;

    internal partial class Form1 : Form
    {
        private readonly ShaderData _data;
        private readonly RenderControl _renderControl;

        public Form1(ShaderData data)
        {
            InitializeComponent();

            _data = data;

            // create the RenderControl
            _renderControl = new RenderControl();

            pnRenderControlPanel.Controls.Add(_renderControl);

            // initialize the controls state from initial data
            cbInvert.Checked = _data.EnableInvertColor;
            cbGrayscale.Checked = _data.EnableGrayscale;

            tbHeader.Text = _data.Header;
            tbSource.Text = _data.Source;
        }

        // Expose the render conttrol to the game class
        public RenderControl RenderControl { get { return _renderControl; } }

        private void BtnRebuildClick(object sender, EventArgs e)
        {
            // set the updated data, effect will be rebuild at next frame update
            _data.EnableInvertColor = cbInvert.Checked;
            _data.EnableGrayscale = cbGrayscale.Checked;
            _data.Source = tbSource.Text;
        }
    }
}
