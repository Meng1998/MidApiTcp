using LntegratedMiddleware.HIK.M;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LntegratedMiddleware.CQLJ
{
    class authentication
    {
        private JObject Msg;
        public authentication()
        {
            //String url = "http://172.16.2.222:8314/WPMS/getPublicKey";
            
            String data = "{\r\n    \"loginName\": \"system\"\r\n}";
            String pwd = "system123";
            //String PublicKey = alarm.Post(url, data)["publicKey"].ToString();
            //String loginPass= RSAEncrypt(PublicKey, pwd);

            String basekey = alarmSubscribe.Post("http://172.16.2.222:8314/WPMS/getCryptKey", data)["publicKey"].ToString();
            String basepass = EncodeBase64(pwd+ basekey);

            //String logurl = "http://172.16.2.222:8314/WPMS/login";
            String logdata = JsonConvert.SerializeObject(new
            {
                loginName = "system",
                loginPass = basepass
            });


            Msg = alarmSubscribe.Post("http://172.16.2.222:8314/WPMS/apiLogin", logdata);
            //String msg = alarm.Post(logurl, logdata).ToString();
        }

        public String DHpostRespan(Int32 Index,String data,Int32 restype)
        {
            DHSecretKey key = DHDataSet.GetDHkey(Index, 0);
            String url = $"http://{key.Host}:{key.Port}{key.API}?userId={Msg["id"]}&userName={Msg["loginName"]}&token={Msg["token"]}";
            JObject msg = null;
            switch (restype)
            {
                case 1:
                    msg = alarmSubscribe.Post(url, data);
                    break;
                case 2:
                    foreach (var item in JObject.Parse(data))
                    {
                        url += $"&{item.Key}={item.Value}";
                    }
                    msg = alarmSubscribe.GetSubscribe(url);
                    if (Index==6)
                    {
                        foreach (var item in msg["data"]["pageData"])
                        {
                            //item["carImgUrl"] = item["carImgUrl"].ToString().Insert(4, "s");
                            item["carImgUrl"] = System.Web.HttpUtility.UrlDecode(item["carImgUrl"].ToString().Split('=')[1]);
                            //"http%3A%2F%2F172.16.2.222%3A8081%2Fd%2F1000148%241%240%240%2F20210915%2F10%2F4320-528223-0.jpg"
                           // String addurl = System.Web.HttpUtility.UrlDecode("http%3A%2F%2F172.16.2.222%3A8081%2Fd%2F1000148%241%240%240%2F20210915%2F10%2F4320-528223-0.jpg");

                        }
                    }
                    break; 
                case 3:
                    url += "pageNo=1&pageSize=1000&sortName=cap_date&sortOrder=desc&searchBean.dateTypeFlg=0&searchBean.removeFlg=0&searchBean.carNumPlace=&searchBean.carNumPlace_selectText=全部&searchBean.carNum=&searchBean.vagueFlg=0&searchBean.highQueryFlg=0";
                    foreach (var item in JObject.Parse(data))
                    {
                        url += $"&searchBean.{item.Key}={item.Value}";
                    }
                    msg = alarmSubscribe.Post(url,"");
                    break;
                default:
                    break;
            }
            return JsonConvert.SerializeObject(msg);

        }

        public String DHgetRespan(Int32 Index, String data)
        {
            DHSecretKey key = DHDataSet.GetDHkey(Index, 0);
            String url = $"http://{key.Host}:{key.Port}{key.API}?userId={Msg["id"]}&userName={Msg["loginName"]}&token={Msg["token"]}";

            return JsonConvert.SerializeObject(alarmSubscribe.Post(url, data));

        }



        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            publickey = @"<RSAKeyValue><Modulus>"+ publickey + "</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(String source)
        {
            String encode = "";
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
    }
}
