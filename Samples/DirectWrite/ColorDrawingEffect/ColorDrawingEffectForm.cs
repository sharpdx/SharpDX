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
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

using Factory = SharpDX.Direct2D1.Factory;

namespace ColorDrawingEffect
{
    /// <summary>
    /// Demonstrates client drawing effects with a text layout object and a custom text renderer.
    ///  
    /// SharpDX Port of DirectWrite  Color Drawing Effect Example from Windows 7 SDK samples. 
    /// Thanks to Fadi Alsamman for this port.
    /// http://msdn.microsoft.com/en-us/library/dd941709%28v=vs.85%29.aspx
    /// </summary>
    public partial class ColorDrawingEffectForm : Form
    {
        public Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }

        public string FontFamilyName { get; set; }
        public float FontSize { get; set; }
        public string FontText { get; set; }

        public TextFormat CurrentTextFormat { get; private set; }
        public TextLayout CurrentTextLayout { get; private set; }
        public TextRange CurrentTextRange { get { return new TextRange(0, FontText.Length); } }

        private ColorDrawingEffect RedDrawingeffect { get; set; }
        private ColorDrawingEffect BlueDrawingEffect { get; set; }
        private ColorDrawingEffect GreenDrawingEffect { get; set; }

        private CustomTextRenderer CustomTextRenderer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorDrawingEffectForm"/> class.
        /// </summary>
        public ColorDrawingEffectForm()
        {
            InitializeComponent();

            try
            {
                InitDirect2DAndDirectWrite();
                InitTextFormatLayout();
            }
            catch (Exception ex)
            {
                LogException(ex);
                Environment.Exit(1);
            }

            Paint += RenderControlPaint;
            Resize += RenderControlResize;
        }

        /// <summary>
        /// Inits the direct2D and direct write.
        /// </summary>
        private void InitDirect2DAndDirectWrite()
        {
            Factory2D = new SharpDX.Direct2D1.Factory();
            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            var properties = new HwndRenderTargetProperties {Hwnd = Handle, PixelSize = ClientSize, PresentOptions = PresentOptions.None};

            RenderTarget2D = new WindowRenderTarget(Factory2D, new RenderTargetProperties(), properties)
                                 {
                                     AntialiasMode = AntialiasMode.PerPrimitive,
                                     TextAntialiasMode = TextAntialiasMode.Cleartype
                                 };

            SceneColorBrush = new SolidColorBrush(RenderTarget2D, new Color4(1, 0, 0, 0));

            CustomTextRenderer = new CustomTextRenderer(Factory2D, RenderTarget2D);
        
        }


        /// <summary>
        /// Inits the font family names from DirectWrite
        /// </summary>
        private void InitTextFormatLayout()
        {
            FontFamilyName = "Gabriola";
            FontSize = 72;
            FontText = "Client Drawing Effect Example!";

            // Initialize a TextFormat
            CurrentTextFormat = new TextFormat(FactoryDWrite, FontFamilyName, FontSize) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };

            CurrentTextLayout = new TextLayout(FactoryDWrite, FontText, CurrentTextFormat, ClientRectangle.Width, ClientRectangle.Height);

            RedDrawingeffect = new ColorDrawingEffect(new Color4(1, 1, 0, 0));
            BlueDrawingEffect = new ColorDrawingEffect(new Color4(1, 0, 0, 1));
            GreenDrawingEffect = new ColorDrawingEffect(new Color4(1, 0, 1, 0));

            CurrentTextLayout.SetDrawingEffect(RedDrawingeffect, new TextRange(0, 14));
            CurrentTextLayout.SetDrawingEffect(BlueDrawingEffect, new TextRange(14, 7));
            CurrentTextLayout.SetDrawingEffect(GreenDrawingEffect, new TextRange(21, 8));

            // Set a stylistic typography
            var typo = new Typography(FactoryDWrite);
            typo.AddFontFeature(new FontFeature(FontFeatureTag.StylisticSet7, 1));
            CurrentTextLayout.SetTypography(typo, CurrentTextRange);
            typo.Release();

        }

    private void LogException(Exception ex)
        {
            MessageBox.Show(this, string.Format("Unable to use the font {0}. Reason : {1}", FontFamilyName, ex), "Error while setting text layout", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

     /// <summary>
        /// Handles the Resize event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void RenderControlResize(object sender, System.EventArgs e)
        {
            try
            {
                RenderTarget2D.Resize(ClientRectangle.Size);
                CurrentTextLayout.MaxWidth = ClientRectangle.Size.Width;
                CurrentTextLayout.MaxHeight = ClientRectangle.Size.Height;
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
        void RenderControlPaint(object sender, PaintEventArgs e)
        {
            try
            {
                RenderTarget2D.BeginDraw();

                RenderTarget2D.Clear(new Color4(1, 1, 1, 1));

                //RenderTarget2D.DrawTextLayout(new PointF(0, 0), CurrentTextLayout, SceneColorBrush);

                CurrentTextLayout.Draw(CustomTextRenderer, 0, 0);


                RenderTarget2D.EndDraw();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }
    }
}
