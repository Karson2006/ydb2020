using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;

namespace iTR.Lib
{
    public class ExcelHelper
    {
        /// <summary>
        /// 从指定（绝对路径）Excel文件读取相应的数据到DataTable中
        /// Excel文件不能加密
        /// </summary>
        /// <param name="excelFile">Excel文件名（绝对路径）</param>
        /// <param name="SelectCols">拟读取的列名，默认为*</param>
        /// <param name="where">过滤条件</param>
        /// <param name="orderby">排序字段</param>
        /// <returns></returns>
        public static DataTable GetDataFromExcel(string excelFile,string SelectCols="",string where ="",string orderby="")
        {
            DataTable result = new DataTable();

            try
            {
                FileInfo file = new FileInfo(excelFile);
                if (!file.Exists)
                {
                    throw new Exception(excelFile + " 文件不存在");
                }
                string extension = file.Extension;
                string cnnString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFile + ";Extended Properties='Excel 12.0 XML;HDR=YES;IMEX=1;';";
                if (extension==".xls")
                {
                     cnnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                }
                
                
                OleDbConnection dbCnn = new OleDbConnection(cnnString);
                dbCnn.Open();
                result = dbCnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (result != null && result.Rows.Count > 0)
                {
                    result.TableName = result.Rows[0]["TABLE_NAME"].ToString();
                }

                string sql = SelectCols;
                if (sql.Trim().Length == 0)
                    sql = "Select * ";
                else
                    sql = "Select  " + sql;

                sql = sql + " from  [" + result + "A:BU] ";

                if (where.Trim().Length > 0)
                    sql = sql + " Where  " + where;

                if (orderby.Trim().Length > 0)
                    sql = sql + " Order by " + orderby;

                OleDbCommand cmd = new OleDbCommand(sql, dbCnn);
                System.Data.OleDb.OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                result = new DataTable();
                
                da.Fill(result);

               
                dbCnn.Close();
                dbCnn=null;
                GC.Collect();

                return result;

            }
            catch( Exception err)
            {
                throw err;
            }

        }
        public static void ExportToExcel(DataTable dataTable, string fileName)
        {
            var lines = new List<string>();
            string[] columnNames = dataTable.Columns
                                            .Cast<DataColumn>()
                                            .Select(column => column.ColumnName)
                                            .ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable()
                            .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            File.WriteAllLines(fileName, lines);
           
        }


        public static void DataTabletoExcel(DataTable dt, string filePath,string pwd="")
        {
            try
            {
                var excel = new Microsoft.Office.Interop.Excel.Application
                {
                    DisplayAlerts = false
                };

                excel.Visible = false;
                excel.ScreenUpdating = false;
                excel.AlertBeforeOverwriting = false;

                //生成一个新的工资薄
                var excelworkBook = excel.Workbooks.Add(Type.Missing);
                var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                //获得表的行，列数目
                int row_num = dt.Rows.Count;
                int column_num = dt.Columns.Count;
                //生成一个二维数组
                object[,] dataArry = new object[row_num, column_num];
                object[,] headArry = new object[1, column_num];
                //把表中的数据放到数组中 
                for (int i = 0; i < row_num; i++)
                {
                    for (int j = 0; j < column_num; j++)
                    {
                        dataArry[i, j] = dt.Rows[i][j].ToString();
                    }
                }

                for (int j = 0; j < column_num; j++)
                {
                    headArry[0, j] = dt.Columns[j].ColumnName.ToString();
                }
                excel.Range[excel.Cells[1, 1], excel.Cells[1, column_num]].Value = headArry;
                //把数组中的数据放到Excel中
                excel.Range[excel.Cells[2, 1], excel.Cells[row_num + 1, column_num]].Value = dataArry;
                excelworkBook.Password = pwd;
                excelworkBook.SaveAs(filePath);
                excelworkBook.Close();
                excel.Quit();
                excel = null;
                GC.Collect();
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]

        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        public static void KillExcelProcess(Microsoft.Office.Interop.Excel.Application obj)
        {
            try
            {
                //杀死后台进程,不然无法写入
                IntPtr t = new IntPtr(obj.Hwnd);
                int id = 0;
                GetWindowThreadProcessId(t, out id);
                System.Diagnostics.Process P = System.Diagnostics.Process.GetProcessById(id);
                P.Kill();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
