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
    public class Department
    {
        private string sql = "";
        private DataTable dt = null;
        private SQLServerHelper runner = null;
        private XmlDocument doc = null;

        public Department()
        {
            runner = new SQLServerHelper(DataHelper.CnnString);
            doc = new XmlDocument();
        }

        public string Upload(DataTable dt)
        {
            string result = "";

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["FUploadOption"].ToString() == "1")//选择了上传的才上传
                    {
                        #region XMLString

                        string xmlString = @"<UpdateDepartment>
	                                <AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>
	                                <FClassID>d8dbcc3f-dc92-4695-9542-a2ac4f00c162</FClassID>
	                                <ID>{0}</ID>
	                                <FName>{1}</FName>
	                                <FNumber>{2}</FNumber>
	                                <FParentID>{3}</FParentID>
	                                <FSortIndex>{4}</FSortIndex>
	                                <FSupervisorID>{5}</FSupervisorID>
                                    <Action>{6}</Action>
                                </UpdateDepartment>";

                        #endregion XMLString

                        xmlString = string.Format(xmlString, dr["FDeptID"].ToString(), dr["FDeptName"].ToString(), dr["FDeptNumber"].ToString(),
                                                            dr["FParentID"].ToString(), 0, dr["FSupervisorID"].ToString(), dr["FAction"].ToString());
                        System.Diagnostics.Trace.WriteLine(xmlString);
                        string xmlResult = ydb.DataService.DataHelper.DeptDatanvoke("UpdateDepartment", xmlString);
                        System.Diagnostics.Trace.WriteLine("结果");
                        System.Diagnostics.Trace.WriteLine(xmlResult);
                        doc.LoadXml(xmlResult);
                        if (doc.SelectSingleNode("UpdateDepartment/Result").InnerText == "True")//YRB数据库上传成功，OA-YRB数据插入相应数据
                        {
                            if (dr["FACtion"].ToString() == "1")//新建
                            {
                                sql = @"INSERT INTO [DataService].[dbo].[YDBDepartment]([FDeptID],[FDeptName],[FDeptNumber],[FSupervisorID] ,[FSupervisorName],[FParentID] ,[FTID],[FParentName])
                                VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ";
                                sql = string.Format(sql, dr["FDeptID"].ToString(), dr["FDeptName"].ToString(), dr["FDeptNumber"].ToString(), dr["FSupervisorID"].ToString(),
                                                    dr["FSupervisorName"].ToString(), dr["FParentID"].ToString(), dr["FTID"].ToString(), dr["FParentName"].ToString());
                            }
                            else
                            {
                                sql = "Update [DataService].[dbo].[YDBDepartment] Set FParentID='{0}',FParentName ='{1}',FSupervisorID='{2}',FSupervisorName='{3}',FTID='{5}' Where FDeptID='{4}'";
                                sql = string.Format(sql, dr["FParentID"].ToString(), dr["FParentName"].ToString(), dr["FSupervisorID"].ToString(), dr["FSupervisorName"].ToString(), dr["FDeptID"].ToString(), dr["FTID"].ToString());
                            }
                            runner.ExecuteSqlNone(sql);
                            sql = $"update [DataService].[dbo].[OADepartment] set FUploadStatus='1' Where FID='{dr["FID"].ToString()}'";
                            runner.ExecuteSql(sql);
                        }
                        else
                        {
                            
                            sql = $"update [DataService].[dbo].[OADepartment] set FUploadStatus='-1',FErrorMessage='{doc.SelectSingleNode("UpdateDepartment/Description").InnerText}' Where FID='{dr["FID"].ToString()}'";
                            runner.ExecuteSql(sql);
                            //throw new Exception(doc.SelectSingleNode("UpdateDepartment/Description").InnerText.Trim());
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
            DataTable dtDept = null;

            sql = @"Delete from [DataService].dbo.OADepartment";
            runner.ExecuteSqlNone(sql);
            //将OA数据库中所有FTID（部门ID+主管ID+上级部门ID）在YRB中没有的提取处理（可能是新增或有变化的）
            sql = @"Insert Into [DataService].dbo.OADepartment(FDeptID,FDeptName,FSupervisorID,FParentID,FLevel,FDeptNumber,FParentName,FSupervisorName,FDetail,FTID)
                                   Select t2.ID As FDeptID,t2.Name As FDeptName,t1.field0005 As FSupervisorID,t1.field0012 As FParentID,t1.field0004 As FLevel,
               t1.field0001 As FDeptNumber,t3.Name As FParentName,isnull(t4.Name,'') As FSupervisorName, 1 AS FDetail,
                    (t1.field0002+'_'+ t1.field0005+'_'+t1.field0012) As FTID
                    From v3x.dbo.formmain_8662 t1
                    Left Join v3x.dbo.ORG_UNIT t2 On t1.field0002= t2.ID
                    Left Join v3x.dbo.ORG_UNIT t3 On t1.field0012= t3.ID
                    Left Join v3x.dbo.ORG_MEMBER t4 On t1.field0005= t4.ID
                    Where  Left(t2.PATH,16) In ('0000000100220005','0000000100220015') and  (t1.field0002+'_'+ CAST(t1.field0004 as nvarchar(500)) +'_'+t1.field0005) Not In(Select FTID From [DataService].dbo.[YDBDepartment])
                    Order by t1.field0009 asc,t1.field0012 ";

            runner.ExecuteSqlNone(sql);
            sql = @" Select *,'0' As FUploadOption,'1' As FAction from  [DataService].dbo.OADepartment";
            dtDept = runner.ExecuteSql(sql);
            foreach (DataRow dr in dtDept.Rows)
            {
                sql = "Select FDeptID from [DataService].dbo.YDBDepartment Where  FDeptID ='" + dr["FDeptID"].ToString() + "'";
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                    dr["FAction"] = 2;
            }
            return dtDept;
        }
    }
}