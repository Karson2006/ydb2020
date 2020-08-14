using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using ydb.BLL;
using System.Xml;
using System.Globalization;
using ydb.BLL.Works; 


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
    public class SCMInvoke : System.Web.Services.WebService
    {

        #region UpdateHospitalStock
        [WebMethod]
        public string UpdateHospitalStock(string callType, string xmlMessage)
        {
            string result = "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode("UpdateHospitalStock", xmlMessage))//数据校验通过
                {
                     SCM s = new  SCM();
                    result = s.UpdateHospitalStock(xmlMessage);
                    if (result != "-1")
                        result = "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                  "<ID>" + result + "</ID>" +
                                 "<Description></Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result ="<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UpdateHospitalStockJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateHospitalStock");
            string result = UpdateHospitalStock(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateHospitalStock");
            return result;
        }
        #endregion

        #region GetHospitalStockDetail
        [WebMethod]
        public string GetHospitalStockDetail(string callType, string xmlMessage)
        {
            string result ="<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();
            
            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    SCM s = new SCM();
                    result = s.GetHospitalStockDetail(xmlMessage);
                    
                }
            }
            catch (Exception err)
            {
                result ="<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetHospitalStockDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, callType);
            string result = GetHospitalStockDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, callType);
            return result;
        }
        #endregion

        #region GetHospitalStockList
        [WebMethod]
        public string GetHospitalStockList(string callType, string xmlMessage)
        {
            string result = "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            string logID = Guid.NewGuid().ToString();

            try
            {

                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    SCM s = new SCM();
                    result = s.GetHospitalStockList(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetHospitalStockListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, callType);
            string result = GetHospitalStockList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, callType);
            return result;
        }
        #endregion



    }

}
