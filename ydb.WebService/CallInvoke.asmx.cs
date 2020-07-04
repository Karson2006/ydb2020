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
    /// CallService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class CallInvoke : System.Web.Services.WebService
    {

        #region GetCallList
        [WebMethod]
        public string GetCallList(string callType, string xmlMessage)
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
                    CallData call = new CallData();
                    result = call.GetListXML(xmlMessage);
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
        public string GetCallListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetCallList");
            string result = GetCallList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetCallList");
            return result;
        }
        #endregion

        #region GetMyCallList
        [WebMethod]
        public string GetMyCallList(string callType, string xmlMessage)
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
                    CallData call = new CallData();
                    result = call.GetMyList(xmlMessage);
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
        public string GetMyCallListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMyCallList");
            string result = GetMyCallList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMyCallList");
            return result;
        }
        #endregion

        #region GetTeamCallList
        [WebMethod]
        public string GetTeamCallList(string callType, string xmlMessage)
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
                    CallData call = new CallData();
                    result = call.GetTeamList(xmlMessage);
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
        public string GetTeamCallListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetTeamCallList");
            string result = GetTeamCallList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetTeamCallList");
            return result;
        }

        #endregion


        #region GetCallDetail
        [WebMethod]
        public string GetCallDetail(string callType, string xmlMessage)
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
                   
                    CallData call = new CallData();
                    result = call.GetDetail(xmlMessage);
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
        public string GetCallDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetCallDetail");
            string result = GetCallDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetCallDetail");
            return result;
        }

        #endregion

        #region DeleteCall
        [WebMethod]
        public string DeleteCall(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
 
                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                   

                    CallData call = new CallData();
                    result = call.Delete(xmlMessage);
                    if (result != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>"+result +"</ID>" +
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
        public string DeleteCallJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteCall");
            string result = DeleteCall(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteCall");
            return result;
        }
        #endregion

        #region UpdateCallData
        [WebMethod]
        public string UpdateCallData(string callType, string xmlMessage)
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
                    CallData call = new CallData();
                    result = call.Update(xmlMessage);
                    if(result!="-1")
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
                          "<UpdateCallData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></UpdateCallData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UpdateCallDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateCallData");
            string result = UpdateCallData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateCallData");
            return result;
        }
        #endregion
    }
}
