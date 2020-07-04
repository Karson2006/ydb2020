using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;
using System.Globalization;

namespace ydb.BLL
{
    public class WorkReport
    {
        public WorkReport()
        {

        }

        #region UpdateDaily
        public string UpdateDaily(string xmlString)
        {
            string sql = "", employeeid = "-1", date = "", summary = "", plan = "", id = "-1";
            DataTable tb = null;

            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                DataTable dt = new DataTable();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("UpdateWorkReport/EmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("报告人ID不能为空");
                else
                {
                    employeeid = vNode.InnerText;
                }
              
                vNode = doc.SelectSingleNode("UpdateWorkReport/Summary");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    summary = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateWorkReport/Plan");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    plan = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateWorkReport/Date");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("日报日期不能为空");
                else//有日期
                {
                    date = vNode.InnerText;
                    
                    DateTime rptDate = DateTime.Parse(date);
                    date = rptDate.ToString("yyyy-MM-dd");
                    TimeSpan ts = rptDate.Subtract(DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd")));
                    if(ts.Days<-1)
                        throw new Exception("只能补录昨天的工作总结");
                    else if (ts.Days > 7)//计划不需要控制
                        throw new Exception("只能提前一周填写工作计划");
                    else if (ts.Days==0)//今天
                    {
                        if (summary.Trim().Length == 0 && plan.Trim().Length == 0)
                            throw new Exception("今日工作计划或总结不能同时为空");
                    }
                    else if (ts.Days == -1)//昨天
                    {
                        if (summary.Trim().Length == 0)
                            throw new Exception("工作总结不能为空");
                        plan = "";
                    }
                    else if(ts.Days==1)//明天
                    {

                        if (plan.Trim().Length == 0)
                            throw new Exception("工作计划不能为空");
                        summary = "";
                    }

                    sql = "Select FID from WorkReport_Daily Where FEmployeeID='" + employeeid + "' and FDate between'" + date + "' and '" + date + "'";
                    tb = runner.ExecuteSql(sql);
                    if (tb.Rows.Count > 0)//已存在
                    {
                        id = tb.Rows[0]["FID"].ToString();
                        if (ts.Days == -1)//昨天
                        {
                            sql = "Update WorkReport_Daily Set FDeleted=0, FSummary='" + summary + "' Where  FID ='" + id + "'";
                        }
                        else if (ts.Days == 0) //今天
                        {
                            sql = "Update WorkReport_Daily Set FDeleted=0,FPlan='" + plan + "', FSummary='" + summary + "' Where  FID ='" + id + "'";
                        }
                        else if (ts.Days == 1)//明天
                        {
                            sql = "Update WorkReport_Daily Set FDeleted=0,FPlan='" + plan + "' Where  FID ='" + id + "'";
                        }
                        runner.ExecuteSqlNone(sql);
                    }
                    else
                    {
                        id = Guid.NewGuid().ToString();
                        sql = "Insert Into WorkReport_Daily(FID,FDate,FPlan,FSummary,FEmployeeID)Values('{0}','{1}','{2}','{3}','{4}')";
                        sql = string.Format(sql, id, date, plan, summary, employeeid);
                        runner.ExecuteSqlNone(sql);
                    }
                }

            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }

            return id;
        }

        #endregion

