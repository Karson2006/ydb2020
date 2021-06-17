using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using iTR.Lib;

namespace iTR.Tool
{
    public partial class OADataClearner : Form
    {
        string selectedMTable = "";
        string selectedSTables = "";
        public OADataClearner()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void OADataClearner_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "Select [Name],[ID] from OAServer.v3x.dbo.CTP_TEMPLATE_CATEGORY " +
                             " Where isnull(State,'')='' and ORG_ACCOUNT_ID=670869647114347 and IS_DELETE=0 order by [Name] Asc";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                lstCategory.Items.Clear();
                foreach(DataRow row in dt.Rows)
                {
                    lstCategory.Items.Add(row["Name"].ToString() + "                                    |" + row["ID"].ToString());
                }

                lvFormList.View = View.Details;//设置视图  
                lvFormList.Columns.Add("表单", 180, HorizontalAlignment.Center);
                lvFormList.Columns.Add("是否启用", 60, HorizontalAlignment.Center);
                lvFormList.Columns.Add("主表名", 130, HorizontalAlignment.Center);
                lvFormList.Columns.Add("从表名", 240, HorizontalAlignment.Center);
                lvFormList.Columns.Add("ID", 0, HorizontalAlignment.Center);

                lvOARecord.View = View.Details;//设置视图  
                lvOARecord.Columns.Add("申请人", 60, HorizontalAlignment.Center);
                lvOARecord.Columns.Add("主题", 400, HorizontalAlignment.Center);
                lvOARecord.Columns.Add("申请日期", 150, HorizontalAlignment.Center);
                lvOARecord.Columns.Add("状态", 60, HorizontalAlignment.Center);
                lvOARecord.Columns.Add("Id_String", 0, HorizontalAlignment.Center);

                Date1.Value = DateTime.Now.AddDays(-7);
                Date2.Value = DateTime.Now;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

       
        private void RefreshTableList(DataTable dt)
        {
            lvFormList.Items.Clear();
            if (dt.Rows.Count == 0) return;
            foreach (DataRow row in dt.Rows)
            {
                var item = new ListViewItem();
                item.Text = row["Name"].ToString();
                item.SubItems.Add(row["UseFlag"].ToString());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(row["FIELD_INFO"].ToString());
                XmlNodeList tblist = doc.SelectNodes("TableList/Table");

                string masterTb = "", slaveTables = "";
                foreach (XmlNode node in tblist)
                {
                    if (node.Attributes["tabletype"].Value == "master")
                    {
                        masterTb = node.Attributes["name"].Value;
                    }
                    else
                    {
                        slaveTables = node.Attributes["name"].Value + "|" + slaveTables;
                    }
                }
                item.SubItems.Add(masterTb);
                if (slaveTables.Length > 1) slaveTables = slaveTables.Substring(0, slaveTables.Length - 1);
                item.SubItems.Add(slaveTables);
                lvFormList.Items.Add(item);
            }
        }

        private void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCategory.SelectedItem.ToString() == "") return;
            string categoryID = lstCategory.SelectedItem.ToString().Split('|')[1];
            string sql = "Select [Name],[ID], (Case Use_Flag when 1 then '是'  else '否' End) AS UseFlag,FIELD_INFO from  OAServer.v3x.dbo.Form_Definition where Category_ID = {0} order by Delete_Flag ASC,Name ASC";
            sql = string.Format(sql, categoryID);
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            RefreshTableList(dt);
        }

        private void Query_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvFormList.SelectedItems.Count > 0)
                {

                    ListViewItem selectedItem = lvFormList.SelectedItems[0];
                    selectedMTable = selectedItem.SubItems[2].Text;
                    selectedSTables = selectedItem.SubItems[3].Text;
                    
                    RefreshOARecordList();
                }
                else
                {
                    throw new Exception("请先选择表单");
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void RefreshOARecordList()
        {
            lvOARecord.Items.Clear();

            string sql = "Select t2.Name, t3.Subject,t1.start_date,(Case t1.finishedflag when 1 then '结束' Else '进行中' End) As finishedflag ,t1.ID As Form_RecordID,t3.ID As OBJECT_ID,t3.PROCESS_ID" +
                           " From OAServer.v3x.dbo.{0} t1" +
                           " Left Join OAServer.v3x.dbo.ORG_MEMBER  t2  on t1.start_member_id = t2.ID" +
                           " Left Join  OAServer.v3x.dbo.Col_Summary t3 On t1.ID =t3.Form_RecordID" +
                           " Where t1.start_date between  '{1}' and  '{2}'";
            sql = string.Format(sql, selectedMTable, Date1.Value.ToString("yyyy-MM-dd 0:0:0.000"), Date2.Value.ToString("yyyy-MM-dd 23:59:59.999"));
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);

           
            if (dt.Rows.Count == 0) return;
            foreach (DataRow row in dt.Rows)
            {
                var item = new ListViewItem();
                item.Text = row["Name"].ToString();
                item.SubItems.Add(row["Subject"].ToString());
                item.SubItems.Add(row["start_date"].ToString());
                item.SubItems.Add(row["finishedflag"].ToString());
                item.SubItems.Add(row["Form_RecordID"].ToString()+"|"+row["OBJECT_ID"].ToString() + "|" + row["PROCESS_ID"].ToString());
               
                lvOARecord.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                OAInterface oa = new OAInterface();
                foreach (ListViewItem item in lvOARecord.Items)
                {
                    if (item.Checked)
                    {
                        oa.DeleteFormReocord(selectedMTable + "|" + selectedSTables, item.SubItems[4].Text);
                    }
                }
                RefreshOARecordList();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            this.Cursor = Cursors.Arrow;
        }

        private void lvFormList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvFormList.SelectedItems.Count>0)
            {
                ListViewItem item  = lvFormList.SelectedItems[0];
                lblSelectForm.Text = item.SubItems[0].Text + "  " + item.SubItems[2].Text +"  " + item.SubItems[3].Text;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem item in  lvOARecord.Items)
            {
                item.Checked = checkBox1.Checked;
            }
        }
        
    }
}
