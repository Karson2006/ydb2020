using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;
using System.Configuration;


namespace ydb.BLL.Works
{
    public class SCM
    {
        public SCM()
        {

        }
        #region UpdateHospitalStock

        public string UpdateHospitalStock(string xmlString)
        {
            string id = "", sql = "", valueString = "";

            try
            {
                List<Dictionary<string, string>> formson = new List<Dictionary<string, string>>();
                Dictionary<string, string> mainform = Common.GetFieldValuesFromXmlEx(xmlString, "UpdateHospitalStock",out formson,"1","");
                //获取周序数
                int year, weekofyear;
                Common.GetWeekIndexOfYear(mainform["FWeekIndex"], out year,  out weekofyear);
                mainform["FYear"] = year.ToString();
                mainform["FWeekIndex"] = weekofyear.ToString();

                SQLServerHelper runner = new SQLServerHelper();

               

                if (mainform["FID"] == "-1" || mainform["FID"].Trim().Length == 0)
                {
                    //判断是否已存在相应的本周进销存记录
                    sql = "Select FID from HospitalStock Where FYear='{0}' and  FWeekIndex='{1}' and FEmployeeID ='{2}' and  FHospitalID='{3}'";
                    sql = string.Format(sql, mainform["FYear"], mainform["FWeekIndex"], mainform["FEmployeeID"], mainform["FHospitalID"]);
                    DataTable dt = runner.ExecuteSql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        mainform["FID"] = dt.Rows[0]["FID"].ToString();
                        id = mainform["FID"];
                    }
                    else
                    {
                        id = Guid.NewGuid().ToString();
                        sql = "Insert Into HospitalStock(FID) Values('" + id + "')";
                        runner.ExecuteSqlNone(sql);
                    }
                }
                else
                    id = mainform["FID"];

                foreach (string key in mainform.Keys)
                {
                    if (key == "FID") continue;
                    valueString = valueString + key + "='" + mainform[key] + "',";
                }

                if (valueString.Length > 0)
                    sql = "Update HospitalStock Set " + valueString.Substring(0, valueString.Length - 1) + " Where  FID ='" + id + "'";

                runner.ExecuteSqlNone(sql);
                //插入明细表
                sql = "Delete from [HospitalStock_Detail] Where FFormmainID='" + id + "'";
                runner.ExecuteSqlNone(sql);
                foreach (Dictionary<string, string> dic in formson)
                {
                    sql = @"Insert  Into HospitalStock_Detail(FFormmainID,FProductID,FStock_IB,FStock_IN,FStock_EB,FSaleAmount)
                             Values('{0}','{1}',{2},{3},{4},{5})";
                    sql = string.Format(sql,id,dic["FProductID"], dic["FStock_IB"], dic["FStock_IN"], dic["FStock_EB"], dic["FSaleAmount"]);
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                id = " - 1";
                throw err;
            }
            return id;
        }
        #endregion

        #region GetHospitalStockDetail
        public string GetHospitalStockDetail(string xmlString)
        {
            string sql = "", where = "",id="";
            string result = "";

            result = "<GetHospitalStockDetail>" +
                         "<Result>False</Result>" +
                         "<Description></Description>" +
                         "<DataRows></DataRows>" +
                         "</GetHospitalStockDetail>";
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param = Common.GetFieldValuesFromXml(xmlString, "GetHospitalStockDetail", "", "0");

                SQLServerHelper runner = new SQLServerHelper();
                if(param.ContainsKey ("FID") && param["FID"].Length >0)//有ID
                {
                    id = param["FID"];
                    where = " t2.FID='" + id + "'";
                }
                else
                {
                    int year, weekofYear;
                    Common.GetWeekIndexOfYear(param["FWeekIndex"], out year, out weekofYear);
                    param["FYear"] = year.ToString();
                    param["FWeekIndex"] = weekofYear.ToString();
                    foreach (string key in param.Keys)
                    {
                        if (key.ToUpper() == "FID" ||   key.ToUpper() == "FPRODUCTID" ) continue;
                        if (param[key].Trim().Length > 0)
                        {
                            where =  where.Trim().Length==0 ? "t2."+key + "='" + param[key] + "' " : where + " and " + "t2."+key + "='" + param[key] + "' ";
                        }
                    }
                    //where = "t2.FEmployeeID='{0}' and FHospitalID ='{1}' and  t2.FWeekIndex='{2}' and t2.FYear ='{3}'";
                    //where = string.Format(where, param["FEmployeeID"], param["FHospitalID"], weekofYear, year); 
                }
                sql = @"Select t2.FDate,t2.FEmployeeID,t2.FHospital,t2.FHospitalID,t2.FID,t2.FWeekIndex,t2.FYear,Isnull(t3.FName,'') AS FEmployeeName
                                From HospitalStock t2 
                                Left Join t_Items t3 On t2.FEmployeeID = t3.FID  Where  {0}";
                sql = string.Format(sql, where);

                DataTable maindt = runner.ExecuteSql(sql);
                if(maindt.Rows.Count >0)
                {
                    id = maindt.Rows[0]["FID"].ToString() ;
                    sql = @"Select  t1.FProductID,Isnull(t3.FName,'') AS  FProductName,t1.FStock_IB,t1.FSaleAmount,t1.FStock_EB,t1.FStock_IN
                                From  HospitalStock_Detail t1
                                Left Join HospitalStock t2 On t1.FFormmainID = t2.FID
                                Left Join t_Items t3 On t1.FProductID = t3.FID";
                    sql = sql + "  Where  FFormmainID='" + id + "'";
                    if (param.ContainsKey("FProductID"))
                    {
                        if (param["FProductID"].Trim().Length > 0)
                            sql = sql + "  and  FProductID ='" + param["FProductID"] + "'";
                    }

                    DataTable sondt = runner.ExecuteSql(sql);

                    result = Common.DataTableToXmlEx(maindt, sondt, "GetHospitalStockDetail");
                }

            }
            catch (Exception err)
            {
                throw err;
            }

            return result;
        }

        #endregion

    }



}
