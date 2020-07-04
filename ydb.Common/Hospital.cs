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
    public class Hospital
    {
        Items iClass;
        public Hospital()
        {
            iClass = new Items();
        }


        #region GetHospitalDetail
        public string GetHospitalDetail(string xmlString)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetHospitalDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetHospitalDetail>", "GetHospitalList>");
                    result = GetHospitalList(xmlString).Replace("GetHospitalList>", "GetHospitalDetail>");
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetHospitalList
        public string GetHospitalList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FIsDeleted=0 ", val = "";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetHospitalList/ID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetHospitalList/Number");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FNumber like '%" + val + "%'" : "t2.FNumber like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetHospitalList/GrandID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FGrandID= '" + val + "'" : " t1.FGrandID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/ProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FProvinceID= '" + val + "'" : " t1.FProvinceID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCityID= '" + val + "'" : " t1.FCityID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCityID= '" + val + "'" : " t1.FCityID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/CountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCountryID= '" + val + "'" : " t1.FCountryID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/RevenueLevelID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FRevenueLevelID= '" + val + "'" : " t1.FRevenueLevelID= '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalList/ModeID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FModeID= '" + val + "'" : " t1.FModeID= '" + val + "'";
                }

                sql = "Select t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t2.FClassID,t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail," +
                        " Isnull(t3.FName,'') As FGrandName,Isnull(t4.FName,'') As FProvinceName,Isnull(t5.FName,'') As FCityName,Isnull(t6.FName,'') As FCountryName," +
                        " Isnull(t7.FName,'') As FTownName,Isnull(t8.FName,'') As FRevenueLevelName,Isnull(t9.FName,'') As FModeName" +
                        " From t_Hospital t1" +
                        " Left Join t_Items t2 On t1.FID = t2.FID" +
                        " Left Join t_Items t3 On t1.FGrandID = t3.FID" +
                        " Left Join t_Items t4 On t1.FProvinceID = t4.FID" +
                        " Left Join t_Items t5 On t1.FCityID = t5.FID" +
                        " Left Join t_Items t6 On t1.FCountryID = t6.FID" +
                        " Left Join t_Items t7 On t1.FTownID = t7.FID" +
                        " Left Join t_Items t8 On t1.FRevenueLevelID = t8.FID" +
                        " Left Join t_Items t9 On t1.FModeID = t9.FID";
               if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " order by t1.FSortIndex Asc";

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

                        vNode = doc.CreateElement("FHighLevelID");
                        vNode.InnerText = row["FHighLevelID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FGrandID");
                        vNode.InnerText = row["FGrandID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FGrandName");
                        vNode.InnerText = row["FGrandName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FProvinceID");
                        vNode.InnerText = row["FProvinceID"].ToString();
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

                        vNode = doc.CreateElement("FTownID");
                        vNode.InnerText = row["FTownID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTownName");
                        vNode.InnerText = row["FTownName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLatitude");
                        vNode.InnerText = row["FLatitude"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLongitude");
                        vNode.InnerText = row["FLongitude"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAddress");
                        vNode.InnerText = row["FAddress"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPostcode");
                        vNode.InnerText = row["FPostcode"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTelNumber");
                        vNode.InnerText = row["FTelNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FContrac");
                        vNode.InnerText = row["FContrac"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRevenueLevelID");
                        vNode.InnerText = row["FRevenueLevelID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRevenueLevelName");
                        vNode.InnerText = row["FRevenueLevelName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FModeID");
                        vNode.InnerText = row["FModeID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FModeName");
                        vNode.InnerText = row["FModeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAbbreviation");
                        vNode.InnerText = row["FAbbreviation"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
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
                dataString = dataString.Replace("UpdateHospital>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FGrandID");
                string val = "";
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("医院等级ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FGrandID='" + val + "',";
                }
                
                vNode = doc.SelectSingleNode("UpdateItem/FProvinceID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("省/直辖市ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProvinceID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FLatitude");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("纬度不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FLatitude='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FLongitude");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("经度不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FLongitude='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FAddress");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("地址不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FAddress='" + val + "',";
                }
                
                id = iClass.Update(dataString);
                if (id == "-1")//插入t_items表错误
                    result = "-1";
                datachecked = true;

                if (doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "" || doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "-1")//新增
                {
                    sql = "Insert into t_Hospital(FID) Values('" + id + "')";
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

                vNode = doc.SelectSingleNode("UpdateItem/FTownID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTownID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FPostcode");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FPostcode='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FTelNumber");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTelNumber='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FContrac");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FContrac='" + val + "',";
                }
               
                vNode = doc.SelectSingleNode("UpdateItem/FAbbreviation");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FAbbreviation='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FSortIndex");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSortIndex='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FRevenueLevelID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRevenueLevelID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FHighLevelID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FHighLevelID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FModeID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FModeID='" + val + "',";
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
                    sql = "Update t_Hospital Set " + valueString + " Where FID='" + id + "'";
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
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_Hospital Where FID='" + id + "'";
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
                xmlString = xmlString.Replace("DeleteHospital>", "DeleteItem>");
                Items item = new Items();
                id = item.Delete(xmlString);

                if (id.Trim() != "-1")//t_Items删除成功
                {
                    sql = " Update t_Hospital Set FIsDeleted =1 Where  FID='" + id + "' And FIsDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                sql = "Update t_Hospital Set FIsDeleted =0 Where FID='" + id + "' And FIsDeleted=1    Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
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
