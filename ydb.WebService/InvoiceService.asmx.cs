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
using iTR.OP.Invoice;


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
    public class InvoiceService : System.Web.Services.WebService
    {

        #region UpdateHospitalStock
        [WebMethod]
        public string InvoiceCheck(string xmlMessage)
        {
            string result = "<UpdateData>" +
                          "<Result>False</Result>" +
                          "<Description></Description></UpdateData>";

            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", "InvoiceCheck");

                if (Helper.CheckAuthCode("UpdateData", xmlMessage))//数据校验通过
                {
                    InvoiceHelper obj = new InvoiceHelper();

                    result = obj.InvoiceCheck(xmlMessage);
                   
                }
            }
            catch (Exception err)
            {
                result = "<UpdateData>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></UpdateData>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", "InvoiceCheck");
            return result;
        }

        [WebMethod]
        public string InvoiceCheckJson(string JsonMessage)
        {
            FileLogger.WriteLog("Start:" + JsonMessage, 1, "", "InvoiceCheckJson");
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateData");
            string result = InvoiceCheck( xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateData");
            return result;
        }
        #endregion

        [WebMethod]
        public string GetIDJson(string callType, string JsonMessage)
        {
            string result = "";
            result = OAInvoiceHelper.GetID();
            return result;
        }

    }

}
