using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;

namespace ydb.BLL
{
    public class AuthData
    {
        public AuthData()
        {
            
        }

        #region Update
        public string Update(string dataString)
        {
            string id = "", sql = "" ;
            string institutionID = "", institutionType = "", objectID = "", authType = "",comID="";;
            string result = "-1";

            SQLServerHelper runner = new SQLServerHelper();
            try
            {

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateAuthData/FInstitutionID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("医院/控销区划/代理商ID不能为空");
                else
                    institutionID = vNode.InnerText;
                
                vNode = doc.SelectSingleNode("UpdateAuthData/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    comID="-1";
                else
                    comID = vNode.InnerText;
                    
                vNode = doc.SelectSingleNode("UpdateAuthData/FInstitutionType");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("机构类型ID不能为空");
                else
                    institutionType = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateAuthData/FAuthObjectID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("授权对象ID不能为空");
                else
                    objectID = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateAuthData/FAuthType");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("授权类型ID不能为空");
                else
                    authType = vNode.InnerText;
                DataTable dt;

                switch (institutionType)
                {
                    case "1"://医院
                        #region 医院授权

                        if (authType == "1")//产品
                        {

                            sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID='" + objectID + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count == 0)
                            {
                                sql = "Insert into AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values(";
                                sql = sql + "'" + comID + "','" + institutionID + "','" + institutionType + "','" + objectID + "','" + authType + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                runner.ExecuteSqlNone(sql);
                            }
                        }
                        else if (authType == "2")//代理商
                        {
                            sql = "Select FModeID From t_Hospital Where FIsdeleted =0 And FID= '" + institutionID + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count == 0)
                            {
                                throw new Exception("拟授权的医院不存在");
                            }
                            else
                            {
                                if (dt.Rows[0]["FModeID"].ToString().Trim().Equals("c293ddac-52a1-45dc-8f64-1d40b3e2c31a"))//该医院为招商
                                {
                                    //该院是否已授权于其他代理商？
                                    sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID<>'" + objectID + "' and FAuthType='" + authType + "'";
                                    dt = runner.ExecuteSql(sql);
                                    if (dt.Rows.Count > 0)//存在，先取消其授权
                                    {
                                        sql = "Update AuthDatas Set FDeleted=1,FEndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' Where FID='" + dt.Rows[0]["FID"].ToString() + "'";
                                        runner.ExecuteSqlNone(sql);
                                    }
                                    //该医院此类型授权记录是否存在
                                    sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID='" + objectID + "' and FAuthType='" + authType + "'";
                                    dt = runner.ExecuteSql(sql);
                                    if (dt.Rows.Count == 0)
                                    {
                                        sql = "Insert into AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values(";
                                        sql = sql + "'" + comID + "','" + institutionID + "','" + institutionType + "','" + objectID + "','" + authType + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                        runner.ExecuteSqlNone(sql);
                                    }
                                }
                                else
                                    throw new Exception("该医院是直营的，不能授权于代理商");
                            }
                        }
                        else
                        {

                            sql = "Select FModeID From t_Hospital Where FIsdeleted =0 And FID= '" + institutionID + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count == 0)
                            {
                                throw new Exception("拟授权的医院不存在");
                            }
                            else
                            {
                                sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID='" + objectID+"'";
                                dt = runner.ExecuteSql(sql);
                                if (dt.Rows.Count == 0)//不存在该授权，授权
                                {
                                    sql = "Insert into AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values(";
                                    sql = sql + "'" + comID + "','" + institutionID + "','" + institutionType + "','" + objectID + "','" + authType + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                    runner.ExecuteSqlNone(sql);
                                }
                            }
                        }
                        break;
                        #endregion

                    case "2"://控销区域
                        #region 控销区域授权
                        if (authType == "1")//产品
                        {
                            sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID='" + objectID + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count == 0)
                            {
                                sql = "Insert into AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values(";
                                sql = sql + "'" + comID + "','" + institutionID + "','" + institutionType + "','" + objectID + "','" + authType + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                runner.ExecuteSqlNone(sql);
                            }
                        }
                        else if (authType == "4")//控销区域不能授权于医药代码
                        {
                            throw new Exception("控销区域不能授权于医药代表");
                        }
                        else//代理商、招商经理、市场经理
                        {
                            //该院是否已授权于其他代理商？
                            sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID<>'" + objectID + "' and FAuthType='" + authType + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count > 0)//存在，先取消其授权
                            {
                                sql = "Update AuthDatas Set FDeleted=1,FEndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' Where FID='" + dt.Rows[0]["FID"].ToString() + "'";
                                runner.ExecuteSqlNone(sql);

                            }
                            //该医院此类型授权记录是否存在
                            sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID='" + objectID + "' and FAuthType='" + authType + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count == 0)
                            {
                                sql = "Insert into AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values(";
                                sql = sql + "'" + comID + "','" + institutionID + "','" + institutionType + "','" + objectID + "','" + authType + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                runner.ExecuteSqlNone(sql);
                            }
                        }
                        break;
                        #endregion

                    case "3"://代理商
                        #region 代理商授权
                        if (authType == "1")//产品
                        {
                            throw new Exception("产品不能直接授权于代理商");
                        }
                        else if (authType == "4")//控销区域不能授权于医药代码
                        {
                            throw new Exception("代理商不能授权于医药代表");
                        }
                        else if (authType == "2")
                        {
                            throw new Exception("代理商不能授权于别的代理商");
                        }
                        else//招商经理、市场经理
                        {
                            //该院是否已授权于其他代理商？
                            sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID<>'" + objectID + "' and FAuthType='" + authType + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count > 0)//存在，先取消其授权
                            {
                                sql = "Update AuthDatas Set FDeleted=1,FEndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' Where FID='" + dt.Rows[0]["FID"].ToString() + "'";
                                runner.ExecuteSqlNone(sql);

                            }
                            //该医院此类型授权记录是否存在
                            sql = "Select FID from AuthDatas Where FInstitutionID='" + institutionID + "' and FDeleted=0 and FAuthObjectID='" + objectID + "' and FAuthType='" + authType + "'";
                            dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count == 0)
                            {
                                sql = "Insert into AuthDatas(FCompanyID,FInstitutionID,FInstitutionType,FAuthObjectID,FAuthType,FBeginDate) Values(";
                                sql = sql + "'" + comID + "','" + institutionID + "','" + institutionType + "','" + objectID + "','" + authType + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                                runner.ExecuteSqlNone(sql);
                            }

                        }
                        break;
                        #endregion

                }
            }
            catch (Exception err)
            {
                id = "-1";//失败
                throw err;
            }
            result = id;

