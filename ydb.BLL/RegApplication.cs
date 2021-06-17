using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using System.Xml;
using System.Configuration;
using iTR.Lib;

namespace ydb.BLL
{
    public class RegApplication
    {
        public RegApplication()
        { }

        #region List

        //public string List(string filter = "")
        //{
        //    DataTable result = new DataTable();
        //    try
        //    {
        //        SQLServerHelper runer = new SQLServerHelper();
        //        string sql = "Select  t1.*,t2.FName As FTypeName,t3.FName As FProductName,t4.FName As FProvinceName,t5.FName As FCityName,"+
        //                    " t6.FName As FCountryName,t6.FName As FCountryName,t7.FName As FApproveryName" +
        //                    " From Reg_Application t1"+
        //                    " Left Join t_items t2 On t2.FID = t1.FTypeID"+
        //                    " Left Join t_items t3 On t3.FID = t1.FProductID"+
        //                    " Left Join t_items t4 On t4.FID = t1.FProvinceID"+
        //                    " Left Join t_items t5 On t5.FID = t1.FCityID"+
        //                    " Left Join t_items t6 On t6.FID = t1.FCountryID"+
        //                    " Left Join t_items t7 On t7.FID = t1.FApproverID";

        //        if (filter.Length > 0)
        //            sql = sql + " Where " + filter;
        //        result = runer.ExecuteSql(sql);

        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //    return result;
        //}

        #endregion List

        #region Update

        public string Update(string dataString)
        {
            string id = "", sql = "", valueString = "", result = "-1", val = "", mobile = "";

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);

                vNode = doc.SelectSingleNode("UpdateRegistration/Mobile");
                if (vNode != null)
                {
                    mobile = vNode.InnerText;
                    if (mobile.Trim().Length > 0)
                        valueString = valueString + "FMobile='" + mobile + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRegistration/ID");
                if (vNode == null || vNode.InnerText.Trim() == "-1")//新增
                {
                    //if(val.Trim().Length ==0)
                    //    throw new Exception("手机号码不能为空");
                    //else
                    //{
                    //    sql = "Select FID  from Reg_Application Where FMobile ='{0}'";
                    //    sql = string.Format(sql, mobile);
                    //    DataTable dt = runner.ExecuteSql(sql);
                    //    if (dt.Rows.Count > 0)
                    //        id = dt.Rows[0]["FID"].ToString();
                    //    else
                    //    {
                    id = Guid.NewGuid().ToString();
                    sql = "Insert into Reg_Application(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入新日程失败
                        throw new Exception("新建失败");
                    //    }
                    //}
                }
                else
                {
                    id = vNode.InnerText.Trim();
                }
                //更新信息
                vNode = doc.SelectSingleNode("UpdateRegistration/Applicant");

                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApplicant='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/RegType");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRegType='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRegistration/Registed");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRegisted='" + val + "',";
                }

                //vNode = doc.SelectSingleNode("UpdateRegistration/Mobile");
                //if (vNode != null)
                //{
                //    val = vNode.InnerText;
                //    if (val.Trim().Length > 0)
                //        valueString = valueString + "FMobile='" + val + "',";
                //}

                vNode = doc.SelectSingleNode("UpdateRegistration/ProductID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProductID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/ProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProvinceID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCityID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/CountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCountryID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/ApproverID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApproverID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/ApproveDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApproveDate='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRegistration/ProductTypeID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProductTypeID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/HospitalID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FHospitalID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRegistration/HistoryPerformance");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FHistoryPerformance='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRegistration/ForecastPerformance");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FForecastPerformance='" + val + "',";
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

        #endregion Update

        #region GetDetail

