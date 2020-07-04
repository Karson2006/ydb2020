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
    public class Finance
    {
        Items iClass;
        public Finance()
        {
            iClass = new Items();
        }

        #region GetCompanyList
        public string GetCompanyList(string xmlString)
        {
            string result = "", nodeString = "GetCompanyList",filter="",val="";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(nodeString + "/ID");

                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/Name");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FName like '%" + val + "%'" : "t1.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode(nodeString + "/Group");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FParentID = '" + val + "'" : "t1.FParentID = '" + val + "'";
                }

                string sql = "SELECT FID,FNumber ,FName,FParentID,LeaderID FROM t_Company t1";
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                SQLServerHelper runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);

                result = Common.DataTableToXml(dt, "GetCompanyList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetCostCenterList
        public string GetCostCenterList(string xmlString)
        {
            string result = "", nodeString = "GetCostCenterList", filter = "", val = "";
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(nodeString + "/CompanyID");

                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

                vNode = doc.SelectSingleNode(nodeString + "/BusissUnit");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FBUID = '" + val + "'" : "t1.FBUID = '" + val + "'";
                }

                //string sql = "Select FID AS ID ,FNumber ,FName,FBUID,FLeaderID,FCompanyID FROM t_CostCenter t1";
                string sql = "Select FID AS ID  ,FName AS CostCenter FROM t_CostCenter t1";
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                SQLServerHelper runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);

                result = Common.DataTableToXml(dt, "GetCostCenterList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetAccountList
        public string GetAccountList(string xmlString)
        {
            string result = "", nodeString = "GetAccountList", filter = "", val = "", val1 = ""; 
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(nodeString + "/AccountID1");

                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val1 = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FAccountID1='" + val1 + "'" : "t1.FAccountID1='" + val1 + "'";
                }

                vNode = doc.SelectSingleNode(nodeString + "/AccountID2");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FAccountID2 = '" + val + "'" : "t1.FAccountID2 = '" + val + "'";
                }

                string sql = "Select FID,FAccountID1 ,FAccountID2,FAccountCaptial1,FAccountCaptial2 FROM t_Account t1";
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                if (val1.Trim()=="-1")
                    sql = "Select distinct FAccountID1 ,FAccountCaptial1 ,'' As FAccountID2, ''AsFAccountCaptial2  FROM t_Account t1";
                
                SQLServerHelper runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);

                result = Common.DataTableToXml(dt, "GetAccountList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion
    }

}
