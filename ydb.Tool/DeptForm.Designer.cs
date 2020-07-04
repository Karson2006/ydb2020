namespace ydb.Tool
{
    partial class DeptForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeptForm));
            this.btUploadData = new System.Windows.Forms.Button();
            this.btGetData = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.tbDeptName = new System.Windows.Forms.TextBox();
            this.tbSupervisorName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btSearch = new System.Windows.Forms.Button();
            this.dgDeptData = new System.Windows.Forms.DataGridView();
            this.FDeptNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FDeptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FSupervisor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FParentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUploadOption = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FDeptID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgDeptData)).BeginInit();
            this.SuspendLayout();
            // 
            // btUploadData
            // 
            this.btUploadData.Location = new System.Drawing.Point(633, 404);
            this.btUploadData.Name = "btUploadData";
            this.btUploadData.Size = new System.Drawing.Size(114, 25);
            this.btUploadData.TabIndex = 4;
            this.btUploadData.Text = "上传数据";
            this.btUploadData.UseVisualStyleBackColor = true;
            this.btUploadData.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btGetData
            // 
            this.btGetData.Location = new System.Drawing.Point(633, 370);
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(113, 25);
            this.btGetData.TabIndex = 3;
            this.btGetData.Text = "载待传数据";
            this.btGetData.UseVisualStyleBackColor = true;
            this.btGetData.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btExit
            // 
            this.btExit.Location = new System.Drawing.Point(633, 456);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(113, 25);
            this.btExit.TabIndex = 6;
            this.btExit.Text = "退出";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tbDeptName
            // 
            this.tbDeptName.Location = new System.Drawing.Point(633, 49);
            this.tbDeptName.Name = "tbDeptName";
            this.tbDeptName.Size = new System.Drawing.Size(114, 21);
            this.tbDeptName.TabIndex = 8;
            // 
            // tbSupervisorName
            // 
            this.tbSupervisorName.Location = new System.Drawing.Point(633, 94);
            this.tbSupervisorName.Name = "tbSupervisorName";
            this.tbSupervisorName.Size = new System.Drawing.Size(114, 21);
            this.tbSupervisorName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(633, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "部门名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(634, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "部门主管";
            // 
            // btSearch
            // 
            this.btSearch.Location = new System.Drawing.Point(633, 121);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(113, 25);
            this.btSearch.TabIndex = 12;
            this.btSearch.Text = "查询";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.ButtonClick);
            // 
            // dgDeptData
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDeptData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDeptData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDeptData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FDeptNumber,
            this.FDeptName,
            this.FSupervisor,
            this.FParentName,
            this.FUploadOption,
            this.FDeptID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDeptData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgDeptData.Location = new System.Drawing.Point(3, 6);
            this.dgDeptData.Name = "dgDeptData";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDeptData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgDeptData.RowHeadersWidth = 50;
            this.dgDeptData.RowTemplate.Height = 23;
            this.dgDeptData.Size = new System.Drawing.Size(627, 477);
            this.dgDeptData.TabIndex = 13;
            this.dgDeptData.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dgDeptData_RowStateChanged);
            // 
            // FDeptNumber
            // 
            this.FDeptNumber.HeaderText = "部门代码";
            this.FDeptNumber.Name = "FDeptNumber";
            this.FDeptNumber.ReadOnly = true;
            // 
            // FDeptName
            // 
            this.FDeptName.HeaderText = "部门名称";
            this.FDeptName.Name = "FDeptName";
            this.FDeptName.ReadOnly = true;
            // 
            // FSupervisor
            // 
            this.FSupervisor.HeaderText = "部门主管";
            this.FSupervisor.Name = "FSupervisor";
            this.FSupervisor.ReadOnly = true;
            // 
            // FParentName
            // 
            this.FParentName.HeaderText = "上级部门";
            this.FParentName.Name = "FParentName";
            // 
            // FUploadOption
            // 
            this.FUploadOption.HeaderText = "是否上传";
            this.FUploadOption.Name = "FUploadOption";
            // 
            // FDeptID
            // 
            this.FDeptID.HeaderText = "FDeptID";
            this.FDeptID.Name = "FDeptID";
            this.FDeptID.Visible = false;
            // 
            // DeptForm
            // 
            this.AcceptButton = this.btGetData;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 489);
            this.Controls.Add(this.dgDeptData);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSupervisorName);
            this.Controls.Add(this.tbDeptName);
            this.Controls.Add(this.btUploadData);
            this.Controls.Add(this.btGetData);
            this.Controls.Add(this.btExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeptForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "组织架构同步";
            this.Load += new System.EventHandler(this.Employees_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgDeptData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btUploadData;
        private System.Windows.Forms.Button btGetData;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.TextBox tbDeptName;
        private System.Windows.Forms.TextBox tbSupervisorName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.DataGridView dgDeptData;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDeptNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDeptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FSupervisor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FParentName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FUploadOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn FDeptID;
    }
}