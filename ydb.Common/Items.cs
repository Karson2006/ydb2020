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
    public class Items
    {
        public Items()
        { 
        }

        public DataTable GetListByClass(string classID,string level="1")
        {
            DataTable result = new DataTable();
            string sql = "Select t1.* ,ISNULL(t2.FName,'') As FParentName,ISNULL(t3.FName,'') As FClassName" +
                        " From t_items t1" +
                        " Left Join t_items t2 On t1.FParentID=t2.FID" +
                        " Left Join t_itemclass t3 On t1.FClassID = t3.FID" +
                        " Where t1.FLevel=" + level.ToString() + " and t1.FIsDeleted =0 And t1.FClassID='" + classID + "'" +
                        " Order by t1.FNumber Asc";
            SQLServerHelper  runner = new SQLServerHelper();
            result = runner.ExecuteSql(sql);
            return result;
        }

        public string GetListByClassXml(string classID, string level = "1")
        {
            string result = "";
            try
            {
                
                DataTable dt = GetListByClass(classID, level);
                if(dt.Rows.Count>0)
                    result = TableToXml(dt);
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetItemList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetItemList>";
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        public DataTable GetListByParent(string parentID)
        {
            DataTable result = new DataTable();
            string sql = "Select t1.* ,ISNULL(t2.FName,'') As FParentName,ISNULL(t3.FName,'') As FClassName"+
                        " From t_items t1"+
                        " Left Join t_items t2 On t1.FParentID=t2.FID"+
                        " Left Join t_itemclass t3 On t1.FClassID = t3.FID"+
                       " Where t1.FParentID='" + parentID + "' and t1.FIsDeleted =0 "+
                       " Order by t1.FNumber Asc";
            SQLServerHelper runner = new SQLServerHelper();
            result = runner.ExecuteSql(sql);
            return result;
        }
        public string GetListByParentXml(string parentID)
        {
            string result = "";
            try
            {
                DataTable dt = GetListByParent(parentID);
                if (dt.Rows.Count > 0)
                    result = TableToXml(dt);
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetItemList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetItemList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
        public DataTable GetAllByParent(string parentID,bool isDetail=false)
        {
            DataTable result = new DataTable();
            try
            {
                string pFNumber = "";
                 
                string sql = "select FNumber,FClassID from t_items Where FID='" + parentID + "' and FIsDeleted =0 ";
                SQLServerHelper runner = new SQLServerHelper();
                result = runner.ExecuteSql(sql);
                if (result.Rows.Count>0)
                {
                    pFNumber = result.Rows[0]["FNumber"].ToString();
                    sql = "Select t1.* ,ISNULL(t2.FName,'') As FParentName,ISNULL(t3.FName,'') As FClassName" +
                        " From t_items t1" +
                        " Left Join t_items t2 On t1.FParentID=t2.FID" +
                        " Left Join t_itemclass t3 On t1.FClassID = t3.FID" +
                        " Where t1.FFullNumber like '" + pFNumber + "%' and t1.FIsDeleted =0 And t1.FClassID='" + result.Rows[0]["FClassID"].ToString()+"'";
                    if (isDetail)
                        sql = sql + " And t1.FIsDetail=1";
                    sql = sql + " Order by t1.FNumber Asc";
                    result = runner.ExecuteSql(sql);
                }
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }

        public string GetAllDetailByParentXml(string parentID)
        {
            string result = "";
            try
            {
                DataTable dt = GetAllByParent(parentID,true);
                if (dt.Rows.Count > 0)
                    result = TableToXml(dt);
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetItemList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetItemList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }
       
        public string GetAllByParentXml(string parentID)
        {
            string result = "";
            try
            {
                DataTable dt = GetAllByParent(parentID);
                if (dt.Rows.Count > 0)
                    result = TableToXml(dt);
                else
                    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetItemList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetItemList>";
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        private string TableToXml(DataTable dt)
        {
            XmlDocument doc = new XmlDocument();
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetItemList>" +
                            "<Result>True</Result>" +
                            "<Description></Description>" +
                            "<DataRows></DataRows>" +
                            "</GetItemList>"; 
            doc.LoadXml(result);
            XmlNode pNode = doc.SelectSingleNode("GetItemList/DataRows");
            foreach(DataRow row in dt.Rows)
            {
                XmlNode cNode = doc.CreateElement("DataRow");
                pNode.AppendChild(cNode);

                XmlNode vNode = doc.CreateElement("FItemID");
                vNode.InnerText = row["FID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FName");
                vNode.InnerText = row["FName"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FClassID");
                vNode.InnerText = row["FClassID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FClassName");
                vNode.InnerText = row["FClassName"].ToString();
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

                vNode = doc.CreateElement("FDescription");
                vNode.InnerText = row["FDescription"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FLevel");
                vNode.InnerText = row["FLevel"].ToString();
                cNode.AppendChild(vNode);

            }
            result = doc.OuterXml;
            return result;
        }

        #region Delete
        public string  Delete(string xmlString)
        {
            string result = "-1", itemID="-1",parentID="-1";
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("DeleteItem/ID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("ID不能为空");
                itemID = vNode.InnerText.Trim();
                string sql = "Select FName, FID from t_items Where FIsDeleted=0 And FParentID='" + itemID + "'";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                    throw new Exception("该节点还下级节点，不能删除");

                sql = "Update t_items Set FIsDeleted =1 Where FIsDeleted=0 And FID='" + itemID + "'";
                if (runner.ExecuteSqlNone(sql) < 1)
                {
                    itemID = "-1";
                    throw new Exception("操作成功");
                }
                sql = "Select FParentID From t_Items Where  FID='" + itemID + "'";
                dt = runner.ExecuteSql(sql);
                if (dt.Rows.Count > 0)
                {
                    parentID = dt.Rows[0]["FParentID"].ToString();
                    sql = "Select FParentID From t_Items Where FIsDetail=0 And FParentID='" + parentID + "'";
                    dt = runner.ExecuteSql(sql);
                    if (dt.Rows.Count == 0)
                    {
                        sql = "Update t_items Set FIsdetail =1 Where FID='" + parentID + "'";
                        runner.ExecuteSqlNone(sql);
                    }
                }
            }
            catch(Exception err)
            {
                throw err;
            }
            result = itemID;
            return result;
        }
        #endregion

        #region Update
        public string Update(string dataString)
        {
            string id = "", sql = "", fullnumber = "", valueString = "", parentID = "-1",level="1",action="";
            string result = "-1";
            int editMode = 0;
            DataTable itemtb = new DataTable();
            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                if (!CheckUpateData(dataString))
                    result = "-1";

                XmlDocument doc = new XmlDocument();
                XmlNode vNode ;
                doc.LoadXml(dataString);

                vNode = doc.SelectSingleNode("UpdateItem/Action");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    action = vNode.InnerText.Trim();
                }

                id = doc.SelectSingleNode("UpdateItem/ID").InnerText;
                if (action.Length == 0)//没有传此参数
                {
                    if (id.Trim() == "" || id.Trim() == "-1")//新增
                        editMode = 1;
                }
                else
                    editMode = int.Parse(action);
                
                if (editMode == 1)//新增
                {
                    vNode= doc.SelectSingleNode("UpdateItem/FParentID");
                    if (vNode == null || vNode.InnerText.Trim().Length == 0 || vNode.InnerText.Trim()=="-1")//没有设置父节点，默认为一级节点
                    {
                        parentID = "-1";
                        level ="1";
                        fullnumber = doc.SelectSingleNode("UpdateItem/FNumber").InnerText;
                    }
                    else
                    {
                        parentID = vNode.InnerText;
                        sql = "Select FID,FName,FFullNumber,FLevel From t_items Where FID='" + parentID + "'";
                        itemtb = runner.ExecuteSql(sql);
                        if (itemtb.Rows.Count > 0)//存在该父级节点
                        {
                            fullnumber = itemtb.Rows[0]["FFullNumber"].ToString()+"."+ doc.SelectSingleNode("UpdateItem/FNumber").InnerText;
                            level =(int.Parse( itemtb.Rows[0]["FLevel"].ToString())+1).ToString();
                        }
                        else//不存在该父节点
                        {
                            parentID = "-1";
                            level ="1";
                            fullnumber = doc.SelectSingleNode("UpdateItem/FNumber").InnerText;
                        }
                    }
                    if(id==""||id =="-1")//没有插入ID值，新增
                        id = Guid.NewGuid().ToString();

                    sql = "Insert into [t_Items](FID,FLevel,FParentID,FFullNumber,FNumber) Values('" + id + "','" + level + "','" + parentID + "','" + fullnumber + "','" + doc.SelectSingleNode("UpdateItem/FNumber").InnerText + "')";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入基础资料失败
                        throw new Exception("新建资料失败");
                    sql = "Update t_Items Set FIsDetail=0 Where FIsDetail=1 And  FID='" + parentID + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入新日程失败
                        throw new Exception("新建资料失败");

                }
                
                //更新消息信息
                vNode = doc.SelectSingleNode("UpdateItem/FName");
                string val = "";
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FName='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FDescription");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDescription='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateItem/FClassID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FClassID='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update [t_Items] Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {
                        id = "-1";
                        throw new Exception("更新资料失败");
                    }

                }
            }
            catch (Exception err)
            {
                throw err;
            }
            result = id;

            return result;
        }

        private Boolean CheckUpateData(string xmlString)
        {
            Boolean result = false;
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNode vNode = doc.SelectSingleNode("UpdateItem/FName");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
                throw new Exception("名称不能为空");
           
            //vNode = doc.SelectSingleNode("UpdateItem/ItemID");
            //if (vNode == null || vNode.InnerText == "-1" || vNode.InnerText.Trim().Length == 0)//新增检查
            //{
                

                vNode = doc.SelectSingleNode("UpdateItem/FClassID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("资料类型ID不能为空");


                vNode = doc.SelectSingleNode("UpdateItem/FNumber");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("代码不能为空");
                else
                {
                    XmlNode idNode = doc.SelectSingleNode("UpdateItem/ID");
                    if(idNode==null)
                    {
                        throw new Exception("没有<ID>节点");
                    }
                    else
                    {
                        if (idNode.InnerText.Trim().Length == 0 || idNode.InnerText.Trim().Equals("-1"))//新增
                        {
                            string sql = "Select FID,FNumber from t_items Where FIsDeleted=0 And FClassID='" + doc.SelectSingleNode("UpdateItem/FClassID").InnerText + "' And FNumber ='" + doc.SelectSingleNode("UpdateItem/FNumber").InnerText + "'";
                            SQLServerHelper runner = new SQLServerHelper();
                            DataTable dt = runner.ExecuteSql(sql);
                            if (dt.Rows.Count > 0)
                                throw new Exception("代码：" + dt.Rows[0]["FNumber"].ToString() + "已存在");
                        }
                    }
                }
            //}

            result = true;
            return result;
        }
        #endregion  


        public string GetItemDetailXml(string ID)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetItemDetail>" +
                        "<Result>False</Result>" +
                        "<Description></Description>" +
                        "<DataRow></DataRow>" +
                        "</GetItemDetail>";

            DataTable dt = new DataTable();
            string sql = "Select t1.* ,ISNULL(t2.FName,'') As FParentName,ISNULL(t3.FName,'') As FClassName" +
                        " From t_items t1" +
                        " Left Join t_items t2 On t1.FParentID=t2.FID" +
                        " Left Join t_itemclass t3 On t1.FClassID = t3.FID" +
                        " Where t1.FID='" + ID +"'";
            SQLServerHelper runner = new SQLServerHelper();
            dt = runner.ExecuteSql(sql);
            if (dt.Rows.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                doc.SelectSingleNode("GetItemDetail/Result").InnerText = "True";
                XmlNode cNode = doc.SelectSingleNode("GetItemDetail/DataRow");

                XmlNode vNode = doc.CreateElement("ID");
                vNode.InnerText = dt.Rows[0]["FID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FName");
                vNode.InnerText = dt.Rows[0]["FName"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FClassID");
                vNode.InnerText = dt.Rows[0]["FClassID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FClassName");
                vNode.InnerText = dt.Rows[0]["FClassName"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FParentID");
                vNode.InnerText = dt.Rows[0]["FParentID"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FParentName");
                vNode.InnerText = dt.Rows[0]["FParentName"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FNumber");
                vNode.InnerText = dt.Rows[0]["FNumber"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FFullNumber");
                vNode.InnerText = dt.Rows[0]["FFullNumber"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FIsDetail");
                vNode.InnerText = dt.Rows[0]["FIsDetail"].ToString();
                cNode.AppendChild(vNode);

                vNode = doc.CreateElement("FLevel");
                vNode.InnerText = dt.Rows[0]["FLevel"].ToString();
                cNode.AppendChild(vNode);
                result = doc.OuterXml;
            }
            return result;
            
        }
    }
}
