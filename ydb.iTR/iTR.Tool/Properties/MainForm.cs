using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ydb.BLL;
using System.Xml;
using System.Security.Cryptography;

namespace iTR.Tool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

      

        

        private void tsMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           
        }

        private void tsRSA_Click(object sender, EventArgs e)
        {
            try
            {
                Form f= new RSAForm();
                f.Show();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tsInvoke_Click(object sender, EventArgs e)
        {
             
            try
            {
                Form f = new ResponeForm();
                f.ShowDialog();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Form f = new WebServiceForm();
                f.Show();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void toolGuid_Click(object sender, EventArgs e)
        {
            GuIdForm f = new GuIdForm();
            f.ShowDialog();
        }

        private void comTools_Click(object sender, EventArgs e)
        {
            ComTools f = new ComTools();
            f.ShowDialog();
        }

        private void tbSQLScript_Click(object sender, EventArgs e)
        {
            SQLGenerator f = new SQLGenerator();
            f.ShowDialog(); 
        }

        private void bugetData_Click(object sender, EventArgs e)
        {
            //ImportBugetData f = new ImportBugetData();
            //f.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //OADataClearner f = new OADataClearner();
            //f.Show();
        }
    }
}