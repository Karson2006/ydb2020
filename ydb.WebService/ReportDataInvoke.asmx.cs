using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTR.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
      string result = "<GetData>" +
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
    public string GetCallReport1Json(string JsonMessage)
    {
      string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
      string result = GetCallReport1(xmlString);
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

    //一级个人页面
    [WebMethod]
    public string GetPersonSummaryReport(string JsonMessage)
    {
      string result = "";
      result = GetCompassReport(JsonMessage, "GetPersonSummaryReport");
      return result;
    }

    #region 已经替换到15的数据接口

    //流程子页面
    [WebMethod]
    public string GetPersonFlowReport(string JsonMessage)
    {
      string result = "";
      result = GetCompassReport(JsonMessage, "GetPersonFlowReport");
      return result;
    }

    //支付子页面
    [WebMethod]
    public string GetPersonPayReport(string JsonMessage)
    {
      string result = "";
      result = GetCompassReport(JsonMessage, "GetPersonPayReport");
      return result;
    }

    //销量子页面
    [WebMethod]
    public string GetPersonSalesReport(string JsonMessage)
    {
      string result = "";
      result = GetCompassReport(JsonMessage, "GetPersonSalesReport");
      return result;
    }

    //支付查询
    [WebMethod]
    public string PayQuery(string JsonMessage)
    {
      string result = "";
      result = GetCompassReport(JsonMessage, "PayQuery");
      return result;
    }

    #endregion 已经替换到15的数据接口

    //报表统一入口
    public string GetCompassReport(string JsonMessage, string callType)
    {
      string result, FormatResult = "{{\"{0}\":{{\"Result\":{1},\"Description\":{2},\"DataRows\":{3} }} }}";
      result = string.Format(FormatResult, callType, "\"False\"", "", "");
      string logID = Guid.NewGuid().ToString();

      try
      {
        FileLogger.WriteLog(logID + "|Start:" + JsonMessage + " FormatResult:" + FormatResult, 1, "", callType);
        if (Helper.CheckAuthCode("GetData", JsonMessage, "json"))
        {
          //罗盘主页
          if (callType == "GetPersonSummaryReport")
          {
            PersonalCompass perRpt = new PersonalCompass();
            //没有类型判断，全部获取
            result = perRpt.GetPersonPerReport(JsonMessage, FormatResult, callType);
          }
          //流程子页面
          else if (callType == "GetPersonFlowReport")
          {
            PersonalChildpage perChildRpt = new PersonalChildpage();
            result = perChildRpt.GetPersonChildData(JsonMessage, FormatResult, callType, "3");
          }
          //支付子页面
          else if (callType == "GetPersonPayReport")
          {
            PersonalChildpage perChildRpt = new PersonalChildpage();
            result = perChildRpt.GetPersonChildData(JsonMessage, FormatResult, callType, "4");
          }
          //销量子页面
          else if (callType == "GetPersonSalesReport")
          {
            PersonalChildpage perChildRpt = new PersonalChildpage();
            result = perChildRpt.GetPersonChildData(JsonMessage, FormatResult, callType, "6");
          }
          //支付查询
          else if (callType == "PayQuery")
          {
            PersonalChildpage perChildRpt = new PersonalChildpage();
            result = perChildRpt.PayQuery(JsonMessage, FormatResult, callType);
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

    [WebMethod]
    //流程同步
    public string SyncFlow(string callType, string xmlMessage)
    {
      string result = "";
      result = GetSyncResult(xmlMessage, "SyncFlow");
      return result;
    }

    [WebMethod]
    //销量同步
    public string SyncSales(string callType, string xmlMessage)
    {
      string result = "";
      result = GetSyncResult(xmlMessage, "SyncSales");
      return result;
    }

    [WebMethod]
    //支付同步
    public string SyncPay(string callType, string xmlMessage)
    {
      string result = "";
      result = GetSyncResult(xmlMessage, "SyncPay");
      return result;
    }

    //同步统一入口
    public string GetSyncResult(string xmlMessage, string callType)
    {
      string logID = Guid.NewGuid().ToString();
      string result = "<GetData>" +
         "<Result>False</Result>" +
         "<Description></Description><DataRows></DataRows></GetData>";
      try
      {
        FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
        if (Helper.CheckAuthCode("GetData", xmlMessage))
        {
          result = OASyncHelper.SyncHelper(xmlMessage, callType);
        }
      }
      catch (Exception err)
      {
        result = "" +

                  "<GetData>" +
                  "<Result>False</Result>" +
                  "<Description>" + err.Message + "</Description><DataRows></DataRows></GetData>";
      }
      FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
      return result;
    }

    #region 多级

    [WebMethod]
    public string GetMultiReportJson(string JsonMessage)
    {
      string result = @"{{""GetMultiReportJson"":{{ ""Result"":""false"",""Description"":"""",""DataRows"":"""" }} }}";
      string logID = Guid.NewGuid().ToString();
      FileLogger.WriteLog(logID + "|Start:" + JsonMessage, 1, "", "GetMultiReportJson");
      try
      {
        if (Helper.CheckAuthCode("GetMultiReportJson", JsonMessage, "json"))
        {
          string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
          CallRpt rpt = new CallRpt();
          result = rpt.GetMultiCallReport(xmlString);
        }
      }
      catch (Exception err)
      {
        result = $@"{{""GetMultiReportJson"":{{ ""Result"":""false"",""Description"":""{ err.Message}"",""DataRows"":"""" }} }}";
      }
      FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetMultiReportJson");
      return result;
    }

    [WebMethod]
    public string GetMenuListJson(string JsonMessage)
    {
      string result = @"{{""GetMenuListJson"":{{ ""Result"":""false"",""Description"":"""",""DataRows"":"""" }} }}";
      string logID = Guid.NewGuid().ToString();
      FileLogger.WriteLog(logID + "|Start:" + JsonMessage, 1, "", "GetMenuListJson");
      try
      {
        if (Helper.CheckAuthCode("GetMenuListJson", JsonMessage, "json"))
        {
          string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetData");
          CallRpt rpt = new CallRpt();
          result = rpt.GetMenuList(xmlString);
        }
      }
      catch (Exception err)
      {
        result = $@"{{""GetMenuListJson"":{{ ""Result"":""false"",""Description"":""{ err.Message}"",""DataRows"":"""" }} }}";
      }
      FileLogger.WriteLog(logID + "|End:" + result, 1, "", "GetMenuListJson");
      return result;
    }

    //[WebMethod]
    //public string GetMultiReportJson(string JsonMessage)
    //{
    //    string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMultiReport");
    //    string result = GetMultiReport(xmlString);
    //    result = iTR.Lib.Common.XML2Json(result, "GetMultiReport");
    //    return result;
    //}

    #endregion 多级
  }
}