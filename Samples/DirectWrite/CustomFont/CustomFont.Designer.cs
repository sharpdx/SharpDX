namespace CustomFont
{
    partial class CustomFont
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
            this.renderControl = new SharpDX.Windows.RenderControl();
            this.comboBoxFonts = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // renderControl
            // 
            this.renderControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.renderControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.renderControl.Location = new System.Drawing.Point(9, 45);
            this.renderControl.Margin = new System.Windows.Forms.Padding(0);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(718, 506);
            this.renderControl.TabIndex = 0;
            // 
            // comboBoxFonts
            // 
            this.comboBoxFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFonts.FormattingEnabled = true;
            this.comboBoxFonts.Location = new System.Drawing.Point(12, 12);
            this.comboBoxFonts.Name = "comboBoxFonts";
            this.comboBoxFonts.Size = new System.Drawing.Size(230, 21);
            this.comboBoxFonts.TabIndex = 1;
            this.comboBoxFonts.SelectedIndexChanged += new System.EventHandler(this.comboBoxFonts_SelectedIndexChanged);
            // 
            // CustomFont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 560);
            this.Controls.Add(this.comboBoxFonts);
            this.Controls.Add(this.renderControl);
            this.Name = "CustomFont";
            this.Text = "SharpDX - DirectWrite CustomFont Sample";
            this.ResumeLayout(false);

        }

        #endregion

        private SharpDX.Windows.RenderControl renderControl;
        private System.Windows.Forms.ComboBox comboBoxFonts;
    }
}

