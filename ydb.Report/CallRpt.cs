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
using System.Web.Services;
using System.Web;

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
        #endregion

        #region ExportCallReport
        public string ExportCallReport(string xmlString)
        {
            string result = "", sql = "", date1 = "", date2 = "", employeeIds = "", employeeId = "", savePath = "", fileName="";

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

                savePath = fullpath+"\\" + fileName + ".xlsx";
             
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
        #endregion

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
        #endregion
    }
}
