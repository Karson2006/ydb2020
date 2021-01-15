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
    public class CallData
    {
        public CallData()
        {
        }

        public string GetMyList(string xmlString)
        {
            string result = "", val = "", filter = "", employeeID = "", sql = "";

            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetMyCallList/EmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("员工ID不能为空");
                else
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        filter = filter.Trim().Length > 0 ? filter + "  and t1.FEmployeeID ='" + val + "'" : "  t1.FEmployeeID ='" + val + "'";
                        employeeID = val;
                    }
                }

                vNode = doc.SelectSingleNode("GetMyCallList/BeginDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter + " And  t1.FStartTime >= '" + val + "  0:0:0.000'" : " t1.FStartTime >= '" + val + "  0:0:0.000'";
                }

                vNode = doc.SelectSingleNode("GetMyCallList/EndDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter + " And  t1.FEndTime <= '" + val + "  23:59:59.999'" : " t1.FEndTime <= '" + val + "  23:59:59.999'";
                }

                //vNode = doc.SelectSingleNode("GetMyCallList/InstitutionName");
                //if (vNode != null)
                //{
                //    val = vNode.InnerText.Trim();
                //    if (val.Length > 0)
                //        filter = filter.Length > 0 ? filter + " And  t2.FName like '%" + val + "%'" : " t2.FName like '%" + val + "%'";
                //}

                sql = "Select t1.FID,Isnull(t2.FName,'') As FInstitutionName,'' As  FClientName,Isnull(t4.FName,'') As  FEmployeeName," +
                      " (Left(CONVERT(varchar(100), t1.FStartTime, 108),5) +'~' + Left(CONVERT(varchar(100), t1.FEndTime, 108),5)) As FTimeString, t1.FStartTime As FDate" +
                      " From [CallActivity] t1" +
                      " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
                      " Left Join t_Items t4 On t1.FEmployeeID= t4.FID";
                if (filter.Trim().Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " Order by t1.FStartTime Desc";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetMyCallList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        //public DataTable GetTeamList(string leaderId, DateTime beginDate, DateTime endDate, string type = "99")
        public string GetTeamList(string xmlString)
        {
            string result = "", employeeIDs = "", val = "", filterString = "", sql = "";
            result = "<GetTeamCallList>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetTeamCallList>";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetTeamCallList/EmployeeIDs");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)//没有设置下属ID时，通过LeaderID来获取其下属的IDs
                {
                    vNode = doc.SelectSingleNode("GetTeamCallList/LeaderID");
                    if (vNode != null && vNode.InnerText.Trim().Length > 0)
                    {
                        val = vNode.InnerText.Trim();

                        WorkShip w = new WorkShip();
                        string xmlParam = "<GetTeamMembers><LeaderID>" + val + "</LeaderID></GetTeamMembers>";
                        employeeIDs = w.GetTeamMemberIDs(xmlParam);
                        if (employeeIDs.Length == 0)//没有直接下属，直接返回
                            return result;
                        else
                            filterString = filterString.Trim().Length > 0 ? filterString + "  and t1.FEmployeeID In ('" + employeeIDs.Replace("|", "','") + "')" : "  t1.FEmployeeID In ('" + employeeIDs.Replace("|", "','") + "')";
                    }
                    else
                        throw new Exception("团队领导ID不能为空");
                }
                else//已设置了相应的下属IDs，直接读取
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filterString = filterString.Trim().Length > 0 ? filterString + "  and t1.FEmployeeID In ('" + val.Replace("|", "','") + "')" : "  t1.FEmployeeID In ('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetTeamCallList/BeginDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filterString = filterString.Length > 0 ? filterString + " And  t1.FDate >= '" + val + "  0:0:0.000'" : " t1.FDate >= '" + val + "  0:0:0.000'";
                }

                vNode = doc.SelectSingleNode("GetTeamCallList/EndDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filterString = filterString.Length > 0 ? filterString + " And  t1.FDate <= '" + val + "  23:59:59.999'" : " t1.FDate <= '" + val + "  23:59:59.999'";
                }

                sql = "Select t1.*,Isnull(t2.FName,'') As InstitutionName,'' As ClientName,Isnull(t4.FName,'') As  EmployeeName," +
                      " (Left(CONVERT(varchar(100), t1.FStartTime, 108),5) +'~'+ Left(CONVERT(varchar(100), t1.FEndTime, 108),5)) As TimeString,t1.FStartTime As Date" +
                      " From [CallActivity] t1" +
                      " Left Join t_Items t2 On t1.FInstitutionID= t2.FID" +
                      " Left Join t_Items t4 On t1.FEmployeeID= t4.FID";

                if (filterString.Length > 0)
                    sql = sql + " Where " + filterString;

                sql = sql + " Order by t1.FStartTime Desc";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);

                result = Common.DataTableToXml(dt, "GetTeamCallList", "", "List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #region GetList

        public DataTable GetList(string xmlString)
        {
            DataTable result = new DataTable();
            try
            {
                string filter = "", val = "";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                string sql = "SELECT t1.*,Isnull(t2.FName,'') As FEmployeeName,Isnull(t3.FName,'') As FInstitutionName,Isnull(t4.FName,'') As FDepartmentName_Ins, " +
                            " '' As FClientName,Isnull(t6.FName,'') As FProductName" +
                            " FROM CallActivity t1" +
                            " Left Join t_items t2 On t1.FEmployeeID=t2.FID" +
                            " Left Join t_items t3 On t1.FInstitutionID=t3.FID" +
                            " Left Join t_items t4 On t1.FDepartmentID_Ins=t4.FID" +
                            " Left Join t_items t6 On t1.FProductID=t4.FID";
                XmlNode vNode = doc.SelectSingleNode("GetCallList/BeginDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = " t1.FDate >= '" + DateTime.Parse(val).ToString("yyyy-MM-dd") + " 0:0:0.000'";
                }
                vNode = doc.SelectSingleNode("GetCallList/EndDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = filter.Length > 0 ? filter + " and t1.FDate <= '" + DateTime.Parse(val).ToString("yyyy-MM-dd") + " 23:59:59.999'" : "t1.Fate <= '" + DateTime.Parse(val).ToString("yyyy-MM-dd") + " 23:59:59.999'";
                }
                vNode = doc.SelectSingleNode("GetCallList/InstitutioName");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = filter.Length > 0 ? filter + " and t3.FName like '%" + val + "%'" : "  t3.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetCallList/EmployeeIDs");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        filter = filter.Length > 0 ? filter + " and t1.FEmployeeID in('" + val.Replace("|", "','") + "')" : " t1.FEmployeeID in('" + val.Replace("|", "','") + "')";
                }
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " Order by t1.FStartTime Desc";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSql(sql);
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion GetList

        #region GetListXML

        public string GetListXML(string xmlString)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetCallList>" +
                                "<Result>False</Result>" +
                                "<Description></Description><DataRows></DataRows>" +
                                "</GetCallList>";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                DataTable dt = GetList(xmlString);
                result = Common.DataTableToXml(dt, "GetCallList", "", "List");
                //if (dt.Rows.Count > 0)
                //{
                //    #region Set XML Node Value
                //    doc.SelectSingleNode("GetCallList/Result").InnerText = "True";

                //    XmlNode pNode = doc.SelectSingleNode("GetCallList/DataRows");
                //    for (int indx = 0; indx < dt.Rows.Count; ++indx)
                //    {
                //        XmlNode cNode = doc.CreateElement("DataRow");
                //        pNode.AppendChild(cNode);

                //        XmlNode vNode = doc.CreateElement("ID");
                //        vNode.InnerText = dt.Rows[indx]["FID"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FScheduleID");
                //        vNode.InnerText = dt.Rows[indx]["FScheduleID"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FSubject");
                //        vNode.InnerText = dt.Rows[indx]["FSubject"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FEmployeeID");
                //        vNode.InnerText = dt.Rows[indx]["FEmployeeID"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FDate");
                //        vNode.InnerText = dt.Rows[indx]["FDate"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FInstitutionID");
                //        vNode.InnerText = dt.Rows[indx]["FInstitutionID"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FDepartmentID_Ins");
                //        vNode.InnerText = dt.Rows[indx]["FDepartmentID_Ins"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FClientID");
                //        vNode.InnerText = dt.Rows[indx]["FClientID"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FActivity");
                //        vNode.InnerText = dt.Rows[indx]["FActivity"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FStartTime");
                //        vNode.InnerText = dt.Rows[indx]["FStartTime"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FEndTime");
                //        vNode.InnerText = dt.Rows[indx]["FEndTime"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FProductID");
                //        vNode.InnerText = dt.Rows[indx]["FProductID"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FConcept");
                //        vNode.InnerText = dt.Rows[indx]["FConcept"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FResult");
                //        vNode.InnerText = dt.Rows[indx]["FResult"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FAction");
                //        vNode.InnerText = dt.Rows[indx]["FAction"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FDeliveries");
                //        vNode.InnerText = dt.Rows[indx]["FDeliveries"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FRemark");
                //        vNode.InnerText = dt.Rows[indx]["FRemark"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FEmployeeName");
                //        vNode.InnerText = dt.Rows[indx]["FEmployeeName"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FInstitutionName");
                //        vNode.InnerText = dt.Rows[indx]["FInstitutionName"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FDepartmentName_Ins");
                //        vNode.InnerText = dt.Rows[indx]["FDepartmentName_Ins"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FClientName");
                //        vNode.InnerText = dt.Rows[indx]["FClientName"].ToString();
                //        cNode.AppendChild(vNode);

                //        vNode = doc.CreateElement("FProductName");
                //        vNode.InnerText = dt.Rows[indx]["FProductName"].ToString();
                //        cNode.AppendChild(vNode);
                //    }
                //    #endregion
                //}
                //result = doc.InnerXml;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion GetListXML

        #region GetDetail

        public string GetDetail(string xmlMessage)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetCallDetail>" +
                            "<Result>False</Result>" +
                            "<Description></Description>";

            string callID = "-1";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlMessage);
            XmlNode vNode = doc.SelectSingleNode("GetCallDetail/ID");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
                throw new Exception("拜访记录ID不能为空");
            callID = vNode.InnerText.Trim();

            string sql = "SELECT t1.*,Isnull(t2.FName,'') As FEmployeeName,Isnull(t3.FName,'') As FInstitutionName,Isnull(t4.FName,'') As FDepartmentName_Ins," +
                        " '' As FClientName,Isnull(t6.FName,'') As FProductName" +
                        " FROM CallActivity t1" +
                        " Left Join t_items t2 On t1.FEmployeeID=t2.FID" +
                        " Left Join t_items t3 On t1.FInstitutionID=t3.FID" +
                        " Left Join t_items t4 On t1.FDepartmentID_Ins=t4.FID" +
                        " Left Join t_items t6 On t1.FProductID=t6.FID";
            sql = sql + " Where t1.FID='" + callID + "'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            result = Common.DataTableToXml(dt, "GetCallDetail", "", "Main");

            //加入PatientList，病人数
            doc.LoadXml(result);
            XmlNode pNode = doc.SelectSingleNode("GetCallDetail");
            XmlNode cNode = doc.CreateElement("PatientList");
            pNode.AppendChild(cNode);
            pNode = doc.SelectSingleNode("GetCallDetail/PatientList");

            sql = @"Select * from CallPatients Where FCallID='{0}'";
            sql = string.Format(sql, callID);
            dt = runner.ExecuteSql(sql);

            foreach (DataRow row in dt.Rows)
            {
                cNode = doc.CreateElement("DataRow");

                pNode.AppendChild(cNode);

                vNode = doc.CreateElement("Type");
                vNode.InnerText = row["FPatientType"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("OldPatient");
                vNode.InnerText = row["FOldPatientCount"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("NewPatient");
                vNode.InnerText = row["FNewPatientCount"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("Total");
                vNode.InnerText = row["FTotal"].ToString();
                cNode.AppendChild(vNode);
            }

            pNode = doc.SelectSingleNode("GetCallDetail");
            cNode = doc.CreateElement("CallList");
            pNode.AppendChild(cNode);
            pNode = doc.SelectSingleNode("GetCallDetail/CallList");

            sql = @"  Select t1.*,Isnull(t2.FName,'') As FDeptName,Isnull(t3.FName,'') As FClientName
                      From CallDetail t1
                      Left Join t_Items t2 On t1.FDeptID= t2.FID
                      Left Join t_Items t3 On t1.FClientID= t3.FID Where FCallID='{0}'";
            sql = string.Format(sql, callID);
            dt = runner.ExecuteSql(sql);

            foreach (DataRow row in dt.Rows)
            {
                cNode = doc.CreateElement("DataRow");

                pNode.AppendChild(cNode);

                vNode = doc.CreateElement("DeptID");
                vNode.InnerText = row["FDeptID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("DeptName");
                vNode.InnerText = row["FDeptName"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("ClientID");
                vNode.InnerText = row["FClientID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("ClientName");
                vNode.InnerText = row["FClientName"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("Aims");
                vNode.InnerText = row["FAims"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("Result");
                vNode.InnerText = row["FResult"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("Improvement");
                vNode.InnerText = row["FImprovement"].ToString();
                cNode.AppendChild(vNode);
            }
            result = doc.OuterXml;

            return result;
        }

        #endregion GetDetail

        #region Update

        public string Update(string xmlString)
        {
            string id = "", sql = "", valueString = "", FScheduleID = "-1", FEmployeeID = "-1", val = "";
            string FRouteID = "";
            int isNew = 0;

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                XmlDocument doc = new XmlDocument();
                DataTable dt = new DataTable();
                doc.LoadXml(xmlString);

                id = doc.SelectSingleNode("UpdateCallData/ID").InnerText;
                if (id.Trim() == "" || id.Trim() == "-1")//新增
                {
                    isNew = 1;
                }

                XmlNode vNode = doc.SelectSingleNode("UpdateCallData/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    FEmployeeID = val.Trim();
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FEmployeeID='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访人ID不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/Date");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (DateTime.Parse(val).CompareTo(DateTime.Now) > 0)
                        throw new Exception("拜访日期不能大于今天");
                    if (DateTime.Parse(val).CompareTo(DateTime.Now.AddDays(-2)) < 0)
                        throw new Exception("不能补录2天前的拜访记录");

                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDate='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访日期不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/Subject");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSubject='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访主题不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/InstitutionID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInstitutionID='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访机构不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/DepartmentID_Ins");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDepartmentID_Ins='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateCallData/ClientID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FClientID='" + val + "',";
                }
                else
                {
                    //if (isNew == 1) throw new Exception("拜访客户不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/Activity");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FActivity='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访内容不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/StartTime");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FStartTime='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访开始时间不能为空");
                }

                vNode = doc.SelectSingleNode("UpdateCallData/EndTime");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FEndTime='" + val + "',";
                }
                else
                {
                    if (isNew == 1) throw new Exception("拜访结束时间不能为空");
                }
                vNode = doc.SelectSingleNode("UpdateCallData/ProductID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FProductID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateCallData/Concept");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FConcept='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateCallData/Result");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FResult='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateCallData/Action");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FAction='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateCallData/ScheduleID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Trim().Length > 0)
                    {
                        FScheduleID = val;
                        valueString = valueString + "FScheduleID='" + val + "',";
                    }
                }

                vNode = doc.SelectSingleNode("UpdateCallData/RouteID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();

                    FRouteID = val;
                    valueString = valueString + "FRouteID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateCallData/Deliveries");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDeliveries='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateCallData/Remark");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRemark='" + val + "',";
                }

                //id = doc.SelectSingleNode("UpdateCallData/ID").InnerText;
                //if (id.Trim() == "" || id.Trim() == "-1")//新增
                if (isNew == 1)
                {
                    if (FScheduleID.Trim() != "4484030a-28d1-4e5e-ba72-6655f1cb2898")
                    {
                        sql = "Select FID From CallActivity Where FScheduleID='" + FScheduleID + "'";
                        dt = runner.ExecuteSql(sql);
                        if (dt.Rows.Count > 0)
                            throw new Exception("该日程已完成拜访，不能再选择");
                    }

                    if (FRouteID.Trim().Length > 0)
                    {
                        sql = "Select FID From CallActivity Where FRouteID='" + FRouteID + "'";
                        dt = runner.ExecuteSql(sql);
                        if (dt.Rows.Count > 0)
                            throw new Exception("该签到已完成拜访，不能再选择");
                    }

                    id = Guid.NewGuid().ToString();
                    sql = "Insert into CallActivity(FID) Values('" + id + "') ";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                        throw new Exception("添加拜访记录失败");
                    else
                        isNew = 2;
                }

                //经理点评
                vNode = doc.SelectSingleNode("UpdateCallData/Comment");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    valueString = valueString + "FComment='" + val + "',";
                }
                //拜访类型
                vNode = doc.SelectSingleNode("UpdateCallData/Type");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FType='" + val + "',";
                }
                //病人数
                vNode = doc.SelectSingleNode("UpdateCallData/PatientList");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    string[] types = doc.SelectSingleNode("UpdateCallData/PatientList/Type").InnerText.Trim().Split(new[] { '|' });
                    string[] oldpatients = doc.SelectSingleNode("UpdateCallData/PatientList/OldPatient").InnerText.Trim().Split(new[] { '|' });
                    string[] newpatients = doc.SelectSingleNode("UpdateCallData/PatientList/NewPatient").InnerText.Trim().Split(new[] { '|' });
                    //删除已存在的
                    sql = "Delete from CallPatients Where FCallID ='{0}'";
                    sql = string.Format(sql, id);
                    runner.ExecuteSqlNone(sql);
                    for (int i = 0; i < types.Length; i++)
                    {
                        int iold = 0, inew = 0, itotal = 0;
                        if (oldpatients[i].Trim().Length > 0)
                            iold = int.Parse(oldpatients[i].Trim());
                        if (newpatients[i].Trim().Length > 0)
                            inew = int.Parse(newpatients[i].Trim());
                        itotal = iold + inew;
                        if (itotal > 0)
                        {
                            sql = @"Insert Into CallPatients( FCallID,FPatientType,FOldPatientCount,FNewPatientCount,FTotal)
                                Values('{0}','{1}',{2},{3},{4})";
                            sql = string.Format(sql, id, types[i], iold, inew, itotal);
                            runner.ExecuteSqlNone(sql);
                        }
                    }
                }

                //拜访详情
                vNode = doc.SelectSingleNode("UpdateCallData/CallList");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    string[] deptids = doc.SelectSingleNode("UpdateCallData/CallList/DeptID").InnerText.Trim().Split(new[] { '|' });
                    string[] clientids = doc.SelectSingleNode("UpdateCallData/CallList/ClientID").InnerText.Trim().Split(new[] { '|' });
                    string[] aims = doc.SelectSingleNode("UpdateCallData/CallList/Aims").InnerText.Trim().Split(new[] { '|' });
                    string[] results = doc.SelectSingleNode("UpdateCallData/CallList/Result").InnerText.Trim().Split(new[] { '|' });
                    string[] improvements = doc.SelectSingleNode("UpdateCallData/CallList/Improvement").InnerText.Trim().Split(new[] { '|' });
                    //删除已存在的
                    sql = "Delete from CallDetail Where FCallID ='{0}'";
                    sql = string.Format(sql, id);
                    runner.ExecuteSqlNone(sql);
                    int year, weekOfyear;
                    for (int i = 0; i < deptids.Length; i++)
                    {
                        Common.GetWeekIndexOfYear("0", out year, out weekOfyear);
                        sql = @"Insert Into CallDetail( FCallID,FDeptID,FClientID,FAims,FResult,FImprovement)
                                Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
                        sql = string.Format(sql, id, deptids[i], clientids[i], aims[i], results[i], improvements[i]);
                        runner.ExecuteSqlNone(sql);
                    }
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update CallActivity Set " + valueString + " Where FID='" + id + "'";

                    if (FScheduleID != "4484030a-28d1-4e5e-ba72-6655f1cb2898")//计划内拜访，更新日程的是否执行信息
                    {
                        sql = sql + " Update ScheduleExecutor Set FIsExcuted =1 Where FScheduleID='" + FScheduleID + "' and FExcutorID='" + FEmployeeID + "'";
                    }

                    runner.ExecuteSqlNone(sql).ToString();
                    int year, weekOfyear;
                    string month;
                    Common.GetWeekIndexOfYear("0", out year, out weekOfyear);
                    month = DateTime.Now.Month.ToString();
                    sql = $"Update CallActivity Set FWeek='{DateTime.Now.Year + "-" + weekOfyear}',FMonth='{DateTime.Now.ToString("yyyy-MM")}' Where FID='" + id + "'";
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                if (isNew == 2)//新增异常，删除相关已插入的数据
                {
                    sql = "Delete from CallActivity where FID='" + id + "'";
                    runner.ExecuteSqlNone(sql);
                    sql = "Delete from CallPatients where FCallID='" + id + "'";
                    runner.ExecuteSqlNone(sql);
                    sql = "Delete from CallDetail where FCallID='" + id + "'";
                    runner.ExecuteSqlNone(sql);
                }
                id = "-1";
                throw err;
            }

            return id;
        }

        #endregion Update

        #region Delete

        public string Delete(string xmlMessage)
        {
            string result = "-1", callID = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlMessage);
                XmlNode vNode = doc.SelectSingleNode("DeleteCall/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("拜访记录ID不能为空");
                callID = vNode.InnerText.Trim();
                string sql = "Delete from CallActivity Where FID = '" + callID + "'";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
            if (int.Parse(result) > 0)
                result = callID;
            else
                result = "-1";
            return result;
        }

        #endregion Delete
    }
}