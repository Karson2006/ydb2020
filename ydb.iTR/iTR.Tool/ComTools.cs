using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTR.Lib;
using System.IO;
using System.Xml;
using ydb.BLL;
using System.Net;
using Newtonsoft.Json;


namespace iTR.Tool
{
    public partial class ComTools : Form
    {
        public ComTools()
        {
            InitializeComponent();
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            //string province = "", city = "", country = "",id="";
            //string pid = "", cid = "", name = "";
            //int level = 0;
            string jasonString = "";

            try
            {
                #region OldCode
                //string sql = "Select Code,IsNull(FProvince,'') As FProvince ,IsNull(FCity,'') As FCity ,IsNull(FCountry,'') As FCountry  From Region_Old  Order by Code Asc";
                //SQLServerHelper runner = new SQLServerHelper();
                //DataTable dt = runner.ExecuteSql(sql);
                //foreach (DataRow row in dt.Rows)
                //{
                //    id = Guid.NewGuid().ToString();
                //    province = row["FProvince"].ToString();
                //    city = row["FCity"].ToString();
                //    country = row["FCountry"].ToString();
                //    string code = row["Code"].ToString();

                //    if (country.Trim().Length > 0 && city.Trim().Length > 0 && province.Trim().Length > 0)
                //    {
                //        level = 3;
                //        name = country;
                //        sql = "Insert Into RegionInfo (FID,FName,FNumber,FFullNumber,FParentID,FLevel,FIsDetail,FRegionName,FProvinceID,FCityID,FCountryID) Values('";
                //        sql = sql + id + "','" + name + "','" + code + "','" + code + "','" + cid + "','" + level.ToString() + "',1,'"+province +"_"+city +"_"+country+"','"+pid+"','"+cid+"','"+id+"')";
                //    }
                //    else if (country.Trim().Length == 0 && city.Trim().Length > 0 && province.Trim().Length > 0)
                //    {
                //        level = 2;
                //        cid = id;
                //        name = city;

                //        sql = "Insert Into RegionInfo (FID,FName,FNumber,FFullNumber,FParentID,FLevel,FIsDetail,FRegionName,FProvinceID,FCityID,FCountryID) Values('";
                //        sql = sql + id + "','" + name + "','" + code + "','" + code + "','" + pid + "','" + level.ToString() + "',0,'" + province + "_" + city + "_" + country + "','" + pid + "','" + cid + "','-1')";

                //        //sql = "Insert Into RegionInfo (FID,FName,FNumber,FFullNumber,FParentID,FLevel,FIsDetail) Values('";
                //        //sql = sql + id + "','" + name + "','" + code + "','" + code + "','" + pid + "','" + level.ToString() + "',0)";
                //    }
                //    else if (country.Trim().Length == 0 && city.Trim().Length == 0 && province.Trim().Length > 0)
                //    {
                //        level = 1;
                //        name = province;
                //        pid = id;

                //        sql = "Insert Into RegionInfo (FID,FName,FNumber,FFullNumber,FParentID,FLevel,FIsDetail,FRegionName,FProvinceID,FCityID,FCountryID) Values('";
                //        sql = sql + id + "','" + name + "','" + code + "','" + code + "','-1','" + level.ToString() + "',0,'" + province + "_" + city + "_" + country + "','" + pid + "','-1','-1')";

                //        //sql = "Insert Into RegionInfo (FID,FName,FNumber,FFullNumber,FParentID,FLevel,FIsDetail) Values('";
                //        //sql = sql + id + "','" + name + "','" + code + "','" + code + "','" + "-1" + "','" + level.ToString() + "',0)";
                //    }
                //    runner.ExecuteSql(sql);

                #endregion

                string sql = "Select FID,FName from t_Items where FClassID ='10ad26f5-220a-424d-b4e4-36591dc9021e' and FLevel=1 and FIsDeleted=0  order by FNumber asc";
                SQLServerHelper runner = new SQLServerHelper();
                DataTable dt = runner.ExecuteSql(sql);
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    DataRow row = dt.Rows[i];
                    jasonString = jasonString + "{\r\n\"" + row["FID"].ToString() + "\":{\r\n";
                    jasonString = jasonString + "\"name\":" + row["FName"].ToString() + ",\r\n" + "\"child\":{\r\n";
                    //地市
                    sql = "Select FName,FID from t_Items where FParentID ='{0}' and FIsDeleted=0 order by FNumber asc";
                    sql = string.Format(sql, row["FID"].ToString());
                    DataTable citydt = runner.ExecuteSql(sql);
                    for (int j = 0; j < citydt.Rows.Count; ++j)
                    {
                        DataRow cityRow = citydt.Rows[j];
                        jasonString = jasonString + "\"" + cityRow["FID"].ToString() + "\":{\r\n";
                        jasonString = jasonString + "\"name\":" + cityRow["FName"].ToString() + ",\r\n" + "\"child\":{\r\n";
                        //县市
                        sql = "Select FName,FID from t_Items where FParentID ='{0}' and FIsDeleted=0 order by FNumber asc";
                        sql = string.Format(sql, cityRow["FID"].ToString());
                        DataTable towndt = runner.ExecuteSql(sql);
                        foreach (DataRow townRow in towndt.Rows)
                        {
                            jasonString = jasonString + "\'" + townRow["FID"].ToString() + "\":\"" + townRow["FName"].ToString() + "\",\r\n";
                        }
                        
                        if (j+1 < citydt.Rows.Count)
                        {
                            jasonString = jasonString.Substring(0, jasonString.Length - 3) + "\r\n}\r\n}\r\n},\r\n";
                        }
                        else
                        { 
                            jasonString = jasonString.Substring(0, jasonString.Length - 3) + "\r\n}\r\n}\r\n}\r\n"; 
                        }
                    }
                    if (i+1 < dt.Rows.Count)
                    {
                        jasonString = jasonString + "},\r\n";
                    }
                    else
                    {
                        jasonString = jasonString + "}\r\n";
                    }
                }
                jasonString = jasonString.Substring(0,jasonString.Length -3);

                string path= AppDomain.CurrentDomain.BaseDirectory + "\\RegionInfo.txt";
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                       
                        sw.WriteLine(jasonString);
                        sw.Flush();
                    }
                }

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //invatationCode.Text = iTR.Lib.CommonCommon.CreateCode(int.Parse(uid.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
           //uid.Text = Common.Decode(invatationCode.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //textBox1.Text = Common.EncryptDES(textBox2.Text,Common.DesKey);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //textBox2.Text = Common.DecryptDES(textBox1.Text, Common.DesKey);
        }

        private void button5_Click(object sender, EventArgs e)
        {


            //FileHelper.UploadImage(base64String, "", "cui.jpg", "1001", "wwwww1111");

            XmlDocument doc = new XmlDocument();
            string xmlString = "<UploadRegImage>" +
                               "<AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>" +
                               "<Base64String></Base64String>" +
                               "<FileName>cui.jpg</FileName>" +
                               "<PageID>MKT001</PageID>" +
                               "<OwnerID>082d3899-e880-4784-a09b-97ad407e08b5</OwnerID>" +
                               "<FileNum>1|1</FileNum>" +
                               "</UploadRegImage>";
            doc.LoadXml(xmlString);

            string path = @"D:\cui.jpg";//本地路径
            byte[] bytes = GetBytesByPath(path);//获取文件byte[]

            string base64String = Convert.ToBase64String(bytes);

            doc.SelectSingleNode("UploadRegImage/Base64String").InnerText = base64String;

            xmlString = doc.InnerXml;
            RegApplication reg = new RegApplication();
            //doc.Save(@"D:\Work\yaodaibao\Release\uploadfile.xml");
            //reg.UploadFile(xmlString);
            //doc.LoadXml(xmlString);
            //string pstring = "UploadRegImage" + ";" + xmlString;
            WebInvoke invoke = new WebInvoke();
            //object[] p = pstring.Split(';');
            string result = reg.UploadImage(xmlString);


            //FileLogger.WriteLog("test", 1);
            //string path = @"D:\发票2.jpg";//本地路径
            //byte[] bytes = GetBytesByPath(path);//获取文件byte[]

            //string base64String = Convert.ToBase64String(bytes);

            //object[] parm = new object[] { "2", base64String };
            ////object result = invoke.Invoke("http://oa.tenrypharm.com:8888/seeyon/services/authorityService?wsdl", "authorityService", "authenticate", parm, null, 60);

            ////string url = @"http://duzy.top:8080/microService/api/invoice/check";
            ////object result =  Post(url, parm, 15);
            ////object[] parm = new object[] { "service-admin", "123456" };
            ////object result = invoke.Invoke("http://oa.tenrypharm.com:8888/seeyon/services/authorityService?wsdl", "authorityService", "authenticate", parm, null, 60);


            ////xmlString = invoke.Invoke(@"http://ydb.tenrypharm.com:6060/RegistrationInvoke.asmx", "RegistrationInvoke", "UploadRegImage", p).ToString();
            ////doc.LoadXml(result);
            ////doc.Save(@"D:\Work\yaodaibao\Release\result.xml");

        }
        public static byte[] GetBytesByPath(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((int)fs.Length);
            fs.Flush();
            fs.Close();
            return bytes;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime date1, date2;

            iTR.Lib.Common.CalcWeekDay(2018,46,out date1,out date2);

        }

        private void Json2XML_Click(object sender, EventArgs e)
        {
            //string JsonString ="{\"AuthCode\": \"1d340262-52e0-413f-b0e7-fc6efadc2ee5\",\"LoginName\": \"\",\"Mobile\": \"13811002288\",\"Password\": \"123456\"}";
            string JsonString = "{\"front_formmain\":{\"emptyLineFields\":[],\"tableType\":\"master\",\"frontTableName\":\"front_formmain\",\"display\":\"主表字段\",\"emptyLineRule\":\"0\",\"id\":\"2800006078892623399\",\"fieldInfo\":[{\"isPrimary\":\"true\",\"display\":\"ID\",\"isNull\":\"true\",\"name\":\"id\",\"className\":\"\",\"id\":\"7529200171560812954\",\"type\":\"text\",\"barcode\":\"\",\"fieldLength\":\"20\"},{\"isPrimary\":\"false\",\"display\":\"审核状态\",\"isNull\":\"true\",\"name\":\"state\",\"className\":\"\",\"id\":\"1886531005241249020\",\"type\":\"text\",\"barcode\":\"\",\"fieldLength\":\"10\"},{\"isPrimary\":\"false\",\"display\":\"创建人\",\"isNull\":\"true\",\"name\":\"start_member_id\",\"className\":\"\",\"id\":\"3944685295939505204\",\"type\":\"member\",\"barcode\":\"\",\"fieldLength\":\"20\"},{\"isPrimary\":\"false\",\"display\":\"创建时间\",\"isNull\":\"true\",\"name\":\"start_date\",\"className\":\"\",\"id\":\"5390069248916865585\",\"type\":\"datetime\",\"barcode\":\"\",\"fieldLength\":\"\"},{\"isPrimary\":\"false\",\"display\":\"审核人\",\"isNull\":\"true\",\"name\":\"approve_member_id\",\"className\":\"\",\"id\":\"5140458333966256814\",\"type\":\"member\",\"barcode\":\"\",\"fieldLength\":\"20\"},{\"isPrimary\":\"false\",\"display\":\"审核时间\",\"isNull\":\"true\",\"name\":\"approve_date\",\"className\":\"\",\"id\":\"8614768763615920439\",\"type\":\"datetime\",\"barcode\":\"\",\"fieldLength\":\"\"},{\"isPrimary\":\"false\",\"display\":\"流程状态\",\"isNull\":\"true\",\"name\":\"finishedflag\",\"className\":\"\",\"id\":\"1796685994957751374\",\"type\":\"text\",\"barcode\":\"\",\"fieldLength\":\"10\"},{\"isPrimary\":\"false\",\"display\":\"核定状态\",\"isNull\":\"true\",\"name\":\"ratifyflag\",\"className\":\"\",\"id\":\"-4631810656349960856\",\"type\":\"text\",\"barcode\":\"\",\"fieldLength\":\"10\"},{\"isPrimary\":\"false\",\"display\":\"核定人\",\"isNull\":\"true\",\"name\":\"ratify_member_id\",\"className\":\"\",\"id\":\"-7038870983961982514\",\"type\":\"member\",\"barcode\":\"\",\"fieldLength\":\"20\"},{\"isPrimary\":\"false\",\"display\":\"核定时间\",\"isNull\":\"true\",\"name\":\"ratify_date\",\"className\":\"\",\"id\":\"-7715976529746508326\",\"type\":\"datetime\",\"barcode\":\"\",\"fieldLength\":\"\"},{\"isPrimary\":\"false\",\"display\":\"修改人\",\"isNull\":\"true\",\"name\":\"modify_member_id\",\"className\":\"\",\"id\":\"-5043880160048562044\",\"type\":\"member\",\"barcode\":\"\",\"fieldLength\":\"20\"},{\"isPrimary\":\"false\",\"display\":\"修改时间\",\"isNull\":\"true\",\"name\":\"modify_date\",\"className\":\"\",\"id\":\"-64581915925856095\",\"type\":\"datetime\",\"barcode\":\"\",\"fieldLength\":\"\"},{\"display\":\"制单人\",\"name\":\"field0001\",\"id\":\"-2415308216442335940\",\"type\":\"member\",\"formatType\":\"\",\"fieldType\":\"VARCHAR\",\"placeHolder\":\"\",\"fieldLength\":\"20,0\",\"desc\":\"\"},{\"display\":\"工号\",\"name\":\"field0002\",\"relationId\":\"7429934805363606249\",\"id\":\"2049119641211653068\",\"type\":\"text\",\"formatType\":\"\",\"fieldType\":\"VARCHAR\",\"placeHolder\":\"\",\"fieldLength\":\"100,0\",\"desc\":\"\"},{\"display\":\"所在部门\",\"name\":\"field0003\",\"relationId\":\"3857801848575181222\",\"id\":\"6890335549461057066\",\"type\":\"department\",\"formatType\":\"\",\"fieldType\":\"VARCHAR\",\"placeHolder\":\"\",\"fieldLength\":\"20,0\",\"desc\":\"\"},{\"display\":\"公司\",\"name\":\"field0004\",\"id\":\"3281351669587920870\",\"type\":\"text\",\"formatType\":\"\",\"fieldType\":\"VARCHAR\",\"placeHolder\":\"\",\"fieldLength\":\"100,0\",\"desc\":\"\"},{\"display\":\"制单日期\",\"name\":\"field0005\",\"id\":\"9037746017693025527\",\"type\":\"date\",\"formatType\":\"\",\"fieldType\":\"TIMESTAMP\",\"placeHolder\":\"\",\"fieldLength\":\"255,0\",\"desc\":\"\"}],\"tableName\":\"formmain_8027\",\"ownerTable\":\"\"},\"formsons\":[{\"emptyLineFields\":[],\"tableType\":\"slave\",\"frontTableName\":\"front_formson_1\",\"display\":\"明细表1\",\"emptyLineRule\":\"0\",\"id\":\"-2722395116129857938\",\"fieldInfo\":[{\"display\":\"物料编码\",\"name\":\"field0006\",\"id\":\"-1073399896670189852\",\"type\":\"text\",\"formatType\":\"\",\"fieldType\":\"VARCHAR\",\"placeHolder\":\"\",\"fieldLength\":\"100,0\",\"desc\":\"\"}],\"tableName\":\"formson_8028\",\"ownerTable\":\"formmain_8027\"}]}";
            string xmlString = iTR.Lib.Common.Json2XML(JsonString, "Login");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string xmlString = "";//"<?xml version=\"1.0\" encoding=\"utf-8\"?><UpdateScheduleData><Result>True</Result><ID>6ac81139-61df-428c-9204-ef9aefbee764</ID><Description></Description></UpdateScheduleData>";
                                  //xmlString = @"<UpdateHospitalStock>
                                  //                         <AuthCode>1d340262-52e0-413f-b0e7-fc6efadc2ee5</AuthCode>
                                  //                         <FID>-1</FID>
                                  //                         <FDate>2020-07-28</FDate>
                                  //                         <FWeekIndex>0</FWeekIndex>
                                  //                         <FHospital>医院名称</FHospital>
                                  //                         <FHospitalID>医院代码</FHospitalID>
                                  //                         <FEmployeeID>登陆者ID</FEmployeeID>
                                  //                         <DataRows>
                                  //                          <DataRow>
                                  //                           <FProductID>产品ID</FProductID>
                                  //                           <FStock_IB>1</FStock_IB>
                                  //                           <FStock_IN>2</FStock_IN>
                                  //                           <FStock_EB>3</FStock_EB>
                                  //                           <FSaleAmount>4</FSaleAmount>
                                  //                          </DataRow>
                                  //                          <DataRow>
                                  //                           <FProductID>产品ID2</FProductID>
                                  //                           <FStock_IB>10</FStock_IB>
                                  //                           <FStock_IN>20</FStock_IN>
                                  //                           <FStock_EB>30</FStock_EB>
                                  //                           <FSaleAmount>40</FSaleAmount>
                                  //                          </DataRow>
                                  //                         </DataRows>
                                  //                        </UpdateHospitalStock>";
            xmlString = tbInput.Text;
            XmlDocument doc = new XmlDocument();
            doc.Load(tbInput.Text);
           
            string jsonString = iTR.Lib.Common.XML2Json(xmlString, "UpdateHospitalStock");
            tbOutput.Text = jsonString;
        }


        // public static string Post(string url, object obj = null, int timeoutSeconds = 15)
        //{


        //    if (string.IsNullOrWhiteSpace(url))
        //        return "";

        //    Encoding encode = Encoding.UTF8;
        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
        //    myRequest.ContentType = "application/json;charset=utf-8";
        //    myRequest.Method = "POST";// HttpUtil.UrlMethod.POST.ToString();
        //    if (timeoutSeconds < 3)
        //        timeoutSeconds = 3;

        //    byte[] bs = null;
        //    myRequest.Timeout = timeoutSeconds * 1000;
        //    if (obj != null)
        //    {
        //        bs = encode.GetBytes(JsonConvert.SerializeObject(obj, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }));

        //        myRequest.ContentLength = bs.Length;
        //    }
        //    else
        //        myRequest.ContentLength = 0;


        //    using (Stream reqStream = myRequest.GetRequestStream())
        //    {
        //        if (obj != null)
        //            reqStream.Write(bs, 0, bs.Length);
        //        reqStream.Close();
        //    }
        //    try
        //    {
        //        using (HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse())
        //        {
        //            using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding33))
        //            {
        //                var responseData = reader.ReadToEnd().ToString();
        //                return responseData;
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
