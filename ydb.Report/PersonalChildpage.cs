using iTR.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace ydb.Report
{
    public class PersonalChildpage
    {
        //自定义》1,签到，2,拜访，3,流程，4,待定,5,艾夫吉夫 6,丙戊酸钠 7,待支付金额，8，奖金
        public string GetPersonChildData(string dataString, string FormatResult, string callType, string childtype)
        {
            int childType = int.Parse(childtype);
            string sql = "", result = "", yearweek, weekindex = ",\"FWeekIndex\":";
            //dataString = "{\"FWeekIndex\":\"-11\",\"AuthCode\":\"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"EmployeeID\":\"4255873149499886263\",\"BeginDate\":\"2020-08-05\",\"EndDate\":\"2020-08-31\"}";
            string rowcontent, dataRow = "";
            List<string> rowList = new List<string>();
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
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = new DataTable();
                switch (childType)
                {
                    case 1: break;
                    case 2: break;
                    //流程
                    case 3:
                        sql = $"SELECT [FStart_Member_Name] as FStart_Member_Name, [FSubject] as FSubject ,[FStart_Date] as StartDate,[FCurrent_Member_Name] as CurrentMemberName FROM [yaodaibao].[dbo].[OAProcessStatus]  where   '{startTime}' <= [FStart_Date]  and [FStart_Date] <= '{endTime}' and FState in ('流转中') and FStart_Member_ID in ({routeEntity.EmployeeIds}) order by FStart_Date desc";
                        dt = runner.ExecuteSql(sql);
                        foreach (DataRow item in dt.Rows)
                        {
                            rowcontent = "{\"Time\":\"" + DateTime.Parse(item["StartDate"].ToString()).ToString("yyyy年MM月yy日") + "\",\"Subject\":\"" + item["FSubject"] + "\",\"Code\":\"" + item["FSubject"] + "\",\"CurrentName\":\"" + item["CurrentMemberName"] + "\",\"Name\":\"" + item["FStart_Member_Name"] + "\",\"startTime\":\"" + startTime.ToString("yyyyMMdd") + "\",\"endTime\":\"" + endTime.ToString("yyyyMMdd") + "\",\"FWeekIndex\":\"" + routeEntity.FWeekIndex + "\"}";
                            rowList.Add(rowcontent);
                        }
                        break;
                    //支付
                    case 4:
                        //Ffield0006 可为空
                        sql = $"select Ffield0005 as PayType,Ffield0007 as PayCode ,Ffield0008 as Amount ,FApplyName as ApplyName,(Ffield0008-Ffield0034) as  Paid,Ffield0034 as Balance from [yaodaibao].[dbo].[formmain_3460]  where   '{startTime}' <= [FStart_Date]  and [FStart_Date] <= '{endTime}' and Ffield0006 in ('{routeEntity.EmployeeIds}') order by FStart_Date desc";
                        dt = runner.ExecuteSql(sql);
                        foreach (DataRow item in dt.Rows)
                        {
                            rowcontent = "{\"Year\":\"" + DateTime.Now.ToString("yyyy") + "\",\"PayType\":\"" + item["PayType"] + "\",\"PayCode\":\"" + item["PayCode"] + "\",\"ApplyName\":\"" + item["ApplyName"] + "\",\"Paid\":\"" + item["Paid"] + "\",\"Amount\":\"" + item["Amount"] + "\",\"Balance\":\"" + item["Balance"] + "\",\"startTime\":\"" + startTime.ToString("yyyyMMdd") + "\",\"endTime\":\"" + endTime.ToString("yyyyMMdd") + "\"}";
                            rowList.Add(rowcontent);
                        }
                        break;
                    case 5: break;
                    //销量
                    case 6:
                        //Ffield0014 可为空
                        sql = $"select   [Ffield0002] as Hospital,[Ffield0008] as  Total FROM [yaodaibao].[dbo].[formmain_6786]  where Ffield0011 in ('招商') and  '{startTime}' <= [FStart_Date]  and [FStart_Date] <= '{endTime}' and Ffield0014 in ('{routeEntity.EmployeeIds}') order by FStart_Date desc";
                        dt = runner.ExecuteSql(sql);
                        string nextpage = "false";
                        //没有招商
                        if (dt.Rows.Count < 0)
                        {
                            sql = $"select   [Ffield0002] as Hospital,[Ffield0008] as  Total FROM [yaodaibao].[dbo].[formmain_6786]  where  '{startTime}' <= [FStart_Date]  and [FStart_Date] <= '{endTime}' and Ffield0014 in ('{routeEntity.EmployeeIds}') order by FStart_Date desc";
                            dt = runner.ExecuteSql(sql);
                        }
                        //有招商
                        else
                        {
                            nextpage = "true";
                        }
                        foreach (DataRow item in dt.Rows)
                        {
                            rowcontent = "{\"nextpage\":\"" + nextpage + "\",\"Year\":\"" + DateTime.Now.ToString("yyyy") + "\",\"Hospital\":\"" + item["Hospital"] + "\",\"Total\":\"" + item["Total"] + "\",\"startTime\":\"" + startTime.ToString("yyyyMMdd") + "\",\"endTime\":\"" + endTime.ToString("yyyyMMdd") + "\",\"FWeekIndex\":\"" + routeEntity.FWeekIndex + "\"}";
                            rowList.Add(rowcontent);
                        }
                        break;
                    case 7: break;
                    case 8: break;
                    default:
                        break;
                }
                dataRow = string.Join(",", rowList.ToArray());
                //最后结果
                result = string.Format(FormatResult, callType, "\"True\"", "\"\"", "{\"DataRow\":[" + dataRow + "]}");
            }
            catch (Exception err)
            {
                result = string.Format(FormatResult, callType, "\"False\"", err.Message, "");
            }

            return result;
        }


        //支付查询
        public string PayQuery(string dataString, string FormatResult, string callType)
        {
            string rowcontent, dataRow = "", sql = "", result = "",partsql="";
            List<string> rowList = new List<string>();
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = new DataTable();
            try
            {
                RouteEntity routeEntity = JsonConvert.DeserializeObject<RouteEntity>(dataString);
                //未支付 只看应付余额是否为0
                if (routeEntity.FilterType=="-1")
                {
                    partsql = "  and Ffield0034 > 0  ";
                }
                //已支付
                else if (routeEntity.FilterType == "1")
                {
                    partsql = "  and Ffield0034 = 0  ";
                }
                //另外情况全查
                //else
                //Ffield0006 可为空 Ffield0009//已付
                sql = $"select Ffield0005 as PayType,Ffield0007 as PayCode ,Ffield0008 as Amount ,FApplyName as ApplyName,(Ffield0008-Ffield0034)  as  Paid,Ffield0034 as Balance from [yaodaibao].[dbo].[formmain_3460]  where   '{routeEntity.StartTime}' <= [FStart_Date]  and [FStart_Date] <= '{routeEntity.EndTime}' and Ffield0006 in ('{routeEntity.EmployeeIds}')  {partsql} order by FStart_Date desc";
                dt = runner.ExecuteSql(sql);
                foreach (DataRow item in dt.Rows)
                {
                    rowcontent = "{\"Year\":\"" + DateTime.Now.ToString("yyyy") + "\",\"PayType\":\"" + item["PayType"] + "\",\"PayCode\":\"" + item["PayCode"] + "\",\"ApplyName\":\"" + item["ApplyName"] + "\",\"Paid\":\"" + item["Paid"] + "\",\"Amount\":\"" + item["Amount"] + "\",\"Balance\":\"" + item["Balance"] + "\",\"startTime\":\"" + routeEntity.StartTime + "\",\"endTime\":\"" + routeEntity.EndTime + "\"}";
                    rowList.Add(rowcontent);
                }
                dataRow = string.Join(",", rowList.ToArray());
                //最后结果
                result = string.Format(FormatResult, callType, "\"True\"", "\"\"", "{\"DataRow\":[" + dataRow + "]}");
            }
            catch (Exception err)
            {
                result = string.Format(FormatResult, callType, "\"False\"", err.Message, "");
            }

            return result;
        }
    }
}