            return result;
        }
        
        #endregion  

        #region GetList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="getOption:Employee,人员（药代/招商/市场）Product：产品></param>
        /// <returns></returns>
        public string GetList(string xmlString,string getOption ="Employee")
        {
            string result = "", sql = "", filter = " t1.FDeleted=0 ", val = "";
            string institutionType = "1";

            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                xmlString= xmlString.Replace("GetAuthDataList>","GetList>");
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetList/InstitutionID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    
                    if (val.Length > 0 && val!="-1")
                        filter = filter.Length > 0 ? filter = filter + " And t1.FInstitutionID='" + val + "'" : "t1.FInstitutionID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetList/AuthObjectID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FAuthObjectID='" + val + "'" : "t1.FAuthObjectID='" + val + "'";
                }

                
                switch(getOption)
                {  
                    case "Employee":
                        filter = filter.Length > 0 ? filter = filter + " And t1.FAuthType in('3','4','5','6')" : "t1.FAuthType in('3','4','5','6')";
                        break;
                    case "Product":
                        filter = filter.Length > 0 ? filter = filter + " And t1.FAuthType in('1')" : "t1.FAuthType in('1')";
                        break;
                    default:
                        vNode = doc.SelectSingleNode("GetList/AuthType");
                        if (vNode != null)
                        {
                            val = vNode.InnerText.Trim();
                            if (val.Length > 0)
                                filter = filter.Length > 0 ? filter = filter + " And t1.FAuthType in('" + val + "')" : "t1.FAuthType In('" + val + ")'";
                        }
                                    if (val.Length > 0)
                            filter = filter.Length > 0 ? filter = filter + " And t1.FAuthType in('" + val + "')" : "t1.FAuthType In('" + val + ")'";
                        break;
                           
                    }

                    
                   

                vNode = doc.SelectSingleNode("GetList/InstitutionType");

                if (vNode != null)
                {
                    
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        institutionType = val;

                        filter = filter.Length > 0 ? filter = filter + " And t1.FInstitutionType='" + institutionType + "'" : "t1.FInstitutionType='" + institutionType + "'";
                    }
                }

                #region 读取Hospital_Auth_Data 记录 废代码
//                if (institutionType == "1")
//                {

//                    sql = @"Select t1.field0005 AS FAuthObjectID,t2.FName As FAuthObjectName,t3.FID AS FInstitutionID,t3.FName AS FInstitutionName,
//                        t3.FID AS ID
//                        from Hospital_Auth_Data t1 
//                        Left Join t_Items t2 On t1.field0005 = t2.FID
//                        Left Join t_Items t3 On t1.field0001=t3.FNumber
//                        Where t1.field0005='{0}'
//
//                        Union
//
//                        Select t1.field0006 AS FAuthObjectID,t2.FName As FAuthObjectName,t3.FID AS FInstitutionID,t3.FName AS FInstitutionName,
//                        t3.FID AS ID
//                        from Hospital_Auth_Data t1 
//                        Left Join t_Items t2 On t1.field0006 = t2.FID
//                        Left Join t_Items t3 On t1.field0001=t3.FNumber
//                        Where t1.field0006='{0}'
//
//                        Union
//
//                        Select t1.field0007 AS FAuthObjectID,t2.FName As FAuthObjectName,t3.FID AS FInstitutionID,t3.FName AS FInstitutionName,
//                        t3.FID AS ID
//                        from Hospital_Auth_Data t1 
//                        Left Join t_Items t2 On t1.field0007 = t2.FID
//                        Left Join t_Items t3 On t1.field0001=t3.FNumber
//                        Where t1.field0007='{0}'";
//                    sql = string.Format(sql, objectid);
//                }
//                else
//                {
//                    sql = " Select t1.*,Isnull(t2.FName,'') As FInstitutionName,Isnull(t3.FName,'') As FAuthObjectName" +
//                            " From AuthDatas t1" +
//                            " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
//                            " Left Join t_Items t3 On t1.FAuthObjectID= t3.FID";

