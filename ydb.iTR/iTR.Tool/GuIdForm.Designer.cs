namespace iTR.Tool
{
    partial class GuIdForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GuIdForm));
            this.Generate = new System.Windows.Forms.Button();
            this.calssExit = new System.Windows.Forms.Button();
            this.number = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guidBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Generate
            // 
            this.Generate.Location = new System.Drawing.Point(260, 269);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(75, 23);
            this.Generate.TabIndex = 10;
            this.Generate.Text = "Generate";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.Generate_Click);
            // 
            // calssExit
            // 
            this.calssExit.Location = new System.Drawing.Point(344, 269);
            this.calssExit.Name = "calssExit";
            this.calssExit.Size = new System.Drawing.Size(75, 23);
            this.calssExit.TabIndex = 11;
            this.calssExit.Text = "Exit";
            this.calssExit.UseVisualStyleBackColor = true;
            this.calssExit.Click += new System.EventHandler(this.calssExit_Click);
            // 
            // number
            // 
            this.number.Location = new System.Drawing.Point(63, 12);
            this.number.Name = "number";
            this.number.Size = new System.Drawing.Size(100, 21);
            this.number.TabIndex = 12;
            this.number.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "Number";
            // 
            // guidBox
            // 
            this.guidBox.Location = new System.Drawing.Point(63, 39);
            this.guidBox.Multiline = true;
            this.guidBox.Name = "guidBox";
            this.guidBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.guidBox.Size = new System.Drawing.Size(356, 224);
            this.guidBox.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "Guids";
            // 
            // GuIdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 299);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.guidBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.number);
            this.Controls.Add(this.Generate);
            this.Controls.Add(this.calssExit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GuIdForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GUID";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.Button calssExit;
        private System.Windows.Forms.TextBox number;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox guidBox;
        private System.Windows.Forms.Label label2;
    }
}