using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using ydb.BLL;
using System.Xml;
using Newtonsoft.Json;

//using System.Collections;
//using System.ComponentModel;
//using System.Data;
////using System.Linq;

//using System.Web.Services.Protocols;
//using System.Xml.Linq;
using System.IO;

//using System.Text;
//using System.Security.Cryptography;

namespace ydb.WebService
{
    /// <summary>
    /// ItemInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。
    // [System.Web.Script.Services.ScriptService]
    public class RegistrationInvoke : System.Web.Services.WebService
    {
        #region GetRegStatusByMobile

        [WebMethod]
        public string GetRegStatusByMobile(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee em = new Employee();
                    result = em.GetRegStatusByMobile(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegStatusByMobileJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegStatusByMobile");
            string result = GetRegStatusByMobile(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegStatusByMobile");
            return result;
        }

        #endregion GetRegStatusByMobile

        #region ChangePassword

        [WebMethod]
        public string ChangePassword(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee emp = new Employee();
                    result = emp.ChangePassword(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        public string ChangePasswordJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "ChangePassword");
            string result = ChangePassword(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "ChangePassword");
            return result;
        }

        #endregion ChangePassword

        #region SetPassword

        [WebMethod]
        public string SetPassword(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee emp = new Employee();
                    result = emp.SetPassword(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SetPasswordJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "SetPassword");
            string result = SetPassword(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SetPassword");
            return result;
        }

        #endregion SetPassword

        #region Login

        [WebMethod]
        public string Login(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<Login>" +
                          "<Result>False</Result>" +
                          "<Description></Description></Login>";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:", 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee emp = new Employee();
                    result = emp.Login(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<Login>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></Login>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string LoginJson(string callType, string JsonMessage)
        {
            string logID = Guid.NewGuid().ToString();
            FileLogger.WriteLog(logID + "|Start:" + JsonMessage, 1, "", callType);
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "Login");

            FileLogger.WriteLog(logID + "|Json2XML:" + xmlString, 1, "", callType);
            string result = Login(callType, xmlString);
            FileLogger.WriteLog(logID + "|Login:" + result, 1, "", callType);
            result = iTR.Lib.Common.XML2Json(result, "Login");
            FileLogger.WriteLog(logID + "|XML2Json:" + result, 1, "", callType);
            return result;
        }

        #endregion Login

        #region CheckRegistration

        [WebMethod]
        public string CheckRegistration(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<Registration>" +
                          "<Result>False</Result>" +
                          "<EmployeeID></EmployeeID>" +
                          "<Description></Description></Registration>";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("Registration", xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode("Registration/Mobile");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("手机号码不能为空");

                    string xmlString = " <GetEmployeeList>" +
                                            "<AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>" +
                                            "<ID></ID>" +
                                            "<Name></Name>" +
                                            "<Number></Number>" +
                                            "<PositionName></PositionName>" +
                                            "<TypeID></TypeID>" +
                                            "<IsAgency></IsAgency>" +
                                            "<Mobile>" + vNode.InnerText.Trim() + "</Mobile>" +
                                        "</GetEmployeeList>";
                    Employee emp = new Employee();
                    doc.LoadXml(emp.GetEmployeeList(xmlString));
                    if (doc.SelectSingleNode("GetEmployeeList/Result").InnerText == "True")//该手机号码已建档
                    {
                        string employeeID = doc.SelectSingleNode("GetEmployeeList/DataRows/DataRow/ID").InnerText;
                        doc.LoadXml(result);
                        doc.SelectSingleNode("Registration/Result").InnerText = "True";
                        doc.SelectSingleNode("Registration/EmployeeID").InnerText = employeeID;
                        result = doc.OuterXml;
                    }
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<Registration>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></Registration>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string CheckRegistrationJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "Registration");
            string result = CheckRegistration(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "Registration");
            return result;
        }

        #endregion CheckRegistration

        #region CheckInvitationCode

        /// <summary>
        /// 邀请码检查
        /// </summary>
        [WebMethod]
        public string CheckInvitationCode(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee emp = new Employee();
                    result = emp.CheckInvitationCode(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string CheckInvitationCodeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "CheckInvitationCode");
            string result = CheckInvitationCode(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "CheckInvitationCode");
            return result;
        }

        #endregion CheckInvitationCode

        #region CreateInvitationCode

        /// <summary>
        /// 邀请码检查
        /// </summary>
        [WebMethod]
        public string CreateInvitationCode(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee emp = new Employee();
                    result = emp.CreateInvitationCode(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string CreateInvitationCodeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "CreateInvitationCode");
            string result = CreateInvitationCode(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "CreateInvitationCode");
            return result;
        }

        #endregion CreateInvitationCode

        #region GetInvitationCode

        /// <summary>
        /// 邀请码检查
        /// </summary>
        [WebMethod]
        public string GetInvitationCode(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Employee emp = new Employee();
                    result = emp.GetInvitationCode(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetInvitationCodeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetInvitationCode");
            string result = GetInvitationCode(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetInvitationCode");
            return result;
        }

        #endregion GetInvitationCode

        #region SaveRegType

        [WebMethod]
        public string SaveRegType(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateRegistration", xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    string id = regApp.Update(xmlMessage);
                    if (id.Trim().Length == 36)
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<" + callType + ">" +
                                  "<Result>True</Result>" +
                                  "<ID>" + id + "</ID>" +
                                  "<Description>操作成功</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SaveRegTypeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetInvitationCode");
            string result = SaveRegType(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetInvitationCode");
            return result;
        }

        #endregion SaveRegType

        #region GetRegType

        [WebMethod]
        public string GetRegType(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetRegistrationData", xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode pNode = doc.SelectSingleNode("GetRegistrationData");
                    XmlNode cNode = doc.CreateElement("Cols");
                    cNode.InnerText = "FID|FRegType";
                    pNode.AppendChild(cNode);

                    RegApplication regApp = new RegApplication();
                    result = regApp.GetDetail(doc.OuterXml);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegTypeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegistrationData");
            string result = GetRegType(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegistrationData");
            return result;
        }

        #endregion GetRegType

        #region SaveRegProduct

        [WebMethod]
        public string SaveRegProduct(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateRegistration", xmlMessage))
                {
                    RegApplication regApp = new RegApplication();

                    string id = regApp.Update(xmlMessage);

                    if (id.Trim().Length == 36)
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<" + callType + ">" +
                                  "<Result>True</Result>" +
                                  "<ID>" + id + "</ID>" +
                                  "<Description>操作成功</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SaveRegProductJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegistration");
            string result = SaveRegProduct(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SaveRegProduct");
            return result;
        }

        #endregion SaveRegProduct

        #region GetRegProduct

        [WebMethod]
        public string GetRegProduct(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetRegistrationData", xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode pNode = doc.SelectSingleNode("GetRegistrationData");
                    XmlNode cNode = doc.CreateElement("Cols");
                    cNode.InnerText = "FID|FProductTypeID|FProductID|FProductName|FProductTypeName";
                    pNode.AppendChild(cNode);

                    RegApplication regApp = new RegApplication();
                    result = regApp.GetDetail(doc.OuterXml);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<GetRegistrationData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetRegistrationData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegProductJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegistrationData");
            string result = GetRegProduct(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegistrationData");
            return result;
        }

        #endregion GetRegProduct

        #region SaveRegAuthData

        [WebMethod]
        public string SaveRegAuthData(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateRegistration", xmlMessage))
                {
                    //RegApplication regApp = new RegApplication();
                    //result = regApp.Update(xmlMessage);

                    RegApplication regApp = new RegApplication();

                    string id = regApp.Update(xmlMessage);

                    if (id.Trim().Length == 36)
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<" + callType + ">" +
                                  "<Result>True</Result>" +
                                  "<ID>" + id + "</ID>" +
                                  "<Description>操作成功</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SaveRegAuthDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegistration");
            string result = SaveRegAuthData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SaveRegAuthData");
            return result;
        }

        #endregion SaveRegAuthData

        #region GetRegAuthData

        [WebMethod]
        public string GetRegAuthData(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetRegistrationData", xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode pNode = doc.SelectSingleNode("GetRegistrationData");
                    XmlNode cNode = doc.CreateElement("Cols");
                    cNode.InnerText = "FID|FHospitalID|FProvinceID|FCityID|FCountryID|FHospitalName|FProvinceName|FCityName|FCountryName";
                    pNode.AppendChild(cNode);

                    RegApplication regApp = new RegApplication();
                    result = regApp.GetDetail(doc.OuterXml);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegAuthDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegistrationData");
            string result = GetRegAuthData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegistrationData");
            return result;
        }

        #endregion GetRegAuthData

        #region SaveRegPerformance

        [WebMethod]
        public string SaveRegPerformance(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateRegistration", xmlMessage))
                {
                    //RegApplication regApp = new RegApplication();
                    //result = regApp.Update(xmlMessage);

                    RegApplication regApp = new RegApplication();

                    string id = regApp.Update(xmlMessage);

                    if (id.Trim().Length == 36)
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<" + callType + ">" +
                                  "<Result>True</Result>" +
                                  "<ID>" + id + "</ID>" +
                                  "<Description>操作成功</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SaveRegPerformanceJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegistration");
            string result = SaveRegPerformance(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SaveRegProduct");
            return result;
        }

        #endregion SaveRegPerformance

        #region GetRegPerformance

        [WebMethod]
        public string GetRegPerformance(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetRegistrationData", xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode pNode = doc.SelectSingleNode("GetRegistrationData");
                    XmlNode cNode = doc.CreateElement("Cols");
                    cNode.InnerText = "FID|FHistoryPerformance|FForecastPerformance";
                    pNode.AppendChild(cNode);

                    cNode = doc.CreateElement("PageID");
                    cNode.InnerText = "Reg002|Reg003";
                    pNode.AppendChild(cNode);

                    RegApplication regApp = new RegApplication();
                    result = regApp.GetDetail(doc.OuterXml);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegPerformanceJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegistrationData");
            string result = GetRegPerformance(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegistrationData");
            return result;
        }

        #endregion GetRegPerformance

        #region SaveRegApplicant

        [WebMethod]
        public string SaveRegApplicant(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateRegistration", xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    string id = regApp.UpdateRegApplicant(xmlMessage);

                    if (id.Trim().Length == 36)
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<" + callType + ">" +
                                  "<Result>True</Result>" +
                                  "<ID>" + id + "</ID>" +
                                  "<Description>操作成功</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SaveRegApplicantJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegistration");
            string result = SaveRegApplicant(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SaveRegApplicant");
            return result;
        }

        #endregion SaveRegApplicant

        #region GetRegApplicant

        [WebMethod]
        public string GetRegApplicant(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetRegistrationData", xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.GetRegApplicant(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegApplicantJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegistrationData");
            string result = GetRegApplicant(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegistrationData");
            return result;
        }

        #endregion GetRegApplicant

        #region GetRegRelationShip

        [WebMethod]
        public string GetRegRelationShip(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetRegistrationData", xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.GetRegRelationShip(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegRelationShipJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegistrationData");
            string result = GetRegRelationShip(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegistrationData");
            return result;
        }

        #endregion GetRegRelationShip

        #region SendVCode

        [WebMethod]
        public string SendVCode(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.SendVCode(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SendVCodeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "SendVCode");
            string result = SendVCode(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SendVCode");
            return result;
        }

        #endregion SendVCode

        #region CheckVCode

        [WebMethod]
        public string CheckVCode(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.CheckVCode(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string CheckVCodeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "CheckVCode");
            string result = CheckVCode(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "CheckVCode");
            return result;
        }

        #endregion CheckVCode

        #region SaveRegRelationship

        [WebMethod]
        public string SaveRegRelationship(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                           "<" + callType + ">" +
                           "<Result>False</Result>" +
                           "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateRegistration", xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    string id = regApp.UpdateRegRelationship(xmlMessage);

                    if (id.Trim().Length == 36)
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<" + callType + ">" +
                                  "<Result>True</Result>" +
                                  "<ID>" + id + "</ID>" +
                                  "<Description>操作成功</Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string SaveRegRelationshipJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegistration");
            string result = SaveRegRelationship(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SaveRegRelationship");
            return result;
        }

        #endregion SaveRegRelationship

        #region UploadImage

        [WebMethod]
        public string UploadRegImage(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                           "<" + callType + ">" +
                           "<Result>False</Result>" +
                           "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.UploadImage(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UploadRegImageJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UploadRegImage");
            string result = UploadRegImage(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UploadRegImage");
            return result;
        }

        #endregion UploadImage

        #region GetRegImage

        [WebMethod]
        public string GetRegImage(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                           "<" + callType + ">" +
                           "<Result>False</Result>" +
                           "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.GetImage(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetRegImageJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRegImage");
            string result = GetRegImage(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRegImage");
            return result;
        }

        #endregion GetRegImage

        #region UploadFile

        [WebMethod]
        public string UploadFile(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                           "<" + callType + ">" +
                           "<Result>False</Result>" +
                           "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RegApplication regApp = new RegApplication();
                    result = regApp.UploadFile(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UploadFileJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UploadFile");
            string result = UploadFile(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UploadFile");
            return result;
        }

        #endregion UploadFile
    }
}