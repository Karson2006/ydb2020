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
    public class ItemClass
    {
        public ItemClass()
        { 
        }

        public DataTable GetList(string xmlString)
        {
            DataTable result = new DataTable();
            try
            {
                XmlDocument doc = new XmlDocument();
                string filter="",val="";

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetClassList/Number");
                if(vNode!=null)
                {
                     val = vNode.InnerText.Trim();
                    if(val.Length>0)
                        filter = " FNumber like '%" + val+"%'";
                }

                vNode = doc.SelectSingleNode("GetClassList/Visible");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter + " And FVisible=  '" + val + "'" : " FVisible=  '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetClassList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length>0 ? filter+ " And FName like '%" + val + "%'" : " FName like '%" + val + "%'";
                }
                string sql = "Select * from t_ItemClass Where FIsDeleted=0";
                if (filter.Length > 0)
                    sql = sql + " And " + filter;
                sql = sql + " Order by FNumber Asc";
 
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSql(sql);
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }

        #region GetListXml
        public string GetListXml(string xmlString)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetClassList>" +
                           "<Result>False</Result>" +
                           "<Description></Description>" +
                           "<DataRows></DataRows>" +
                           "</GetClassList>"; 
            try
            {
                DataTable dt = GetList(xmlString);
                if (dt.Rows.Count > 0)
                {
                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(result);
                    doc.SelectSingleNode("GetClassList/Result").InnerText = "True";
                    XmlNode pNode = doc.SelectSingleNode("GetClassList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        XmlNode vNode = doc.CreateElement("FClassID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FName");
                        vNode.InnerText = row["FName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTableName");
                        vNode.InnerText = row["FTableName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FNumber");
                        vNode.InnerText = row["FNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetClassList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetClassList>";
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDetailXml
        public string GetDetailXml(string classID)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetClassDetail>" +
                           "<Result>False</Result>" +
                           "<Description></Description>" +
                           "<DataRows></DataRows>" +
                           "</GetClassDetail>";
            try
            {
                string sql = "Select * from t_ItemClass Where FID ='" + classID + "' and FIsDeleted=0";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(result);
                    doc.SelectSingleNode("GetClassDetail/Result").InnerText = "True";
                    XmlNode cNode = doc.SelectSingleNode("GetClassDetail");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode vNode = doc.CreateElement("FClassID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FName");
                        vNode.InnerText = row["FName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTableName");
                        vNode.InnerText = row["FTableName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FNumber");
                        vNode.InnerText = row["FNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetClassDetail>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "</GetClassDetail>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region Delete
        public string  Delete(string classID)
        {
            string result = "";
            XmlDocument doc = new XmlDocument();

            try
            {
                string sql = "Update t_ItemClass Set FIsDeleted =1 Where FIsDeleted=0 and FID='" + classID + "'";
                SQLServerHelper runner = new SQLServerHelper();
                if (runner.ExecuteSqlNone(sql) < 1)
                {
                    classID = "-1";
                    throw new Exception("删除信息失败");
                }
            }
            catch(Exception err)
            {
                throw err;
            }
            result = classID;
            return result;
        }
        #endregion

        #region Update
        public string Update(string dataString)
        {
            string id = "-1", sql = "", valueString = "";
            string result = "-1";
            DataTable itemtb = new DataTable();
            
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                if (!CheckUpateData(dataString))
                    id = "-1";
                XmlDocument doc = new XmlDocument();
                XmlNode vNode ;
                doc.LoadXml(dataString);
                id = doc.SelectSingleNode("UpdateClass/ClassID").InnerText;

                if (id.Trim() == "" || id.Trim() == "-1")//新增
                {
                    vNode = doc.SelectSingleNode("UpdateClass/FName");
                    id = Guid.NewGuid().ToString();
                    sql = "Insert into t_ItemClass(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                    {
                        id = "-1";
                        throw new Exception("新建资料类型失败");
                    }
                }
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateClass/FName");
                string val = "";
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FName='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateClass/FNumber");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FNumber='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateClass/FTableName");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTableName='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateClass/FDescription");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDescription='" + val + "',";
                }
                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update [t_ItemClass] Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新资料类型失败");
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

        private Boolean CheckUpateData(string xmlString)
        {
            Boolean result = false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("UpdateClass/FName");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("名称不能为空");

                vNode = doc.SelectSingleNode("UpdateClass/ClassID");
                if (vNode == null || vNode.InnerText == "-1" || vNode.InnerText.Trim().Length == 0)//新增检查
                {
                    vNode = doc.SelectSingleNode("UpdateClass/FNumber");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("类别代码不能为空");
                    else
                    {
                        string sql = "Select FID,FNumber,FName from t_itemClass Where FIsDeleted=0 And  (FNumber ='" + doc.SelectSingleNode("UpdateClass/FNumber").InnerText + "'";
                        sql = sql + " Or FName ='" + doc.SelectSingleNode("UpdateClass/FName").InnerText + "')";
                        SQLServerHelper runner = new SQLServerHelper();
                        DataTable dt = runner.ExecuteSql(sql);
                        if (dt.Rows.Count > 0)
                            throw new Exception("代码（" + doc.SelectSingleNode("UpdateClass/FNumber").InnerText + "）或名称（" + doc.SelectSingleNode("UpdateClass/FName").InnerText + "）已存在");
                    }
                }
            }
            catch(Exception err)
            {
                throw err;
            }
            result = true;
            return result;
        }
        #endregion  

    }
}
