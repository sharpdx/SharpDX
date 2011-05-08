// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Drawing;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace CustomFont
{
    /// <summary>
    /// How to render fonts that are embedded as resources. This application implements
    /// a custom font file loader to provide access to the embedded font data. This
    /// allows DirectWrite to recognize the family name of the fonts even though these
    /// fonts are not installed on the system.
    /// 
    /// Port of DirectWrite sample CustomFont from Windows 7 SDK samples.
    /// http://msdn.microsoft.com/en-us/library/dd941710%28v=VS.85%29.aspx
    /// </summary>
    public partial class CustomFont : Form
    {
        public SharpDX.Direct2D1.Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }

        public TextFormat CurrentTextFormat { get; private set; }
        public TextLayout CurrentTextLayout { get; private set; }
        public ResourceFontLoader CurrentResourceFontLoader { get; set; }
        public FontCollection CurrentFontCollection { get; set; }

        public string FontFamilyName { get; set; }
        public string FontText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFont"/> class.
        /// </summary>
        public CustomFont()
        {
            InitializeComponent();

            try
            {
                InitDirect2DAndDirectWrite();
                InitCustomFont();
            }
            catch (Exception ex)
            {
                LogException(ex);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void LogException(Exception ex)
        {
            MessageBox.Show(this, string.Format("Unexpected error. Reason : {0}", ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Inits the direct2D and direct write.
        /// </summary>
        private void InitDirect2DAndDirectWrite()
        {
            Factory2D = new SharpDX.Direct2D1.Factory();
            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            var properties = new HwndRenderTargetProperties();
            properties.Hwnd = renderControl.Handle;
            properties.PixelSize = renderControl.ClientSize;
            properties.PresentOptions = PresentOptions.None;

            RenderTarget2D = new WindowRenderTarget(Factory2D, new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)), properties);
            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;
            RenderTarget2D.TextAntialiasMode = TextAntialiasMode.Cleartype;

            SceneColorBrush = new SolidColorBrush(RenderTarget2D, new Color4(1, 0, 0, 0));

            renderControl.Paint += new PaintEventHandler(renderControl_Paint);
            renderControl.Resize += new EventHandler(renderControl_Resize);
        }

        /// <summary>
        /// Inits the custom font.
        /// </summary>
        private void InitCustomFont()
        {
            CurrentResourceFontLoader = new ResourceFontLoader(FactoryDWrite);
            CurrentFontCollection = new FontCollection(FactoryDWrite, CurrentResourceFontLoader, CurrentResourceFontLoader.Key);
            comboBoxFonts.Items.Add("Pericles");
            comboBoxFonts.Items.Add("Kootenay");
            comboBoxFonts.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Resize event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void renderControl_Resize(object sender, EventArgs e)
        {
            try
            {
                RenderTarget2D.Resize(renderControl.Size);
                CurrentTextLayout.MaxWidth = renderControl.Size.Width;
                CurrentTextLayout.MaxHeight = renderControl.Size.Height;
                Refresh();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// Handles the Paint event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        void renderControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                RenderTarget2D.BeginDraw();

                RenderTarget2D.Clear(new Color4(1, 1, 1, 1));

                RenderTarget2D.DrawLine(new PointF(0, 0), new PointF(renderControl.ClientSize.Width, renderControl.ClientSize.Height), SceneColorBrush);
                RenderTarget2D.DrawLine(new PointF(0, renderControl.ClientSize.Height), new PointF(renderControl.ClientSize.Width, 0), SceneColorBrush);

                RenderTarget2D.DrawTextLayout(new PointF(0, 0), CurrentTextLayout, SceneColorBrush);

                RenderTarget2D.EndDraw();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// Updates the TextFormat and TextLayout.
        /// </summary>
        private void UpdateTextFormatAndLayout()
        {
            try
            {
                if (CurrentTextFormat != null)
                {
                    CurrentTextFormat.Release();
                    CurrentTextFormat = null;
                }

                if (CurrentTextLayout != null)
                {
                    CurrentTextLayout.Release();
                    CurrentTextLayout = null;
                }

                FontText = "SharpDX - This font was loaded from a resource";

                // Initialize a TextFormat
                CurrentTextFormat = new TextFormat(FactoryDWrite, FontFamilyName, CurrentFontCollection, FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 64) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };

                CurrentTextLayout = new TextLayout(FactoryDWrite, FontText, CurrentTextFormat, renderControl.ClientSize.Width, renderControl.ClientSize.Height);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxFonts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void comboBoxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FontFamilyName = comboBoxFonts.SelectedItem.ToString();
                UpdateTextFormatAndLayout();
                renderControl.Refresh();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

        }

    }
}