        #region UpdateWeek
        public string UpdateWeek(string xmlString)
        {
            string sql = "", employeeid = "-1",  summary = "", plan = "", id = "-1";
            DataTable tb = null;
            int weekIdx = 0,year=0;

            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                DataTable dt = new DataTable();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("UpdateWorkReport/EmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("报告人ID不能为空");
                else
                {
                    employeeid = vNode.InnerText;
                }

                vNode = doc.SelectSingleNode("UpdateWorkReport/Summary");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    summary = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateWorkReport/Plan");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    plan = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateWorkReport/WeekIndx");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("周报周参数不能为空");
                else
                {
                    GregorianCalendar gc = new GregorianCalendar();
                    year = DateTime.Now.Year;

                    int   weekOfYear   =   gc.GetWeekOfYear(DateTime.Now,   System.Globalization.CalendarWeekRule.FirstDay,   DayOfWeek.Monday);
                    int maxweekOfYear = gc.GetWeekOfYear(DateTime.Parse(year.ToString() +  "-" + DateTime.Now.ToString("MM-dd")),System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    
                    weekIdx =int.Parse( vNode.InnerText);
                    weekOfYear = weekOfYear + weekIdx;

                    if (weekIdx == 0)//本周
                    {
                        
                    }
                    else if (weekIdx == -1)//上周
                    {
                        if (weekIdx==0)
                        {
                            year = year - 1;
                            weekOfYear = gc.GetWeekOfYear(DateTime.Parse(year.ToString() + "-" + DateTime.Now.ToString("MM-dd")), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        }
                        plan = "";
                        
                    }
                    else if (weekIdx == 1)//下周
                    {
                        if (weekOfYear==maxweekOfYear)
                        {
                            weekOfYear = 1;
                            year = year + 1;
                        }
                        summary = "";
                    }


                    sql = "Select FID from WorkReport_Week Where FEmployeeID='" + employeeid + "' and  FWeekIndxOfYear =" + weekOfYear.ToString() + " And FYear=" + year.ToString();
                    tb = runner.ExecuteSql(sql);
                    if (tb.Rows.Count > 0)//已存在
                    {
                        id = tb.Rows[0]["FID"].ToString();

                        if (weekIdx == 0)//本周
                        {
                            sql = "Update WorkReport_Week Set FDeleted=0,FPlan='" + plan + "',FSummary='" + summary + "' Where FID = '" + id + "'";
                        }
                        else if (weekIdx == -1)//上周
                        {
                            sql = "Update WorkReport_Week Set FDeleted=0,FSummary='" + summary + "' Where FID = '" + id + "'";
                        }
                        else if (weekIdx == 1)//下周
                        {
                            sql = "Update WorkReport_Week Set FDeleted=0,FPlan='" + plan + "' Where FID = '" + id + "'";
                        }

                       
                        runner.ExecuteSqlNone(sql);
                    }
                    else
                    {
                        id = Guid.NewGuid().ToString();
                        sql = "Insert Into WorkReport_Week(FID,FWeekIndxOfYear,FPlan,FSummary,FEmployeeID,FYear) Values('{0}','{1}','{2}','{3}','{4}','{5}')";
                        sql = string.Format(sql, id, weekOfYear, plan, summary,  employeeid, year);
                        runner.ExecuteSqlNone(sql);
                    }
                }


            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }

            return id;
        }

        #endregion

        #region UpdateMonth
        public string UpdateMonth(string xmlString)
        {
            string sql = "", employeeid = "-1",  summary = "", plan = "", id = "-1";
            DataTable tb = null;
            int monthIdx = 0, year = 0;
            //string sMonth="";

            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                DataTable dt = new DataTable();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("UpdateWorkReport/FEmployeeID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("报告人ID不能为空");
                else
                {
                    employeeid = vNode.InnerText;
                }

                vNode = doc.SelectSingleNode("UpdateWorkReport/Summary");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    summary = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateWorkReport/Plan");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    plan = vNode.InnerText;

                vNode = doc.SelectSingleNode("UpdateWorkReport/MonthIndx");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("月报月参数不能为空");
                else
                {
                    int month = DateTime.Now.Month;

                    monthIdx = int.Parse(vNode.InnerText);
                    month = month + monthIdx;
                    year = DateTime.Now.Year;
                    
                    if (monthIdx == 0)//本月
                    {
                        if (plan.Trim().Length == 0 && summary.Trim().Length == 0)
                            throw new Exception("工作计划和总结不同同时为空");
                    }
                    else if (monthIdx == -1)//上月
                    {
                        if(month ==0)
                        {
                            year = year - 1;
                            month =12;
                        }
                        plan = "";
                    }
                    else if (monthIdx == 1)//下周
                    {
                        if (month == 13) 
                        {
                            month = 1;
                            year = year + 1;
                        }
                        summary = "";
                    }
                    //sMonth="0"+ month.ToString();
                    //sMonth = sMonth.Substring(sMonth.Length - 2, 2);

                    if (monthIdx == 0)//本月
                    {
                        //sql = "Update WorkReport_Month Set FDeleted=0,FPlan='" + plan + "',FSummary = '" + summary + "' Where FID='" + id + "'";
                    }
                    else if (monthIdx == -1)//上月
                    {
                        plan = "";
                        //sql = "Update WorkReport_Month Set FDeleted=0,FSummary = '" + summary + "' Where FID='" + id + "'";
                    }
                    else if (monthIdx == 1)//下月
                    {
                        summary = "";
                        //sql = "Update WorkReport_Month Set FDeleted=0,FPlan='" + plan + "' Where FID='" + id + "'";
                    }

                    sql = "Select FID from WorkReport_Month Where FEmployeeID='" + employeeid + "' and  FMonth =" + month.ToString()  + " And FYear=" + year.ToString();
                    tb = runner.ExecuteSql(sql);
                    if (tb.Rows.Count > 0)//已存在
                    {
                        id = tb.Rows[0]["FID"].ToString();
                        sql = "Update WorkReport_Month Set FDeleted=0,FPlan='" + plan + "',FSummary = '" + summary + "' Where FID='" + id + "'";
                        runner.ExecuteSqlNone(sql);
                    }
                    else
                    {
                        id = Guid.NewGuid().ToString();
                        sql = "Insert Into WorkReport_Month(FID,FMonth,FPlan,FSummary,FEmployeeID,FYear) Values('{0}','{1}','{2}','{3}','{4}','{5}')";
                        sql = string.Format(sql, id, month, plan, summary, employeeid, year);
                        runner.ExecuteSqlNone(sql);
                    }
                }

            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }

            return id;
        }

        #endregion

        #region GetMonthList
        public string GetMonthList(string xmlString)
        {
            string sql = "", where = "", val = "", month1 = "", month2 = "", year1 = "", year2 = "";
            string result = "";
            
            result = "<GetWorkReportList>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetWorkReportList>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetWorkReportList/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    where = where.Trim().Length == 0 ? " t1.FEmployeeID In('" + val.Replace("|", "','") + "')" : where + " and t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetWorkReportList/BeginMonth");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    //year1 = val.Split('-')[0];
                    //month1 = val.Split('-')[1];
                    DateTime sDate = DateTime.Parse(val);
                    year1 = sDate.ToString("yyyy");
                    month1 = sDate.ToString("MM");
                    if (where.Trim().Length > 0)
                    {
                        where = where + " And t1.FMonth >=" +int.Parse( month1).ToString() + " and t1.FYear >=" + year1 + "";
                    }
                    else
                        where = " t1.FMonth >=" + int.Parse(month1).ToString() + " and t1.FYear >=" + year1 + "";
                }

                vNode = doc.SelectSingleNode("GetWorkReportList/EndMonth");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    //year2 = val.Split('-')[0];
                    //month2 = val.Split('-')[1];
                    DateTime eDate = DateTime.Parse(val);
                    year2 = eDate.ToString("yyyy");
                    month2 = eDate.ToString("MM");
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FMonth <=" + int.Parse(month2).ToString() + " and t1.FYear <=" + year2 + "";
                    else
                        where = " t1.FMonth <=" + int.Parse(month2).ToString() + " and t1.FYear <=" + year2 + "";
                }

              

