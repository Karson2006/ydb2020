namespace iTR.Tool
{
    partial class ResponeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResponeForm));
            this.parameters = new System.Windows.Forms.TextBox();
            this.returns = new System.Windows.Forms.TextBox();
            this.methodList = new System.Windows.Forms.ListBox();
            this.classList = new System.Windows.Forms.ListBox();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.parameterBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.classInvoke = new System.Windows.Forms.Button();
            this.calssExit = new System.Windows.Forms.Button();
            this.saveResult = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // parameters
            // 
            this.parameters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.parameters.Location = new System.Drawing.Point(178, 368);
            this.parameters.Multiline = true;
            this.parameters.Name = "parameters";
            this.parameters.Size = new System.Drawing.Size(181, 100);
            this.parameters.TabIndex = 13;
            // 
            // returns
            // 
            this.returns.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.returns.Location = new System.Drawing.Point(12, 368);
            this.returns.Multiline = true;
            this.returns.Name = "returns";
            this.returns.Size = new System.Drawing.Size(159, 100);
            this.returns.TabIndex = 12;
            this.returns.TextChanged += new System.EventHandler(this.returns_TextChanged);
            // 
            // methodList
            // 
            this.methodList.FormattingEnabled = true;
            this.methodList.ItemHeight = 12;
            this.methodList.Location = new System.Drawing.Point(178, 23);
            this.methodList.Name = "methodList";
            this.methodList.Size = new System.Drawing.Size(181, 340);
            this.methodList.TabIndex = 11;
            this.methodList.SelectedValueChanged += new System.EventHandler(this.methodList_SelectedValueChanged);
            // 
            // classList
            // 
            this.classList.FormattingEnabled = true;
            this.classList.ItemHeight = 12;
            this.classList.Location = new System.Drawing.Point(12, 22);
            this.classList.Name = "classList";
            this.classList.Size = new System.Drawing.Size(159, 340);
            this.classList.TabIndex = 10;
            this.classList.SelectedValueChanged += new System.EventHandler(this.classList_SelectedValueChanged);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(371, 266);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(343, 205);
            this.outputBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(363, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Output:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(369, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parameters:以分号隔开不同参数值";
            // 
            // parameterBox
            // 
            this.parameterBox.Location = new System.Drawing.Point(365, 22);
            this.parameterBox.Multiline = true;
            this.parameterBox.Name = "parameterBox";
            this.parameterBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.parameterBox.Size = new System.Drawing.Size(343, 226);
            this.parameterBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Method:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Class:";
            // 
            // classInvoke
            // 
            this.classInvoke.Location = new System.Drawing.Point(556, 478);
            this.classInvoke.Name = "classInvoke";
            this.classInvoke.Size = new System.Drawing.Size(75, 23);
            this.classInvoke.TabIndex = 8;
            this.classInvoke.Text = "Invoke";
            this.classInvoke.UseVisualStyleBackColor = true;
            this.classInvoke.Click += new System.EventHandler(this.classInvoke_Click);
            // 
            // calssExit
            // 
            this.calssExit.Location = new System.Drawing.Point(633, 477);
            this.calssExit.Name = "calssExit";
            this.calssExit.Size = new System.Drawing.Size(75, 23);
            this.calssExit.TabIndex = 9;
            this.calssExit.Text = "Exit";
            this.calssExit.UseVisualStyleBackColor = true;
            this.calssExit.Click += new System.EventHandler(this.calssExit_Click);
            // 
            // saveResult
            // 
            this.saveResult.Location = new System.Drawing.Point(459, 478);
            this.saveResult.Name = "saveResult";
            this.saveResult.Size = new System.Drawing.Size(91, 23);
            this.saveResult.TabIndex = 15;
            this.saveResult.Text = "Save Result";
            this.saveResult.UseVisualStyleBackColor = true;
            this.saveResult.Click += new System.EventHandler(this.saveResult_Click);
            // 
            // ResponeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 501);
            this.Controls.Add(this.saveResult);
            this.Controls.Add(this.parameters);
            this.Controls.Add(this.returns);
            this.Controls.Add(this.classInvoke);
            this.Controls.Add(this.methodList);
            this.Controls.Add(this.calssExit);
            this.Controls.Add(this.classList);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.parameterBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ResponeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Class Invoke";
            this.Load += new System.EventHandler(this.ResponeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button calssExit;
        private System.Windows.Forms.Button classInvoke;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox parameterBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox methodList;
        private System.Windows.Forms.ListBox classList;
        private System.Windows.Forms.TextBox parameters;
        private System.Windows.Forms.TextBox returns;
        private System.Windows.Forms.Button saveResult;
    }
}