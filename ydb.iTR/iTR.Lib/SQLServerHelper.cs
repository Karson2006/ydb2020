using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security;


namespace iTR.Lib
{
    public class SQLServerHelper

    {
        //public string CnnString = "Data Source=192.168.20.13;Initial Catalog=yaodaibao;uid=sa;pwd=tengrui#erp20150926";
        //public string CnnString = "Data Source=.;Initial Catalog=yaodaibao;uid=sa;pwd=qazwsx";
        public string CnnString = "";
        private  SqlConnection dbCnn;
        private SqlTransaction trans ;
        public SQLServerHelper()
        {
            CnnString = ConfigurationManager.AppSettings["ConnectionString"];
            if (CnnString==null||CnnString.Trim().Length == 0)
                CnnString = "Data Source=.;Initial Catalog=yaodaibao;uid=sa;pwd=Tenry@20190123";
            else
            {

                string privateKey = "<RSAKeyValue>" +
                                    "<Modulus>7ZCc4DTkpqkzbOqwphgyLA6+/dRF3dKfCseIN6H5/7GLHc2BwGasQNgQwkAfH5iTKmsqJ4qqiksOvSRS5fH4Q8IQ4QLkyMuwWedZiuhCbyl+/NDtmckQV4a+9+byXxGdPYFfnQVEOi1qlsA+iCqP27GShx3tgol6fCL1dm94QCs=</Modulus>" +
                                    "<Exponent>AQAB</Exponent>" +
                                    "<P>9/HpQT2/5nlgw3l/mI6BvVnfrXW6Dguslucr2rdRGcjygZdbVcboZl9BMjJI2yW5oudNHAcTVUOBHKrlKL/NyQ==</P>" +
                                    "<Q>9UhfnVhOr9YAtZZVxRsXU7aJJ+8Mcaa/uo0mFYtMNfHJRDCylgLxRWaBbcGtF7pCRK3DYhxoNpJ9C0drIChIUw==</Q>" +
                                    "<DP>RaGddSIHU42A3ESxzcEvtGKaC5fFUY57wMFZMopK72fcmwJLtzIuMBnOMG+owEq+8H3uzNE737UefFOOGbyL6Q==</DP>" +
                                    "<DQ>hwFW31wDs3Su30Pn4Z2PsVv/EiPZTZTiYuPd2m3ZfLegeA/1u+vSsMhC5Q59H1o9r1+U8yN/mMn4WYTtyb1iUQ==</DQ>" +
                                    "<InverseQ>ijjAXuao2NvMzdGVro5zpyWif47HOm383UY9T/2kKgRnDMsg/2vR0cn9JwNQIleM++Xkk9U+/0EaBjD/Z0ZUlA==</InverseQ>" +
                                    "<D>tew3Zi67JrGN8wtqSVdgHIMSWXkUI8GmD3ArbUb6FofUm/cDNN6rbGDJvKez7dM+Z453Up6K6Kp/1/IYFYUN8wPVbd7n9wrnfneetIo1bkZ5b5OA/SRC6tr9lkhp4XvjRCvu7SO6pmrBWbx8vqosYbjiA9RrtpzIOb1edPm1DrE=</D>" +
                                    "</RSAKeyValue>";
                CnnString = iTR.Lib.Common.RSADecrypt(privateKey, CnnString);
            }


            dbCnn = new SqlConnection();
            dbCnn = new SqlConnection(CnnString);
            
            dbCnn.Open();
        }



        public SQLServerHelper(string connectionString)
        {
            this.CnnString = connectionString;
            dbCnn = new SqlConnection(CnnString);
            dbCnn.Open();
        }

        public SqlConnection databaseConnection
        {
            set { this.dbCnn = value; }
            get 
                {
                    if(dbCnn.State ==  ConnectionState.Closed)
                    {
                        dbCnn.ConnectionString = CnnString;
                        dbCnn.Open(); 
                    }
                    return this.dbCnn; 
                }
        }
        #region



