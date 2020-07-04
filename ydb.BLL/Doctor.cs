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
    public class Doctor
    {
        Items iClass;
        public Doctor()
        {
            iClass = new Items();
        }

        #region GetDetail
        public string GetDetail(string xmlString)
        {
            string result = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetDoctorDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetDoctorDetail>", "GetDoctorList>");
                    result = GetList(xmlString).Replace("GetDoctorList>", "GetDoctorDetail>");
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region GetList
        public string GetList(string xmlString)
        {
            string result = "", sql = "", filter = " t1.FIsDeleted=0 ", val = "";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetDoctorList/ID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetDoctorList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetDoctorList/Number");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t2.FNumber like '%" + val + "%'" : "t2.FNumber like '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetDoctorList/HospitalName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t3.FName like  '%" + val + "%'" : " t3.FName like  '%" + val + "%'";
                }

                vNode = doc.SelectSingleNode("GetDoctorList/RankID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t1.FRankID='" + val + "'" : " t1.FRankID='" + val + "'" ;
                }

                vNode = doc.SelectSingleNode("GetDoctorList/DepartmentID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And  t1.FDeptID='" + val + "'" : " t1.FDeptID='" + val + "'";
                }

                sql = "SELECT t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t2.FClassID,t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,"+
                    " Isnull(t3.FName,'') As FHosptal,Isnull(t4.FName,'') As FDeptment,Isnull(t5.FName,'') As FTitleName,Isnull(t6.FName,'') As FRankName" +
                    " From t_Doctors t1"+
                    " Left Join t_Items t2 On t1.FID = t2.FID"+
                    " Left Join t_Items t3 On t3.FID = t1.FHospitalID" +
                    " Left Join t_Items t4 On t4.FID = t1.FDeptID"+
                    " Left Join t_Items t5 On t5.FID = t1.FTitleID"+
                    " Left Join t_Items t6 On t6.FID = t1.FRankID";

               if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " order by t1.FSortIndex Asc";

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetHospitalDeptList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetHospitalDeptList>";
                    doc.LoadXml(result);
                    XmlNode pNode = doc.SelectSingleNode("GetHospitalDeptList/DataRows");
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
                        
                        vNode = doc.CreateElement("FDescription");
                        vNode.InnerText = row["FDescription"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FHosptalName");
                        vNode.InnerText = row["FHosptal"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FHospitalID");
                        vNode.InnerText = row["FHospitalID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDeptID");
                        vNode.InnerText = row["FDeptID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDeptmentName");
                        vNode.InnerText = row["FDeptment"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTitleID");
                        vNode.InnerText = row["FTitleID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FTitleName");
                        vNode.InnerText = row["FTitleName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRankID");
                        vNode.InnerText = row["FRankID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FRankName");
                        vNode.InnerText = row["FRankName"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSpeciality");
                        vNode.InnerText = row["FSpeciality"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FPhotoFile");
                        vNode.InnerText = row["FPhotoFile"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FLicenseNumber");
                        vNode.InnerText = row["FLicenseNumber"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FApproved");
                        vNode.InnerText = row["FApproved"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FApproverID");
                        vNode.InnerText = row["FApproverID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FDate");
                        vNode.InnerText = row["FDate"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FApproveDate");
                        vNode.InnerText = row["FApproveDate"].ToString();
                        cNode.AppendChild(vNode);
                    }
                    result = doc.OuterXml;
                }
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetHospitalList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetHospitalList>";
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
            string id = "", sql = "",  valueString = "";
            bool datachecked = true;
            string result = "-1";

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                dataString = dataString.Replace("UpdateDoctor>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FHospitalID");
                string val = "";
              
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("医院ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FHospitalID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FDeptID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("科室ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDeptID ='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FTitleID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    throw new Exception("职称ID不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTitleID ='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FCompanyID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FCompanyID='" + val + "',";
                }
               
                
                id = iClass.Update(dataString);
                if (id == "-1")//插入t_items表错误
                    result = "-1";
                datachecked = true;

                if (doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "" || doc.SelectSingleNode("UpdateItem/ID").InnerText.Trim() == "-1")//新增
                {
                    sql = "Insert into t_Doctors(FID) Values('" + id + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                        throw new Exception("新建失败");
                }


                vNode = doc.SelectSingleNode("UpdateItem/FSortIndex");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSortIndex='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FRankID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FRankID='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FSpeciality");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSpeciality='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FIntroduce");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIntroduce='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FPhotoFile");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FPhotoFile='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FLicenseNumber");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FLicenseNumber='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FApproved");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FApproved='" + val + "',";
                }
                

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update t_Doctors Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新失败");
                    }

                }

                
            }
            catch (Exception err)
            {
                if (id != "-1" && datachecked)//t_tems已插入数据成功，要删除
                {
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_Doctors Where FID='" + id + "'";
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
                xmlString = xmlString.Replace("DeleteDoctor>", "DeleteItem>");
                Items item = new Items();
                id = item.Delete(xmlString);

                if (id.Trim() != "-1")//t_Items删除成功
                {
                    sql = " Update t_Doctors Set FIsDeleted =1 Where  FID='" + id + "' And FIsDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                sql = "Update t_Doctors Set FIsDeleted =0 Where FID='" + id + "' And FIsDeleted=1    Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

        #region Approve
        public string Approve(string xmlString)
        {
            string result = "-1", sql = "", id = "-1", valueString="";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("ApproveDoctor/ID");
                if (vNode == null || vNode.InnerText.Length == 0)
                {
                    throw new Exception("医生ID不能为空");
                }
                else
                {
                    id = vNode.InnerText.Trim();
                }

                vNode = doc.SelectSingleNode("UpdateItem/FApproverID");
                if (vNode != null)
                {
                    string val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = ",FApproverID='" + val + "'";
                }

               if (id.Trim() != "-1")//t_Items删除成功
               {
                   sql = " Update t_Doctors Set FApproved =1,FApproveDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + valueString + " Where  FID='" + id + "'";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
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
    }

}
