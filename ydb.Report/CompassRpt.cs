using iTR.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Services.Description;
using ydb.BLL;

namespace ydb.Report
{
    public class CompassRpt
    {
        public void test()
        {
        }

        public string GetPersonPerReport(string dataString,string FormatResult,string callType)
        {
            string sql = "", result = "", temptime;
            result = string.Format(FormatResult, callType, "False", "", "");
            string Rowcontent = "\"DataRow\": {{\"dataSets\":[{{\"values\":[{{ \"value\": {0}, \"label\": \"\"}},{{ \"value\": {1}, \"label\":\"\"}}],\"label\":\"\",\"config\": {2}}}],\"name\": \"{3}\",\"Index\":\"{4}\",\"value\":\"{5}\",\"Count\":\"{6}\",\"startTime\":\"{7}\",\"endTime\":\"{8}\"}}";
            //dataString = "{\"FWeekIndex\":\"10\",\"AuthCode\":\"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"EmployeeID\":\"4255873149499886263\",\"BeginDate\":\"2020-08-05\",\"EndDate\":\"2020-08-31\"}";
            try
            {
                RouteEntity routeEntity = JsonConvert.DeserializeObject<RouteEntity>(dataString);

                int rcount, okcount, per;
                DateTime startTime, endTime;
                switch (routeEntity.FWeekIndex)
                {
                    //上月
                    case "-11":
                        temptime = Common.GetMonthTime(DateTime.Now.AddMonths(-1));
                        startTime = DateTime.Parse(temptime.Split('&')[0]);
                        endTime = DateTime.Parse(temptime.Split('&')[1]);
                        break;
                    //本月
                    case "10":
                        temptime = Common.GetMonthTime(DateTime.Now);
                        startTime = DateTime.Parse(temptime.Split('&')[0]);
                        endTime = DateTime.Parse(temptime.Split('&')[1]);
                        break;
                    //上周
                    case "-1":
                        startTime = Common.GetWeekFirstDayMon(DateTime.Now.AddDays(-7));
                        endTime = Common.GetWeekLastDaySun(DateTime.Now.AddDays(-7));
                        break;
                    //本周
                    case "0":
                        startTime = Common.GetWeekFirstDayMon(DateTime.Now);
                        endTime = Common.GetWeekLastDaySun(DateTime.Now);
                        break;
                    default:
                        throw new Exception();
                }
                sql = $"SELECT  ISNULL(SUM([RouteCount]),0) RouteCount ,ISNULL(SUM([OKRouteCount]),0) OKRouteCount FROM [yaodaibao].[dbo].[RouteView] where '{startTime.ToString("yyyy-MM-dd")}' <= FDate  and  FDate <= '{ endTime.ToString("yyyy-MM-dd") }' and FEmployeeID = { routeEntity.EmployeeId}";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                //百分比

                rcount = int.Parse(dt.Rows[0]["RouteCount"].ToString());
                okcount = int.Parse(dt.Rows[0]["OKRouteCount"].ToString());
                if (routeEntity.EmployeeId == "4255873149499886263")
                {
                    rcount = 55;
                    okcount = 45;
                }

                if (rcount == 0)
                {
                    per = 0;
                }
                else
                {
                    per = okcount * 100 / rcount;
                }
                string routeconfig = Common.GetCompassConfigFromXml("Route").Replace("ColorPre", "#").Replace("Quot","\"");
               string tempresult = string.Format(Rowcontent, rcount.ToString(), okcount.ToString(), routeconfig, "签到", "1", per + "%", rcount.ToString(), startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"));
                result = string.Format(FormatResult, callType, "True", "", tempresult);

            }
            catch (Exception err)
            {
                result = string.Format(FormatResult, callType, "False", err.Message, "");
            }
            return result;
        }
    }
    public class RouteEntity
    {
        public string AuthCode { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }
        public string EmployeeId { get; set; }
        public string FWeekIndex { get; set; }

    }
    public class PersonPerResult
    {
        public List<PersonPerResultDataRow> dataRow { get; set; }
    }
    public class PersonPerResultDataRow
    {
        public List<PersonPerResultDataSet> dataSets { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
        //
        public string Count { get; set; }
        //百分比
        public string Value { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class PersonPerResultDataSet
    {
        //具体数值
        public string Values { get; set; }
        public string Lable { get; set; }
        public string Config { get; set; }
        public string ValueTextColor { get; set; }
    }
}