        public string GetDetail(string xmlString)
        {
            string result = "", id = "", sql = "", cols = "";
            string[] pageIDs = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetRegistrationData/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("ID 不能为空");
                else
                    id = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("GetRegistrationData/Cols");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    cols = vNode.InnerText.Trim();
                vNode = doc.SelectSingleNode("GetRegistrationData/PageID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    pageIDs = vNode.InnerText.Trim().Split('|');

                sql = "SELECT t1.FID,t1.FApplicant,t1.FMobile ,t1.FRegType,t1.FDate,t1.FProductTypeID,t1.FProductID ,t1.FProvinceID ,t1.FCityID,FCountryID,t1.FHospitalID,t1.FApproverID,t1.FApproveDate," +
                    " isnull(t2.FName,'') As FProductName,isnull(t3.FName,'') As FProvinceName,isnull(t4.FName,'') As FCityName,isnull(t5.FName,'') As FCountryName,isnull(t6.FName,'')As FHospitalName," +
                    " t1.FHistoryPerformance,t1.FForecastPerformance,ISnull(t7.FName,'') As FProductTypeName " +
                    " FROM Reg_Application t1 " +
                    " Left Join t_items t2 On t2.FID = t1.FProductID" +
                    " Left Join t_items t3 On t3.FID = t1.FProvinceID" +
                    " Left Join t_items t4 On t4.FID = t1.FCityID" +
                    " Left Join t_items t5 On t5.FID = t1.FCountryID" +
                    " Left Join t_items t7 On t7.FID = t1.FProductTypeID" +
                    " Left Join t_items t6 On t6.FID = t1.FHospitalID  Where t1.FID='" + id + "'";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = iTR.Lib.Common.DataTableToXml(dt, "GetRegistrationData", cols, "List");

                //加载图片
                doc.LoadXml(result);
                XmlNode pNode = null;
                XmlNode cNode = null;
                XmlNodeList rows = null;
                if (pageIDs != null)
                {
                    for (int i = 0; i < pageIDs.Length; i++)
                    {
                        switch (pageIDs[i])
                        {
                            case "Reg002"://历史业绩
                                //加载图片
                                rows = doc.SelectNodes("GetRegistrationData/DataRows/DataRow");
                                foreach (XmlNode row in rows)
                                {
                                    pNode = row;
                                    cNode = doc.CreateElement("HisPerformance_Images");
                                    Common.SetImageXmlNode("Reg002", id, ref cNode, ref doc);
                                    pNode.AppendChild(cNode);
                                }
                                break;

                            case "Reg003"://预算业绩
                                rows = doc.SelectNodes("GetRegistrationData/DataRows/DataRow");
                                foreach (XmlNode row in rows)
                                {
                                    pNode = row;
                                    cNode = doc.CreateElement("ForPerformance_Images");
                                    Common.SetImageXmlNode("Reg003", id, ref cNode, ref doc);
                                    pNode.AppendChild(cNode);
                                }
                                break;
                        }
                    }
                }

                result = doc.OuterXml;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion GetDetail

        #region UpdateRegApplicant

        public string UpdateRegApplicant(string dataString)
        {
            string id = "", sql = "", result = "-1", regType = "0";

            DataTable dt = null;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataString);
                SQLServerHelper runner = new SQLServerHelper();

                XmlNode vNode = doc.SelectSingleNode("UpdateRegistration/ID");

                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("ID不能为空");
                else
                {
                    id = vNode.InnerText.Trim();
                    sql = "Select FID,FRegType From Reg_Application Where FID='" + id + "'";
                    dt = runner.ExecuteSql(sql);
                    if (dt.Rows.Count == 0)
                        throw new Exception("ID不存在");
                    regType = dt.Rows[0]["FRegType"].ToString();
                }

                if (regType == "0")
                    result = UpdateRepresentative(dataString);
            }
            catch (Exception err)
            {
                throw err;
            }
            result = id;

            return result;
        }

        public string UpdateRepresentative(string xmlString)
        {
            string result = "-1", sql;
            string name = "", idNumber = "", gerder = "", education = "", major = "", mobile = "", address = "", experience = "", companyID = "", id = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                SQLServerHelper runner = new SQLServerHelper();

                XmlNode vNode = doc.SelectSingleNode("UpdateRegistration/Name");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("姓名不能为空");
                else
                    name = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/ID");
                id = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/IDNumber");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("身份证号码不能为空");
                else
                    idNumber = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/Gender");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("性别不能为空");
                else
                    gerder = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/Mobile");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("手机号码不能为空");
                else
                    mobile = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/Education");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    education = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/MajorID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    major = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/Address");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    address = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/Experience");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    experience = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegistration/CompanyID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    companyID = vNode.InnerText.Trim();

                //更新手机号码到注册登记表
                Update(xmlString);

