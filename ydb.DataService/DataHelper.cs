using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTR.Lib;

namespace ydb.DataService
{
    public class DataHelper
    {
        static string WSURL = @"http://ydb.tenrypharm.com:6060/";
        //static string WSURL = @"http://192.168.20.149:6060/";
        public static string CnnString = @"Data Source=192.168.20.15;Initial Catalog=v3x;uid=sa;pwd=Tenry#369520";
        public static string EmployeeInvoke(string mothed,string xmlString)
        {
            string result = "";
            string[] parm = new string[] { mothed, xmlString };
          
            WebInvoke invoke = new WebInvoke();
            result = invoke.Invoke(WSURL +"OrganizationInvoke.asmx","OrganizationInvoke",mothed,parm).ToString();
            return result;
        }
        public static string OASyncInvoke(string mothed, string xmlString)
        {
            string result = "";
            string[] parm = new string[] { mothed, xmlString };

            WebInvoke invoke = new WebInvoke();
            result = invoke.Invoke(WSURL + "ReportDataInvoke.asmx", "ReportDataInvoke", mothed, parm).ToString();
            return result;
        }
        public static string AuthDatanvoke(string mothed, string xmlString)
        {
            string result = "";
            string[] parm = new string[] { mothed, xmlString };

            WebInvoke invoke = new WebInvoke();
            result = invoke.Invoke(WSURL + "AuthInvoke.asmx", "AuthInvoke", mothed, parm).ToString();
            return result;
        }

        public static string DeptDatanvoke(string mothed, string xmlString)
        {
            string result = "";
            string[] parm = new string[] { mothed, xmlString };

            WebInvoke invoke = new WebInvoke();
            result = invoke.Invoke(WSURL + "OrganizationInvoke.asmx", "OrganizationInvoke", mothed, parm).ToString();
            return result;
        }
    }
    
    
}
