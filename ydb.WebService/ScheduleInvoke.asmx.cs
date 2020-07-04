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
    /// Invoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ScheduleInvoke : System.Web.Services.WebService
    {

        #region GetScheduleList

        [WebMethod]
        public string GetScheduleList(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                Schedule o = new Schedule();
                result = o.GetList(xmlMessage);
            }
            catch(Exception err)
            {
                result = "" +
                          "<GetScheduleList>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetScheduleList>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetScheduleListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetScheduleList");
            string result = GetScheduleList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetScheduleList");
            return result;
        }
        #endregion

        #region GetScheduleDetail
        [WebMethod]
        public string GetScheduleDetail(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Schedule h = new Schedule();
                    result = h.GetDetail(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result ="<GetScheduleDetail>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetScheduleDetail>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetScheduleDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetScheduleDetail");
            string result = GetScheduleDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetScheduleDetail");
            return result;
        }
        #endregion


        #region UpdateScheduleData
        [WebMethod]
        public string UpdateScheduleData(string callType, string xmlMessage)
        {
            string result = "",id="-1";
            string logID = Guid.NewGuid().ToString();
           
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    ydb.BLL.Schedule sch = new Schedule();
                    id = sch.Update(xmlMessage);

                    if (id != "-1")
                    {
                        result = "<UpdateScheduleData>" +
                            "<Result>True</Result>" +
                            "<ID>" + id + "</ID>" +
                            "<Description></Description>" +
                            "</UpdateScheduleData>";
                    }
                    else
                    {
                        result = "<UpdateScheduleData>" +
                                    "<Result>False</Result>" +
                                    "<Description></Description>" +
                                    "</UpdateScheduleData>";
                    }
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<UpdateScheduleData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></UpdateScheduleData>";
            }
            FileLogger.WriteLog("UpdateSchedule End:"+result);
            return result;
        }
        [WebMethod]
        public string UpdateScheduleDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateScheduleData");
            FileLogger.WriteLog("UpdateScheduleData XML:" + xmlString, 1, "", callType);
            string result = UpdateScheduleData(callType, xmlString);
            FileLogger.WriteLog("UpdateScheduleData Result:" + result, 1, "", callType);
            result = iTR.Lib.Common.XML2Json(result, "UpdateScheduleData");
            FileLogger.WriteLog("UpdateScheduleData Result2:" + result, 1, "", callType);
            return result;
        }
        #endregion

        #region DeleteSchedule
        [WebMethod]
        public string DeleteSchedule(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    ydb.BLL.Schedule sch = new Schedule();
                    result = sch.Delete(xmlMessage);
                    if (result != "-1")
                        result = "<DeleteSchedule>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<ID>" + result + "</ID>" +
                            "</DeleteSchedule>";
                    else
                        result = "<DeleteSchedule>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "</DeleteSchedule>";
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<GetScheduleList>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetScheduleList>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }
        [WebMethod]
        public string DeleteScheduleJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteSchedule");
            string result = DeleteSchedule(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteSchedule");
            return result;
        }
        #endregion

        #region GetMyScheduleList
        [WebMethod]
        public string GetMyScheduleList(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Schedule s = new Schedule();
                    result = s.GetMyList(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;

        }
        [WebMethod]
        public string GetMyScheduleListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMyScheduleList");
            string result = GetMyScheduleList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMyScheduleList");
            return result;
        }
        #endregion GetMyScheduleList

        #region GetTeamScheduleList
        [WebMethod]
        public string GetTeamScheduleList(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Schedule s = new Schedule();
                    result = s.GetTeamList(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetTeamScheduleListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetTeamScheduleList");
            string result = GetTeamScheduleList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetTeamScheduleList");
            return result;
        }
        #endregion

        
    }
}
