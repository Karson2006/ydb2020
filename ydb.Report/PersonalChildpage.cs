using iTR.Lib;
using Newtonsoft.Json;
using System;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ydb.Report
{
    public class PersonalChildpage
    {
        //自定义》1,签到，2,拜访，3,流程，4,待定,5,艾夫吉夫 6,丙戊酸钠 7,待支付金额，8，奖金
        public string GetPersonChildData(string dataString, string FormatResult, string callType, string childtype)
        {
            int childType=int.Parse(childtype);
            string sql = "", result = "", yearweek, weekindex = ",\"FWeekIndex\":";
            //dataString = "{\"FWeekIndex\":\"-11\",\"AuthCode\":\"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"EmployeeID\":\"4255873149499886263\",\"BeginDate\":\"2020-08-05\",\"EndDate\":\"2020-08-31\"}";
            string rowcontent,dataRow="";
            List<string> rowList = new List<string>();
            try
            {
                //查询实体
                RouteEntity routeEntity = JsonConvert.DeserializeObject<RouteEntity>(dataString);
                weekindex += routeEntity.FWeekIndex;
                childType = routeEntity.ChildType;
                DateTime startTime, endTime;
                Tuple<DateTime, DateTime> pertime = ReportHelper.GetPerTime(routeEntity.FWeekIndex);
                //开始时间
                startTime = pertime.Item1;
                //结束时间
                endTime = pertime.Item2;
                //5-8使用
                yearweek = ReportHelper.GetYearWithWeeks(routeEntity.FWeekIndex);
                SQLServerHelper runner = new SQLServerHelper();
                switch (childType)
                {
                    case 1: break;
                    case 2: break;
                    //流程
                    case 3:
                        sql = $"SELECT [FSubject] as FSubject ,[FStart_Date] as StartDate,[FCurrent_Member_Name] as CurrentMemberName FROM [yaodaibao].[dbo].[OAProcessStatus]  where   '{startTime}' <= [FStart_Date]  and [FStart_Date] <= '{endTime}' and FState in ('流转中') and FStart_Member_ID in ({routeEntity.EmployeeIds})";
                        DataTable dt = runner.ExecuteSql(sql);
                        foreach (DataRow item in dt.Rows)
                        {
                            rowcontent = "{\"Time\":\"" + DateTime.Parse(item["StartDate"].ToString()).ToString("yyyyMMyy") + "\",\"Subject\":\"" + item["FSubject"] + "\",\"Name\":\"" + item["CurrentMemberName"] + "\",\"startTime\":\"" + startTime.ToString("yyyyMMdd") + "\",\"endTime\":\"" + endTime.ToString("yyyyMMdd") + "\",\"FWeekIndex\":\"" + routeEntity.FWeekIndex + "\"}";
                            rowList.Add(rowcontent);
                        }        
                        break;
                    case 4: break;
                    case 5: break;
                    case 6: break;
                    case 7: break;
                    case 8: break;
                    default:
                        break;
                }
                dataRow = string.Join(",", rowList.ToArray());
                //最后结果
                result = string.Format(FormatResult, callType, "\"True\"", "\"\"", "{\"DataRow\":["+ dataRow + "]}");
            }
            catch (Exception err)
            {
                result = string.Format(FormatResult, callType, "\"False\"", err.Message, "");
            }

            return result;
        }
    }
}
