using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using D;
using H;
using static H.H;
using LntegratedMiddleware.SqlData;
using System.Data;
using System.Linq;
using static LntegratedMiddleware.Hold_All.MQTT.MQTT;
using System.Threading.Tasks;
using Serilog;

namespace LntegratedMiddleware.dahua
{

    public class DahuaInit
    {
        Backup Backup = new Backup();
        public void Init()
        {
            Console.WriteLine(Backup.init());//初始化


            H.H.Login_Info_t loginInfo = new H.H.Login_Info_t();

            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
           .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
            IConfiguration configuration = builder.Build();

            loginInfo.szIp = configuration["DH:szIp"].ToString();//"10.35.92.116";
            loginInfo.nPort = (uint)int.Parse(configuration["DH:nPort"]);
            loginInfo.szUsername = configuration["DH:szUsername"].ToString();
            loginInfo.szPassword = configuration["DH:szPassword"].ToString();
            loginInfo.nProtocol = H.H.dpsdk_protocol_version_e.DPSDK_PROTOCOL_VERSION_II;
            loginInfo.iType = 1;

            Console.WriteLine(Backup.Login(loginInfo));//登录




            H.H.nFun = AlarmCallback;

            //开启报警监听
            IntPtr pUser = default(IntPtr);
            IntPtr result = DPSDK_SetDPSDKAlarmCallback(nPDLLHandle, H.H.nFun, pUser);
            if (result == (IntPtr)0) { Console.WriteLine("设置报警回调成功"); }
            else { Console.WriteLine("设置报警回调失败，错误码：" + result.ToString()); }


            IntPtr res = H.H.DPSDK_LoadDGroupInfo(nPDLLHandle, ref nGroupLen, (IntPtr)60000);
            if (res == (IntPtr)0)
            {
                byte[] szGroupStr = new byte[(int)nGroupLen + 1];
                IntPtr iRet = H.H.DPSDK_GetDGroupStr(nPDLLHandle, szGroupStr, nGroupLen, (IntPtr)10000);
                if (iRet != IntPtr.Zero) { Console.WriteLine("获取数据失败"); }
            }
            else { Console.WriteLine("获取数据初始化失败"); }
        }

        IntPtr AlarmCallback(IntPtr nPDLLHandle,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder szAlarmId,
                 IntPtr nDeviceType,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder szCameraId,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder szDeviceName,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder szChannelName,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder szCoding,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder szMessage,
                 IntPtr nAlarmType,
                 IntPtr nEventType,
                 IntPtr nLevel,
                 Int64 nTime,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder pAlarmData,
                 IntPtr nAlarmDataLen,
                 [MarshalAs(UnmanagedType.LPStr)] StringBuilder pPicData,
                 IntPtr nPicDataLen,
                 IntPtr pUserParam)
        {



            Alarm alarm = new Alarm();
            alarm.status = 0;

            DateTime dAlarmTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtAlarmTime = dAlarmTime.AddSeconds((UInt64)nTime);
            string uAlarmTime = dtAlarmTime.ToString("yyyy-MM-dd HH:mm:ss");
            alarm.start_time = uAlarmTime;
            alarm.stop_time = uAlarmTime;

            alarm.event_id = szAlarmId.ToString();

            byte[] bName = System.Text.Encoding.Default.GetBytes(szChannelName.ToString());
            string strname = System.Text.Encoding.UTF8.GetString(bName);
            alarm.device_name = strname;
            alarm.device_code = szCameraId.ToString();





            byte[] bMessage = System.Text.Encoding.Default.GetBytes(szMessage.ToString());
            string alarmMessage = System.Text.Encoding.UTF8.GetString(bMessage);
            alarm.ext_info = alarmMessage;


            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
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
            }
            foreach (MyConfiguration.m item in c)
            {
                if (item.Event_b == nAlarmType.ToString())
                {
                    JObject json = null;
                    switch (nAlarmType.ToString())
                    {
                        case "81":
                            alarm.event_name = "一键报警";
                            break;
                        case "579":
                            alarm.event_name = "人脸报警";
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            Encoding encoding = System.Text.Encoding.GetEncoding("GB2312");

                            byte[] ab = encoding.GetBytes(pAlarmData.ToString());
                            string b = System.Text.Encoding.UTF8.GetString(ab);
                            try
                            {
                                json = new JObject(
                                     new JProperty("faceTime", alarm.start_time),
                                     new JProperty("faceName", JObject.Parse(b)["personlist"][0]["name"].ToString()),
                                     new JProperty("faceImg", JObject.Parse(alarm.ext_info))
                                    );
                            }
                            catch (Exception ex)
                            {
                                json = new JObject(
                                   new JProperty("faceTime", alarm.start_time),
                                   new JProperty("faceName", JObject.Parse(b.Substring(0, b.LastIndexOf("}") + 1))["personlist"][0]["name"].ToString()),
                                   new JProperty("faceImg", JObject.Parse(alarm.ext_info))
                                  );
                            }
                            break;
                        case "302":
                            alarm.event_name = "周界报警";
                            break;
                    }


                    alarm.event_type = item.Event_a;
                    if (alarmMessage != "2")
                    {
                        var rbs = new Object();
                        rbs = new
                        {
                            msg = new
                            {
                                event_id = alarm.event_id,
                                event_type = alarm.event_type,
                                status = alarm.status,
                                start_time = alarm.start_time,
                                stop_time = alarm.stop_time,
                                event_name = alarm.event_name,
                                device_code = alarm.device_code,
                                device_name = alarm.device_name,
                                ext_info = nAlarmType.ToString() == "579" ? json : JObject.Parse(alarm.ext_info)
                            }
                        };


                        JObject rbc = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(rbs));
                        Console.WriteLine(rbc);

                        Task<String> t = LntegratedMiddleware.Hold_All.TCP.PostMsg.DataHttpPostRaw(rbc);
                        Task.Run(async () =>
                        {
                            String msg = await t;
                            if (String.IsNullOrEmpty(msg))
                                Console.WriteLine("Post失败,消息异常投递,当前消息处理方式5.0 , 可能是数据库名称初始化错误 当前名称为：" + configuration["Configs:DATABASE"]);
                            // Log.Debug("Post失败,消息异常投递,当前消息处理方式5.0 , 可能是数据库名称初始化错误 当前名称为：" + configuration["Configs:DATABASE"]);//失败
                            else
                            {
                                Console.WriteLine($"Post消息投递成功 返回为：{msg} ,当前消息处理方式5.0 数据库：" + configuration["Configs:DATABASE"]);
                            }
                        });
                    }

                }
            }


            return (IntPtr)0;
        }


        class Alarm
        {
            public string event_id { get; set; }//报警ID
            public string event_type { get; set; }//报警类型ID
            public int status { get; set; }//报警状态
            public string start_time { get; set; }//报警开始时间
            public string stop_time { get; set; }//报警结束时间
            public string event_name { get; set; }//报警类型名称
            public string device_code { get; set; }//报警设备编码

            public string device_name { get; set; }//报警设备名称
            public string ext_info { get; set; }//报警信息



        }
    }
}
