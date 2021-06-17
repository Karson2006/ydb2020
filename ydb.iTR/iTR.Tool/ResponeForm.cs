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

namespace iTR.Tool
{
    public partial class ResponeForm : Form
    {
        private XmlDocument doc = new XmlDocument();

        public ResponeForm()
        {
            InitializeComponent();
        }

        private void calssExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ResponeForm_Load(object sender, EventArgs e)
        {
            string interfacefilename = AppDomain.CurrentDomain.BaseDirectory + "\\interface_CL.xml";
           
            if (!File.Exists(interfacefilename))
                throw new Exception("接口配置文件Interface.xml不存在");
            doc.Load(interfacefilename);
            XmlNodeList classNodes = doc.SelectNodes("InterFace/Class");
            classList.Items.Clear();
            foreach (XmlNode node in classNodes)
            {
                classList.Items.Add(node.Attributes["Name"].Value + "                                |" + node.Attributes["Namespace"].Value + "|" + node.Attributes["Dll"].Value);
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
            XmlNodeList nodes = doc.SelectNodes("InterFace/Class");
            string [] classAttributes = classList.SelectedItem.ToString().Split('|');

            foreach(XmlNode classNode in nodes)
            {
                if(classNode.Attributes["Name"].Value==classAttributes[0].Trim())
                {
                    XmlNodeList methodeNodes = classNode.SelectNodes("Method");
                    foreach (XmlNode methodNode in methodeNodes)
                        methodList.Items.Add(methodNode.Attributes["Name"].Value + "                               |" + classAttributes[0].Trim() + "|" + classAttributes[1].Trim()+ "|" + classAttributes[2].Trim());
                    returns.Text = "Class NameSpace:" + classNode.Attributes["Namespace"].Value+ "\r\nDll File:" + classNode.Attributes["Dll"].Value;
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
                Assembly ass = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory+methodString[3].Trim());

                Type t = ass.GetType(methodString[2].Trim()+"."+methodString[1].Trim());
                MethodInfo info = t.GetMethod(methodString[0].Trim()); 
                object ob = Activator.CreateInstance(t);
                object [] p =parameterBox.Text.Split(';');
                outputBox.Text = info.Invoke(ob, p).ToString(); 
                
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
            XmlNodeList nodes = doc.SelectNodes("InterFace/Class");
            string[] classAttributes = classList.SelectedItem.ToString().Split('|');

            foreach (XmlNode classNode in nodes)
            {
                if (classNode.Attributes["Name"].Value == classAttributes[0].Trim())
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
                                parameters.Text = parameters.Text + "Param [" + pNode.Attributes["Name"].Value + "]," + pNode.Attributes["Type"].Value+ "\r\n";
                            }
                        }
                    }
                }
            }
            
        }

        private void saveResult_Click(object sender, EventArgs e)
        {
            if (outputBox.Text.Length > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(outputBox.Text);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory + "\\Result.xml");
            }

        }
    }
}