                sql = "Select FMobile from Reg_Representative Where  FMobile ='" + mobile + "'";
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)// 已存在，更新
                {
                    sql = "Update Reg_Representative Set FName='" + name + "',FIDNumber='" + idNumber + "',FGender='" + gerder + "',FEducation='" + education + "',FMajorID='" + major + "',FMobile='" + mobile + "',FAddress='" + address + "'" +
                        ",FExperience='" + experience + "',FCompanyID='" + companyID + "' Where FMobile ='" + mobile + "'";
                }
                else
                {
                    sql = "Insert Into Reg_Representative(FApplicationID,FName,FIDNumber,FGender,FEducation,FMajorID,FMobile,FAddress,FExperience,FCompanyID)" +
                        " Values('" + id + "','" + name + "','" + idNumber + "','" + gerder + "','" + education + "','" + major + "','" + mobile + "','" + address + "','" + experience + "','" + companyID + "')";
                }
                runner.ExecuteSqlNone(sql);
                result = "1";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion UpdateRegApplicant

        #region GetRegApplicant

        public string GetRegApplicant(string xmlString)
        {
            string result = "", id = "", sql = "", cols = "", regType = "0";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetRegistrationData/Mobile");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("手机号码不能为空");
                else
                    id = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("GetRegistrationData/Cols");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    cols = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("GetRegistrationData/RegType");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                }
                else
                    regType = vNode.InnerText.Trim();

                if (regType == "0")
                {
                    sql = " Select t1.FApplicationID,t1.FName,t1.FIDNumber,t1.FGender,t1.FEducation,t1.FMajorID,t1.FMobile,t1.FAddress,t1.FExperience,t1.FCompanyID" +
                          " From  Reg_Representative t1" +
                          " Where t1.FMobile='" + id + "' Order by t1.FSortIndx Desc";
                }
                else
                {
                }
                //sql = " Select t1.FApplicationID,t1.FName,t1.FIDNumber,t1.FGender,t1.FEducation,t1.FMajorID,t1.FMobile,t1.FAddress,t1.FExperience,t1.FCompanyID,isnull(t2.FName,'') As FMajorName" +
                //      " From  Reg_Representative t1" +
                //      " Left Join t_Items t2 On t1.FMajorID= t2.FID  Where t1.FMobile='" + id + "' Order by t1.FSortIndx Desc";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = iTR.Lib.Common.DataTableToXml(dt, "GetRegistrationData", cols, "List");

                if (dt.Rows.Count > 0)
                    id = dt.Rows[0]["FApplicationID"].ToString();

                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetRegistrationData/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode("Reg001", id, ref cNode, ref doc);
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

        #endregion GetRegApplicant

        #region UpdateRegRelationship

