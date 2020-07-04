using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iTR.Lib;
using System.Xml;

namespace ydb.BLL
{
    public class Employee
    {
        Items iClass;
        public Employee()
        {
            iClass = new Items();
        }

        #region GetEmployeeDetail
        public string GetEmployeeDetail(string xmlString)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetEmployeeDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetEmployeeDetail>", "GetEmployeeList>");
                    result = GetEmployeeList(xmlString).Replace("GetEmployeeDetail>", "GetEmployeeDetail>");
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetEmployeeList
        public string GetEmployeeList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FIsDeleted=0 ", val = "";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetEmployeeList/ID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetEmployeeList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetEmployeeList/Number");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FNumber like '%" + val + "%'" : "t2.FNumber like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetEmployeeList/PositionName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t3.FName like '%" + val + "%'" : " t3.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetEmployeeList/TypeID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FTypeID = '" + val + "'" : " t1.FTypeID = '" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetEmployeeList/IsAgency");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FIsAgency = '" + val + "'" : " t1.FIsAgency = '" + val + "'";
                }

                sql = "Select t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t4.FName As FDepartmentName,t3.FName As FPostitionName,t2.FClassID," +
                    " t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,t5.FName As FTypeName" +
                    " From t_Employees t1" +
                    " Left Join t_Items t2 On t1.FID=t2.FID" +
                    " Left Join t_Items t3 On t3.FID= t1.FPositionID" +
                    " Left Join t_Items t4 On t4.FID= t1.FDeptID" +
                    " Left Join t_Items t5 On t5.FID= t1.FTypeID";
               if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " order by t1.FSortIndex Asc";

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetEmployeeList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetEmployeeList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetEmployeeList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FName");
                        vNode.InnerText = row["FName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FClassID");
                        vNode.InnerText = row["FClassID"].ToString();
                        cNode.AppendChild(vNode);

                        

                        vNode = doc.CreateElement("FDeptID");
                        vNode.InnerText = row["FDeptID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FNumber");
                        vNode.InnerText = row["FNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FFullNumber");
                        vNode.InnerText = row["FFullNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIsDetail");
                        vNode.InnerText = row["FIsDetail"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLevel");
                        vNode.InnerText = row["FLevel"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDepartmentName");
                        vNode.InnerText = row["FDepartmentName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPostitionName");
                        vNode.InnerText = row["FPostitionName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPositionID");
                        vNode.InnerText = row["FPositionID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FMobile");
                        vNode.InnerText = row["FMobile"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLoginName");
                        vNode.InnerText = row["FLoginName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLoginPwd");
                        vNode.InnerText = row["FLoginPwd"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FMail");
                        vNode.InnerText = row["FMail"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRoleID");
                        vNode.InnerText = row["FRoleID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPageID");
                        vNode.InnerText = row["FPageID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FWechat");
                        vNode.InnerText = row["FWechat"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRemark");
                        vNode.InnerText = row["FRemark"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTypeID");
                        vNode.InnerText = row["FTypeID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTypeName");
                        vNode.InnerText = row["FTypeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIsAgency");
                        vNode.InnerText = row["FIsAgency"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetEmployeeList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetEmployeeList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region Update
        public string Update(string dataString)
        {
            string id = "", sql = "",  valueString = "",dptId="";
            string result = "-1";

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                dataString = dataString.Replace("UpdateEmployee>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FDeptID");
                string val = "";
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("所在部门ID不能为空");
                else
                {
                    val = vNode.InnerText;
                    dptId = val;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDeptID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FPositionID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("职位ID不能为空");
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FPositionID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FLoginName");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("登录名不能为空");
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FLoginName='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FLoginPwd");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                    throw new Exception("登录密码不能为空");
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FLoginPwd='" + val + "',";
                }

                id = iClass.Update(dataString);
                if (id == "-1")//插入t_items表错误
                    result = "-1";
                if (doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "" || doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "-1")//新增
                {
                    sql = "Insert into t_Employees(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入新日程失败
                        throw new Exception("新建失败");
                }
                vNode = doc.SelectSingleNode("UpdateItem/FIntroduce");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIntroduce='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FMobile");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FMobile='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FMail");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FMail='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FRoleID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRoleID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FPageID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FPageID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FRemark");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRemark='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FWechat");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FWechat='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FTypeID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTypeID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FIsAgency");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIsAgency='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FSortIndex");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSortIndex='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update t_Employees Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新失败");
                    }

                }

                if(dptId!="-1")
                {
                    WorkShip ws = new WorkShip();
                    ws.Update(dptId);
                }
            }
            catch (Exception err)
            {
                if(id!="-1")//t_tems已插入数据成功，要删除
                {
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_Employees Where FID='" + id + "'";
                    runner.ExecuteSqlNone(sql);
                }
                throw err;
            }
            result = id;

            return result;
        }

        
        #endregion  

        #region Delete
        public string Delete(string xmlString)
        {
            string result = "-1", id = "-1", sql = "";
            XmlDocument doc = new XmlDocument();

            try
            {
                xmlString = xmlString.Replace("DeleteEmployee>", "DeleteItem>");
                Items item = new Items();
                id = item.Delete(xmlString);

                if (id.Trim() != "-1")//t_Items删除成功
                {
                    sql = " Update t_Employees Set FIsDeleted =1 Where  FID='" + id + "' And FIsDeleted=0   Update t_Workships Set FIsDeleted =1 Where  FEmployeeID='" + id + "' And FIsDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                sql = "Update t_Employees Set FIsDeleted =0 Where FID='" + id + "' And FIsDeleted=1   Update t_Workships Set FIsDeleted =0 Where  FEmployeeID='" + id + "' And FIsDeleted=1  Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region AppendFromRegApplication

        public string AppendFromRegApplication(string regApplicationID)
        {
            string id = "-1", xmlData = "",instituteID="";
            string  result = "";
            XmlDocument doc = new XmlDocument();
            try
            {
                string sql ="Select t1.*,t2.FName" +
                            "From Reg_Application t1" +
                            "Left Join Reg_Representative t2 On t1.FID= t2.FApplicationID"+
                            "Where FID='" + regApplicationID + "' and FRegisted=1";

                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)//该申请已审批
                {
                    if(dt.Rows[0]["FProductCategoryID"].ToString().Equals(" ab867182-94c6-4385-82e7-decbefbeb105"))//专科药
                    {
                        instituteID= dt.Rows[0]["FHospitalID"].ToString();
                    }
                    else
                    {
                        instituteID= dt.Rows[0]["FCountryID"].ToString();
                    }
                    sql="Select * from AuthData Where FIsDeleted=0 And FInstitutionID='"+instituteID+"' And FBeginDate<="+ DateTime.Now.ToString("yyyy-MM-dd") +" 23:59:59";
                    sql="Select t1.*,t2.FDeptID "+
                        " From AuthData t1"+
                        " Left Join t_Employees t2 On t1.FEmployeeID= t2.FID"+
                        " Where t1.FIsDeleted=0 And t1.FInstitutionID=''"+instituteID+"' And t1.FBeginDate<='"+ DateTime.Now.ToString("yyyy-MM-dd") +" 23:59:59'";
                    sql = sql + " And t2.FTypeID='3cc192c2-122b-4f5c-bc5e-904dbae26070'";//只查询招商类型
                    string dptId ="";
                    DataTable dtAuthData = runner.ExecuteSql(sql);
                    if(dtAuthData.Rows.Count>0)
                        dptId=dtAuthData.Rows[0]["FDeptID"].ToString();
                   
                    xmlData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                "<UpdateEmployee>" +
                                "<AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>" +
                                "<FClassID>63de6d24-41cc-4471-8876-90a765aa1614</FClassID>" +
                                "<ID></ID>" +
                                "<FName>" + dt.Rows[0]["FName"].ToString() + "</FName>" +
                                "<FNumber>A"+DateTime.Now.ToString("yyyyMMddHHmmss")+"</FNumber>" +
                                "<FParentID>-1</FParentID>" +
                                "<FDeptID>"+dptId+"</FDeptID>" +
                                "<FSortIndex></FSortIndex>" +
                                "<FPositionID>098f2a1b-be95-414f-8d76-475891bcdef0</FPositionID>" +//医药代码
                                "<FMobile>"+ dt.Rows[0]["FMobile"].ToString()  +"</FMobile>" +
                                "<FLoginName>"+ dt.Rows[0]["FMobile"].ToString()  +"</FLoginName>" +
                                "<FLoginPwd></FLoginPwd>" +
                                "<FMail></FMail>" +
                                "<FRoleID></FRoleID>" +
                                "<FPageID></FPageID>" +
                                "<FWechat></FWechat>" +
                                "<FRemark></FRemark>" +
                                "<FTypeID></FTypeID>" +
                                "<FIsAgency>1</FIsAgency>" +//授权医药代表
                                "</UpdateEmployee>";
                    Employee emp = new Employee();
                    id = emp.Update(xmlData);
                }
            }
            catch (Exception err)
            {
                id = "-1";
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region ChangePassword
        //修改建档表中的登录密码
        public string ChangePassword(string xmlString)
        {
            string result = "", sql = "", loginName = "", loginpwd="",confirmpwd="";
            Boolean checkLogin = true;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("ChangePassword/LoginName");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    checkLogin = false;
                else
                    loginName = vNode.InnerText.Trim();

                if (checkLogin)
                {
                    vNode = doc.SelectSingleNode("ChangePassword/Mobile");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        checkLogin = false;
                    else
                        loginName = vNode.InnerText.Trim();
                }
                vNode = doc.SelectSingleNode("ChangePassword/Password");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("密码不能为空");
                else
                    loginpwd = vNode.InnerText.Trim();

                vNode = doc.SelectSingleNode("ChangePassword/ConfirmPassword");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("确认密码不能为空");
                else
                    confirmpwd = vNode.InnerText.Trim();

                if (loginpwd != confirmpwd)
                    throw new Exception("密码与确认密码不一致");

                sql = "Update t_Employees Set FLoginPwd='" + vNode.InnerText.Trim() + "' Where FMobile='" + doc.SelectSingleNode("UpdateRegistration/Mobile").InnerText.Trim() + "' ";
                SQLServerHelper runner = new SQLServerHelper();
                if (runner.ExecuteSqlNone(sql) < 1)
                    throw new Exception("修改密码失败");
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><UpdateRegistration>" +
                           "<Result>True</Result>" +
                           "<Description>成功</Description>" +
                           "</UpdateRegistration>";
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><UpdateRegistration>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description>" +
                          "</UpdateRegistration>";
                throw err;
            }
            return result;
        }
        #endregion

        #region Login
        //登录
        public string Login(string xmlString)
        {
            string result = "", sql = "", loginName = "",loginpwd;
            Boolean checkLogin=true;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("Login/LoginName");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    checkLogin = false;
                else
                    loginName = vNode.InnerText.Trim();

                if (checkLogin)
                {
                    vNode = doc.SelectSingleNode("Login/Mobile");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0)
                        checkLogin = false;
                    else
                        loginName = vNode.InnerText.Trim();
                }
               vNode = doc.SelectSingleNode("Login/Password");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("密码不能为空");
                else
                    loginpwd=vNode.InnerText.Trim();

                sql = "Select * From  t_Employees  Where (FMobile='" + loginName + "' OR  FLoginName ='" + loginName + "') And FLoginPwd='" + loginpwd + "' And FIsDeleted=0";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count < 1)
                    throw new Exception("登录失败");
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><UpdateRegistration>" +
                           "<Result>True</Result>" +
                           "<CompanyID>" + dt.Rows[0]["FCompanyID"].ToString() + "</CompanyID>" +
                           "<EmployeeID>" + dt.Rows[0]["FID"].ToString() + "</EmployeeID>" +
                           "<RoleID>" + dt.Rows[0]["FRoleID"].ToString() + "</RoleID>" +
                           "<PageID>" + dt.Rows[0]["FPageID"].ToString() + "</PageID>" +
                           "<Description>登录成功</Description>" +
                           "</UpdateRegistration>";
            }
            catch (Exception err)
            {
                result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><UpdateRegistration>" +
                          "<Result>False</Result>" +
                          "<Description>" + err.Message + "</Description>" +
                          "</UpdateRegistration>";
                throw err;
            }
            return result;
        }
        #endregion


        #region GetEmployeeListByDepartment
        public string GetEmployeeListByDepartment(string departmentID)
        {
            string result = "", sql = "";
            

            try
            {
                SQLServerHelper runner;
               
                if (departmentID.Trim().Length == 0)
                    throw new Exception("部门ID不能为空");

                sql = "Select t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t4.FName As FDepartmentName,t3.FName As FPostitionName,t2.FClassID," +
                    " t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,t5.FName As FTypeName" +
                    " From t_Employees t1" +
                    " Left Join t_Items t2 On t1.FID=t2.FID" +
                    " Left Join t_Items t3 On t3.FID= t1.FPositionID" +
                    " Left Join t_Items t4 On t4.FID= t1.FDeptID" +
                    " Left Join t_Items t5 On t5.FID= t1.FTypeID"+
                    " Where t1.FIsDeleted=0 and t1.FDeptID ='" + departmentID + "'";
                sql = sql + " order by t1.FSortIndex Asc";

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetEmployeeList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetEmployeeList>";
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetEmployeeList/DataRows");
                    foreach (DataRow row in dt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);

                        XmlNode vNode = doc.CreateElement("ID");
                        vNode.InnerText = row["FID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FName");
                        vNode.InnerText = row["FName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FClassID");
                        vNode.InnerText = row["FClassID"].ToString();
                        cNode.AppendChild(vNode);



                        vNode = doc.CreateElement("FDeptID");
                        vNode.InnerText = row["FDeptID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FNumber");
                        vNode.InnerText = row["FNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FFullNumber");
                        vNode.InnerText = row["FFullNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIsDetail");
                        vNode.InnerText = row["FIsDetail"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLevel");
                        vNode.InnerText = row["FLevel"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDepartmentName");
                        vNode.InnerText = row["FDepartmentName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPostitionName");
                        vNode.InnerText = row["FPostitionName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPositionID");
                        vNode.InnerText = row["FPositionID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FMobile");
                        vNode.InnerText = row["FMobile"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLoginName");
                        vNode.InnerText = row["FLoginName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLoginPwd");
                        vNode.InnerText = row["FLoginPwd"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FMail");
                        vNode.InnerText = row["FMail"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRoleID");
                        vNode.InnerText = row["FRoleID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPageID");
                        vNode.InnerText = row["FPageID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FWechat");
                        vNode.InnerText = row["FWechat"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRemark");
                        vNode.InnerText = row["FRemark"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTypeID");
                        vNode.InnerText = row["FTypeID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTypeName");
                        vNode.InnerText = row["FTypeName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIsAgency");
                        vNode.InnerText = row["FIsAgency"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetEmployeeList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetEmployeeList>";
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
