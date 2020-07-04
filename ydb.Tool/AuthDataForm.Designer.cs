namespace ydb.Tool
{
    partial class AuthDataForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthDataForm));
            this.tabAuthData = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgAuthData = new System.Windows.Forms.DataGridView();
            this.FHospitalNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FHospitalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FMR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUploadOption = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FTID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btUploadData = new System.Windows.Forms.Button();
            this.btGetData = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.tbHospitalName = new System.Windows.Forms.TextBox();
            this.tbEmployeeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btSearch = new System.Windows.Forms.Button();
            this.ChkAllSelected = new System.Windows.Forms.CheckBox();
            this.tabAuthData.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAuthData)).BeginInit();
            this.SuspendLayout();
            // 
            // tabAuthData
            // 
            this.tabAuthData.Controls.Add(this.tabPage1);
            this.tabAuthData.Controls.Add(this.tabPage2);
            this.tabAuthData.Location = new System.Drawing.Point(4, 12);
            this.tabAuthData.Name = "tabAuthData";
            this.tabAuthData.SelectedIndex = 0;
            this.tabAuthData.Size = new System.Drawing.Size(640, 481);
            this.tabAuthData.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ChkAllSelected);
            this.tabPage1.Controls.Add(this.dgAuthData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(632, 455);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgAuthData
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAuthData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgAuthData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAuthData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FHospitalNumber,
            this.FHospitalName,
            this.FMR,
            this.FType,
            this.FUploadOption,
            this.FTID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgAuthData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgAuthData.Location = new System.Drawing.Point(2, 5);
            this.dgAuthData.Name = "dgAuthData";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAuthData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgAuthData.RowHeadersWidth = 60;
            this.dgAuthData.RowTemplate.Height = 23;
            this.dgAuthData.Size = new System.Drawing.Size(627, 425);
            this.dgAuthData.TabIndex = 1;
            this.dgAuthData.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgAuthData_RowStateChanged);
            // 
            // FHospitalNumber
            // 
            this.FHospitalNumber.HeaderText = "代码";
            this.FHospitalNumber.Name = "FHospitalNumber";
            this.FHospitalNumber.ReadOnly = true;
            // 
            // FHospitalName
            // 
            this.FHospitalName.HeaderText = "名称";
            this.FHospitalName.Name = "FHospitalName";
            this.FHospitalName.ReadOnly = true;
            // 
            // FMR
            // 
            this.FMR.HeaderText = "责任人";
            this.FMR.Name = "FMR";
            this.FMR.ReadOnly = true;
            // 
            // FType
            // 
            this.FType.HeaderText = "类型";
            this.FType.Name = "FType";
            // 
            // FUploadOption
            // 
            this.FUploadOption.HeaderText = "是否上传";
            this.FUploadOption.Name = "FUploadOption";
            // 
            // FTID
            // 
            this.FTID.HeaderText = "FTID";
            this.FTID.Name = "FTID";
            this.FTID.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(632, 455);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btUploadData
            // 
            this.btUploadData.Location = new System.Drawing.Point(647, 404);
            this.btUploadData.Name = "btUploadData";
            this.btUploadData.Size = new System.Drawing.Size(100, 25);
            this.btUploadData.TabIndex = 4;
            this.btUploadData.Text = "上传数据";
            this.btUploadData.UseVisualStyleBackColor = true;
            this.btUploadData.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btGetData
            // 
            this.btGetData.Location = new System.Drawing.Point(646, 370);
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(100, 25);
            this.btGetData.TabIndex = 3;
            this.btGetData.Text = "载待传数据";
            this.btGetData.UseVisualStyleBackColor = true;
            this.btGetData.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(646, 458);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(100, 25);
            this.btExit.TabIndex = 6;
            this.btExit.Text = "退出";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tbHospitalName
            // 
            this.tbHospitalName.Location = new System.Drawing.Point(641, 49);
            this.tbHospitalName.Name = "tbHospitalName";
            this.tbHospitalName.Size = new System.Drawing.Size(100, 21);
            this.tbHospitalName.TabIndex = 8;
            // 
            // tbEmployeeName
            // 
            this.tbEmployeeName.Location = new System.Drawing.Point(643, 94);
            this.tbEmployeeName.Name = "tbEmployeeName";
            this.tbEmployeeName.Size = new System.Drawing.Size(100, 21);
            this.tbEmployeeName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(644, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "医院名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(643, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "责任人";
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(643, 121);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(100, 25);
            this.btSearch.TabIndex = 12;
            this.btSearch.Text = "查询";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.ButtonClick);
            // 
            // ChkAllSelected
            // 
            this.ChkAllSelected.AutoSize = true;
            this.ChkAllSelected.Location = new System.Drawing.Point(6, 434);
            this.ChkAllSelected.Name = "ChkAllSelected";
            this.ChkAllSelected.Size = new System.Drawing.Size(48, 16);
            this.ChkAllSelected.TabIndex = 2;
            this.ChkAllSelected.Text = "全选";
            this.ChkAllSelected.UseVisualStyleBackColor = true;
            this.ChkAllSelected.CheckedChanged += new System.EventHandler(this.ChkAllSelected_CheckedChanged);
            // 
            // AuthDataForm
            // 
            this.AcceptButton = this.btGetData;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 489);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbEmployeeName);
            this.Controls.Add(this.tbHospitalName);
            this.Controls.Add(this.btUploadData);
            this.Controls.Add(this.btGetData);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.tabAuthData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuthDataForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人员数据同步";
            this.Load += new System.EventHandler(this.Employees_Load);
            this.tabAuthData.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAuthData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabAuthData;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgAuthData;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btUploadData;
        private System.Windows.Forms.Button btGetData;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.TextBox tbHospitalName;
        private System.Windows.Forms.TextBox tbEmployeeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn FHospitalNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn FHospitalName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FMR;
        private System.Windows.Forms.DataGridViewTextBoxColumn FType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FUploadOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn FTID;
        private System.Windows.Forms.CheckBox ChkAllSelected;
    }
}