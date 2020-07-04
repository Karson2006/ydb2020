using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;

namespace ydb.BLL
{
    public class RegRepresentative
    {

        public RegRepresentative() 
        { }

        #region List

        public  string  ListXml(string filter = "")
        {
            string result = "";
            result = DataTableToXml(List(filter));
            return result;
        }
        private  DataTable  List(string filter="")
        {
            DataTable result = new DataTable();
            try
            {
                SQLServerHelper runer = new SQLServerHelper();
                string sql = "Select  t1.*,t2.FName As FTypeName,t3.FName As FProductName,t4.FName As FProvinceName,t5.FName As FCityName,"+
                            " t6.FName As FCountryName,t6.FName As FCountryName,t7.FName As FApproveryName" +
                            " From Reg_Application t1"+
                            " Left Join t_items t2 On t2.FID = t1.FTypeID"+
                            " Left Join t_items t3 On t3.FID = t1.FProductID"+
                            " Left Join t_items t4 On t4.FID = t1.FProvinceID"+
                            " Left Join t_items t5 On t5.FID = t1.FCityID"+
                            " Left Join t_items t6 On t6.FID = t1.FCountryID"+
                            " Left Join t_items t7 On t7.FID = t1.FApproverID";

                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                result = runer.ExecuteSql(sql);

            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion


        #region Update
        public  string Update(string dataString)
        {
            string id = "", sql = "",  valueString = "",result = "-1";
            
            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                id = doc.SelectSingleNode("UpdateRegistration/ID").InnerText;

                if (id.Trim() == "" || id.Trim() == "-1")//新增
                {
                    id = Guid.NewGuid().ToString();
                    sql = "Insert into Reg_Application(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入新日程失败
                        throw new Exception("新建失败");
                }
                //更新信息
                vNode = doc.SelectSingleNode("UpdateRegistration/FApplicant");
                string val = "";
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApplicant='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FIsCompany");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIsCompany='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FTypeID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTypeID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FInviter");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInviter='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FInviterMobile");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInviterMobile='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRegistration/FInviteCode");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInviteCode='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FRegisted");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRegisted='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FProductID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProductID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProvinceID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FCityID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCityID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FCountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCountryID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FApproverID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApproverID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/FApproveDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApproveDate='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update Reg_Application Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新失败");
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            result = id;

            return result;
        }
        #endregion

        #region DataTableToXml
        private string DataTableToXml(DataTable dt)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetRegApplicationList>" +
                                "<Result>False</Result>" +
                                "<Description></Description>" +
                                "<DataRows></DataRows>" +
                                "</GetRegApplicationList>";
            if(dt.Rows.Count>0)
            {
                XmlDocument doc = new XmlDocument();
                
                doc.LoadXml(result);
                doc.SelectSingleNode("GetRegApplicationList/Result").InnerText = "True";

                XmlNode pNode = doc.SelectSingleNode("GetRegApplicationList/DataRows");
                foreach (DataRow row in dt.Rows)
                {
                    XmlNode cNode = doc.CreateElement("DataRow");
                    pNode.AppendChild(cNode);

                    XmlNode vNode = doc.CreateElement("ID");
                    vNode.InnerText = row["FID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FApplicant");
                    vNode.InnerText = row["FApplicant"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FIsCompany");
                    vNode.InnerText = row["FIsCompany"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FTypeID");
                    vNode.InnerText = row["FTypeID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FTypeName");
                    vNode.InnerText = row["FTypeName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FDate");
                    vNode.InnerText = row["FDate"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FInviter");
                    vNode.InnerText = row["FInviter"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FInviterMobile");
                    vNode.InnerText = row["FInviterMobile"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FInviteCode");
                    vNode.InnerText = row["FInviteCode"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FRegisted");
                    vNode.InnerText = row["FRegisted"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FProductID");
                    vNode.InnerText = row["FProductID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FProvinceID");
                    vNode.InnerText = row["FProvinceID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FCityID");
                    vNode.InnerText = row["FCityID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FCountryID");
                    vNode.InnerText = row["FCountryID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FApproverID");
                    vNode.InnerText = row["FApproverID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FApproveDate");
                    vNode.InnerText = row["FApproveDate"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FProductName");
                    vNode.InnerText = row["FProductName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FProvinceName");
                    vNode.InnerText = row["FProvinceName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FCityName");
                    vNode.InnerText = row["FCityName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FCountryName");
                    vNode.InnerText = row["FCountryName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FApproveryName");
                    vNode.InnerText = row["FApproveryName"].ToString();
                    cNode.AppendChild(vNode);
                }
                result = doc.OuterXml;
                return result;
            }
            return result;
        }
        #endregion 


    }

}
