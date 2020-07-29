namespace ydb.Tool
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainTool = new System.Windows.Forms.ToolStrip();
            this.tbDeptment = new System.Windows.Forms.ToolStripButton();
            this.tbEmployee = new System.Windows.Forms.ToolStripButton();
            this.tbHospitalAuth = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbSqlScript = new System.Windows.Forms.ToolStripButton();
            this.btExit = new System.Windows.Forms.ToolStripSeparator();
            this.tbExit = new System.Windows.Forms.ToolStripButton();
            this.button1 = new System.Windows.Forms.Button();
            this.mainTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTool
            // 
            this.mainTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbDeptment,
            this.tbEmployee,
            this.tbHospitalAuth,
            this.toolStripSeparator1,
            this.tbSqlScript,
            this.btExit,
            this.tbExit});
            this.mainTool.Location = new System.Drawing.Point(0, 0);
            this.mainTool.Name = "mainTool";
            this.mainTool.Size = new System.Drawing.Size(284, 25);
            this.mainTool.TabIndex = 0;
            this.mainTool.Text = "toolStrip1";
            // 
            // tbDeptment
            // 
            this.tbDeptment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbDeptment.Image = ((System.Drawing.Image)(resources.GetObject("tbDeptment.Image")));
            this.tbDeptment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDeptment.Name = "tbDeptment";
            this.tbDeptment.Size = new System.Drawing.Size(23, 22);
            this.tbDeptment.Text = "组织架构同步";
            this.tbDeptment.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tbEmployee
            // 
            this.tbEmployee.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbEmployee.Image = ((System.Drawing.Image)(resources.GetObject("tbEmployee.Image")));
            this.tbEmployee.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbEmployee.Name = "tbEmployee";
            this.tbEmployee.Size = new System.Drawing.Size(23, 22);
            this.tbEmployee.Text = "人员同步";
            this.tbEmployee.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tbHospitalAuth
            // 
            this.tbHospitalAuth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbHospitalAuth.Image = ((System.Drawing.Image)(resources.GetObject("tbHospitalAuth.Image")));
            this.tbHospitalAuth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbHospitalAuth.Name = "tbHospitalAuth";
            this.tbHospitalAuth.Size = new System.Drawing.Size(23, 22);
            this.tbHospitalAuth.Text = "授权数据同步";
            this.tbHospitalAuth.Click += new System.EventHandler(this.ButtonClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbSqlScript
            // 
            this.tbSqlScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSqlScript.Image = ((System.Drawing.Image)(resources.GetObject("tbSqlScript.Image")));
            this.tbSqlScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSqlScript.Name = "tbSqlScript";
            this.tbSqlScript.Size = new System.Drawing.Size(23, 22);
            this.tbSqlScript.Text = "生成Insert 语句";
            this.tbSqlScript.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btExit
            // 
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(6, 25);
            this.btExit.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tbExit
            // 
            this.tbExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbExit.Image = ((System.Drawing.Image)(resources.GetObject("tbExit.Image")));
            this.tbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExit.Name = "tbExit";
            this.tbExit.Size = new System.Drawing.Size(23, 22);
            this.tbExit.Text = "退出";
            this.tbExit.Click += new System.EventHandler(this.ButtonClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mainTool);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "药代宝管理工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainTool.ResumeLayout(false);
            this.mainTool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainTool;
        private System.Windows.Forms.ToolStripButton tbEmployee;
        private System.Windows.Forms.ToolStripButton tbHospitalAuth;
        private System.Windows.Forms.ToolStripButton tbDeptment;
        private System.Windows.Forms.ToolStripSeparator btExit;
        private System.Windows.Forms.ToolStripButton tbExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbSqlScript;
        private System.Windows.Forms.Button button1;
    }
}