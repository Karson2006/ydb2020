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
    public class ItemInvoke : System.Web.Services.WebService
    {
        #region GetClassList
        [WebMethod]
        public string GetClassList(string callType, string xmlMessage)
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
                    ItemClass iclass = new ItemClass();
                    result = iclass.GetListXml(xmlMessage);
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
        public string GetClassListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetClassList");
            string result = GetClassList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetClassList");
            return result;
        }
        #endregion

        #region GetClasslDetail
        [WebMethod]
        public string GetClassDetail(string callType, string xmlMessage)
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
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ClassID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("资料类型ID不能为空");
                    ItemClass iclass = new ItemClass();
                    result = iclass.GetDetailXml(vNode.InnerText.Trim());
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
        public string GetClassDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetClassDetail");
            string result = GetClassDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetClassDetail");
            return result;
        }
        #endregion

        #region DeleteClass
        [WebMethod]
        public string DeleteClass(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ClassID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("资料类型ID不能为空");

                    ItemClass iClass = new ItemClass();
                    id = vNode.InnerText.Trim();
                    if (iClass.Delete(id) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ClassID>"+id+"</ClassID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteClassJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteClass");
            string result = DeleteClass(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteClass");
            return result;
        }
        #endregion

        #region UpdateClassData
        [WebMethod]
        public string UpdateClassData(string callType, string xmlMessage)
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
                    ItemClass iClass  = new ItemClass ();
                    result=iClass.Update(xmlMessage) ;
                    if (result != "-1")
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                 "<ClassID>" + result + "</ClassID>" +
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
        public string UpdateClassDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateClassData");
            string result = UpdateClassData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateClassData");
            return result;
        }
        #endregion

        #region GetItemListByClass
        [WebMethod]
        public string GetItemListByClass(string callType, string xmlMessage)
        {
            string result = "", id = "",level="1";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ClassID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("资料类型ID不能为空");
                    id = vNode.InnerText.Trim();
                    vNode = doc.SelectSingleNode(callType + "/Level");
                    if(vNode!=null)
                    {
                        level = vNode.InnerText.Trim();
                        level = level.Length>0? level:"1";
                     }
                    Items item = new Items();
                    result = item.GetListByClassXml(id, level);
                    
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
        public string GetItemListByClassJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetItemList");
            string result = GetItemListByClass(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetItemList");
            return result;
        }
        #endregion

        #region GetItemListByParent
        [WebMethod]
        public string GetItemListByParent(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ParentID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("资料父级节点ID不能为空");
                    id = vNode.InnerText.Trim();
                   
                    Items item = new Items();
                    result = item.GetListByParentXml(id);

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
        public string GetItemListByParentJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetItemList");
            string result = GetItemListByParent(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetItemList");
            return result;
        }
        #endregion

        #region GetAllByParent
        [WebMethod]
        public string GetAllItemsByParent(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
 
                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ParentID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("父级节点ID不能为空");
                    id = vNode.InnerText.Trim();

                    Items item = new Items();
                    result = item.GetAllByParentXml(id);

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
        public string GetAllItemsByParentJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAllItemsByParent");
            string result = GetAllItemsByParent(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAllItemsByParent");
            return result;
        }
        #endregion

        #region GetAllDetailItemsByParent
        [WebMethod]
        public string GetAllDetailItemsByParent(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
 
                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ParentID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("父级节点ID不能为空");
                    id = vNode.InnerText.Trim();

                    Items item = new Items();
                    result = item.GetAllDetailByParentXml(id);

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
        public string GetAllDetailItemsByParentJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetItemList");
            string result = GetAllDetailItemsByParent(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetItemList");
            return result;
        }
        #endregion

        #region UpdateItemData
        [WebMethod]
        public string UpdateItemData(string callType, string xmlMessage)
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
                    Items item = new Items();
                    result = item.Update(xmlMessage);
                    if (result != "-1")
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                 "<" + callType + ">" +
                                 "<Result>True</Result>" +
                                 "<ClassID>" + result + "</ClassID>" +
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
        public string UpdateItemDataJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateItemData");
            string result = UpdateItemData(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateItemData");
            return result;
        }
        #endregion

        #region DeleteClass
        [WebMethod]
        public string DeleteItem(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    Items item = new Items();
                    if (item.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteItemJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteItem");
            string result = DeleteItem(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteItem");
            return result;
        }
        #endregion

        #region GetItemDetail
        [WebMethod]
        public string GetItemDetail(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();
            
            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);
 
                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("资料ID不能为空");
                    Items iclass = new Items();
                    result = iclass.GetItemDetailXml(vNode.InnerText.Trim());
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
        public string GetItemDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetItemDetail");
            string result = GetItemDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetItemDetail");
            return result;
        }
        #endregion

        #region GetProductList
        [WebMethod]
        public string GetProductList(string callType, string xmlMessage)
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
                    Product  p = new Product();

                    result = p.GetList(xmlMessage);
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
        public string GetProductListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetProductList");
            string result = GetProductList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetProductList");
            return result;
        }
        #endregion

        #region GetProductDetail
        [WebMethod]
        public string GetProductDetail(string callType, string xmlMessage)
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
                    Product h = new Product();
                    result = h.GetDetail(xmlMessage);
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
        public string GetProductDetailJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetProductDetail");
            string result = GetProductDetail(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetProductDetail");
            return result;
        }
        #endregion

        #region DeleteProduct
        [WebMethod]
        public string DeleteProduct(string callType, string xmlMessage)
        {
            string result = "", id = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlMessage);
                    XmlNode vNode = doc.SelectSingleNode(callType + "/ID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        throw new Exception("ID不能为空");

                    id = vNode.InnerText.Trim();
                    Product h = new Product();
                    if (h.Delete(xmlMessage) != "-1")
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>True</Result>" +
                                "<ID>" + id + "</ID>" +
                                "<Description></Description></" + callType + ">";
                    }
                    else
                    {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<" + callType + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description></" + callType + ">";
                    }
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
        public string DeleteProductJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "DeleteProduct");
            string result = DeleteProduct(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "DeleteProduct");
            return result;
        }
        #endregion

        #region UpdateProduct
        [WebMethod]
        public string UpdateProduct(string callType, string xmlMessage)
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
                    Product h = new Product();
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
            FileLogger.WriteLog(logID + "|End:" + result, 1, "", callType);
            return result;
        }

        [WebMethod]
        public string UpdateProductJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "UpdateProduct");
            string result = UpdateProduct(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "UpdateProduct");
            return result;
        }
        #endregion

        #region GetCompanyList
        [WebMethod]
        public string GetCompanyList(string callType, string xmlMessage)
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
                    Finance f = new Finance();
                    result = f.GetCompanyList(xmlMessage);
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
        public string GetCompanyListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetCompanyList");
            string result = GetCompanyList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetCompanyList");
            return result;
        }
        #endregion

        #region GetCostCenterList
        [WebMethod]
        public string GetCostCenterList(string callType, string xmlMessage)
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
                    Finance f = new Finance();
                    result = f.GetCostCenterList(xmlMessage);
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
        public string GetCostCenterListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetCostCenterList");
            string result = GetCostCenterList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetCostCenterList");
            return result;
        }
        #endregion

        #region GetAccountList
        [WebMethod]
        public string GetAccountList(string callType, string xmlMessage)
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
                    Finance f = new Finance();
                    result = f.GetAccountList(xmlMessage);
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
        public string GetAccountListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetAccountList");
            string result = GetAccountList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetAccountList");
            return result;
        }
        #endregion


        #region GetConceptList
        [WebMethod]
        public string GetConceptList(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    ProductConcept concept = new ProductConcept();
                    result = concept.GetList(xmlMessage);
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
        public string GetConceptListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetConceptList");
            string result = GetConceptList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetConceptList");
            return result;
        }
        #endregion

        #region GetSKUList
        [WebMethod]
        public string GetSKUList(string callType, string xmlMessage)
        {
            string result = "";
            string logID = Guid.NewGuid().ToString();

            try
            {
                FileLogger.WriteLog(logID + "|Start:" + xmlMessage, 1, "", callType);

                if (Helper.CheckAuthCode(callType, xmlMessage))
                {
                    ProductSKU sku = new ProductSKU();
                    result = sku.GetList(xmlMessage);
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
        public string GetSKUListJson(string callType, string JsonMessage)
        {
            string xmlString = iTR.Lib.Common.Json2XML(JsonMessage, "GetSKUList");
            string result = GetSKUList(callType, xmlString);
            result = iTR.Lib.Common.XML2Json(result, "GetSKUList");
            return result;
        }
        #endregion
    }
}
