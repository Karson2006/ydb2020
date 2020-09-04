using iTR.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ydb.Report
{
  public  class PersonalCompass
    {

        public string GetPersonPerReport(string dataString, string FormatResult, string callType)
        {
            string result = "", temptime, rdataRow, datarows;
            List<string> dataRowList = new List<string>();
            //初始化状态
            result = string.Format(FormatResult, callType, "False", "", "");
            //每个DataRow格式
            //string rowcontent = "{{\"DataRow\": {{\"dataSets\":[{{\"values\":[{{ \"value\": {0}, \"label\": \"\"}},{{ \"value\": {1}, \"label\":\"\"}}],\"label\":\"\",\"config\": {2}}}],\"name\": \"{3}\",\"Index\":\"{4}\",\"value\":\"{5}\",\"Count\":\"{6}\",\"startTime\":\"{7}\",\"endTime\":\"{8}\"}}}}";

            string rowcontent = "{{\"dataSets\":[{{\"values\":[{{ \"value\": {0}, \"label\": \"\"}},{{ \"value\": {1}, \"label\":\"\"}}],\"label\":\"\",\"config\": {2}}}],\"name\": \"{3}\",\"Index\":\"{4}\",\"value\":\"{5}\",\"Count\":\"{6}\",\"startTime\":\"{7}\",\"endTime\":\"{8}\"}}";



            //dataString = "{\"FWeekIndex\":\"10\",\"AuthCode\":\"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"EmployeeID\":\"4255873149499886263\",\"BeginDate\":\"2020-08-05\",\"EndDate\":\"2020-08-31\"}";
            try
            {
                //查询实体
                RouteEntity routeEntity = JsonConvert.DeserializeObject<RouteEntity>(dataString);


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

                for (int i = 1; i < 5; i++)
                {
                    //还没有流程跳过不处理
                    if (i > 2)
                    {
                        continue;
                    }
                    rdataRow = GetDataRow(rowcontent, startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"), routeEntity.EmployeeId, i);
                    dataRowList.Add(rdataRow);
                }
                datarows = string.Join(",", dataRowList.ToArray());
                //最后结果
                result = string.Format(FormatResult, callType, "\"True\"", "\"\"","{\"DataRow\":["+datarows+"]}");

            }
            catch (Exception err)             
            {
                result = string.Format(FormatResult, callType, "\"False\"", err.Message, "");
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="EmployeeId"></param>
        /// <param name="rowContent"></param>
        /// <param name="type">1,签到，2,拜访，3,流程，4,待定</param>
        /// <returns></returns>
        public string GetDataRow(string rowContent, string startTime,string endTime,string EmployeeId,int viewType)
        {
            string sql = "",viewName="";
            int total, okcount, per;
            switch (viewType)
            {
                //1,签到
                case 1:
                    viewName = "签到";
                    sql = $"SELECT  ISNULL(SUM([RouteCount]),0) Total ,ISNULL(SUM([OKRouteCount]),0) OKCount FROM [yaodaibao].[dbo].[RouteView] where '{startTime}' <= FDate  and  FDate <= '{ endTime }' and FEmployeeID = {EmployeeId}";
                    
                    break;
                //2,拜访
                case 2:
                    viewName = "拜访";
                    sql = $"SELECT  ISNULL(SUM([CallCount]),0) Total ,ISNULL(SUM([CallCount] - [UnPlanedCallCount]),0) OKCount FROM [yaodaibao].[dbo].[Route_Call_View] where '{startTime}' <= FDate  and  FDate <= '{ endTime }' and FEmployeeID = {EmployeeId}";
                    break;
                //3,流程
                case 3:
                    viewName = "流程";
                    sql = "";
                    break;
                //4,待定
                case 4:
                    sql = "";
                    break;
            }
           

            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            //百分比

            total = int.Parse(dt.Rows[0]["Total"].ToString());
            okcount = int.Parse(dt.Rows[0]["OKCount"].ToString());
            if (total == 0)
            {
                per = 0;
            }
            else
            {
                per = okcount * 100 / total;
            }
            //获取配置文件
            string routeconfig = Common.GetCompassConfigFromXml("Route").Replace("Quot", "\"");
            //DataRow数据

            string tempresult = string.Format(rowContent, per, (100-per), routeconfig, viewName, viewType, per + "%", total.ToString(), startTime, endTime);
            return tempresult;
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
