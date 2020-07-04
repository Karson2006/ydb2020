using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;
using System.Configuration;

namespace ydb.BLL
{
    public class MktActivity
    {
        public MktActivity()
        {

        }

        #region Update

        public string Update(string xmlString)
        {
            string id = "", sql = "", valueString = "";

            try
            {
                Dictionary<string, string> fieldValues = Common.GetFieldValuesFromXml(xmlString, "UpdateMarketingActivity");
 
                SQLServerHelper runner = new SQLServerHelper();

                if (fieldValues["FID"] == "-1" || fieldValues["FID"].Trim().Length ==0)
                {
                    id = Guid.NewGuid().ToString();
                    sql="Insert Into MarketingActivity(FID) Values('"+id+"')";
                    runner.ExecuteSqlNone(sql);
                }
                else
                {
                    id = fieldValues["FID"];
                }
                
                foreach(string key in fieldValues.Keys)
                {
                    if (key == "FID") continue;
                    valueString = valueString + key + "='" + fieldValues[key] + "',";
                }

                if (valueString.Length > 0)
                    sql = "Update MarketingActivity Set " + valueString.Substring(0, valueString.Length - 1) + " Where  FID ='" + id + "'";

                runner.ExecuteSqlNone(sql);
            }
            catch(Exception err)
            {
                id = "-1";
                throw err;
            }
            return id;
        }
        #endregion

        #region Delete
        public string Delete(string xmlMessage)
        {
            string result = "-1", id = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlNode vNode = doc.SelectSingleNode("DeleteMarketingActivity/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("活动ID不能为空");
                id = vNode.InnerText.Trim();
                string sql = "Delete from MarketingActivity Where FID = '" + id + "'";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
            if (int.Parse(result) > 0)
                result = id;
            else
                result = "-1";
            return result;
        }
        #endregion

        #region GetList
        public string GetList(string xmlString)
        {
            string result = "";
            try
            {
                DataTable dt = Query(xmlString, "GetMarketingActivityList");
                result = Common.DataTableToXml(dt, "GetMarketingActivityList", "", "List");
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDetail
        public string GetDetail(string xmlString)
        {
            string result = "",id="";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetMarketingActivityDetail" + "/ID");
                if (vNode== null )
                {
                    throw new Exception("市场活动ID不能为空");
                }
                else if(vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("市场活动ID不能为空");
                }
                else
                {
                    id = vNode.InnerText.Trim();
                }

                DataTable dt = Query(xmlString, "GetMarketingActivityDetail");
                result = Common.DataTableToXml(dt, "GetMarketingActivityDetail","","List");

                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetMarketingActivityDetail/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode("MKT001", id, ref cNode, ref doc);
                    pNode.AppendChild(cNode);
                }

                result = doc.OuterXml;

            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region Query
        private DataTable  Query(string xmlString,string nodeString)
        {
            string  sql = "", filter = "", val = "";
            DataTable result = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(nodeString+"/ID");

                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/BeginDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val =  vNode.InnerText.Trim() + " 0:0:0.000";
                    filter = filter.Length > 0 ? filter = filter + " And t1.FBeginDate>='" + val + "'" : "t1.FBeginDate>='" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/EndDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim()+" 23:59:59.999";
                    filter = filter.Length > 0 ? filter = filter + " And t1.FEndDate<='" + val + "'" : "t1.FEndDate <= '" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FEmployeeID In ('" + val.Replace("|", "','") + "')" : "t1.FEmployeeID In ('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode(nodeString + "/ActivityTypeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FActivityTypeID= '" + val + "'" : "t1.FActivityTypeID= '" + val + "'";
                }
                sql = "SELECT t1.*,Isnull(t2.FName,'') AS FEmployeeName,Isnull(t3.FName,'') AS FActivityTypeName " +
                     " FROM MarketingActivity t1 " +
                     " Left Join t_Items t2 On t1.FEmployeeID=t2.FID" +
                     " Left Join t_Items t3 On t1.FActivityTypeID=t3.FID";
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSql(sql);
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
