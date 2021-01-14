using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using iTR.Lib;
using System.Data;
using ydb.BLL;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Web.Services;
using System.Web;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

namespace ydb.Report
{
    public class CallRpt
    {
        public CallRpt()
        {
        }

        #region GetCallRepotr1

        /// <summary>
        /// 读取拜访汇总报表数据
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public string GetCallRepotr1(string xmlString)
        {
            string result = "", sql = "", date1 = "", date2 = "", employeeIds = "", employeeId = "";

            result = "<GetData>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetData>";
            date1 = DateTime.Now.ToString("yyyy-MM") + "-01";
            date2 = DateTime.Now.ToString("yyyy-MM-dd");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode node = doc.SelectSingleNode("GetData/BeginDate");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                date1 = node.InnerText.Trim();
            }
            node = doc.SelectSingleNode("GetData/EndDate");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                date2 = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("GetData/EmployeeID");//若为团队负责人，要读取其及直接下属的数据
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                employeeId = node.InnerText.Trim();
                //WorkShip ws = new WorkShip();
                //employeeIds = ws.GetAllMemberIDsByLeaderID(employeeId);
            }

            node = doc.SelectSingleNode("GetData/EmployeeIDList");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                employeeIds = node.InnerText.Trim();
                if (employeeIds == "99")//查询其所有下属
                {
                    WorkShip ws = new WorkShip();
                    employeeIds = ws.GetAllMemberIDsByLeaderID(employeeId);
                }
                employeeIds = employeeIds.Replace("|", "','");
            }
            if (employeeIds.Length == 0)
                employeeIds = employeeId;

            sql = @"Select FEmployeeName As EmployeeName,FEmployeeID As EmployeeID, sum(RouteCount) RouteCount,Sum(OKRouteCount) AS ValidRouteCount,Sum(CallCount) As CallCount,sum(unplanedCallCount) As UnplannedCallCount,
                    Sum(IsNull(FTimeSpan,0)) As TimeSpan
                    From Route_Call_View
                    Where FDate between '{0}' and  '{1}' and FEmployeeID In('{2}')
                    Group by FEmployeeName,FEmployeeID
                    Order by RouteCount Desc,CallCount Desc";
            sql = string.Format(sql, date1, date2, employeeIds);
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            result = Common.DataTableToXml(dt, "GetData", "", "List");
            return result;
        }

        #endregion GetCallRepotr1

        #region ExportCallReport

        public string ExportCallReport(string xmlString)
        {
            string result = "", sql = "", date1 = "", date2 = "", employeeIds = "", employeeId = "", savePath = "", fileName = "";

            result = "<GetData>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetData>";
            date1 = DateTime.Now.ToString("yyyy-MM") + "-01";
            date2 = DateTime.Now.ToString("yyyy-MM-dd");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode node = doc.SelectSingleNode("GetData/BeginDate");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                date1 = node.InnerText.Trim();
            }
            node = doc.SelectSingleNode("GetData/EndDate");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                date2 = node.InnerText.Trim();
            }

            node = doc.SelectSingleNode("GetData/EmployeeID");//若为团队负责人，要读取其及直接下属的数据
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                employeeId = node.InnerText.Trim();
                //WorkShip ws = new WorkShip();
                //employeeIds = ws.GetAllMemberIDsByLeaderID(employeeId);
            }

            node = doc.SelectSingleNode("GetData/EmployeeIDList");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                employeeIds = node.InnerText.Trim();
                if (employeeIds == "99")//查询其所有下属
                {
                    WorkShip ws = new WorkShip();
                    employeeIds = ws.GetAllMemberIDsByLeaderID(employeeId);
                }
                employeeIds = employeeIds.Replace("|", "','");
            }
            if (employeeIds.Length == 0)
                employeeIds = employeeId;

            sql = @"select Fdate as 日期, [FEmployeeName] as 姓名,
                        [RouteCount] as 签到次数,
                        [OKRouteCount] as 有效签到次数,
                        [CallCount]  as 拜访次数,
                        [UnPlanedCallCount] as 非计划拜访次数
                         from [dbo].[Route_Call_View]    Where FDate between '{0}' and  '{1}' and FEmployeeID In('{2}') order by  Fdate desc ,[RouteCount] desc";
            SQLServerHelper runner = new SQLServerHelper();
            sql = string.Format(sql, date1, date2, employeeIds);
            DataTable dt = runner.ExecuteSql(sql);
            //如果没有数据返回错误
            if (dt.Rows.Count == 0)
            {
                result = @"<GetData>" +
              "<Result>False</Result>" +
              "<DataRow><FileURL>" + "无数据可供下载." + "</FileURL></DataRow></GetData>";
                return result;
            }

            fileName = Guid.NewGuid().ToString().Replace("-", "");
            try
            {
                //移除列
                dt.Columns.Remove("日期");
                var excel = new Microsoft.Office.Interop.Excel.Application
                {
                    DisplayAlerts = false
                };
                //生成一个新的工资薄
                var excelworkBook = excel.Workbooks.Add(Type.Missing);
                var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                //获得表的行，列数目
                int row_num = dt.Rows.Count;
                int column_num = dt.Columns.Count;
                //生成一个二维数组
                object[,] dataArry = new object[row_num, column_num];
                object[,] headArry = new object[1, column_num];
                //把表中的数据放到数组中
                for (int i = 0; i < row_num; i++)
                {
                    for (int j = 0; j < column_num; j++)
                    {
                        dataArry[i, j] = dt.Rows[i][j].ToString();
                    }
                }

                for (int j = 0; j < column_num; j++)
                {
                    headArry[0, j] = dt.Columns[j].ColumnName.ToString();
                }
                excel.Range[excel.Cells[1, 1], excel.Cells[1, column_num]].Value = headArry;
                //把数组中的数据放到Excel中
                excel.Range[excel.Cells[2, 1], excel.Cells[row_num + 1, column_num]].Value = dataArry;
                string path = System.Configuration.ConfigurationManager.AppSettings["Path"];
                string fullpath = System.Web.HttpContext.Current.Server.MapPath(path);

                savePath = fullpath + "\\" + fileName + ".xlsx";

                excelworkBook.SaveAs(savePath);
                excelworkBook.Close();
                excel.Quit();
            }
            catch (Exception err)
            {
                result = "" + "<GetData>" +
          "<Result>False</Result>" +
          "<Description>" + err.Message + "</Description></GetData>";
                return result;
            }
            string url = "http://ydb.tenrypharm.com:6060/Files/" + fileName + ".xlsx";
            result = @"<GetData>" +
                       "<Result>True</Result>" +
                       "<DataRow><FileURL>" + url + "</FileURL></DataRow></GetData>";
            return result;
        }

        #endregion ExportCallReport

        #region GetCallRepotr2

        /// <summary>
        /// 读取指定人员某时段的拜访汇总报表数据
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public string GetCallRepotr2(string xmlString)
        {
            string result = "", sql = "", date1 = "", date2 = "", employeeIds = "";
            result = "<GetData>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetData>";
            date1 = DateTime.Now.ToString("yyyy-MM") + "-01";
            date2 = DateTime.Now.ToString("yyyy-MM-dd");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNode node = doc.SelectSingleNode("GetData/BeginDate");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                date1 = node.InnerText.Trim();
            }
            node = doc.SelectSingleNode("GetData/EndDate");
            if (node != null && node.InnerText.Trim().Length > 0)
            {
                date2 = node.InnerText.Trim();
            }

            if (employeeIds.Length == 0)
            {
                node = doc.SelectSingleNode("GetData/EmployeeID");//若为团队负责人，要读取其及直接下属的数据
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    employeeIds = node.InnerText.Trim();
                }
            }
            sql = @"  Select t3.FName As EmployeeName, t1.FEmployeeID As EmployeeID,t2.FName As InstitutionName ,
                      sum(Case FScheduleID When '4484030a-28d1-4e5e-ba72-6655f1cb2898' Then 1 Else 0 End) AS UnplanedCallCount,
                      Sum(1) AS CallCount,SUM(ISNULL(DATEDIFF(mi, t1.FStartTime, t1.FEndTime), 0)) AS TimeSpan
                      From CallActivity t1
                      Left Join t_Items t2 On t1.FInstitutionID = t2.FID
                      Left Join t_Items t3 On t1.FEmployeeID = t3.FID
                      Where FDate between '{0}' and  '{1}' and FEmployeeID In('{2}')
                      Group by t3.FName,t2.FName,t1.FEmployeeID
                      Order by CallCount Desc,TimeSpan Desc";
            sql = string.Format(sql, date1, date2, employeeIds);
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            result = Common.DataTableToXml(dt, "GetData", "", "List");
            return result;
        }

        #endregion GetCallRepotr2

        #region 多级数据获取

        public string GetMultiCallReport(string xmlString)
        {
            string result = "",
                startdate = "",
                enddate = "",
                employeeId = "",
                weekIndex = "",
                typeid = "",
                superid = "",
                viewtype = "",
                itemtype = "",
                viewweek = "",
                viewmonth = "",

                id = "";
            int querytype = -1, year;
            result = @"{{""GetMultiCallReport"":{{ ""result"":""false"",""Description"":"""",""dataRow"":"""" }} }}";
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
                XmlNode node = doc.SelectSingleNode("GetData/weekIndex");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    weekIndex = node.InnerText.Trim();
                }
                //显示详情
                node = doc.SelectSingleNode("GetData/typeid");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    typeid = node.InnerText.Trim();
                }
                //是月份，周次，或者具体日期0,1,2
                node = doc.SelectSingleNode("GetData/viewtype");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    viewtype = node.InnerText.Trim();
                }
                //查看周次
                node = doc.SelectSingleNode("GetData/viewweek");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    viewweek = node.InnerText.Trim();
                }
                //查看月份
                node = doc.SelectSingleNode("GetData/viewmonth");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    viewmonth = node.InnerText.Trim();
                }
                //获取查询类型部门或人
                node = doc.SelectSingleNode("GetData/querytype");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    querytype = int.Parse(node.InnerText.Trim());
                }
                node = doc.SelectSingleNode("GetData/id");
                if (node != null && node.InnerText.Trim().Length > 0)
                {
                    id = node.InnerText.Trim();
                }
                // Tuple<DateTime, DateTime> tupletPerTime = Common.GetPerTime(weekIndex);
                //拼接的sql语句
                List<string> showdataList = new List<string>();
                //列名
                List<string> columnname = new List<string>();
                List<string> timeList = new List<string>();

                string sql = "", allId = "", startDate = "", endDate = "";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = new DataTable();
                WorkShip workShip = new WorkShip();
                if (viewmonth.Contains('|'))
                {
                    year = int.Parse(viewmonth.Split('|')[0]);
                }
                else
                {
                    year = int.Parse(viewweek.Split('|')[0]);
                }
                //部门获取superID 修改 查询id
                if (querytype == 0)
                {
                    sql = $"SELECT [FSupervisorID] FROM [yaodaibao].[dbo].[t_Departments] where FID = '{id}'";
                    runner = new SQLServerHelper();
                    dt = runner.ExecuteSql(sql);
                    employeeId = dt.Rows[0]["FSupervisorID"].ToString();
                }
                //月-周
                if (viewtype == "0")
                {
                    startDate = Common.GetMonthTime(Convert.ToDateTime(viewmonth)).Split('&')[0];
                    endDate = Common.GetMonthTime(Convert.ToDateTime(viewmonth)).Split('&')[1];
                    //获取月的所有周次序
                    string monthWeeks = Common.GetMonthsWeek(year, int.Parse(viewmonth));
                    foreach (string item in monthWeeks.Split('|'))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            columnname.Add(year + item);
                            Tuple<DateTime, DateTime> monsunTuple = GetMonSunTime(year, int.Parse(item));
                            timeList.Add("\"" + $"{item}" + "\"");
                            showdataList.Add($"Sum(Case FWeek When '{year + item}' Then 1 Else 0 End) AS '{year + item}'");
                        }
                    }
                }
                //天-次，一周七天固定列数
                else
                {
                    result = GetAlllVisitRecord(employeeId, startDate, endDate);
                    return result;
                    //Tuple<DateTime, DateTime> monsunTuple = GetMonSunTime(year, int.Parse(viewweek));
                    //for (int i = 0; i < 7; i++)
                    //{
                    //    columnname.Add(monsunTuple.Item1.AddDays(i).ToString("yyyyMMdd"));
                    //    weektimeList.Add("\"" + monsunTuple.Item1.AddDays(i).ToString("yyyy-MM-dd") + "\"");
                    //    showdataList.Add($"Sum(Case  CONVERT(varchar(100), FDate, 112) When '{monsunTuple.Item1.AddDays(i).ToString("yyyyMMdd")}' Then 1 Else 0 End) AS '{monsunTuple.Item1.AddDays(i).ToString("yyyyMMdd")}'");
                    //}
                }
                //下载所有的 直接返回結果
                if (typeid == "0")
                {
                    result = DownloadAllReportDate(employeeId, startDate, endDate);
                    return result;
                }
                //startdate = tupletPerTime.Item1.ToString();
                //enddate = tupletPerTime.Item2.ToString();
                if (employeeId.Length == 0)
                {
                    node = doc.SelectSingleNode("GetData/employeeID");
                    if (node != null && node.InnerText.Trim().Length > 0)
                    {
                        employeeId = node.InnerText.Trim();
                    }
                }

                sql = $"select FDeptID from  yaodaibao.dbo.t_Employees WHERE FID ='{employeeId}'";
                dt = runner.ExecuteSql(sql);
                //获取直属下属部门的ID 如果是管理人员
                Tuple<bool, DataTable> tupledata = GetAllSubDepId(employeeId, dt.Rows[0]["FDeptID"].ToString());
                Dictionary<string, string> dicsubs = new Dictionary<string, string>();
                DataTable tempTable = new DataTable();
                List<string> subList = new List<string>();
                string subids = "", subdata = "";
                //统计型 遍历部门和人
                if (tupledata.Item2.Rows.Count > 0)
                {
                    foreach (DataRow item in tupledata.Item2.Rows)
                    {
                        if (!item["FID"].ToString().Contains("E"))
                        {
                            querytype = 0;
                            //根据depid获取管理人员employeeId
                            sql =
                                "SELECT ti.FName,td.FSupervisorID  FROM yaodaibao.dbo.t_Departments td LEFT JOIN t_Items ti ON td.FID = ti.FID Where FIsDeleted =0 and FID='" +
                                item["FID"] + "'";
                            runner = new SQLServerHelper();
                            dt = runner.ExecuteSql(sql);
                            //根据部门管理者ID获取当前部门和子部门所有的成员ID
                            subids = workShip.GetAllMemberIDsByLeaderID(dt.Rows[0]["FSupervisorID"].ToString());
                        }
                        else
                        {
                            querytype = 1;
                            subids = item["FID"].ToString().Replace("E", "");
                        }
                        if (viewtype == "0")
                        {
                            //根据周分组
                            sql = $" Select FType, {string.Join(",", showdataList.ToArray()) } From CallActivity t1 where FWeek in ('{string.Join(",", timeList.ToArray())}') and FEmployeeID In('{subids}') Group by FType ";
                        }
                        //这一周的拜访医院
                        else
                        {
                            //根据天分组
                            sql = $" Select FType,  {string.Join(",", showdataList.ToArray()) } From CallActivity t1 where FWeek in ('{string.Join(",", timeList.ToArray())}') and FEmployeeID In('{subids}') Group by FType ";
                        }
                        runner = new SQLServerHelper();
                        //统计晨访，夜访....
                        tempTable = runner.ExecuteSql(sql);
                        //保存行的
                        List<string> rowList = new List<string>();
                        //给定格式
                        foreach (DataRow row in tempTable.Rows)
                        {
                            //保存单个数据的
                            List<string> timesList = new List<string>();
                            for (int i = 0; i < tempTable.Columns.Count; i++)
                            {
                                timesList.Add("\"" + row[i].ToString() + "\"");
                            }
                            rowList.Add("[" + string.Join(",", timesList.ToArray()) + "]");
                        }
                        subList.Add($@"{{""name"":{item["FName"]},""id"":{item["FID"].ToString().Replace("E", "")},""querytype"":""{ querytype}"",""tableHead"":[{string.Join(",", timeList.ToArray())}], ""tableData"":[{string.Join(", ", rowList.ToArray())}] }}");
                    }
                }
                result = $@"{{""GetMultiCallReport"":{{ ""result"":""true"",""Description"":"""",""dataRow"":[{string.Join(",", subList.ToArray())}] }} }}";
            }
            catch (Exception e)
            {
                result = $@"{{""GetMultiCallReport"":{{ ""result"":""false"",""Description"":""{ e.Message}"",""dataRow"":"""" }} }}";
            }
            return result;
        }

        //列出人员的拜访医院列表
        public string GetAlllVisitRecord(string employeeId, string startDate, string endDate)
        {
            try
            {
                string sql = $"SELECT (Left(CONVERT(varchar(100), t2.FStartTime, 120),16) +'~'+ Left(CONVERT(varchar(100), t2.FEndTime, 120),16)) As TimeString,t2.FEmployeeID as employeeId,  t1.FExcutorID,t3.FName AS FExcutorName,t2.FSubject As SubjectString ,t1.FScheduleID As FID,t2.FInstitutionID As FInstitutionID,t4.FName As InstitutionName  FROM ScheduleExecutor t1  Left Join Schedule t2 On t1.FScheduleID= t2.FID  Left Join t_Items t3 On t1.FExcutorID= t3.FID  Left Join t_Items t4 On t4.FID= t2.FInstitutionID where   t2.FEmployeeID = '{employeeId}' and FStartTime between '{startDate}'   and   DATEADD(year, 1, '{endDate}') and  order by t2.FStartTime Desc";
                SQLServerHelper runHelper = new SQLServerHelper();
                runHelper.ExecuteSql(sql);
                DataTable dt = new DataTable();
                string result = Common.DataTableToXml(dt, "GetMyScheduleList", "", "List");
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        ////获取人员的医院列表
        //private string GetHospital(string employeeId, string startdate, string endstart)
        //{
        //    try
        //    {
        //        string sql = $"SELECT t_Items.FName,sum(Case FScheduleID When '4484030a-28d1-4e5e-ba72-6655f1cb2898' Then 1 Else 0 End) AS UnplanedCallCount,Sum(1) AS CallCount, FROM yaodaibao.dbo.CallActivity LEFT JOIN t_items ON CallActivity.FInstitutionID = t_Items.FID WHERE FEmployeeID ='{employeeId}' and FDate between '{startdate}' and  '{endstart}' Group by FName ,FEmployeeID";
        //        SQLServerHelper serverHelper = new SQLServerHelper();
        //        DataTable dt = serverHelper.ExecuteSql(sql);
        //        Dictionary<string, string> dichops = new Dictionary<string, string>();
        //        foreach (DataRow iteRow in dt.Rows)
        //        {
        //            dichops.Add(iteRow["FName"].ToString(), iteRow[""].ToString());
        //        }
        //        string hosdata = JsonConvert.SerializeObject(dichops);
        //        string result = $@"{{""GetMultiCallReport"":{{ ""result"":""true"",""Description"":"""",""DataRows"":""{ hosdata}"" }} }}";
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// 根据employeeid 判断是否有权限查看其它人数据
        /// </summary>
        /// <param name="leaderID">员工ID</param>
        /// <param name="deptID">部门ID</param>
        /// <returns>是否有权限查看其它人的数据，包含部门ID和直属员工ID的DataTable</returns>
        private Tuple<bool, DataTable> GetAllSubDepId(string leaderID, string deptID)
        {
            try
            {
                bool manager = false;
                string sql = "Select FID from t_Departments Where FIsDeleted =0 and FSupervisorID='" + leaderID + "'";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                DataTable empdt = new DataTable();
                //可以查看其它人的数据
                if (dt.Rows.Count > 0)
                {
                    manager = true;
                    sql = "Select ti.FID ,ti.FName from t_Departments td   LEFT JOIN t_Items ti ON ti.FID = td.FID  Where td.FIsDeleted =0 and FParentID='" + deptID + "'";
                    runner = new SQLServerHelper();
                    dt = runner.ExecuteSql(sql);
                    sql = $"Select 'E'+ ti.FID FID,ti.FName from t_Employees te   LEFT JOIN t_Items ti ON ti.FID = te.FID    Where te.FIsDeleted =0  and FDeptID in ('{deptID}') or FLeaderList like '%{leaderID}%'";
                    empdt = runner.ExecuteSql(sql);
                    //合并部门结果和直属人员结果
                    dt.Merge(empdt, false, MissingSchemaAction.Ignore);
                }
                else
                {
                    sql = $"Select 'E'+ ti.FID FID,ti.FName from t_Employees te   LEFT JOIN t_Items ti ON ti.FID = te.FID   Where te.FIsDeleted =0 and FID ={leaderID} ";
                    dt = runner.ExecuteSql(sql);
                }
                return new Tuple<bool, DataTable>(manager, dt);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 读取指定人员某时段的拜访汇总报表数据,如果是部门管理，获取所有人的employeeID包含子部门
        /// </summary>
        /// <param name="employeeId">人员ID</param>
        /// <returns></returns>
        public string DownloadAllReportDate(string employeeId, string startDate, string endDate)
        {
            string result = "", sql = "", date1 = "", date2 = "", employeeIds = "";
            result = "<GetData>" +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetData>";
            WorkShip workShip = new WorkShip();
            employeeIds = workShip.GetAllMemberIDsByLeaderID(employeeId);

            sql = $"  Select t3.FName As EmployeeName, t1.FEmployeeID As EmployeeID,t2.FName As InstitutionName , sum(Case FScheduleID When '4484030a-28d1-4e5e-ba72-6655f1cb2898' Then 1 Else 0 End) AS UnplanedCallCount,  Sum(1) AS CallCount,SUM(ISNULL(DATEDIFF(mi, t1.FStartTime, t1.FEndTime), 0)) AS TimeSpan  From CallActivity t1 Left Join t_Items t2 On t1.FInstitutionID = t2.FID  Left Join t_Items t3 On t1.FEmployeeID = t3.FID   Where FDate between '{startDate}' and  '{endDate}' and FEmployeeID In('{employeeIds}') Group by t3.FName,t2.FName,t1.FEmployeeID  Order by CallCount Desc,TimeSpan Desc";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            result = Common.DataTableToXml(dt, "GetData", "", "List");
            return result;
        }

        #endregion 多级数据获取

        /// <summary>
        /// 获取某年的某一周，周一和周日的日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public Tuple<DateTime, DateTime> GetMonSunTime(int year, int week)
        {
            DateTime yearTime = Convert.ToDateTime(year + "-01-01");
            DateTime sunTime, monTime;
            monTime = Common.GetWeekFirstDayMon(yearTime);
            sunTime = Common.GetWeekLastDaySun(yearTime);
            monTime = monTime.AddDays((week - 1) * 7);
            sunTime = sunTime.AddDays((week - 1) * 7);
            return new Tuple<DateTime, DateTime>(monTime, sunTime);
        }
    }
}