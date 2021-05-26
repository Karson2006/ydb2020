using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTR.Lib;
using System.Data;
using ydb.BLL;
using System.Xml;

namespace ydb.DataService
{
    public class Employee
    {
        private string sql = "";
        private DataTable dt = null;
        private SQLServerHelper runner = null;
        private XmlDocument doc = null;

        public Employee()
        {
            runner = new SQLServerHelper(DataHelper.CnnString);
            doc = new XmlDocument();
        }

        public string Upload(DataTable employeeData)
        {
            string result = "";

            try
            {
                foreach (DataRow dr in employeeData.Rows)
                {
                    if (dr["FUploadOption"].ToString() == "1")//选择了上传的才上传
                    {
                        #region XMLString

                        string xmlString = @"<UpdateEmployee>
	                                <AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>
	                                <FClassID>63de6d24-41cc-4471-8876-90a765aa1614</FClassID>
	                                <ID>{0}</ID>
	                                <FName>{1}</FName>
	                                <FNumber>{2}</FNumber>
	                                <FParentID>-1</FParentID>
	                                <FDeptID>{3}</FDeptID>
	                                <FSortIndex>{4}</FSortIndex>
	                                <FPositionID>{5}</FPositionID>
	                                <FMobile>{6}</FMobile>
	                                <FLoginName>{7}</FLoginName>
	                                <FTypeID>{8}</FTypeID>
	                                <FIsAgency>{9}</FIsAgency>
	                                <FIDNumber>{10}</FIDNumber>
                                    <Action>{11}</Action>
                                </UpdateEmployee>";

                        #endregion XMLString

                        xmlString = string.Format(xmlString, dr["FID"].ToString(), dr["FEmployeeName"].ToString(), dr["FEmployeeNumber"].ToString(),
                                                            dr["FDeptID"].ToString(), dr["FSortIndex"].ToString(), dr["FPositionID"].ToString(),
                                                            dr["FMobile"].ToString(), dr["FMobile"].ToString(), dr["FTypeID"].ToString(),
                                                            dr["FIsAgency"].ToString(), dr["FIDNumber"].ToString(), dr["FStatus"].ToString());

                        string xmlResult = ydb.DataService.DataHelper.EmployeeInvoke("UpdateEmployee", xmlString);
                        doc.LoadXml(xmlResult);
                        if (doc.SelectSingleNode("UpdateEmployee/Result").InnerText == "True")//YRB数据库上传成功，OA-YRB数据插入相应数据
                        {
                            if (dr["FStatus"].ToString() == "1")//新建
                            {
                                sql = @"INSERT INTO [DataService].[dbo].[YDBEmployee]([FID],[FEmployeeName],[FDeptID],[FEmployeeNumber] ,[FDeptName],[FPositionName] ,[FTID],[FPositionID],[FMobile] ,[FTypeID])
                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ";
                                sql = string.Format(sql, dr["FID"].ToString(), dr["FEmployeeName"].ToString(), dr["FDeptID"].ToString(), dr["FEmployeeNumber"].ToString(),
                                                    dr["FDeptName"].ToString(), dr["FPositionName"].ToString(), dr["FTID"].ToString(),
                                                    dr["FPositionID"].ToString(), dr["FMobile"].ToString(), dr["FTypeID"].ToString());
                            }
                            else
                            {
                                sql = "Update [DataService].[dbo].[YDBEmployee] Set FDeptID='{0}',FDeptName='{1}',FPositionID='{2}',FPositionName='{3}' ,FMobile='{5}' , FTID='{6}' Where FID='{4}'";
                                sql = string.Format(sql, dr["FDeptID"].ToString(), dr["FDeptName"].ToString(), dr["FPositionID"].ToString(), dr["FPositionName"].ToString(), dr["FID"].ToString(), dr["FMobile"].ToString(), dr["FTID"].ToString());
                            }
                            runner.ExecuteSqlNone(sql);
                            sql = $"update [DataService].[dbo].[OAEmployee] set FUploadStatus='1' Where FID='{dr["FID"].ToString()}'";
                            runner.ExecuteSql(sql);
                        }
                        else
                        {
                            // throw new Exception(doc.SelectSingleNode("UpdateEmployee/Description").InnerText.Trim());
                            sql = $"update [DataService].[dbo].[OAEmployee] set FUploadStatus='-1',FErrorMessage='{doc.SelectSingleNode("UpdateEmployee/Description")?.InnerText ?? ""}' Where FID='{dr["FID"].ToString()}'";

                            runner.ExecuteSql(sql);
                        }
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

            sql = @"Delete from [DataService].dbo.OAEmployee";
            runner.ExecuteSqlNone(sql);
            //将OA数据库中所有FTID（工号+职位+部门）在YRB中没有的提取处理（可能是新增或有变化的）
            sql = @"Insert Into [DataService].dbo.OAEmployee(FID,FEmployeeName,FEmployeeNumber,FPositionName,FPositionID,FDeptID,FDeptName,FTID,FStatus,FMobile)
                    Select  cast(t1.ID as nvarchar(50)) As FID,t1.Name As FEmployeeName,t1.Code As FEmployeeNumber,t2.NAME As FPositionName,t1.ORG_POST_ID AS FPositionID,
                    cast(t1.ORG_DEPARTMENT_ID As nvarchar(50)) As FDeptID,t3.NAME As FDeptName,(t1.Code+'|'+cast(t1.ORG_POST_ID As nvarchar(50)) +'|'+cast(t1.ORG_DEPARTMENT_ID As nvarchar(50))+'|'+t1.EXT_ATTR_1) AS FTID, '1' As FStatus,t1.EXT_ATTR_1 As FMobile
                    From ORG_MEMBER t1
                    Left Join ORG_POST t2 on t1.ORG_POST_ID= t2.ID
                    Left Join ORG_UNIT t3 on t1.ORG_DEPARTMENT_ID= t3.ID
                    Where t1.IS_DELETED=0 and state= 1 and  Left(t3.PATH,16) In('0000000100220005','0000000100220015')
                    and (t1.Code+'|'+cast(t1.ORG_POST_ID As nvarchar(50)) +'|'+cast(t1.ORG_DEPARTMENT_ID As nvarchar(50))+'|'+t1.EXT_ATTR_1)  Not In(Select  FTID from [DataService].dbo.[YDBEmployee]) and t1.ORG_POST_ID<>-1549616942846885069";

            runner.ExecuteSqlNone(sql);
            sql = @" Select *,'0' As FSortIndex,'0' As FIsAgency,'' As FIDNumber,'' As FTypeID,'0' As FUploadOption from  [DataService].dbo.OAEmployee";
            dtEmployee = runner.ExecuteSql(sql);
            foreach (DataRow dr in dtEmployee.Rows)
            {
                sql = " Select FEmployeeNumber From [DataService].dbo.YDBEmployee Where FEmployeeNumber='{0}'";
                sql = string.Format(sql, dr["FEmployeeNumber"].ToString());
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)//在YRB数据中已有相应的工号，则为修改
                    dr["FStatus"] = 2;
            }
            return dtEmployee;
        }
    }
}