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
    public class HospitalDepartment
    {
        Items iClass;
        public HospitalDepartment()
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
                XmlNode vNode = doc.SelectSingleNode("GetHospitalDeptDetail/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    throw new Exception("ID不能为空");
                }
                else
                {
                    xmlString = xmlString.Replace("GetHospitalDeptDetail>", "GetHospitalDeptList>");
                    result = GetList(xmlString).Replace("GetHospitalDeptList>", "GetHospitalDeptDetail>");
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
            string result = "", sql = "", filter = " t1.FIsDeleted=0 ", val = "", filter1 = " FIsDeleted=0 ";
            string hosptaId = "", hospitalName = "";
            SQLServerHelper runner;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetHospitalDeptList/ID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();

                    if (val.Length > 0)
                    {
                        filter = filter.Length > 0 ? filter = filter + " And t1.FID='" + val + "'" : "t1.FID='" + val + "'";
                        filter1 = filter1.Length > 0 ? filter1 = filter1 + " And FID='" + val + "'" : "FID='" + val + "'";
                    }
                }

                vNode = doc.SelectSingleNode("GetHospitalDeptList/Name");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        filter = filter.Length > 0 ? filter = filter + " And t2.FName like '%" + val + "%'" : "t2.FName like '%" + val + "%'";
                        filter1 = filter1.Length > 0 ? filter1 = filter1 + " And FName like '%" + val + "%'" : " FName like '%" + val + "%'";
                    }
                }

                vNode = doc.SelectSingleNode("GetHospitalDeptList/Number");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    if (val.Length > 0)
                    {
                        filter = filter.Length > 0 ? filter = filter + " And t2.FNumber like '%" + val + "%'" : "t2.FNumber like '%" + val + "%'";
                        filter1 = filter1.Length > 0 ? filter1 = filter1 + " And FNumber like '%" + val + "%'" : " FNumber like '%" + val + "%'";
                    }
                }
                vNode = doc.SelectSingleNode("GetHospitalDeptList/HospitalID");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    hosptaId = val;
                    if (val.Length > 0 && val.Trim() !="-1")
                        filter = filter.Length > 0 ? filter = filter + " And t2.FParentID= '" + val + "'" : "And t2.FParentID='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetHospitalDeptList/HospitalName");
                if (vNode != null)
                {
                    val = vNode.InnerText.Trim();
                    hospitalName = val;
                    if (val.Length > 0)
                        filter = filter.Length > 0 ? filter = filter + " And t3.FName like  '%" + val + "%'" : " t3.FName like  '%" + val + "%'";
                }
                if(hosptaId.Length ==0 && hospitalName.Length==0 )
                    throw new Exception("医院ID和医院名称不能同时为空");
               sql = "SELECT t1.*,t2.FName,t2.FNumber,t2.FFullNumber,t2.FClassID,t2.FLevel,t2.FDescription,t2.FLevel,t2.FIsDetail,"+
                    " Isnull(t3.FName,'') As FHosptal,Isnull(t3.FID,'') As FHosptalID" +
                    " From t_HospitalDepartments t1"+
                    " Left Join t_Items t2 On t1.FID = t2.FID"+
                    " Left Join t_Items t3 On t3.FID = t2.FParentID";

               if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                sql = sql + " order by t1.FSortIndex Asc";

                runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                //有医院单设的科室，读取相应科室，若没有，读取通用科室
                if(dt.Rows.Count == 0)
                {
                    string sql1 = "";
                    if (hosptaId.Length > 0)
                    {
                        sql1 = @"SELECT FID,FNAME FROM T_ITEMS WHERE  FID='{0}' AND  FIsDeleted=0";
                        sql1 = string.Format(sql1, hosptaId);
                    }

                    if (hospitalName.Length > 0)
                    {
                        sql1 = @"SELECT FID,FNAME FROM T_ITEMS WHERE  FNAME LIKE '%{0}%' AND  FIsDeleted=0";
                        sql1 = string.Format(sql1, hospitalName);
                    }
                    dt = runner.ExecuteSql(sql1);
                    if(dt.Rows.Count>0 )
                    {
                        hosptaId = dt.Rows[0]["FID"].ToString();
                        hospitalName = dt.Rows[0]["FNAME"].ToString();
                        sql = @"Select *,'{0}' As FHosptal,'{1}' As FHosptalID, '' As FSupervisor,'' As  FIntroduce,'0' As FSortIndex from t_items 
                            where FclassID = 'e856cfda-441c-4539-8b64-4dc90a408ce9' and FParentID='-1'";
                        sql = string.Format(sql, hospitalName, hosptaId);

                        dt = runner.ExecuteSql(sql);
                    }
                  }
                        
                   
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

                        vNode = doc.CreateElement("FHosptalID");
                        vNode.InnerText = row["FHosptalID"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSupervisor");
                        vNode.InnerText = row["FSupervisor"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FIntroduce");
                        vNode.InnerText = row["FIntroduce"].ToString();
                        cNode.AppendChild(vNode);

                        vNode = doc.CreateElement("FSortIndex");
                        vNode.InnerText = row["FSortIndex"].ToString();
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
                dataString = dataString.Replace("UpdateHospitalDept>", "UpdateItem>");

                XmlDocument doc = new XmlDocument();
                XmlNode vNode;
                doc.LoadXml(dataString);
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FSupervisor");
                string val = "";
              
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSupervisor='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FIntroduce");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    //throw new Exception("经度不能为空");
                }
                else
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIntroduce='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateItem/FCompanyID");
                if (vNode == null || vNode.InnerXml.Trim().Length == 0)
                {
                    datachecked = false;
                    //throw new Exception("经度不能为空");
                }
                else
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
                    sql = "Insert into t_HospitalDepartments(FID) Values('" + id + "')";
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

                vNode = doc.SelectSingleNode("UpdateItem/FTelNumber");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTelNumber='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FPictureFile");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FPictureFile='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update t_HospitalDepartments Set " + valueString + " Where FID='" + id + "'";
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
                    sql = "Delete from t_Items Where FID='" + id + "'  Delete from t_HospitalDepartments Where FID='" + id + "'";
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
                xmlString = xmlString.Replace("DeleteHospitalDept>", "DeleteItem>");
                Items item = new Items();
                id = item.Delete(xmlString);

                if (id.Trim() != "-1")//t_Items删除成功
                {
                    sql = " Update t_HospitalDepartments Set FIsDeleted =1 Where  FID='" + id + "' And FIsDeleted=0";
                    SQLServerHelper runner = new SQLServerHelper();
                    runner.ExecuteSqlNone(sql);
                }
            }
            catch (Exception err)
            {
                sql = "Update t_HospitalDepartments Set FIsDeleted =0 Where FID='" + id + "' And FIsDeleted=1    Update t_Items Set FIsDeleted =0 Where  FID='" + id + "' And FIsDeleted=1";
                SQLServerHelper runner = new SQLServerHelper();
                runner.ExecuteSqlNone(sql);
                throw err;
            }
            result = id;
            return result;
        }
        #endregion

    }

}
