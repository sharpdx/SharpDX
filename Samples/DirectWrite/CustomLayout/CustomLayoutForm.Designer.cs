namespace CustomLayout
{
    partial class CustomLayoutForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miLatinText = new System.Windows.Forms.ToolStripMenuItem();
            this.miArabicText = new System.Windows.Forms.ToolStripMenuItem();
            this.miJapaneesText = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miCircleShape = new System.Windows.Forms.ToolStripMenuItem();
            this.miFunnelShape = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miNominalNumbers = new System.Windows.Forms.ToolStripMenuItem();
            this.miArabicContextual = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(768, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLatinText,
            this.miArabicText,
            this.miJapaneesText,
            this.toolStripMenuItem1,
            this.miCircleShape,
            this.miFunnelShape,
            this.toolStripMenuItem2,
            this.miNominalNumbers,
            this.miArabicContextual,
            this.toolStripMenuItem3,
            this.miExit});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // miLatinText
            // 
            this.miLatinText.Name = "miLatinText";
            this.miLatinText.Size = new System.Drawing.Size(216, 22);
            this.miLatinText.Text = "Latin text";
            this.miLatinText.Click += new System.EventHandler(this.miLatinText_Click);
            // 
            // miArabicText
            // 
            this.miArabicText.Name = "miArabicText";
            this.miArabicText.Size = new System.Drawing.Size(216, 22);
            this.miArabicText.Text = "Arabic text";
            this.miArabicText.Click += new System.EventHandler(this.miArabicText_Click);
            // 
            // miJapaneesText
            // 
            this.miJapaneesText.Name = "miJapaneesText";
            this.miJapaneesText.Size = new System.Drawing.Size(216, 22);
            this.miJapaneesText.Text = "Japanees text";
            this.miJapaneesText.Click += new System.EventHandler(this.miJapaneesText_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(213, 6);
            // 
            // miCircleShape
            // 
            this.miCircleShape.Name = "miCircleShape";
            this.miCircleShape.Size = new System.Drawing.Size(216, 22);
            this.miCircleShape.Text = "Circle shape";
            this.miCircleShape.Click += new System.EventHandler(this.miCircleShape_Click);
            // 
            // miFunnelShape
            // 
            this.miFunnelShape.Name = "miFunnelShape";
            this.miFunnelShape.Size = new System.Drawing.Size(216, 22);
            this.miFunnelShape.Text = "Funnel shape";
            this.miFunnelShape.Click += new System.EventHandler(this.miFunnelShape_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(213, 6);
            // 
            // miNominalNumbers
            // 
            this.miNominalNumbers.Name = "miNominalNumbers";
            this.miNominalNumbers.Size = new System.Drawing.Size(216, 22);
            this.miNominalNumbers.Text = "Nominal numbers";
            this.miNominalNumbers.Click += new System.EventHandler(this.miNominalNumbers_Click);
            // 
            // miArabicContextual
            // 
            this.miArabicContextual.Name = "miArabicContextual";
            this.miArabicContextual.Size = new System.Drawing.Size(216, 22);
            this.miArabicContextual.Text = "Arabic contextual numbers";
            this.miArabicContextual.Click += new System.EventHandler(this.miArabicContextual_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(213, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(216, 22);
            this.miExit.Text = "Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(768, 542);
            this.panel1.TabIndex = 1;
            // 
            // CustomLayoutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 566);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CustomLayoutForm";
            this.Text = "SharpDX - DirectWrite Custom Layout Sample";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miLatinText;
        private System.Windows.Forms.ToolStripMenuItem miArabicText;
        private System.Windows.Forms.ToolStripMenuItem miJapaneesText;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miCircleShape;
        private System.Windows.Forms.ToolStripMenuItem miFunnelShape;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem miNominalNumbers;
        private System.Windows.Forms.ToolStripMenuItem miArabicContextual;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.Panel panel1;
    }
}

