using iTR.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ydb.Report
{
    /// <summary>
    /// 罗盘一些共用方法
    /// </summary>
    public class ReportHelper
    {

        /// <summary>
        /// 根据查询条件获取具体的开始时间和结束时间
        /// </summary>
        /// <param name="weekIndex"></param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetPerTime(string weekIndex)
        {
            DateTime startTime, endTime;
            string temptime;
            try
            {

                switch (weekIndex)
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
                return Tuple.Create(startTime, endTime);

            }
            catch (Exception err)
            {

                throw err;
            }
        }

 
        /// <summary>
        /// 根据查询条件获取周具体属于哪一年
        /// </summary>
        /// <param name="weekIndex"></param>
        /// <returns>返回结果例如 202035|202036|202037 </returns>
        public static string GetYearWithWeeks(string weekIndex) {
            string years, weekOfyears;
            List<string> list = new List<string>();
            Common.GetWeekIndexOfYearEx(weekIndex, out years, out weekOfyears);
            string[] weeks =weekOfyears.Split('|');
            for (int i = 0; i < weeks.Length; i++)
            {
                list.Add("'"+years + weeks[i]+"'");
            }
            return string.Join(",",list.ToArray());
        }
            
    }
}
