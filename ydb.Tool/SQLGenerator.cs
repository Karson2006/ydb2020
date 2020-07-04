using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ydb.DataService; 

namespace ydb.Tool
{
    public partial class SQLGenerator : Form
    {
        private string tbName = "";
        public SQLGenerator()
        {
            InitializeComponent();
        }

        private void btSelectFolder_Click(object sender, EventArgs e)
        {
            fbSQLfolder.ShowNewFolderButton = true;
            fbSQLfolder.ShowDialog();
            if (fbSQLfolder.SelectedPath.Trim().Length > 0)
            {
                tbFolderName.Text = fbSQLfolder.SelectedPath;
            }
            else
            {
                MessageBox.Show("SQL 文件目录不能为空");
            }

        }

        private void SQLGenerator_Load(object sender, EventArgs e)
        {
            DataTable dt = SQLScript.GetTableList();
            foreach(DataRow row in dt.Rows)
            {
                cbTableName.Items.Add(row["name"].ToString() + "                                        |" + row["id"].ToString());
            }
              
            lvFields.View = View.Details;//设置视图  
            lvFields.Columns.Add("Field Name", lvFields.Width / 2, HorizontalAlignment.Center);
            lvFields.Columns.Add("DataType", lvFields.Width / 2 - 10, HorizontalAlignment.Center);
        }

        private void cbTableName_SelectedValueChanged(object sender, EventArgs e)
        {
            lvFields.Items.Clear();

            if (cbTableName.SelectedItem == null)
                return;
            string tbID = cbTableName.SelectedItem.ToString().Split('|')[1].Trim();
            DataTable dt = SQLScript.GetTableFiedList(tbID);
            foreach(DataRow row in dt.Rows)
            {
                 var item = new ListViewItem();
                item.Text = row["name"].ToString();
                string result = row["xtype"].ToString();
                item.Checked = true;
                item.SubItems.Add(result);
                lvFields.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                if (tbFolderName.Text.Length == 0)
                    throw new Exception("SQL 文件保存目录不能为空");
                string sql = "";

                if (tabControl1.SelectedTab.Name == "tabPage1")
                {
                    tbName = cbTableName.SelectedItem.ToString().Split('|')[0].Trim();


                    foreach (ListViewItem item in lvFields.Items)
                    {
                        if (item.Checked)
                        {
                            sql = sql + item.Text + ",";
                        }
                    }
                    if (sql.Trim().Length > 0)
                        sql = sql.Substring(0, sql.Length - 1);
                    else
                        throw new Exception("字段没有选择");

                    sql = "Select " + sql + " From " + tbName;
                    
                }
                else
                {
                  
                    if (tbSQLScript.Text.Trim().Length == 0)
                        throw new Exception("SQL script is null");
                    if (tbTableName.Text.Trim().Length == 0)
                        tbName = DateTime.Now.ToString("yyyyMMDDHHmmss");
                    else
                        tbName = tbTableName.Text.Trim();
                    sql = tbSQLScript.Text;
                }

                DataTable dt = SQLScript.GetRecordSet(sql);  
                string txtFileName = tbFolderName.Text + "\\" + tbName + ".txt";
                ExportToTxt(dt,txtFileName);
                throw new Exception("数据文件已生成：" + txtFileName);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void ExportToTxt(DataTable _dtExport,string txtFileName)
        {
            try
            {
                if (_dtExport.Rows.Count == 0)
                    throw new Exception("没有要导出的数据记录");

                using (StreamWriter _objWriter = new StreamWriter(txtFileName, false, System.Text.Encoding.UTF8))
                {
                    foreach (DataRow row in _dtExport.Rows)
                    {
                        string fieldString = "";
                        string valueString = "";
                        foreach (DataColumn col in _dtExport.Columns)
                        {
                            fieldString = fieldString + col.ColumnName + ",";
                            valueString = valueString + "'" + row[col.ColumnName].ToString() + "',";
                            //_objWriter.WriteLine(row[col.ColumnName].ToString());
                        }
                        if (fieldString.Length > 0)
                        {
                            fieldString = fieldString.Substring(0, fieldString.Length - 1);
                            valueString = valueString.Substring(0, valueString.Length - 1);
                            string lineTxt = "Insert Into " + tbName + "(" + fieldString + ") Values(" + valueString + ")";
                            _objWriter.WriteLine(lineTxt);
                            lineTxt = "Go";
                            _objWriter.WriteLine(lineTxt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("导出到文本文件失败，原因:{0}", ex.Message.Trim()));
            }
        }
        
    }
}
