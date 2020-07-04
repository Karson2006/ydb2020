using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ydb.BLL
{
    public class BLCommon
    {
        private static string AuthCode = "1d340262-52e0-413f-b0e7-fc6efadc2ee5";//将来采用不对称密钥加密

        public BLCommon()
        {

        }
        public static Boolean CheckAuthCode(string code)
        {
            Boolean result = false;

            result = AuthCode == code.Trim() ? true : false;

            return result; 
        }
    }
     
}
