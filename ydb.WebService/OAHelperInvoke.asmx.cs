using iTR.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

//using ydb.OAHelper;

namespace ydb.WebService
{
    /// <summary>
    /// OAHelperInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。
    // [System.Web.Script.Services.ScriptService]
    public class OAHelperInvoke : System.Web.Services.WebService
    {
        //OA表单统一入口
        public string GetFinalResult(string JsonMessage, string callType)
        {
            string result, FormatResult = "{{\"{0}\":{{\"Result\":{1},\"Description\":{2},\"DataRows\":{3} }} }}";
            result = string.Format(FormatResult, callType, "\"False\"", "", "");
            string logID = Guid.NewGuid().ToString();
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + JsonMessage, 1, "", callType);
                if (Helper.CheckAuthCode("GetData", JsonMessage, "json"))
                {
                    //自动查验发票
                    if (callType == "AutoCheck")
                    {
                        //   InvoiceCheck.Check(FormatResult,callType,"","");
                    }
                    //手动查验发票
                    else if (callType == "ManualCheck")
                    {
                        // InvoiceCheck.ManualCheck();
                    }
                }
            }
            catch (Exception err)
            {
                result = string.Format(FormatResult, callType, "\"False\"", err.Message, "");
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }
    }
}