        public string UpdateRegRelationship(string xmlString)
        {
            string result = "-1", sql = "", id = "", hospitalID = "";

            DataTable dt = null;
            try
            {
                SQLServerHelper runner = new SQLServerHelper();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("UpdateRegistration/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("ID不能为空");
                else
                {
                    id = vNode.InnerText.Trim();
                    sql = "Select FID from Reg_Application Where FID='" + id + "'";
                    dt = runner.ExecuteSql(sql);
                    if (dt.Rows.Count == 0)
                        throw new Exception("ID不存在");
                }
                vNode = doc.SelectSingleNode("UpdateRegistration/HospitalID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("HospitalID不能为空");
                }
                else
                {
                    hospitalID = vNode.InnerText.Trim();
                }

                sql = "Delete from Reg_HospitalRelationShip Where FApplicationID='" + id + "'";
                runner.ExecuteSqlNone(sql);

                XmlNode dataNode = doc.SelectSingleNode("UpdateRegistration/Datas");
                foreach (XmlNode node in dataNode.ChildNodes)
                {
                    string name = node["Name"].InnerText;
                    string title = node["Title"].InnerText;
                    string dept = node["Department"].InnerText;
                    string relationship = node["RelationShipTypeID"].InnerText;
                    string sortindx = node["SortIndx"].InnerText;

                    sql = "Insert Into Reg_HospitalRelationShip(FApplicationID,FHospitalID,FName,FTitle,FDepartment,FRelationShipTypeID,FSortIndx) Values('";
                    sql = sql + id + "','" + hospitalID + "','" + name + "','" + title + "','" + dept + "','" + relationship + "'," + sortindx + ")";
                    runner.ExecuteSqlNone(sql);
                }

                result = id;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion UpdateRegRelationship

        #region GetRegRelationShip

        public string GetRegRelationShip(string xmlString)
        {
            string result = "", id = "", sql = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetRegistrationData/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("ID不能为空");
                else
                    id = vNode.InnerText.Trim();

                sql = " SELECT t1.FApplicationID,t1.FHospitalID,t1.FDate,t1.FName,t1.FTitle,t1.FDepartment,t1.FRelationShipTypeID,t1.FSortIndx, isnull(t2.FName,'') As FHospitalName,Isnull(t3.FName,'') As FRelationShipTypeName " +
                      " FROM Reg_HospitalRelationShip t1" +
                      " Left Join t_Items t2 On t1.FHospitalID = t2.FID" +
                      " Left Join t_Items t3 On t1.FRelationShipTypeID = t3.FID";
                sql = sql + " Where FApplicationID='" + id + "'";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = iTR.Lib.Common.DataTableToXml(dt, "GetRegistrationData", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion GetRegRelationShip

        #region SendVCode

        public string SendVCode(string xmlString)
        {
            string result = "", mobile = "", sql = "";
            string callType = "SendVCode";
            result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            try
            {
                string vCode = "";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("SendVCode/Mobile");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("Mobile不能为空");
                else
                    mobile = vNode.InnerText.Trim();
                string curTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                sql = "Select FCode from VCodes Where '" + curTime + "' Between FCreateTime and FExpireTime and FStatus =0 and FMobile='" + mobile + "'";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)//存在未验证且在有限期内
                {
                    vCode = dt.Rows[0]["FCode"].ToString();
                }
                else
                {
                    Random ran = new Random();
                    vCode = ran.Next(1000, 9999).ToString();
                }
                AliDayuSMS smsSender = new AliDayuSMS();
                if (smsSender.SendSms(vCode, mobile) == "1" && dt.Rows.Count == 0)//发送成功,且不存在该记录
                {
                    DateTime expireTime = DateTime.Now.AddMinutes(5);
                    sql = "Insert Into VCodes(FMobile,FCode)Values('" + mobile + "','" + vCode + "')";
                    runner = new SQLServerHelper();
                    if (runner.ExecuteSqlNone(sql) > 0)
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                              "<" + callType + ">" +
                              "<Result>True</Result>" +
                              "<Description>OK</Description></" + callType + ">";
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion SendVCode

        #region CheckVCode

        public string CheckVCode(string xmlString)
        {
            string result = "0", mobile = "", sql = "", code = "";

            string callType = "CheckVCode";
            result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("CheckVCode/Mobile");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("Mobile不能为空");
                else
                    mobile = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("CheckVCode/Code");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("Code不能为空");
                else
                    code = vNode.InnerText.Trim();
                string curTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                sql = "Select FExpireTime From VCodes Where FMobile='" + mobile + "' and FCode ='" + code + "' and FStatus =0 and  '" + curTime + "' Between FCreateTime and FExpireTime";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count == 0)//
                {
                    throw new Exception("验证码错误或已过期");
                }
                else
                {
                    sql = "Update VCodes Set FStatus =1 Where  FMobile='" + mobile + "' and FCode ='" + code + "' and FStatus =0 and  '" + curTime + "' Between FCreateTime and FExpireTime";
                    runner.ExecuteSqlNone(sql);
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<" + callType + ">" +
                         "<Result>True</Result>" +
                         "<Description>验证码正确</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion CheckVCode

        #region UploadImage

        public string UploadImage(string xmlString)
        {
            string callType = "UploadRegImage";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            try
            {
                string base64String = "", fileName = "", ownerID = "", formId = "", fileNum = "";

                string path = System.Configuration.ConfigurationManager.AppSettings["Path"];
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode(callType + "/FileNum");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("FileNum不能为空");
                else
                    fileNum = vNode.InnerText.Trim();

                string[] fileNos = fileNum.Split('|');
                if (fileNos.Length == 0)
                    throw new Exception("FileNum参数格式不正确");

                if (int.Parse(fileNos[1]) > 0)//只有上传附件个数大于0，才做判断
                {
                    vNode = doc.SelectSingleNode(callType + "/Base64String");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("Base64String不能为空");
                    else
                        base64String = vNode.InnerText.Trim();

                    vNode = doc.SelectSingleNode(callType + "/FileName");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("FileName不能为空");
                    else
                        fileName = vNode.InnerText.Trim();
                }

                vNode = doc.SelectSingleNode(callType + "/PageID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("PageID不能为空");
                else
                    formId = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode(callType + "/OwnerID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("OwnerID不能为空");
                else
                    ownerID = vNode.InnerText.Trim();

                if (int.Parse(fileNos[1]) > 9)
                {
                    throw new Exception("拟上传的附件个数已大于最大数9");
                }
                else if (int.Parse(fileNos[0]) == 1)//上传第一个附件，删除数据库中的相干附件
                {
                    string sql = "Update Attachments Set FDeleted=1 where FPageID='{0}' and FOwnerID='{1}' ";
                    sql = string.Format(sql, formId, ownerID);
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);

                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>True</Result>" +
                          "<Description></Description></" + callType + ">";
                }

                if (int.Parse(fileNos[1]) > 0)
                {
                    string fileextra = "jpg";
                    if (fileName.Split('.').Length > 1)
                        fileextra = fileName.Split('.')[1];

                    fileName = Guid.NewGuid().ToString().Replace("-", "") + "." + fileextra;
                    if (FileHelper.UploadImage(base64String, path, fileName, formId, ownerID))
                    {
                        string url = System.Configuration.ConfigurationManager.AppSettings["URL"];

                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                              "<" + callType + ">" +
                              "<Result>True</Result>" +
                              "<ImageUrl>" + url + "/" + path + "/" + fileName + "</ImageUrl>" +
                              "<T_ImageUrl>" + url + "/" + path + "/T_" + fileName + "</T_ImageUrl>" +
                              "<Description></Description></" + callType + ">";
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion UploadImage

        #region GetImage

        public string GetImage(string xmlString)
        {
            string callType = "GetRegImage";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            try
            {
                string ownerID = "", pageID = "";

                string url = System.Configuration.ConfigurationManager.AppSettings["URL"];
                url = "http://ydb.tenrypharm.com:6060";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(callType + "/PageID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("PageID不能为空");
                else
                    pageID = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode(callType + "/OwnerID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("OwnerID不能为空");
                else
                    ownerID = vNode.InnerText.Trim();

                string sql = "SELECT (FPath + '\\' + FFileName) As FPath1,(FPath + '\\T_' + FFileName) As FPath2  FROM Attachments Where FPageID='{0}' and FOwnerID='{1}' and FDeleted=0 ";
                sql = string.Format(sql, pageID, ownerID);
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);

                if (dt.Rows.Count > 0)
                {
                    string imageString = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        imageString = imageString + "<Image Original=" + url + "\\" + row["FPath1"].ToString() + ">" + url + "\\" + row["FPath2"].ToString() + "</Image>";
                        //imageString = imageString + "<Image>" + url + row["FPath2"].ToString() + "</Image>";
                    }
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                             "<" + callType + ">" +
                             "<Result>True</Result><Rows>" + imageString + "</Rows><Description></Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion GetImage

        #region UploadFile

        public string UploadFile(string xmlString)
        {
            string callType = "UploadFile";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            try
            {
                string base64String = "", fileName = "", ownerID = "", fileSize = "", mimeType = "";

                string path = System.Configuration.ConfigurationManager.AppSettings["Path"];
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(callType + "/Base64String");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("Base64String不能为空");
                else
                    base64String = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode(callType + "/FileName");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("FileName不能为空");
                else
                    fileName = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode(callType + "/FileSize");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("FileSize不能为空");
                else
                    fileSize = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode(callType + "/MimeType");

                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("MimeType不能为空");
                else
                    mimeType = vNode.InnerText.Trim();

                byte[] gb = Guid.NewGuid().ToByteArray();
                long fileID = BitConverter.ToInt64(gb, 0);
                string sql = string.Format("insert into [yaodaibao].[dbo].[CTP_FILE](ID,FILENAME,MIME_TYPE,CREATE_DATE,CREATE_MEMBER,FILE_SIZE) values('{0}','{1}','{2}','{3}','{4}','{5}')", new object[]
                {
                    fileID,
                    fileName,
                    mimeType,
                    DateTime.Now.ToString(),
                    "",
                    fileSize
                });
                if (FileHelper.UploadFile(base64String, path, fileID, sql))
                {
                    string url = System.Configuration.ConfigurationManager.AppSettings["URL"];

                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>True</Result>" +
                          "<FileID>" + fileID + "</FileID>" +
                          "<Description></Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion UploadFile
    }
}