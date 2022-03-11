using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace LntegratedMiddleware.CQLJ
{
    class alarmSubscribe
    {
        public alarmSubscribe()
        {

            #region 获取注册名
            String url = "http://172.16.2.222:82/register";
            String data = "{\r\n    \"callbackAddress\": \"http://172.16.2.77:8082/CQLJ/CQAlarmDataCS\",\r\n    \"hasExpir\": 0,\r\n    \"memo\": \"杭州图洋\",\r\n    \"name\": \"学院\"\r\n}";

            JObject msg = Post(url, data);
            Console.WriteLine("注册数据："+JsonConvert.SerializeObject(msg));
            #endregion

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
            .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
            IConfiguration configuration = builder.Build();

            String[] subscribelist = configuration["DH:SubscribeList"].ToString().Split(',');
            //zhongqinglujunxueyuan
            String suburl = "http://172.16.2.222:82/register/subscribe/xueyuan/";//register/subscribe //register/cancellation 取消订阅//lujungongchengxueyuan
            //String suburl = "http://172.16.2.222:82/register/cancellation/lujungongchengxueyuan/";//register/subscribe //register/cancellation 取消订阅//zhongqinglujunxueyuan

            Int32 i = 0;
            foreach (var item in subscribelist)
            {
                if (Int32.Parse(GetSubscribe(suburl + item)["code"].ToString()) != 200)
                {
                    Console.WriteLine("事件码订阅失败：" + item);
                }
            }
            Console.WriteLine(i < 1 ? "订阅成功" : "");
        }


        public static void speedAlarm()
        {
            new Thread(() =>
            {
                Thread.Sleep(30000);
               // String url = $"http://172.16.2.222:8314/portal/rest/alarm/getPage?userId=1&userName=system&token=7d7130856060e18c512bc9c8eb1500f1&pageNum=1&pageSize=400&AlarmType=1&captureType=1,2&startTime=2021-09-13 10:59:00&endTime=2021-09-13 13:59:00";
                String Url = "https://172.16.2.222/portal/rest/alarm/getPage?pageSize=10&pageNo=1&startTime=2021-09-01 00:59:00&endTime=2021-09-03 10:59:00&captureType=1,2&AlarmType=3";

                JObject data= GetSubscribe(Url);

            });
        }
        public static void Addalarm(JToken rbs,String type)
        {
            var Object = new
            {
                //eventTypeName= eventTypeName,

                event_id = "",
                event_type = rbs["params"]["events"][0]["eventType"].ToString(),
                status = 0,
                start_time = rbs["params"]["events"][0]["happenTime"].ToString(),
                stop_time = rbs["params"]["events"][0]["happenTime"].ToString(),
                event_name = rbs["params"]["events"][0]["data"]["eventName"].ToString(),
                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                device_name = rbs["params"]["events"][0]["srcName"].ToString(),

                //eventTypeName = rbs["params"]["events"][0]["data"]["eventTypeName"].ToString(),
            };
            String data = null;
            switch (type)
            {
                case "mq":
                    data = JsonConvert.SerializeObject(new
                    {
                        event_id = Guid.NewGuid().ToString(),
                        event_type = rbs["interfaceCode"].ToString(),
                        status = 0,
                        start_time = rbs["data"]["detectTime"].ToString(),
                        stop_time = rbs["data"]["detectTime"].ToString(),
                        event_name ="黑名单报警",
                        device_code = rbs["data"]["channelCode"].ToString(),
                        device_name = rbs["data"]["channelName"].ToString(),

                    });
                    break;
                case "tp":
                    data = JsonConvert.SerializeObject(new
                    {
                        event_id = Guid.NewGuid().ToString(),
                        event_type = "SpeedAlarm",
                        status = 0,
                        start_time = rbs["data"]["pageData"][0]["alarmTime"].ToString(),
                        stop_time = rbs["data"]["pageData"][0]["alarmTime"].ToString(),
                        event_name = "车辆超速",
                        device_code = rbs["data"]["pageData"][0]["alarmTime"].ToString(),
                        device_name = rbs["data"]["pageData"][0]["alarmTime"].ToString()

                    });
                    break;
                default:
                    break;
            }

            Post("http://10.0.168.200:8090/backoffice/event/info/add", JsonConvert.SerializeObject(Object));
           

        }


        public static JObject Post(String url, String data)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            if (data != "")
                request.AddParameter("application/json", data, ParameterType.RequestBody);
            else
                request.AddParameter("application/json", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return JObject.Parse(response.Content);
        }

        public static JObject GetSubscribe(String url)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                client.RemoteCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
            }
            IRestResponse response = client.Execute(request);
            return JObject.Parse(response.Content);
        }


        

    }


}

