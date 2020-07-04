using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using ydb.BLL;
using System.Xml;


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
    public class ClientInvoke : System.Web.Services.WebService
    {
        #region GetHospitalList
        [WebMethod]
        public string GetHospitalList(string callType, string xmlMessage)
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
                    Hospital h = new Hospital();

                    result = h.GetHospitalList(xmlMessage);
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
        public string GetHospitalListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetHospitalList");
            string result = GetHospitalList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetHospitalList");
            return result;
        }
        #endregion

        #region GetHospitalDetail
        [WebMethod]
        public string GetHospitalDetail(string callType, string xmlMessage)
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
                    Hospital h = new Hospital();
                    result = h.GetHospitalDetail(xmlMessage);
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
        public string GetHospitalDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetHospitalDetail");
            string result = GetHospitalDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetHospitalDetail");
            return result;
        }

        #endregion

        #region DeleteHospital
        [WebMethod]
        public string DeleteHospital(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();
            try
            {
                
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("ID不能为空");

                    id = vNode.InnerText.Trim();
                    Hospital h = new Hospital();
                    if (h.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteHospitalJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteHospital");
            string result = DeleteHospital(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteHospital");
            return result;
        }

        #endregion

        #region UpdateHospital
        [WebMethod]
        public string UpdateHospital(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

            try
            {
                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    Hospital h = new  Hospital();
                    result = h.Update(xmlMessage);
                    if (result != "-1")
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                 "<ID>" + result + "</ID>" +
                                 "<Description></Description></" + callType + ">";

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
        public string UpdateHospitalJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateHospital");
            string result = UpdateHospital(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateHospital");
            return result;
        }

        #endregion

        #region GetHospitalDeptList
        [WebMethod]
        public string GetHospitalDeptList(string callType, string xmlMessage)
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
                    HospitalDepartment d = new HospitalDepartment();

                    result = d.GetList(xmlMessage);
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
        public string GetHospitalDeptListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetHospitalDeptList");
            string result = GetHospitalDeptList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetHospitalDeptList");
            return result;
        }
        #endregion

        #region GetHospitalDeptDetail
        [WebMethod]
        public string GetHospitalDeptDetail(string callType, string xmlMessage)
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
                    HospitalDepartment d = new HospitalDepartment();
                    result = d.GetDetail(xmlMessage);
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
        public string GetHospitalDeptDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetHospitalDeptDetail");
            string result = GetHospitalDeptDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetHospitalDeptDetail");
            return result;
        }
        #endregion

        #region DeleteHospitalDept
        [WebMethod]
        public string DeleteHospitalDept(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("ID不能为空");

                    id = vNode.InnerText.Trim();
                    HospitalDepartment d = new HospitalDepartment();
                    if (d.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteHospitalDeptJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteHospitalDept");
            string result = DeleteHospitalDept(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteHospitalDept");
            return result;
        }

        #endregion

        #region UpdateHospitalDept
        [WebMethod]
        public string UpdateHospitalDept(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
 
            try
            {
                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    HospitalDepartment d = new HospitalDepartment();
                    result = d.Update(xmlMessage);
                    if (result != "-1")
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                 "<ID>" + result + "</ID>" +
                                 "<Description></Description></" + callType + ">";

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
        public string UpdateHospitalDeptDeptJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateHospitalDept");
            string result = UpdateHospitalDept(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateHospitalDept");
            return result;
        }
        #endregion

        #region GetDoctorList
        [WebMethod]
        public string GetDoctorList(string callType, string xmlMessage)
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
                    Doctor h = new Doctor();

                    result = h.GetList(xmlMessage);
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
        public string GetDoctorListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetDoctorList");
            string result = GetDoctorList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetDoctorList");
            return result;
        }

        #endregion

        #region GetDoctorDetail
        [WebMethod]
        public string GetDoctorDetail(string callType, string xmlMessage)
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
                    Doctor h = new Doctor();
                    result = h.GetDetail(xmlMessage);
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
        public string GetDoctorDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetDoctorDetail");
            string result = GetDoctorDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetDoctorDetail");
            return result;
        }
        #endregion

        #region DeleteDoctor
        [WebMethod]
        public string DeleteDoctor(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
 
                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("ID不能为空");

                    id = vNode.InnerText.Trim();
                    Doctor h = new Doctor();
                    if (h.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteDoctorJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteDoctor");
            string result = DeleteDoctor(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteDoctor");
            return result;
        }
        #endregion

        #region UpdateDoctor
        [WebMethod]
        public string UpdateDoctor(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    Doctor h = new Doctor();
                    result = h.Update(xmlMessage);
                    if (result != "-1")
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                 "<ID>" + result + "</ID>" +
                                 "<Description></Description></" + callType + ">";

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
        public string UpdateDoctorJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateDoctor");
            string result = UpdateDoctor(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateDoctor");
            return result;
        }
        #endregion

        #region GetAgencyList
        [WebMethod]
        public string GetAgencyList(string callType, string xmlMessage)
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
                    ydbAgency h = new ydbAgency();

                    result = h.GetList(xmlMessage);
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
        public string GetAgencyListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAgencyList");
            string result = GetAgencyList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAgencyList");
            return result;
        }
        #endregion

        #region GetAgencyDetail
        [WebMethod]
        public string GetAgencyDetail(string callType, string xmlMessage)
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
                    ydbAgency h = new ydbAgency();
                    result = h.GetDetail(xmlMessage);
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
        public string GetAgencyDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAgencyDetail");
            string result = GetAgencyDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAgencyDetail");
            return result;
        }
        #endregion

        #region DeleteAgency
        [WebMethod]
        public string DeleteAgency(string callType, string xmlMessage)
        {
            string result = "", id = "";

            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("ID不能为空");

                    id = vNode.InnerText.Trim();
                    ydbAgency h = new ydbAgency();
                    if (h.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteAgencyJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteAgency");
            string result = DeleteAgency(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteAgency");
            return result;
        }
        #endregion

        #region ApproveDoctor
        [WebMethod]
        public string ApproveDoctor(string callType, string xmlMessage)
        {
            string result = "", id = "";

            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {

                    Doctor h = new Doctor();
                    id = h.Approve(xmlMessage);
                    if ( id!= "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string ApproveDoctorJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteAgency");
            string result = ApproveDoctor(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteAgency");
            return result;
        }
        #endregion

        #region UpdateAgency
        [WebMethod]
        public string UpdateAgency(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    ydbAgency h = new ydbAgency();
                    result = h.Update(xmlMessage);
                    if (result != "-1")
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                 "<ID>" + result + "</ID>" +
                                 "<Description></Description></" + callType + ">";

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
        public string UpdateAgencyJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateAgency");
            string result = UpdateAgency(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateAgency");
            return result;
        }
        #endregion


        [WebMethod]
        public string GetMyDoctorList(string xmlMessage)
        {
            string result = "<GetData>" +
                           "<Result>False</Result>" +
                           "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "GetMyDoctorList");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    Doctor  doc = new Doctor();
                    result = doc.GetMyList(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetMyDoctorList");
            return result;
        }


        [WebMethod]
        public string GetMyDoctorListJson(string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = GetMyDoctorList(xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }

    }
    
}
