using iTR.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ydb.Report
{
    public class PersonalCompass
    {

        public string GetPersonPerReport(string dataString, string FormatResult, string callType)
        {
            //加,连接起前面的json字符串
            string result = "", rdataRow, datarows, yearweek,panelRow="",weekindex= ",\"FWeekIndex\":";
            List<string> dataRowList = new List<string>();
            //初始化状态
            //   result = string.Format(FormatResult, callType, "\"False\"", "", "");
            //每个DataRow格式
            string rowcontent = "{{\"dataSets\":[{{\"values\":[{{ \"value\": {0}, \"label\": \"\"}},{{ \"value\": {1}, \"label\":\"\"}}],\"label\":\"\",\"config\": {2}}}],\"name\": \"{3}\",\"Index\":\"{4}\",\"value\":\"{5}\",\"Count\":\"{6}\",\"startTime\":\"{7}\",\"endTime\":\"{8}\"}}";
            //dataString = "{\"FWeekIndex\":\"10\",\"AuthCode\":\"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"EmployeeID\":\"4255873149499886263\",\"BeginDate\":\"2020-08-05\",\"EndDate\":\"2020-08-31\"}";
            try
            {
                //查询实体
                RouteEntity routeEntity = JsonConvert.DeserializeObject<RouteEntity>(dataString);
                weekindex += routeEntity.FWeekIndex;
                DateTime startTime, endTime;
                Tuple<DateTime, DateTime> pertime = ReportHelper.GetPerTime(routeEntity.FWeekIndex);
                //开始时间
                startTime = pertime.Item1;
                //结束时间
                endTime = pertime.Item2;
                //5-8使用
                yearweek = ReportHelper.GetYearWithWeeks(routeEntity.FWeekIndex);
                //目前有些数据没有，暂时跳过
                for (int i = 1; i < 9; i++)
                {
                    //还没有流程跳过不处理
                    if (i == 1 || i == 2||i==3)
                    {
                        rdataRow = GetDataRow(i, rowcontent, routeEntity.EmployeeId, startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"), "");
                        dataRowList.Add(rdataRow);
                    }
                    else if (i == 5 || i == 6)
                    {
                        panelRow += GetDataRow(i, rowcontent, routeEntity.EmployeeId, "", "", yearweek);
                    }
                    else
                    {
                        continue;
                    }


                }
                //加，拼接下面的json
                datarows = string.Join(",", dataRowList.ToArray())+",";
                //最后结果
                result = string.Format(FormatResult, callType, "\"True\"", "\"\"", "{\"DataRow\":[" + datarows + "{" +panelRow + weekindex +"}"+ "]}");

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
        /// <param name="EmployeeId">团队的EmployeeId是'0001','0002'格式</param>
        /// <param name="rowContent"></param>
        /// <param name="type">1,签到，2,拜访，3,流程，4,待定,5,艾夫吉夫 6,丙戊酸钠 7,待支付金额，8，奖金</param>
        /// <returns></returns>
        public string GetDataRow(int viewType, string rowContent, string EmployeeId, string startTime, string endTime, string yearweek)
        {
            string sql = "", viewName = "", tempresult="", routeconfig,p1,p2;
            int total, okcount, per;
            //产品多的时候 获取前两个产品 select t1.FProductID,t2.FProductName from ( select top 2 FProductID, COUNT(FProductID) as ProductCount from [yaodaibao].[dbo].[HospitalStock] group by FProductID order by ProductCount desc)  t1 left join (Select FID AS  FProductID, FName As FProductName From t_Items Where FClassID = '36d33d13-4f8b-4ee4-84c5-49ff7c8691c4' and FIsDeleted=0 )  t2 on t1.FProductID = t2.FProductID

            //SQLServerHelper runner = new SQLServerHelper();
            //sql = "select top 2 FProductID as  ProductID from [yaodaibao].[dbo].[HospitalStock]";
            //DataTable dtproduct = runner.ExecuteSql(sql);
            //p1 = dtproduct.Rows[0]["ProductID"] ==DBNull.Value? "" :dtproduct.Rows[0]["ProductID"].ToString();
            //p2 = dtproduct.Rows[1]["ProductID"] == DBNull.Value ? "" : dtproduct.Rows[1]["ProductID"].ToString();

            switch (viewType)
            {
                //1,签到
                case 1:
                    viewName = "签到";
                    sql = $"SELECT  ISNULL(SUM([RouteCount]),0) Total ,ISNULL(SUM([OKRouteCount]),0) OKCount FROM [yaodaibao].[dbo].[RouteView] where '{startTime}' <= FDate  and  FDate <= '{ endTime }' and FEmployeeID in ({EmployeeId})";

                    break;
                //2,拜访
                case 2:
                    viewName = "拜访";
                    sql = $"SELECT  ISNULL(SUM([CallCount]),0) Total ,ISNULL(SUM([CallCount] - [UnPlanedCallCount]),0) OKCount FROM [yaodaibao].[dbo].[Route_Call_View] where '{startTime}' <= FDate  and  FDate <= '{ endTime }' and FEmployeeID in ({EmployeeId})";
                    break;
                //3,流程
                case 3:
                    viewName = "流程";
                    sql = $"Select  count(*) As Total , sum(case FState when '完成' then 1 Else 0 End) As OKCount From[dbo].[OAProcessStatus] where '{startTime}' <= [FStart_Date]  and  [FStart_Date] <= '{ endTime }' and FStart_Member_ID in ({EmployeeId})";
                    break;
                //4,待定
                case 4:
                    sql = "";
                    break;
                //5,艾夫吉夫
                case 5:
                    viewName = "艾夫吉夫";                  
                    sql = $"select SUM(FStock_IB) StockIB,SUM(FStock_IN) Total,SUM(FStock_EB) StockEB,SUM(FSaleAmount) OKCount from [yaodaibao].[dbo].[HospitalStock_Detail] where FFormmainID in (SELECT FID FROM [yaodaibao].[dbo].[HospitalStock] where(cast(FYear as nvarchar(4)) + cast(FWeekIndex as nvarchar(2))) in({yearweek}) and FEmployeeID in({EmployeeId}) and FProductID = '69d55ff7-d9d6-4f20-bcbc-b5244894f36e' )";
                    break;
                // 6,丙戊酸钠
                case 6:
                    viewName = "丙戊酸钠";
                    sql = $"select SUM(FStock_IB) StockIB,SUM(FStock_IN) Total,SUM(FStock_EB) StockEB,SUM(FSaleAmount) OKCount from [yaodaibao].[dbo].[HospitalStock_Detail] where FFormmainID in (SELECT FID FROM [yaodaibao].[dbo].[HospitalStock] where(cast(FYear as nvarchar(4)) + cast(FWeekIndex as nvarchar(2))) in({yearweek}) and FEmployeeID in({EmployeeId})  and FProductID = '6e927e6d-b03a-4601-b4dd-2ec583a46768' )";
                    break;
                // 7,待支付金额
                case 7:
                    viewName = "待支付金额";
                    sql = "";
                    break;
                //8，奖金
                case 8:
                    viewName = "奖金";
                    sql = "";
                    break;
            }
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            //百分比
            total = int.Parse((dt.Rows[0]["Total"] ==DBNull.Value ) ? "0" : dt.Rows[0]["Total"].ToString());
            okcount = int.Parse(dt.Rows[0]["OKCount"]== DBNull.Value ? "0": dt.Rows[0]["OKCount"].ToString());
            if (viewType<5)
            {
                if (total == 0)
                {
                    per = 0;
                }
                else
                {
                    per = okcount * 100 / total;
                }
                //获取配置文件
                routeconfig = Common.GetCompassConfigFromXml("Route").Replace("Quot", "\"");
                //DataRow数据            
                tempresult = string.Format(rowContent, per, (100 - per), routeconfig, viewName, viewType, per + "%", total.ToString(), startTime, endTime);
            }
            else
            {
                //艾夫吉夫
                if (viewType == 5)
                {
                    //加,拼后面的json
                    tempresult = $"\"AFJFName\":\"艾夫吉夫\",\"AFJFCount\":{okcount},\"AFJFProductID\":\"69d55ff7-d9d6-4f20-bcbc-b5244894f36e\""+",";
                }
                //丙戊酸钠
               else if (viewType==6)
                {
                    tempresult = $"\"BWSNName\":\"丙戊酸钠\",\"BWSNCount\":{okcount},\"BWSNProductID\":\"6e927e6d-b03a-4601-b4dd-2ec583a46768\"";
                }
            }

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
