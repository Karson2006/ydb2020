using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ydb.Report
{
    /// <summary>
    /// 罗盘用到相关实体类
    /// </summary>
    class ReportAboutEntity
    {
    }
    //json转实体不区分大小写
    public class RouteEntity
    {        
        public string AuthCode { get; set; }
        public string EndTime { get; set; }
        public string StartTime { get; set; }
        //主页用到
        public string EmployeeId { get; set; }
        //子页用到

        public string EmployeeIds { get; set; }
        public string FWeekIndex { get; set; }
        //罗盘类型ID 1,签到，2,拜访，3,流程，4,支付,5,艾夫吉夫 6,数量 7,，8，奖金
        public int ChildType { get; set; }
        //有没有下一页
        public string NextPage { get; set; }
        //查询需要
        public string FilterType { get; set; }

    }
}
