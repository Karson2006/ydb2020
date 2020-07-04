namespace ydb.Tool
{
    partial class EmployeeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeForm));
            this.dgEmployee = new System.Windows.Forms.DataGridView();
            this.FID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FEmployeeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FDept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FMobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FTypeID = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.UploadOption = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.btSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.btUploadData = new System.Windows.Forms.Button();
            this.btGetData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmployee)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgEmployee
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEmployee.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgEmployee.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmployee.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FID,
            this.FEmployeeNumber,
            this.FName,
            this.FPosition,
            this.FDept,
            this.FMobile,
            this.FTypeID,
            this.UploadOption});
            this.dgEmployee.Location = new System.Drawing.Point(2, 12);
            this.dgEmployee.Name = "dgEmployee";
            this.dgEmployee.RowHeadersWidth = 60;
            this.dgEmployee.RowTemplate.Height = 23;
            this.dgEmployee.Size = new System.Drawing.Size(829, 407);
            this.dgEmployee.TabIndex = 0;
            this.dgEmployee.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgEmployee_RowStateChanged);
            // 
            // FID
            // 
            this.FID.HeaderText = "ID";
            this.FID.Name = "FID";
            this.FID.ReadOnly = true;
            // 
            // FEmployeeNumber
            // 
            this.FEmployeeNumber.HeaderText = "代码";
            this.FEmployeeNumber.Name = "FEmployeeNumber";
            this.FEmployeeNumber.ReadOnly = true;
            this.FEmployeeNumber.Width = 70;
            // 
            // FName
            // 
            this.FName.HeaderText = "姓名";
            this.FName.Name = "FName";
            this.FName.ReadOnly = true;
            this.FName.Width = 70;
            // 
            // FPosition
            // 
            this.FPosition.HeaderText = "职位";
            this.FPosition.Name = "FPosition";
            this.FPosition.ReadOnly = true;
            // 
            // FDept
            // 
            this.FDept.HeaderText = "部门";
            this.FDept.Name = "FDept";
            this.FDept.ReadOnly = true;
            // 
            // FMobile
            // 
            this.FMobile.HeaderText = "手机号码";
            this.FMobile.Name = "FMobile";
            // 
            // FTypeID
            // 
            this.FTypeID.HeaderText = "类型";
            this.FTypeID.Items.AddRange(new object[] {
            "自营                                          |97a5c945-c986-4380-b83c-3f9de245aa7f" +
                "",
            "招商                                          |3cc192c2-122b-4f5c-bc5e-904dbae26070" +
                "",
            "市场                                          |9e02e6f3-f6a0-431a-9dd2-395456612248" +
                "",
            "控销                                          |bfa3ccfe-d9b9-4b22-9fc9-4624e9c8d51b" +
                "",
            "其他                                          |e22e482b-f838-402f-9e5d-cdc16ee9e553" +
                ""});
            this.FTypeID.Name = "FTypeID";
            this.FTypeID.Width = 70;
            // 
            // UploadOption
            // 
            this.UploadOption.HeaderText = "是否上传";
            this.UploadOption.Name = "UploadOption";
            this.UploadOption.Width = 80;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.btSearch);
            this.buttonPanel.Controls.Add(this.label1);
            this.buttonPanel.Controls.Add(this.txtKeyword);
            this.buttonPanel.Controls.Add(this.button2);
            this.buttonPanel.Controls.Add(this.button1);
            this.buttonPanel.Controls.Add(this.btExit);
            this.buttonPanel.Controls.Add(this.btUploadData);
            this.buttonPanel.Controls.Add(this.btGetData);
            this.buttonPanel.Location = new System.Drawing.Point(2, 423);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(826, 33);
            this.buttonPanel.TabIndex = 3;
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(182, 2);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(77, 28);
            this.btSearch.TabIndex = 10;
            this.btSearch.Text = "查询";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.ButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "姓名";
            // 
            // txtKeyword
            // 
            this.txtKeyword.Location = new System.Drawing.Point(43, 7);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(130, 21);
            this.txtKeyword.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(471, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(86, 28);
            this.button2.TabIndex = 7;
            this.button2.Text = "删除数据";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(371, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 28);
            this.button1.TabIndex = 6;
            this.button1.Text = "载入YRB库数据";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(744, 2);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(77, 28);
            this.btExit.TabIndex = 5;
            this.btExit.Text = "退出";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btUploadData
            // 
            this.btUploadData.Location = new System.Drawing.Point(662, 2);
            this.btUploadData.Name = "btUploadData";
            this.btUploadData.Size = new System.Drawing.Size(77, 28);
            this.btUploadData.TabIndex = 4;
            this.btUploadData.Text = "上传数据";
            this.btUploadData.UseVisualStyleBackColor = true;
            this.btUploadData.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btGetData
            // 
            this.btGetData.Location = new System.Drawing.Point(562, 2);
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(95, 28);
            this.btGetData.TabIndex = 3;
            this.btGetData.Text = "载入待传数据";
            this.btGetData.UseVisualStyleBackColor = true;
            this.btGetData.Click += new System.EventHandler(this.ButtonClick);
            // 
            // EmployeeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 456);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.dgEmployee);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmployeeForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人员数据同步";
            this.Load += new System.EventHandler(this.Employees_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmployee)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgEmployee;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button btUploadData;
        private System.Windows.Forms.Button btGetData;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.DataGridViewTextBoxColumn FID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FEmployeeNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn FName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDept;
        private System.Windows.Forms.DataGridViewTextBoxColumn FMobile;
        private System.Windows.Forms.DataGridViewComboBoxColumn FTypeID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UploadOption;
    }
}