using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Configuration;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Linq;

namespace iTR.Lib
{
    public class Common
    {
        #region 非对称加密、解密

        public static string RSAEncrypt(string publicKey, string message)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            int maxBlockSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制
            rsa.FromXmlString(publicKey);
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            MemoryStream ms = new MemoryStream();
            for (int i = 0; i < bytes.Length; i += maxBlockSize)
            {
                int length = Math.Min(maxBlockSize, bytes.Length - i);

                byte[] newBytes = new byte[length];
                Array.Copy(bytes, i, newBytes, 0, length);
                byte[] plaintbytes = rsa.Encrypt(newBytes, false);
                ms.Write(plaintbytes, 0, plaintbytes.Length);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="message">待解密信息</param>
        /// <returns>解密后的信息</returns>
        public static string RSADecrypt(string privateKey, string message)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            int maxBlockSize = rsa.KeySize / 8; //解密块最大长度限制
            rsa.FromXmlString(privateKey);
            string result = "";
            byte[] bytes = Convert.FromBase64String(message);
            for (int i = 0; i < bytes.Length; i += maxBlockSize)
            {
                int length = Math.Min(maxBlockSize, bytes.Length - i);
                byte[] newBytes = new byte[length];
                Array.Copy(bytes, i, newBytes, 0, length);
                byte[] plaintbytes = rsa.Decrypt(newBytes, false);
                string temp = Encoding.UTF8.GetString(plaintbytes);
                result = result + temp;
            }
            return result;
        }

        #endregion 非对称加密、解密

        #region 解码邀请码

        public static string Decode(string code)
        {
            //提取用户id
            string str = "";
            string uid = "";
            for (int i = 0; i < code.Length; i += 2)
            {
                str += code.Substring(i, 1);
            }
            //剔除高位零
            for (int i = 0; i < str.Length; i++)
            {
                if (!str.Substring(i, 1).Equals("0"))
                {
                    str = str.Substring(i, str.Length - i);
                    break;
                }
            }
            uid = Convert.ToInt32(str, 16).ToString();

            return uid;
        }

        #endregion 解码邀请码

        #region 生成邀请码

        public static string CreateCode(int uid)
        {
            //十进制转十六进制
            string hexid = Convert.ToString(uid, 16);

            int len = 8;
            string sourcecode = "MWX89FCDG3J2RS1TUKLYZE5V67HQA4BNP";

            //高位补零
            string str = "";

            for (int i = len / 2; i > hexid.Length; i--)
            {
                str += "0";
            }
            str += hexid;

            //插入随机字符

            Random ran = new Random();
            for (int i = 1; i < str.Length + 1; i += 2)
            {
                str = str.Insert(i, sourcecode.Substring(ran.Next(0, sourcecode.Length - 1), 1));
            }
            return str;
        }

        #endregion 生成邀请码

        #region DES加密/解密

