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
    public partial class AuthDataForm : Form
    {
        private ydb.DataService.AuthData dataAuth= null;
        private DataTable dt = null;

        public AuthDataForm()
        {
            InitializeComponent();

        }

        private void Employees_Load(object sender, EventArgs e)
        {
            dataAuth = new ydb.DataService.AuthData();
            FHospitalNumber.Width = 100;
            FHospitalName.Width = 230;
            FMR.Width = 70;
            FType.Width = 70;
            FUploadOption.Width = 70;
            FTID.Width = 0;
            
        }


        private void ButtonClick(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                switch (b.Name.ToUpper())
                {
                    case "BTGETDATA":
                        dt = dataAuth.GetUploadDataFromOA();
                        RefreshData(dt);
                        break;
                    case "BTUPLOADDATA":
                        if (ChechUploadData())
                        {
                            UpdateData(dt);
                            dataAuth.Upload(dt);
                            dt = dataAuth.GetUploadDataFromOA();
                            RefreshData(dt);
                        }
                        break;
                    case "BTEXIT":
                        this.Dispose(); 
                        break;
                    case "BTSEARCH":
                        RefreshData(QueryData(dt,tbEmployeeName.Text,tbHospitalName.Text));
                        break;

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "系统提示"); 
            }
 
           
        }
        private void RefreshData(DataTable dt)
        {
            dgAuthData.Rows.Clear(); 
            foreach(DataRow dr in dt.Rows)
            {
                DataGridViewRow dvRow = new DataGridViewRow();
                int index = dgAuthData.Rows.Add(dvRow);

                dgAuthData.Rows[index].Cells[0].Value = dr["FInstitutionNumber"].ToString();
                dgAuthData.Rows[index].Cells[1].Value = dr["FInstitutionNanme"].ToString();
                dgAuthData.Rows[index].Cells[2].Value = dr["FObjectName"].ToString();
                dgAuthData.Rows[index].Cells[3].Value =GetTypeCaptionByID( dr["FAuthType"].ToString().Trim());
                dgAuthData.Rows[index].Cells[5].Value = dr["FTID"].ToString().Trim();
            }
        }

        private void UpdateData(DataTable dt)
        {
            for (int i = 0; i < dgAuthData.Rows.Count;i++ )
            {

                if (dgAuthData.Rows[i].Cells[4].Value != null && bool.Parse(dgAuthData.Rows[i].Cells[4].Value.ToString()))
                {
                    string id = dgAuthData.Rows[i].Cells[5].Value.ToString().Trim();
                    if (id.Length > 0)
                    {
                        DataRow[] rows = dt.Select("FTID='" + id + "'");
                        if (rows.Length > 0)
                        {
                            if (dgAuthData.Rows[i].Cells[4].Value != null && dgAuthData.Rows[i].Cells[4].Value.ToString().Trim().Length > 0)
                            {
                                rows[0]["FUploadOption"] = 1;
                            }
                        }
                    }
                }

            }
        }

        private bool ChechUploadData()
        {
            bool result = false;
            int checkedcount = 0;
            for(int i = 0;i<dgAuthData.Rows.Count;i++ )
            {
                if(dgAuthData.Rows[i].Cells[4].Value != null && dgAuthData.Rows[i].Cells[4].Value.ToString().Trim().Length>0)
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

            DataRow[] rows = dt.Select("FInstitutionNanme like '%" + keyword2 + "%' and FObjectName like '%" + keyword1 + "%'");
            for (int i = 0; i < rows.Length; i++)
            {
                
                result.ImportRow(rows[i]);

            }

            return result;
        }

        private void dgAuthData_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }
        private string GetTypeCaptionByID(string id)
        {
            string result = "";
            switch(id)
            {
                case "3":
                    result = "招商";
                    break;
                case "4":
                    result = "代表";
                    break;
                case "5":
                    result = "市场";
                    break;
                case "6":
                    result = "管理";
                    break;
            }
            return result;
        }

        private void ChkAllSelected_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgAuthData.Rows.Count; i++)
            {
                dgAuthData.Rows[i].Cells[4].Value = ChkAllSelected.Checked = true ? true : false; 
            }

        }
    }
}
