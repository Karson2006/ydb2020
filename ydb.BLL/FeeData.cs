using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;
using System.Configuration;

namespace ydb.BLL
{
    public class FeeData
    {
        public FeeData()
        {

        }

        #region Update

        public string Update(string xmlString)
        {
            string id = "", sql = "", valueString = "", type = "1", detailNodeString = "UpdateExpendData"; ;

            try
            {
                Dictionary<string, string> fieldValues=null;
                List <Dictionary<string, string>> rowsList= new List <Dictionary<string, string>>();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode typeNode = doc.SelectSingleNode("UpdateFeeData/Type");
                if (typeNode == null || typeNode.InnerText.Length == 0)
                    throw new Exception("Type不能为空");
                else
                    type = typeNode.InnerText.Trim();
                if (type == "2")
                {
                    detailNodeString = "UpdateTripData";
                }
                XmlNodeList nodeList = doc.SelectNodes("UpdateFeeData/DataRows/DataRow");
                foreach(XmlNode node in nodeList)//数据检查,且获取到明细数据
                {
                    fieldValues = Common.GetFieldValuesFromXml(node.OuterXml, detailNodeString,"DataRow");
                    rowsList.Add(fieldValues);
                }
                fieldValues = Common.GetFieldValuesFromXml(xmlString, "UpdateFeeData");
                
                SQLServerHelper runner = new SQLServerHelper();

                if (fieldValues["FID"] == "-1" || fieldValues["FID"].Trim().Length ==0)
                {
                    id = Guid.NewGuid().ToString();
                    sql = "Insert Into FeeList(FID) Values('" + id + "')";
                    runner.ExecuteSqlNone(sql);
                }
                else
                {
                    id = fieldValues["FID"];
                }
                
                foreach(string key in fieldValues.Keys)
                {
                    if (key == "FID") continue;
                    valueString = valueString + key + "='" + fieldValues[key] + "',";
                }

                if (valueString.Length > 0)
                    sql = "Update FeeList Set " + valueString.Substring(0, valueString.Length - 1) + " Where  FID ='" + id + "'";

                runner.ExecuteSqlNone(sql);
                //保存明细费用
                if(type=="1")
                {        
                    foreach(Dictionary<string, string> row in rowsList)
                    {
                        sql = "Insert Into ExpendDetail(FFeeID,FAccountID1,FAccountID2,FAmount,FDigest) Values('{0}','{1}','{2}','{3}','{4}')";
                        sql = string.Format(sql, id, row["FAccountID1"], row["FAccountID2"], row["FAmount"], row["FDigest"]);
                        runner.ExecuteSqlNone(sql);
                    }
                }
                else if(type=="2")
                {
                    foreach (Dictionary<string, string> row in rowsList)
                    {
                        sql = "Insert Into TripDetail(FFeeID,FStartDate,FDepartDate,FStartCity,FDepartCity,FFee1,FFee2,FFee3,FFee4) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
                        sql = string.Format(sql, id, row["FStartDate"], row["FDepartDate"], row["FStartCity"], row["FDepartCity"], row["FFee1"], row["FFee2"], row["FFee3"], row["FFee4"]);
                        runner.ExecuteSqlNone(sql);
                    }
                }
            }
            catch(Exception err)
            {
                id = "-1";
                throw err;
            }
            return id;
        }
        #endregion

