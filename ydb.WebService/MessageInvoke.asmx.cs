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
    public class MessageInvoke : System.Web.Services.WebService
    {

        #region GetMessageList

        [WebMethod]
        public string GetMessageList(string callType, string xmlMessage)
        {
            string result = "",  type = "99",receiverId="";
            DateTime bdate= DateTime.Now, expiratoindate=DateTime.Now,edate= DateTime.Now;

            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (CheckAuthCode(callType, xmlMessage))
                {
                    ydbMessage msg = new ydbMessage();
                    result = msg.GetListXML(xmlMessage);
                }
            }
            catch(Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<GetMessageList>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetMessageList>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetMessageListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMessageList");
            string result = GetMessageList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMessageList");
            return result;
        }
        #endregion

        #region UpdateMassage
        [WebMethod]
        public string UpdateMessage(string callType, string xmlMessage)
        {
            string result = "";

            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    ydb.BLL.ydbMessage msg = new ydbMessage();
                    result = msg.Update(xmlMessage);
                    if (result.Length == 36)//成功更新记录
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><" + callType + ">" +
                                    "<Result>True</Result>" +
                                    "<Description></Description>" +
                                    "<FMsgID>" + result + "</FMsgID>" +
                                    "</" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(callType+" End:"+result);
            return result;
        }
         [WebMethod]
        public string UpdateMessageJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateMessage");
            string result = UpdateMessage(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateMessage");
            return result;
        }

        private Boolean CheckAuthCode(string callType, string xmlString)
        {
            Boolean result = false;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNode vNode = doc.SelectSingleNode(callType + "/AuthCode");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
                throw new Exception("授权代码不能为空");
            else
            {
                if (!BLCommon.CheckAuthCode(vNode.InnerText))
                    throw new Exception("授权代码不正确");
            }
            
            result = true;
            return result;
        }

        #endregion

        #region DeleteMessage
        [WebMethod]
        public string DeleteMessage(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
           
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();

                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/MessageID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("消息ID不能为空");
                    ydbMessage msg = new ydbMessage();
                    result = msg.Delete(vNode.InnerText);
                    if(result!="-1")//删除成功
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result><FMsgID>" + vNode.InnerText + "</FMsgID>" +
                                "<Description></Description></" + callType + ">";
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<DeleteMessage>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></DeleteMessage>";
            }
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }
        public string DeleteMessageJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteMessage");
            string result = DeleteMessage(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteMessage");
            return result;
        }
        
        #endregion

        #region GetMessageDetail
        [WebMethod]
        public string GetMessageDetail(string callType, string xmlMessage)
        {
           string result = "";
           string logID = Guid.NewGuid().ToString();
           
           try
           {
               FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

               if (CheckAuthCode(callType, xmlMessage))
               {
                   XmlDocument doc = new XmlDocument();

                   doc.LoadXml(xmlMessage);
                   XmlNode vNode = doc.SelectSingleNode(callType + "/MessageID");
                   if (vNode == null || vNode.InnerText.Trim().Length == 0)
                       throw new Exception("消息ID不能为空");

                   ydb.BLL.ydbMessage msg = new ydbMessage();
                   result = msg.GetDetailXML(vNode.InnerText.Trim());
               }
           }
           catch (Exception err)
           {
               result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                         "<GetMessageDetail>" +
                         "<Result>False</Result>" +
                         "<Description>" + err.Message + "</Description></GetMessageDetail>";
           }
           FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
           return result;
        }
         [WebMethod]
        public string GetMessageDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMessageDetail");
            string result = GetMessageDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMessageDetail");
            return result;
        }
        #endregion

        #region GetMessageList

        [WebMethod]
        public string GetLogList(string callType, string xmlMessage)
        {
            string result =  "<GetLogList>" +
                          "<Result>False</Result>" +
                          "<Description></Description>" +
                          "</GetLogList>";

            try
            {

                if (CheckAuthCode(callType, xmlMessage))
                {

                    ydbMessage msg = new ydbMessage();
                    result = msg.GetLogList(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<GetLogList>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></GetLogList>";
            }
            return result;
        }
        [WebMethod]
        public string GetLogListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetLogList");
            string result = GetLogList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetLogList");
            return result;
        }
        #endregion
        

    }
}
