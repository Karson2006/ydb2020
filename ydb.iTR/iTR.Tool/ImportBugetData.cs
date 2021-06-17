using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using iTR.Lib;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace iTR.Tool
{
    public partial class ImportBugetData : Form
    {
        public ImportBugetData()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ImportBugetData_Load(object sender, EventArgs e)
        {
            excelFile.ShowReadOnly = true;
            excelFile.Filter = "Excel File|*.xls;*.xlsx";
        }

        private void btnSelectExcelFile_Click(object sender, EventArgs e)
        {
            excelFile.ShowDialog();
            excelFilePath.Text = excelFile.FileName;
        }

        private void ImportData_Click(object sender, EventArgs e)
        {
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                string[] depts = txDeparts.Text.Split('|');
                string[] sheets = tbSheet.Text.Split('|');
                if (depts.Length != sheets.Length)
                    throw new Exception("成本中心个数与Excel页签个数不一致");
                decimal year = Numyear.Value;
                for(int i=0;i<depts.Length ;++i)
                {
                    string sql = "Delete from BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString();
                    runner.ExecuteSqlNone(sql);

                    string excelCols = ConfigurationManager.AppSettings["BugetExcelCols"];
                    string excelversion = "";
                    if (Path.GetExtension(excelFilePath.Text) == ".xls")
                        excelversion = "8.0";
                    else
                        excelversion = "12.0";

                    sql = "Insert into BugetData (FCompany,FDeptName,FYear,FAcctCode,FAcctName,M1,M2,M3,M4,M5,M6,M7,M8,M9,M10,M11,M12) select '" + tbCompany.Text + "','" + depts[i] + "','" + year.ToString() + "'," + excelCols;
                    sql = sql + " From  OPENDATASOURCE('Microsoft.Ace.OleDb.12.0','Extended Properties=\"Excel " + excelversion + ";HDR=YES;IMEX=1\";Data Source=\"" + excelFilePath.Text + "\"')...[" + sheets[i] + "$] Where 合计>0";
                
                    runner.ExecuteSqlNone(sql);

                    #region Insert Data into Buget_Month
                      sql = "Delete from Buget_Month Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString();
                      runner.ExecuteSqlNone(sql);
                      sql = "Insert Into Buget_Month(FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear,FMonth,FAmout,FQuater)" +
                          "("+
	                          "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '1' As FMonth,Isnull(M1,0) As FAmout,'Q1' As FQuater "  +
	                          "From BugetData  Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() + 
                          ")"+
                          "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '2' As FMonth,Isnull(M2,0) As FAmout,'Q1'As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                         "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '3' As FMonth,Isnull(M3,0) As FAmout,'Q1' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                        "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '4' As FMonth,Isnull(M4,0) As FAmout,'Q2' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                         "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '5' As FMonth,Isnull(M5,0) As FAmout,'Q2' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                          "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '6' As FMonth,Isnull(M6,0) As FAmout,'Q2' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                         " )"+
                         "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '7' As FMonth,Isnull(M7,0) As FAmout,'Q3' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                        "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '8' As FMonth,Isnull(M8,0) As FAmout,'Q3' As FQuater " +
                             " From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                         "Union"+
                          "("+
                               "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '9' As FMonth,Isnull(M9,0) As FAmout,'Q3' As FQuater " +
                               "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                        "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '10' As FMonth, Isnull(M10,0)As FAmou,'Q4' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                        "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '11' As FMonth,Isnull(M11,0) As FAmout,'Q4' As FQuater " +
                              "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString() +
                          ")"+
                         "Union"+
                          "("+
                              "Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear, '12' As FMonth,Isnull(M12,0) As FAmout,'Q4' As FQuater " +
	                          "From BugetData Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString()+")";
                      runner.ExecuteSqlNone(sql);
                    #endregion
                    #region
                      sql = "Delete from Buget_Quarter Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString();
                      runner.ExecuteSqlNone(sql);
                      sql = " Insert Into Buget_Quarter(FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear,FQuater,FAmout) " +
                            " Select FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear,FQuater,Sum(Isnull(FAmout,0)) As FAmout " +
                            " From Buget_Month  Where FCompany='" + tbCompany.Text + "' and FDeptName='" + depts[i] + "' and FYear=" + year.ToString()+
                            " Group by FCompany,FDeptCode,FDeptName,FAcctCode,FAcctName,FYear,FQuater ";
                      runner.ExecuteSqlNone(sql);
                    #endregion


                }

            }
            catch(Exception err)
            {
                throw err;
            }
        }

        private void MonthBuget_Click(object sender, EventArgs e)
        {
            try
            {
                if(excelExportFolder.Text.Trim().Length ==0)
                    throw new Exception("Excel文件导出文件夹不能为空");
                System.Data.DataTable tb =null;
                string sql = " Select FCompany As 公司,FDeptName As 成本中心,FDeptCode As 成本中心编码,FAcctCode AS 科目编码,FAcctName AS 科目,FYear AS 年度,FMonth As 期间,FAmout As 期初额,FAmout As 余额" +
                            " FROM Buget_Month ";
                SQLServerHelper runner = new SQLServerHelper();
                tb=runner.ExecuteSql(sql);
                string excelFileName = excelExportFolder.Text.Trim();
                if(excelFileName.Substring(excelFileName.Length-1,1)!= "\\" )
                    excelFileName = excelFileName + "\\成本中心月度预算导入.xlsx";
                else
                    excelFileName = excelFileName + "成本中心月度预算导入.xlsx";

                ExportExcel(tb, excelFileName);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "系统提示");
            }
        }

        public void ExportExcel(System.Data.DataTable  tb, string saveFileName)
        {
            try
            {
               

                bool fileSaved = false;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    throw new Exception( "创建Excel对象错误");
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1  
                //写入字段  
                for (int i = 0; i < tb.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = tb.Columns[i].ColumnName;
                }
                //写入数值  
                for (int r = 0; r < tb.Rows.Count; r++)
                {
                    for (int i = 0; i < tb.Columns.Count; i++)
                    {
                        worksheet.Cells[r + 2, i + 1] = tb.Rows[r][i];
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。  
                if (saveFileName != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(saveFileName);
                        fileSaved = true;
                    }
                    catch (Exception ex)
                    {
                        fileSaved = false;
                        MessageBox.Show("导出文件时出错,请先关闭该文件！\n" + ex.Message);
                    }
                }
                else
                {
                    fileSaved = false;
                }
                xlApp.Quit();
                GC.Collect();//强行销毁  
                if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL  
              
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            excelfolderBrowser.ShowNewFolderButton = true;
            excelfolderBrowser.ShowDialog();
            excelExportFolder.Text = excelfolderBrowser.SelectedPath;
        }

        private void QuaterBuget_Click(object sender, EventArgs e)
        {
            try
            {
                if (excelExportFolder.Text.Trim().Length == 0)
                    throw new Exception("Excel文件导出文件夹不能为空");
                System.Data.DataTable tb = null;
                string sql = " Select FCompany As 公司,FDeptName As 成本中心,FDeptCode As 成本中心编码,FAcctCode AS 科目编码,FAcctName AS 科目,FYear AS 年度,FQuater As 季度,FAmout As 期初额,FAmout As 余额" +
                            " FROM Buget_Quarter ";
                SQLServerHelper runner = new SQLServerHelper();
                tb = runner.ExecuteSql(sql);
                string excelFileName = excelExportFolder.Text.Trim();
                if (excelFileName.Substring(excelFileName.Length - 1, 1) != "\\")
                    excelFileName = excelFileName + "\\成本中心季度预算导入.xlsx";
                else
                    excelFileName = excelFileName + "成本中心季度预算导入.xlsx";

                ExportExcel(tb, excelFileName);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "系统提示");
            }
        }  
    }
}
