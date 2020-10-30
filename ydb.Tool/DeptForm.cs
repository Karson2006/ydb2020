using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ydb.DataService;



namespace ydb.Tool
{
    public partial class DeptForm : Form
    {
        private ydb.DataService.Department deptData= null;
        private DataTable dt = null;

        public DeptForm()
        {
            InitializeComponent();

        }

        private void Employees_Load(object sender, EventArgs e)
        {
            deptData = new ydb.DataService.Department();
            FDeptNumber.Width = 100;
            FDeptName.Width = 180;
            FSupervisor.Width = 100;
            FParentName.Width = 100;
            FUploadOption.Width = 70;
            FDeptID.Width = 0;
            
        }


        private void ButtonClick(object sender, EventArgs e)
        {
            //try
            //{
                Button b = (Button)sender;
                switch (b.Name.ToUpper())
                {
                    case "BTGETDATA":
                        dt = deptData.GetUploadDataFromOA();
                        RefreshData(dt);
                        break;
                    case "BTUPLOADDATA":
                        if (ChechUploadData())
                        {
                            UpdateData(dt);
                            deptData.Upload(dt);
                            dt = deptData.GetUploadDataFromOA();
                            RefreshData(dt);
                        }
                        break;
                    case "BTEXIT":
                        this.Dispose(); 
                        break;
                    case "BTSEARCH":
                        RefreshData(QueryData(dt,tbSupervisorName.Text,tbDeptName.Text));
                        break;

                }
            //}
            //catch (Exception err)
            //{
            //    MessageBox.Show(err.Message, "系统提示"); 
            //}
 
           
        }
        private void RefreshData(DataTable dt)
        {
            dgDeptData.Rows.Clear(); 
            foreach(DataRow dr in dt.Rows)
            {
                DataGridViewRow dvRow = new DataGridViewRow();
                int index = dgDeptData.Rows.Add(dvRow);

                dgDeptData.Rows[index].Cells[0].Value = dr["FDeptNumber"].ToString();
                dgDeptData.Rows[index].Cells[1].Value = dr["FDeptName"].ToString();
                dgDeptData.Rows[index].Cells[2].Value = dr["FSupervisorName"].ToString();
                dgDeptData.Rows[index].Cells[3].Value =dr["FParentName"].ToString().Trim();
                dgDeptData.Rows[index].Cells[5].Value = dr["FTID"].ToString().Trim();
            }
        }

        private void UpdateData(DataTable dt)
        {
            for (int i = 0; i < dgDeptData.Rows.Count;i++ )
            {

                if (dgDeptData.Rows[i].Cells[4].Value != null && bool.Parse(dgDeptData.Rows[i].Cells[4].Value.ToString()))
                {
                    string id = dgDeptData.Rows[i].Cells[5].Value.ToString().Trim();
                    if (id.Length > 0)
                    {
                        DataRow[] rows = dt.Select("FTID='" + id + "'");
                        if (rows.Length > 0)
                        {
                            if (dgDeptData.Rows[i].Cells[4].Value != null && dgDeptData.Rows[i].Cells[4].Value.ToString().Trim().Length > 0)
                            {
                                rows[0]["FUploadOption"] = 1;
                            }
                            else
                                rows[0]["FUploadOption"] = 1;
                        }
                    }
                }

            }
        }

        private bool ChechUploadData()
        {
            bool result = false;
            int checkedcount = 0;
            for(int i = 0;i<dgDeptData.Rows.Count;i++ )
            {
                if(dgDeptData.Rows[i].Cells[4].Value != null && dgDeptData.Rows[i].Cells[4].Value.ToString().Trim().Length>0)
                {
                    checkedcount = checkedcount + 1;
                }
            }
            if (checkedcount == 0)
                throw new Exception("请选择要上传的数据");
            result = true;

            return result;
        }

       

        private DataTable QueryData(DataTable dt,string keyword1,string keyword2)
        {
            DataTable result = dt.Clone();

            DataRow[] rows = dt.Select("FSupervisorName like '%" + keyword1 + "%' and FDeptName like '%" + keyword2 + "%'");
            for (int i = 0; i < rows.Length; i++)
            {
                
                result.ImportRow(rows[i]);

            }

            return result;
        }

        private void dgDeptData_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }
    
    }
}
