using System;
using System.Configuration;
using System.IO;
using System.Threading;


namespace iTR.Lib
{
    /// <summary>
    /// 写日志类
    /// </summary>
    public class FileLogger
    {
        #region 字段
        public static object _lock = new object();
        #endregion

        #region 写文件
        /// <summary>
        /// 写文件
        /// </summary>
        public static void WriteFile(string log, string path,int mode=1)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(delegate(object obj)
            {
                lock (_lock)
                {
                    if (!File.Exists(path))
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create)) { }
                    }

                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            #region 日志内容
                            string value = string.Format(@"{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), obj.ToString());
                            #endregion

                            sw.WriteLine(value);
                            sw.Flush();
                        }
                    }
                }
            }));
            thread.Start(log);
        }
        #endregion

        #region 写日志
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">日志信息</param>
        /// <param name="mode">1：写入数据库，其他：写文件，执行程序根目录yyyyMMdd目录中</param>
        /// <param name="caller">调用对象，对写日志文件无效</param>
        /// <param name="method">调用方法对写日志文件无效</param>
        public static void WriteLog(string log,int mode = 1,string caller="",
                                    string method="",string dataBase ="yaodaibao",string logtype="App") 
        {
             String strPath = AppDomain.CurrentDomain.BaseDirectory;
             if (mode == 1)//写入数据库
             {
                 WriteToDB(log, logtype,caller, method,dataBase);
             }
             else//写入Log文件
             {
                 if (strPath != "")
                 {
                     strPath = strPath + (strPath.Substring(strPath.Length - 1) == @"\" ? "" : @"\") + DateTime.Now.ToString("yyyyMMdd");
                     if (!Directory.Exists(strPath))
                         Directory.CreateDirectory(strPath);
                 }
                 string logPath = strPath + "\\iTR_Log.txt";
                 WriteFile(log, logPath);
             }
 
            
        }
        #endregion

        public static void WriteToDB(string log, string type = "AppMessage", string caller = "",string method="",string database ="yaodaibao")
        {
            try
            {
                string sql = "Insert Into "+ database+".dbo.AppLogs(FCaller,FMethod,FLog,FType)Values('{0}','{1}','{2}','{3}')";
                sql = string.Format(sql,  caller, method, log.Replace("'","''"), type);
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                runner = null;
            }
            catch (Exception err)
            {
                throw err;
            }
            
        }


        #region 写错误日志
        /// <summary>
        /// 写错误日志
        /// </summary>
        public static void WriteErrorLog(string log,int mode=1,string caller="",string method="")
        {
            if (mode == 1)
            {
                WriteToDB(log, "ErrMessage", caller, method);
            }
            else
            {
                string logPath = ConfigurationManager.AppSettings["LogPath"] + "\\iTService_ErrorLog.txt";

                WriteFile(log, logPath);
            }
        }
        #endregion

    }
}