        #region Delete
        public string Delete(string xmlMessage)
        {
            string result = "-1", id = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlNode vNode = doc.SelectSingleNode("DeleteFeeData/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("FeeID不能为空");
                id = vNode.InnerText.Trim();
                string sql = "Update FeeList Set FDeleted=1  Where FID = '" + id + "'";

                sql = sql + " Update ExpendDetail Set FDeleted=1  Where FFeeID = '" + id + "'";

                sql = sql + " Update TripDetail Set FDeleted=1  Where FFeeID = '" + id + "'";

                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
            if (int.Parse(result) > 0)
                result = id;
            else
                result = "-1";
            return result;
        }
        #endregion

        #region DeleteExpendData
        public string DeleteExpendData(string xmlMessage)
        {
            string result = "-1", id = "",filter ="",feeId="";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlNode vNode = doc.SelectSingleNode("DeleteExpendData/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {}
                else
                {
                    id = vNode.InnerText.Trim();
                    filter = filter.Trim().Length > 0 ? filter = filter + " and FID ='" + id + "'" : filter = " FID ='" + id + "'";
                }
                vNode = doc.SelectSingleNode("DeleteExpendData/FeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {}
                else
                {
                    feeId = vNode.InnerText.Trim();
                    filter = filter.Trim().Length > 0 ? filter = filter + " and FFeeID ='" + feeId + "'" : filter = " FFeeID ='" + feeId + "'";
                }
                if (filter.Trim().Length == 0)
                    throw new Exception("明细费用ID和费用列表ID,不能同时为空");

                string sql = "Update ExpendDetail  Set FDeleted=1  Where " + filter ;
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
            if (int.Parse(result) > 0)
                result = id;
            else
                result = "-1";
            return result;
        }
        #endregion

        #region GetList
        public string GetList(string xmlString)
        {
            string result = "";
            try
            {
                DataTable dt = Query(xmlString, "GetFeeList");
                result = Common.DataTableToXml(dt, "GetFeeList", "", "List");

            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDetail
        //public string GetDetail(string xmlString)
        //{
        //    string result = "";
        //    try
        //    {
        //        DataTable dt = Query(xmlString, "GetMarketingActivityDetail");
        //        result = Common.DataTableToXml(dt, "GetMarketingActivityDetail");
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //    return result;
        //}
        #endregion

        #region Query
        private DataTable  Query(string xmlString,string nodeString)
        {
            string  sql = "", filter = "", val = "";
            DataTable result = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(nodeString+"/ID");

                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/BeginDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim() + " 0:0:0.000";
                    filter = filter.Length > 0 ? filter = filter + " And t1.FDate>'" + val + "'" : "t1.FDate>='" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/EndDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim()+" 23:59:59.999";
                    filter = filter.Length > 0 ? filter = filter + " And t1.FDate<'" + val + "'" : "t1.FDate <= '" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FEmployeeID In ('" + val.Replace("|", "','") + "')" : "t1.FEmployeeID In ('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode(nodeString + "/Type");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FType= '" + val + "'" : "t1.FType= '" + val + "'";
                }
                sql = " Select t1.FID, t1.FCompanyID,t1.FDeptID,t1.FCostCenter,t1.FEmployeeID,t1.FType,t1.FDate,t1.FApproveDate,t1.FAmount,"+
                      " Isnull(t2.FName,'') As FCompanyName,Isnull(t3.FName,'') as FDeptName,Isnull(t4.FName,'') As FEmployeeName,t1.FRemark,"+
                      " (Case t1.FStatus When 0 then '审批中' When '1' Then '已审'  When '-2' Then '未提交'  Else '已取消' End) As FStatus" +
                      "  From FeeList t1"+
                      "  Left Join t_Company t2 On t1.FCompanyID= t2.FID" +
                      "  Left Join t_Items t3 On t1.FDeptID= t3.FID"+
                      "  Left Join t_Items t4 On t1.FEmployeeID = t4.FID"+
                      "  Where t1.FDeleted=0 ";
                if (filter.Length > 0)
                    sql = sql + " and  " + filter;
                sql = sql + "  Order by t1.FDate Desc";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region QueryExpend
        public string GetExpendList(string xmlString)
        {
            string sql = "", feeID="";
            string  result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
            
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetExpendList/FeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("FeeID不能为空");
                else
                    feeID = vNode.InnerText;
                sql = " Select t1.FID,t1.FFeeID,t1.FAccountID1,ISnull(t2.FAccountCaptial1,'') As FAccountCaption1,t1.FAccountID2,IsNull(t2.FAccountCaptial2,'') As FAccountCaption2," +
                      " t1.FAmount,t1.FDigest"+
                      " From ExpendDetail t1"+
                      " Left Join t_Account t2 On t1.FAccountID1 = t2.FAccountID1 and t1.FAccountID2 = t2.FAccountID2" +
                      " Where t1.FDeleted=0 and t1.FFeeID = '{0}' and t1.FDeleted=0";
                sql = string.Format(sql, feeID);
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetExpendList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion


        #region QueryTrip
        public string  GetTripList(string xmlString)
        {
            string sql = "", feeID="";
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetTripList/FeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("FeeID不能为空");
                else
                    feeID = vNode.InnerText;

                sql = "Select FID,FFeeID,FStartDate,FDepartDate,FStartCity,FDepartCity,FFee1,FFee2,FFee3,FFee4 From TripDetail Where FFeeID ='{0}' and FDeleted=0";
                sql = string.Format(sql, feeID);
                SQLServerHelper runner = new SQLServerHelper();
                
                DataTable dt  = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetTripList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetFeeDetail
        public string GetFeeDetail(string xmlString)
        {
            string sql = "", feeID = "",type="";
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetFeeDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("ID不能为空");
                else
                    feeID = vNode.InnerText;
                vNode = doc.SelectSingleNode("GetFeeDetail/Type");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("Type不能为空");
                else
                    type = vNode.InnerText;

                sql = " Select t1.FID, t1.FCompanyID,t1.FDeptID,t1.FCostCenter,t1.FEmployeeID,t1.FType,t1.FDate,t1.FApproveDate,t1.FAmount,"+
                      " Isnull(t2.FName,'') As FCompanyName,Isnull(t3.FName,'') As FDeptName,Isnull(t4.FName,'') As FEmployeeName,t1.FRemark," +
                      " (Case t1.FStatus When 0 then '审批中' When '1' Then '已审批'  When '-2' Then '未提交'  Else '已取消' End) As FStatus" +
                      "  From FeeList t1"+
                      "  Left Join t_Company t2 On t1.FCompanyID= t2.FID" +
                      "  Left Join t_Items t3 On t1.FDeptID= t3.FID"+
                      "  Left Join t_Items t4 On t1.FEmployeeID = t4.FID"+
                      "  Where t1.FDeleted=0 and t1.FID ='{0}'";
                sql = string.Format(sql, feeID);
                SQLServerHelper runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetFeeDetail", "", "Main");
                if(dt.Rows.Count >0)
                {
                    string detailResult = "",pageID="";
                    
                    if(type=="1")//日常费用
                    {
                        xmlString = "<GetExpendList><FeeID>" + feeID + "</FeeID></GetExpendList>";
                        detailResult = GetExpendList(xmlString);
                        doc.LoadXml(detailResult);
                        vNode = doc.SelectSingleNode("GetExpendList/DataRows");
                        result = result.Substring(0, result.Length - 15) + vNode.OuterXml + "</GetFeeDetail>";
                        pageID = "Fee02";
                    }
                    else
                    {
                        xmlString = "<GetTripList><FeeID>" + feeID + "</FeeID></GetTripList>";
                        detailResult = GetTripList(xmlString);
                        doc.LoadXml(detailResult);
                        vNode = doc.SelectSingleNode("GetTripList/DataRows");
                        result = result.Substring(0, result.Length - 15) + vNode.OuterXml + "</GetFeeDetail>";
                        pageID = "Fee01";
                    }

                    //加载图片
                    doc.LoadXml(result);
                    XmlNodeList rows = doc.SelectNodes("GetFeeDetail");
                    foreach (XmlNode row in rows)
                    {
                        XmlNode pNode = row;
                        XmlNode cNode = doc.CreateElement("Iamges");
                        Common.SetImageXmlNode(pageID, feeID, ref cNode, ref doc);
                        pNode.AppendChild(cNode);
                    }

                    result = doc.OuterXml;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region DeleteTripData
        public string DeleteTripData(string xmlMessage)
        {
            string result = "-1", id = "", filter = "", feeId = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlNode vNode = doc.SelectSingleNode("DeleteTripData/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {}
                else
                {
                    id = vNode.InnerText.Trim();
                    filter = filter.Trim().Length > 0 ? filter = filter + " and FID ='" + id + "'" : filter = " FID ='" + id + "'";
                }
                vNode = doc.SelectSingleNode("DeleteTripData/FeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {}
                else
                {
                    feeId = vNode.InnerText.Trim();
                    filter = filter.Trim().Length > 0 ? filter = filter + " and FFeeID ='" + feeId + "'" : filter = " FFeeID ='" + feeId + "'";
                }
                if (filter.Trim().Length == 0)
                    throw new Exception("差旅费用ID和费用列表ID,不能同时为空");

                string sql = "Update TripDetail  Set FDeleted=1  Where " + filter;
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
            if (int.Parse(result) > 0)
                result = id;
            else
                result = "-1";
            return result;
        }
        #endregion
    }
}
