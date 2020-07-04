using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using ydb.BLL;
using System.Xml;
using System.Globalization;


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
    public class WorkReportInvoke : System.Web.Services.WebService
    {
        #region UpdateDailyReport
        [WebMethod]
        public string UpdateDailyReport(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString(); 
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateWorkReport", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.UpdateDaily(xmlMessage);
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
        public string UpdateDailyReportJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateWorkReport");
            string result = UpdateDailyReport(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateDailyReport");
            return result;
        }
        #endregion

        #region UpdateWeekReport
        [WebMethod]
        public string UpdateWeekReport(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateWorkReport", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.UpdateWeek(xmlMessage);
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
        public string UpdateWeekReportJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateWorkReport");
            string result = UpdateWeekReport(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateWeekReport");
            return result;
        }
        #endregion

        #region UpdateMonthReport
        [WebMethod]
        public string UpdateMonthReport(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateWorkReport", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.UpdateMonth(xmlMessage);
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
        public string UpdateMonthReportJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateWorkReport");
            string result = UpdateMonthReport(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateMonthReport");
            return result;
        }
        #endregion

        #region GetMonthReportList
        [WebMethod]
        public string GetMonthReportList(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetWorkReportList", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.GetMonthList(xmlMessage);
                    
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
        public string GetMonthReportListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetWorkReportList");
            string result = GetMonthReportList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetWorkReportList");
            return result;
        }
        #endregion

        #region GetWeekReportList
        [WebMethod]
        public string GetWeekReportList(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

            try
            {
                if (Helper.CheckAuthCode("GetWorkReportList", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.GetWeekList(xmlMessage);
                    
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
        public string GetWeekReportListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetWorkReportList");
            string result = GetWeekReportList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetWorkReportList");
            return result;
        }
        #endregion

        #region GetDailyReportList
        [WebMethod]
        public string GetDailyReportList(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("GetWorkReportList", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.GetDailyList(xmlMessage);
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
        public string GetDailyReportListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetWorkReportList");
            string result = GetDailyReportList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetWorkReportList");
            return result;
        }
        #endregion

        #region UpdateMarketingActivity
        [WebMethod]
        public string UpdateMarketingActivity(string callType, string xmlMessage)
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
                    MktActivity mkt = new MktActivity();
                    result = mkt.Update(xmlMessage);
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
        public string UpdateMarketingActivityJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateMarketingActivity");
            string result = UpdateMarketingActivity(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateMarketingActivity");
            return result;
        }
        #endregion

        #region GetMarketingActivityList
        [WebMethod]
        public string GetMarketingActivityList(string callType, string xmlMessage)
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
                    MktActivity mkt = new MktActivity();
                    result = mkt.GetList(xmlMessage);
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
        public string GetMarketingActivityListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMarketingActivityList");
            string result = GetMarketingActivityList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMarketingActivityList");
            return result;
        }
        #endregion

        #region GetMarketingActivityDetail
        [WebMethod]
        public string GetMarketingActivityDetail(string callType, string xmlMessage)
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
                    MktActivity mkt = new MktActivity();
                    result = mkt.GetDetail(xmlMessage);
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
        public string GetMarketingActivityDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMarketingActivityDetail");
            string result = GetMarketingActivityDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMarketingActivityDetail");
            return result;
        }
        #endregion

        #region DeleteMarketingActivity
        [WebMethod]
        public string DeleteMarketingActivity(string callType, string xmlMessage)
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
                    MktActivity mkt = new MktActivity();
                    result = mkt.Delete(xmlMessage);
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
        public string DeleteMarketingActivityJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteMarketingActivity");
            string result = DeleteMarketingActivity(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteMarketingActivity");
            return result;
        }
        #endregion

        #region GetFeeList
        [WebMethod]
        public string GetFeeList(string callType, string xmlMessage)
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
                    FeeData fee = new  FeeData();
                    result = fee.GetList(xmlMessage);
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
        public string GetFeeListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetFeeList");
            string result = GetFeeList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetFeeList");
            return result;
        }
        #endregion

        #region UpdateFeeData
        [WebMethod]
        public string UpdateFeeData(string callType, string xmlMessage)
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
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode pNode = doc.SelectSingleNode("UpdateFeeData");
                    XmlNode cNode = doc.CreateElement("Status");
                    cNode.InnerText = "-2";
                    pNode.AppendChild(cNode);
                    xmlMessage = doc.OuterXml;

                    FeeData fee = new FeeData();
                    result = fee.Update(xmlMessage);
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
        public string UpdateFeeDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateFeeData");
            string result = UpdateFeeData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateFeeData");
            return result;
        }
        #endregion

        #region SubmitFeeData
        [WebMethod]
        public string SubmitFeeData(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateFeeData", xmlMessage))//数据校验通过
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode pNode = doc.SelectSingleNode("UpdateFeeData");
                    XmlNode cNode = doc.CreateElement("Status");
                    cNode.InnerText = "0";
                    pNode.AppendChild(cNode);
                    xmlMessage = doc.OuterXml;

                    FeeData fee = new FeeData();

                    result = fee.Update(xmlMessage);
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
        public string SubmitFeeDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "SubmitFeeData");
            string result = SubmitFeeData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "SubmitFeeData");
            return result;
        }
        #endregion

        #region DeleteFeeData
        [WebMethod]
        public string DeleteFeeData(string callType, string xmlMessage)
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
                    FeeData fee = new FeeData();
                    result = fee.Delete(xmlMessage);
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
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);;
            return result;
        }


        [WebMethod]
        public string DeleteFeeDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteFeeData");
            string result = DeleteFeeData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteFeeData");
            return result;
        }
        #endregion

        #region GetTripList
        [WebMethod]
        public string GetTripList(string callType, string xmlMessage)
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
                    FeeData fee = new FeeData();
                    result = fee.GetTripList(xmlMessage);
                   
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType); ;
            return result;
        }

        [WebMethod]
        public string GetTripListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetTripList");
            string result = GetTripList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetTripList");
            return result;
        }
        #endregion

        #region GetExpendList
        [WebMethod]
        public string GetExpendList(string callType, string xmlMessage)
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
                    FeeData fee = new FeeData();
                    result = fee.GetExpendList(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType); ;
            return result;
        }

        [WebMethod]
        public string GetExpendListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetExpendList");
            string result = GetExpendList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetExpendList");
            return result;
        }
        #endregion
        
        #region DeleteExpendData
        [WebMethod]
        public string DeleteExpendData(string callType, string xmlMessage)
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
                    FeeData fee = new FeeData();
                    result = fee.DeleteExpendData(xmlMessage);
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
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType); ;
            return result;
        }


        [WebMethod]
        public string DeleteExpendDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteExpendData");
            string result = DeleteExpendData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteExpendData");
            return result;
        }
        #endregion

        #region DeleteTripData
        [WebMethod]
        public string DeleteTripData(string callType, string xmlMessage)
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
                    FeeData fee = new FeeData();
                    result = fee.DeleteTripData(xmlMessage);
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
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType); ;
            return result;
        }


        [WebMethod]
        public string DeleteTripDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteTripData");
            string result = DeleteTripData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteTripData");
            return result;
        }
        #endregion

        #region GetPerformanceList
        [WebMethod]
        public string GetPerformanceList(string callType, string xmlMessage)
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
                    WorkReport w = new  WorkReport();
                    result = w.GetPerformanceList(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType); ;
            return result;
        }


        [WebMethod]
        public string GetPerformanceListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetPerformanceList");
            string result = GetPerformanceList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetPerformanceList");
            return result;
        }
        #endregion

        #region GetPerformanceDetail
        [WebMethod]
        public string GetPerformanceDetail(string callType, string xmlMessage)
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
                    WorkReport w = new WorkReport();
                    result = w.GetPerformanceDetail(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType); ;
            return result;
        }


        [WebMethod]
        public string GetPerformanceDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetPerformanceDetail");
            string result = GetPerformanceDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetPerformanceDetail");
            return result;
        }
        #endregion

        #region GetFeeDetail
        [WebMethod]
        public string GetFeeDetail(string callType, string xmlMessage)
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
                    FeeData fee = new FeeData();

                    result = fee.GetFeeDetail(xmlMessage);
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
        public string GetFeeDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetFeeDetail");
            string result = GetFeeDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetFeeDetail");
            return result;
        }
        #endregion


        #region GetWeekReportListByIndex
        [WebMethod]
        public string GetWeekReportListByIndex(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<GetWorkReportList>" +
                          "<Result>False</Result>" +
                          "<Description></Description></GetWorkReportList>";

            string logID = Guid.NewGuid().ToString();
           

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                try
                {
                    if (Helper.CheckAuthCode("GetWorkReportListByIndex", xmlMessage))//数据校验通过
                    {
                        WorkReport wr = new WorkReport();
                        result = wr.GetWeekListByIndx(xmlMessage);

                    }
                }
                catch (Exception err)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                              "<GetWorkReportList>" +
                              "<Result>False</Result>" +
                              "<Description>" + err.Message + "</Description></GetWorkReportList>";
                }
                FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
                return result;
          }
                //if (Helper.CheckAuthCode("GetWorkReportList", xmlMessage))//数据校验通过
                //{

                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(xmlMessage);
                    //string weekIndx ="0";
                    //XmlNode vNode = doc.SelectSingleNode("GetWorkReportList/WeekIndex");
                    //if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    //    weekIndx = vNode.InnerText.Trim();

                    //GregorianCalendar gc = new GregorianCalendar();
                    //int year = DateTime.Now.Year;
                    //DateTime date1 = DateTime.Now, date2 = DateTime.Now;
                    //int weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    //switch(weekIndx)
                    //{
                    //    case "0"://本周
                    //        Common.CalcWeekDay(year, weekOfYear, out date1, out date2);
                    //        break;
                    //    case "-1"://上周
                    //        Common.CalcWeekDay(year, weekOfYear-1, out date1, out date2);
                    //        break;
                    //    case "1"://下周
                    //        Common.CalcWeekDay(year, weekOfYear +1, out date1, out date2);
                    //        break;
                    //}
                    //XmlNode pNode = doc.SelectSingleNode("GetWorkReportList");
                    //XmlNode cNode = doc.CreateElement("BeginWeek");
                    //cNode.InnerText = date1.ToString("yyyy-MM-dd");
                    //pNode.AppendChild(cNode);

                    //cNode = doc.CreateElement("EndWeek");
                    //cNode.InnerText = date2.ToString("yyyy-MM-dd");
                    //pNode.AppendChild(cNode);

                    ////WorkReport wr = new WorkReport();
                    ////result = wr.GetWeekList(doc.OuterXml);

                    //result = GetWeekReportList("GetWeekReportList", doc.OuterXml);
            //    }
            //}
            //catch (Exception err)
            //{
            //    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            //              "<" + callType + ">" +
            //              "<Result>False</Result>" +
            //              "<Description>" + err.Message + "</Description></" + callType + ">";
            //}
            //FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            //return result;
        //}

        [WebMethod]
        public string GetWeekReportListByIndexJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetWorkReportListByIndex");
            string result = GetWeekReportListByIndex(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetWorkReportList");
            return result;
        }
        #endregion

        #region
        [WebMethod]
        public string GetMonthReportListByIndex(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<GetWorkReportList>" +
                          "<Result>False</Result>" +
                          "<Description></Description></GetWorkReportList>";

            string logID = Guid.NewGuid().ToString();


            FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

            try
            {
                if (Helper.CheckAuthCode("GetWorkReportListByIndex", xmlMessage))//数据校验通过
                {
                    WorkReport wr = new WorkReport();
                    result = wr.GetMonthListByIndex(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<GetWorkReportList>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetWorkReportList>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetMonthReportListByIndexJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetWorkReportListByIndex");
            string result = GetMonthReportListByIndex(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetWorkReportList");
            return result;
        }
        #endregion
    }
    
}
