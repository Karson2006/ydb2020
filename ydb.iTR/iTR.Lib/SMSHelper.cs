using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;


using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;

using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System.Web.Services.Discovery;

namespace iTR.Lib
{
    public class AliDayuSMS
    {

        static String product = "Dysmsapi";//短信API产品名称
        static String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
        private  String accessId = "";
        private String accessSecret = "";
       
        static String regionIdForPop = "cn-hangzhou";

        public AliDayuSMS()
        {
            //获取SMS敏感参数
            iTR.Lib.Common comm = new Common();
            Dictionary<string, string> info = comm.GetSysInfo("SMS");
            accessId = info["accessId"].ToString();
            accessSecret = info["accessSecret"].ToString();

        }


        public  string SendSms(string code, string mobile, string templateID = "SMS_140731928")
        {

            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            string result = "0";

            try
            {
                //request.SignName = "上云预发测试";//"管理控制台中配置的短信签名（状态必须是验证通过）"
                //request.TemplateCode = "SMS_71130001";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
                //request.RecNum = "13567939485";//"接收号码，多个号码可以逗号分隔"
                //request.ParamString = "{\"name\":\"123\"}";//短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
                //SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
                request.PhoneNumbers = mobile;
                request.SignName = "药代宝";
                request.TemplateCode = templateID;
                //request.TemplateParam = "{\"code\":\"123\"}";
                request.TemplateParam = "{\"code\":\""+code+"\"}";
                request.OutId = "";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if (sendSmsResponse.Code == "OK")//发送成功
                {
                    result = "1";
                }

                return result;

            }
            catch (ServerException e)
            {
                throw e;
            }
            catch (ClientException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 阿里平台发短信方法
        /// </summary>
        /// <param name="mobile">接收短信手机号码</param>
        /// <param name="templateID">阿里短信平台短信模板号</param>
        /// <param name="SignName">阿里短信平台短信签名</param>
        /// <param name="templateParam">短信内容Json格式，参数在短信模板中定义</param>
        /// <returns></returns>
        public  string ALISendSMS(string mobile, string templateID, string SignName, string templateParam)
        {
            IClientProfile profile = DefaultProfile.GetProfile(regionIdForPop, accessId, accessSecret);
            DefaultProfile.AddEndpoint(regionIdForPop, regionIdForPop, product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            string result = "0";
            try
            {

                request.PhoneNumbers = mobile;
                //request.SignName = "";
                request.SignName = SignName;
                request.TemplateCode = templateID;
                request.TemplateParam = templateParam;
               
                request.OutId = "";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                if (sendSmsResponse.Code == "OK")//发送成功
                {
                    result = "1";
                }
                else
                {
                    result = sendSmsResponse.Code;
                }
                return result;
            }
            catch (ServerException e)
            {
                throw e;
            }
            catch (ClientException e)
            {
                throw e;
            }
        }

       
    }
}