//                    if (filter.Length > 0)
//                        sql = sql + " Where " + filter;
//                }
                #endregion

                sql = " Select t1.*,Isnull(t2.FName,'') As FInstitutionName,Isnull(t3.FName,'') As FAuthObjectName" +
                            " From AuthDatas t1" +
                            " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
                            " Left Join t_Items t3 On t1.FAuthObjectID= t3.FID";

                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetAuthDataList", "", "List");
                
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetProductListByHospitalID
        public string GetProductListByHospitalID(string xmlString)
        {
            string result = "";

            try
            {
                XmlDocument doc = new XmlDocument();
                xmlString = xmlString.Replace("HospitalID>", "InstitutionID>");
                doc.LoadXml(xmlString);
                //XmlNode pNode = doc.SelectSingleNode("GetAuthDataList");
                //XmlNode cNode = doc.CreateElement("AuthType");
                //cNode.InnerText = "1";
                //pNode.AppendChild(cNode);

                //xmlString = doc.OuterXml ;

                XmlNode vNode = doc.SelectSingleNode("GetAuthDataList/InstitutionID");
                if (vNode == null || vNode.InnerText.Trim().Length ==0)
                {
                    throw new Exception("医院ID不能为空");
                }
                result = GetList(xmlString,"Product");
                result = result.Replace("FInstitutionID>", "FHospitalID>");
                result = result.Replace("FInstitutionName>", "FHospitalName>");
                result = result.Replace("FAuthObjectID>", "FProductID>");
                result = result.Replace("FAuthObjectName>", "FProductName>");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region UpdateHospitalProducts
        public string UpdateHospitalProducts(string xmlString)
        {
            string result = "", hospitalID = "", xmlData = "", comID="",id="-1";
            string[] AdddProducts, DeleteProducts;

            try
            {
                XmlDocument doc = new XmlDocument();
             
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("UpdateHospitalProducts/HospitalID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("医院ID不能为空");
                }
                else
                {
                    hospitalID = vNode.InnerText.Trim();
                }
                vNode = doc.SelectSingleNode("UpdateHospitalProducts/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    comID = "-1";
                else
                    comID = vNode.InnerText;
                vNode = doc.SelectSingleNode("UpdateHospitalProducts/AddProducIDList");//ProductId;分号隔开
                if (vNode != null)
                {
                    AdddProducts = vNode.InnerText.Trim().Split('|');
                    if(AdddProducts.Length >0)//新增的产品
                    {
                        for(int i=0;i<AdddProducts.Length;i++)
                        {
                            if (AdddProducts[i].Trim().Length >0)
                            {
                                xmlData = "<UpdateAuthData>" +
                                            "<FCompanyID>"+ comID +"</FCompanyID>" +
                                            "<FInstitutionType>1</FInstitutionType>" +
                                            "<FAuthObjectID>" + AdddProducts[i] + "</FAuthObjectID>" +
                                            "<FAuthType>1</FAuthType>" +
                                            "<FInstitutionID>" + hospitalID + "</FInstitutionID>" +
                                            "</UpdateAuthData>";
                                Update(xmlData);
                            }
                        }
                    }
                }
                vNode = doc.SelectSingleNode("UpdateHospitalProducts/DeleteProducIDList");//ProductId;分号隔开
                if (vNode != null)
                {
                    DeleteProducts = vNode.InnerText.Trim().Split('|');
                    if (DeleteProducts.Length > 0)//删除的产品
                    {
                        string sql = "";
                        for (int i = 0; i < DeleteProducts.Length; i++)
                        {
                            sql =sql+ "   Update AuthDatas Set FDeleted=1,FEndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' Where FInstitutionID='" + hospitalID + "' And FAuthObjectID='" + DeleteProducts[i]  + "' And FAuthType=1";
                        }
                        SQLServerHelper runner = new SQLServerHelper();
                        runner.ExecuteSqlNone(sql);
                    }
                }
                id = hospitalID;

            }
            catch (Exception err)
            {
                id = "-1";
               throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region UpdateRegionProducts
        public string UpdateRegionProducts(string xmlString)
        {
            string result = "", regionID = "", xmlData = "", comID = "", id = "-1";
            string[] AdddProducts, DeleteProducts;

            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("UpdateRegionProducts/RegionID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("控销区域ID不能为空");
                }
                else
                {
                    regionID = vNode.InnerText.Trim();
                }

                vNode = doc.SelectSingleNode("UpdateRegionProducts/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    comID = "-1";
                else
                    comID = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateRegionProducts/AddProducIDList");//ProductId;分号隔开
                if (vNode != null)
                {
                    AdddProducts = vNode.InnerText.Trim().Split('|');
                    if (AdddProducts.Length > 0)//新增的产品
                    {
                        for (int i = 0; i < AdddProducts.Length; i++)
                        {
                            if (AdddProducts[i].Trim().Length > 0)
                            {
                                xmlData = "<UpdateAuthData>" +
                                            "<FCompanyID>" + comID + "</FCompanyID>" +
                                            "<FInstitutionType>2</FInstitutionType>" +
                                            "<FAuthObjectID>" + AdddProducts[i] + "</FAuthObjectID>" +
                                            "<FAuthType>1</FAuthType>" +
                                            "<FInstitutionID>" + regionID + "</FInstitutionID>" +
                                            "</UpdateAuthData>";
                                Update(xmlData);
                            }
                        }
                    }
                }
                vNode = doc.SelectSingleNode("UpdateRegionProducts/DeleteProducIDList");//ProductId;分号隔开
                if (vNode != null)
                {
                    DeleteProducts = vNode.InnerText.Trim().Split('|');
                    if (DeleteProducts.Length > 0)//删除的产品
                    {
                        string sql = "";
                        for (int i = 0; i < DeleteProducts.Length; i++)
                        {
                            sql = sql + "   Update AuthDatas Set FDeleted=1,FEndDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' Where FInstitutionID='" + regionID + "' And FAuthObjectID='" + DeleteProducts[i] + "' And FAuthType=1";
                        }
                        SQLServerHelper runner = new SQLServerHelper();
                        runner.ExecuteSqlNone(sql);
                    }
                }
                id = regionID;

            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region AppendFromRegistrion
        //将注册申请表的授权信息添加到经营授权表
        public bool AppendFromRegistrion(string applicationID, string employeeId, string agentId)
        {
            bool result = false;
            //FModeID（代理）:08bcbe1b-72ea-4a64-881a-0f08bd44d9e2；FProductCategoryID（产品大类ID）：ab867182-94c6-4385-82e7-decbefbeb105
            try
            {
                string sql = " Insert Into AuthData(FInstitutionID,FInstitutionType,FEmployeeID,FSKUID,FAgentID,FModeID)"+
                      "Select (Case FProductCategoryID When 'ab867182-94c6-4385-82e7-decbefbeb105' Then FHospitalID Else FCountryID End ) As FInstitutionID,"+
                      "(Case FProductCategoryID When 'ab867182-94c6-4385-82e7-decbefbeb105' Then 1 Else 2 End ) As FInstitutionType,"+
                      "'" + employeeId + "', FProductID,'" + agentId + "','08bcbe1b-72ea-4a64-881a-0f08bd44d9e2'" +
                      "From Reg_Application Where FID='" + applicationID + "' And FDeleted=0";
                
                SQLServerHelper runner = new SQLServerHelper();
                if (runner.ExecuteSqlNone(sql) > 0)
                    result = true;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion  

        #region UpdateHospitalOwners
        public string UpdateHospitalOwners(string xmlString)
        {
            string result = "", hospitalID = "", xmlData = "", comID = "", id = "-1";
            string agencyID = "", developerID = "", mktManagerID = "", representiveID = "", managerID = "";
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("UpdateHospitalOwners/HospitalID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("医院ID不能为空");
                }
                else
                {
                    hospitalID = vNode.InnerText.Trim();
                }
                vNode = doc.SelectSingleNode("UpdateHospitalOwners/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    comID = "-1";
                else
                    comID = vNode.InnerText;
                vNode = doc.SelectSingleNode("UpdateHospitalOwners/DevelopeManagerID");
                if (vNode != null)
                {
                    if(vNode.InnerText.Trim().Length >0)//设置招商经理
                    { 
                        developerID=vNode.InnerText.Trim();
                            xmlData = "<UpdateAuthData>" +
                                        "<FCompanyID>" + comID + "</FCompanyID>" +
                                        "<FInstitutionType>1</FInstitutionType>" +
                                        "<FAuthObjectID>" + developerID + "</FAuthObjectID>" +
                                        "<FAuthType>3</FAuthType>" +
                                        "<FInstitutionID>" + hospitalID + "</FInstitutionID>" +
                                        "</UpdateAuthData>";
                            Update(xmlData);
                    }
                }

                vNode = doc.SelectSingleNode("UpdateHospitalOwners/RepresentativeID");
                if (vNode != null)
                {
                    representiveID=vNode.InnerText.Trim();
                    if (vNode.InnerText.Trim().Length > 0)//医药代表
                    {

                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>1</FInstitutionType>" +
                                    "<FAuthObjectID>" + representiveID + "</FAuthObjectID>" +
                                    "<FAuthType>4</FAuthType>" +
                                    "<FInstitutionID>" + hospitalID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }
                vNode = doc.SelectSingleNode("UpdateHospitalOwners/MarketingManagerID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//市场经理
                    {
                        mktManagerID=vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>1</FInstitutionType>" +
                                    "<FAuthObjectID>" + mktManagerID + "</FAuthObjectID>" +
                                    "<FAuthType>5</FAuthType>" +
                                    "<FInstitutionID>" + hospitalID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }
                vNode = doc.SelectSingleNode("UpdateHospitalOwners/AgencyID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//代理商
                    {
                        agencyID=vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>1</FInstitutionType>" +
                                    "<FAuthObjectID>" +agencyID  + "</FAuthObjectID>" +
                                    "<FAuthType>2</FAuthType>" +
                                    "<FInstitutionID>" + hospitalID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }
                vNode = doc.SelectSingleNode("UpdateHospitalOwners/ManagerID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//
                    {
                        managerID = vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>1</FInstitutionType>" +
                                    "<FAuthObjectID>" + managerID + "</FAuthObjectID>" +
                                    "<FAuthType>6</FAuthType>" +
                                    "<FInstitutionID>" + hospitalID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }

                #region 暂时用不上的代码 维护AuthData_Hospital
                //string sql = "Select FID from AuthData_Hospital Where FHospitalID ='" + hospitalID + "'";
                //SQLServerHelper runner = new SQLServerHelper();
                //DataTable dt = runner.ExecuteSql(sql);
                //if(dt.Rows.Count ==0)//已存在该医院的授权记录
                //{
                //    sql = "Insert Into AuthData_Hospital(FHospitalID,FAgencyID,FDeveloperID,FRepresentativeID,FMarketManagerID) Values('";
                //    sql = sql + hospitalID + "','" + agencyID + "','" + developerID + "','" + representiveID + "','" + mktManagerID + "')";
                //}
                //else
                //{
                //    sql = "Update AuthData_Hospital Set FAgencyID='" + agencyID + "',FDeveloperID='" + developerID + "',FRepresentativeID='" + representiveID + "',FMarketManagerID='" + mktManagerID + "' Where FHospitalID='" + hospitalID + "'";
                //}
                //runner.ExecuteSqlNone(sql);
                #endregion

                id = hospitalID;
                
            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region UpdateRegionOwners
        public string UpdateRegionOwners(string xmlString)
        {
            string result = "", regionID = "", xmlData = "", comID = "", id = "-1",sql;
            string agencyID = "", developerID = "", mktManagerID = "";
            DataTable dt;
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("UpdateRegionOwners/RegionID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("控销区域ID不能为空");
                }
                else
                    regionID = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("UpdateRegionOwners/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    comID = "-1";
                else
                    comID = vNode.InnerText;
                vNode = doc.SelectSingleNode("UpdateRegionOwners/DevelopeManagerID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//设置招商经理
                    {
                        developerID = vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>2</FInstitutionType>" +
                                    "<FAuthObjectID>" + developerID + "</FAuthObjectID>" +
                                    "<FAuthType>3</FAuthType>" +
                                    "<FInstitutionID>" + regionID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }


                vNode = doc.SelectSingleNode("UpdateRegionOwners/MarketingManagerID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//市场经理
                    {
                        mktManagerID = vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>2</FInstitutionType>" +
                                    "<FAuthObjectID>" + mktManagerID + "</FAuthObjectID>" +
                                    "<FAuthType>5</FAuthType>" +
                                    "<FInstitutionID>" + regionID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }
                vNode = doc.SelectSingleNode("UpdateRegionOwners/AgencyID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//代理商
                    {
                        agencyID = vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>2</FInstitutionType>" +
                                    "<FAuthObjectID>" + agencyID + "</FAuthObjectID>" +
                                    "<FAuthType>2</FAuthType>" +
                                    "<FInstitutionID>" + regionID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }

                sql = "Select FID from AuthData_Region Where FID ='" + regionID + "'";
                
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count == 0)//已存在该医院的授权记录
                {
                    sql = "Insert Into AuthData_Region(FID,FAgencyID,FDeveloperID,FMarketManagerID) Values('";
                    sql = sql + regionID + "','" + agencyID + "','" + developerID  + "','" + mktManagerID + "')";
                }
                else
                {
                    sql = "Update AuthData_Region Set FAgencyID='" + agencyID + "',FDeveloperID='" + developerID + "',FMarketManagerID='" + mktManagerID + "',FDate='"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"' Where FID='" + regionID + "'";
                }
                runner.ExecuteSqlNone(sql);

                id = regionID;

            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region UpdateAgencyOwners
        public string UpdateAgencyOwners(string xmlString)
        {
            string result = "", agencyID = "", xmlData = "", comID = "", id = "-1";
            string developerID = "", mktManagerID = "", sql;

            DataTable dt;
            try
            {
                XmlDocument doc = new XmlDocument();
                SQLServerHelper runner = new SQLServerHelper();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("UpdateAgencyOwners/AgencyID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("代理商ID不能为空");
                }
                else
                {
                    agencyID = vNode.InnerText.Trim();
                }

                vNode = doc.SelectSingleNode("UpdateAgencyOwners/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    comID = "-1";
                else
                    comID = vNode.InnerText;
                vNode = doc.SelectSingleNode("UpdateAgencyOwners/DevelopeManagerID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//设置招商经理
                    {
                        developerID= vNode.InnerText.Trim() ;
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>3</FInstitutionType>" +
                                    "<FAuthObjectID>" + developerID + "</FAuthObjectID>" +
                                    "<FAuthType>3</FAuthType>" +
                                    "<FInstitutionID>" + agencyID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }

                vNode = doc.SelectSingleNode("UpdateAgencyOwners/MarketingManagerID");
                if (vNode != null)
                {
                    if (vNode.InnerText.Trim().Length > 0)//市场经理
                    {
                        mktManagerID = vNode.InnerText.Trim();
                        xmlData = "<UpdateAuthData>" +
                                    "<FCompanyID>" + comID + "</FCompanyID>" +
                                    "<FInstitutionType>2</FInstitutionType>" +
                                    "<FAuthObjectID>" + mktManagerID + "</FAuthObjectID>" +
                                    "<FAuthType>5</FAuthType>" +
                                    "<FInstitutionID>" + agencyID + "</FInstitutionID>" +
                                    "</UpdateAuthData>";
                        Update(xmlData);
                    }
                }
                sql = "Select FID from AuthData_Agency Where FAgencyID ='" + agencyID + "'";
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count == 0)//代理商授权不存在
                {
                    sql = "Insert Into AuthData_Agency(FAgencyID,FDeveloperID,FMarketManagerID) Values('";
                    sql = sql +  agencyID + "','" + developerID + "','" + mktManagerID + "')";
                }
                else
                {
                    sql = "Update AuthData_Agency Set FDeveloperID='" + developerID + "',FMarketManagerID='" + mktManagerID + "' Where FAgencyID='" + agencyID + "'";
                }
                runner.ExecuteSqlNone(sql);

                id = agencyID;

            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region GetMyClientList
        public string GetMyClientList(string xmlString)
        {
            string result = "";

            try
            {
                XmlDocument doc = new XmlDocument();
                xmlString = xmlString.Replace("EmployeeID>", "AuthObjectID>");
                xmlString = xmlString.Replace("ClientType>", "InstitutionType>");
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetAuthDataList/AuthObjectID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("员工ID不能为空");
                }
                result = GetList(xmlString);
                result = result.Replace("FInstitutionID>", "ClientID>");
                result = result.Replace("FInstitutionName>", "ClientName>");
                result = result.Replace("FAuthObjectID>", "EmployeeID>");
                result = result.Replace("FAuthObjectName>", "EmployeeName>");
                result = result.Replace("FInstitutionType>", "ClientType>");

                doc.LoadXml(result);
               
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDetailByHospitalID
        public string GetDetailByHospitalID(string xmlString)
        {
            string result = "",sql="",hospitalID="";
            DataTable dt;
            result = "<GetDetailByHospitalID>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<ProductList></ProductList>" +
                    "<HospitalID></HospitalID>" +
                    "<HospitalName></HospitalName>" +
                    "<ModeID></ModeID>" +
                    "<ModeName></ModeName>" +
                    "<AgencyID></AgencyID>" +
                    "<AgencyName></AgencyName>" +
                    "<DevelopeManagerID></DevelopeManagerID>" +
                    "<DevelopeManagerName></DevelopeManagerName>" +
                    "<RepresentativeID></RepresentativeID>" +
                    "<RepresentativeName></RepresentativeName>" +
                    "<MarketingManagerID></MarketingManagerID>" +
                    "<MarketingManagerName></MarketingManagerName>" +
                    "</GetDetailByHospitalID>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetDetailByHospitalID/HospitalID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("医院ID不能为空");
                else
                {
                    hospitalID = vNode.InnerText.Trim();
                    sql = "Select t1.*,IsNull(t2.FName,'') As FHospitalName,IsNull(t3.FName,'') As FModeName,t5.FAgencyID,t5.FDeveloperID,t5.FRepresentativeID,t5.FMarketManagerID," +
                           " IsNull(t6.FName,'') As FAgencyName,IsNull(t7.FName,'') As FDeveloperName,IsNull(t8.FName,'') As FRepresentativeName,IsNull(t9.FName,'') As MarketManagerName"+
                           " From  t_Hospital t1 "+
                           " Left Join t_Items t2 On t1.FID= t2.FID"+
                           " Left Join t_Items t3 On t1.FModeID= t3.FID"+
                           " Left Join AuthData_Hospital t5 On t1.FID= t5.FHospitalID"+
                           " Left Join t_Items t6 On t5.FAgencyID= t6.FID"+
                           " Left Join t_Items t7 On t5.FDeveloperID= t7.FID"+
                           " Left Join t_Items t8 On t5.FRepresentativeID= t8.FID"+
                           " Left Join t_Items t9 On t5.FMarketManagerID= t9.FID"+
                           " Where t1.FID='" + hospitalID + "' and t1.FDeleted=0";

                    dt = runner.ExecuteSql(sql);
                    doc = new XmlDocument();
                    doc.LoadXml(result);
                    if (dt.Rows.Count > 0)//存在此医院
                    {
                        doc.SelectSingleNode("GetDetailByHospitalID/Result").InnerText = "True";
                        doc.SelectSingleNode("GetDetailByHospitalID/HospitalName").InnerText = dt.Rows[0]["FHospitalName"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/HospitalID").InnerText = dt.Rows[0]["FID"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/ModeName").InnerText = dt.Rows[0]["FModeName"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/ModeID").InnerText = dt.Rows[0]["FModeID"].ToString();

                        doc.SelectSingleNode("GetDetailByHospitalID/AgencyID").InnerText = dt.Rows[0]["FAgencyID"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/AgencyName").InnerText = dt.Rows[0]["FAgencyName"].ToString();

                        doc.SelectSingleNode("GetDetailByHospitalID/DevelopeManagerID").InnerText = dt.Rows[0]["FDeveloperID"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/DevelopeManagerName").InnerText = dt.Rows[0]["FDeveloperName"].ToString();

                        doc.SelectSingleNode("GetDetailByHospitalID/MarketingManagerID").InnerText = dt.Rows[0]["FMarketManagerID"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/MarketingManagerName").InnerText = dt.Rows[0]["MarketManagerName"].ToString();

                        doc.SelectSingleNode("GetDetailByHospitalID/RepresentativeID").InnerText = dt.Rows[0]["FRepresentativeID"].ToString();
                        doc.SelectSingleNode("GetDetailByHospitalID/RepresentativeName").InnerText = dt.Rows[0]["FRepresentativeName"].ToString();
                    }
                    else
                        throw new Exception("医院不能存在");
                }
                sql = "Select t1.*,Isnull(t2.FName,'') As FInstitutionName,Isnull(t3.FName,'') As FAuthObjectName" +
                     " From AuthDatas t1" +
                     " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
                     " Left Join t_Items t3 On t1.FAuthObjectID= t3.FID";

                sql = sql + " Where t1.FAuthType=1 And t1.FDeleted=0 and t1.FInstitutionID='" + hospitalID + "'";
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)//产品
                {
                    doc.SelectSingleNode("GetDetailByHospitalID/Result").InnerText ="True";
                    XmlNode pNode = doc.SelectSingleNode("GetDetailByHospitalID/ProductList");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("Product");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FAuthObjectID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("Name");
                        vNode.InnerText = row["FAuthObjectName"].ToString();
                        cNode.AppendChild(vNode);
                    }
                }
               
                result = doc.OuterXml;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDetailByAgencyID
        public string GetDetailByAgencyID(string xmlString)
        {
            string result = "", sql = "", agencyID = "";
            result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetDetailByAgencyID>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<AgencyID></AgencyID>" +
                    "<AgencyName></AgencyName>" +
                    "<ProductList></ProductList>" +
                    "<HospitalList></HospitalList>" +
                    "<DevelopeManagerID></DevelopeManagerID>" +
                    "<DevelopeManagerName></DevelopeManagerName>" +
                    "<MarketingManagerID></MarketingManagerID>" +
                    "<MarketingManagerName></MarketingManagerName>" +
                    "</GetDetailByAgencyID>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetDetailByAgencyID/AgencyID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("代理商ID不能为空");
                else
                    agencyID = vNode.InnerText.Trim();

                sql = "Select t1.*,Isnull(t2.FName,'') As FInstitutionName,Isnull(t3.FName,'') As FAuthObjectName" +
                     " From AuthDatas t1" +
                     " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
                     " Left Join t_Items t3 On t1.FAuthObjectID= t3.FID";
                sql = sql + " Where t1.FAuthType=1 And t1.FDeleted=0 and t1.FInstitutionID In (Select FInstitutionID From AuthDatas Where FDeleted=0 and FAuthType =2 and FAuthObjectID='" + agencyID + "')";
                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);

                doc.LoadXml(result);
                if (dt.Rows.Count > 0)//产品
                {
                    doc.SelectSingleNode("GetDetailByAgencyID/Result").InnerText = "True";
                    XmlNode pNode = doc.SelectSingleNode("GetDetailByAgencyID/ProductList");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("Product");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FAuthObjectID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("Name");
                        vNode.InnerText = row["FAuthObjectName"].ToString();
                        cNode.AppendChild(vNode);
                    }
                }
                //医院
                sql = "Select t1.*,Isnull(t2.FName,'') As FInstitutionName,Isnull(t3.FName,'') As FAuthObjectName" +
                     " From AuthDatas t1" +
                     " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
                     " Left Join t_Items t3 On t1.FAuthObjectID= t3.FID";
                sql = sql + " Where t1.FAuthType=2 And t1.FDeleted=0 and t1.FAuthObjectID='" + agencyID + "'";
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    doc.SelectSingleNode("GetDetailByAgencyID/Result").InnerText = "True";
                    XmlNode pNode = doc.SelectSingleNode("GetDetailByAgencyID/HospitalList");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("Hospital");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FAuthObjectID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("Name");
                        vNode.InnerText = row["FAuthObjectName"].ToString();
                        cNode.AppendChild(vNode);
                    }
                }

                //招商经理、市场经理
                sql = "Select t1.* ,IsNull(t3.FName,'') As FDevelopeName,IsNull(t2.FName,'') As FAgencyName,IsNull(t4.FName,'') As FMarketManagerName"+
                      " From AuthData_Agency t1"+
                      "  Left Join t_Items t2 On t1.FAgencyID= t2.FID"+
                      "  Left Join t_Items t3 On t3.FID= t1.FDeveloperID"+
                      "  Left Join t_Items t4 On t4.FID= t1.FMarketManagerID"+
                      "  Where  t1.FAgencyID='" + agencyID + "'";

                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {   
                    doc.SelectSingleNode("GetDetailByAgencyID/Result").InnerText = "True";
                    doc.SelectSingleNode("GetDetailByAgencyID/AgencyID").InnerText = dt.Rows[0]["FAgencyID"].ToString();
                    doc.SelectSingleNode("GetDetailByAgencyID/AgencyName").InnerText = dt.Rows[0]["FAgencyName"].ToString();

                    doc.SelectSingleNode("GetDetailByAgencyID/DevelopeManagerID").InnerText = dt.Rows[0]["FDeveloperID"].ToString();
                    doc.SelectSingleNode("GetDetailByAgencyID/DevelopeManagerName").InnerText = dt.Rows[0]["FDevelopeName"].ToString();

                    doc.SelectSingleNode("GetDetailByAgencyID/MarketingManagerID").InnerText = dt.Rows[0]["FMarketManagerID"].ToString();
                    doc.SelectSingleNode("GetDetailByAgencyID/MarketingManagerName").InnerText = dt.Rows[0]["FMarketManagerName"].ToString();
                }

                result = doc.OuterXml;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDetailByRegionID
        public string GetDetailByRegionID(string xmlString)
        {
            string result = "", sql = "", regionID = "";
            result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetDetailByRegionID>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<ProductList></ProductList>" +
                    "<AgencyID></AgencyID>" +
                    "<AgencyName></AgencyName>" +
                    "<DevelopeManagerID></DevelopeManagerID>" +
                    "<DevelopeManagerName></DevelopeManagerName>" +
                    "<MarketingManagerID></MarketingManagerID>" +
                    "<MarketingManagerName></MarketingManagerName>" +
                    "</GetDetailByRegionID>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetDetailByRegionID/RegionID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("控销区域ID不能为空");
                else
                    regionID = vNode.InnerText.Trim();

                sql = "Select t1.*,t2.FRegionName,Isnull(t3.FName,'') As FAuthObjectName" +
                     " From AuthDatas t1" +
                     " Left Join AuthData_Region t2 On t1.FInstitutionID= t2.FID" +
                     " Left Join t_Items t3 On t1.FAuthObjectID= t3.FID";
                sql = sql + " Where t1.FAuthType=1 And t1.FDeleted=0 and t1.FInstitutionID='" + regionID + "'";
                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                doc.LoadXml(result);
                if (dt.Rows.Count > 0)//产品
                {
                    doc.SelectSingleNode("GetDetailByRegionID/Result").InnerText = "True";
                    XmlNode pNode = doc.SelectSingleNode("GetDetailByRegionID/ProductList");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("Product");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FAuthObjectID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("Name");
                        vNode.InnerText = row["FAuthObjectName"].ToString();
                        cNode.AppendChild(vNode);
                    }
                }

                sql = "Select t1.* ,IsNull(t3.FName,'') As FDevelopeName,IsNull(t2.FName,'') As FAgencyName,IsNull(t4.FName,'') As FMarketManagerName" +
                      "  From AuthData_Region t1" +
                      "  Left Join t_Items t2 On t2.FID=t1.FAgencyID" +
                      "  Left Join t_Items t3 On t3.FID= t1.FDeveloperID" +
                      "  Left Join t_Items t4 On t4.FID= t1.FMarketManagerID";
                sql = sql + " Where t1.FID ='" + regionID + "'"; 
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    doc.SelectSingleNode("GetDetailByRegionID/Result").InnerText = "True";
                    doc.SelectSingleNode("GetDetailByRegionID/AgencyID").InnerText = dt.Rows[0]["FAgencyID"].ToString();
                    doc.SelectSingleNode("GetDetailByRegionID/AgencyName").InnerText = dt.Rows[0]["FAgencyName"].ToString();

                    doc.SelectSingleNode("GetDetailByRegionID/DevelopeManagerID").InnerText = dt.Rows[0]["FDeveloperID"].ToString();
                    doc.SelectSingleNode("GetDetailByRegionID/DevelopeManagerName").InnerText = dt.Rows[0]["FDevelopeName"].ToString();

                    doc.SelectSingleNode("GetDetailByRegionID/MarketingManagerID").InnerText = dt.Rows[0]["FMarketManagerID"].ToString();
                    doc.SelectSingleNode("GetDetailByRegionID/MarketingManagerName").InnerText = dt.Rows[0]["FMarketManagerName"].ToString();
                }

               
                result = doc.OuterXml;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetAuthHospitalList
        public string GetAuthHospitalList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FDeleted=0 ", val = "",hospitalIDs="";
            SQLServerHelper runner;
             DataTable dt;
            try
            {
                runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetAuthHospitalList/ModeID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FModeID='" + val + "'" : "t1.FModeID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/HospitalName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/ProductName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        sql="Select distinct t1.FInstitutionID "+
                            " From AuthDatas t1"+
                            " Left Join t_Items t2 On t2.FID= t1.FAuthObjectID"+
                            " Where t1.FAuthType=1 And t2.FName like '%" + val + "%'";
                        dt = runner.ExecuteSql(sql);
                        foreach(DataRow row in dt.Rows)
                        {
                            hospitalIDs = hospitalIDs + "'" + row["FInstitutionID"].ToString() + "',";
                        }
                        if (hospitalIDs.Length > 0)
                        {
                            hospitalIDs = hospitalIDs.Substring(0, hospitalIDs.Length - 1);
                            filter = filter.Length > 0 ? filter = filter + " And t1.FID In(" + hospitalIDs + ")" : "t1.FID In(" + hospitalIDs + ")";
                        }
                        else
                            filter = filter.Length > 0 ? filter = filter + " And t1.FID In('-999')" : "t1.FID In('-999')";
                    }
                }


                vNode = doc.SelectSingleNode("GetAuthHospitalList/DeveloperManager");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t7.FName like '%" + val + "%'" : "t7.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetAuthHospitalList/Representative");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t8.FName like '%" + val + "%'" : "t8.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/MarketingManager");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t9.FName like '%" + val + "%'" : "t9.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetAuthHospitalList/AgencyName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t6.FName like '%" + val + "%'" : "t6.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/GrandID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FGrandID='" + val + "'" : "t1.FGrandID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/RevenueLevelID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FRevenueLevelID='" + val + "'" : "t1.FRevenueLevelID='" + val + "'";
                }
                vNode = doc.SelectSingleNode("GetAuthHospitalList/ProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FProvinceID='" + val + "'" : "t1.FProvinceID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/CountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCountryID='" + val + "'" : "t1.FCountryID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthHospitalList/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCityID='" + val + "'" : "t1.FCityID='" + val + "'";
                }

                sql = "Select t1.*,IsNull(t2.FName,'') As FHospitalName,IsNull(t3.FName,'') As FModeName,IsNull(t4.FName,'') As FGrandName,"+
                    " IsNull(t6.FName,'') As FAgencyName,IsNull(t7.FName,'') As FDeveloperName,IsNull(t8.FName,'') As FRepresentativeName,IsNull(t9.FName,'') As MarketManagerName"+
                    " From  t_Hospital t1 "+
                    " Left Join t_Items t2 On t1.FID= t2.FID"+
                    " Left Join t_Items t3 On t1.FModeID= t3.FID"+
                    " Left Join t_Items t4 On t1.FGrandID= t4.FID"+
                    " Left Join AuthData_Hospital t5 On t1.FID= t5.FHospitalID"+
                    " Left Join t_Items t6 On t5.FAgencyID= t6.FID"+
                    " Left Join t_Items t7 On t5.FDeveloperID= t7.FID"+
                    " Left Join t_Items t8 On t5.FRepresentativeID= t8.FID"+
                    " Left Join t_Items t9 On t5.FMarketManagerID= t9.FID";

                if (filter.Length > 0)
                    sql = sql + " Where " + filter;


                
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAuthHospitalList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAuthHospitalList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetAuthHospitalList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("HospitalID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCompanyID");
                        vNode.InnerText = row["FCompanyID"].ToString();
                        cNode.AppendChild(vNode);


                        vNode = doc.CreateElement("FHospitalName");
                        vNode.InnerText = row["FHospitalName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FModeName");
                        vNode.InnerText = row["FModeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FGrandName");
                        vNode.InnerText = row["FGrandName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAgencyName");
                        vNode.InnerText = row["FAgencyName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDeveloperName");
                        vNode.InnerText = row["FDeveloperName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRepresentativeName");
                        vNode.InnerText = row["FRepresentativeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("MarketManagerName");
                        vNode.InnerText = row["MarketManagerName"].ToString();
                        cNode.AppendChild(vNode);
                        
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAuthHospitalList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAuthHospitalList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetAuthRegionList
        public string GetAuthRegionList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FDeleted=0 ", val = "", regionIDs = "";

            SQLServerHelper runner;
            DataTable dt;
            try
            {
                runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetAuthRegionList/ProductName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                   
                    if (val.Length > 0)
                    {
                        sql = "Select distinct t1.FInstitutionID"+
                            " From AuthDatas t1"+
                            " Left Join t_Items t2 On t1.FAuthObjectID= t2.FID"+
                            " Where t1.FInstitutionType=2 And t1.FAuthType=1 and  t2.FName like '%" + val + "%'";
                        dt = runner.ExecuteSql(sql);
                        foreach (DataRow row in dt.Rows)
                        {
                            regionIDs = regionIDs + "'" + row["FInstitutionID"].ToString() + "',";
                        }

                        if (regionIDs.Length > 0)
                        {
                            regionIDs = regionIDs.Substring(0, regionIDs.Length - 1);
                            filter = filter.Length > 0 ? filter = filter + " And t1.FID In(" + regionIDs + ")" : " t1.FID In(" + regionIDs + ")";
                        }
                        else
                            filter = filter.Length > 0 ? filter = filter + " And t1.FID In('-999')" : " t1.FID In('-999')";
                    }
                }



                vNode = doc.SelectSingleNode("GetAuthRegionList/DeveloperManager");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t3.FName like '%" + val + "%'" : "t3.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthRegionList/MarketingManager");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t4.FName like '%" + val + "%'" : "t4.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetAuthRegionList/AgencyName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthRegionList/ProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FProvinceID='" + val + "'" : "t1.FProvinceID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthRegionList/CountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCountryID='" + val + "'" : "t1.FCountryID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthRegionList/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCityID='" + val + "'" : "t1.FCityID='" + val + "'";
                }

                sql = "Select t1.*,Isnull(t2.FName,'') As FAgencyName,Isnull(t3.FName,'') As FDeveloperName,IsNull(t4.FName,'') As FMarketManagerName"+
                      "  From AuthData_Region t1"+
                      "  Left Join t_Items t2 On t1.FAgencyID = t2.FID"+
                      "  Left Join t_Items t3 On t1.FDeveloperID = t3.FID"+
                      "  Left Join t_Items t4 On t1.FMarketManagerID = t4.FID";

                if (filter.Length > 0)
                    sql = sql + " Where " + filter;



                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAuthRegionList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAuthRegionList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetAuthRegionList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("FRegionID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCompanyID");
                        vNode.InnerText = row["FCompanyID"].ToString();
                        cNode.AppendChild(vNode);


                        vNode = doc.CreateElement("FRegionName");
                        vNode.InnerText = row["FRegionName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDeveloperName");
                        vNode.InnerText = row["FDeveloperName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FMarketManagerName");
                        vNode.InnerText = row["FMarketManagerName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAgencyName");
                        vNode.InnerText = row["FAgencyName"].ToString();
                        cNode.AppendChild(vNode);

                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAuthHospitalList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAuthHospitalList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetAuthAgencyList
        public string GetAuthAgencyList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FDeleted=0 ", val = "",agencyIds="";
            DataTable dt;
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                runner = new SQLServerHelper();
                XmlNode vNode = doc.SelectSingleNode("GetAuthAgencyList/HospitalName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        sql = "Select t1.FInstitutionID,ISNULL(t2.FName,'') As FHospitalName,t1.FAuthObjectID As FAgencyID" +
                            "From AuthDatas t1" +
                            "Left Join t_Items t2 On t1.FInstitutionID=t2.FID" +
                            " Where t1.FInstitutionType=1 and t1.FAuthType=2  and t2.FName like '%" + val + "%'";
                        dt = runner.ExecuteSql(sql);
                        foreach(DataRow row in dt.Rows)
                        {
                            agencyIds = agencyIds +"'"+ row["FAgencyID"].ToString() + "',";
                        }
                        if(agencyIds.Length > 0)
                        {
                            agencyIds = agencyIds.Substring(0, agencyIds.Length - 1);
                            filter = filter.Length > 0 ? filter = filter + " And t2.FInstitutionID In(" + agencyIds + ")" : " t2.FInstitutionID In(" + agencyIds + ")";
                        }
                        else
                            filter = filter.Length > 0 ? filter = filter + " And t2.FInstitutionID In('-999')" : " t2.FInstitutionID In('-999')";
                    }
                }

                vNode = doc.SelectSingleNode("GetAuthAgencyList/ProductName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        sql = "Select t1.FInstitutionID,ISNULL(t2.FName,'') As FHospitalName,t1.FAuthObjectID As FAgencyID" +
                            "From AuthDatas t1" +
                            "Left Join t_Items t2 On t1.FInstitutionID=t2.FID" +
                            " Where t1.FInstitutionType=1 and t1.FAuthType=2  and t1.FInstitutionID In(Select distinct FInstitutionID From AuthDatas t1 Left Join t_Items t2 On t1.FAuthObjectID= t2.FID Where t2.FName like '%" + val + "%')";
                        dt = runner.ExecuteSql(sql);
                        foreach (DataRow row in dt.Rows)
                        {
                            agencyIds = agencyIds + "'" + row["FAgencyID"].ToString() + "',";
                        }
                        if (agencyIds.Length > 0)
                        {
                            agencyIds = agencyIds.Substring(0, agencyIds.Length - 1);
                            filter = filter.Length > 0 ? filter = filter + " And t1.FID In(" + agencyIds + ")" : " t1.FID In(" + agencyIds + ")";
                        }
                        else
                            filter = filter.Length > 0 ? filter = filter + " And t1.FID In('-999')" : " t1.FID In('-999')";
                    }
                }

                vNode = doc.SelectSingleNode("GetAuthAgencyList/DeveloperManager");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t3.FName like '%" + val + "%'" : "t3.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthAgencyList/MarketingManager");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t4.FName like '%" + val + "%'" : "t4.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetAuthAgencyList/ProvinceID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FProvinceID='" + val + "'" : "t1.FProvinceID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthAgencyList/CountryID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCountryID='" + val + "'" : "t1.FCountryID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetAuthAgencyList/CityID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FCityID='" + val + "'" : "t1.FCityID='" + val + "'";
                }

                sql = "Select t1.* ,IsNull(t3.FName,'') As FDevelopeName,IsNull(t5.FName,'') As FAgencyName,IsNull(t4.FName,'') As FMarketManagerName"+
                      "  From t_Agency t1"+
                      "  Left Join AuthData_Agency t2 On t1.FID=t2.FAgencyID"+
                      "  Left Join t_Items t3 On t3.FID= t2.FDeveloperID"+
                      "  Left Join t_Items t4 On t4.FID= t2.FMarketManagerID"+
                      "  Left Join t_Items t5 On t5.FID= t1.FID";

                if (filter.Length > 0)
                    sql = sql + " Where " + filter;


                
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAuthAgencyList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAuthAgencyList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetAuthAgencyList/DataRows");
                    XmlNode cNode = null;
                    foreach (DataRow row in dt.Rows)
                    {
                        cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FCompanyID");
                        vNode.InnerText = row["FCompanyID"].ToString();
                        cNode.AppendChild(vNode);
                       

                        vNode = doc.CreateElement("DeveloperManagerName");
                        vNode.InnerText = row["FDevelopeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("MarketingManagerName");
                        vNode.InnerText = row["FMarketManagerName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FAgencyName");
                        vNode.InnerText = row["FAgencyName"].ToString();
                        cNode.AppendChild(vNode);       
                       
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAuthAgencyList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAuthAgencyList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion


        #region GetMyProductList
        public string GetMyProductList(string xmlString)
        {
            string val ="";
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetMyProdutList><DataRows>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "</GetMyProdutList>";

            try
            {
                XmlDocument doc = new XmlDocument();
               
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetMyProductList/EmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("员工ID不能为空");
                }
                else
                {
                    val = vNode.InnerText.Trim();
                }

                string sql = "Select Distinct t1.FAuthObjectID As FProductID,Isnull(t2.FName,'') AS  FProductName " +
                            " From AuthDatas t1" +
                            " Left Join t_Items t2 On t1.FAuthObjectID = t2.FID" +
                            " Where t1.FAuthType= '1' and  t1.FInstitutionID In (Select Distinct FInstitutionID from AuthDatas Where FAuthObjectID ='{0}')";
                sql = string.Format(sql, val);
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if(dt.Rows.Count ==0)
                {
                    sql = " Select  FID AS  FProductID, FName As FProductName From t_Items Where FClassID = '36d33d13-4f8b-4ee4-84c5-49ff7c8691c4' and FIsDeleted=0";
                    dt = runner.ExecuteSql(sql);
                }
                result = Common.DataTableToXml(dt, "GetMyProductList", "", "List"); 

            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion
    }

}
