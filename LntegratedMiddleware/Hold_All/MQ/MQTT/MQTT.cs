using LntegratedMiddleware.HIK.C;
using LntegratedMiddleware.Hold_All.TCP;
using LntegratedMiddleware.SqlData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Core.Client;
using MQTTnet.Core.Packets;
using MQTTnet.Core.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
namespace LntegratedMiddleware.Hold_All.MQTT
{
    class MQTT
    {
        private static MQTTKEY.MQSecretKey MQlink;//需要发送的mq集合
        PGDataProcessing PGdata = new PGDataProcessing();//pg数据库操作类
        /// <summary>
        /// 获取要发送的MQ 4.0
        /// </summary>
        /// <param name="error"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        private JToken GetMQJson(out Boolean error)
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
          .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("Event_Type");
            List<MyConfiguration.m> c = new List<MyConfiguration.m>();
            foreach (IConfigurationSection section in a.GetChildren())
            {
                c.Add(
                        new MyConfiguration.m
                        {
                            Event_a = section.GetValue<string>("Event_TypeName"),
                            Event_b = section.GetValue<string>("Event_Code")
                        }
                    );
                Log.Debug($"缓冲：{section.GetValue<string>("Event_TypeName")}  {section.GetValue<string>("Event_Code")}");

            }
            List<long> MQs = new List<long>();
            foreach (var item in c)
            {
                MQs.Add(long.Parse(item.Event_b));
            }
            var eventTypes = new
            {
                a = new { eventTypes = MQs.ToArray() }

            };
            error = true;
            //List<long> MQs = new List<long>();
            //DataSet Dss = PGdata.ExecuteQuery($"SELECT plat_event_code,push_data FROM \"event_type\"", out error);
            //if (Dss.Tables.Count > 0 && Dss.Tables[0].Rows.Count > 0 && error)
            //{
            //    for (int i = 0; i < Dss.Tables[0].Rows.Count; i++)
            //    {
            //        if ((Boolean)Dss.Tables[0].Rows[i]["push_data"])
            //        {
            //            MQs.Add(long.Parse(Dss.Tables[0].Rows[i]["plat_event_code"].ToString()));

            //        }
            //    }
            //}
            //else
            //{
            //    Log.Debug($"SPCC初始化提示：数据库报错无法链接，或无表无字段");
            //}
            //var eventTypes = new
            //{
            //    a = new { eventTypes = MQs.ToArray() }

