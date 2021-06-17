using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;
using iTR.Lib;

namespace iTR.Tool
{
    public partial class WebServiceForm : Form
    {
        private XmlDocument doc = new XmlDocument();

        public WebServiceForm()
        {
            InitializeComponent();
        }

        private void calssExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }






        private void WebServiceForm_Load(object sender, EventArgs e)
        {
            try
            {
                string interfacefilename = AppDomain.CurrentDomain.BaseDirectory + "\\interface_WS.xml";

                if (!File.Exists(interfacefilename))
                    throw new Exception("接口配置文件Interface.xml不存在");
                doc.Load(interfacefilename);
                XmlNodeList classNodes = doc.SelectNodes("InterFace/WS");
                classList.Items.Clear();
                foreach (XmlNode node in classNodes)
                {
                    classList.Items.Add(node.Attributes["Class"].Value + "                      |" + node.Attributes["Http"].Value);
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void classList_SelectedValueChanged(object sender, EventArgs e)
        {
            
            RefreshMethodList();
        }
        private void RefreshMethodList()
        {
            if (classList.SelectedItem == null) return;
            methodList.Items.Clear();
            returns.Text = "";   
            XmlNodeList nodes = doc.SelectNodes("InterFace/WS");
            string [] classAttributes = classList.SelectedItem.ToString().Split('|');

            foreach(XmlNode classNode in nodes)
            {
                if (classNode.Attributes["Class"].Value == classAttributes[0].Trim())
                {
                    XmlNodeList methodeNodes = classNode.SelectNodes("Method");
                    foreach (XmlNode methodNode in methodeNodes)
                        methodList.Items.Add(methodNode.Attributes["Name"].Value + "                               |" + classAttributes[0].Trim() + "|" + classAttributes[1].Trim());
                    returns.Text = "WS Http:" + classNode.Attributes["Http"].Value + "\r\nWS Description:" + classNode.Attributes["Description"].Value;
                   
                }
            }
            
        }

        private void classInvoke_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (methodList.SelectedItem == null)
                    throw new Exception("请先选择方法");
                if(parameterBox.Text.Trim().Length==0)
                    throw new Exception("调用参数不能为空");
                string [] methodString = methodList.SelectedItem.ToString().Split('|');
                object[] p = parameterBox.Text.Split(';');
                string[] classString = methodString[1].Trim().Split('.');//ydb.CommonService.Invoke

                WebInvoke invoke = new WebInvoke();
                outputBox.Text = invoke.Invoke(methodString[2].Trim(), classString[classString.Length-1].Trim(), methodString[0].Trim(), p).ToString();
                
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void returns_TextChanged(object sender, EventArgs e)
        {

        }

        private void methodList_SelectedValueChanged(object sender, EventArgs e)
        {
            parameters.Text = "";
            parameterBox.Text = "";
            outputBox.Text = "";
            XmlNodeList nodes = doc.SelectNodes("InterFace/WS");
            string[] classAttributes = classList.SelectedItem.ToString().Split('|');

            foreach (XmlNode classNode in nodes)
            {
                if (classNode.Attributes["Class"].Value == classAttributes[0].Trim())
                {
                    XmlNodeList methodeNodes = classNode.SelectNodes("Method");
                    foreach (XmlNode methodNode in methodeNodes)
                    {
                        string[] methodAttributes =methodList.SelectedItem.ToString().Split('|');
                        if (methodNode.Attributes["Name"].Value == methodAttributes[0].Trim())
                        {
                            XmlNode returnNode = methodNode.SelectSingleNode("Return");
                            parameters.Text = "Return Type:" + returnNode.Attributes["Type"].Value + "\r\n";
                            XmlNodeList pNodes =methodNode.SelectNodes("Parameter");
                            foreach(XmlNode pNode in pNodes)
                            {
                                parameters.Text = parameters.Text + "Parameter [" + pNode.Attributes["Name"].Value + "]," + pNode.Attributes["Type"].Value+ "\r\n";
                            }
                        }
                    }
                }
            }

            
        }

        private void saveResult_Click(object sender, EventArgs e)
        {
            if(outputBox.Text.Length >0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outputBox.Text);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory + "\\Result.xml");
            }

        }
    }
}