                sql = "	Select t1.FID,t1.FEmployeeID,Isnull(t2.FName,'') As FEmployeeName,t1.FPlan,t1.FSummary,t1.FMonth,t1.FYear"+
	                " From WorkReport_Month t1"+
	                " Left Join t_Items t2 On t1.FEmployeeID = t2.FID";
                if(where.Length>0)
                    sql = sql + " Where " +  where;
                sql = sql + " Order by t1.FEmployeeID,t1.FYear desc,t1.FMonth Desc";

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetWorkReportList", "", "List");

                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetWorkReportList/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode("Rpt003", row.SelectSingleNode("ID").InnerText, ref cNode, ref doc);
                    pNode.AppendChild(cNode);
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

        #region GetMonthListByIndex
        public string GetMonthListByIndex(string xmlString)
        {
            string sql = "", where = "", val = "";
            int month1 = 0, year1 = 0;
            string result = "";

            result = "<GetWorkReportList>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetWorkReportList>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetWorkReportListByIndex/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    where = where.Trim().Length == 0 ? " t1.FEmployeeID In('" + val.Replace("|", "','") + "')" : where + " and t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetWorkReportListByIndex/MonthIndex");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    int monthIdx = int.Parse(val);
                    month1 = DateTime.Now.Month ;

                    year1 = DateTime.Now.Year;

                    if (monthIdx> 1)//无效参数
                    {
                        throw new Exception("无效的MonthIndex参数");
                    }
                    else if (monthIdx == -1)//上月
                    {
                        if (month1 == 1)
                        {
                            year1 = year1 - 1;
                            month1 = 12;
                        }
                        else
                        {
                            month1 = month1 - 1;
                        }
                    }
                    else if (monthIdx == 1)//下周
                    {
                        if (month1 == 12)
                        {
                            month1 = 1;
                            year1 = year1 + 1;
                        }
                        else
                        {
                            month1 = month1 + 1;
                        }
                       
                    }
                   
                    if (where.Trim().Length > 0)
                    {
                        where = where + " And t1.FMonth =" + month1.ToString() + " and t1.FYear =" + year1.ToString()  + "";
                    }
                    else
                        where = " t1.FMonth =" + month1.ToString() + " and t1.FYear =" + year1.ToString() + "";
                }
                

                
                sql = "	Select t1.FID,t1.FEmployeeID,Isnull(t2.FName,'') As FEmployeeName,t1.FPlan,t1.FSummary,t1.FMonth,t1.FYear" +
                    " From WorkReport_Month t1" +
                    " Left Join t_Items t2 On t1.FEmployeeID = t2.FID";
                if (where.Length > 0)
                    sql = sql + " Where " + where;
                sql = sql + " Order by t1.FEmployeeID,t1.FYear desc,t1.FMonth Desc";

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetWorkReportList", "", "List");

                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetWorkReportList/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode("Rpt003", row.SelectSingleNode("ID").InnerText, ref cNode, ref doc);
                    pNode.AppendChild(cNode);
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
        #region GetWeekList
        public string GetWeekList(string xmlString)
        {
            string sql = "", where = "", val = "",week1="",week2="",year1="",year2="";
            string result = "";

            result = "<GetWorkReportList>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetWorkReportList>";
            try
            {
                GregorianCalendar gc = new GregorianCalendar();
                int weekOfYear;

                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetWorkReportList/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();

                    where = where.Trim().Length ==0? " t1.FEmployeeID In('" + val.Replace("|", "','") + "')": where + " And t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetWorkReportList/BeginWeek");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    weekOfYear = gc.GetWeekOfYear(DateTime.Parse(val), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    week1 = weekOfYear.ToString();
                    year1 = DateTime.Parse(val).Year.ToString();
                    where = where.Trim().Length == 0 ? "  t1.FWeekIndxOfYear >=" + week1 + " and t1.FYear >=" + year1 : where + " and t1.FWeekIndxOfYear >=" + week1 + " and t1.FYear >=" + year1;
                }

                vNode = doc.SelectSingleNode("GetWorkReportList/EndWeek");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    weekOfYear = gc.GetWeekOfYear(DateTime.Parse(val), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    year2 = DateTime.Parse(val).Year.ToString();
                    week2 = weekOfYear.ToString();

                    where = where.Trim().Length == 0 ? "  t1.FWeekIndxOfYear <=" + week2 + " and t1.FYear <=" + year2 : where + " and t1.FWeekIndxOfYear <=" + week2 + " and t1.FYear <=" + year2;
                }

                sql = "	Select t1.FID,t1.FEmployeeID,Isnull(t2.FName,'') As FEmployeeName,t1.FPlan,t1.FSummary,t1.FWeekIndxOfYear,t1.FYear"+
                      " From WorkReport_Week t1" +
	                  " Left Join t_Items t2 On t1.FEmployeeID = t2.FID";
                if (where.Length > 0)
                    sql = sql + " Where " + where;
                sql = sql + " Order by t1.FEmployeeID,t1.FYear desc,t1.FWeekIndxOfYear Desc";

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetWorkReportList", "", "List");
                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetWorkReportList/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode("Rpt002", row.SelectSingleNode("ID").InnerText, ref cNode, ref doc);
                    pNode.AppendChild(cNode);
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

        #region GetWeekListByIndx
        public string GetWeekListByIndx(string xmlString)
        {
            string sql = "", where = "", val = "";
            string result = "";
            int week1 = 0, year1 = 0;

            result = "<GetWorkReportList>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetWorkReportList>";
            try
            {
                GregorianCalendar gc = new GregorianCalendar();
                int weekOfYear;

                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetWorkReportListByIndex/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();

                    where = where.Trim().Length == 0 ? " t1.FEmployeeID In('" + val.Replace("|", "','") + "')" : where + " And t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }
                else
                {
                    throw new Exception("FEmployeeID不能为空");
                }

                vNode = doc.SelectSingleNode("GetWorkReportListByIndex/WeekIndex");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    weekOfYear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    year1 = DateTime.Now.Year;
                    week1 = weekOfYear;
                    if (val == "-1")//上个月
                    {
                        if (weekOfYear == 1)
                        {
                            week1 = 12;
                            year1 = DateTime.Now.Year - 1;
                        }
                        else
                        {
                            week1 = weekOfYear - 1;
                        }
                    }
                    if (val == "1")//下月
                    {
                        string lastDateOfYear = DateTime.Now.Year.ToString() + "-12-31";
                        int maxWeekofYear = gc.GetWeekOfYear(DateTime.Parse(lastDateOfYear), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                        if (weekOfYear == maxWeekofYear)
                        {
                            week1 = 1;
                            year1 = DateTime.Now.Year +1;
                        }
                        else
                        {
                            week1 = weekOfYear +1;
                        }
                    }
                    
                    where = where.Trim().Length == 0 ? "  t1.FWeekIndxOfYear =" + week1.ToString() + " and t1.FYear =" + year1.ToString() : where + " and t1.FWeekIndxOfYear =" + week1.ToString() + " and t1.FYear =" + year1.ToString();
                }
                else
                {
                    throw new Exception("WeekIndex不能为空");
                }

               

                sql = "	Select t1.FID,t1.FEmployeeID,Isnull(t2.FName,'') As FEmployeeName,t1.FPlan,t1.FSummary,t1.FWeekIndxOfYear,t1.FYear" +
                      " From WorkReport_Week t1" +
                      " Left Join t_Items t2 On t1.FEmployeeID = t2.FID";
                if (where.Length > 0)
                    sql = sql + " Where " + where;
                

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetWorkReportList", "", "List");
                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetWorkReportList/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode("Rpt002", row.SelectSingleNode("ID").InnerText, ref cNode, ref doc);
                    pNode.AppendChild(cNode);
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

        #region GetDailyList
        public string GetDailyList(string xmlString)
        {
            string sql = "", where = "", val = "";
            string result = "";

            result = "<GetWorkReportList>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetWorkReportList>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetWorkReportList/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    where = " t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetWorkReportList/BeginDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    string sDate = vNode.InnerText.Trim();
                    sDate = DateTime.Parse(sDate).ToString("yyyy-MM-dd");
                    val = "'" + sDate + " 0:0:0.000'";
                    if(where.Trim().Length >0)
                        where = where + " And t1.FDate >=" + val;
                    else
                        where = where + " t1.FDate >" + val;
                }

                vNode = doc.SelectSingleNode("GetWorkReportList/EndDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    string eDate = vNode.InnerText.Trim();
                    eDate = DateTime.Parse(eDate).ToString("yyyy-MM-dd");
                    val = "'" + eDate + " 23:59:59.999'";
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FDate <" + val;
                    else
                        where = where + " t1.FDate <=" + val;
                }

                sql = "	Select t1.FID,t1.FEmployeeID,Isnull(t2.FName,'') As FEmployeeName,t1.FPlan,t1.FSummary,t1.FDate"+
                      " From WorkReport_Daily t1" +
	                  " Left Join t_Items t2 On t1.FEmployeeID = t2.FID";
                if (where.Length > 0)
                    sql = sql + " Where " + where;
                sql = sql + " Order by t1.FEmployeeID,t1.FDate Desc";

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetWorkReportList", "", "List");
                //加载图片
                doc.LoadXml(result);
                XmlNodeList rows = doc.SelectNodes("GetWorkReportList/DataRows/DataRow");
                foreach (XmlNode row in rows)
                {
                    XmlNode pNode = row;
                    XmlNode cNode = doc.CreateElement("Iamges");
                    Common.SetImageXmlNode ("Rpt001", row.SelectSingleNode("ID").InnerText, ref cNode ,ref doc);
                    pNode.AppendChild(cNode);
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

  
        #region GetPerformanceList
        public string GetPerformanceList(string xmlString)
        {
            string sql = "", where = "", val = "",month1="",month2="",year1="",year2="";
            string result = "";

            result = "<GetPerformanceList>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetPerformanceList>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetPerformanceList/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    where = " t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetPerformanceList/BeginMonth");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    year1 = val.Split('-')[0];
                    month1 = val.Split('-')[1];
                    if (where.Trim().Length > 0)
                    {
                        where = where + " And t1.FMonth >='" + month1 +"' and t1.FYear >='"+  year1 +"'";
                    }
                    else
                        where = " t1.FMonth >='" + month1 + "' and t1.FYear >='" + year1 + "'";
                }

                vNode = doc.SelectSingleNode("GetPerformanceList/EndMonth");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    year2 = val.Split('-')[0];
                    month2 = val.Split('-')[1];
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FMonth <='" + month2+"' and t1.FYear <='" + year2+"'" ;
                    else
                        where = " t1.FMonth <='" + month2 + "' and t1.FYear <='" + year2 + "'";
                }

                vNode = doc.SelectSingleNode("GetPerformanceList/Spec");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FSpec='" + val+ "'";
                    else
                        where = " t1.FSpec='" +val + "'";
                }
                
                vNode = doc.SelectSingleNode("GetPerformanceList/ProductName");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FProductName like '%" + val + "%'";
                    else
                        where = where + " t1.FProductName like '%" + val + "%'";
                }
                                

                vNode = doc.SelectSingleNode("GetPerformanceList/FAmount");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = "'" + vNode.InnerText.Trim() + "'";
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FAmount >=" + val;
                    else
                        where = where + " t1.FAmount >=" + val;
                }