            //};

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(eventTypes))["a"];
        }

        public class MyConfiguration
        {
            public class m { 
                public String Event_a { get; set; }
                public String Event_b { get; set; }
            }
            public class k
            {
                public String[] eventTypes { get; set; }
            }
        }

        /// <summary>
        /// 获取要发送的MQ 5.0
        /// </summary>
        /// <param name="error"></param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        private JToken GetMQJsonV2(out Boolean error)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
            .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("Event_Type");
            List<MyConfiguration.m>  c = new List<MyConfiguration.m>();
            foreach (IConfigurationSection section in a.GetChildren())
            {
                c.Add(
                        new MyConfiguration.m
                        {
                            Event_a = section.GetValue<string>("Event_TypeName"),
                            Event_b = section.GetValue<string>("Event_Code")
                        }
                    );
                Log.Debug($"缓冲：{section.GetValue<string>("Event_TypeName")}  {section.GetValue<string>("Event_Code")}");

            }
            List<long> MQs = new List<long>();
            DataSet Dss = PGdata.ExecuteQuery($"SELECT id,push_data,event_name FROM \"event_type\"", out error);
            if (Dss.Tables.Count > 0 && Dss.Tables[0].Rows.Count > 0 && error)
            {           
                for (int i = 0; i < Dss.Tables[0].Rows.Count; i++)
                {
                    foreach (var item in c)
                    {
                        if (item.Event_a == (string)Dss.Tables[0].Rows[i]["id"]) {

                            if ((Boolean)Dss.Tables[0].Rows[i]["push_data"])
                            {
                                if (!String.IsNullOrEmpty(item.Event_b))
                                {
                                    Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                                    Console.BackgroundColor = ConsoleColor.Green; //设置背景色

                                    Log.Information($"{(String)Dss.Tables[0].Rows[i]["event_name"]}=>{item.Event_a}匹配成功 代码{item.Event_b}");

                                    Console.ResetColor(); //将控制台的前景色和背景色设为默认值
                                    MQs.Add(long.Parse(item.Event_b));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Log.Debug($"SPCC初始化提示：数据库报错无法链接，或无表无字段");
            }
            var eventTypes = new
            {
                a = new { eventTypes = MQs.ToArray() }

            };

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(eventTypes))["a"];
        }
        public MQTT()
        {
            Log.Debug($"Starting Haikang SPCC MQTT service");//正在启动MQTT服务
                                                                                      //Log.Debug($"Whether to start the SPCC mqtt service of Haikang (YES/NO):");
                                                                                      //String condition = Console.ReadLine().ToUpper();//接受控制台输入的一个字符串
            var builder = new ConfigurationBuilder()
                                         .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                                         .AddJsonFile("appsettings.json");
           
            //AppContext.BaseDirectory + "appsettings.json";
            MyConfiguration.k c = new MyConfiguration.k();
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("Event_Type");
           // GetMQJsonV2( out _);
            ArrayList eventTypes = new ArrayList();
            switch (configuration["Edition:VersionNo"])
            {
                case "4.0":

                    // var list = GetMQJsonV2(out _);
                    foreach (IConfigurationSection section in a.GetChildren())
                    {
                        eventTypes.Add(section.GetValue<string>("Event_Code"));
                    }
                    c.eventTypes = (String[])eventTypes.ToArray(typeof(String));
                   
                    JToken list = JToken.Parse(JsonConvert.SerializeObject(c));
                    //string aa = "{\"eventTypes\": [131588,786434,786436]}";
                    //JToken list = JToken.Parse(aa);
                    String GetData = new HIKoperation().HIKGETDATA(list, out _, 1, 0);
                    //String GetData = "{\"msg\":\"success\",\"code\":\"0\",\"data\":{\"host\":\"tcp://34.203.114.2:1883\",\"clientId\":\"29044094\",\"userName\":\"artemis_29044094_0RVC2BPF\",\"password\":\"89OUUJ90\",\"topicName\":{\"3187675137\":\"artemis/event_face/3187675137/admin\",\"3204452353\":\"artemis/event_veh/3204452353/admin\"}}}";
                    try
                    {
                        JObject rb = JsonConvert.DeserializeObject<JObject>(GetData);
                        //Console.WriteLine(rb.ToString());
                        if ((String)rb["msg"] == "success")
                        {
                            List<String> topicName = new List<String>();
                            foreach (var item in list["eventTypes"])
                            {
                                topicName.Add((String)rb["data"]["topicName"][item.ToString()]);
                            }
                            MQlink = new MQTTKEY.MQSecretKey
                            {
                                host = (String)rb["data"]["host"],
                                clientId = Guid.NewGuid().ToString("N"),//(String)rb["data"]["clientId"],
                                userName = (String)rb["data"]["userName"],
                                password = (String)rb["data"]["password"],
                                topicName = topicName.ToArray()
                            };
                            try
                            {
                                StartMQMonitoring();
                            }
                            catch (Exception ex)
                            {
                                Log.Debug($"超时无响应或参数错误{ex.Message}");
                            }
                        }
                        else
                        {
                           
                            MQlink = null;
                            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                            Console.BackgroundColor = ConsoleColor.Yellow; //设置背景色

                            Log.Debug($"SPCC接口报错，无法正常进行初始化，重启软件！无法连接SPCC服务器,如单模式请忽略。");

                            Console.ResetColor(); //将控制台的前景色和背景色设为默认值

                        }
                    }
                    catch (Exception ex)
                    {
                       
                        MQlink = null;
                        Log.Debug(ex.ToString());
                        Log.Debug($"SPCC接口报错，无法正常进行初始化，重启软件！");

                    }
                    break;
                case "5.0":
                     list = GetMQJson(out _);
                     GetData = new HIKoperation().HIKGETDATA(list, out _, 1, 0);
                    //String GetData = "{\"msg\":\"success\",\"code\":\"0\",\"data\":{\"host\":\"tcp://34.203.114.2:1883\",\"clientId\":\"29044094\",\"userName\":\"artemis_29044094_0RVC2BPF\",\"password\":\"89OUUJ90\",\"topicName\":{\"3187675137\":\"artemis/event_face/3187675137/admin\",\"3204452353\":\"artemis/event_veh/3204452353/admin\"}}}";
                    try
                    {
                        JObject rb = JsonConvert.DeserializeObject<JObject>(GetData);
                        Console.WriteLine(rb.ToString());
                        if ((String)rb["msg"] == "success")
                        {
                            List<String> topicName = new List<String>();
                            foreach (var item in list["eventTypes"])
                            {
                                topicName.Add((String)rb["data"]["topicName"][item.ToString()]);
                            }
                            MQlink = new MQTTKEY.MQSecretKey
                            {
                                host = (String)rb["data"]["host"],
                                clientId = Guid.NewGuid().ToString("N"),//(String)rb["data"]["clientId"],
                                userName = (String)rb["data"]["userName"],
                                password = (String)rb["data"]["password"],
                                topicName = topicName.ToArray()
                            };
                            try
                            {
                                StartMQMonitoring();
                            }
                            catch (Exception ex)
                            {
                                Log.Debug($"超时无响应或参数错误{ex.Message}");
                            }
                        }
                        else
                        {
                            MQlink = null;
                            Log.Debug($"SPCC接口报错，无法正常进行初始化，重启软件！");

                        }
                    }
                    catch (Exception)
                    {
                        MQlink = null;
                        Log.Debug($"SPCC接口报错，无法正常进行初始化，重启软件！");

                    }
                    break;
                default:
                    Log.Debug("无指定版本信息 请到appsettings.json配置");
                    break;
            }
            //if (!String.IsNullOrEmpty(condition) && condition == "YES" || condition == "NO")
            //    if (condition == "YES" ? !true : !false)
            //        return;
            //    else
            //        ;
            //else
            //    return;

            
        }
        MqttClient mqttClient;
        /// <summary>
        /// 开启mq通信通道
        /// </summary>
        private void StartMQMonitoring()
        {

            mqttClient = new MqttClientFactory().CreateMqttClient() as MqttClient;
            mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
            mqttClient.Connected += MqttClient_Connected;
            mqttClient.Disconnected += MqttClient_Disconnected;
            var options = new MqttClientTcpOptions
            {
                Server = MQlink.host.Replace("tcp://", "").Split(':')[0],
                Port = Int32.Parse(MQlink.host.Replace("tcp://", "").Split(':')[1]),
                ClientId = MQlink.clientId,
                UserName = MQlink.userName,
                Password = MQlink.password,
                CleanSession = true
            };
            mqttClient.ConnectAsync(options);
        }
        /// <summary>
        /// 服务器连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Connected(object sender, EventArgs e)
        {

            Log.Debug($"MQTT提示：MQTT链接成功，准备转发中");

            foreach (var item in MQlink.topicName)//连接MQTT后订阅主题接收信息
            {
                SubscriptionTheme(item);
            }

        }
        /// <summary>
        /// 断开服务器连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Disconnected(object sender, EventArgs e)
        {

            Log.Debug($"已断开MQTT连接！自动重连中...");
            Thread.Sleep(3000);
            StartMQMonitoring();//断开重新连接 并且重新主城订阅
        }
        /// <summary>
        /// MQTT订阅主题
        /// </summary>
        /// <param name="topicname">订阅主题</param>
        private void SubscriptionTheme(String topicname)
        {
            string topic = topicname.Trim();


            if (string.IsNullOrEmpty(topic))
            {
                Log.Debug($"MQTT提示：订阅主题为空！");
                return;
            }

            if (!mqttClient.IsConnected)
            {
                Log.Debug($"MQTT报错：MQTT通信未开启，因此无法订阅！");
                return;
            }

            mqttClient.SubscribeAsync(new List<TopicFilter> {
                new TopicFilter(topic, MqttQualityOfServiceLevel.AtMostOnce)
            });

            Log.Debug($"MQTT提示：已订阅[{topic}]主题");


        }
        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {

            String msg = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine(msg);
            //发送信息
            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.DarkGray; //设置背景色
            Log.Debug($"SPCCMQTT：Msg[{msg}]");
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                             .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
            IConfiguration configuration = builder.Build();
            switch (configuration["Edition:VersionNo"])
            {
                case "5.0":
                    PostMsg.SetPostMsgSPCC(msg);
                    break;
                case "4.0":
                    SocketMsg.WebSocketMsg(msg);
                    break;
                default:
                    SocketMsg.WebSocketMsg(msg);
                    PostMsg.SetPostMsgSPCC(msg);
                    break;
            }
        
        }

    }
    public class GetToTime
    {
        public static String DealTimeFormat(String oldDateStr)
        {
            if (!String.IsNullOrEmpty(oldDateStr))
            {
                return Convert.ToDateTime(oldDateStr).ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
