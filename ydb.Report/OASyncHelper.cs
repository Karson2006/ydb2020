using iTR.Lib;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ydb.Report
{
    public class OASyncHelper
    {
        //同步流程状态
        public static string SyncFlow(string xmlMessage)
        {
            string result = "<GetData> " +
                    "<Result>False</Result>" +
                    "<Description></Description>" +
                    "<Description></Description>" +
                    "<DataRows></DataRows>" +
                    "</GetData>";
            string sql = "", updateValue = "";
            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                List<Dictionary<string, string>> formson = new List<Dictionary<string, string>>();
                Dictionary<string, string> mainform = Common.GetFieldValuesFromXmlEx(xmlMessage, "GetData", out formson, "1", "");
                //先查ID有没有记录
                sql = $"select [FID] from  [yaodaibao].[dbo].[OAProcessStatus]  where FID = {mainform["FID"]}";
                System.Data.DataTable data = runner.ExecuteSql(sql);
                //没有查到数据，先插入在更新
                if (data.Rows.Count < 1)
                {
                    sql = $"Insert Into [yaodaibao].[dbo].[OAProcessStatus](FID) Values({mainform["FID"]})";
                    runner.ExecuteSqlNone(sql);
                }
                foreach (var item in mainform)
                {
                    updateValue += (item.Key + "='" + item.Value + "',");
                }                
                if (updateValue.Trim().Length > 1)
                {
                    updateValue = updateValue.Remove(updateValue.Length - 1, 1);
                }
                sql = $"update  [yaodaibao].[dbo].[OAProcessStatus]  set {updateValue}  where FID = {mainform["FID"]}";
                if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                {
                    throw new Exception("更新失败");
                }
                result = "<GetData> " +
                                    "<Result>True</Result>" +
                                    "<Description></Description>" +
                                    "<DataRows></DataRows></GetData>";
            }
            catch (Exception err)
            {
                result = "<GetData> " +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description>" +
                          "<DataRows></DataRows></GetData>";
            }
            return result;
        }
    }
}
