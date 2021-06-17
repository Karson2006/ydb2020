namespace iTR.Tool
{
    partial class OADataClearner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OADataClearner));
            this.lvFormList = new System.Windows.Forms.ListView();
            this.lstCategory = new System.Windows.Forms.ListBox();
            this.lvOARecord = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Date1 = new System.Windows.Forms.DateTimePicker();
            this.Date2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.Query = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TXApplicant = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblSelectForm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvFormList
            // 
            this.lvFormList.FullRowSelect = true;
            this.lvFormList.Location = new System.Drawing.Point(138, 21);
            this.lvFormList.MultiSelect = false;
            this.lvFormList.Name = "lvFormList";
            this.lvFormList.Size = new System.Drawing.Size(611, 148);
            this.lvFormList.TabIndex = 0;
            this.lvFormList.UseCompatibleStateImageBehavior = false;
            this.lvFormList.SelectedIndexChanged += new System.EventHandler(this.lvFormList_SelectedIndexChanged);
            // 
            // lstCategory
            // 
            this.lstCategory.FormattingEnabled = true;
            this.lstCategory.ItemHeight = 12;
            this.lstCategory.Location = new System.Drawing.Point(4, 21);
            this.lstCategory.Name = "lstCategory";
            this.lstCategory.Size = new System.Drawing.Size(128, 148);
            this.lstCategory.TabIndex = 1;
            this.lstCategory.SelectedIndexChanged += new System.EventHandler(this.lstCategory_SelectedIndexChanged);
            // 
            // lvOARecord
            // 
            this.lvOARecord.CheckBoxes = true;
            this.lvOARecord.FullRowSelect = true;
            this.lvOARecord.Location = new System.Drawing.Point(4, 194);
            this.lvOARecord.Name = "lvOARecord";
            this.lvOARecord.Size = new System.Drawing.Size(745, 287);
            this.lvOARecord.TabIndex = 2;
            this.lvOARecord.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(443, 487);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "删除表单";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(547, 487);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 24);
            this.button2.TabIndex = 4;
            this.button2.Text = "删除数据";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(651, 487);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 24);
            this.button3.TabIndex = 5;
            this.button3.Text = "退出";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "表单列表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "流程列表";
            // 
            // Date1
            // 
            this.Date1.Location = new System.Drawing.Point(309, 171);
            this.Date1.Name = "Date1";
            this.Date1.Size = new System.Drawing.Size(141, 21);
            this.Date1.TabIndex = 8;
            // 
            // Date2
            // 
            this.Date2.Location = new System.Drawing.Point(483, 171);
            this.Date2.Name = "Date2";
            this.Date2.Size = new System.Drawing.Size(141, 21);
            this.Date2.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(465, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "~";
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(648, 170);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(98, 24);
            this.Query.TabIndex = 11;
            this.Query.Text = "查询";
            this.Query.UseVisualStyleBackColor = true;
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(137, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "申请人：";
            // 
            // TXApplicant
            // 
            this.TXApplicant.Location = new System.Drawing.Point(188, 171);
            this.TXApplicant.Name = "TXApplicant";
            this.TXApplicant.Size = new System.Drawing.Size(90, 21);
            this.TXApplicant.TabIndex = 13;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 487);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "全选";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblSelectForm
            // 
            this.lblSelectForm.AutoSize = true;
            this.lblSelectForm.Location = new System.Drawing.Point(140, 5);
            this.lblSelectForm.Name = "lblSelectForm";
            this.lblSelectForm.Size = new System.Drawing.Size(0, 12);
            this.lblSelectForm.TabIndex = 15;
            // 
            // OADataClearner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 511);
            this.Controls.Add(this.lblSelectForm);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.TXApplicant);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Query);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Date2);
            this.Controls.Add(this.Date1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lvOARecord);
            this.Controls.Add(this.lstCategory);
            this.Controls.Add(this.lvFormList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OADataClearner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OADataClearner";
            this.Load += new System.EventHandler(this.OADataClearner_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvFormList;
        private System.Windows.Forms.ListBox lstCategory;
        private System.Windows.Forms.ListView lvOARecord;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker Date1;
        private System.Windows.Forms.DateTimePicker Date2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Query;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TXApplicant;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label lblSelectForm;
    }
}