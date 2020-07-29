using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ydb.Tool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                ToolStripButton b = (ToolStripButton)sender;
                switch (b.Name)
                {

                    case "tbEmployee":
                        EmployeeForm f = new EmployeeForm();
                        f.ShowDialog();
                        break;
                    case "tbHospitalAuth":
                        AuthDataForm f2 = new AuthDataForm();
                        f2.ShowDialog();
                        break;
                    case "tbDeptment":
                        DeptForm f3 = new DeptForm();
                        f3.ShowDialog();
                        break;
                    case "tbExit":
                        this.Dispose();
                        break;
                    case "tbSqlScript":
                        SQLGenerator f4 = new SQLGenerator();
                        f4.ShowDialog(); 
                        break;
                        

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "系统提示");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iTR.Lib.SQLServerHelper runner = new iTR.Lib.SQLServerHelper(ydb.DataService.DataHelper.CnnString);
            string sql = " Select * from v3x.dbo.ORG_MEMBER";
            DataTable dt = runner.ExecuteSql(sql);
            iTR.Lib.FileLogger.WriteLog(DateTime.Now.ToString() + " ExportToExcel Start",0); 
            iTR.Lib.ExcelHelper.ExportToExcel(dt, @"D:\test.xlsx");
            iTR.Lib.FileLogger.WriteLog(DateTime.Now.ToString() + " ExportToExcel End",0);

        }
    }
    
}
