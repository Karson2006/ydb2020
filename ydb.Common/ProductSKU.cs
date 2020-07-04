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
    public class ProductSKU
    {
        Items iClass;
        public ProductSKU()
        {
            iClass = new Items();
           
        }

        #region GetList
        public string GetList(string xmlString)
        {
            string result = "", nodeString = "GetSKUList", filter = "", val = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlNode vNode = doc.SelectSingleNode(nodeString + "/ProductID");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t1.FProductID='" + val + "'" : "t1.FProductID='" + val + "'";
                }
                vNode = doc.SelectSingleNode(nodeString + "/ProductName");
                if (vNode != null && vNode.InnerText.Trim().Length > 0)
                {
                    val = vNode.InnerText.Trim();
                    filter = filter.Length > 0 ? filter = filter + " And t2.FName = '" + val + "'" : "t2.FName = '" + val + "'";
                }



                string sql = "Select t1.FID,t1.FProductID,t1.FSKU,t1.FConten,t1.FRemark,t2.FName As FProductName,t1.FWrappage " +
                            " From  ProductSKU t1" +
                            " Left Join t_Items t2 On t1.FProductID= t2.FID";

                if (filter.Length > 0)
                    sql = sql + " Where " + filter;
                SQLServerHelper runner = new SQLServerHelper();

                DataTable dt = runner.ExecuteSql(sql);

                result = Common.DataTableToXml(dt, "GetSKUList", "", "List");
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
