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
    public partial class EmployeeForm : Form
    {
        private ydb.DataService.Employee dataEmployee = null;
        private DataTable dt = null;

        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            dataEmployee = new ydb.DataService.Employee();
            FID.Width = 130;
            FEmployeeNumber.Width = 80;

            //string sql = @" Select *,'0' As FSortIndex,'0' As FIsAgency,'' As FIDNumber,'' As FTypeID,'0' As FUploadOption from  [DataService].dbo.OAEmployee where 1=0";
            //dt = runner.ExecuteSql(sql);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                switch (b.Name.ToUpper())
                {
                    case "BTGETDATA":
                        dt = dataEmployee.GetUploadDataFromOA();
                        RefreshData(dt);
                        break;

                    case "BTUPLOADDATA":
                        if (CheckUploadData())
                        {
                            UpdateData();
                            dataEmployee.Upload(dt);
                            dt = dataEmployee.GetUploadDataFromOA();
                            RefreshData(dt);
                            throw new Exception("上传成功");
                        }
                        break;

                    case "BTEXIT":
                        this.Dispose();
                        break;

                    case "BTSEARCH":
                        RefreshData(QueryData(dt, txtKeyword.Text));
                        break;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "系统提示");
            }
            //dataEmployee.Upload(dt);
        }

        private void RefreshData(DataTable dt)
        {
            dgEmployee.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                DataGridViewRow dvRow = new DataGridViewRow();
                int index = dgEmployee.Rows.Add(dvRow);

                dgEmployee.Rows[index].Cells[0].Value = dr["FID"].ToString();
                dgEmployee.Rows[index].Cells[1].Value = dr["FEmployeeNumber"].ToString();
                dgEmployee.Rows[index].Cells[2].Value = dr["FEmployeeName"].ToString();
                dgEmployee.Rows[index].Cells[3].Value = dr["FPositionName"].ToString();
                dgEmployee.Rows[index].Cells[4].Value = dr["FDeptName"].ToString();
                dgEmployee.Rows[index].Cells[5].Value = dr["FMobile"].ToString();
            }
        }

        private void UpdateData()
        {
            //DataTable updateDt = dt.Clone();
            for (int i = 0; i < dgEmployee.Rows.Count; i++)
            {
                if (dgEmployee.Rows[i].Cells[7].Value != null && bool.Parse(dgEmployee.Rows[i].Cells[7].Value.ToString()))
                {
                    string id = dgEmployee.Rows[i].Cells[0].Value.ToString().Trim();
                    if (id.Length > 0)
                    {
                        DataRow[] rows = dt.Select("FID='" + id + "'");
                        if (rows.Length > 0)
                        {
                            if (dgEmployee.Rows[i].Cells[6].Value != null && dgEmployee.Rows[i].Cells[6].Value.ToString().Trim().Length > 0)
                            {
                                string typeString = dgEmployee.Rows[i].Cells[6].Value.ToString().Trim();
                                string typeID = typeString.Split('|')[1];
                                rows[0]["FTypeID"] = typeID;
                                rows[0]["FUploadOption"] = 1;
                            }
                            else
                                rows[0]["FUploadOption"] = 0;
                            //else
                            //    throw new Exception("类型不能为空");
                        }
                    }
                }
            }
        }

        private bool CheckUploadData()
        {
            bool result = false;
            int uplodRowCount = 0;
            try
            {
                for (int i = 0; i < dgEmployee.Rows.Count; i++)
                {
                    if (dgEmployee.Rows[i].Cells[7].Value != null && bool.Parse(dgEmployee.Rows[i].Cells[7].Value.ToString()))
                    {
                        if (dgEmployee.Rows[i].Cells[5].Value == null || dgEmployee.Rows[i].Cells[5].Value.ToString().Length == 0)
                            throw new Exception("第" + Convert.ToString(i + 1) + "行手机号码不能为空");

                        if (dgEmployee.Rows[i].Cells[6].Value == null || dgEmployee.Rows[i].Cells[6].Value.ToString().Length == 0)
                            throw new Exception("第" + Convert.ToString(i + 1) + "行类型不能为空");
                    }
                    else
                        uplodRowCount = uplodRowCount + 1;
                }

                if (uplodRowCount == dgEmployee.Rows.Count)
                    throw new Exception("请选择要上传的数据");
            }
            catch (Exception err)
            {
                throw err;
            }
            result = true;
            return result;
        }

        private void dgEmployee_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private DataTable QueryData(DataTable dt, string keyword)
        {
            DataTable result = dt.Clone();

            DataRow[] rows = dt.Select("FEmployeeName like '%" + keyword + "%'");
            for (int i = 0; i < rows.Length; i++)
            {
                //DataRow row1 = result.NewRow();
                result.ImportRow(rows[i]);

                //result.Rows.Add(row1);
            }

            return result;
        }
    }
}