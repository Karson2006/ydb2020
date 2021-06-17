using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTR.Lib;
using System.IO;

namespace iTR.Tool
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
            string sql = "Select [name],[id] from [sysobjects] where xtype='U' Order By [name]";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
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
            string sql = "select name,xtype  from syscolumns where  id='" + tbID + "' Order by [name]";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
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
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = null;
                string txtFileName = "";

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
                    dt = runner.ExecuteSql(sql);
                    txtFileName = tbFolderName.Text + "\\" + tbName + ".txt";
                    ExportToTxt(dt, txtFileName);
                    
                }
                else if (tabControl1.SelectedTab.Name == "tabPage2")
                {
                    if (tbInsertTableName.Text.Trim().Length == 0)
                        throw new Exception("Insert into tablename is null");

                    if (tbSQLScript.Text.Trim().Length == 0)
                        throw new Exception("SQL script is null");

                    tbName = tbInsertTableName.Text;
                    sql = tbSQLScript.Text;

                    dt = runner.ExecuteSql(sql);
                    txtFileName = tbFolderName.Text + "\\" + tbName + ".txt";
                    ExportToTxt(dt, txtFileName);
                }
                else
                {
                    tbName =  textBox2.Text;
                    sql = textBox1.Text;
                    dt = runner.ExecuteSql(sql);
                    string[] updateFields = textBox3.Text.Split(',');
                    string[] whereFields = textBox4.Text.Split(',');
                    txtFileName = tbFolderName.Text + "\\" + "Update_" +  tbName + ".txt";
                    ExportUpdateScipt(dt, txtFileName, updateFields, whereFields);
                }
                
               
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

        private void ExportUpdateScipt(DataTable _dtExport,string txtFileName,string[] updateFields,string[] whereFields)
        {
            try
            {
                if (_dtExport.Rows.Count == 0)
                    throw new Exception("没有要导出的数据记录");

                using (StreamWriter _objWriter = new StreamWriter(txtFileName, false, System.Text.Encoding.UTF8))
                {
                    foreach (DataRow row in _dtExport.Rows)
                    {
                        string updateString = "";
                        string whereString = "";

                        for(int i = 0;i< updateFields.Length;i++)
                        {
                            if (updateString.Length == 0)
                            {
                                updateString = updateFields[i] + "='" + row[updateFields[i]].ToString() + "'";
                            }
                            else
                            {
                                updateString = updateString + "," + updateFields[i] + "='" + row[updateFields[i]].ToString() + "'";
                            }
                        }

                        for (int i = 0; i < whereFields.Length; i++)
                        {
                            if (whereString.Length == 0)
                            {
                                whereString = whereFields[i] + "='" + row[whereFields[i]].ToString() + "'";
                            }
                            else
                            {
                                whereString = whereString + " and " + whereFields[i] + "='" + row[whereFields[i]].ToString() + "'";
                            }
                        }


                        string lineTxt = "Update  " + tbName + " Set " + updateString + "  Where " + whereString ;
                        _objWriter.WriteLine(lineTxt);
                        lineTxt = "Go";
                        _objWriter.WriteLine(lineTxt);
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