        //默认密钥向量,用于密码加密
        public const string DesKey = @"TR-Karon";

        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));

                byte[] rgbIV = Keys;

                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                cStream.Write(inputByteArray, 0, inputByteArray.Length);

                cStream.FlushFinalBlock();

                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);

                byte[] rgbIV = Keys;

                byte[] inputByteArray = Convert.FromBase64String(decryptString);

                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                cStream.Write(inputByteArray, 0, inputByteArray.Length);

                cStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        #endregion DES加密/解密

        #region DataTableToXml

        ///List:DataRows/DataRow
        ///Detail:DataRow
        ///Main:None DataRow
        public static string DataTableToXml(DataTable dt, string nodeName, string cols = "", string type = "Detail")
        {
            string result = "<" + nodeName + ">" +
                                "<Result>False</Result>" +
                                "<Description></Description>";

            result = result + (type == "List" ? "<DataRows></DataRows>" : "") + "</" + nodeName + ">";
            if (dt.Rows.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                doc.SelectSingleNode(nodeName + "/Result").InnerText = "True";

                XmlNode pNode = null;
                if (type == "Detail" || type == "Main")
                    pNode = doc.SelectSingleNode(nodeName);
                else if (type == "List")
                {
                    pNode = doc.SelectSingleNode(nodeName + "/DataRows");
                }
                foreach (DataRow row in dt.Rows)
                {
                    XmlNode cNode = null;
                    if (type == "Main")
                        cNode = pNode;
                    else
                    {
                        cNode = doc.CreateElement("DataRow");
                        pNode.AppendChild(cNode);
                    }
                    XmlNode vNode = null;
                    string colName = "";
                    if (cols.Trim().Length == 0)
                    {
                        foreach (DataColumn col in dt.Columns)
                        {
                            colName = col.Caption;
                            if (colName == "FID")
                                vNode = doc.CreateElement("ID");
                            else
                                vNode = doc.CreateElement(colName);

                            if (col.DataType.ToString() == "System.DateTime")
                            {
                                if (row[colName].ToString() != "")
                                {
                                    vNode.InnerText = (DateTime.Parse(row[colName].ToString())).ToString("yyyy-MM-dd HH:mm");
                                }
                                else
                                    vNode.InnerText = "";
                            }
                            else
                                vNode.InnerText = row[colName].ToString();
                            cNode.AppendChild(vNode);
                        }
                    }
                    else
                    {
                        string[] dtColumns = cols.Split('|');
                        for (int i = 0; i < dtColumns.Length; ++i)
                        {
                            colName = dtColumns[i];
                            if (colName == "FID")
                                vNode = doc.CreateElement("ID");
                            else
                                vNode = doc.CreateElement(colName);

                            vNode.InnerText = row[colName].ToString();
                            cNode.AppendChild(vNode);
                        }
                    }
                }
                result = doc.OuterXml;
                return result;
            }
            return result;
        }

        #endregion DataTableToXml

        #region DataTableToXmlEx

        /// <summary>
        ///
        /// </summary>
        /// <param name="mainform">主表DataTable</param>
        /// <param name="sonform">字表DataTable</param>
        /// <param name="nodeName">根节点字符窜</param>
        /// <param name="mainCols">主表返回列明，默认全部主表DataTable中列</param>
        /// <param name="sonCols">字表返回列明，默认全部子表DataTable中列</param>
        /// <returns></returns>
        public static string DataTableToXmlEx(DataTable mainform, DataTable sonform, string nodeName, string mainCols = "", string sonCols = "")
        {
            string result = "<" + nodeName + ">" +
                                "<Result>False</Result>" +
                                 "<DataRows></DataRows>" +
                                "<Description></Description></" + nodeName + ">";

            string colName = "";

            if (mainform.Rows.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                doc.SelectSingleNode(nodeName + "/Result").InnerText = "True";

                XmlNode pNode = null, vNode = null;
                pNode = doc.SelectSingleNode(nodeName);
                DataRow mainRow = mainform.Rows[0];
                if (mainCols.Trim().Length == 0)
                {
                    foreach (DataColumn col in mainform.Columns)
                    {
                        colName = col.Caption;
                        if (colName == "FID")
                            vNode = doc.CreateElement("ID");
                        else
                            vNode = doc.CreateElement(colName);

                        if (col.DataType.ToString() == "System.DateTime")
                        {
                            if (mainRow[colName].ToString() != "")
                            {
                                vNode.InnerText = (DateTime.Parse(mainRow[colName].ToString())).ToString("yyyy-MM-dd HH:mm");
                            }
                            else
                                vNode.InnerText = "";
                        }
                        else
                            vNode.InnerText = mainRow[colName].ToString();
                        pNode.AppendChild(vNode);
                    }
                }
                else
                {
                    string[] dtColumns = mainCols.Split('|');
                    for (int i = 0; i < dtColumns.Length; ++i)
                    {
                        colName = dtColumns[i];
                        if (colName == "FID")
                            vNode = doc.CreateElement("ID");
                        else
                            vNode = doc.CreateElement(colName);

                        vNode.InnerText = mainRow[colName].ToString();
                        pNode.AppendChild(vNode);
                    }
                }

                //子表数据
                pNode = doc.SelectSingleNode(nodeName + " /DataRows");
                foreach (DataRow sonRow in sonform.Rows)
                {
                    XmlNode cNode = cNode = doc.CreateElement("DataRow");
                    if (sonCols.Trim().Length == 0)
                    {
                        foreach (DataColumn col in sonform.Columns)
                        {
                            colName = col.Caption;
                            vNode = doc.CreateElement(colName);
                            if (col.DataType.ToString() == "System.DateTime")
                            {
                                if (sonRow[colName].ToString() != "")
                                {
                                    vNode.InnerText = (DateTime.Parse(sonRow[colName].ToString())).ToString("yyyy-MM-dd HH:mm");
                                }
                                else
                                    vNode.InnerText = "";
                            }
                            else
                                vNode.InnerText = sonRow[colName].ToString();

                            cNode.AppendChild(vNode);
                        }
                        pNode.AppendChild(cNode);
                    }
                    else
                    {
                        string[] dtColumns = mainCols.Split('|');
                        for (int i = 0; i < dtColumns.Length; ++i)
                        {
                            colName = dtColumns[i];

                            vNode = doc.CreateElement(colName);

                            vNode.InnerText = sonRow[colName].ToString();

                            cNode.AppendChild(vNode);
                        }
                    }

                    pNode.AppendChild(cNode);
                }
                result = doc.OuterXml;
                return result;
            }
            return result;
        }

        #endregion DataTableToXmlEx

        #region GetFieldValuesFromXmlEx

        /// <summary>
        ///
        /// </summary>
        /// <param name="xmlString">Xml格式参数字符串</param>
        /// <param name="callType">方法，也是Xml参数的根节点</param>
        /// <param name="results">主表Field-Value键值对</param>
        /// <param name="valMode">1：数据校验；其他不校验数据，一般用于查询条件参数读取</param>
        /// <param name="sonDataFormat">XML格式：DataRows/DataRow；非XML格式，<DataRows><fieldName1>val1|val2|val3</fieldName1></DataRows></param>
        /// <returns>字表Field-Value键值对List</returns>
        public static Dictionary<string, string> GetFieldValuesFromXmlEx(string xmlString, string callType, out List<Dictionary<string, string>> results, string valMode = "1", string sonDataFormat = "XML")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<Dictionary<string, string>> rows = new List<System.Collections.Generic.Dictionary<string, string>>();

            try
            {
                XmlDocument fieldconfigDoc = new XmlDocument();
                XmlNode fieldNode = null;
                string colName = "";
                //string fieldConfig=ConfigurationManager.AppSettings[callType];
                String xmlFileName = AppDomain.CurrentDomain.BaseDirectory;

                xmlFileName = xmlFileName + (xmlFileName.Substring(xmlFileName.Length - 1) == @"\" ? "" : @"\") + "FormConfiguration.xml";
                fieldconfigDoc.Load(xmlFileName);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(callType);
                //读取主表数据域值
                foreach (XmlNode node in vNode.ChildNodes)
                {
                    if (node.Name.Equals("AuthCode")) continue;//授权码忽略
                    if (node.Name.Equals("DataRows")) continue;//
                    if (valMode == "1")//需要校验
                    {
                        colName = node.Name.Substring(0, 1) == "F" ? node.Name : "F" + node.Name;
                        fieldNode = fieldconfigDoc.SelectSingleNode("FormCofiguration/" + callType + "/FieldList/" + node.Name);
                        if (fieldNode != null)//存在干预的字段配置信息
                        {
                            if (fieldNode.Attributes["NotNull"].Value == "T")//需要非空控制
                            {
                                if (node.InnerText.Trim().Length == 0)
                                {
                                    throw new Exception(fieldNode.Attributes["ErrMassage"].Value);
                                }
                            }
                        }
                    }

                    result[colName] = node.InnerText.Trim();
                }
                //DataRows/DataRow格式
                if (sonDataFormat == "XML")
                {
                    // 读取字表数据域的值
                    XmlNodeList datarows = doc.SelectNodes(callType + "/DataRows/DataRow");
                    foreach (XmlNode node in datarows)
                    {
                        Dictionary<string, string> dicfield = new Dictionary<string, string>();
                        foreach (XmlNode cNode in node.ChildNodes)
                        {
                            colName = cNode.Name.Substring(0, 1) == "F" ? cNode.Name : "F" + cNode.Name;
                            if (valMode == "1")//需要校验
                            {
                                fieldNode = fieldconfigDoc.SelectSingleNode("FormCofiguration/" + callType + "/FieldList/" + cNode.Name);
                                if (fieldNode != null)//存在干预的字段配置信息
                                {
                                    if (fieldNode.Attributes["NotNull"].Value == "T")//需要非空控制
                                    {
                                        if (cNode.InnerText.Trim().Length == 0)
                                        {
                                            throw new Exception(fieldNode.Attributes["ErrMassage"].Value);
                                        }
                                    }
                                }
                            }

                            dicfield[colName] = cNode.InnerText.Trim();
                        }
                        rows.Add(dicfield);
                    }
                }
                else//<DataRows><FieldName>val1|val2</FieldName></DataRows>
                {
                    XmlNode sonDataNode = doc.SelectSingleNode(callType + "/DataRows");
                    List<Dictionary<string, string>> sonrows = new List<System.Collections.Generic.Dictionary<string, string>>();
                    int sonRowCount = 1;
                    foreach (XmlNode field in sonDataNode.ChildNodes)
                    {
                        colName = field.Name.Substring(0, 1) == "F" ? field.Name : "F" + field.Name;
                        Dictionary<string, string> dicfield = new Dictionary<string, string>();
                        string[] vals = field.InnerText.Trim().Split('|');
                        for (sonRowCount = 1; sonRowCount <= vals.Length; sonRowCount++)
                        {
                            dicfield[sonRowCount.ToString() + colName] = vals[sonRowCount - 1];
                        }
                        sonrows.Add(dicfield);
                    }

                    for (int i = 1; i < sonRowCount; i++)
                    {
                        Dictionary<string, string> dicfield = new Dictionary<string, string>();

                        foreach (Dictionary<string, string> valList in sonrows)
                        {
                            foreach (string key in valList.Keys)
                            {
                                if (key.Substring(0, 1) == i.ToString())
                                {
                                    string fieldName = key.Substring(1, key.Length - 1);
                                    dicfield[fieldName] = valList[key];

                                    if (valMode == "1")//需要校验
                                    {
                                        fieldNode = fieldconfigDoc.SelectSingleNode("FormCofiguration/" + callType + "/FieldList/" + fieldName);
                                        if (fieldNode != null)//存在干预的字段配置信息
                                        {
                                            if (fieldNode.Attributes["NotNull"].Value == "T")//需要非空控制
                                            {
                                                if (valList[key].Trim().Length == 0)
                                                    throw new Exception(fieldNode.Attributes["ErrMassage"].Value);
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        rows.Add(dicfield);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            results = rows;
            return result;
        }

        #endregion GetFieldValuesFromXmlEx

        #region 获取罗盘UI配置信息

        public static string GetCompassConfigFromXml(string fieldNodeName)
        {
            string result = "";
            try
            {
                XmlDocument fieldconfigDoc = new XmlDocument();

                //string fieldConfig=ConfigurationManager.AppSettings[callType];
                String xmlFileName = AppDomain.CurrentDomain.BaseDirectory;
                if (fieldNodeName.Length == 0)
                {
                    return "";
                };

                xmlFileName = xmlFileName + (xmlFileName.Substring(xmlFileName.Length - 1) == @"\" ? "" : @"\") + "FormConfiguration.xml";
                fieldconfigDoc.Load(xmlFileName);
                XmlNode fieldNode = fieldconfigDoc.SelectSingleNode("FormCofiguration/CompassConfig/" + fieldNodeName);

                if (result == null)
                {
                    return "";
                }
                result = fieldNode.Attributes["UI"].InnerText;
            }
            catch (Exception err)
            {
                FileLogger.WriteToDB(err.Message, "ErrMessage", fieldNodeName, "GetCompassConfig");
            }
            return result;
        }

        #endregion 获取罗盘UI配置信息

        #region GetFieldValuesFromXml

        public static Dictionary<string, string> GetFieldValuesFromXml(string xmlString, string callType, string fieldNodeName = "", string valMode = "1")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            try
            {
                XmlDocument fieldconfigDoc = new XmlDocument();

                //string fieldConfig=ConfigurationManager.AppSettings[callType];
                String xmlFileName = AppDomain.CurrentDomain.BaseDirectory;
                if (fieldNodeName.Length == 0) fieldNodeName = callType;

                xmlFileName = xmlFileName + (xmlFileName.Substring(xmlFileName.Length - 1) == @"\" ? "" : @"\") + "FormConfiguration.xml";
                fieldconfigDoc.Load(xmlFileName);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode vNode = doc.SelectSingleNode(fieldNodeName);
                //"FormCofiguration/"
                foreach (XmlNode node in vNode.ChildNodes)
                {
                    if (node.Name.Equals("AuthCode")) continue;//授权码忽略
                    if (node.Name.Equals("DataRows")) continue;//授权字表内容

                    string colName = "";
                    colName = node.Name.Substring(0, 1) == "F" ? node.Name : "F" + node.Name;
                    if (valMode == "1")
                    {
                        XmlNode fieldNoe = fieldconfigDoc.SelectSingleNode("FormCofiguration/" + callType + "/FieldList/" + node.Name);
                        if (fieldNoe != null)//存在干预的字段配置信息
                        {
                            if (fieldNoe.Attributes["NotNull"].Value == "T")//需要非空控制
                            {
                                if (node.InnerText.Trim().Length == 0)
                                {
                                    throw new Exception(fieldNoe.Attributes["ErrMassage"].Value);
                                }
                            }
                            switch (fieldNoe.Attributes["NotNull"].Value)
                            {
                                case "1"://整数
                                    break;

                                case "2"://小数
                                    break;

                                case "3"://文本
                                    break;

                                default://
                                    break;
                            }
                        }
                    }

                    result[colName] = node.InnerText.Trim();
                }
            }
            catch (Exception err)
            {
                FileLogger.WriteToDB(err.Message, "ErrMessage", callType, "GetFieldValuesFromXml");
                throw err;
            }
            return result;
        }

        #endregion GetFieldValuesFromXml

        #region GetImageXmlString

        public static void SetImageXmlNode(string pageID, string ownerID, ref XmlNode result, ref XmlDocument doc)
        {
            //XmlDocument doc = new XmlDocument();

            //doc.LoadXml("<Images></Images>");
            //XmlNode result = doc.SelectSingleNode("Images");
            string url = "http://ydb.tenrypharm.com:6060";
            string sql = "SELECT (FPath + '/' + FFileName) As FPath1,(FPath + '/T_' + FFileName) As FPath2  FROM Attachments Where FPageID='{0}' and FOwnerID='{1}' and FDeleted=0 ";
            sql = string.Format(sql, pageID, ownerID);
            SQLServerHelper runner = new SQLServerHelper();
            DataTable dt = runner.ExecuteSql(sql);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    XmlNode cNode = doc.CreateElement("Image");
                    cNode.InnerText = url + "/" + row["FPath2"].ToString();
                    XmlAttribute attr = doc.CreateAttribute("Original");
                    attr.Value = url + "/" + row["FPath1"].ToString();
                    cNode.Attributes.Append(attr);
                    result.AppendChild(cNode);
                }
            }
        }

        #endregion GetImageXmlString

        #region CalcWeekDay

        public static bool CalcWeekDay(int year, int week, out DateTime first, out DateTime last)
        {
            first = DateTime.MinValue;
            last = DateTime.MinValue;
            //年份超限
            if (year < 1700 || year > 9999) return false;
            //周数错误
            if (week < 1 || week > 53) return false;
            //指定年范围
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year, 12, 31);
            int startWeekDay = (int)start.DayOfWeek;
            //周的起始日期
            first = start.AddDays((7 - startWeekDay) + (week - 2) * 7);
            last = first.AddDays(6);
            //结束日期跨年
            return (last <= end);
        }

        #endregion CalcWeekDay

        #region SerializeDataTableToJson

        /// <summary>
        /// 将DataTable返回JSon
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string SerializeDataTableToJson(DataTable dt)
        {
            string rtn = "";

            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss" };
            rtn = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented, timeConverter);
            return rtn;
        }

        #endregion SerializeDataTableToJson

        #region DataTalbeToJson

        /// <summary>
        /// 将DataTable返回JSon
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTalbeToJson(DataTable dt, string notSerializeFields = "")
        {
            string result = "-1";
            string[] fs = null;
            string errcode = "0000";
            string description = "操作成功";
            StringBuilder listString = new StringBuilder();
            try
            {
                result = "{\"errcode\": \"{0}\",\"description\": \"{1}\",\"list\": [{2}]}";

                fs = notSerializeFields.Split('|');

                for (int rowindex = 0; rowindex < dt.Rows.Count; rowindex++)
                {
                    listString.Append("{");
                    for (int colindex = 0; colindex < dt.Columns.Count; colindex++)
                    {
                        for (int i = 0; i < fs.Length; i++)
                        {
                            if (fs[i].Trim().ToUpper().Equals(dt.Columns[colindex].ColumnName.ToUpper()))
                            {
                                continue;
                            }
                        }
                        if (colindex < dt.Columns.Count - 1)
                        {
                            listString.Append("\"" + dt.Columns[colindex].ColumnName + "\":" + "\"" + dt.Rows[rowindex][colindex].ToString() + "\",");
                        }
                        else
                        {
                            listString.Append("\"" + dt.Columns[colindex].ColumnName + "\":" + "\"" + dt.Rows[rowindex][colindex].ToString() + "\"");
                        }
                    }
                    if (rowindex == dt.Rows.Count - 1)
                    {
                        listString.Append("}");
                    }
                    else
                    {
                        listString.Append("},");
                    }
                }
            }
            catch (Exception err)
            {
                errcode = "-1";
                description = err.Message;
            }

            result = "{\"errcode\": \"" + errcode + "\",\"description\": \"" + description + "\",\"list\": [" + listString.ToString() + "]}";

            return result;
        }

        #endregion DataTalbeToJson

        #region XML2Json

        public static string XML2Json(string str, string nodename)
        {
            string result = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(str);
            XmlNode node = xmldoc.SelectSingleNode(nodename);
            result = Newtonsoft.Json.JsonConvert.SerializeXmlNode(node);
            return result;
        }

        #endregion XML2Json

        #region XML2Json

        public static string Json2XML(string jsonstring, string rootNode = "root")
        {
            //string result = null;
            //XmlDocument xml = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(str);
            //result = xml.OuterXml;
            //return result;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(jsonstring);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDec;
            xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement nRoot = doc.CreateElement(rootNode);
            doc.AppendChild(nRoot);
            foreach (KeyValuePair<string, object> item in Dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                nRoot.AppendChild(element);
            }
            return doc.OuterXml;
        }

        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> Source)
        {
            object kValue = Source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> item in kValue as Dictionary<string, object>)
                {
                    XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                    KeyValue2Xml(element, item);
                    node.AppendChild(element);
                }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                object[] o = kValue as object[];
                for (int i = 0; i < o.Length; i++)
                {
                    XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                    KeyValuePair<string, object> item = new KeyValuePair<string, object>("Item", o[i]);
                    KeyValue2Xml(xitem, item);
                    node.AppendChild(xitem);
                }
            }
            else
            {
                XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                node.AppendChild(text);
            }
        }

        #endregion XML2Json

        /// <summary>
        /// 从DataService.dbo.SysInfo 表读取关键参数值,
        /// </summary>
        /// <param name="system">系统，如短信系统SMS</param>
        /// <param name="key">关键参数的键值</param>
        /// <param name="cnnString">DataService数据库的连接字符串。若整个应用以通过别的地方配置了连接字符串，这样可以为空</param>
        /// <returns></returns>
        public Dictionary<string, string> GetSysInfo(string system = "SMS", string key = "", string cnnString = "")
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                string sql = "Select * from  DataService.dbo.SysInfos Where FSystem='" + system + "'";

                if (key.Trim().Length > 0)
                    sql = sql + " and FKey ='" + key + "'";

                SQLServerHelper runner = null;
                if (cnnString.Trim().Length > 0)
                    runner = new SQLServerHelper(cnnString);
                else
                    runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(dr["FKey"].ToString(), dr["FValue"].ToString());
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return result;
        }

        #region GetWeekIndexOfYear

        /// <summary>
        /// 获取年度中的周序号
        /// </summary>
        /// <param name="weekIndx">0：本周；-1：上周；1：下周</param>
        /// <param name="year">返回年度</param>
        /// <param name="weekOfyear">返回周序号</param>
        public static void GetWeekIndexOfYear(string weekIndx, out int year, out int weekOfyear)
        {
            int weekIdx, monyear, sunyear, maxMonWeekofYear;
            bool crossyear = false, mulValue = false;
            string mulyear = "", mulweek = "";
            year = DateTime.Now.Year;
            GregorianCalendar gc = new GregorianCalendar();

            //星期一的年份
            monyear = GetWeekFirstDayMon(DateTime.Now).Year;
            //星期日的年份
            sunyear = GetWeekLastDaySun(DateTime.Now).Year;
            //是否跨年
            if (monyear != sunyear)
            {
                crossyear = true;
            }
            //获取周一所属年份最大周序号
            maxMonWeekofYear = gc.GetWeekOfYear(DateTime.Parse(monyear.ToString() + "-" + "12-31"), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //获取年度最大周序号
            int maxweekOfYear = gc.GetWeekOfYear(DateTime.Parse(year.ToString() + "-" + "12-31"), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            // int maxweekOfYear = gc.GetWeekOfYear(DateTime.Parse(year.ToString() + "-" + DateTime.Now.ToString("MM-dd")), System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            weekOfyear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            weekIdx = int.Parse(weekIndx);
            weekOfyear = weekOfyear + weekIdx;

            //本周跨年，本周已经跨年 上周下周不会跨年 -------跨年周属于下一年
            if (crossyear)
            {
                if (weekIdx == 0)//本周
                {
                    weekOfyear = 1;
                    year = sunyear;
                }
                else if (weekIdx == -1)//上周
                {
                    year = monyear;
                    //周一所属年份最大周序号减1
                    weekOfyear = maxMonWeekofYear - 1;
                }
                else if (weekIdx == 1)//下周
                {
                    year = sunyear;
                    weekOfyear = 2;
                }
            }
            //本周不跨年， 有可能上周下周会跨年
            else
            {
                if (weekIdx == -1)//上周
                {
                    //周一日期减1天获取上周周一日期， 周一日期减去7天获取周日的日期，   根据周一和周日的年份判断是否在同一年
                    if (GetWeekFirstDayMon(DateTime.Now).AddDays(-1).Year != GetWeekFirstDayMon(DateTime.Now).AddDays(-7).Year)
                    {
                        weekOfyear = 1;
                        year = year;
                    }
                }
                else if (weekIdx == 1)//下周
                {
                    //周日日期加1天获取下周周一日期， 周日期加7天获取周日的日期，   根据周一和周日的年份判断是否在同一年
                    if (GetWeekLastDaySun(DateTime.Now).AddDays(1).Year != GetWeekLastDaySun(DateTime.Now).AddDays(7).Year)
                    {
                        weekOfyear = 1;
                        year = year + 1;
                    }
                }
            }
        }

        #endregion GetWeekIndexOfYear

        #region GetWeekIndexOfYearEx

        /// <summary>
        /// 获取年度中的周序号
        /// </summary>
        /// <param name="weekIndx">0：本周；-1：上周；1：下周；-11：上月；10：本月；11下月</param>
        /// <param name="year">返回年度</param>
        /// <param name="weekOfyear">返回周序号</param>
        public static void GetWeekIndexOfYearEx(string weekIndx, out string years, out string weekOfyears)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekIdx = int.Parse(weekIndx);
            int year = DateTime.Now.Year, currentMonth;
            int weekOfyear = gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //如果是周 GetWeekIndexOfYear方法里面操作
            GetWeekIndexOfYear(weekIndx, out year, out weekOfyear);
            //放在月操作之前，不然获取月份数据之后，会重新覆盖月的结果
            weekOfyears = weekOfyear.ToString();
            currentMonth = DateTime.Now.Month;
            //跨月周，属于下一月
            //重新赋值 weekOfyears

            //上月
            if (weekIdx == -11)
            {
                if (DateTime.Now.Month == 12)
                {
                    year = year - 1;
                    //去年十二月
                    weekOfyears = GetMonthsWeek(year, 12);
                }
                else
                {
                    weekOfyears = GetMonthsWeek(year, currentMonth - 1);
                }
            }
            //本月
            else if (weekIdx == 10)
            {
                weekOfyears = GetMonthsWeek(year, currentMonth);
            }
            //下月
            else if (weekIdx == 11)
            {
                if (DateTime.Now.Month == 12)
                {
                    year = year + 1;
                    //下一年一月
                    weekOfyears = GetMonthsWeek(year + 1, 1);
                }
                else
                {
                    weekOfyears = GetMonthsWeek(year, currentMonth + 1);
                }
            }

            years = year.ToString();
        }

        /// <summary>
        /// 获取属于某月的所有周序号
        /// </summary>
        /// <param name="year">某年</param>
        /// <param name="month">某月</param>
        /// <returns></returns>
        public static string GetMonthsWeek(int year, int month)
        {
            if (month == 0)
            {
                month = 12;
                year -= 1;
            }
            string monthweek;
            int firstweek;
            int lastweek;
            int days = DateTime.DaysInMonth(year, month);
            DateTime firstday = new DateTime(year, month, 1);
            DateTime lastday = new DateTime(year, month, days);
            GregorianCalendar gc = new GregorianCalendar();
            //月的第一天周序号
            firstweek = gc.GetWeekOfYear(firstday, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //月的最后一天周序号
            lastweek = gc.GetWeekOfYear(lastday, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //判断月的最后一周是否跨月，如果跨月属于下月
            if (lastday.DayOfWeek != DayOfWeek.Sunday)
            {
                lastweek -= 1;
            }
            List<string> list = new List<string>();

            for (; firstweek <= lastweek; firstweek++)
            {
                list.Add(firstweek.ToString());
            }
            monthweek = string.Join("|", list.ToArray());
            return monthweek;
        }

        #region 获取本周周一日期和周日日期

        /// <summary>
        /// 得到本周第一天(以星期一为第一天）
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        /// <summary>
        /// 得到本周最后一天(以星期天为最后一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }

        #endregion 获取本周周一日期和周日日期

        #endregion GetWeekIndexOfYearEx

        public static string GetYearSETime(DateTime dateTime)
        {
            int year = dateTime.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime lastDay = new DateTime(year, 12, 31);
            return firstDay.ToString("yyyy-MM-dd") + "&" + lastDay.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取月份的第一天和最后一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetMonthTime(DateTime dateTime)
        {
            //获取某年某月有多少天
            int monthDay = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

            DateTime d1 = new DateTime(dateTime.Year, dateTime.Month, 1);
            DateTime d2 = new DateTime(dateTime.Year, dateTime.Month, monthDay);
            return d1.ToString("yyyy-MM-dd") + "&" + d2.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取流水号 需要在 NumberConfig 表中，提前配置好各表的流水号规则
        ///
        /// （年号和重置规则： 0 按年份，1 按月份，2 按天）
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="fieldname">字段名</param>
        /// <returns></returns>

        public static string GetNumber(string tablename, string fieldname)
        {
            //年号规则
            string rule = "";
            int comrule = 99;
            int initvalue;
            //初始化的流水号
            int number = 1;
            //补0的位数
            int diff;
            //流水号位数
            int digit;
            //生成保存到数据库的相关项
            string savenumber, header, renumber, subffix;
            //插入日期格式的间隔符
            string formatstr = "";

            try
            {
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dataTable = new DataTable();
                //根据表名和位数获取 唯一代码规则
                string sql = $"select FHeader,FDigit,FResetRule, FReignRule,FInitialValue from [yaodaibao].[dbo].[NumberConfig] where FTableName = '{tablename}' and FFieldName='{fieldname}'";
                dataTable = runner.ExecuteSql(sql);
                if (dataTable.Rows.Count < 1)
                {
                    throw new Exception($"获取{tablename + fieldname} 流水号配置失败");
                }
                //获取流水号初始值
                initvalue = int.Parse(dataTable.Rows[0]["FInitialValue"].ToString());
                //获取位数
                digit = int.Parse(dataTable.Rows[0]["FDigit"].ToString());
                //头定义
                header = dataTable.Rows[0]["FHeader"].ToString();
                //获取重置规则
                comrule = int.Parse(dataTable.Rows[0]["FResetRule"].ToString());

                rule = dataTable.Rows[0]["FReignRule"] != null ? dataTable.Rows[0]["FReignRule"].ToString() : "";
                rule = DateTime.Now.ToString(rule);
                //获取该规则的最新的数据记录
                sql = $"select top 1 FIncreaseNumber,FCreateTime from [yaodaibao].[dbo].[Number] where FTableFieldName = '{tablename + fieldname}' order by FCreateTime desc";
                dataTable = runner.ExecuteSql(sql);

                //有该类型的记录
                if (dataTable.Rows.Count > 0)
                {
                    //年
                    if (comrule == 0)
                    {
                        //达到重置条件
                        if (TimeCompare(DateTime.Now, DateTime.Parse(dataTable.Rows[0]["FCreateTime"].ToString()), 0))
                        {
                            //重置起始值
                            number = initvalue;
                        }
                        else
                        {
                            number = Convert.ToInt32(dataTable.Rows[0]["FIncreaseNumber"]) + 1;
                        }
                    }
                    //月
                    else if (comrule == 1)
                    {
                        //达到重置条件
                        if (TimeCompare(DateTime.Now, DateTime.Parse(dataTable.Rows[0]["FCreateTime"].ToString()), 1))
                        {
                            //重置起始值
                            number = initvalue;
                        }
                        else
                        {
                            number = Convert.ToInt32(dataTable.Rows[0]["FIncreaseNumber"]) + 1;
                        }
                    }
                    //日
                    else if (comrule == 2)
                    {
                        //达到重置条件
                        if (TimeCompare(DateTime.Now, DateTime.Parse(dataTable.Rows[0]["FCreateTime"].ToString()), 2))
                        {
                            //重置起始值
                            number = initvalue;
                        }
                        else
                        {
                            number = Convert.ToInt32(dataTable.Rows[0]["FIncreaseNumber"]) + 1;
                        }
                    }
                    //其他情况不重置 流水号一直加
                    else
                    {
                        number = Convert.ToInt32(dataTable.Rows[0]["FIncreaseNumber"]) + 1;
                    }
                }
                else
                {
                    //无记录给定起始值
                    number = initvalue;
                }

                //用 number计算补多少位0
                diff = digit - number.ToString().Length;
                savenumber = number.ToString().Insert(0, new string('0', diff));

                renumber = header + rule + savenumber;
                sql = $"insert into [yaodaibao].[dbo].[Number](FTableFieldName,FIncreaseNumber,FCreateTime) values('{tablename + fieldname}',{savenumber},'{DateTime.Now.ToString()}')";

                //生成成功 返回流水号
                if (runner.ExecuteSqlNone(sql) > 0)
                {
                    return renumber;
                }
                else
                {
                    throw new Exception("生成流水号失败");
                }
            }
            catch (Exception err)
            {
                throw new Exception("生成流水号失败");
            }
        }

        /// <summary>
        /// 日期大小比较条件 如果第一个日期参数大于第二个日期参数返回true，否则返回false
        /// </summary>
        /// <param name="t1">日期转换后的字符串</param>
        /// <param name="t2">日期字符串</param>
        /// <param name="type">0.按年份比较，1按月份比较，2按天数比较, 其他按传入的日期最小单位比较</param>
        /// <returns></returns>
        private static bool TimeCompare(DateTime t1, DateTime t2, int type)
        {
            try
            {
                //年
                if (type == 0)
                {
                    //t1 大于 t2
                    if ((int.Parse(t1.ToString("yyyy")) - int.Parse(t2.ToString("yyyy"))) > 0)
                    {
                        return true;
                    }
                }
                //月
                else if (type == 1)
                {
                    //t1 大于 t2
                    if ((int.Parse(t1.ToString("yyyyMM")) - int.Parse(t2.ToString("yyyyMM"))) > 0)
                    {
                        return true;
                    }
                }
                //日
                else if (type == 2)
                {
                    //t1 大于 t2
                    if ((int.Parse(t1.ToString("yyyyMMdd")) - int.Parse(t2.ToString("yyyyMMdd"))) > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    //t1 大于 t2
                    if ((int.Parse(t1.ToString()) - int.Parse(t2.ToString())) > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        /// <summary>
        /// 根据查询条件获取具体的开始时间和结束时间
        /// </summary>
        /// <param name="weekIndex">周索引</param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetPerTime(string weekIndex)
        {
            DateTime startTime, endTime;
            string temptime;
            try
            {
                switch (weekIndex)
                {
                    //本年
                    case "-1000":
                        temptime = Common.GetYearSETime(DateTime.Now);
                        startTime = DateTime.Parse(temptime.Split('&')[0]);
                        endTime = DateTime.Parse(temptime.Split('&')[1]);
                        break;
                    //上月
                    case "-11":
                        temptime = Common.GetMonthTime(DateTime.Now.AddMonths(-1));
                        startTime = DateTime.Parse(temptime.Split('&')[0]);
                        endTime = DateTime.Parse(temptime.Split('&')[1]);
                        break;
                    //本月
                    case "10":
                        temptime = Common.GetMonthTime(DateTime.Now);
                        startTime = DateTime.Parse(temptime.Split('&')[0]);
                        endTime = DateTime.Parse(temptime.Split('&')[1]);
                        break;
                    //上周
                    case "-1":
                        startTime = Common.GetWeekFirstDayMon(DateTime.Now.AddDays(-7));
                        endTime = Common.GetWeekLastDaySun(DateTime.Now.AddDays(-7));
                        break;
                    //本周
                    case "0":
                        startTime = Common.GetWeekFirstDayMon(DateTime.Now);
                        endTime = Common.GetWeekLastDaySun(DateTime.Now);
                        break;

                    default:
                        throw new Exception();
                }
                return Tuple.Create(startTime, endTime);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        /// <summary>
        /// 根据查询条件获取周具体属于哪一年
        /// </summary>
        /// <param name="weekIndex">周索引</param>
        /// <returns>返回结果例如 202035|202036|202037 </returns>
        public static string GetYearWithWeeks(string weekIndex)
        {
            try
            {
                string years, weekOfyears;
                List<string> list = new List<string>();
                Common.GetWeekIndexOfYearEx(weekIndex, out years, out weekOfyears);
                string[] weeks = weekOfyears.Split('|');
                for (int i = 0; i < weeks.Length; i++)
                {
                    list.Add("'" + years + weeks[i] + "'");
                }
                return string.Join(",", list.ToArray());
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}