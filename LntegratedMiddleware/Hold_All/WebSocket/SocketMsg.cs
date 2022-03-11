using LntegratedMiddleware.HIK.C;
using LntegratedMiddleware.POST;
using LntegratedMiddleware.SqlData;
using LntegratedMiddleware.Websocket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static LntegratedMiddleware.Hold_All.MQTT.MQTT;

namespace LntegratedMiddleware.Hold_All.TCP
{
    class SocketMsg
    {
        public static void WebSocketMsg(String msg)
        {
            //Console.WriteLine(msg);
            JObject rbs = JsonConvert.DeserializeObject<JObject>(msg);
            string eventTypeName = "";
            switch (rbs["params"]["events"][0]["eventType"].ToString())
            {
                case "917761": 
                    eventTypeName = "呼叫报警";
                    break;
                case "327681":
                    eventTypeName = "防区报警";
                    break;
                case "197377":
                    eventTypeName = "197377";
                    break;
                case "198400":
                    eventTypeName = "开门超时";
                    break;
                case "198915":
                    eventTypeName = "刷卡加代码开门";
                    break;
                case "198913":
                    eventTypeName = "开门";
                    break;
                case "199169":
                    eventTypeName = "门已关闭";
                    break;
                case "6107119621":
                    eventTypeName = "非法入侵";
                    break;
                case "131598":
                    eventTypeName = "起身";
                    break;
                case "131603":
                    eventTypeName = "离岗";
                    break;
                case "131607":
                    eventTypeName = "折线攀高";
                    break;
                case "131609":
                    eventTypeName = "防风场滞留";
                    break;
                case "131664":
                    eventTypeName = "人数异常";
                    break;
                case "131667":
                    eventTypeName = "静坐";
                    break;
                case "198914":
                    eventTypeName = "门已打开";
                    break;
                case "327692":
                      eventTypeName = "防自缢";
                    break;
            }

            var Object = new {
                //eventTypeName= eventTypeName,

                event_id = "",
                event_type = rbs["params"]["events"][0]["eventType"].ToString(),
                status = 0,
                start_time = rbs["params"]["events"][0]["happenTime"].ToString(),
                stop_time = rbs["params"]["events"][0]["happenTime"].ToString(),
                event_name= rbs["params"]["events"][0]["data"]["eventName"].ToString(),
                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                device_name = rbs["params"]["events"][0]["srcName"].ToString(),

                //eventTypeName = rbs["params"]["events"][0]["data"]["eventTypeName"].ToString(),
            };

            //var Object = new
            //{
            //    device_info = device_info
            //};

          //  WebsocketServer.SetWebSocketMsg(JsonConvert.SerializeObject(Object));

                var client = new RestClient("http://10.0.168.200:8090/backoffice/event/info/add");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(Object), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                //Console.WriteLine("我返回了："+response.Content);
             


            //Boolean error = false;
            //{
            //    JObject rbs = JsonConvert.DeserializeObject<JObject>(msg);
            //    DataSet Ds = new DataSet();
            //    try
            //    {
            //        Ds = new PGDataProcessing().ExecuteQuery($"SELECT * FROM \"public\".\"event_type\"  WHERE plat_event_code = '{rbs["params"]["events"][0]["eventType"].ToString()}';", out error);
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Debug(ex.Message + ":WebSocketMsg TOP1");//TCP终端服务器开启成功.
            //    }
            //    try
            //    {
            //        if (error)
            //        {
            //            var Object = new Object();
            //            switch (rbs["params"]["events"][0]["eventDetails"][0]["eventType"].ToString())
            //            {
            //                case "1644175361":
            //                case "3187675137"://重点人员抓拍
            //                    #region 重点人员识别事件 固定数据格式
            //                    Object = new
            //                    {
            //                        msg = new
            //                        {
            //                            id = rbs["params"]["events"][0]["eventId"].ToString(),
            //                            event_type = Ds.Tables[0].Rows[0]["id"].ToString(),
            //                            status = rbs["params"]["events"][0]["status"].ToString(),
            //                            start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
            //                            stop_time = (rbs["params"]["sendTime"].ToString()),
            //                            event_level = (Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_level"].ToString() : ""),
            //                            event_name = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
            //                            device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
            //                            device_name = rbs["params"]["events"][0]["srcName"].ToString(),
            //                            event_type_name = (Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_name"].ToString() : ""),
            //                            ext_info = new
            //                            {
            //                                face_url = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? rbs["params"]["events"][0]["data"]["snappedPicUrl"].ToString() : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["snap"]["faceUrl"].ToString(),
            //                                match_url = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? rbs["params"]["events"][0]["data"]["refrencePicUrl"].ToString() : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["snap"]["bkgUrl"].ToString(),
            //                                camera_name = rbs["params"]["events"][0]["srcName"].ToString(),
            //                                face_name = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? rbs["params"]["events"][0]["data"]["personName"].ToString() : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["faceMatch"][0]["faceInfoName"].ToString(),
            //                                face_time = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? (rbs["params"]["events"][0]["happenTime"].ToString()) : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["snap"]["faceTime"].ToString()
            //                            }
            //                        }
            //                    };
            //                    #endregion
            //                    break;
            //                case "198400":
            //                case "198657"://重点人员抓拍
            //                    #region 门禁事件
            //                    Object = new
            //                    {
            //                        msg = new
            //                        {
            //                            id = rbs["params"]["events"][0]["eventId"].ToString(),
            //                            event_type = Ds.Tables[0].Rows[0]["id"].ToString(),
            //                            //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
            //                            start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
            //                            stop_time = (rbs["params"]["sendTime"].ToString()),
            //                            event_level = (Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_level"].ToString() : ""),
            //                            device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
            //                            device_name = rbs["params"]["events"][0]["srcName"].ToString(),
            //                            event_type_name = (Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_name"].ToString() : ""),
            //                            ext_info = new
            //                            {
            //                                //door_name = rbs["params"]["events"][0]["srcName"].ToString(),
            //                                //event_name = rbs["params"]["events"][0]["data"]["eventName"].ToString(),
            //                                //event_time = rbs["params"]["events"][0]["happenTime"].ToString()
            //                            }
            //                        }
            //                    };
            //                    #endregion
            //                    break;
            //                default:
            //                    #region 其他
            //                    Object = new
            //                    {
            //                        msg = new
            //                        {
            //                            rbs
            //                        }
            //                    };
            //                    #endregion
            //                    break;
            //            }
            //            {// 发送回调
            //                JObject rbc = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Object));

            //              //  WebsocketServer.SetWebSocketMsg(rbc["msg"].ToString());
            //            }
            //        }
            //        else
            //        {
            //           // Log.Debug("数据库错误，但依然接受到了消息。进行容错回调");
            //            {// 发送回调

            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        //Log.Debug(ex.Message + ":WebSocketMsg TOP2");//TCP终端服务器开启成功.
            //    }
            //}
            //#region 数据库存储
            //{
            //    //数据库存储
            //    try
            //    {
            //        JObject rbs = JsonConvert.DeserializeObject<JObject>(msg);
            //        DataSet Ds = new PGDataProcessing().ExecuteQuery($"SELECT * FROM \"public\".\"event_type\"  WHERE plat_event_code = '{rbs["params"]["events"][0]["eventType"].ToString()}';", out error);

            //        if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0 && error)
            //        {
            //            #region 存库
            //            String ID = Guid.NewGuid().ToString("N");
            //            String ability = Ds.Tables[0].Rows[0]["id"].ToString();//ID
            //            String status = rbs["params"]["events"][0]["status"].ToString();//事件状态, 0-瞬时 1-开始 2-停止 3-事件脉冲 4-事件联动结果更新 5-异步图片上传
            //            String happenTime = (rbs["params"]["events"][0]["happenTime"].ToString()); //开始时间
            //            String sendTime = (rbs["params"]["sendTime"].ToString());//结束时间
            //            String eventType = rbs["params"]["events"][0]["eventType"].ToString();//事件名称代码
            //            String srcParentIndex = rbs["params"]["events"][0]["srcIndex"].ToString();//事件源设备
            //            String srcName = null;
            //            DataSet Dss = new PGDataProcessing().ExecuteQuery($"SELECT * FROM \"public\".\"dock_device\"  WHERE device_code = '{srcParentIndex}';", out error);

            //            if (Dss.Tables.Count > 0 && Dss.Tables[0].Rows.Count > 0)
            //                srcName = Dss.Tables[0].Rows[0]["device_name"].ToString();//事件源名称


            //            new PGDataProcessing().ExecuteQuery($"INSERT INTO \"public\".\"event_info\"(\"id\", \"event_type\", \"status\", \"start_time\", \"stop_time\", \"event_level\", \"event_name\", \"device_code\", \"device_name\", \"ext_info\",\"event_type_name\") " +
            //            $"VALUES ('{ID}', '{ability}', '{status}', '{happenTime}', '{sendTime}', '{(Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_level"].ToString() : "")}', '{eventType}', '{srcParentIndex}', '{srcName}','{rbs["params"]}','{(Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_name"].ToString() : "")}') ", out error);
            //            if (error)
            //                Log.Debug($"MQTT提示：MQTT信息数据库存储正常");
            //            else
            //                Log.Debug($"MQTT数据库存储信息报错.");
            //            #endregion
            //        }
            //    }
            //    catch (Exception ex)
            //    {            
            //       // Log.Debug($"PGSQL ERROR. :" + ex.Message);

            //    }
            //}
            //#endregion
        }
    }

