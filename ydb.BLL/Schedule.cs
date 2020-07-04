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
    public class Schedule
    {
        public Schedule()
        {

        }

        

        public string GetList(string xmlString)
        {

            #region Build the XML Schema
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ScheduleList><DataRows>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "</ScheduleList>";
            #endregion
            string val="",filterString="",dateFilter="";
            try
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetScheduleList>" +
                            "<Result>False</Result>" +
                            "<Description></Description><DataRows></DataRows>" +
                            "</GetScheduleList>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetScheduleList/StartDate");
                if(vNode!=null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        dateFilter = dateFilter.Length > 0 ? dateFilter + " And  t2.FStartTime >= '" + val + "  0:0:0.000'" : " t2.FStartTime >= '" + val + "  0:0:0.000'";
                }

                vNode = doc.SelectSingleNode("GetScheduleList/EndDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        dateFilter = dateFilter.Length > 0 ? dateFilter + " And  t2.FEndTime <= '" + val + "  23:59:59.999'" : " t2.FEndTime <= '" + val + "  23:59:59.999'";
                }

                vNode = doc.SelectSingleNode("GetScheduleList/EmployeeIDs");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filterString = filterString.Trim().Length > 0 ? filterString + "  and t1.FExcutorID In ('" + val.Replace("|", "','") + "')" : "  t1.FExcutorID In ('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetScheduleList/Type");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0 && val != "99")
                    {
                        ///将类型代码换为ID
                        val = TypeNumber2ID(val);
                     
                        filterString = filterString.Length > 0 ? filterString + " and  t2.FType In ('" + val.Replace("|", "','") + "')" : " t2.FType In ('" + val.Replace("|", "','") + "')";
                    }
                }
                if (dateFilter.Length > 0)
                    filterString = filterString + (filterString.Trim().Length > 0 ? " and  " + dateFilter : dateFilter);

                string sql = "SELECT t1.FScheduleID,t1.FExcutorID,t3.FName AS FExcutorName,t1.FIsExcuted,t4.FName As FEmployee,t2.FType,t2.FSubject," +
                                   "t2.FDepartment,t2.FDate,t2.FInstitutionType,t2.FInstitutionID,t5.FName as FInstitutionName,t6.FName AS FDepartment_Ins," +
                                   "t7.FName AS FClientName,t2.FActivity,Left(CONVERT(varchar, t2.FStartTime, 120 ),16) As FStartTime,Left(CONVERT(varchar, t2.FEndTime, 120 ),16) As FEndTime,t2.FReminderTime,t2.FRemark," +
                                   "t8.FName As FCoachName,t2.FDepartmentID_Ins,t2.FClientID,t2.FCoachID,t9.FName  As FTypeName" +
                            " FROM ScheduleExecutor t1" +
                            " Left Join Schedule t2 On t1.FScheduleID= t2.FID" +
                            " Left Join t_Items t3 On t1.FExcutorID= t3.FID" +
                            " Left Join t_Items t4 On t4.FID= t2.FEmployeeID" +
                            " Left Join t_Items t5 On t5.FID= t2.FInstitutionID" +
                            " Left Join t_Items t6 On t6.FID= t2.FDepartmentID_Ins" +
                            " Left Join t_Items t7 On t7.FID= t2.FClientID" +
                            " Left Join t_Items t8 On t8.FID= t2.FCoachID "+
                            " Left Join t_Items t9 On t9.FID= t2.FType";
                if (filterString.Trim().Length > 0)
                    sql = sql + " Where " + filterString;
                sql = sql + " order by t2.FStartTime Desc";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);

                result = Common.DataTableToXml(dt, "GetScheduleList", "", "List");

              
               
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }

        public string GetDetail(string dataString)
        {
            string scheduleID = "";
            #region Build the XML Schema
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetScheduleDetail>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "</GetScheduleDetail>";
            #endregion
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dataString);
            XmlNode vNode = doc.SelectSingleNode("GetScheduleDetail/ID");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
            {
                throw new Exception("日程ID不能为空");
            }
            else
                scheduleID = vNode.InnerText.Trim();

            string sql = " SELECT t4.FName As FEmployee,t2.FType,t2.FSubject,t2.FID, t1.FName As FTypeName," +
                                "t2.FDepartment,t2.FDate,t2.FInstitutionType,t2.FInstitutionID,t5.FName as FInstitutionName,t6.FName AS FDepartment_Ins," +
                                "t7.FName AS FClientName,t2.FActivity,Left(CONVERT(varchar, t2.FStartTime, 120 ),16) AS FStartTime,Left(CONVERT(varchar, t2.FEndTime, 120 ),16) AS FEndTime,t2.FReminderTime,t2.FRemark," +
                                "t8.FName As FCoachName,t2.FDepartmentID_Ins,t2.FClientID,t2.FCoachID" +
                         " From Schedule t2 " +
                         " Left Join t_Items t4 On t4.FID= t2.FEmployeeID" +
                         " Left Join t_Items t5 On t5.FID= t2.FInstitutionID" +
                         " Left Join t_Items t6 On t6.FID= t2.FDepartmentID_Ins" +
                         " Left Join t_Items t7 On t7.FID= t2.FClientID" +
                         " Left Join t_Items t8 On t8.FID= t2.FCoachID"+
                         " Left Join t_Items t1 On t1.FID= t2.FType";

            sql= sql + " Where "+" t2.FID='" + scheduleID + "'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            doc.LoadXml(result);
            if(dt.Rows.Count>0)
            {
                #region Set XMLNode Value
                 result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetScheduleDetail>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<ID></ID>" +
                            "<FSubject></FSubject>" +
                            "<FType></FType>" +
                            "<FEmployeeID></FEmployeeID>" +
                            "<FEmployee></FEmployee>" +
                            "<FDepartment></FDepartment>" +
                            "<FDate></FDate>" +
                            "<FInstitutionType></FInstitutionType>" +
                            "<FInstitutionID></FInstitutionID>" +
                            "<FInstitutionName></FInstitutionName>" +
                            "<FDepartmentID_Ins></FDepartmentID_Ins>" +
                            "<FDepartment_Ins></FDepartment_Ins>" +
                            "<FClientName></FClientName>" +
                            "<FClientID></FClientID>" +
                            "<FActivity></FActivity>" +
                            "<FStartTime></FStartTime>" +
                            "<FEndTime></FEndTime>" +
                            "<FReminderTime></FReminderTime>" +
                            "<ExecutorList></ExecutorList>" +
                            "<CoachList></CoachList>" +
                            "<ClientList></ClientList>" +
                            "<FCoachID></FCoachID>" +
                            "<FCoachName></FCoachName>" +
                            "<FTypeName></FTypeName>" +
                            "<FRemark></FRemark><ExcutorList></ExcutorList>" +
                            "</GetScheduleDetail>";
                doc.LoadXml(result);

                doc.SelectSingleNode("GetScheduleDetail/ID").InnerText = dt.Rows[0]["FID"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FSubject").InnerText = dt.Rows[0]["FSubject"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FDepartment").InnerText = dt.Rows[0]["FDepartment"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FType").InnerText = dt.Rows[0]["FType"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FTypeName").InnerText = dt.Rows[0]["FTypeName"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FDate").InnerText = dt.Rows[0]["FDate"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FInstitutionType").InnerText = dt.Rows[0]["FInstitutionType"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FInstitutionID").InnerText = dt.Rows[0]["FInstitutionID"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FInstitutionName").InnerText = dt.Rows[0]["FInstitutionName"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FDepartmentID_Ins").InnerText = dt.Rows[0]["FDepartmentID_Ins"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FDepartment_Ins").InnerText = dt.Rows[0]["FDepartment_Ins"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FClientID").InnerText = dt.Rows[0]["FClientID"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FClientName").InnerText = dt.Rows[0]["FClientName"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FActivity").InnerText = dt.Rows[0]["FActivity"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FStartTime").InnerText =DateTime.Parse( dt.Rows[0]["FStartTime"].ToString()).ToString("yyyy-MM-dd  HH:mm") ;
                doc.SelectSingleNode("GetScheduleDetail/FEndTime").InnerText =DateTime.Parse( dt.Rows[0]["FEndTime"].ToString()).ToString("yyyy-MM-dd  HH:mm") ;
                doc.SelectSingleNode("GetScheduleDetail/FReminderTime").InnerText = dt.Rows[0]["FReminderTime"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FCoachID").InnerText = dt.Rows[0]["FCoachID"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FCoachName").InnerText = dt.Rows[0]["FCoachName"].ToString();
                doc.SelectSingleNode("GetScheduleDetail/FRemark").InnerText = dt.Rows[0]["FRemark"].ToString();
                #endregion

                sql = "Select t1.*,Isnull(t2.FName,'') As FExecutorName"+
                     " From ScheduleExecutor t1"+
                     " Left Join t_Items t2 On t1.FExcutorID = t2.FID";
                sql = sql + " Where t1.FScheduleID='" + scheduleID + "'";

                dt = runner.ExecuteSql(sql);
                XmlNode pNode = doc.SelectSingleNode("GetScheduleDetail/ExecutorList");
                XmlNode pNode_Coach = doc.SelectSingleNode("GetScheduleDetail/CoachList");
                foreach (DataRow row in dt.Rows)
                {
                    XmlNode cNode = doc.CreateElement("ExecutorRow");
                    if (row["FType"].ToString() == "1")//执行人
                    {
                        pNode.AppendChild(cNode);
                    }
                    else
                    {
                        pNode_Coach.AppendChild(cNode);
                    }

                    vNode = doc.CreateElement("FExcutorID");
                    vNode.InnerText = row["FExcutorID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FExecutorName");
                    vNode.InnerText = row["FExecutorName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FIsExcuted");
                    vNode.InnerText = row["FIsExcuted"].ToString();
                    cNode.AppendChild(vNode);
                }

                sql = @"Select t1.*,Isnull(t2.FName,'') As FClientName,Isnull(t3.FName,'') As FDeptName
                        From Scheduleclients t1
                        Left Join t_Items t2 On t1.FClientID= t2.FID
                        Left Join t_Items t3 On t1.FDeptID = t3.FID
                        Where t1.FScheduleID='{0}'";
                sql = string.Format(sql, scheduleID);
                

                dt = runner.ExecuteSql(sql);
                pNode = doc.SelectSingleNode("GetScheduleDetail/ClientList");
                
                foreach (DataRow row in dt.Rows)
                {
                    XmlNode cNode = doc.CreateElement("DataRow");
                    
                     pNode.AppendChild(cNode);
                  
                    vNode = doc.CreateElement("ClientID");
                    vNode.InnerText = row["FClientID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("DeptID");
                    vNode.InnerText = row["FDeptID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("Aims");
                    vNode.InnerText = row["FAims"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("Remark");
                    vNode.InnerText = row["FRemark"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("ClientName");
                    vNode.InnerText = row["FClientName"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("DeptName");
                    vNode.InnerText = row["FDeptName"].ToString();
                    cNode.AppendChild(vNode);

                }
            }

            result = doc.InnerXml;
            return result;
        }
        public string  GetMyList(string xmlString)
        {
            string result = "", val = "", filter = "", employeeID = "", dateFilter = "";

            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetMyScheduleList/ID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        filter = filter.Trim().Length > 0 ? filter + "  and t1.FExcutorID ='" + val + "'" : "  t1.FExcutorID ='" + val + "'";
                        employeeID = val;
                    }
                }
                else
                    throw new Exception("员工ID不能为空");

                vNode = doc.SelectSingleNode("GetMyScheduleList/StartDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        dateFilter = dateFilter.Length > 0 ? dateFilter + " And  t2.FStartTime >= '" + val + "  0:0:0.000'" : " t2.FStartTime >= '" + val + "  0:0:0.000'";
                }

                vNode = doc.SelectSingleNode("GetMyScheduleList/EndDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        dateFilter = dateFilter.Length > 0 ? dateFilter + " And  t2.FEndTime <= '" + val + "  23:59:59.999'" : " t2.FEndTime <= '" + val + "  23:59:59.999'";
                }

                vNode = doc.SelectSingleNode("GetMyScheduleList/Type");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0 && val != "99")
                        filter = filter.Length > 0 ? filter + " and  t2.FType In ('" + val.Replace("|", "','") + "')" : " t2.FType In ('" + val.Replace("|", "','") + "')";
                }
                filter = filter + (dateFilter.Length > 0 ? " and " + dateFilter:"");
                string sql = "SELECT (Left(CONVERT(varchar(100), t2.FStartTime, 108),5) +'~'+ Left(CONVERT(varchar(100), t2.FEndTime, 108),5)) As TimeString," +
                                " t1.FExcutorID,t3.FName AS FExcutorName,t2.FSubject As SubjectString ,t1.FScheduleID As FID,t2.FInstitutionID As FInstitutionID,t4.FName As InstitutionName" +
                                " FROM ScheduleExecutor t1" +
                                " Left Join Schedule t2 On t1.FScheduleID= t2.FID" +
                                " Left Join t_Items t3 On t1.FExcutorID= t3.FID" +
                                " Left Join t_Items t4 On t4.FID= t2.FInstitutionID";
                if (filter.Length > 0)
                    sql = sql + "  Where " + filter ;
             
                 sql = sql + " order by t2.FStartTime Desc ";
               
                //sql = sql + " Union (";
                //sql = sql + "SELECT (Left(CONVERT(varchar(100), t2.FStartTime, 108),5) +'~'+ Left(CONVERT(varchar(100), t2.FEndTime, 108),5)) As TimeString," +
                //            " t2.FCoachID,t3.FName AS FExcutorName,Isnull(t4.FName,'') +'：'+ t2.FSubject As SubjectString ,t2.FID,t2.FInstitutionID As FInstitutionID,t4.FName As InstitutionName" +
                //            " FROM Schedule t2"+
                //            " Left Join t_Items t3 On t2.FCoachID= t3.FID"+
                //            " Left Join t_Items t4 On t4.FID= t2.FInstitutionID ";
                //sql = sql + " where t2.FCoachID='" + employeeID+"'";

                //if(dateFilter.Length >0)
                //    sql = sql + "  and  " + dateFilter + " order by t2.FStartTime Desc )";
                //else
                //    sql = sql + " order by t2.FStartTime Desc)";
                
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetMyScheduleList", "", "List");
               

            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }


        //public DataTable GetTeamList(string leaderId, DateTime beginDate, DateTime endDate, string type = "99")
        public string GetTeamList(string xmlString)
        {

            string result = "", employeeIDs = "", val = "", filterString = "", dateFilter = "";
            result = "<GetTeamScheduleList>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetTeamScheduleList>";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetTeamScheduleList/EmployeeIDs");
                if (vNode == null||vNode.InnerText.Trim().Length==0)//没有设置下属ID时，通过LeaderID来获取其下属的IDs
                {
                    vNode = doc.SelectSingleNode("GetTeamScheduleList/LeaderID");
                    if (vNode == null || vNode.InnerText.Trim().Length ==0)
                    {
                        throw new Exception("团队领导ID不能为空");
                    }
                    else
                    {
                        val = vNode.InnerText.Trim();
                        WorkShip w = new WorkShip();
                        string xmlParam = "<GetTeamMembers><LeaderID>" + val + "</LeaderID></GetTeamMembers>";
                        employeeIDs = w.GetTeamMemberIDs(xmlParam);
                        if (employeeIDs.Length == 0)//没有直接下属，直接返回
                            return result;
                        else
                            filterString = filterString.Trim().Length > 0 ? filterString + "  and t1.FExcutorID In ('" + employeeIDs.Replace("|", "','") + "')" : "  t1.FExcutorID In ('" + employeeIDs.Replace("|", "','") + "')";
                    }
                }
                else//已设置了相应的下属IDs，直接读取
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filterString = filterString.Trim().Length > 0 ? filterString + "  and t1.FExcutorID In ('" + val.Replace("|", "','") + "')" : "  t1.FExcutorID In ('" + val.Replace("|", "','") + "')";
                }



                vNode = doc.SelectSingleNode("GetTeamScheduleList/BeginDate");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        dateFilter = dateFilter.Length > 0 ? dateFilter + " And  t2.FStartTime >= '" + val + "  0:0:0.000'" : " t2.FStartTime >= '" + val + "  0:0:0.000'";
                }

                vNode = doc.SelectSingleNode("GetTeamScheduleList/FSEndTime");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        dateFilter = dateFilter.Length > 0 ? dateFilter + " And  t2.FEndTime <= '" + val + "  23:59:59.999'" : " t2.FSEndTime <= '" + val + "  23:59:59.999'";
                }


                vNode = doc.SelectSingleNode("GetTeamScheduleList/Type");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0 && val != "99")
                    {
                        ///将类型代码换为ID
                        val = TypeNumber2ID(val);
                        filterString = filterString.Length > 0 ? filterString + " and  t2.FType In ('" + val.Replace("|", "','") + "')" : " t2.FType In ('" + val.Replace("|", "','") + "')";
                    }
                }

                if (dateFilter.Length > 0)
                    filterString = filterString + (filterString.Trim().Length > 0 ? " and  " + dateFilter : dateFilter);

                string sql = "SELECT (Left(CONVERT(varchar(100), t2.FStartTime, 108),5) +'~'+ Left(CONVERT(varchar(100), t2.FEndTime, 108),5)) As TimeString," +
                        " t1.FExcutorID,t3.FName AS FExcutorName,Isnull(t4.FName,'') +':'+ t2.FSubject As SubjectString ,t1.FScheduleID" +
                        " FROM ScheduleExecutor t1" +
                        " Left Join Schedule t2 On t1.FScheduleID= t2.FID" +
                        " Left Join t_Items t3 On t1.FExcutorID= t3.FID" +
                        " Left Join t_Items t4 On t4.FID= t2.FInstitutionID";
                if (filterString.Length >0)
                    sql = sql + " Where " + filterString;
                sql = sql + " order by t2.FStartTime Desc";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetTeamScheduleList", "", "List");
                
               
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }


        public string Update(string dataString)
        {
            string id = "", sql = "", employeeID = "", valueString = "",val="",bDate="",eDate="",executorList="";

            try
            {
               
                SQLServerHelper runner = new SQLServerHelper();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataString);

                XmlNode vNode = doc.SelectSingleNode("UpdateScheduleData/FEmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("日程创建者ID不能为空");

                vNode = doc.SelectSingleNode("UpdateScheduleData/FSubject");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("日程主题不能为空");

                vNode = doc.SelectSingleNode("UpdateScheduleData/FStartTime");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("开始时间不能为空");
                else
                {
                    bDate= vNode.InnerText;
                    DateTime beginDate = DateTime.Parse(bDate);
                    if (beginDate.Date < DateTime.Now.Date)
                        throw new Exception("不能新建或修改今天以前的日程安排");

                    bDate= vNode.InnerText;
                    valueString = valueString + "FStartTime='" + bDate + "',";

                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FEndTime");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("结束时间不能为空");
                else
                {
                    eDate = vNode.InnerText;
                    valueString = valueString + "FEndTime='" + eDate + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FEmployeeID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    employeeID = val;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FEmployeeID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/ExcutorIDList");
                if (vNode != null)
                {
                    executorList = vNode.InnerText.Trim();
                }

                DataTable dt = null;
                vNode = doc.SelectSingleNode("UpdateScheduleData/ID");
                if (vNode != null)
                {
                    id = vNode.InnerText;
                    if (id.Trim() == "" || id.Trim() == "-1")//新增日程
                    {

                        //sql = "Select t1.FID" +
                        //      " From  ScheduleExecutor t1" +
                        //      " Left Join Schedule t2 On t1.FScheduleID=t2.FID" +
                        //      " Where (t2.FStartTime <= '" + bDate + "' and  '" + bDate + "' < =t2.FEndTime) or (t2.FStartTime <= '" + eDate + "' and  '" + eDate + "' <= t2.FEndTime) ";
                        //sql = sql + " and t1.FExcutorID In ('" + (executorList.Length > 0 ? executorList.Replace("|", "','") : employeeID) + "')";
                        //dt = runner.ExecuteSql(sql);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    throw new Exception("执行人日程时间有冲突，请调整执行人或日程起止时间");
                        //}

                        id = Guid.NewGuid().ToString();
                        sql = "Insert into Schedule(FID) Values('" + id + "') ";
                        if (runner.ExecuteSqlNone(sql) < 0)//插入新日程失败
                            throw new Exception("新增失败");
                    }
                }
                else
                    throw new Exception("ID节点缺失");

                //更新日程信息
                vNode = doc.SelectSingleNode("UpdateScheduleData/FSubject");
                
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSubject='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FType");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FType='" + val + "',";
                }

                //创建时间
                valueString = valueString + "FDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',";

                vNode = doc.SelectSingleNode("UpdateScheduleData/FInstitutionType");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInstitutionType='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FInstitutionID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FInstitutionID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FDepartmentID_Ins");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDepartmentID_Ins='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FClientID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FClientID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FActivity");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    //if (val.Trim().Length > 0)
                        valueString = valueString + "FActivity='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateScheduleData/FReminderTime");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FReminderTime='" + val + "',";
                }

               

                vNode = doc.SelectSingleNode("UpdateScheduleData/FRemark");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    //if (val.Trim().Length > 0)
                    valueString = valueString + "FRemark='" + val + "',";
                }
                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update Schedule Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新日程失败
                    {
                        throw new Exception("更新失败");
                    }
                }
                //更新协访人
                vNode = doc.SelectSingleNode("UpdateScheduleData/FCoachID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    //if (val.Trim().Length > 0)
                    valueString = valueString + "FCoachID='" + val + "',";
                    val = vNode.InnerText;
                    
                    sql = "Delete from ScheduleExecutor Where  FType=2 And FScheduleID ='" + id + "' and FIsExcuted=0";//删除尚未执行的
                    runner.ExecuteSqlNone(sql);
                    string[] coachs = val.Split(new[] { '|' });
                    for (int i = 0; i < coachs.Length; ++i)
                    {
                        sql = "Insert Into ScheduleExecutor(FScheduleID,FExcutorID,FIsExcuted,FType) Values(" + "'" + id + "','" + coachs[i] + "',0,2)";
                        runner.ExecuteSqlNone(sql).ToString();
                    }
                    

                }
                //更新执行者
                vNode = doc.SelectSingleNode("UpdateScheduleData/FExcutorIDList");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                    {
                        sql = "Delete from ScheduleExecutor Where  FType=1 And   FScheduleID ='" + id + "' and FIsExcuted=0";//删除尚未执行的
                        runner.ExecuteSqlNone(sql);
                        string[] executors = val.Split(new[] { '|' });
                        for (int i = 0; i < executors.Length; ++i)
                        {
                            sql = "Insert Into ScheduleExecutor(FScheduleID,FExcutorID,FIsExcuted,FType) Values(" + "'" + id + "','" + executors[i] + "',0,1)";
                            runner.ExecuteSqlNone(sql).ToString();
                        }
                    }
                    else//执行者为空，创建者为执行者
                    {
                        sql = "Select FExcutorID from ScheduleExecutor Where FExcutorID='" + employeeID + "' and FScheduleID='" + id + "'";
                        dt = runner.ExecuteSql(sql);
                        if (dt.Rows.Count == 0)
                        {
                            sql = "Insert Into ScheduleExecutor(FScheduleID,FExcutorID,FIsExcuted) Values(" + "'" + id + "','" + employeeID + "',0)";
                            runner.ExecuteSqlNone(sql).ToString();
                        }
                    }
                }
                else//执行者为空(没有设置此节点），创建者为执行者
                {
                    sql = "Select FExcutorID from ScheduleExecutor Where FType=1 And FExcutorID='" + employeeID + "' and FScheduleID='" + id + "'";
                    dt = runner.ExecuteSql(sql);
                    if (dt.Rows.Count == 0)
                    {
                        sql = "Insert Into ScheduleExecutor(FScheduleID,FExcutorID,FIsExcuted,FType) Values(" + "'" + id + "','" + employeeID + "',0,1)";
                        runner.ExecuteSqlNone(sql).ToString();
                    }
                }
                vNode = doc.SelectSingleNode("UpdateScheduleData/ClientList");//保存拜访客户
                if (vNode != null)
                {
                    string[] depts = doc.SelectSingleNode("UpdateScheduleData/ClientList/DeptID").InnerText.Split(new[] { '|' });
                    string[] clients = doc.SelectSingleNode("UpdateScheduleData/ClientList/ClientID").InnerText.Split(new[] { '|' });
                    string[] aims = doc.SelectSingleNode("UpdateScheduleData/ClientList/Aims").InnerText.Split(new[] { '|' });
                    
                    val = vNode.InnerText;
                    sql = "Delete from ScheduleClients Where   FScheduleID ='" + id + "' ";//删除已存在的
                    runner.ExecuteSqlNone(sql);
                    //string[] executors = val.Split(new[] { '|' });
                    for (int i = 0; i < depts.Length; ++i)
                    {
                        sql = "Insert Into ScheduleClients(FScheduleID,FDeptID,FClientID,FAims,FRemark) Values('{0}','{1}','{2}','{3}','{4}')";
                        sql = string.Format(sql, id, depts[i],clients[i], aims[i], "");

                        runner.ExecuteSqlNone(sql).ToString();
                    }
                    

                }

            }
            catch(Exception err)
            {
                id = "-1";
                throw   err;
            }
            return id;
        }

        public string Delete(string xmlString)
        {
            string result = "", scheduleID = "-1";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("DeleteSchedule/ID");
                if (vNode == null || vNode.InnerText.Length == 0)
                    throw new Exception("日程ID不能为空");
                else
                    scheduleID = vNode.InnerText.Trim();

                string sql = "Delete from Schedule Where FID = '" + scheduleID + "'  Delete from ScheduleExecutor Where FScheduleID = '" + scheduleID + "'";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSqlNone(sql).ToString();
            }
            catch(Exception err)
            {
                scheduleID = "-1";
                throw err;
            }
            result = scheduleID;
            return result;
        }
        private string TypeNumber2ID(string val)
        {
            string result = "";
            string[] vals = val.Split('|');

            for (int i = 0; i < vals.Length;++i )
            {
                switch(vals[i])
                {
                    case "01":
                        vals[i] = vals[i].Replace("01", "8c7e241a-8167-48e5-8c02-637e8cd77050");
                        break;
                    case "02":
                        vals[i] = vals[i].Replace("02", "73e0f637-451f-4f86-a016-27aab3e1a2cf");
                        break;
                    case "03":
                        vals[i] = vals[i].Replace("03", "520cf28c-efbf-4c25-99aa-f6a8ad3b7407");
                        break;
                    case "04":
                        vals[i] = vals[i].Replace("04", "34b4bc2f-cfbd-46cf-89be-a8853dc27280");
                        break;
                }

            }
            for (int i = 0; i < vals.Length; ++i)
            {
                result = result + vals[i] + "|";
            }
            if (result.Length > 0)
                result = result.Substring(0, result.Length - 1);

            return result;
  
        }
    }
}
