using iTR.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ydb.DataService
{
    public class Hospital
    {
        private string sql = "";
        private DataTable dt = null;
        private SQLServerHelper runner = null;
        private XmlDocument doc = null;

        public Hospital()
        {
            runner = new SQLServerHelper(DataHelper.CnnString);
            doc = new XmlDocument();
        }

        public string Upload(DataTable hospitalData)
        {
            string result = "";

            try
            {
                foreach (DataRow dr in hospitalData.Rows)
                {
                    try
                    {
                        #region XMLString

                        string xmlString = @"<UpdateHospital>
	                                <AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>
	                                <FClassID>aa6e8a63-1ce3-40ef-9254-0d6b2b3838dd</FClassID>
	                                <ID>{0}</ID>
	                                <FGrandID>6d14eafd-e3d8-4a8f-ae07-437670c8ef3f</FGrandID>
	                                <FProvinceID>0009fc01-5144-4310-bc19-1616b52decba</FProvinceID>
	                                <FLatitude>-1</FLatitude>
	                                <FLongitude>-1</FLongitude>
	                                <FAddress>{1}</FAddress>
                                     <FName>{2}</FName>
                                     <FNumber>{3}</FNumber>
                                     <Action>{4}</Action>
                                </UpdateHospital>";

                        #endregion XMLString

                        xmlString = string.Format(xmlString, dr["FID"].ToString(), dr["FName"].ToString(), dr["FName"].ToString(), dr["FCode"].ToString(), dr["FStatus"].ToString());

                        string xmlResult = ydb.DataService.DataHelper.HospitalDataInvoke("UpdateHospital", xmlString);
                        doc.LoadXml(xmlResult);
                        System.Diagnostics.Debug.WriteLine("同步结果：" + xmlResult);
                        if (doc.SelectSingleNode("UpdateHospital/Result").InnerText == "True")//YRB数据库上传成功，OA-YRB数据插入相应数据
                        {
                            if (dr["FStatus"].ToString() == "1")//新建
                            {
                                sql = @"INSERT INTO [DataService].[dbo].[YDBHospital]([FID],[FCode],[FName],[FTID])
                                VALUES('{0}','{1}','{2}','{3}') ";
                                sql = string.Format(sql, dr["FID"].ToString(), dr["FCode"].ToString(), dr["FName"].ToString(), dr["FTID"].ToString());
                            }
                            else
                            {
                                sql = "Update [DataService].[dbo].[YDBHospital] Set FCode='{1}',FName='{2}',FTID='{3}' where  FID='{0}' ";
                                sql = string.Format(sql, dr["FID"].ToString(), dr["FCode"].ToString(), dr["FName"].ToString(), dr["FTID"].ToString());
                            }
                            runner.ExecuteSqlNone(sql);
                            sql = $"update [DataService].[dbo].[OAHospital] set FStatus='1' Where FID='{dr["FID"].ToString()}'";
                            runner.ExecuteSql(sql);
                        }
                        else
                        {
                            // throw new Exception(doc.SelectSingleNode("UpdateEmployee/Description").InnerText.Trim());
                            sql = $"update [DataService].[dbo].[OAHospital] set FStatus='-1',FErrorMessage='{doc.SelectSingleNode("UpdateHospital/Description")?.InnerText ?? ""}' Where FID='{dr["FID"].ToString()}'";

                            runner.ExecuteSql(sql);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
            catch (Exception err)
            {
                // throw err;
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetUploadDataFromOA()
        {
            DataTable dtEmployee = null;

            sql = @"Delete from [DataService].dbo.OAHospital";
            runner.ExecuteSqlNone(sql);
            //将OA数据库中所有FTID（工号+职位+部门）在YRB中没有的提取处理（可能是新增或有变化的）
            sql = @"Insert Into [DataService].dbo.OAHospital(
                    [FID], [FCode], [FName], [FTID])
                    select ID, field0001,field0002,(cast(ID As nvarchar(50))+'-'+cast(field0002 As nvarchar(50))) AS FTID  from v3x.dbo.formmain_8044
                   where cast(ID As nvarchar(50))+'-'+cast(field0002 As nvarchar(50)) Not In(Select  FTID from [DataService].dbo.[YDBHospital])";
            runner.ExecuteSqlNone(sql);
            sql = @" Select  * from  [DataService].dbo.OAHospital";
            dtEmployee = runner.ExecuteSql(sql);
            foreach (DataRow dr in dtEmployee.Rows)
            {
                System.Diagnostics.Debug.WriteLine(dr["FID"] + "   " + DateTime.Now);
                sql = " Select FID From [DataService].dbo.YDBHospital Where FID='{0}'";
                sql = string.Format(sql, dr["FID"].ToString());
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)//在YRB数据中已有相应的工号，则为修改
                    dr["FStatus"] = 2;
            }
            return dtEmployee;
        }
    }
}