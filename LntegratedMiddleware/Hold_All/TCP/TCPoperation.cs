using LntegratedMiddleware.CQLJ;
using LntegratedMiddleware.HIK.C;
using LntegratedMiddleware.Hold_All;
using LntegratedMiddleware.Hold_All.TCP;
using LntegratedMiddleware.Hold_All.WebSocket;
using LntegratedMiddleware.NBE.C;
using LntegratedMiddleware.Websocket;
using LntegratedMiddleware.YS.C;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace LntegratedMiddleware.TCP
{
    class TCPoperation
    {
        static AsyncTcpServer server;
        public TCPoperation()
        {
            server = new AsyncTcpServer(9999);
            server.ClientConnected +=
              new EventHandler<TcpClientConnectedEventArgs>(server_ClientConnected);
            server.ClientDisconnected +=
              new EventHandler<TcpClientDisconnectedEventArgs>(server_ClientDisconnected);
            server.PlaintextReceived +=
              new EventHandler<TcpDatagramReceivedEventArgs<string>>(server_PlaintextReceived);
            server.Start();
            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.Green; //设置背景色
            Log.Debug("TCP start OK!");//TCP终端服务器开启成功.
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值
        }

        #region 消息处理
        void server_ClientConnected(object sender, TcpClientConnectedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.Green; //设置背景色
            Log.Debug(string.Format(CultureInfo.InvariantCulture,
                "TCP client {0} has connected.",
                e.TcpClient.Client.RemoteEndPoint.ToString()) + System.Environment.NewLine);
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            server.Send(e.TcpClient, "HHHHHHH");


        }
        void server_ClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.Red; //设置背景色
            Log.Debug(string.Format(CultureInfo.InvariantCulture,
                "TCP client {0} has disconnected.",
                e.TcpClient.Client.RemoteEndPoint.ToString()) + System.Environment.NewLine);
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值

        }
        void server_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            if (PublicMOD.InitStste)
            {
                Log.Debug(string.Format("Client : {0} --> ",
                      e.TcpClient.Client.RemoteEndPoint.ToString()));
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.DarkGray; //设置背景色
                Log.Debug(string.Format("{0}", e.Datagram) + System.Environment.NewLine);
                Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            }


            JObject rb;
            try
            {
                rb = JsonConvert.DeserializeObject<JObject>(e.Datagram);
                ObjectRequestProcessing(rb, e);

            }
            catch (Exception)
            {
                try
                {
                    //防止淤积请求
                    foreach (var item in e.Datagram.Split("\n"))
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(item);
                        ObjectRequestProcessing(rb, e);
                    }
                }
                catch (Exception)
                {

                    WebsocketServer.SetWebSocketMsg(e.Datagram);

                }
            }


        }
        /// <summary>
        /// 对发送对象做数据处理 并且返回
        /// </summary>
        /// <param name="rb">json</param>
        /// <param name="e">发送对象链接</param>
        public void ObjectRequestProcessing(JObject rb, TcpDatagramReceivedEventArgs<string> e)
        {
            #region 宇视WEBAPI接口处理
            switch (rb["MsgType"].ToString())
            {
                case "YSWEBAPI":
                    switch (rb["eventType"].ToString())
                    {
                        case "RequestMe"://对外接口 给第三方调用获取回调并且转发WebSocket
                            server.Send(e.TcpClient, JsonConvert.SerializeObject(new
                            {
                                msg = "Success",
                                MsgTxt = "执行成功。"
                            }));
                            WebsocketServer.SetWebSocketMsg(rb["Parameter"].ToString());
                            break;
                        case "GetAreaList"://区域列表
                            server.Send(e.TcpClient, VisionOperation.GETRegionList());
                            break;
                        case "AlarmSubscription"://区域列表
                            server.Send(e.TcpClient, VisionOperation.AlarmSubscription(rb["Parameter"]["Url"].ToString(), Int32.Parse(rb["Parameter"]["Type"].ToString())));
                            break;
                        case "GETEquipmentList"://设备列表
                            server.Send(e.TcpClient, VisionOperation.GETEquipmentList());
                            break;
                        case "ControlCloudPlatform"://云台控制
                            server.Send(e.TcpClient, JsonConvert.SerializeObject(new
                            {
                                Msg =
                                VisionOperation.ControlCloudPlatform(
                                    rb["Parameter"]["EquipmentCode"].ToString(),
                                    Int32.Parse(rb["Parameter"]["PTZCmdID"].ToString()),
                                    Int32.Parse(rb["Parameter"]["PTZCmdPara1"].ToString())
                                    )
                            }));
                            break;

                    }
                    break;
            }
            #endregion
            #region 纽贝尔WEBAPI接口处理
            switch (rb["MsgType"].ToString())
            {
                case "NBEWEBAPI":
                    switch (rb["eventType"].ToString())
                    {
                        case "OpenDoorsV2":
                            server.Send(e.TcpClient, TCPGated.OpenDoorsV2(rb["Parameter"]["DoorNo"].ToString()));
                            break;
                        case "OpenDoors":
                            server.Send(e.TcpClient, TCPGated.OpenDoors(rb["Parameter"]["ComType"].ToString(), rb["Parameter"]["DoorNo"].ToString(), rb["Parameter"]["CmdParam"].ToString()));
                            break;
                        case "GateState":
                            server.Send(e.TcpClient, TCPGated.GateState(rb["Parameter"]["DoorNo"].ToString()));
                            break;
                    }
                    break;
            }
            #endregion
            #region 大华WEBAPI接口处理
            switch (rb["MsgType"].ToString())
            {
                case "DHWEBAPI":
                    switch (rb["eventType"].ToString())
                    {
                        case "EGTTotal":
                            //Log.Debug(D.Backup.EGTTotal());
                            //TCPGated.OpenDoorsV2(rb["Parameter"]["DoorNo"].ToString()
                            server.Send(e.TcpClient, JsonConvert.SerializeObject(D.Backup.EGTTotal(rb["Parameter"]["CameraId"].ToString(), rb["Parameter"]["nBeginStr"].ToString(), rb["Parameter"]["nEndStr"].ToString())));
                            break;
                        case "RequestMe":
                            server.Send(e.TcpClient, JsonConvert.SerializeObject(new
                            {
                                msg = "Success",
                                MsgTxt = "执行成功。"
                            }));

                            PostMsg.SetPostMsgDH(rb["Parameter"].ToString());
                            break;
                        case "H89Door":
                            
                            server.Send(e.TcpClient,new authentication().DHpostRespan(Int32.Parse(rb["Index"].ToString()),rb["Parameter"].ToString(),Int32.Parse(rb["Restype"].ToString())));
                            break;
                        case "list":
                            server.Send(e.TcpClient, xmlInfo.GetXmlData());
                            break;
                    }
                    break;
            }
            #endregion
            #region 海康WEBAPI接口处理
            switch (rb["MsgType"].ToString())
            {
                case "HIKWEBAPI":
                    switch (rb["eventType"].ToString())
                    {
                        case "RequestMe"://对外接口 给第三方调用获取回调并且转发WebSocket
                            server.Send(e.TcpClient, JsonConvert.SerializeObject(new
                            {
                                msg = "Success",
                                MsgTxt = rb["Parameter"].ToString()//"执行成功。"
                            }));
                            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                              .AddJsonFile("appsettings.json");
                            //AppContext.BaseDirectory + "appsettings.json";
                            IConfiguration configuration = builder.Build();

                            //   Log.Debug(configuration["Edition:VersionNo"]);
                            switch (configuration["Edition:VersionNo"])
                            {
                                case "5.0":
                                    PostMsg.SetPostMsgISC(rb["Parameter"].ToString());
                                    break;
                                case "4.0":
                                    SocketMsg.WebSocketMsg(rb["Parameter"].ToString());
                                    break;
                                default:
                                    PostMsg.SetPostMsgISC(rb["Parameter"].ToString());
                                    SocketMsg.WebSocketMsg(rb["Parameter"].ToString());
                                    break;
                            }
                            break;
                        case "ISCWEBAPI":
                            new HIKoperation().HIKGETDATA(server, e.TcpClient, rb["Parameter"], 0, Int32.Parse(rb["GetKEYIndex"].ToString()));
                            break;
                        case "SPCCWEBAPI":
                            new HIKoperation().HIKGETDATA(server, e.TcpClient, rb["Parameter"], 1, Int32.Parse(rb["GetKEYIndex"].ToString()));
                            //server.Send(e.TcpClient, new HIKoperation().HIKGETDATA(rb["Parameter"], out _, 1, Int32.Parse(rb["GetKEYIndex"].ToString())));
                            break;

                    }
                       break;
              }
          
            //广拓
            switch (rb["MsgType"].ToString())
            {
                case "GuangTuo":
                    switch (rb["eventType"].ToString())
                    {
                        case "Alarm":
                            server.Send(e.TcpClient, JsonConvert.SerializeObject(new
                            {
                                msg = "Success",
                                MsgTxt = "执行成功。"
                            }));
                            new GuangTuo().ALarm(rb["Parameter"].ToString());
                            break;
                        case "Defense":
                            new GuangTuo().Defense(server, e.TcpClient, rb["Parameter"].ToString());
                            break;
                        case "GuangTuoState":
                            //server.Send(e.TcpClient, JsonConvert.SerializeObject(new
                            //{
                            //    msg = "Success",
                            //    MsgTxt = "执行成功。"
                            //}));
                            new GuangTuo().GuangTuoState(rb["Parameter"].ToString());               
                            break;
                    }     
                    break;
            }

            #endregion
        }
    #endregion
    public static void TCPserverSend(String msg)
    {
        server.SendAll(msg);
    }

}
}
