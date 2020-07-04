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
    /// ItemInvoke 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class AuthInvoke : System.Web.Services.WebService
    {
        #region UpdateAuthData
        [WebMethod]
        public string UpdateAuthData(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.Update(xmlMessage);
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
            FileLogger.WriteLog(logID+"End:" + result, 1, "", callType);;
            return result;
        }

        [WebMethod]
        public string UpdateAuthDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateAuthData");
            string result = UpdateAuthData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateAuthData");
            return result;
        }
        #endregion

        #region GetAuthDataList
        [WebMethod]
        public string GetAuthDataList(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetList(xmlMessage);
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
        public string GetAuthDataListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAuthDataList");
            string result = GetAuthDataList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAuthDataList");
            return result;
        }

        #endregion

        #region GetProductListByHospitalID
        [WebMethod]
        public string GetProductListByHospitalID(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
            callType = "GetAuthDataList";
            try
            {
                if (Helper.CheckAuthCode(callType, xmlMessage))//数据校验通过
                {
                    AuthData h = new AuthData();
                    result = h.GetProductListByHospitalID(xmlMessage);
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
        public string GetProductListByHospitalIDJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAuthDataList");
            string result = GetProductListByHospitalID(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAuthDataList");
            return result;
        }
        #endregion

        #region UpdateHospitalProducts
        [WebMethod]
        public string UpdateHospitalProducts(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.UpdateHospitalProducts(xmlMessage);
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
        public string UpdateHospitalProductsJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateHospitalProducts");
            string result = UpdateHospitalProducts(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateHospitalProducts");
            return result;
        }
        #endregion

        #region UpdateRegionProducts
        [WebMethod]
        public string UpdateRegionProducts(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.UpdateRegionProducts(xmlMessage);
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
        public string UpdateRegionProductsJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegionProducts");
            string result = UpdateRegionProducts(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateRegionProducts");
            return result;
        }
        #endregion

        #region UpdateHospitalOwners
        [WebMethod]
        public string UpdateHospitalOwners(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.UpdateHospitalOwners(xmlMessage);
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
        public string UpdateHospitalOwnersJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateHospitalOwners");
            string result = UpdateHospitalOwners(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateHospitalOwners");
            return result;
        }
        #endregion

        #region UpdateRegionOwners
        [WebMethod]
        public string UpdateRegionOwners(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.UpdateRegionOwners(xmlMessage);
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
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UpdateRegionOwnersJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateRegionOwners");
            string result = UpdateRegionOwners(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateRegionOwners");
            return result;
        }

        #endregion

        #region UpdateAgencyOwners
        [WebMethod]
        public string UpdateAgencyOwners(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.UpdateAgencyOwners(xmlMessage);
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
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UpdateAgencyOwnersJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateAgencyOwners");
            string result = UpdateAgencyOwners(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateAgencyOwners");
            return result;
        }
        #endregion

        #region GetMyClientList
        [WebMethod]
        public string GetMyClientList(string callType, string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";

            string logID = Guid.NewGuid().ToString();
            FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
            try
            {
                if (Helper.CheckAuthCode("GetAuthDataList", xmlMessage))//数据校验通过
                {
                    AuthData h = new AuthData();
                    result = h.GetMyClientList(xmlMessage);
                   
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetMyClientListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAuthDataList");
            string result = GetMyClientList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAuthDataList");
            return result;
        }
        #endregion

        #region GetMyProductList
        [WebMethod]
        public string GetMyProductList(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetMyProductList(xmlMessage);

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
        public string GetMyProductListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetMyProductList");
            string result = GetMyProductList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetMyProductList");
            return result;
        }

        #endregion

        #region GetDetailByHospitalID
        [WebMethod]
        public string GetDetailByHospitalID(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetDetailByHospitalID(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetDetailByHospitalIDJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetDetailByHospitalID");
            string result = GetDetailByHospitalID(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetDetailByHospitalID");
            return result;
        }
        #endregion

        #region GetDetailByAgencyID
        [WebMethod]
        public string GetDetailByAgencyID(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetDetailByAgencyID(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }
        #endregion

        #region GetDetailByRegionID
        [WebMethod]
        public string GetDetailByRegionID(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetDetailByRegionID(xmlMessage);
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetDetailByRegionIDJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetDetailByRegionID");
            string result = GetDetailByRegionID(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetDetailByRegionID");
            return result;
        }
        #endregion

        #region GetAuthHospitalList
        [WebMethod]
        public string GetAuthHospitalList(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetAuthHospitalList(xmlMessage);
                   
                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetAuthHospitalListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAuthHospitalList");
            string result = GetAuthHospitalList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAuthHospitalList");
            return result;
        }
        #endregion

        #region GetAuthRegionList
        [WebMethod]
        public string GetAuthRegionList(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetAuthRegionList(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID +"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetAuthRegionListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAuthRegionList");
            string result = GetAuthRegionList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAuthRegionList");
            return result;
        }
        #endregion

        #region GetAuthAgencyList
        [WebMethod]
        public string GetAuthAgencyList(string callType, string xmlMessage)
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
                    AuthData h = new AuthData();
                    result = h.GetAuthAgencyList(xmlMessage);

                }
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description></" + callType + ">";
            }
            FileLogger.WriteLog(logID+"|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string GetAuthAgencyListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAuthAgencyList");
            string result = GetAuthAgencyList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAuthAgencyList");
            return result;
        }
        #endregion

    }
    
}
