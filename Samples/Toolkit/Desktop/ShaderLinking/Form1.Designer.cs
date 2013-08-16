namespace ShaderLinking
{
    internal partial class Form1
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
            this.gbComposition = new System.Windows.Forms.GroupBox();
            this.btnRebuild = new System.Windows.Forms.Button();
            this.cbGrayscale = new System.Windows.Forms.CheckBox();
            this.cbInvert = new System.Windows.Forms.CheckBox();
            this.gbHeader = new System.Windows.Forms.GroupBox();
            this.tbHeader = new System.Windows.Forms.TextBox();
            this.gbSource = new System.Windows.Forms.GroupBox();
            this.tbSource = new System.Windows.Forms.TextBox();
            this.pnRenderControlPanel = new System.Windows.Forms.Panel();
            this.gbComposition.SuspendLayout();
            this.gbHeader.SuspendLayout();
            this.gbSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbComposition
            // 
            this.gbComposition.Controls.Add(this.btnRebuild);
            this.gbComposition.Controls.Add(this.cbGrayscale);
            this.gbComposition.Controls.Add(this.cbInvert);
            this.gbComposition.Location = new System.Drawing.Point(13, 13);
            this.gbComposition.Name = "gbComposition";
            this.gbComposition.Size = new System.Drawing.Size(468, 65);
            this.gbComposition.TabIndex = 0;
            this.gbComposition.TabStop = false;
            this.gbComposition.Text = "Composition";
            // 
            // btnRebuild
            // 
            this.btnRebuild.Location = new System.Drawing.Point(308, 19);
            this.btnRebuild.Name = "btnRebuild";
            this.btnRebuild.Size = new System.Drawing.Size(154, 40);
            this.btnRebuild.TabIndex = 2;
            this.btnRebuild.Text = "Rebuild";
            this.btnRebuild.UseVisualStyleBackColor = true;
            this.btnRebuild.Click += new System.EventHandler(this.BtnRebuildClick);
            // 
            // cbGrayscale
            // 
            this.cbGrayscale.AutoSize = true;
            this.cbGrayscale.Location = new System.Drawing.Point(7, 44);
            this.cbGrayscale.Name = "cbGrayscale";
            this.cbGrayscale.Size = new System.Drawing.Size(73, 17);
            this.cbGrayscale.TabIndex = 1;
            this.cbGrayscale.Text = "Grayscale";
            this.cbGrayscale.UseVisualStyleBackColor = true;
            // 
            // cbInvert
            // 
            this.cbInvert.AutoSize = true;
            this.cbInvert.Location = new System.Drawing.Point(7, 20);
            this.cbInvert.Name = "cbInvert";
            this.cbInvert.Size = new System.Drawing.Size(79, 17);
            this.cbInvert.TabIndex = 0;
            this.cbInvert.Text = "Invert color";
            this.cbInvert.UseVisualStyleBackColor = true;
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.tbHeader);
            this.gbHeader.Location = new System.Drawing.Point(13, 84);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(468, 104);
            this.gbHeader.TabIndex = 1;
            this.gbHeader.TabStop = false;
            this.gbHeader.Text = "Header";
            // 
            // tbHeader
            // 
            this.tbHeader.Location = new System.Drawing.Point(7, 20);
            this.tbHeader.Multiline = true;
            this.tbHeader.Name = "tbHeader";
            this.tbHeader.ReadOnly = true;
            this.tbHeader.Size = new System.Drawing.Size(455, 78);
            this.tbHeader.TabIndex = 0;
            // 
            // gbSource
            // 
            this.gbSource.Controls.Add(this.tbSource);
            this.gbSource.Location = new System.Drawing.Point(13, 194);
            this.gbSource.Name = "gbSource";
            this.gbSource.Size = new System.Drawing.Size(468, 271);
            this.gbSource.TabIndex = 2;
            this.gbSource.TabStop = false;
            this.gbSource.Text = "Source";
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(7, 20);
            this.tbSource.Multiline = true;
            this.tbSource.Name = "tbSource";
            this.tbSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSource.Size = new System.Drawing.Size(455, 245);
            this.tbSource.TabIndex = 0;
            // 
            // pnRenderControlPanel
            // 
            this.pnRenderControlPanel.Location = new System.Drawing.Point(488, 13);
            this.pnRenderControlPanel.Name = "pnRenderControlPanel";
            this.pnRenderControlPanel.Size = new System.Drawing.Size(666, 452);
            this.pnRenderControlPanel.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1166, 478);
            this.Controls.Add(this.pnRenderControlPanel);
            this.Controls.Add(this.gbSource);
            this.Controls.Add(this.gbHeader);
            this.Controls.Add(this.gbComposition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.gbComposition.ResumeLayout(false);
            this.gbComposition.PerformLayout();
            this.gbHeader.ResumeLayout(false);
            this.gbHeader.PerformLayout();
            this.gbSource.ResumeLayout(false);
            this.gbSource.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbComposition;
        private System.Windows.Forms.CheckBox cbGrayscale;
        private System.Windows.Forms.CheckBox cbInvert;
        private System.Windows.Forms.GroupBox gbHeader;
        private System.Windows.Forms.GroupBox gbSource;
        private System.Windows.Forms.Panel pnRenderControlPanel;
        private System.Windows.Forms.Button btnRebuild;
        private System.Windows.Forms.TextBox tbHeader;
        private System.Windows.Forms.TextBox tbSource;
    }
}

