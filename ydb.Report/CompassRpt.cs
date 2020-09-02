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
        public CompassRpt()
        {
        }
        public string GetPersonPerReport(string dataString)
        {
            dataString = "{\"FWeekIndex\":\"10\",\"AuthCode\":\"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"EmployeeID\":\"4255873149499886263\",\"BeginDate\":\"2020-08-05\",\"EndDate\":\"2020-08-31\"}";
            try
            {
                RouteEntity routeEntity = JsonConvert.DeserializeObject<RouteEntity>(dataString);
                string sql = "", result = "", temptime;
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
                sql = $"SELECT  ISNULL(SUM([RouteCount]),0) RouteCount ,ISNULL(SUM([OKRouteCount]),0) OKRouteCount FROM [yaodaibao].[dbo].[RouteView] where '{startTime.ToString("yyyy-MM-dd")}'<= FDate  and  FDate <= '{endTime.ToString("yyyy-MM-dd")}' and FEmployeeID = {routeEntity.EmployeeId}";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                //百分比

                rcount = int.Parse(dt.Rows[0]["RouteCount"].ToString());
                okcount = int.Parse(dt.Rows[0]["OKRouteCount"].ToString());
                if (rcount == 0)
                {
                    per = 0;
                }
                else
                {
                    per = okcount * 100 / rcount;
                }


                PersonPerResult mainResult = new PersonPerResult() { dataRow = new List<PersonPerResultDataRow>() };
                //签到的
                PersonPerResultDataRow roteRow = new PersonPerResultDataRow() { dataSets = new List<PersonPerResultDataSet>() };
                roteRow.Count = rcount.ToString();
                roteRow.Name = "签到";
                roteRow.Index = "1";
                roteRow.Value = per.ToString() + "%";
                roteRow.StartTime = startTime.ToString("yyyy-MM-dd");
                roteRow.EndTime = endTime.ToString("yyyy-MM-dd");

                PersonPerResultDataSet routesets = new PersonPerResultDataSet();
                routesets.Values = "[{value: " + rcount + ", label: ''},{value: " + okcount + ", label: ''}]";
                routesets.Lable = "";
                routesets.Config = Common.GetCompassConfigFromXml("Route").Replace("ColorPre", "#");
                //拜访的
                PersonPerResultDataRow callRow = new PersonPerResultDataRow() { dataSets = new List<PersonPerResultDataSet>() };
                callRow.Count = rcount.ToString();
                callRow.Name = "拜访";
                callRow.Index = "2";
                callRow.Value = per.ToString() + "%";
                callRow.StartTime = startTime.ToString("yyyy-MM-dd");
                callRow.EndTime = endTime.ToString("yyyy-MM-dd");

                PersonPerResultDataSet callsets = new PersonPerResultDataSet();
                callsets.Values = "[{value: " + rcount + ", label: ''},{value: " + okcount + ", label: ''}]";
                callsets.Lable = "";
                callsets.Config = Common.GetCompassConfigFromXml("Call").Replace("ColorPre", "#");

                //进销存的
                PersonPerResultDataRow stockRow = new PersonPerResultDataRow() { dataSets = new List<PersonPerResultDataSet>() };
                stockRow.Count = rcount.ToString();
                stockRow.Name = "进销存";
                stockRow.Index = "3";
                stockRow.Value = per.ToString() + "%";
                stockRow.StartTime = startTime.ToString("yyyy-MM-dd");
                stockRow.EndTime = endTime.ToString("yyyy-MM-dd");

                PersonPerResultDataSet stocksets = new PersonPerResultDataSet();
                stocksets.Values = "[{value: " + rcount + ", label: ''},{value: " + okcount + ", label: ''}]";
                stocksets.Lable = "";
                stocksets.Config = Common.GetCompassConfigFromXml("Call").Replace("ColorPre", "#");

                //datarow添加一个datasets
                roteRow.dataSets.Add(routesets);
                callRow.dataSets.Add(callsets);
                stockRow.dataSets.Add(stocksets);

                //添加一个datarow
                mainResult.dataRow.Add(roteRow);
                mainResult.dataRow.Add(callRow);
                mainResult.dataRow.Add(stockRow);
                result = JsonConvert.SerializeObject(mainResult);
                return result;
            }
            catch (Exception err)
            {
                throw err;
            }
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
