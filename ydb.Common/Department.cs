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
    public class Department
    {
        Items iClass;
        public Department()
        {
            iClass = new Items();
        }

        #region GetDepartmentDetail
        public string GetDepartmentDetail(string xmlString)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetDepartmentDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length==0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetDepartmentDetail>", "GetDepartmentList>");
                    result = GetDepartmentList(xmlString).Replace("GetDepartmentList>","GetDepartmentDetail>");
                }
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetDepartmentList
        public string GetDepartmentList(string xmlString)
        {
            string result = "",sql="",filter="t1.FIsDeleted=0 ",val="";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetDepartmentList/ID");
                if(vNode!=null)
                {
                    val = vNode.InnerText.Trim();
                    if(val.Length>0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetDepartmentList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length >0 ? filter = filter + " And t2.FName like '%" + val + "%'":"t2.FName like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetDepartmentList/Number");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FNumber like '%" + val + "%'" : "t2.FNumber like '%" + val + "%'";
                }
                vNode = doc.SelectSingleNode("GetDepartmentList/LeaderName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t3.FName like '%" + val + "%'" : " t3.FName like '%" + val + "%'";
                }

                sql = "Select t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t2.FParentID,t4.FName As FParentName,t3.FName As FSupervisorName,t2.FClassID,"+
                    " t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail"+
                    " From t_departments t1"+
                    " Left Join t_Items t2 On t1.FID=t2.FID"+
                    " Left Join t_Items t3 On t3.FID= t1.FSupervisorID"+
                    " Left Join t_Items t4 On t4.FID= t2.FParentID";
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " order by t1.FSortIndex Asc"; 

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetDepartmentList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetDepartmentList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetDepartmentList/DataRows");
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

                        vNode = doc.CreateElement("FParentID");
                        vNode.InnerText = row["FParentID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FParentName");
                        vNode.InnerText = row["FParentName"].ToString();
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

                        vNode = doc.CreateElement("FSupervisorName");
                        vNode.InnerText = row["FSupervisorName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSupervisorID");
                        vNode.InnerText = row["FSupervisorID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPostions");
                        vNode.InnerText = row["FPostions"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIsPartTime");
                        vNode.InnerText = row["FIsPartTime"].ToString();
                        cNode.AppendChild(vNode);

                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetDepartmentList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetDepartmentList>";
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
            string id = "", sql = "",  valueString = "",leaderId="-1";
            string result = "-1", action = "1";
            int editMode = 1;

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                dataString = dataString.Replace("UpdateDepartment>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FSupervisorID");
                string val = "";
                if (vNode != null && vNode.InnerText.Trim().Length > 0 && vNode.InnerText.Trim() != "-1")
                {
                    val = vNode.InnerText.Trim();
                    
                        valueString = valueString + "FSupervisorID='" + val + "',";
                        leaderId = val;
                }
                else
                    throw new Exception("部门主管ID不能为空");

                id = iClass.Update(dataString);
                if (id == "-1")//插入t_items表错误
                {
                    result = "-1";
                    throw new Exception("更新t_items表失败");
                }

                vNode = doc.SelectSingleNode("UpdateItem/Action");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    action = vNode.InnerText.Trim();
                    editMode = int.Parse(action);
                }
                else
                {
                    if (doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "" || doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "-1")
                        editMode = 1;
                    else
                        editMode = 2;
                }

                if (editMode==1)//新增
                {
                    sql = "Insert into t_Departments(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入部门失败
                        throw new Exception("新建失败");
                }
                vNode = doc.SelectSingleNode("UpdateItem/FIntroduce");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIntroduce='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FIsPartTime");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIsPartTime='" + val + "',";
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
                    sql = "Update t_Departments Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新失败");
                    }

                }
                if(leaderId!="-1")//主管更新,维护t_Workships表
                {
                    WorkShip ws = new WorkShip();
                    ws.Update(leaderId, id);
                }
            }
            catch (Exception err)
            {
                if(id!="-1")//t_tems已插入数据成功，要删除
                {
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_Departments Where FID='" + id + "'";
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
                xmlString = xmlString.Replace("DeleteDepartment>", "DeleteItem>");
                Items item = new Items();

                id=item.Delete(xmlString);

                if (id.Trim()!="-1")//t_Items删除成功
                {
                    sql = " Update t_Departments Set FIsDeleted =1 Where  FID='" + id + "' And FIsDeleted=0   Update t_Workships Set FIsDeleted =1 Where  FTeamID='" + id + "' And FIsDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    if (runner.ExecuteSqlNone(sql) < 1)
                        id = "-1";
                }
                else
                {
                    id = "-1";
                }
            }
            catch (Exception err)
            {
                sql = "Update t_Departments Set FIsDeleted =0 Where FID='" + id + "' And FIsDeleted=1   Update t_Workships Set FIsDeleted =0 Where  FTeamID='" + id + "' And FIsDeleted=1  Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region GetAllSubItemList
        //返回该部门的下一级子部门和相应的人员
        public string GetAllSubItemList(string xmlString)
        {
            string result = "", sql = "",id="";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetAllSubItemList/DeptID");
                if (vNode == null ||vNode.InnerText.Trim().Length ==0 )
                    throw  new  Exception("部门ID不能为空");
                else
                {
                    id = vNode.InnerText.Trim();
                }

                sql = "Select FID,FName,FNumber,FFullNumber,FParentID,FLevel,FClassID,FIsDetail" +
                         " From t_Items" +
                         " Where FIsDeleted=0 And FParentID='" + id + "'" +
                         " Union" +
                         " Select t1. FID,t1.FName,t1.FNumber,t1.FFullNumber,t1.FParentID,t1.FLevel,t1.FClassID,t1.FIsDetail" +
                         " From t_Items t1" +
                         " Left Join t_employees t2 On t1.FID = t2.FID" +
                         " Where t1.FIsDeleted=0 And t2.FDeptID='" + id + "'";

               
     

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetAllSubItemList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetAllSubItemList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetAllSubItemList/DataRows");
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

                        vNode = doc.CreateElement("FParentID");
                        vNode.InnerText = row["FParentID"].ToString();
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
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetDepartmentList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetDepartmentList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region CreateCompany
        //根据注册公司信息创建组织架构的根节点（公司）
        public string CreateCompany(string xmlString)
        {
            string result = "",  id = "",companyName="";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("CreateCompany/CompanyName");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("公司名称不能为空");
                else
                {
                    companyName = vNode.InnerText.Trim();
                }
            
                id = Guid.NewGuid().ToString();
                string sql = "Insert Into t_Items(FID,FCompanyID,FName,FNumber,FLevel,FFullNumber,FIsDetail)";
                sql = sql+ "Values('"+id+"','"+id+"','"+companyName+"','"+"A"+ DateTime.Now.ToString("yyyyMMddhhmmss")+"','1','"+"A"+ DateTime.Now.ToString("yyyyMMddhhmmss")+"',1)";
                SQLServerHelper runner = new SQLServerHelper();
                if (runner.ExecuteSqlNone(sql)>0)
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CreateCompany>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<ID>"+id+"</ID>" +
                            "</CreateCompany>";
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><CreateCompany>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<ID>"+id+"</ID>" +
                            "</CreateCompany>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

    }


    public class WorkShip
    {
        public WorkShip()
        { }

        private string memberIDs = "";
        private string deptIDs = "";
        #region Update

        public string Update(string teamID)
        {
            string result = "-1", sql = "";
            SQLServerHelper runner;
            try
            {
                runner = new SQLServerHelper();
                sql = "Select FSupervisorID From t_Departments Where FID='" + teamID + "' and FIsDeleted=0";
                DataTable dt = runner.ExecuteSql(sql);
                if(dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["FSupervisorID"].ToString().Length > 0)
                    {
                        Update(dt.Rows[0]["FSupervisorID"].ToString(), teamID);
                    }
                }

            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        public string Update(string supervisorID,string teamID)
        {
            string result = "-1",sql="";
            SQLServerHelper runner;
            try
            {
                runner = new SQLServerHelper();
                sql = "Select FID As FEmployeeID,FTypeID,FIsAgency From t_Employees Where FDeptID='"+ teamID+"' And FIsDeleted=0";//找出所有该团队成员
                DataTable dt = runner.ExecuteSql(sql);
                foreach(DataRow row in dt.Rows)
                {
                    sql = "Select * from t_Workships Where FEmployeeID='" + row["FEmployeeID"].ToString() + "' and FIsDeleted=0";
                    DataTable dtWorkship = runner.ExecuteSql(sql);
                    if(dtWorkship.Rows.Count==0)//此员工尚未建立工作关系
                    {
                        sql = "INSERT INTO t_Workships(FTeamID,FEmployeeID,FTeamLeaderID,FBeginDate,FIsDeleted,FIsAgency)VALUES('" + teamID + "','" + row["FEmployeeID"].ToString()+
                               "','" + supervisorID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',0,'" + row["FIsAgency"].ToString() + "')";
                        runner.ExecuteSqlNone(sql);
                    }
                    else//workship表有此记录
                    {
                        foreach(DataRow wsRow in dtWorkship.Rows)
                        {
                            if(!wsRow["FTeamLeaderID"].ToString().Trim().Equals(supervisorID))//Workship表中的主管ID不同于当前LeaderID
                            {
                                sql = "Select FDeptID From t_Employees Where FID= '" + row["FEmployeeID"].ToString() + "' and FIsDeleted=0 And FDeptID !='" + teamID + "'";
                                DataTable dt2 = runner.ExecuteSql(sql);
                                if(dt2.Rows.Count==0)//同部门，Leader变化
                                {
                                    sql = "Udpdate t_Workships Set FIsDeleted=1,FEndDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                                    sql = sql + " Where FID=" + wsRow["FID"].ToString();
                                    runner.ExecuteSqlNone(sql);
                                }
                                else//该员工在多个部门任职
                                {
                                    sql = "INSERT INTO t_Workships(FTeamID,FEmployeeID,FTeamLeaderID,FBeginDate,FIsDeleted,FIsAgency)VALUES('" + teamID + "','" + row["FEmployeeID"].ToString() +
                                          "','" + supervisorID + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',0," + row["FIsAgency"].ToString() + ")";
                                    runner.ExecuteSqlNone(sql);//插入此主管的汇报关系记录
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetTeamMemberIDs
        public string GetTeamMemberIDs(string xmlString)
        {
            string result = "",leaderID="";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetTeamMembers/LeaderID");
                if(vNode==null||vNode.InnerText.Trim().Length ==0)
                    throw new Exception("团队领导ID不能为空");
                else
                    leaderID = vNode.InnerText.Trim();

                //string sql = "Select t1.*,Isnull(t2.FName,'') As FDepartmentName" +
                //            " FROM t_Workships t1" +
                //            " Left Join t_Items t2 On t1.FTeamID= t2.FID ";
                //sql = sql + " Where t1.FTeamLeaderID='" + leaderID + "' Order by t1.FTeamID";

                string sql = @"Select t1.FID As FEmployeeID
                                From t_Employees t1 
                                Where t1.FDeptID In(Select Distinct FID from t_Departments where FSupervisorID='{0}')";
                sql = string.Format(sql, leaderID);
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                foreach(DataRow row in dt.Rows )
                {
                    result = result + row["FEmployeeID"].ToString() + "|";
                }
                result = result.Length > 0 ? result.Substring(0, result.Length - 1) : "";
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetTeamMemberList
        public string GetTeamMemberList(string xmlString)
        {

            string result = "", sql = "", leaderID = "";
           

            SQLServerHelper runner;
            result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetTeamMemberList>" +
                               "<Result>False</Result>" +
                               "<Description></Description><DataRows></DataRows>" +
                               "</GetTeamMemberList>";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetTeamMemberList/LeaderID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("团队负责人ID不能为空");
                else
                {
                    leaderID = vNode.InnerText.Trim();
                }
                
                sql = @"Select FID From t_Departments Where FSupervisorID ='{0}' and  FIsDeleted = 0";
                sql = string.Format(sql, leaderID);
                
                runner = new SQLServerHelper();
                DataTable deptDt = runner.ExecuteSql(sql);
                doc.LoadXml(result);
                XmlNode pNode = doc.SelectSingleNode("GetTeamMemberList/DataRows");
                doc.SelectSingleNode("GetTeamMemberList/Result").InnerText = "True";
                foreach( DataRow  dr in deptDt.Rows)
                {
                    sql = @"Select t1.FID As ID ,t1.FName As Name,'{0}' As PID,'1' As Detail,t2.FID AS LeaderID
                            From t_Items t1
                            Left Join t_Employees t2 On t1.FID = t2.FID  
                            Where t2.FDeptID In ('{0}') or t2.FLeaderList like '%{1}%'
                            Union
                            Select t1.FID As ID ,t1.FName As Name,'{0}' As PID,'0' As Detail,t2.FSupervisorID AS LeaderID
                            From t_Items t1
                            Left Join t_Departments t2 On t1.FID = t2.FID  
                            Where t1.FParentID In ('{0}')";

                    sql = string.Format(sql, dr["FID"].ToString(),leaderID);

                    DataTable memberDt = runner.ExecuteSql(sql);
                    foreach (DataRow memberRow in memberDt.Rows)
                    {
                        XmlNode cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);
                        vNode = null;
                        string colName = "";
                        foreach (DataColumn col in memberDt.Columns)
                        {
                            colName = col.Caption;
                            vNode = doc.CreateElement(colName);
                            vNode.InnerText = memberRow[colName].ToString();
                            cNode.AppendChild(vNode);
                        }
                    }
                }
                

               result = doc.OuterXml;
                
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        public  string  GetAllMemberIDsByLeaderID(string leaderID)
        {
            string deptID = "",sql = "";
            deptIDs = "";
            memberIDs = leaderID;

            sql = "Select FID from t_Departments Where FIsDeleted =0 and FSupervisorID='" + leaderID + "'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
           
            foreach (DataRow row in dt.Rows)
            {
                deptID = row["FID"].ToString(); 
                GetAllSbuDeptsByDeptID(deptID);
            }
            if (deptIDs.Length >0)
            {
                deptIDs=deptIDs.Replace("|","','"); 
                sql = "Select FID from t_Employees Where FIsDeleted =0 and FDeptID in ('{0}')  or  FLeaderList like '%{1}%'";
                sql = string.Format(sql, deptIDs, leaderID);
            }
            else
            {
                sql = "Select FID from t_Employees Where FIsDeleted =0 and   FLeaderList like '%{0}%'";
                sql = string.Format(sql,  leaderID);
            }
          
            dt = runner.ExecuteSql(sql);

            foreach (DataRow row in dt.Rows)
            {
                if (memberIDs.Length == 0)
                    memberIDs = row["FID"].ToString();
                else
                    memberIDs = memberIDs + "|" + row["FID"].ToString();
            }

            return memberIDs ;
        }
        
        private   void GetAllSbuDeptsByDeptID(string deptID)
        {
            if (deptIDs.Length == 0)
                deptIDs = deptID;
            else
                deptIDs = deptIDs + "|" + deptID;
            string sql = "Select FID from t_Items Where FIsDeleted =0 and FParentID='" + deptID + "'";
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);
            if (dt.Rows.Count >0)
            {
                foreach (DataRow row in dt.Rows)
                    GetAllSbuDeptsByDeptID(row["FID"].ToString());
            }
        }
    }

}
