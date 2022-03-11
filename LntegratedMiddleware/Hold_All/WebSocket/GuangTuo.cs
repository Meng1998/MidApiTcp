using LntegratedMiddleware.POST;
using LntegratedMiddleware.TCP;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using LntegratedMiddleware;
using System.Linq;
using Serilog;
using LntegratedMiddleware.Websocket;

namespace LntegratedMiddleware.Hold_All.WebSocket
{
  public  class GuangTuo
    {
            /// <summary>
            /// 存储海口和广拓的数据
            /// </summary>
         Dictionary<string, string> Sector = new Dictionary<string, string>();


        /// <summary>
        /// 接收报警
        /// </summary>
        /// <param name="msg"></param>
        public void ALarm(String msg)
        {
          
              SectorAdd();
              JObject alarm = JObject.Parse(msg);
            Console.WriteLine("广拓的报警：" + msg);
            //alarm["alarms"][0]["uniquecode"].ToString()
            string code = null;
            try
            {
                code = Sector.FirstOrDefault(q => q.Value == alarm["alarms"][0]["uniquecode"].ToString()).Key;
                var Object = new Object();
                if (code != null)
                {
                    Object = new
                    {
                        msg = new
                        {
                            event_id = Guid.NewGuid().ToString(),
                            event_type = "FCBF427CF1B1A045E0FA495D79C00E78",//Ds.Tables[0].Rows[0]["id"].ToString(),
                            status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                            start_time = alarm["alarms"][0]["alarmtime"].ToString(),
                            stop_time = alarm["alarms"][0]["alarmtime"].ToString(),
                            event_name = "防区报警",//事件类型
                            device_code = code.ToString(),
                            device_name = alarm["alarms"][0]["chnname"].ToString(),
                            ext_info = alarm
                        }
                    };


                    JObject rbc = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Object));
                    Console.WriteLine("广拓发送的数据：" + rbc.ToString());
                    Task<String> t = DataHttpPostRaw(rbc);
                    Task.Run(async () =>
                    {
                        var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                        .AddJsonFile("appsettings.json");
                        IConfiguration configuration = builder.Build();
                        String msg = await t;
                        if (String.IsNullOrEmpty(msg))
                            Log.Debug("Post失败,消息异常投递,当前消息处理方式5.0 , 可能是数据库名称初始化错误 当前名称为：" + configuration["Configs:DATABASE"]);//失败
                        else
                        {
                            Log.Debug($"Post消息投递成功 返回为：{msg} ,当前消息处理方式5.0 数据库：" + configuration["Configs:DATABASE"]);//成功
                        }
                    });
                }
                else
                {
                    Console.WriteLine("广拓的设备编码未找到");
                }
            }
            catch {
                Console.WriteLine("广拓的设备编码未找到");
            }
          
        }



        /// <summary>
        /// 撤布防http请求
        /// </summary>
        /// <param name="rbc"></param>
        /// <returns></returns>
        public static async Task<String> DataHttpPostRaw(JObject rbc)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
              .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
            IConfiguration configuration = builder.Build();
            try
            {
                var list = await Task.Run(() => Post.HttpPostRaw($"http://127.0.0.1:8090/{configuration["Configs:DATABASE"]}/event/info/accept", rbc["msg"].ToString(), out Boolean _));
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
            return "";
        }

        /// <summary>
        /// 撤防布防
        /// </summary>
        public async Task Defense(AsyncTcpServer server, TcpClient e, string msg)
        {
            JObject rbc = JObject.Parse(msg);
            SectorAdd();
            if (Sector.ContainsKey(rbc["uniquecode"].ToString()))
            {
                rbc["uniquecode"] = Sector[rbc["uniquecode"].ToString()];//c0f885ce0c744a8a809743ead0043dc3
                bool flag = false;
                var list = await Task.Run(() => Post.HttpPostRaw($"http://192.168.3.251:20433/resource/ctrl", rbc.ToString(), out flag));
                if (flag)
                    server.Send(e, list);
                else
                    server.Send(e, JsonConvert.SerializeObject(new
                    {
                        msg = "error",
                        Remarks = "接口超时或报错！" + list
                    }));
            }
            else {
                server.Send(e, JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = "未找到设备编码！"
                }));
            }
          
        }



        public void GuangTuoState(String msg)
        {

            //SectorAdd();
            //foreach (JObject item in msg)
            //{
            //    string code = Sector.FirstOrDefault(q => q.Value == item["chnuniquecode"].ToString()).Key;
            //    if (code != null)
            //    {
            //        item["chnuniquecode"] = code;
            //    }                 
            //}
            //Console.WriteLine("报警信息："+ msg);
            Log.Debug(msg);

            WebsocketServer.SetWebSocketMsg(JsonConvert.SerializeObject(msg));
        }


        /// <summary>
        /// 把设备的信息存入进来
        /// </summary>
        public  void SectorAdd() {
            if (Sector.Count == 0) {
                if (Sector.Count == 0)
                {
                    var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                     .AddJsonFile("appsettings.json");
                    //AppContext.BaseDirectory + "appsettings.json";
                    IConfiguration configuration = builder.Build();
                    try
                    {
                        for (int i = 0; i < 88; i++)
                        {
                            string device_code = configuration["GuangTuo:" + i + ":device_code"];
                            string chnuniquecode = configuration["GuangTuo:" + i + ":chnuniquecode"];
                            Sector.Add(device_code, chnuniquecode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex.ToString());
                    }
                }
            }
        }
    }
}
