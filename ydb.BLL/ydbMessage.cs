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
    public class ydbMessage
    {
        public ydbMessage()
        {

        }

        public string Delete(string id)
        {
            string  sql = "";
            string result = "-1";

            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                sql = "Update [Messages] Set FDeleted=1 Where FID= '" + id + "' Update MsgReceivers Set FDeleted=1 Where FMsgID='" + id + "'";
                if(runner.ExecuteSqlNone(sql)>0)
                    result = id;
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        public DataTable GetList(string employeeIDs,DateTime beginDate,DateTime endDate,DateTime expirationDate,string type ="99")
        {
            DataTable result = new DataTable();
            string filterString = "",sql="";

            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                //result = runner.ExecuteSql(sql);


                filterString = " FSentDate between '" + beginDate.ToString("yyyy-MM-dd") + " 0:0:0.000' and '" + endDate.ToString("yyyy-MM-dd") + " 23:59:59.999'";
                filterString = filterString + "  and FExpirationDate <= '" + expirationDate.ToString("yyyy-MM-dd") + " 23:59:59.999'";
                if (type != "99")
                    filterString = filterString + " and  FType In (" + type + ")";
                sql = "Select * from [Messages] Where FIsPulic=1 and FDeleted=0 and " + filterString;
                if (employeeIDs.Trim().Length > 0)
                {

                    sql = sql + " union Select * from [Messages] Where FIsPulic=0 and FDeleted=0 and FID In( Select FMsgID from MsgReceivers Where FReceiverID In('" + employeeIDs.Replace(";", "';'") + "'))  and  " + filterString;
                }

                result = runner.ExecuteSql(sql);
            }
            catch(Exception err)
            {
                throw err;
            }
            
            return result;
        }

        //public string GetListXML(string employeeIDs, DateTime beginDate, DateTime endDate, DateTime expirationDate,string type = "99")
        public string GetListXML(string xmlString)
        {

            
            #region Build the XML Schema
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetMessageList>" +
                            "<Result>False</Result>" +
                            "<Description></Description>" +
                            "</GetMessageList>";
            #endregion
            try
            {
                string callType = "GetMessageList";
                string receiverId = "", type = "";
                DateTime bdate= DateTime.Now.AddDays(-7)  , edate= DateTime.Now  , expiratoindate= DateTime.Parse("2099-12-31") ;
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(callType + "/ReceiverID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    receiverId = doc.SelectSingleNode(callType + "/ReceiverID").InnerText;

                vNode = doc.SelectSingleNode(callType + "/BeginDate");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    bdate = DateTime.Parse(doc.SelectSingleNode(callType + "/BeginDate").InnerText);

                vNode = doc.SelectSingleNode(callType + "/EndDate");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    edate = DateTime.Parse(doc.SelectSingleNode(callType + "/EndDate").InnerText);

                vNode = doc.SelectSingleNode(callType + "/ExpirationDate");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                { }
                else
                    expiratoindate = DateTime.Parse(doc.SelectSingleNode(callType + "/ExpirationDate").InnerText);

                vNode = doc.SelectSingleNode(callType + "/Type");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                {
                    type = "99";
                }
                else
                    type = doc.SelectSingleNode(callType + "/Type").InnerText;

                DataTable dt = GetList(receiverId, bdate, edate, expiratoindate, type);
                result = Common.DataTableToXml(dt, "GetMessageList", "", "List");
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }

        #region GetDetailXML
        public string GetDetailXML(string msgID)
        {
            #region Build the XML Schema
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GetMessageDetail>" +
                            "<Result>False</Result>" +
                            "<Description></Description><MsgReceiverList></MsgReceiverList>" +
                            "</GetMessageDetail>";
            #endregion
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                string sql = "Select * from [Messages] Where FID= '" + msgID + "'";
                SQLServerHelper runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);

                #region Set　MessageDetail XML Value
                if (dt.Rows.Count > 0)
                {
                    doc.SelectSingleNode("GetMessageDetail/Result").InnerText = "True";
                    XmlNode cNode = doc.SelectSingleNode("GetMessageDetail");
                    int indx = 0;
                    XmlNode vNode = doc.CreateElement("FMsgID");
                    vNode.InnerText = msgID;
                    cNode.AppendChild(vNode);


                    vNode = doc.CreateElement("FSubject");
                    vNode.InnerText = dt.Rows[indx]["FSubject"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FType");
                    vNode.InnerText = dt.Rows[indx]["FType"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FContent");
                    vNode.InnerText = dt.Rows[indx]["FContent"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FEmployeeID");
                    vNode.InnerText = dt.Rows[indx]["FEmployeeID"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FDate");
                    vNode.InnerText = dt.Rows[indx]["FDate"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FKeyword");
                    vNode.InnerText = dt.Rows[indx]["FKeyword"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FHttp");
                    vNode.InnerText = dt.Rows[indx]["FHttp"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FTransfer");
                    vNode.InnerText = dt.Rows[indx]["FTransfer"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FIsPulic");
                    vNode.InnerText = dt.Rows[indx]["FIsPulic"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FSentDate");
                    vNode.InnerText = dt.Rows[indx]["FSentDate"].ToString();
                    cNode.AppendChild(vNode);

                    vNode = doc.CreateElement("FExpirationDate");
                    vNode.InnerText = dt.Rows[indx]["FExpirationDate"].ToString();
                    cNode.AppendChild(vNode);


                    if (!Boolean.Parse(dt.Rows[indx]["FIsPulic"].ToString()))//非广播消息，读取相应的接收者
                    {
                        sql = "Select t1.FMsgID ,t1.FReceiverID,Isnull(t2.FName,'') As FReceiverName,Isnull(t1.FCommend,'') As FCommend ,t1.FIsRed From MsgReceivers t1 Left Join t_Items t2 On t1.FReceiverID= t2.FID";
                        sql = sql + " Where t1.FMsgID = '" + msgID + "' ";
                        dt = runner.ExecuteSql(sql);
                        XmlNode pNode = doc.SelectSingleNode("GetMessageDetail/MsgReceiverList");
                        foreach (DataRow row in dt.Rows)
                        {
                            cNode = doc.CreateElement("ReceiverRow");
                            pNode.AppendChild(cNode);

                            vNode = doc.CreateElement("FReceiverID");
                            vNode.InnerText = row["FReceiverID"].ToString();
                            cNode.AppendChild(vNode);

                            vNode = doc.CreateElement("FReceiverName");
                            vNode.InnerText = row["FReceiverName"].ToString();
                            cNode.AppendChild(vNode);

                            vNode = doc.CreateElement("FCommend");
                            vNode.InnerText = row["FCommend"].ToString();
                            cNode.AppendChild(vNode);

                            vNode = doc.CreateElement("FIsRed");
                            vNode.InnerText = row["FIsRed"].ToString();
                            cNode.AppendChild(vNode);
                        }
                    }
                }
                #endregion

                result = doc.InnerXml;
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion

        #region Update 
        public string Update(string dataString)
        {
            string id = "", sql = "", employeeID = "", valueString = "", isPublic = "1";
            string result = "-1";
            SQLServerHelper runner = new SQLServerHelper();
            try
            {
                if (!CheckUpateData(dataString))
                    result= "-1";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataString);
                id = doc.SelectSingleNode("UpdateMessage/MessageID").InnerText;
                if (id.Trim() == "" || id.Trim() == "-1")//新增消息
                {
                    id = Guid.NewGuid().ToString();
                    sql = "Insert into [Messages](FID) Values('" + id + "') ";
                    if (runner.ExecuteSqlNone(sql) < 0)//插入失败
                    {

                    }
                }

                //更新消息信息
                string val="";
                XmlNode vNode  = doc.SelectSingleNode("UpdateMessage/FSubject");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSubject='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateMessage/FType");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    employeeID = val;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FType='" + val + "',";
                }


                vNode = doc.SelectSingleNode("UpdateMessage/FContent");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FContent='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateMessage/FEmployeeID");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FEmployeeID='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateMessage/FDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FDate='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateMessage/FKeyword");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FKeyword='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateMessage/FHttp");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FHttp='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateMessage/FTransfer");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FTransfer='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateMessage/FIsPulic");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    isPublic = val;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FIsPulic='" + val + "',";
                }
                vNode = doc.SelectSingleNode("UpdateMessage/FSentDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FSentDate='" + val + "',";
                }

                vNode = doc.SelectSingleNode("UpdateMessage/FExpirationDate");
                if (vNode != null)
                {
                    val = vNode.InnerText;
                    if (val.Trim().Length > 0)
                        valueString = valueString + "FExpirationDate='" + val + "',";
                }

                if (valueString.Trim().Length > 0)
                {
                    valueString = valueString.Substring(0, valueString.Length - 1);
                    sql = "Update [Messages] Set " + valueString + " Where FID='" + id + "'";
                    if (runner.ExecuteSqlNone(sql) < 0)//更新消息失败
                    {

                    }
                }
                if (isPublic.Equals("0"))//更新接收者
                {
                    vNode = doc.SelectSingleNode("UpdateMessage/FReceiverID");
                    if (vNode != null)
                    {
                        val = vNode.InnerText;
                        if (val.Trim().Length > 0)
                        {
                            sql = "Delete from MsgReceivers Where  FMsgID ='" + id + "' and FIsRed=0";//删除尚未阅读的
                            runner.ExecuteSqlNone(sql);
                            string[] executors = val.Split(new[] { ';' });
                            for (int i = 0; i < executors.Length; ++i)
                            {
                                sql = "Insert Into MsgReceivers(FMsgID,FReceiverID,FIsRed) Values(" + "'" + id + "','" + executors[i] + "',0)";
                                runner.ExecuteSqlNone(sql).ToString();
                            }
                        }

                    }
                }
            }
            catch(Exception err)
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
            XmlNode vNode = doc.SelectSingleNode("UpdateMessage/FEmployeeID");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
                throw new Exception("消息创建者ID不能为空");
            vNode = doc.SelectSingleNode("UpdateMessage/FSubject");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
                throw new Exception("消息主题不能为空");

            vNode = doc.SelectSingleNode("UpdateMessage/FContent");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
                throw new Exception("消息内容不能为空");

            vNode = doc.SelectSingleNode("UpdateMessage/FIsPulic");
            if (vNode == null || vNode.InnerText.Trim().Length == 0)
            { }
            else if (vNode.InnerText.Trim().Equals("0"))//非广播消息
            {
                vNode = doc.SelectSingleNode("UpdateMessage/FReceiverID");
                if (vNode == null || vNode.InnerText.Trim().Length == 0)
                    throw new Exception("非广播消息，接受者不能为空");
            }
            result = true;
            return result;
        }
        #endregion  

        #region
        public string GetLogList(string xmlString)
        {
            string callType ="GetLogList";
            string filter = "", val = "";

            string result ="<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                          "<" + callType + ">" +
                          "<Result>False</Result>" +
                          "<Description></Description></" + callType + ">";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode("GetLogList/BeginDate");

                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim()+" 0:0:0.000";
                    filter = filter.Length > 0 ? filter = filter + " And FDate>='" + val + "'" : " FDate>='" + val + "'";
                }
                vNode = doc.SelectSingleNode("GetLogList/EndDate");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim()+ " 23:59:59.999";
                    filter = filter.Length > 0 ? filter = filter + " And FDate<='" + val + "'" : "FDate<='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetLogList/Type");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And FType='" + val + "'" : "FType='" + val + "'";
                }

                vNode = doc.SelectSingleNode("GetLogList/Method");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And FMethod='" + val + "'" : "FMethod='" + val + "'";
                }
                string sql = "Select FDate,FType,FCaller,FMethod,FLog From AppLogs ";
                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                if(dt.Rows.Count >0)
                    result = Common.DataTableToXml(dt, "GetLogList", "", "List");
            }
            catch(Exception err)
            {
                throw err;
            }
            return result;
        }
        #endregion
    }
}
