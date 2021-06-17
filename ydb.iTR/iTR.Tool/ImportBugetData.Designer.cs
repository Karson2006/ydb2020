namespace iTR.Tool
{
    partial class ImportBugetData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportBugetData));
            this.Numyear = new System.Windows.Forms.NumericUpDown();
            this.tbSheet = new System.Windows.Forms.TextBox();
            this.excelFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectExcelFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.MonthBuget = new System.Windows.Forms.Button();
            this.QuaterBuget = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.ImportData = new System.Windows.Forms.Button();
            this.excelFile = new System.Windows.Forms.OpenFileDialog();
            this.tbCompany = new System.Windows.Forms.TextBox();
            this.txDeparts = new System.Windows.Forms.TextBox();
            this.excelfolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.excelExportFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Numyear)).BeginInit();
            this.SuspendLayout();
            // 
            // Numyear
            // 
            this.Numyear.Location = new System.Drawing.Point(69, 66);
            this.Numyear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.Numyear.Name = "Numyear";
            this.Numyear.Size = new System.Drawing.Size(358, 21);
            this.Numyear.TabIndex = 2;
            this.Numyear.Value = new decimal(new int[] {
            2018,
            0,
            0,
            0});
            // 
            // tbSheet
            // 
            this.tbSheet.Location = new System.Drawing.Point(69, 94);
            this.tbSheet.Name = "tbSheet";
            this.tbSheet.Size = new System.Drawing.Size(358, 21);
            this.tbSheet.TabIndex = 3;
            this.tbSheet.Text = "行政部";
            // 
            // excelFilePath
            // 
            this.excelFilePath.Location = new System.Drawing.Point(69, 122);
            this.excelFilePath.Name = "excelFilePath";
            this.excelFilePath.Size = new System.Drawing.Size(314, 21);
            this.excelFilePath.TabIndex = 4;
            // 
            // btnSelectExcelFile
            // 
            this.btnSelectExcelFile.Location = new System.Drawing.Point(388, 122);
            this.btnSelectExcelFile.Name = "btnSelectExcelFile";
            this.btnSelectExcelFile.Size = new System.Drawing.Size(38, 23);
            this.btnSelectExcelFile.TabIndex = 5;
            this.btnSelectExcelFile.Text = "...";
            this.btnSelectExcelFile.UseVisualStyleBackColor = true;
            this.btnSelectExcelFile.Click += new System.EventHandler(this.btnSelectExcelFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "所属公司";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "成本中心";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "会计年度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "数据页签";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Excel文件";
            // 
            // MonthBuget
            // 
            this.MonthBuget.Location = new System.Drawing.Point(193, 178);
            this.MonthBuget.Name = "MonthBuget";
            this.MonthBuget.Size = new System.Drawing.Size(75, 23);
            this.MonthBuget.TabIndex = 11;
            this.MonthBuget.Text = "月度导出";
            this.MonthBuget.UseVisualStyleBackColor = true;
            this.MonthBuget.Click += new System.EventHandler(this.MonthBuget_Click);
            // 
            // QuaterBuget
            // 
            this.QuaterBuget.Location = new System.Drawing.Point(115, 178);
            this.QuaterBuget.Name = "QuaterBuget";
            this.QuaterBuget.Size = new System.Drawing.Size(75, 23);
            this.QuaterBuget.TabIndex = 13;
            this.QuaterBuget.Text = "季度导出";
            this.QuaterBuget.UseVisualStyleBackColor = true;
            this.QuaterBuget.Click += new System.EventHandler(this.QuaterBuget_Click);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(351, 178);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 14;
            this.Exit.Text = "退出";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // ImportData
            // 
            this.ImportData.Location = new System.Drawing.Point(273, 178);
            this.ImportData.Name = "ImportData";
            this.ImportData.Size = new System.Drawing.Size(75, 23);
            this.ImportData.TabIndex = 15;
            this.ImportData.Text = "月度导入";
            this.ImportData.UseVisualStyleBackColor = true;
            this.ImportData.Click += new System.EventHandler(this.ImportData_Click);
            // 
            // tbCompany
            // 
            this.tbCompany.Location = new System.Drawing.Point(69, 13);
            this.tbCompany.Name = "tbCompany";
            this.tbCompany.Size = new System.Drawing.Size(358, 21);
            this.tbCompany.TabIndex = 16;
            this.tbCompany.Text = "上海青平药业有限公司";
            // 
            // txDeparts
            // 
            this.txDeparts.Location = new System.Drawing.Point(69, 39);
            this.txDeparts.Name = "txDeparts";
            this.txDeparts.Size = new System.Drawing.Size(358, 21);
            this.txDeparts.TabIndex = 17;
            this.txDeparts.Text = "行政部";
            // 
            // excelExportFolder
            // 
            this.excelExportFolder.Location = new System.Drawing.Point(68, 151);
            this.excelExportFolder.Name = "excelExportFolder";
            this.excelExportFolder.Size = new System.Drawing.Size(314, 21);
            this.excelExportFolder.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(388, 151);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "导出文件夹";
            // 
            // ImportBugetData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 207);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.excelExportFolder);
            this.Controls.Add(this.txDeparts);
            this.Controls.Add(this.tbCompany);
            this.Controls.Add(this.ImportData);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.QuaterBuget);
            this.Controls.Add(this.MonthBuget);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectExcelFile);
            this.Controls.Add(this.excelFilePath);
            this.Controls.Add(this.tbSheet);
            this.Controls.Add(this.Numyear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportBugetData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "预算数据格式化";
            this.Load += new System.EventHandler(this.ImportBugetData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Numyear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown Numyear;
        private System.Windows.Forms.TextBox tbSheet;
        private System.Windows.Forms.TextBox excelFilePath;
        private System.Windows.Forms.Button btnSelectExcelFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button MonthBuget;
        private System.Windows.Forms.Button QuaterBuget;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Button ImportData;
        private System.Windows.Forms.OpenFileDialog excelFile;
        private System.Windows.Forms.TextBox tbCompany;
        private System.Windows.Forms.TextBox txDeparts;
        private System.Windows.Forms.FolderBrowserDialog excelfolderBrowser;
        private System.Windows.Forms.TextBox excelExportFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
    }
}