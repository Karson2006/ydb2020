namespace iTR.Tool
{
    partial class RSAForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RSAForm));
            this.publickeyBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.privatekeyBox = new System.Windows.Forms.TextBox();
            this.inputBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.encryptedBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.decryptedBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Keybutton = new System.Windows.Forms.Button();
            this.encryptButton = new System.Windows.Forms.Button();
            this.DecryptButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // publickeyBox
            // 
            this.publickeyBox.Location = new System.Drawing.Point(12, 23);
            this.publickeyBox.Multiline = true;
            this.publickeyBox.Name = "publickeyBox";
            this.publickeyBox.Size = new System.Drawing.Size(513, 61);
            this.publickeyBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "PublicKey:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "PrivateKey:";
            // 
            // privatekeyBox
            // 
            this.privatekeyBox.Location = new System.Drawing.Point(12, 102);
            this.privatekeyBox.Multiline = true;
            this.privatekeyBox.Name = "privatekeyBox";
            this.privatekeyBox.Size = new System.Drawing.Size(513, 60);
            this.privatekeyBox.TabIndex = 3;
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(12, 180);
            this.inputBox.Multiline = true;
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(513, 53);
            this.inputBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "InputText:";
            // 
            // encryptedBox
            // 
            this.encryptedBox.Location = new System.Drawing.Point(12, 253);
            this.encryptedBox.Multiline = true;
            this.encryptedBox.Name = "encryptedBox";
            this.encryptedBox.Size = new System.Drawing.Size(513, 60);
            this.encryptedBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "EncryptedText:";
            // 
            // decryptedBox
            // 
            this.decryptedBox.Location = new System.Drawing.Point(12, 335);
            this.decryptedBox.Multiline = true;
            this.decryptedBox.Name = "decryptedBox";
            this.decryptedBox.Size = new System.Drawing.Size(513, 62);
            this.decryptedBox.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "DecryptedText:";
            // 
            // Keybutton
            // 
            this.Keybutton.Location = new System.Drawing.Point(207, 403);
            this.Keybutton.Name = "Keybutton";
            this.Keybutton.Size = new System.Drawing.Size(75, 23);
            this.Keybutton.TabIndex = 10;
            this.Keybutton.Text = "Keys";
            this.Keybutton.UseVisualStyleBackColor = true;
            this.Keybutton.Click += new System.EventHandler(this.Keybutton_Click);
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(288, 403);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(75, 23);
            this.encryptButton.TabIndex = 11;
            this.encryptButton.Text = "Encrypt";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // DecryptButton
            // 
            this.DecryptButton.Location = new System.Drawing.Point(369, 403);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(75, 23);
            this.DecryptButton.TabIndex = 12;
            this.DecryptButton.Text = "Decrypt";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.Click += new System.EventHandler(this.DecryptButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(450, 403);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 13;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // RSAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 433);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.DecryptButton);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.Keybutton);
            this.Controls.Add(this.decryptedBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.encryptedBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.privatekeyBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.publickeyBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RSAForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RSA加密";
            this.Load += new System.EventHandler(this.RSAForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox publickeyBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox privatekeyBox;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox encryptedBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox decryptedBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Keybutton;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Button DecryptButton;
        private System.Windows.Forms.Button exitButton;
    }
}