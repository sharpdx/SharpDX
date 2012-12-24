namespace MiniCube
{
    partial class MultiControlForm
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
            this.renderControl1 = new SharpDX.Windows.RenderControl();
            this.renderControl3 = new SharpDX.Windows.RenderControl();
            this.renderControl2 = new SharpDX.Windows.RenderControl();
            this.SuspendLayout();
            // 
            // renderControl1
            // 
            this.renderControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.renderControl1.Location = new System.Drawing.Point(0, 0);
            this.renderControl1.Name = "renderControl1";
            this.renderControl1.Size = new System.Drawing.Size(529, 282);
            this.renderControl1.TabIndex = 0;
            // 
            // renderControl3
            // 
            this.renderControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.renderControl3.Location = new System.Drawing.Point(0, 485);
            this.renderControl3.Name = "renderControl3";
            this.renderControl3.Size = new System.Drawing.Size(529, 248);
            this.renderControl3.TabIndex = 1;
            // 
            // renderControl2
            // 
            this.renderControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderControl2.Location = new System.Drawing.Point(0, 282);
            this.renderControl2.Name = "renderControl2";
            this.renderControl2.Size = new System.Drawing.Size(529, 203);
            this.renderControl2.TabIndex = 2;
            // 
            // MultiControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 733);
            this.Controls.Add(this.renderControl2);
            this.Controls.Add(this.renderControl3);
            this.Controls.Add(this.renderControl1);
            this.Name = "MultiControlForm";
            this.Text = "MultiControlForm";
            this.Load += new System.EventHandler(this.MultiControlForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SharpDX.Windows.RenderControl renderControl1;
        private SharpDX.Windows.RenderControl renderControl3;
        private SharpDX.Windows.RenderControl renderControl2;
    }
}