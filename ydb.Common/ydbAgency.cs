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
    public class ydbAgency
    {
        Items iClass;
        public ydbAgency()
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
                XmlNode vNode = doc.SelectSingleNode("GetAgencyDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetAgencyDetail>", "GetAgencyList>");
                    result = GetList(xmlString).Replace("GetAgencyList>", "GetAgencyDetail>");
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
                xmlString = xmlString.Replace("GetAgencyList>", "GetList>");
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
                vNode = doc.SelectSingleNode("GetList/ProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FProvinceID= '" + val + "'" : " t1.FProvinceID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetList/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCityID= '" + val + "'" : " t1.FCityID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetList/CountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCountryID= '" + val + "'" : " t1.FCountryID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetList/DeveloperID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FDeveloperID= '" + val + "'" : " t1.FDeveloperID= '" + val + "'";
                }

                sql = "Select t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t2.FClassID,t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,"+
                        " Isnull(t3.FName,'') As FDeveloperName,Isnull(t4.FName,'') As FProvinceName,Isnull(t5.FName,'') As FCityName,Isnull(t6.FName,'') As FCountryName"+
                        " From t_Agency t1"+
                        " Left Join t_Items t2 On t1.FID = t2.FID"+
                        " Left Join t_Items t3 On t1.FDeveloperID = t3.FID"+
                        " Left Join t_Items t4 On t1.FProvinceID = t4.FID"+
                        " Left Join t_Items t5 On t1.FCityID = t5.FID"+
                        " Left Join t_Items t6 On t1.FCountryID = t6.FID";

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

                        vNode = doc.CreateElement("FProvinceID");
                        vNode.InnerText = row["FProvinceID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FProvinceName");
                        vNode.InnerText = row["FProvinceName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCityID");
                        vNode.InnerText = row["FCityID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCityName");
                        vNode.InnerText = row["FCityName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCountryID");
                        vNode.InnerText = row["FCountryID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCountryName");
                        vNode.InnerText = row["FCountryName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCreditcode");
                        vNode.InnerText = row["FCreditcode"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FBusinessscope");
                        vNode.InnerText = row["FBusinessscope"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAddress");
                        vNode.InnerText = row["FAddress"].ToString();

                        vNode = doc.CreateElement("FContact");
                        vNode.InnerText = row["FContact"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCountryID");
                        vNode.InnerText = row["FCountryID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FContactMobile");
                        vNode.InnerText = row["FContactMobile"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLicenseFile");
                        vNode.InnerText = row["FLicenseFile"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDevelopDate");
                        vNode.InnerText = row["FDevelopDate"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDate");
                        vNode.InnerText = row["FDate"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAgreementNumber");
                        vNode.InnerText = row["FAgreementNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDeveloperID");
                        vNode.InnerText = row["FDeveloperID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDeveloperName");
                        vNode.InnerText = row["FDeveloperName"].ToString();
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
            result = result.Replace("GetList>", "GetAgencyList>");
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
                dataString = dataString.Replace("UpdateAgency>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FProvinceID");
                string val = "";
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("省市ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProvinceID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FCreditcode");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("社会信用代码不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCreditcode='" + val + "',";
                }


                vNode = doc.SelectSingleNode("UpdateItem/FBusinessscope");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("经营范围不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FBusinessscope='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FAgreementNumber");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("代理协议不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FAgreementNumber='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FAddress");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("通讯地址不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FAddress='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FDeveloperID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("招商经理ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDeveloperID='" + val + "',";
                }
                

                id = iClass.Update(dataString);
                if (id == "-1")//插入t_items表错误
                    result = "-1";
                datachecked = true;

                if (doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "" || doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "-1")//新增
                {
                    sql = "Insert into t_Agency(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                        throw new Exception("新建失败");
                }

                vNode = doc.SelectSingleNode("UpdateItem/FCityID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCityID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FCountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCountryID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FContact");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FContact='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FContactMobile");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FContactMobile='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FIntroduce");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIntroduce='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FLicenseFile");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FLicenseFile='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FDevelopDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDevelopDate='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDate='" + val + "',";
                }
                

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update t_Agency Set " + valueString + " Where FID='" + id + "'";
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
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_Agency Where FID='" + id + "'";
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
                xmlString = xmlString.Replace("DeleteAgency>", "DeleteItem>");
                Items item = new Items();
                id = item.Delete(xmlString);

                if (id.Trim() != "-1")//t_Items删除成功
                {
                    sql = " Update t_Agency Set FDeleted =1 Where  FID='" + id + "' And FDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                sql = "Update t_Agency Set FDeleted =0 Where FID='" + id + "' And FDeleted=1    Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
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
