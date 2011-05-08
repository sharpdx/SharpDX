namespace TextDialog
{
    partial class TextDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFont = new System.Windows.Forms.ComboBox();
            this.checkBoxBold = new System.Windows.Forms.CheckBox();
            this.checkBoxItalic = new System.Windows.Forms.CheckBox();
            this.checkBoxUnderline = new System.Windows.Forms.CheckBox();
            this.labelFontSize = new System.Windows.Forms.Label();
            this.trackBarFontSize = new System.Windows.Forms.TrackBar();
            this.renderControl = new SharpDX.Windows.RenderControl();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFontSize)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(16, 49);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(145, 20);
            this.textBox.TabIndex = 2;
            this.textBox.Text = "Formatted Text";
            this.textBox.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Font";
            // 
            // comboBoxFont
            // 
            this.comboBoxFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFont.FormattingEnabled = true;
            this.comboBoxFont.Location = new System.Drawing.Point(16, 88);
            this.comboBoxFont.Name = "comboBoxFont";
            this.comboBoxFont.Size = new System.Drawing.Size(145, 21);
            this.comboBoxFont.TabIndex = 4;
            this.comboBoxFont.SelectedIndexChanged += new System.EventHandler(this.ComboBoxFontSelectedIndexChanged);
            // 
            // checkBoxBold
            // 
            this.checkBoxBold.AutoSize = true;
            this.checkBoxBold.Location = new System.Drawing.Point(16, 115);
            this.checkBoxBold.Name = "checkBoxBold";
            this.checkBoxBold.Size = new System.Drawing.Size(47, 17);
            this.checkBoxBold.TabIndex = 5;
            this.checkBoxBold.Text = "Bold";
            this.checkBoxBold.UseVisualStyleBackColor = true;
            this.checkBoxBold.CheckedChanged += new System.EventHandler(this.CheckBoxBoldCheckedChanged);
            // 
            // checkBoxItalic
            // 
            this.checkBoxItalic.AutoSize = true;
            this.checkBoxItalic.Location = new System.Drawing.Point(16, 138);
            this.checkBoxItalic.Name = "checkBoxItalic";
            this.checkBoxItalic.Size = new System.Drawing.Size(48, 17);
            this.checkBoxItalic.TabIndex = 6;
            this.checkBoxItalic.Text = "Italic";
            this.checkBoxItalic.UseVisualStyleBackColor = true;
            this.checkBoxItalic.CheckedChanged += new System.EventHandler(this.CheckBoxItalicCheckedChanged);
            // 
            // checkBoxUnderline
            // 
            this.checkBoxUnderline.AutoSize = true;
            this.checkBoxUnderline.Location = new System.Drawing.Point(15, 161);
            this.checkBoxUnderline.Name = "checkBoxUnderline";
            this.checkBoxUnderline.Size = new System.Drawing.Size(71, 17);
            this.checkBoxUnderline.TabIndex = 7;
            this.checkBoxUnderline.Text = "Underline";
            this.checkBoxUnderline.UseVisualStyleBackColor = true;
            this.checkBoxUnderline.CheckedChanged += new System.EventHandler(this.CheckBoxUnderlineCheckedChanged);
            // 
            // labelFontSize
            // 
            this.labelFontSize.AutoSize = true;
            this.labelFontSize.Location = new System.Drawing.Point(12, 197);
            this.labelFontSize.Name = "labelFontSize";
            this.labelFontSize.Size = new System.Drawing.Size(51, 13);
            this.labelFontSize.TabIndex = 8;
            this.labelFontSize.Text = "Font Size";
            // 
            // trackBarFontSize
            // 
            this.trackBarFontSize.Location = new System.Drawing.Point(12, 213);
            this.trackBarFontSize.Maximum = 256;
            this.trackBarFontSize.Name = "trackBarFontSize";
            this.trackBarFontSize.Size = new System.Drawing.Size(146, 45);
            this.trackBarFontSize.TabIndex = 9;
            this.trackBarFontSize.TickFrequency = 8;
            this.trackBarFontSize.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarFontSize.Value = 108;
            this.trackBarFontSize.Scroll += new System.EventHandler(this.TrackBarFontSizeScroll);
            // 
            // renderControl
            // 
            this.renderControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.renderControl.Location = new System.Drawing.Point(167, 12);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(507, 500);
            this.renderControl.TabIndex = 0;
            // 
            // TextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 524);
            this.Controls.Add(this.trackBarFontSize);
            this.Controls.Add(this.labelFontSize);
            this.Controls.Add(this.checkBoxUnderline);
            this.Controls.Add(this.checkBoxItalic);
            this.Controls.Add(this.checkBoxBold);
            this.Controls.Add(this.comboBoxFont);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.renderControl);
            this.Name = "TextDialog";
            this.Text = "SharpDX - DirectWrite TextDialog Sample";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFontSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpDX.Windows.RenderControl renderControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxFont;
        private System.Windows.Forms.CheckBox checkBoxBold;
        private System.Windows.Forms.CheckBox checkBoxItalic;
        private System.Windows.Forms.CheckBox checkBoxUnderline;
        private System.Windows.Forms.Label labelFontSize;
        private System.Windows.Forms.TrackBar trackBarFontSize;
    }
}

