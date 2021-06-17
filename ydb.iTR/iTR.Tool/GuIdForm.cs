using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iTR.Tool
{
    public partial class GuIdForm : Form
    {
        public GuIdForm()
        {
            InitializeComponent();
        }

        private void calssExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Generate_Click(object sender, EventArgs e)
        {
            try
            {
                if (number.Text.Trim().Length == 0)
                    new Exception ("Number box is empty");
                for(int i=0;i<int.Parse(number.Text.Trim());i++)
                {
                    guidBox.Text = guidBox.Text+Guid.NewGuid()+ "\r\n";
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