    class PostMsg
    {

        public static async Task<String> DataHttpPostRaw(JObject rbc)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
              .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
            IConfiguration configuration = builder.Build();
            try
            {
                var list = await Task.Run(() => Post.HttpPostRaw($"http://localhost:8098/{configuration["Configs:DATABASE"]}/event/info/add", rbc["msg"].ToString(), out Boolean _));
                return list;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return "";
            }
            return "";
        }


        public static void SetPostMsgISC(String msg)
        {

            JObject rbs = JsonConvert.DeserializeObject<JObject>(msg);

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
              .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("Event_Type");
            List<MyConfiguration.m> c = new List<MyConfiguration.m>();
            var dic = new Dictionary<String, String>();
            foreach (IConfigurationSection section in a.GetChildren())
            {
                c.Add(
                        new MyConfiguration.m
                        {
                            Event_a = section.GetValue<string>("Event_TypeName"),
                            Event_b = section.GetValue<string>("Event_Code")
                        }
                    );
                dic.Add(section.GetValue<string>("Event_Code"), section.GetValue<string>("Event_TypeName"));
                Log.Debug($"缓冲：{section.GetValue<string>("Event_TypeName")}  {section.GetValue<string>("Event_Code")}");
            }
            String EType = "";
            string type = "";
            Log.Debug("原始的数据：" + rbs.ToString());
            try
            {
                 EType = dic[rbs["params"]["events"][0]["eventDetails"][0]["eventType"].ToString()];
                type = rbs["params"]["events"][0]["eventDetails"][0]["eventType"].ToString();
            }
            catch (Exception)
            {
                EType = dic[rbs["params"]["events"][0]["eventType"].ToString()];
                type = rbs["params"]["events"][0]["eventType"].ToString();
            }
          
            try
            {

                var Object = new Object();
                switch (type)
                {
                    case "1644175361":
                    case "3187675137"://重点人员抓拍
                        #region 重点人员识别事件 固定·数据格式
                        try
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                    status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    event_name = "重点人员识别事件",//事件类型
                                    device_code = rbs["params"]["events"][0]["eventDetails"][0]["data"]["resInfo"][0]["indexCode"].ToString(),
                                    device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                    ext_info = rbs
                                    //  linkage_info = rbs["params"]["events"][0]["linkageAcion"][0]["content"].ToString()
                                }
                            };
                        }
                        catch (Exception)
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                    status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    event_name = "重点人员识别事件",//事件类型
                                    device_code = rbs["params"]["events"][0]["data"]["resInfo"][0]["indexCode"].ToString(),
                                    device_name = rbs["params"]["events"][0]["data"]["resInfo"][0]["cn"].ToString(),
                                    ext_info = rbs
                                    //  linkage_info = rbs["params"]["events"][0]["linkageAcion"][0]["content"].ToString()
                                }
                            };

                        }
                      
                        #endregion
                        break;
                    case "327687"://紧急报警
                        #region 紧急报警
                        Object = new
                        {
                            msg = new
                            {
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "紧急报警-报警柱",//事件类型
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString() == "f12024c0c4ee48cfb5e1f5533a4a7b5a" ? "e6b5915b5ed2448984d7bdbd8ad526f1" : "8e36c78df5434f448fff0febb8a3213a",// rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcIndex"].ToString() == "f12024c0c4ee48cfb5e1f5533a4a7b5a" ? "报警柱1" : "报警柱2",// rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                //  linkage_info = rbs["params"]["events"][0]["linkageAcion"][0]["content"].ToString(),
                            }
                        };
                        #endregion
                        break;
                    case "198400":
                    case "198657":
                        #region 门被外力开启
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                event_name = "门被外力开启",//事件类型
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "196893":
                        #region 人脸认证通过
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "人脸认证通过",//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "198914":
                        #region 合法卡比对通过
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "合法卡比对通过",//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //  linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "197635":
                        #region 卡未分配权限
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "卡未分配权限",//事件类型
                                //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "197378":
                        #region 权限不合
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "权限不合",//事件类型
                                //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "196888":
                        #region 人脸+指纹认证通过
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "人脸+指纹认证通过",//事件类型
                                //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "196890":
                        #region 人脸+刷卡认证通过
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "人脸+刷卡认证通过",//事件类型
                                //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "196885":
                        #region 指纹+刷卡认证通过
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "指纹+刷卡认证通过",//事件类型
                                //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //     linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "254005":
                        #region 手动告警
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "手动告警",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "254022":
                        #region 烟雾报警                   
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                event_name = "烟雾报警",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                // linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };

                        #endregion
                        break;
                    case "254043":
                        #region 温度报警
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "温度报警",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                //  linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion                     
                        break;
                    case "254014":
                        #region 火灾按钮报警
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "火灾按钮报警",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                // linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion                     
                        break;
                    case "327681":
                        #region 防区报警
                        try
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "防区报警",//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                    device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                    ext_info = rbs,
                                }
                            };
                        }
                        catch
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "防区报警",//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                    device_name = "",
                                    ext_info = rbs,
                                    //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                                }
                            };
                        }
                        #endregion             
                        break;
                    case "131588":
                        #region 区域入侵
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "区域入侵",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "131586":
                        #region 进入区域
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "进入区域",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                //  linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "131585":
                        #region 穿越警戒面(越界侦测)
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//Ds.Tables[0].Rows[0]["id"].ToString(),
                                status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = "穿越警戒面",//事件类型
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                ext_info = rbs
                                //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "197127":
                        #region 指纹对比通过
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = 0,
                                event_name = "指纹对比通过",//事件类型
                                //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                ext_info = rbs,
                                //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                            }
                        };
                        #endregion
                        break;
                    case "771760130":
                        #region 入场压线事件

                        try
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "入场压线事件",//事件类型
                                     //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                    device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                    ext_info = rbs,
                                    //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                                }
                            };
                        }
                        catch {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "入场压线事件",//事件类型
                                                          //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                    device_name = "",
                                    ext_info = rbs,
                                    //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                                }
                            };
                        }
                    
                        #endregion
                        break;
                    case "771760133":
                        #region 出场压线事件
                        try
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "入场压线事件",//事件类型
                                                          //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                    device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                    ext_info = rbs,
                                }
                            };
                        }
                        catch {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "入场压线事件",//事件类型
                                                          //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                    device_name = "",
                                    ext_info = rbs,
                                    //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                                }
                            };
                        }
                     
                        #endregion
                        break;
                    case "197634":
                        #region 无此卡号
                        try
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "无此卡号",//事件类型
                                                          //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["eventDetails"][0]["srcIndex"].ToString(),
                                    device_name = rbs["params"]["events"][0]["eventDetails"][0]["srcName"].ToString(),
                                    ext_info = rbs,
                                }
                            };
                        }
                        catch
                        {
                            Object = new
                            {
                                msg = new
                                {
                                    event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                    event_type = EType,
                                    status = 0,
                                    event_name = "无此卡号",//事件类型
                                                          //event_type = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                    start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                    stop_time = (rbs["params"]["sendTime"].ToString()),
                                    device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                    device_name = "",
                                    ext_info = rbs,
                                    //linkage_info = new String[0]//rbs["params"]["events"][0]["linkageAcion"][1]["content"].ToString()
                                }
                            };
                        }
                        #endregion
                        break;
                       
                    default:
                        Log.Debug("没找到对应解析");
                        #region 其他
                        Object = new
                        {
                            msg = new
                            {
                                rbs
                            }
                        };
                        #endregion
                        break;
                        
                }
                {// 发送回调
                    JObject rbc = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Object));
                    Log.Debug("传递的数据：" + rbc);

                    Task<String> t = DataHttpPostRaw(rbc);
                    Task.Run(async () =>
                    {
                        String msg = await t;
                        if (String.IsNullOrEmpty(msg))
                            Log.Debug("Post失败,消息异常投递,当前消息处理方式5.0 , 可能是数据库名称初始化错误 当前名称为：" + configuration["Configs:DATABASE"]);//失败
                        else
                        {
                            Log.Debug($"Post消息投递成功 返回为：{msg} ,当前消息处理方式5.0 数据库：" + configuration["Configs:DATABASE"]);//成功
                        }
                    });

                }
            }
            catch (Exception ex)
            {

                Log.Debug(ex.Message + ":SetPostMsg");//TCP终端服务器开启成功.
            }

        }
        /// <summary>
        /// spcc投递至5.0api
        /// </summary>
        /// <param name="msg"></param>
        public static void SetPostMsgSPCC(String msg)
        {
           
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
           .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("Event_Type");
            List<MyConfiguration.m> c = new List<MyConfiguration.m>();
            var dic = new Dictionary<String, String>();
            foreach (IConfigurationSection section in a.GetChildren())
            {
                c.Add(
                        new MyConfiguration.m
                        {
                            Event_a = section.GetValue<string>("Event_TypeName"),
                            Event_b = section.GetValue<string>("Event_Code")
                        }
                    );
                dic.Add(section.GetValue<string>("Event_Code"), section.GetValue<string>("Event_TypeName"));
                Log.Debug($"缓冲：{section.GetValue<string>("Event_TypeName")}  {section.GetValue<string>("Event_Code")}");
            }


            JObject rbs = JsonConvert.DeserializeObject<JObject>(msg);
            Log.Debug("原始的数据：" + rbs.ToString());
            DataSet Ds = new DataSet();
            String EType = null;
            try
            {
                EType = dic[rbs["params"]["events"][0]["eventType"].ToString()];
            }
            catch (Exception)
            {
                EType = dic[rbs["params"]["eventType"].ToString()];
            }

            Boolean error = true;
            try
            {
                var Object = new Object();
                string type = null;
                try
                {
                    type = rbs["params"]["events"][0]["eventType"].ToString();
                }
                catch (Exception)
                {

                    type = rbs["params"]["eventType"].ToString();
                }

                switch (type)
                {
                    case "1644175361":
                    case "3187675137"://重点人员抓拍
                        #region 重点人员识别事件 固定数据格式
                        Object = new
                        {
                            msg = new
                            {
                                id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_level = (Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_level"].ToString() : ""),
                                event_name = rbs["params"]["events"][0]["eventType"].ToString(),//事件类型
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                event_type_name = (Ds.Tables[0].Rows.Count > 0 ? Ds.Tables[0].Rows[0]["event_name"].ToString() : ""),
                                ext_info = new
                                {
                                    face_url = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? rbs["params"]["events"][0]["data"]["snappedPicUrl"].ToString() : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["snap"]["faceUrl"].ToString(),
                                    match_url = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? rbs["params"]["events"][0]["data"]["refrencePicUrl"].ToString() : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["snap"]["bkgUrl"].ToString(),
                                    camera_name = rbs["params"]["events"][0]["srcName"].ToString(),
                                    face_name = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? rbs["params"]["events"][0]["data"]["personName"].ToString() : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["faceMatch"][0]["faceInfoName"].ToString(),
                                    face_time = rbs["params"]["events"][0]["eventType"].ToString() == "3187675137" ? (rbs["params"]["events"][0]["happenTime"].ToString()) : rbs["params"]["events"][0]["data"]["faceRecognitionResult"]["snap"]["faceTime"].ToString()
                                },
                                //       linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    case "198400"://开门超时
                    case "198657"://非法开门
                    case "197377"://权限不合
                    case "198914"://刷卡开门
                    case "197382"://刷卡加代码权限不符
                    case "198915"://刷卡加代码开门
                    case "198913"://开门
                    case "199169"://门已关闭
                    case "199425"://胁迫开门
                    case "199428"://胁迫报警
                    case "198916"://按钮开门
                    case "197634"://无此卡号
                        #region 门禁事件
                        Console.WriteLine("门禁事件");
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),                      
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventTypeName"].ToString()),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"].ToString()),
                                ext_info = rbs,
                                //linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //防区报警
                    case "327681":
                        #region 防区报警               
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //  linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //区域入侵
                    case "131588":
                        #region 区域入侵            
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventTypeName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //    linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //剧烈运动
                    case "131596":
                        #region 剧烈运动   
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //人员站立
                    case "131666":
                        #region 人员站立             
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //如厕超时
                    case "131608":
                        #region 如厕超时
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        break;
                    #endregion
                    //放风场泄露
                    case "131609":
                        #region

                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    case "131597":
                        #region 131597
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //按钮报警
                    case "327687":
                        #region 按钮报警

                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //车底检测
                    case "6158741505":
                        #region 车底检测
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["status"].ToString(),
                                start_time = DateTime.Parse((rbs["params"]["happenTime"].ToString())).ToString("yyyy-MM-dd HH:mm:ss"),
                                stop_time = DateTime.Parse((rbs["params"]["sendTime"].ToString())).ToString("yyyy-MM-dd HH:mm:ss"),
                                device_code = "",
                                device_name = "",
                                ext_info = rbs,
                                //linkage_info = new String[] { }
                            }
                        };

                        #endregion
                        break;
                    case "254005":
                        #region 手动告警
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    //case "198914":
                    //    #region 刷卡门禁
                    //    Object = new
                    //    {
                    //        msg = new
                    //        {
                    //            event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                    //            event_type = EType,//事件类型
                    //            status = rbs["params"]["events"][0]["status"].ToString(),
                    //            start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                    //            stop_time = (rbs["params"]["sendTime"].ToString()),
                    //            event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                    //            device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                    //            device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                    //            ext_info = rbs,
                    //            //   linkage_info = new String[] { }
                    //        }
                    //    };
                    //    #endregion
                        //break;
                    case "771760131":
                        #region 入场压线事件
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                    case "771760133":
                        #region 出场压线事件
                        Object = new
                        {
                            msg = new
                            {
                                event_id = rbs["params"]["events"][0]["eventId"].ToString(),
                                event_type = EType,//事件类型
                                status = rbs["params"]["events"][0]["status"].ToString(),
                                start_time = (rbs["params"]["events"][0]["happenTime"].ToString()),
                                stop_time = (rbs["params"]["sendTime"].ToString()),
                                event_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                device_code = rbs["params"]["events"][0]["srcIndex"].ToString(),
                                device_name = (rbs["params"]["events"][0]["data"]["eventName"]).ToString(),
                                ext_info = rbs,
                                //   linkage_info = new String[] { }
                            }
                        };
                        #endregion
                        break;
                
                    default:
                        #region 其他
                        Object = new
                        {
                            msg = new
                            {
                                rbs
                            }
                        };
                        #endregion
                        break;
                }
                {// 发送回调
                    JObject rbc = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Object));
                    Log.Debug("传递的数据：" + rbc);
                    Task<String> t = DataHttpPostRaw(rbc);
                    Task.Run(async () =>
                    {
                        String msg = await t;
                        if (String.IsNullOrEmpty(msg))
                            Log.Debug("Post失败,消息异常投递,当前消息处理方式5.0 , 可能是数据库名称初始化错误 当前名称为：" + configuration["Configs:DATABASE"]);//失败
                        else
                        {
                            Log.Debug($"Post消息投递成功 返回为：{msg} ,当前消息处理方式5.0 数据库：" + configuration["Configs:DATABASE"]);//成功
                        }
                    });

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);//TCP终端服务器开启成功.
            }
        }


        public static void SetPostMsgDH(String msg)
        {
            JObject rbs = JsonConvert.DeserializeObject<JObject>(msg);

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
              .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("Event_Type");
            List<MyConfiguration.m> c = new List<MyConfiguration.m>();
            var dic = new Dictionary<String, String>();
            foreach (IConfigurationSection section in a.GetChildren())
            {
                c.Add(
                        new MyConfiguration.m
                        {
                            Event_a = section.GetValue<string>("Event_TypeName"),
                            Event_b = section.GetValue<string>("Event_Code")
                        }
                    );
                dic.Add(section.GetValue<string>("Event_Code"), section.GetValue<string>("Event_TypeName"));
                Log.Debug($"缓冲：{section.GetValue<string>("Event_TypeName")}  {section.GetValue<string>("Event_Code")}");
            }
            String EType = "";
            string type = "";
            Log.Debug("原始的数据：" + rbs.ToString());
          

            var Object = new Object();
            switch (rbs["interfaceCode"].ToString())
            {
                case "EVENT_DEVICE_ALARM_RECORD"://周界


                    Object = new
                    {
                        msg = new
                        {
                            event_id = rbs["data"]["alarmCode"].ToString(),
                            event_type = "E82F6B9228642E439358422FB6F8D357",//Ds.Tables[0].Rows[0]["id"].ToString(),
                            status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                            start_time = (rbs["data"]["alarmTime"].ToString()),
                            stop_time = (rbs["data"]["alarmTime"].ToString()),
                            event_name = "周界报警",//事件类型
                            device_code = rbs["data"]["alarmChannelCode"].ToString(),
                            device_name = rbs["data"]["alarmChannelName"].ToString(),
                            ext_info = rbs
                        }
                    };
                    break;
                case "EVENT_FACE_RECOGNITION_RECORD"://周界
                    Object = new
                    {
                        msg = new
                        {
                            event_id = Guid.NewGuid().ToString("N"),
                            event_type = "C1F7A2BD34C8A84704D80C4DF6E72D93",//Ds.Tables[0].Rows[0]["id"].ToString(),
                            status = 0,//rbs["params"]["events"][0]["status"].ToString(),
                            start_time = (rbs["data"]["detectTime"].ToString()),
                            stop_time = (rbs["data"]["detectTime"].ToString()),
                            event_name = "重点人员识别",//事件类型
                            device_code = rbs["data"]["channelCode"].ToString(),
                            device_name = rbs["data"]["channelName"].ToString(),
                            ext_info = rbs
                        }
                    };
                    break;
            }

            {// 发送回调
                JObject rbc = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Object));
                Log.Debug("传递的数据：" + rbc);

                Task<String> t = DataHttpPostRaw(rbc);
                Task.Run(async () =>
                {
                    String msg = await t;
                    if (String.IsNullOrEmpty(msg))
                        Log.Debug("Post失败,消息异常投递,当前消息处理方式5.0 , 可能是数据库名称初始化错误 当前名称为：" + configuration["Configs:DATABASE"]);//失败
                    else
                    {
                        Log.Debug($"Post消息投递成功 返回为：{msg} ,当前消息处理方式5.0 数据库：" + configuration["Configs:DATABASE"]);//成功
                    }
                });

            }
        }
    }
}