                sql = "Select t1.FEmployeeID,IsNull(t2.FName,'') As FEmployeeName,t1.FSpec,t1.FUnit,t1.FProductName,Sum(t1.FAmount) As FAmount,sum(t1.FQuantity) As FQuantity" +
                        " From SaleBillDetail t1" +
                        " Left Join t_Items t2 On t1.FEmployeeID= t2.FID";
                if (where.Length > 0)
                    sql = sql + " Where " + where;
                sql = sql + " Group by t1.FEmployeeID,t2.FName,t1.FSpec,t1.FUnit,t1.FProductName Order by t1.FEmployeeID,t1.FProductName,FAmount Desc";

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetPerformanceList", "","List");
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #endregion

        #region GetPerformanceDetail
        public string GetPerformanceDetail(string xmlString)
        {
            string sql = "", where = "", val = "",month1="",month2="",year1="",year2="";
            string result = "";

            result = "<GetPerformanceDetail>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetPerformanceDetail>";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode("GetPerformanceDetail/EmployeeID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    where = " t1.FEmployeeID In('" + val.Replace("|", "','") + "')";
                }

                vNode = doc.SelectSingleNode("GetPerformanceList/BeginMonth");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    year1 = val.Split('-')[0];
                    month1 = val.Split('-')[1];
                    if (where.Trim().Length > 0)
                    {
                        where = where + " And t1.FMonth >='" + month1 + "' and t1.FYear >='" + year1 + "'";
                    }
                    else
                        where = " t1.FMonth >='" + month1 + "' and t1.FYear >='" + year1 + "'";
                }

