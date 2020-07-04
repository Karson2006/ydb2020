using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using ydb.Report;

namespace ydb.WebService
{
    /// <summary>
    /// ReportDataInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ReportDataInvoke : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetCallReport1(string xmlMessage)
        {
           string result ="<GetData>" +
                          "<Result>False</Result>" +
                          "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "GetCallReport1");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    CallRpt rpt = new CallRpt();
                    result = rpt.GetCallRepotr1(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetCallReport1");
            return result;
        }


        [WebMethod]
        public string GetCallReport1Json( string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = GetCallReport1( xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }

        [WebMethod]
        public string GetCallReport2(string xmlMessage)
        {
            string result = "<GetData>" +
                           "<Result>False</Result>" +
                           "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "GetCallRepotr2");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    CallRpt rpt = new CallRpt();
                    result = rpt.GetCallRepotr2(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "" +
                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetCallRepotr2");
            return result;
        }


        [WebMethod]
        public string GetCallReport2Json(string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = GetCallReport2(xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }

        [WebMethod]
        public string ExportCallReport(string xmlMessage)
        {
            string result = "<GetData>" +
                           "<Result>False</Result>" +
                           "<Description></Description></GetData>";
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "ExportCallReport");

                if (Helper.CheckAuthCode("GetData", xmlMessage))
                {
                    CallRpt rpt = new CallRpt();
                    result = rpt.ExportCallReport(xmlMessage);
                }
 
            }
            catch (Exception err)
            {
                result = "" +

                          "<GetData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "ExportCallReport");
            return result;
        }

        [WebMethod]
        public string ExportCallReportJson(string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
            string result = ExportCallReport(xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetData");
            return result;
        }
    }
}