        public int InsertDatafromDataSet(DataSet dataSource, string tableName, Boolean deleteoption = true, string cnnString = "")
        {
            int result = -1;

            if (deleteoption)
                ExecuteSqlNone("Delete from " + tableName);

            if (dbCnn.State == ConnectionState.Closed)
            {
                if (cnnString.Trim().Length > 0)
                    dbCnn.ConnectionString = cnnString;

                dbCnn.Open();
            }

            SqlBulkCopy sbc = new SqlBulkCopy(dbCnn);
            sbc.DestinationTableName =tableName;
            //将数据集合和目标服务器的字段对应 
            DataTable dt = dataSource.Tables[0];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
            }
            try
            {
                sbc.WriteToServer(dt);
            }
            catch (Exception ex) 
            {
                throw ex; 
            }
            finally
            {
                result = 1;
                dbCnn.Close();
                sbc.Close();
            }
            return result;
        }
        #endregion


        #region ExecuteSqlNone

        public int ExecuteSqlNone(string sql)
        {
            if(dbCnn.State == ConnectionState.Closed)
                dbCnn.Open();

            SqlCommand cd = new SqlCommand(sql, dbCnn);
            int result = cd.ExecuteNonQuery();
            dbCnn.Close();
            return result;

        }
        #endregion

        #region ExecuteSql
        public DataTable  ExecuteSql(string sql)
        {
            if (dbCnn.State == ConnectionState.Closed)
                dbCnn.Open();
            SqlCommand cmd = new SqlCommand(sql, dbCnn);
            DataTable  result = new DataTable() ;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(result);
            dbCnn.Close();
            return result;
        }
        #endregion

        #region ExecuteProcedure
        public DataTable ExecuteProcedure(String procedureName, Dictionary<String, object> parms)
        {
            DataTable result = null;
            //Dictionary<String, object> pa = new Dictionary<string, object>();

            SqlDataAdapter da = null;
            SqlCommand cmd = null;

            try
            {
                if (dbCnn.State == ConnectionState.Closed)
                    dbCnn.Open();

                    da = new SqlDataAdapter();
                    cmd = new SqlCommand(procedureName, dbCnn);
                    //cmd = _isTransactionOpen ? new SqlCommand(procedureName, _conn, _trans) : new SqlCommand(procedureName, _conn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parms != null && parms.Count > 0)
                    {
                        foreach (KeyValuePair<String, object> ele in parms)
                        {
                            cmd.Parameters.Add(new SqlParameter(ele.Key, ele.Value));
                        }
                    }

                    da.SelectCommand = cmd;

                    DataSet dsData = new DataSet();
                    da.Fill(dsData, "Procedure");


                    result = dsData.Tables["Procedure"];
                }
            catch (Exception ex)
            {
                throw ex;
                //_errorMsg = ex.Message;
                //LogWrite.WriteLogFile("方法：SQLServer.ExecuteProcedure 调用：" + procedureName + " 错误：" + ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (da != null) da.Dispose();
            }

            return result;
        }
        #endregion

        #region Update
        public int Update(DataTable dtTable, String syntaxSelect)
        {
            int result = -1;

            SqlDataAdapter da = null;
            try
            {

                if (dbCnn.State == ConnectionState.Closed)
                    dbCnn.Open();
                trans=dbCnn.BeginTransaction();
                da = new SqlDataAdapter();
                SqlCommandBuilder builder = new SqlCommandBuilder(da);
                da.SelectCommand = new SqlCommand(syntaxSelect, dbCnn);
                 
                result = da.Update(dtTable);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (result >= 0)
                {
                    trans.Commit();
                }
                else
                    trans.Rollback();
                trans.Dispose();
                
                //LogWrite.WriteLogFile("方法：SQLServer.Update 调用：result=" + result);
            }

            return result;
        }
        #endregion

    }
}