                vNode = doc.SelectSingleNode("GetPerformanceList/EndMonth");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    year2 = val.Split('-')[0];
                    month2 = val.Split('-')[1];
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FMonth <='" + month2 + "' and t1.FYear <='" + year2 + "'";
                    else
                        where = " t1.FMonth <='" + month2 + "' and t1.FYear <='" + year2 + "'";
                }

                vNode = doc.SelectSingleNode("GetPerformanceDetail/ProductName");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FProductName = '" + val + "'";
                    else
                        where = where + " t1.FProductName = '" + val + "'";
                }


                vNode = doc.SelectSingleNode("GetPerformanceDetail/FAmount");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = "'" + vNode.InnerText.Trim() + "'";
                    if (where.Trim().Length > 0)
                        where = where + " And t1.FAmount >=" + val;
                    else
                        where = where + " t1.FAmount >=" + val;
                }

                sql = "Select t1.FEmployeeID,IsNull(t2.FName,'') As FEmployeeName,t1.FSpec,t1.FUnit,t1.FProductName,Isnull(t3.FName,'') As FHospitalName,"+
                      " t1.FYear,t1.FMonth,Sum(t1.FAmount) As FAmount,sum(t1.FQuantity) As FQuantity" +
                      " From SaleBillDetail t1"+
                      " Left Join t_Items t2 On t1.FEmployeeID= t2.FID" +
                      " Left Join t_Items t3 On t1.FHospitalID= t3.FID";
                if (where.Length > 0)
                    sql = sql + " Where " + where;
                sql = sql + " Group by t1.FEmployeeID,t2.FName,t1.FSpec,t1.FUnit,t1.FProductName,t3.FName,t1.FYear,t1.FMonth Order by t1.FProductName, t1.FYear Desc,t1.FMonth Desc";

                DataTable dt = runner.ExecuteSql(sql);
                result = Common.DataTableToXml(dt, "GetPerformanceDetail", "","List");
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
