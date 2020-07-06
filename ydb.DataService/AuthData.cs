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
        public class AuthData
        {
            private string sql = "";
            private DataTable dt = null;
            SQLServerHelper runner = null;
            XmlDocument doc = null;
            public AuthData()
            {
                runner = new SQLServerHelper(DataHelper.CnnString);
                doc = new XmlDocument();

            }
            public string Upload(DataTable dt)
            {
                string result = "", DevelopeManagerID = "", RepresentativeID = "", MarketingManagerID = "", ManagerID = "";
                
                
                
               

                try
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["FUploadOption"].ToString() == "1")//选择了上传的才上传
                        {
                            DevelopeManagerID = "";
                            RepresentativeID = ""; 
                            MarketingManagerID = "";
                            ManagerID = "";
                            switch(dr["FAuthType"].ToString().Trim())
                            {
                                 case "3":
                                    DevelopeManagerID = dr["FObjectID"].ToString().Trim();
                                    break;
                                case "4":
                                    RepresentativeID = dr["FObjectID"].ToString().Trim();
                                    break;
                                case "5":
                                    MarketingManagerID = dr["FObjectID"].ToString().Trim();
                                    break;
                                case "6":
                                    ManagerID = dr["FObjectID"].ToString().Trim();
                                    break;
                            }

                            #region  XMLString
                            string xmlString = @"<UpdateHospitalOwners>
	                                    <AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>
                                        <DevelopeManagerID>{0}</DevelopeManagerID>
	                                    <RepresentativeID>{1}</RepresentativeID>
	                                    <MarketingManagerID>{2}</MarketingManagerID>
	                                    <ManagerID>{3}</ManagerID>
                                        <HospitalID>{4}</HospitalID>
                                    </UpdateHospitalOwners>";
                            #endregion

                            xmlString = string.Format(xmlString, DevelopeManagerID, RepresentativeID, MarketingManagerID, ManagerID, dr["FInstitutionID"].ToString().Trim());

                            string xmlResult = ydb.DataService.DataHelper.AuthDatanvoke("UpdateHospitalOwners", xmlString);
                            doc.LoadXml(xmlResult);
                            if (doc.SelectSingleNode("UpdateHospitalOwners/Result").InnerText == "True")//YRB数据库上传成功，OA-YRB数据插入相应数据
                            {
                                
                                sql = @"INSERT INTO [DataService].[dbo].[YDBAuthData]([FTID],[FInstitutionNumber] ,[FInstitutionNanme] ,[FObjectID],[FObjectNumber],[FObjectName] ,[FAuthType],[FInstitutionID])
                                        VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ";
                                sql = string.Format(sql, dr["FTID"].ToString(), dr["FInstitutionNumber"].ToString(), dr["FInstitutionNanme"].ToString(), dr["FObjectID"].ToString(),
                                                        dr["FObjectNumber"].ToString(), dr["FObjectName"].ToString(), dr["FAuthType"].ToString(), dr["FInstitutionID"].ToString());
                               
                                runner.ExecuteSqlNone(sql);
                            }
                        }
                    }
                }

                catch (Exception err)
                {
                    throw err;
                }
                return result;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public DataTable GetUploadDataFromOA()
            {
                DataTable dt = null,newDt=null;

                sql = @"Delete from [DataService].dbo.OAAuthData";
                runner.ExecuteSqlNone(sql);

                sql = @"Insert Into [DataService].dbo.[OAAuthData](FInstitutionNumber,FInstitutionNanme,FObjectID,FObjectNumber,FObjectName,FAuthType,FTID,FInstitutionID)
                        Select  t1.field0001 As FInstitutionNumber , t1.field0002 FInstitutionNanme ,t1.field0005 AS FObjectID, t2.CODE  As FObjectNumber,t2.NAME AS FObjectName, 4 As FAuthType,(t1.field0001 +'-' + t2.CODE ) AS FTID,t3.ID AS FInstitutionID  --医药代表 
                        from formmain_5765 t1
                        Left Join ORG_MEMBER t2 On t1.field0005=  Cast(t2.ID  AS  nvarchar(20))
                        Left Join formmain_4851 t3 On t1.field0001= t3.field0001
                        Where Isnull(t1.field0005,'')<>'' and  Isnull(t1.field0001,'')<>'' and  Isnull(t1.field0005,'')<>'' and (t1.field0001 +'-' + t2.CODE ) Not In(Select Distinct FTID from [DataService].dbo.[YDBAuthData])
                        union
                        Select   t1.field0001 FInstitutionNumber, t1.field0002 FInstitutionNanme ,t1.field0006 AS FObjectID,  t2.CODE  As FObjectNumber,t2.NAME AS FObjectName, 3 As FAuthType,(t1.field0001 +'-' + t2.CODE ) AS FTID,t3.ID AS FInstitutionID  --招商经理
                        from formmain_5765 t1
                        Left Join ORG_MEMBER t2 On t1.field0006=  Cast(t2.ID  AS  nvarchar(20)) 
                        Left Join formmain_4851 t3 On t1.field0001= t3.field0001
                        Where Isnull(t1.field0006,'')<>'' and  Isnull(t1.field0001,'')<>'' and  Isnull(t1.field0006,'')<>''and (t1.field0001 +'-' + t2.CODE ) Not In(Select Distinct FTID from [DataService].dbo.[YDBAuthData])
                        union
                        Select   t1.field0001 FInstitutionNumber, t1.field0002 FInstitutionNanme ,t1.field0007 AS FObjectID,  t2.CODE  As FObjectNumber,t2.NAME AS FObjectName, 5 As FAuthType,(t1.field0001 +'-' + t2.CODE ) AS FTID,t3.ID AS FInstitutionID  --市场经理
                        from formmain_5765 t1
                        Left Join ORG_MEMBER t2 On t1.field0007=  Cast(t2.ID  AS  nvarchar(20))
                        Left Join formmain_4851 t3 On t1.field0001= t3.field0001
                        Where Isnull(t1.field0007,'')<>''and  Isnull(t1.field0001,'')<>'' and  Isnull(t1.field0007,'')<>'' and (t1.field0001 +'-' + t2.CODE ) Not In(Select Distinct FTID from [DataService].dbo.[YDBAuthData])
                        union
                        Select   t1.field0001 FInstitutionNumber, t1.field0002 FInstitutionNanme ,t1.field0017 AS FObjectID,  t2.CODE  As FObjectNumber,t2.NAME AS FObjectName, 6 As FAuthType,(t1.field0001 +'-' + t2.CODE )  AS FTID,t3.ID AS FInstitutionID  --地区经理
                        from formmain_5765 t1
                        Left Join ORG_MEMBER t2 On t1.field0017=  Cast(t2.ID  AS  nvarchar(20)) 
                        Left Join formmain_4851 t3 On t1.field0001= t3.field0001
                        Where Isnull(t1.field0017,'')<>''and  Isnull(t1.field0001,'')<>'' and  Isnull(t1.field0017,'')<>'' and (t1.field0001 +'-' + t2.CODE ) Not In(Select Distinct FTID from [DataService].dbo.[YDBAuthData]) ";

                runner.ExecuteSqlNone(sql);
                sql = @" Select *,'0' As FUploadOption from [DataService].dbo.[OAAuthData] order by  FTID,FAuthType Desc";
                dt = runner.ExecuteSql(sql);
                string oldFTID = "";
                newDt = dt.Clone(); 
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["FTID"].ToString() != oldFTID)
                    {
                        oldFTID = dr["FTID"].ToString();
                        newDt.ImportRow(dr); 
                    }

                }
                return newDt;
            }
        }

    }
   
