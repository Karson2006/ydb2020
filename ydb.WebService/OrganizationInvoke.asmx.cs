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
    public class OrganizationInvoke : System.Web.Services.WebService
    {
        #region GetDepartmentList
        [WebMethod]
        public string GetDepartmentList(string callType, string xmlMessage)
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
                    Department dpt = new Department();
                    result = dpt.GetDepartmentList(xmlMessage);
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
        public string GetDepartmentListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetDepartmentList");
            string result = GetDepartmentList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetDepartmentList");
            return result;
        }
        #endregion

        #region GetDepartmentDetail
        [WebMethod]
        public string GetDepartmentDetail(string callType, string xmlMessage)
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
                    Department dpt = new Department();
                    result = dpt.GetDepartmentDetail(xmlMessage);
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
        public string GetDepartmentDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetDepartmentDetail");
            string result = GetDepartmentDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetDepartmentDetail");
            return result;
        }
        #endregion

        #region DeleteDepartment
        [WebMethod]
        public string DeleteDepartment(string callType, string xmlMessage)
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
                    Department dpt = new Department();
                    if (dpt.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>"+id+"</ID>" +
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
        public string DeleteDepartmentJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteDepartment");
            string result = DeleteDepartment(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteDepartment");
            return result;
        }
        #endregion

        #region UpdateDepartment
        [WebMethod]
        public string UpdateDepartment(string callType, string xmlMessage)
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
                    Department dpt = new Department();
                    result=dpt.Update(xmlMessage) ;
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
        public string UpdateDepartmentJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateDepartment");
            string result = UpdateDepartment(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateDepartment");
            return result;
        }
        #endregion

        #region GetEmployeeList
        [WebMethod]
        public string GetEmployeeList(string callType, string xmlMessage)
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
                    result = emp.GetEmployeeList(xmlMessage);
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
        public string GetEmployeeListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetEmployeeList");
            string result = GetEmployeeList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetEmployeeList");
            return result;
        }
        #endregion

        #region GetEmployeeDetail
        [WebMethod]
        public string GetEmployeeDetail(string callType, string xmlMessage)
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
                    Employee emp = new  Employee();
                    result = emp.GetEmployeeDetail(xmlMessage);
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
        public string GetEmployeeDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetEmployeeDetail");
            string result = GetEmployeeDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetEmployeeDetail");
            return result;
        }
        #endregion

        #region DeleteEmployee
        [WebMethod]
        public string DeleteEmployee(string callType, string xmlMessage)
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
                    Employee emp = new Employee();
                    if (emp.Delete(xmlMessage) != "-1")
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
        public string DeleteEmployeeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteEmployee");
            string result = DeleteEmployee(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteEmployee");
            return result;
        }
        #endregion

        #region UpdateEmployee
        [WebMethod]
        public string UpdateEmployee(string callType, string xmlMessage)
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
                    Employee emp = new  Employee();
                    result = emp.Update(xmlMessage);
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
        public string UpdateEmployeeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateEmployee");
            string result = UpdateEmployee(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateEmployee");
            return result;
        }
        #endregion

        #region GetAllSubItemList
        [WebMethod]
        public string GetAllSubItemList(string callType, string xmlMessage)
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
                    Department dpt = new Department();
                    result = dpt.GetAllSubItemList(xmlMessage);
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
        public string GetAllSubItemListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAllSubItemList");
            string result = GetAllSubItemList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAllSubItemList");
            return result;
        }
        #endregion

        #region GetEmployeeListByDepartment
        [WebMethod]
        public string GetEmployeeListByDepartment(string callType, string xmlMessage)
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
                    XmlDocument doc =new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/DepartmentID");
                    if(vNode==null || vNode.InnerText.Trim().Length ==0)
                        throw new Exception("部门ID不能为空");

                    Employee employee = new Employee();
                    result = employee.GetEmployeeListByDepartment(vNode.InnerText.Trim());
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
        public string GetEmployeeListByDepartmentJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetEmployeeListByDepartment");
            string result = GetEmployeeListByDepartment(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetEmployeeListByDepartment");
            return result;
        }
        #endregion

        #region GetTeamMemberList
        [WebMethod]
        public string GetTeamMemberList(string callType, string xmlMessage)
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
                    WorkShip ws = new WorkShip();
                    result = ws.GetTeamMemberList(xmlMessage);
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
        public string GetTeamMemberListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetTeamMemberList");
            string result = GetTeamMemberList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetTeamMemberList");
            return result;
        }
        #endregion

        #region GetInvitationCode
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
                    Employee ws = new Employee();
                    result = ws.GetInvitationCode(xmlMessage);
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
        #endregion

        #region GetInvitationCode
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
                    Employee ws = new Employee();
                    result = ws.CreateInvitationCode(xmlMessage);
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

        public string CreateInvitationCodeJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "CreateInvitationCode");
            string result = CreateInvitationCode(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "CreateInvitationCode");
            return result;
        }
        #endregion
    }
}
