using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using iTR.Lib;
using System.Xml;

namespace ydb.BLL
{
    public class RouteData
    {
        private const double EARTH_RADIUS = 6378137;
        private Dictionary<string, DateTime> safePost = new Dictionary<string, DateTime>();

        public RouteData()
        {
        }

        #region GetList

        public string GetList(string xmlString)
        {
            string result = "<GetRouteList>" +
                            "<Result>False</Result>" +
                            "<Description></Description><DataRows></DataRows>" +
                            "</GetRouteList>";

            try
            {
                string filter = "", val = "";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                //string sql = "SELECT t1.*,Isnull(t2.FName,'') As FEmployeeName,Isnull(t3.FName,'') As FInstitutionName" +
                //            " FROM RouteData t1" +
                //            " Left join t_Items t2 On t1.FEmployeeID= t2.FID" +
                //            " Left join t_Items t3 On t1.FInstitutionID= t3.FID";
                string sql = @"	SELECT t1.*,Isnull(t2.FName,'') As FEmployeeName,Isnull(t3.FName,'')  As FInstitutionName ,case FStatus when -1 then '有效签到' else   '无效签到' end FValid FROM RouteData t1

                               Left join t_Items t2 On t1.FEmployeeID = t2.FID

                               Left join t_Items t3 On t1.FInstitutionID = t3.FID";
                XmlNode vNode = doc.SelectSingleNode("GetRouteList/BeginDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = " t1.FDate >= '" + DateTime.Parse(val).ToString("yyyy-MM-dd") + " 0:0:0.000'";
                }

                vNode = doc.SelectSingleNode("GetRouteList/EndDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = filter.Length > 0 ? filter + " and t1.FDate < '" + DateTime.Parse(val).ToString("yyyy-MM-dd") + " 23:59:59.999'" : "t1.Fate < '" + DateTime.Parse(val).ToString("yyyy-MM-dd") + " 23:59:59.999'";
                }

                vNode = doc.SelectSingleNode("GetRouteList/InstitutionName");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = filter.Length > 0 ? filter + " and t1.FInstitutionName like  '%" + val + "%'" : " t1.FInstitutionName like  '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetRouteList/EmployeeIDs");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = filter.Length > 0 ? filter + " and t1.FEmployeeID in('" + val.Replace("|", "','") + "')" : " t1.FEmployeeID in('" + val.Replace("|", "','") + "')";
                }

