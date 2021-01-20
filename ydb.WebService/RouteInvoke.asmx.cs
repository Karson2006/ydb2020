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
    /// RouteInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。
    // [System.Web.Script.Services.ScriptService]
    public class RouteInvoke : System.Web.Services.WebService
    {
        #region GetRouteList

        [WebMethod]
        public string GetRouteList(string callType, string xmlMessage)
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
                    RouteData rData = new RouteData();
                    result = rData.GetList(xmlMessage);
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
        public string GetRouteListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRouteList");
            string result = GetRouteList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRouteList");
            return result;
        }

        #endregion GetRouteList

        #region GetRouteDetail

        [WebMethod]
        public string GetRouteDetail(string callType, string xmlMessage)
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
                    RouteData rData = new RouteData();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/RouteID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("签到记录ID不能为空");
                    result = rData.GetDetail(vNode.InnerText.Trim());
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
        public string GetRouteDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetRouteDetail");
            string result = GetRouteDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetRouteDetail");
            return result;
        }

        #endregion GetRouteDetail

        #region SignIn

        [WebMethod]
        public string SignIn(string callType, string xmlMessage)
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
                    RouteData rData = new RouteData();
                    result = rData.SignIn(xmlMessage);
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
        public string SignInJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "SignIn");
            string result = SignIn(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SignIn");
            return result;
        }

        #endregion SignIn

        #region SignOut

        [WebMethod]
        public string SignOut(string callType, string xmlMessage)
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
                    RouteData rData = new RouteData();
                    result = rData.SignOut(xmlMessage);
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
        public string SignOutJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "SignOut");
            string result = SignOut(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SignOut");
            return result;
        }

        #endregion SignOut

        #region DeleteRoute

        [WebMethod]
        public string DeleteRoute(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/RouteID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("签到记录ID不能为空");

                    RouteData rData = new RouteData();
                    if (rData.Delete(vNode.InnerText.Trim()) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "C" + doc.SelectSingleNode(callType + "/RouteID").InnerText + "</ID>" +
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
        public string DeleteRouteJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteRoute");
            string result = DeleteRoute(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteRoute");
            return result;
        }

        #endregion DeleteRoute

        /// <summary>
        /// 自动签到和签退
        /// </summary>
        /// <param name="callType"></param>
        /// <param name="xmlMessage"></param>
        /// <returns></returns>

        #region AutoRoute

        [WebMethod]
        public string AutoRoute(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog("自动定位Start:|" + logID + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RouteData rData = new RouteData();
                    result = rData.AutoRoute(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog("自动定位End:|" + logID + "" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string AutoRouteJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "AutoRoute");
            string result = AutoRoute(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "AutoRoute");
            return result;
        }

        [WebMethod]
        public string AlterAutoStatusJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "AlterAutoStatus");
            string result = AlterAutoStatus(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "AlterAutoStatus");
            return result;
        }

        [WebMethod]
        public string AlterAutoStatus(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<" + callType + ">" +
                            "<Result>False</Result>" +
                            "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog("自动签到开关Start:|" + logID + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    RouteData rData = new RouteData();
                    result = rData.AlterAutoStatus(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<" + callType + ">" +
                         "<Result>False</Result>" +
                         "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog("自动签到开关End:|" + logID + "" + result, 1, "", callType);
            return result;
        }

        #endregion AutoRoute
    }
}