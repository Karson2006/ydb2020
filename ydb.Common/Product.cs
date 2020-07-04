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
    public class Product
    {
        Items iClass;
        public Product()
        {
            iClass = new Items();
        }

        #region GetDetail
        public string GetDetail(string xmlString)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetProductDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetProductDetail>", "GetProductList>");
                    result = GetList(xmlString).Replace("GetProductList>", "GetProductDetail>");
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetList
        public string GetList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FDeleted=0 ", val = "";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                xmlString = xmlString.Replace("GetProductList>", "GetList>");
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetList/ID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetList/Number");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FNumber like '%" + val + "%'" : "t2.FNumber like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetList/TypeID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FTypeID= '" + val + "'" : " t1.FTypeID= '" + val + "'";
                }


                sql = "Select t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t2.FClassID,t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,"+
                    " Isnull(t3.FName,'') As FTypeName"+
                    " From t_Products t1"+
                    " Inner Join t_Items t2 On t1.FID = t2.FID"+
                    " Inner Join t_Items t3 On t1.FTypeID = t3.FID";

               if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " order by t1.FSortIndex Asc";

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FName");
                        vNode.InnerText = row["FName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FClassID");
                        vNode.InnerText = row["FClassID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FNumber");
                        vNode.InnerText = row["FNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FFullNumber");
                        vNode.InnerText = row["FFullNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIsDetail");
                        vNode.InnerText = row["FIsDetail"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLevel");
                        vNode.InnerText = row["FLevel"].ToString();
                        cNode.AppendChild(vNode);
                        
                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTypeID");
                        vNode.InnerText = row["FTypeID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTypeName");
                        vNode.InnerText = row["FTypeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSKU");
                        vNode.InnerText = row["FSKU"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPackage");
                        vNode.InnerText = row["FPackage"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            result = result.Replace("GetList>", "GetProductList>");
            return result;
        }
        #endregion

        #region GetMyHospitalList
        public string GetMyHospitalList(string employeeID)
        {
            string result = "", sql = "";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                //doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetHospitalList/ID");
                if (vNode != null)
                {
                    //val = vNode.InnerText.Trim();
                    //if (val.Length > 0)
                        //filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

               string nowString = DateTime.Now.ToString("yyyy-MM-dd");

               sql = " SELECT t1.*,t2.FName As FInstitutionName,t2.FID,t2.FNumber,t2.FClassID" +
                       " FROM AuthData t1 "+
                       " Left Join t_Items t2 On t1.FInstitutionID = t2.FID";
                sql = sql + " Where t1.FIsDeleted=0 And t1.FBeginDate<='" + nowString + " 0:0:0.000' and  t1.FEndDate>='" + nowString + " 23:59:59.999' And t1.FEmployeeID='" + employeeID  + "'";
                sql = sql + " Order by t2.FNumber Asc";

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetHospitalList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetHospitalList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetHospitalList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FName");
                        vNode.InnerText = row["FInstitutionName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FClassID");
                        vNode.InnerText = row["FClassID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FNumber");
                        vNode.InnerText = row["FNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FInstitutionType");
                        vNode.InnerText = row["FInstitutionType"].ToString();
                        cNode.AppendChild(vNode);
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetHospitalList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetHospitalList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region Update
        public string Update(string dataString)
        {
            string id = "", sql = "",  valueString = "";
            bool datachecked = true;
            string result = "-1";

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                dataString = dataString.Replace("UpdateProduct>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FTypeID");
                string val = "";
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("产品类型ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTypeID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FSKU");
                
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("产品SKU不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSKU='" + val + "',";
                }

                
                id = iClass.Update(dataString);
                if (id == "-1")//插入t_items表错误
                    result = "-1";
                datachecked = true;

                if (doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "" || doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "-1")//新增
                {
                    sql = "Insert into t_Products(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                        throw new Exception("新建失败");
                }

                vNode = doc.SelectSingleNode("UpdateItem/FPackage");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FPackage='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FIntroduce");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIntroduce='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update t_Products Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新失败");
                    }

                }

                
            }
            catch (Exception err)
            {
                if (id != "-1" && datachecked)//t_tems已插入数据成功，要删除
                {
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_Products Where FID='" + id + "'";
                    runner.ExecuteSqlNone(sql);
                }
                throw err;
            }
            result = id;

            return result;
        }

        
        #endregion  

        #region Delete
        public string Delete(string xmlString)
        {
            string result = "-1", id = "-1", sql = "";
            XmlDocument doc = new XmlDocument();

            try
            {
                xmlString = xmlString.Replace("DeleteProduct>", "DeleteItem>");
                Items item = new Items();
                id = item.Delete(xmlString);

                if (id.Trim() != "-1")//t_Items删除成功
                {
                    sql = " Update t_Products Set FDeleted =1 Where  FID='" + id + "' And FDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                sql = "Update t_Products Set FDeleted =0 Where FID='" + id + "' And FDeleted=1    Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

    }

}
