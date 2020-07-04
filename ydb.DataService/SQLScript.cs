using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTR.Lib; 
using System.Data;

namespace ydb.DataService
{
    public class SQLScript
    {
        public static DataTable GetTableList()
        {
            DataTable dt = null;
            string sql = "Select [name],[id] from [sysobjects] where xtype='U' Order By [name]";
            SQLServerHelper runner = new SQLServerHelper();
            dt = runner.ExecuteSql(sql);
            return dt;
        }

        public static DataTable GetTableFiedList(string tbID)
        {
            DataTable dt = null;
            string sql = "select name,xtype  from syscolumns where  id='" + tbID + "' Order by [name]";
            SQLServerHelper runner = new SQLServerHelper();
             dt = runner.ExecuteSql(sql);

            return dt;
        }

        public static DataTable GetRecordSet(string sql)
        {
            DataTable dt = null;
            SQLServerHelper runner = new SQLServerHelper();
            dt = runner.ExecuteSql(sql);
            return dt;
        }
    }
    
}
