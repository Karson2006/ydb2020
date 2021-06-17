using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Reflection;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Configuration;


namespace iTR.Lib
{
    public class WebInvoke
    {
        //重载Invoke原来四个参数的方法，确保版本兼容性
        //Modifed by karson 2015年11月1日
        //public object Invoke(String urlFull, String className, String methodName, object[] parm)
        //{
        //   return  Invoke(urlFull, className, methodName, parm, null);
        //}

        private object InnerInvoke(String urlFull, String className, String methodName, object[] parm, SoapHeader soapHeader = null)
        {
            object result = null;

            try
            {
                //防止url中未加入?wsdl导致调用出错
                if (!Regex.IsMatch(urlFull, "\\S*?(?i)(wsdl)$"))
                {
                    urlFull += "?wsdl";
                }
                // 1. 使用 WebClient 下载 WSDL 信息。
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(urlFull);
               
                // 2. 创建和格式化 WSDL 文档。
                ServiceDescription description = ServiceDescription.Read(stream);

                // 3. 创建客户端代理代理类。
                ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
                importer.ProtocolName = "Soap"; // 指定访问协议。
                importer.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
                importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                importer.AddServiceDescription(description, null, null); // 添加 WSDL 文档。

                // 4. 使用 CodeDom 编译客户端代理类。
                CodeNamespace nmspace = new CodeNamespace(); // 为代理类添加命名空间，缺省为全局空间。
                CodeCompileUnit unit = new CodeCompileUnit();
                unit.Namespaces.Add(nmspace);

                ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                
                CompilerParameters parameter = new CompilerParameters();
                parameter.GenerateExecutable = false;
                parameter.GenerateInMemory = true;
                parameter.ReferencedAssemblies.Add("System.dll");
                parameter.ReferencedAssemblies.Add("System.XML.dll");
                parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                parameter.ReferencedAssemblies.Add("System.Data.dll");

                CompilerResults compiler = provider.CompileAssemblyFromDom(parameter, unit);

                // 5. 使用 Reflection 调用 WebService。
                if (!compiler.Errors.HasErrors)
                {
                    Assembly asm = compiler.CompiledAssembly;
                    Type t = asm.GetType(className); // 如果在前面为代理类添加了命名空间，此处需要将命名空间添加到类型前面。

                    object clientkey = null;
                    PropertyInfo propertyInfo = null;
                    if (soapHeader != null)
                    {
                        //Soap头开始     
                        propertyInfo = t.GetProperty(soapHeader.ClassName + "Value");

                        //获取客户端验证对象     
                        Type type = asm.GetType(soapHeader.ClassName);

                        //为验证对象赋值     
                        clientkey = Activator.CreateInstance(type);

                        foreach (KeyValuePair<string, object> property in soapHeader.Properties)
                        {
                            type.GetProperty(property.Key).SetValue(clientkey, property.Value, null);
                        }
                    }

                    object instance = Activator.CreateInstance(t);

                    if (clientkey != null)
                    {
                        propertyInfo.SetValue(instance, clientkey, null);
                    }
                    MethodInfo method = t.GetMethod(methodName);

                    result = method.Invoke(instance, parm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        //带超时时间的动态调用WS
        public object Invoke(String urlFull, String className, String methodName, object[] parm,SoapHeader soapHeader = null, int millionSecond = 0)
        {
            Thread threadToKill = null;
            Func<String, String, String, object[], SoapHeader, object> invokeFunc =
                (urlstr, classstr, methodstr, parms, header) =>
                {
                    threadToKill = Thread.CurrentThread;
                    return InnerInvoke(urlstr, classstr, methodstr, parms, header);
                };
            DateTime startTime = DateTime.Now; 
            IAsyncResult result = invokeFunc.BeginInvoke(urlFull, className, methodName, parm, soapHeader, null, null);

            //WsDelegate webDelegate = new WsDelegate(InnerInvoke);
            //IAsyncResult result = webDelegate.BeginInvoke(urlFull, className, methodName, parm, soapHeader, null, null);
            int status = 0;//成功
            //int timeout = millionSecond > 0 ? millionSecond : Convert.ToInt16(ConfigurationManager.AppSettings["TimeOut"] ?? "15") * 1000;
            int timeout = 15 * 1000;
            if (!result.AsyncWaitHandle.WaitOne(timeout))
            {
                threadToKill.Abort();
                status = 1;//失败
                throw (new TimeoutException("执行WebService超时", null));
            }
            
            object invokeResult = invokeFunc.EndInvoke(result);

          

            return invokeResult;
        }

        /// <summary>
        /// 以Post方式访问Http，并获得JSon字符串
        /// </summary>
        /// <param name="url">URL，含Http</param>
        /// <param name="josn">Json字符串</param>
        /// <param name="timeoutSeconds">访问超时时间，秒默认8秒</param>
        /// <returns>JSon字符串</returns>
        public static string PostJson(string url, string josn, int timeoutSeconds = 8)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //声明一个HttpWebRequest请求

            request.Timeout = timeoutSeconds * 1000;
            //转发机制
            Encoding encoding = Encoding.UTF8;
            Stream streamrequest = request.GetRequestStream();
            StreamWriter streamWriter = new StreamWriter(streamrequest, encoding);
            streamWriter.Write(josn);
            streamWriter.Flush();
            streamWriter.Close();
            streamrequest.Close();

            //设置连接超时时间
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamresponse = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamresponse, encoding);
            string result = streamReader.ReadToEnd();
            streamresponse.Close();
            streamReader.Close();

            return result;
        }
    }

    

    public delegate object WsDelegate(String urlFull, String className, String methodName, object[] parm, SoapHeader soapHeader);

    /// <summary>  
    /// SOAP头  
    /// </summary>  
    public class SoapHeader
    {
        /// <summary>  
        /// 构造一个SOAP头  
        /// </summary>  
        public SoapHeader()
        {
            this.Properties = new Dictionary<string, object>();
        
        }

        /// <summary>  
        /// 构造一个SOAP头  
        /// </summary>  
        /// <param name="className">SOAP头的类名</param>  
        public SoapHeader(string className)
        {
            this.ClassName = className;
            this.Properties = new Dictionary<string, object>();
        }

        /// <summary>  
        /// 构造一个SOAP头  
        /// </summary>  
        /// <param name="className">SOAP头的类名</param>  
        /// <param name="properties">SOAP头的类属性名及属性值</param>  
        public SoapHeader(string className, Dictionary<string, object> properties)
        {
            this.ClassName = className;
            this.Properties = properties;
        }

        /// <summary>  
        /// SOAP头的类名  
        /// </summary>  
        public string ClassName { get; set; }

        /// <summary>  
        /// SOAP头的类属性名及属性值  
        /// </summary>  
        public Dictionary<string, object> Properties { get; set; }

        /// <summary>  
        /// 为SOAP头增加一个属性及值  
        /// </summary>  
        /// <param name="name">SOAP头的类属性名</param>  
        /// <param name="value">SOAP头的类属性值</param>  
        public void AddProperty(string name, object value)
        {
            if (this.Properties == null)
            {
                this.Properties = new Dictionary<string, object>();
            }
            if (!Properties.ContainsKey(name))
            {
                Properties.Add(name, value);
            }
        }
    }  
}
