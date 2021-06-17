namespace iTR.Tool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsRSA = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsInvoke = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolGuid = new System.Windows.Forms.ToolStripButton();
            this.comTools = new System.Windows.Forms.ToolStripButton();
            this.tbSQLScript = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.bugetData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRSA,
            this.toolStripSeparator1,
            this.tsInvoke,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolGuid,
            this.comTools,
            this.tbSQLScript,
            this.toolStripButton3,
            this.bugetData,
            this.toolStripSeparator3,
            this.toolStripButton1});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(741, 25);
            this.tsMain.TabIndex = 12;
            this.tsMain.Text = "toolStrip1";
            this.tsMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsMain_ItemClicked);
            // 
            // tsRSA
            // 
            this.tsRSA.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRSA.Image = ((System.Drawing.Image)(resources.GetObject("tsRSA.Image")));
            this.tsRSA.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRSA.Name = "tsRSA";
            this.tsRSA.Size = new System.Drawing.Size(23, 22);
            this.tsRSA.ToolTipText = "加密";
            this.tsRSA.Click += new System.EventHandler(this.tsRSA_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsInvoke
            // 
            this.tsInvoke.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsInvoke.Image = ((System.Drawing.Image)(resources.GetObject("tsInvoke.Image")));
            this.tsInvoke.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsInvoke.Name = "tsInvoke";
            this.tsInvoke.Size = new System.Drawing.Size(23, 22);
            this.tsInvoke.Text = "Class Invoke";
            this.tsInvoke.Click += new System.EventHandler(this.tsInvoke_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "WS Invoke";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolGuid
            // 
            this.toolGuid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolGuid.Image = ((System.Drawing.Image)(resources.GetObject("toolGuid.Image")));
            this.toolGuid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolGuid.Name = "toolGuid";
            this.toolGuid.Size = new System.Drawing.Size(23, 22);
            this.toolGuid.Text = "GUI Tool";
            this.toolGuid.Click += new System.EventHandler(this.toolGuid_Click);
            // 
            // comTools
            // 
            this.comTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.comTools.Image = ((System.Drawing.Image)(resources.GetObject("comTools.Image")));
            this.comTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.comTools.Name = "comTools";
            this.comTools.Size = new System.Drawing.Size(23, 22);
            this.comTools.Text = "Comm Tools";
            this.comTools.Click += new System.EventHandler(this.comTools_Click);
            // 
            // tbSQLScript
            // 
            this.tbSQLScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbSQLScript.Image = ((System.Drawing.Image)(resources.GetObject("tbSQLScript.Image")));
            this.tbSQLScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbSQLScript.Name = "tbSQLScript";
            this.tbSQLScript.Size = new System.Drawing.Size(23, 22);
            this.tbSQLScript.ToolTipText = "SQL Scription";
            this.tbSQLScript.Click += new System.EventHandler(this.tbSQLScript_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "OADataClearner";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // bugetData
            // 
            this.bugetData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bugetData.Image = ((System.Drawing.Image)(resources.GetObject("bugetData.Image")));
            this.bugetData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bugetData.Name = "bugetData";
            this.bugetData.Size = new System.Drawing.Size(23, 22);
            this.bugetData.Text = "预算数据格式化处理";
            this.bugetData.Click += new System.EventHandler(this.bugetData_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Exit";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 457);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iTR.Tools";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsRSA;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsInvoke;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolGuid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton comTools;
        private System.Windows.Forms.ToolStripButton tbSQLScript;
        private System.Windows.Forms.ToolStripButton bugetData;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
    }
}

