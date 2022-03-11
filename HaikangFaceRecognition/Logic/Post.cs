using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeploymentTools.Logic
{
    class Post
    {

        //post登入数据
        public static String HttpPost(String Url, String postDataStr, out Boolean isSuccess)
        {
            isSuccess = true;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                //request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch (Exception e)
            {
                isSuccess = false;
                Console.Write(e.Message);
                return e.Message;
            }
        }
        /// <summary>
        /// POST整个字符串到URL地址中
        /// </summary>
        /// <param name="Url">Post链接地址</param>
        /// <param name="jsonParas">JSON数据字符串</param>
        /// <param name="TimeStr">当前系统时间(GMT格式)</param>
        /// <param name="EncryptionKey">签名</param>
        /// <param name="AK">APPkey(AK秘钥)</param>
        /// <param name="isSuccess">方法是否正确拿到数据</param>
        /// <returns></returns>
        public static String HttpPostRaw(String Url, String jsonParas, String TimeStr, String EncryptionKey, String AK, ref Boolean isSuccess)
        { 
            string strURL = Url;
            //创建一个HTTP请求 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //声明协议
            // 准备请求...
            // 设置参数
            if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //处理Https请求
                ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(Url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            CookieContainer cookieContainer = new CookieContainer();
            //设置四秒超时
            //request.Timeout = 10000; //单位毫秒
            //Post请求方式 
            request.Method = "POST";
            //内容类型
            request.Headers.Add("X-Ca-Signature", EncryptionKey);//秘钥
            request.Headers.Add("X-Ca-Key", AK);//AppKey
            request.Accept = "*/*";
            request.Date = Convert.ToDateTime(TimeStr);
            request.ContentType = "application/json";
            request.Headers.Add("X-Ca-Signature-Headers", "x-ca-key");
            //设置参数，并进行URL编码
            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);  
            byte[] payload;
            //将Json字符串转化为字节 
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength  
            request.ContentLength = payload.Length;
            //发送请求，获得请求流
            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                Console.Write("连接服务器失败!");
            }
            try
            {
                //将请求参数写入流
                writer.Write(payload, 0, payload.Length);
                writer.Close();//关闭请求流
                               //String strValue = "";//strValue为http响应所返回的字符流
                HttpWebResponse response;
                try
                {
                    //获得响应流
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    try
                    {
                        isSuccess = false;
                        response = ex.Response as HttpWebResponse;
                    }
                    catch (Exception exs)
                    {
                        return exs.Message;//错误
                    }

                }
                Stream s = response.GetResponseStream();
                StreamReader sRead = new StreamReader(s);
                string postContent = sRead.ReadToEnd();
                sRead.Close();
                Log.WriteLog("Post返回", postContent+ "|Url:" + Url + "txt:" + jsonParas);
                return postContent;//返回Json数据
            }
            catch (Exception ex)
            {
                isSuccess = false;
                return "";//错误
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public static String GETHttpRaw(String Url, String jsonParas, out Boolean isSuccess)
        {
            try
            {
                isSuccess = true;
                var client = new RestClient(Url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", jsonParas, ParameterType.RequestBody);
                if (Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    client.RemoteCertificateValidationCallback =
                    (sender, certificate, chain, sslPolicyErrors) => true;
                }
                IRestResponse response = client.Execute(request);
                if (String.IsNullOrEmpty(response.Content))
                    isSuccess = false;
                return response.Content;
            }
            catch (Exception)
            {
                isSuccess = false;
                return null;
            }
           

        }
       

    }
    class Get {
        /// <summary>
        /// get
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr = "")
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception)
            {

                return null;
            }

           
        }
        public static String GetHttp(String Url)
        {
            try
            {
                var client = new RestClient(Url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                return response.Content;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
