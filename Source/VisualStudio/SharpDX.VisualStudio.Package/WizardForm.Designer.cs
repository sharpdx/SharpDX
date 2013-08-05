namespace SharpDX.VisualStudio.ProjectWizard
{
    partial class WizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkPrimitive3d = new System.Windows.Forms.CheckBox();
            this.checkModel3d = new System.Windows.Forms.CheckBox();
            this.checkSpriteFont = new System.Windows.Forms.CheckBox();
            this.checkSpriteBatch = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBloomEffect = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkInputTouch = new System.Windows.Forms.CheckBox();
            this.checkInputMouse = new System.Windows.Forms.CheckBox();
            this.checkInputKeyboard = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCheckAll = new System.Windows.Forms.Button();
            this.buttonUncheckAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkPrimitive3d);
            this.groupBox1.Controls.Add(this.checkModel3d);
            this.groupBox1.Controls.Add(this.checkSpriteFont);
            this.groupBox1.Controls.Add(this.checkSpriteBatch);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 75);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Basic";
            // 
            // checkPrimitive3d
            // 
            this.checkPrimitive3d.AutoSize = true;
            this.checkPrimitive3d.Location = new System.Drawing.Point(93, 42);
            this.checkPrimitive3d.Name = "checkPrimitive3d";
            this.checkPrimitive3d.Size = new System.Drawing.Size(82, 17);
            this.checkPrimitive3d.TabIndex = 3;
            this.checkPrimitive3d.Tag = "$sharpdx_feature_primitive3d$";
            this.checkPrimitive3d.Text = "3D Primitive";
            this.checkPrimitive3d.UseVisualStyleBackColor = true;
            this.checkPrimitive3d.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // checkModel3d
            // 
            this.checkModel3d.AutoSize = true;
            this.checkModel3d.Location = new System.Drawing.Point(93, 19);
            this.checkModel3d.Name = "checkModel3d";
            this.checkModel3d.Size = new System.Drawing.Size(72, 17);
            this.checkModel3d.TabIndex = 2;
            this.checkModel3d.Tag = "$sharpdx_feature_model3d$";
            this.checkModel3d.Text = "3D Model";
            this.checkModel3d.UseVisualStyleBackColor = true;
            this.checkModel3d.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // checkSpriteFont
            // 
            this.checkSpriteFont.AutoSize = true;
            this.checkSpriteFont.Location = new System.Drawing.Point(6, 42);
            this.checkSpriteFont.Name = "checkSpriteFont";
            this.checkSpriteFont.Size = new System.Drawing.Size(74, 17);
            this.checkSpriteFont.TabIndex = 1;
            this.checkSpriteFont.Tag = "$sharpdx_feature_spritefont$";
            this.checkSpriteFont.Text = "SpriteFont";
            this.checkSpriteFont.UseVisualStyleBackColor = true;
            this.checkSpriteFont.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // checkSpriteBatch
            // 
            this.checkSpriteBatch.AutoSize = true;
            this.checkSpriteBatch.Location = new System.Drawing.Point(6, 19);
            this.checkSpriteBatch.Name = "checkSpriteBatch";
            this.checkSpriteBatch.Size = new System.Drawing.Size(81, 17);
            this.checkSpriteBatch.TabIndex = 0;
            this.checkSpriteBatch.Tag = "$sharpdx_feature_spritetexture$";
            this.checkSpriteBatch.Text = "SpriteBatch";
            this.checkSpriteBatch.UseVisualStyleBackColor = true;
            this.checkSpriteBatch.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBloomEffect);
            this.groupBox2.Location = new System.Drawing.Point(203, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 72);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Advanced";
            // 
            // checkBloomEffect
            // 
            this.checkBloomEffect.AutoSize = true;
            this.checkBloomEffect.Location = new System.Drawing.Point(6, 19);
            this.checkBloomEffect.Name = "checkBloomEffect";
            this.checkBloomEffect.Size = new System.Drawing.Size(86, 17);
            this.checkBloomEffect.TabIndex = 1;
            this.checkBloomEffect.Tag = "$sharpdx_feature_bloomeffect$";
            this.checkBloomEffect.Text = "Bloom Effect";
            this.checkBloomEffect.UseVisualStyleBackColor = true;
            this.checkBloomEffect.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkInputTouch);
            this.groupBox3.Controls.Add(this.checkInputMouse);
            this.groupBox3.Controls.Add(this.checkInputKeyboard);
            this.groupBox3.Location = new System.Drawing.Point(12, 93);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(185, 78);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Input";
            // 
            // checkInputTouch
            // 
            this.checkInputTouch.AutoSize = true;
            this.checkInputTouch.Location = new System.Drawing.Point(93, 19);
            this.checkInputTouch.Name = "checkInputTouch";
            this.checkInputTouch.Size = new System.Drawing.Size(57, 17);
            this.checkInputTouch.TabIndex = 4;
            this.checkInputTouch.Tag = "$sharpdx_feature_pointer$";
            this.checkInputTouch.Text = "Touch";
            this.checkInputTouch.UseVisualStyleBackColor = true;
            this.checkInputTouch.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // checkInputMouse
            // 
            this.checkInputMouse.AutoSize = true;
            this.checkInputMouse.Location = new System.Drawing.Point(6, 42);
            this.checkInputMouse.Name = "checkInputMouse";
            this.checkInputMouse.Size = new System.Drawing.Size(58, 17);
            this.checkInputMouse.TabIndex = 3;
            this.checkInputMouse.Tag = "$sharpdx_feature_mouse$";
            this.checkInputMouse.Text = "Mouse";
            this.checkInputMouse.UseVisualStyleBackColor = true;
            this.checkInputMouse.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // checkInputKeyboard
            // 
            this.checkInputKeyboard.AutoSize = true;
            this.checkInputKeyboard.Location = new System.Drawing.Point(6, 19);
            this.checkInputKeyboard.Name = "checkInputKeyboard";
            this.checkInputKeyboard.Size = new System.Drawing.Size(71, 17);
            this.checkInputKeyboard.TabIndex = 2;
            this.checkInputKeyboard.Tag = "$sharpdx_feature_keyboard$";
            this.checkInputKeyboard.Text = "Keyboard";
            this.checkInputKeyboard.UseVisualStyleBackColor = true;
            this.checkInputKeyboard.CheckedChanged += new System.EventHandler(this.features_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(203, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(185, 78);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Audio";
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(112, 177);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "&Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(275, 177);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonCheckAll
            // 
            this.buttonCheckAll.Location = new System.Drawing.Point(394, 25);
            this.buttonCheckAll.Name = "buttonCheckAll";
            this.buttonCheckAll.Size = new System.Drawing.Size(75, 23);
            this.buttonCheckAll.TabIndex = 7;
            this.buttonCheckAll.Text = "Check All";
            this.buttonCheckAll.UseVisualStyleBackColor = true;
            this.buttonCheckAll.Click += new System.EventHandler(this.buttonCheckAll_Click);
            // 
            // buttonUncheckAll
            // 
            this.buttonUncheckAll.Location = new System.Drawing.Point(394, 54);
            this.buttonUncheckAll.Name = "buttonUncheckAll";
            this.buttonUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.buttonUncheckAll.TabIndex = 8;
            this.buttonUncheckAll.Text = "Uncheck All";
            this.buttonUncheckAll.UseVisualStyleBackColor = true;
            this.buttonUncheckAll.Click += new System.EventHandler(this.buttonUncheckAll_Click);
            // 
            // WizardForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(484, 219);
            this.Controls.Add(this.buttonUncheckAll);
            this.Controls.Add(this.buttonCheckAll);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 258);
            this.MinimumSize = new System.Drawing.Size(500, 258);
            this.Name = "WizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select samples to include in the Project";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkPrimitive3d;
        private System.Windows.Forms.CheckBox checkModel3d;
        private System.Windows.Forms.CheckBox checkSpriteFont;
        private System.Windows.Forms.CheckBox checkSpriteBatch;
        private System.Windows.Forms.CheckBox checkBloomEffect;
        private System.Windows.Forms.CheckBox checkInputTouch;
        private System.Windows.Forms.CheckBox checkInputMouse;
        private System.Windows.Forms.CheckBox checkInputKeyboard;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCheckAll;
        private System.Windows.Forms.Button buttonUncheckAll;
    }
}