                if (filter.Length > 0)
                    sql = sql + " Where " + filter + " Order by t1.FEmployeeID,t1.FSignInTime Desc";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                //result = Common.DataTableToXml(dt, "GetRouteList", "", "List");
                if (dt.Rows.Count > 0)
                {
                    #region Set XML Node Value

                    doc.LoadXml(result);
                    doc.SelectSingleNode("GetRouteList/Result").InnerText = "True";

                    XmlNode pNode = doc.SelectSingleNode("GetRouteList/DataRows");
                    for (int indx = 0; indx < dt.Rows.Count; ++indx)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = dt.Rows[indx]["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FEmployeeID");
                        vNode.InnerText = dt.Rows[indx]["FEmployeeID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDate");
                        vNode.InnerText = DateTime.Parse(dt.Rows[indx]["FDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignInTime");
                        if (dt.Rows[indx]["FSignInTime"].ToString().Length > 0)
                            vNode.InnerText = DateTime.Parse(dt.Rows[indx]["FSignInTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            vNode.InnerText = "";
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignOutTime");
                        if (dt.Rows[indx]["FSignOutTime"].ToString().Length > 0)
                            vNode.InnerText = DateTime.Parse(dt.Rows[indx]["FSignOutTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            vNode.InnerText = "";

                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FInstitutionID");
                        vNode.InnerText = dt.Rows[indx]["FInstitutionID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignInLat");
                        vNode.InnerText = dt.Rows[indx]["FSignInLat"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignInLng");
                        vNode.InnerText = dt.Rows[indx]["FSignInLng"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignOutLat");
                        vNode.InnerText = dt.Rows[indx]["FSignOutLat"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignOutLng");
                        vNode.InnerText = dt.Rows[indx]["FSignOutLng"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignInAddress");
                        vNode.InnerText = dt.Rows[indx]["FSignInAddress"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignOutAddress");
                        vNode.InnerText = dt.Rows[indx]["FSignOutAddress"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSignInPhotoPath");
                        vNode.InnerText = dt.Rows[indx]["FSignInPhotoPath"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRemark");
                        vNode.InnerText = dt.Rows[indx]["FRemark"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FEmployeeName");
                        vNode.InnerText = dt.Rows[indx]["FEmployeeName"].ToString();
                        cNode.AppendChild(vNode);
                        vNode = doc.CreateElement("FValid");
                        vNode.InnerText = dt.Rows[indx]["FValid"].ToString();
                        cNode.AppendChild(vNode);
                        vNode = doc.CreateElement("FInstitutionName");
                        if (dt.Rows[indx]["FInstitutionName"].ToString().Trim().Length > 0)
                            vNode.InnerText = dt.Rows[indx]["FInstitutionName"].ToString();
                        else
                            vNode.InnerText = dt.Rows[indx]["FSignInAddress"].ToString();
                        cNode.AppendChild(vNode);
                    }

                    #endregion Set XML Node Value

                    result = doc.OuterXml;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion GetList

        #region GetDetail

        public string GetDetail(string routeID)
        {
            #region Build the XML Schema

            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetRouteDetail>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<ID></ID>" +
                            "<FEmployeeID></FEmployeeID>" +
                            "<FDate></FDate>" +
                            "<FSignOutDate></FSignOutDate>" +
                            "<FSignInTime></FSignInTime>" +
                            "<FSignOutTime></FSignOutTime>" +
                            "<FInstitutionID></FInstitutionID>" +
                            "<FSignInLat></FSignInLat>" +
                            "<FSignInLng></FSignInLng>" +
                            "<FSignOutLat></FSignOutLat>" +
                            "<FSignOutLng></FSignOutLng>" +
                            "<FSignInAddress></FSignInAddress>" +
                            "<FSignOutAddress></FSignOutAddress>" +
                            "<FSignInPhotoPath></FSignInPhotoPath>" +
                            "<FRemark></FRemark>" +
                            "<FEmployeeName></FEmployeeName>" +
                            "<FInstitutionName></FInstitutionName>" +
                            "</GetRouteDetail>";

            #endregion Build the XML Schema

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            string sql = "SELECT t1.*,Isnull(t2.FName,'') As FEmployeeName,Isnull(t3.FName,'') As FInstitutionName" +
                           " FROM RouteData t1" +
                           " Left join t_Items t2 On t1.FEmployeeID= t2.FID" +
                           " Left join t_Items t3 On t1.FInstitutionID= t3.FID";
            sql = sql + " Where t1.FID='" + routeID + "'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            result = Common.DataTableToXml(dt, "GetRouteDetail", "", "Main");

            //if(dt.Rows.Count>0)
            //{
            //    #region Set XMLNode Value
            //    doc.SelectSingleNode("GetRouteDetail/Result").InnerText = "True";
            //    doc.SelectSingleNode("GetRouteDetail/ID").InnerText = dt.Rows[0]["FID"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FEmployeeID").InnerText = dt.Rows[0]["FEmployeeID"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignInTime").InnerText = dt.Rows[0]["FSignInTime"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignOutTime").InnerText = dt.Rows[0]["FSignOutTime"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FInstitutionID").InnerText = dt.Rows[0]["FInstitutionID"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FInstitutionName").InnerText = dt.Rows[0]["FInstitutionName"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignInLat").InnerText = dt.Rows[0]["FSignInLat"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignInLng").InnerText = dt.Rows[0]["FSignInLng"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignOutLat").InnerText = dt.Rows[0]["FSignOutLat"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignOutLng").InnerText = dt.Rows[0]["FSignOutLng"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignInAddress").InnerText = dt.Rows[0]["FSignInAddress"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignOutAddress").InnerText = dt.Rows[0]["FSignOutAddress"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignInPhotoPath").InnerText = dt.Rows[0]["FSignInPhotoPath"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FRemark").InnerText = dt.Rows[0]["FRemark"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FEmployeeName").InnerText = dt.Rows[0]["FEmployeeName"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FSignOutDate").InnerText = dt.Rows[0]["FSignOutDate"].ToString();
            //    doc.SelectSingleNode("GetRouteDetail/FDate").InnerText = dt.Rows[0]["FDate"].ToString();
            //    #endregion

            //}
            //result = doc.InnerXml;
            return result;
        }

        #endregion GetDetail

        #region Update

        public string Update(string dataString)
        {
            string id = "", sql = "", valueString = "";

            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataString);
                id = doc.SelectSingleNode("UpdateRouteData/RouteID").InnerText;

                if (id.Trim() == "" || id.Trim() == "-1")//新增
                {
                    id = Guid.NewGuid().ToString();
                    sql = "Insert into RouteData(FID) Values('" + id + "') ";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                        throw new Exception("新增失败");
                }
                //更新日程信息
                XmlNode vNode = doc.SelectSingleNode("UpdateRouteData/EmployeeID");
                string val = "";
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FEmployeeID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/FType");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FType='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/Date");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDate='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/SignOutDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignOutDate='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRouteData/SignInTime");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignInTime='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRouteData/SignOutTime");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignOutTime='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRouteData/InstitutionID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInstitutionID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/InstitutionName");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInstitutionName='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRouteData/SignInLat");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignInLat='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRouteData/SignInLng");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignInLng='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/SignOutLat");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignOutLat='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/SignOutLng");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignOutLng='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/SignInAddress");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignInAddress='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateRouteData/SignOutAddress");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignOutAddress='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/SignInPhotoPath");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSignInPhotoPath='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateRouteData/FRemark");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRemark='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update RouteData Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新日程失败
                        throw new Exception("更新失败");
                    else
                    {
                        int year, weekOfyear;

                        Common.GetWeekIndexOfYear("0", out year, out weekOfyear);
                        sql = $"Update RouteData Set FWeek='{DateTime.Now.Year + "-" + weekOfyear}',FMonth='{DateTime.Now.ToString("yyyy-MM")}', FSignInTime = CONVERT(varchar(100),FDate,23)+' ' + CONVERT(varchar(100),FSignInTime, 8),FSignOutTime= CONVERT(varchar(100),FSignOutDate,23)+' ' + CONVERT(varchar(100),FSignOutTime, 8) ";
                        sql = sql + " Where FID='" + id + "'";
                        runner.ExecuteSqlNone(sql);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }

            return id;
        }

        #endregion Update

        #region Delete

        public string Delete(string routeID)
        {
            string result = "-1";
            try
            {
                string sql = "Delete from RouteData Where FID = '" + routeID + "'";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
            if (int.Parse(result) > 0)
                result = routeID;
            else
                result = "-1";
            return result;
        }

        #endregion Delete

        #region SignIn

        public string SignIn(string xmlString)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SignIn>" +
                                "<Result>False</Result>" +
                                "<Description/><RoutID></RoutID>" +
                                "</SignIn>";
            try
            {
                XmlDocument doc = new XmlDocument();
                string institutionID = "", institutionName = "";
                XmlNode pNode = null, cNode = null;

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("SignIn/EmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签到者ID不能为空");

                vNode = doc.SelectSingleNode("SignIn/Date");
                if (vNode == null)
                {
                    pNode = doc.SelectSingleNode("SignIn");
                    cNode = doc.CreateElement("Date");
                    cNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                    pNode.AppendChild(cNode);
                }
                else
                {
                    vNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                }

                vNode = doc.SelectSingleNode("SignIn/SignInTime");
                if (vNode == null)
                {
                    pNode = doc.SelectSingleNode("SignIn");
                    cNode = doc.CreateElement("SignInTime");
                    cNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pNode.AppendChild(cNode);
                }
                else
                {
                    vNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                xmlString = doc.OuterXml;

                //vNode = doc.SelectSingleNode("SignIn/InstitutionID");
                //if (vNode == null || vNode.InnerText.Trim().Length == 0)
                //    throw new Exception("签入机构ID不能为空");

                vNode = doc.SelectSingleNode("SignIn/SignInAddress");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签入地址不能为空");
                vNode = doc.SelectSingleNode("SignIn/SignInLat");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签入经度不能为空");
                vNode = doc.SelectSingleNode("SignIn/SignInLng");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签入纬度不能为空");
                //if (signDate.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                //    throw new Exception("签到日期不是今天，不能签入");

                string sql = "Select FID from RouteData Where FEmployeeID='" + doc.SelectSingleNode("SignIn/EmployeeID").InnerText + "'";
                sql = sql + " And FDate between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 0:0:0.000' And '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999'";
                sql = sql + " And FSignOutAddress=''";
                SQLServerHelper runer = new SQLServerHelper();
                DataTable tb = runer.ExecuteSql(sql);
                if (tb.Rows.Count > 0)//存在未签退的签到记录，不能再次签入
                    throw new Exception("当天存在未签退的签到记录，请先签退");
                doc.SelectSingleNode("SignIn/RouteID").InnerText = "";//设置新增标识

                vNode = doc.SelectSingleNode("SignIn/SignOutAddress");
                if (vNode != null)
                    doc.SelectSingleNode("SignIn/SignOutAddress").InnerText = "";

                vNode = doc.SelectSingleNode("SignIn/InstitutionID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    institutionID = vNode.InnerText.Trim();
                else
                    institutionID = "";
                //判断此机构是否是注册机构
                vNode = doc.SelectSingleNode("SignIn/InstitutionName");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    institutionName = vNode.InnerText.Trim();
                else
                    institutionName = "";

                if (institutionID.Length == 0 && institutionName.Length > 0)
                {
                    //sql = "Select FName,FID From t_Items t1 Where FClassID='aa6e8a63-1ce3-40ef-9254-0d6b2b3838dd' and FIsDeleted=0 and FName='{0}'";
                    sql = "Select FName,FID From t_Items t1 Where  FIsDeleted=0 and FName='{0}'";
                    sql = string.Format(sql, institutionName);
                    DataTable dt = runer.ExecuteSql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        institutionID = dt.Rows[0]["FID"].ToString();
                        doc.SelectSingleNode("SignIn/InstitutionID").InnerText = institutionID;
                    }
                    else
                    {
                        institutionID = "";
                        doc.SelectSingleNode("SignIn/InstitutionName").InnerText = "";
                    }
                }
                xmlString = doc.OuterXml;
                xmlString = xmlString.Replace("SignIn>", "UpdateRouteData>");//替换为UpdateRouteData
                result = Update(xmlString);

                runer.ExecuteSqlNone(sql);
                if (result != "-1")//签入成功
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><SignIn>" +
                                "<Result>True</Result>" +
                                "<Description/><RouteID>" + result + "</RouteID>" +
                                "</SignIn>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion SignIn

        #region SignOut

        public string SignOut(string xmlString)
        {
            string result = "<SignOut>" +
                                "<Result>False</Result>" +
                                "<Description/><RoutwID></RouteID>" +
                                "</SignOut>";
            XmlNode pNode = null, cNode = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("SignOut/RouteID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("请选择要签退的签入记录");

                vNode = doc.SelectSingleNode("SignOut/SignOutAddress");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签退地址不能为空");

                vNode = doc.SelectSingleNode("SignOut/SignOutDate");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    pNode = doc.SelectSingleNode("SignOut");
                    cNode = doc.CreateElement("SignOutDate");
                    cNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                    pNode.AppendChild(cNode);
                }
                else
                {
                    vNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                }
                vNode = doc.SelectSingleNode("SignOut/SignOutTime");
                if (vNode == null)
                {
                    pNode = doc.SelectSingleNode("SignOut");
                    cNode = doc.CreateElement("SignOutTime");
                    cNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pNode.AppendChild(cNode);
                }
                else
                {
                    vNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                xmlString = doc.OuterXml;

                string sql = "Select FID,FInstitutionID from RouteData Where FSignOutAddress <> '' and FID='" + doc.SelectSingleNode("SignOut/RouteID").InnerText + "'";
                SQLServerHelper runer = new SQLServerHelper();
                DataTable tb = runer.ExecuteSql(sql);
                if (tb.Rows.Count > 0)//
                    throw new Exception("该签到记录已签退");

                result = Update(xmlString.Replace("SignOut>", "UpdateRouteData>"));
                if (result != "-1")//签入成功
                    result = "<SignOut>" +
                                "<Result>True</Result>" +
                                "<Description/><RouteID>" + result + "</RouteID>" +
                                "</SignOut>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion SignOut

        public string AutoRoute(string xmlString)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><AutoRoute>" +
                                 "<Result>False</Result>" +
                                 "<Description/><RoutID></RoutID>" +
                                 "</AutoRoute>", employeeId, lat, lng, fDate, signTime;
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode pNode = null, cNode = null;

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("AutoRoute/Version");
                //不是新版本不签到
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    return result;
                }

                vNode = doc.SelectSingleNode("AutoRoute/EmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签到者ID不能为空");
                else
                {
                    employeeId = doc.SelectSingleNode("AutoRoute/EmployeeID").InnerText.Trim().ToString();
                }
                vNode = doc.SelectSingleNode("AutoRoute/Date");
                if (vNode == null)
                {
                    pNode = doc.SelectSingleNode("AutoRoute");
                    cNode = doc.CreateElement("Date");
                    cNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                    pNode.AppendChild(cNode);
                }
                else
                {
                    vNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                }
                fDate = DateTime.Now.ToString("yyyy-MM-dd");
                vNode = doc.SelectSingleNode("AutoRoute/SignInTime");
                if (vNode == null)
                {
                    pNode = doc.SelectSingleNode("AutoRoute");
                    cNode = doc.CreateElement("SignInTime");
                    cNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    pNode.AppendChild(cNode);
                }
                else
                {
                    vNode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                signTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                xmlString = doc.OuterXml;
                vNode = doc.SelectSingleNode("AutoRoute/SignInLat");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签入经度不能为空");
                lat = doc.SelectSingleNode("AutoRoute/SignInLat").InnerText.ToString();
                vNode = doc.SelectSingleNode("AutoRoute/SignInLng");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("签入纬度不能为空");
                lng = doc.SelectSingleNode("AutoRoute/SignInLng").InnerText.ToString();
                result = "ID:" + doc.SelectSingleNode("AutoRoute/EmployeeID").InnerText.ToString() + "SignInLat:" + lat + "SignInLng:" + lng + "";
                SQLServerHelper runner = new SQLServerHelper();
                string sql = "";

                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><AutoRoute>" +
                "<Result>True</Result>" +
                "<Description>" + result + "<Description/>" +
                "</AutoRoute>";
                //根据EmployeeID获取签到地址
                sql = $"  SELECT sch.FID, isnull(hos.FLatitude,'') FLatitude,isnull(hos.FLongitude,'') FLongitude,item.FName FROM [yaodaibao].[dbo].[Schedule] sch left join [dbo].[t_Hospital] hos on sch.FID = hos.FID left join  [dbo].[t_Items] item on sch.FInstitutionID = item.FID  where sch.FEmployeeID = '{employeeId}' ";

                DataTable tbmeter = runner.ExecuteSql(sql);
                int meter = 1000;
                //避免有可以同时签到的位置 保存签到ID和签到的地址名称
                Dictionary<string, int> dicDistance = new Dictionary<string, int>();
                foreach (DataRow row in tbmeter.Rows)
                {
                    //############## 签到地址
                    meter = (int)GetDistance(double.Parse(row["FLongitude"].ToString()), double.Parse(row["FLatitude"].ToString()), double.Parse(doc.SelectSingleNode("AutoRoute/SignInLng").InnerText.Replace("-", "")), double.Parse(doc.SelectSingleNode("AutoRoute/SignInLat").InnerText.Replace("-", "")));

                    //比较当前位置和签到地址距离 "-" 移除意外字符

                    dicDistance.Add(row["FID"].ToString(), meter);
                }

                //查找需要签退的地址
                sql = "Select FID,[FSignOutTime] from RouteData Where FEmployeeID='" + doc.SelectSingleNode("AutoRoute/EmployeeID").InnerText + "'";
                sql = sql + " And FDate between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 0:0:0.000' And '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999'";
                sql = sql + " And FSignOutAddress=''";
                DataTable tbsign = runner.ExecuteSql(sql);
                if (tbsign.Rows.Count > 0)//存在未签退的签到记录,自动签退
                {
                    if (0 < dicDistance.Count)
                    {
                        //根据ID查找签退距离 RouteData FID 和 schedule FID 是相同FID
                        meter = dicDistance.FirstOrDefault(x => x.Key == tbmeter.Rows[0]["FID"].ToString()).Value;
                        //大于200签退
                        if (meter > 200)
                        {
                            DataRow row = tbmeter.Rows.Find(tbmeter.Rows[0]["FID"]);
                            sql = $"Update RouteData Set FSignOutAddress ='{row["FName"]}',FSignOutDate='{fDate}',FSignOutTime='{signTime}',FSignOutLat={lat},FSignOutLng={lng},[FDistance] = {meter} where FID ='{tbmeter.Rows[0]["FID"]}'";
                            runner.ExecuteSqlNone(sql);
                        }
                    }
                }
                //签到
                else
                {
                    //先过滤小于200的签到地址 再排序签到地址
                    dicDistance = dicDistance.Where(x => x.Value < 200).ToDictionary(x => x.Key, x => x.Value);
                    dicDistance = dicDistance.OrderBy(o => o.Value).ToDictionary(o => o.Key, o => o.Value);
                    if (0 < dicDistance.Count)
                    {
                        //赋值签退地址
                        meter = dicDistance.Values.First();
                        //小于200签到
                        if (meter < 200)
                        {
                            //先查找签到最后一次的签到记录时间
                            sql = $"SELECT TOP 1 [FSignOutTime] FROM [yaodaibao].[dbo].[RouteData]  where FEmployeeID = '{employeeId}' order by FSignOutTime desc";
                            tbsign = runner.ExecuteSql(sql);
                            if (0 < tbsign.Rows.Count)
                            {
                                //签退之后一定不直接签到 间隔10秒，为了前端异步请求问题
                                DateTime temptime = Convert.ToDateTime(tbsign.Rows[0]["FSignOutTime"]).AddSeconds(20);
                                if (0 < DateTime.Compare(DateTime.Now, temptime))
                                {
                                    //设置第一列为主键
                                    tbmeter.PrimaryKey = new DataColumn[] { tbmeter.Columns[0] };
                                    //根据ID获取签到点名称
                                    DataRow row = tbmeter.Rows.Find(dicDistance.Keys.First());
                                    var nullvalue = DBNull.Value;
                                    sql = $"insert into RouteData ([FID],[FEmployeeID],[FSignInAddress],[FDate],[FSignInTime],[FSignOutTime],[FSignInLat],[FSignInLng],[FDistance]) values('{Guid.NewGuid()}','{employeeId}','{row["FName"]}','{fDate}','{signTime}',NULL,'{lat}','{lng}',{meter})";
                                    runner.ExecuteSqlNone(sql);
                                }
                            }
                        }
                    }
                }

                //方法是状态
                FileLogger.WriteLog("自动定位End:|" + doc.SelectSingleNode("AutoRoute/EmployeeID").InnerText.ToString() + "|" + result, 1, "", "AutoRoute");

                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><AutoRoute>" +
                            "<Result>True</Result>" +
                            "</AutoRoute>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }

        private static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result;
        }

        #region 修改自动签到状态

        public string AlterAutoStatus(string xmlstring)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><AlterState>" +
                       "<Result>False</Result>" +
                       "</AlterState>";
            try
            {
                string status = "", mode = "", employeeId = "000000", sql = "";
                XmlDocument doc = new XmlDocument();
                XmlNode pNode = null, cNode = null;
                doc.LoadXml(xmlstring);
                DataTable dataTable = new DataTable();
                XmlNode vNode = doc.SelectSingleNode("AlterAutoStatus/ID");
                if (vNode == null && vNode.InnerText.Trim() == "")
                {
                    throw new Exception("ID不能为空");
                }

                employeeId = vNode.InnerText.Trim();
                //0是保存，1是获取
                vNode = doc.SelectSingleNode("AlterAutoStatus/Mode");
                mode = vNode.InnerText.Trim();
                vNode = doc.SelectSingleNode("AlterAutoStatus/Status");
                if (vNode != null && vNode.InnerText.Trim() != "")
                {
                    status = vNode.InnerText.Trim();
                }
                SQLServerHelper runner = new SQLServerHelper();
                //保存自动签到状态
                if (mode == "0")
                {
                    sql = $"if exists (select FID from yaodaibao.dbo.Profile   where FEmployeeID='{ employeeId}') update yaodaibao.dbo.Profile set FRouteStatus = '{status}' where FEmployeeID = '{employeeId}' ELSE  insert into yaodaibao.dbo.Profile(FEmployeeID, FRouteStatus) values('{employeeId}', '{status}') select FRouteStatus from Profile where FEmployeeID = '{employeeId}'";
                    dataTable = runner.ExecuteSql(sql);
                    result = $"<AlterAutoStatus><Result>True</Result><Status>{dataTable.Rows[0]["FRouteStatus"]}</Status></AlterAutoStatus>";
                }
                else
                {
                    sql = $"select FRouteStatus from yaodaibao.dbo.Profile where FEmployeeID = '{employeeId}'";
                    dataTable = runner.ExecuteSql(sql);
                    //开启自动了签到
                    if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["FRouteStatus"].ToString() == "True")
                    {
                        result = $"<AlterAutoStatus><Result>True</Result><Status>{dataTable.Rows[0]["FRouteStatus"]}</Status></AlterAutoStatus>";
                    }
                    else
                    {
                        result = $"<AlterAutoStatus><Result>False</Result><Status></Status></AlterAutoStatus>";
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><AlterAutoStatus><Result>False</Result><Description>{e.Message}</Description></AlterAutoStatus>";
            }
            return result;
        }

        #endregion 修改自动签到状态
    }
}