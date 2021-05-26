using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using ydb.BLL;
using System.Web.Script.Serialization;
using iTR.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ydb.WebService
{
    public class Helper
    {
        public Helper()
        { }

        #region CheckAuthCode

        public static Boolean CheckAuthCode(string callType, string xmlString, string strType = "xml")
        {
            Boolean result = false;
            if (strType == "xml")
            {
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
            }
            else
            {
                JObject obj = JObject.Parse(xmlString);
                if (obj["AuthCode"] == null || obj["AuthCode"].ToString().Length == 0)
                {
                    throw new Exception("授权代码不能为空");
                }
                else
                {
                    if (!BLCommon.CheckAuthCode(obj["AuthCode"].ToString()))
                        throw new Exception("授权代码不正确");
                }
            }

            result = true;
            return result;
        }

        public static Boolean CheckAuthCodeJson(string callType, string authcode)
        {
            Boolean result = false;

            if (authcode.Trim().Length == 0)
                throw new Exception("授权代码不能为空");
            else
            {
                if (!BLCommon.CheckAuthCode(authcode))
                    throw new Exception("授权代码不正确");
            }

            result = true;
            return result;
        }

        #endregion CheckAuthCode
    }
}