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
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;

using Factory = SharpDX.Direct2D1.Factory;
using FontStyle = SharpDX.DirectWrite.FontStyle;

namespace TextDialog
{
    /// <summary>
    /// Shows font enumeration, changing of font family, size, weight, slope, and the displayed string of DirectWrite text with changes rendered in real time.
    /// Port of DirectWrite sample TextDialogSample from Windows 7 SDK samples.
    /// http://msdn.microsoft.com/en-us/library/dd742748%28v=VS.85%29.aspx
    /// </summary>
    public partial class TextDialog : Form
    {
        public Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public WindowRenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }

        public string FontFamilyName { get { return comboBoxFont.SelectedItem.ToString();  } }
        public float FontSize { get { return trackBarFontSize.Value; } }
        public string FontText { get { return textBox.Text; } }
        public bool FontIsItalic { get { return checkBoxItalic.Checked; } }
        public bool FontIsBold { get { return checkBoxBold.Checked; } }
        public bool FontIsUnderline { get { return checkBoxUnderline.Checked; } }

        public TextFormat CurrentTextFormat { get; private set; }
        public TextLayout CurrentTextLayout { get; private set; }
        public TextRange CurrentTextRange { get { return new TextRange(0, FontText.Length); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDialog"/> class.
        /// </summary>
        public TextDialog()
        {
            InitializeComponent();

            try
            {
                InitDirect2DAndDirectWrite();
                InitFontFamilyNames();
                UpdateTextFormatAndLayout();
            }
            catch (Exception ex)
            {
                LogException(ex);
                Environment.Exit(1);
            }

            renderControl.Paint += RenderControlPaint;
            renderControl.Resize += new System.EventHandler(RenderControlResize);
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
        }

        /// <summary>
        /// Inits the font family names from DirectWrite
        /// </summary>
        private void InitFontFamilyNames()
        {
            var fontCollection = FactoryDWrite.GetSystemFontCollection(false);
            var familyCount = fontCollection.FontFamilyCount;
            List<string> names = new List<string>();
            for (int i = 0; i < familyCount; i++)
            {
                var fontFamily = fontCollection.GetFontFamily(i);
                var familyNames = fontFamily.FamilyNames;
                int index;

                if (!familyNames.FindLocaleName(CultureInfo.CurrentCulture.Name, out index))
                    familyNames.FindLocaleName("en-us", out index);

                names.Add(familyNames.GetString(index));     
            }

            names.Sort();
            comboBoxFont.Items.AddRange(names.ToArray());

            comboBoxFont.SelectedItem = "Gabriola";
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

                // Initialize a TextFormat
                CurrentTextFormat = new TextFormat(FactoryDWrite, FontFamilyName, FontSize) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };

                CurrentTextLayout = new TextLayout(FactoryDWrite, FontText, CurrentTextFormat, renderControl.Width, renderControl.Height);

                // Set a stylistic typography
                var typo = new Typography(FactoryDWrite);
                typo.AddFontFeature(new FontFeature(FontFeatureTag.StylisticSet7, 1));
                CurrentTextLayout.SetTypography(typo, CurrentTextRange);
                typo.Release();

                UpdateBold();
                UpdateItalic();
                UpdateUnderline();
                UpdateFontSize();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }


        private void LogException(Exception ex)
        {
            MessageBox.Show(this, string.Format("Unable to use the font {0}. Reason : {1}", FontFamilyName, ex), "Error while setting text layout", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Updates the bold.
        /// </summary>
        private void UpdateBold()
        {
            try
            {
                CurrentTextLayout.SetFontWeight(FontIsBold ? FontWeight.Bold : FontWeight.Normal, CurrentTextRange);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }


        /// <summary>
        /// Updates the italic.
        /// </summary>
        private void UpdateItalic()
        {
            try
            {
                CurrentTextLayout.SetFontStyle(FontIsItalic ? FontStyle.Italic : FontStyle.Normal, CurrentTextRange);
            }
            catch (Exception ex)
            {
                LogException(ex);                
            }
        }

        /// <summary>
        /// Updates the underline.
        /// </summary>
        private void UpdateUnderline()
        {
            try
            {
                CurrentTextLayout.SetUnderline(FontIsUnderline, CurrentTextRange);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// Updates the size of the font.
        /// </summary>
        private void UpdateFontSize()
        {
            try
            {
                CurrentTextLayout.SetFontSize(FontSize, CurrentTextRange);
            }
            catch (Exception ex)
            {
                LogException(ex);                
            }
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
                RenderTarget2D.Resize(renderControl.Size);
                CurrentTextLayout.MaxWidth = renderControl.Size.Width;
                CurrentTextLayout.MaxHeight = renderControl.Size.Height;
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

                RenderTarget2D.DrawTextLayout(new PointF(0, 0), CurrentTextLayout, SceneColorBrush);

                RenderTarget2D.EndDraw();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxFont control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ComboBoxFontSelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateTextFormatAndLayout();
            Refresh();
        }

        /// <summary>
        /// Handles the TextChanged event of the textBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TextBoxTextChanged(object sender, System.EventArgs e)
        {
            UpdateTextFormatAndLayout();
            Refresh();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxBold control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxBoldCheckedChanged(object sender, System.EventArgs e)
        {
            UpdateBold();
            Refresh();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxItalic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxItalicCheckedChanged(object sender, System.EventArgs e)
        {
            UpdateItalic();
            Refresh();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxUnderline control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckBoxUnderlineCheckedChanged(object sender, System.EventArgs e)
        {
            UpdateUnderline();
            Refresh();
        }

        /// <summary>
        /// Handles the Scroll event of the trackBarFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TrackBarFontSizeScroll(object sender, System.EventArgs e)
        {
            labelFontSize.Text = "Font Size " + FontSize;
            UpdateFontSize();
            Refresh();
        }
    }
}
