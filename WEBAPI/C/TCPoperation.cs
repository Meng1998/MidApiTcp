using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WEBAPI.M;
using WEBSERVICE.C;

namespace WEBAPI.C
{
    public class TCPoperation
    {
        static GetTcpMOD Msg = new GetTcpMOD();
        static TcpSocketState tcpState = new TcpSocketState();
        static AsyncTcpClient client;
        public void TcpInit() {

            if (!tcpState.GetStateSocket)//启动TCP服务
            {
                
                string name = Dns.GetHostName();
                IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
                List<IPAddress> ipv4 = new List<IPAddress>();
                foreach (IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipv4.Add(ipa);
                        //Console.WriteLine(ipa);
                    }
                }

                client = new AsyncTcpClient(new IPEndPoint(ipv4[0], 9999));
                client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
                client.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client_PlaintextReceived);
                client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
                client.Connect();
            }
        }
        /// <summary>
        /// 发送消息 并且等待返回
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static String SendMsg(Object msg)
        { 
            Msg.GetStateMsg = true;
           
            if (tcpState.GetStateSocket)
                client.SendMsg(JsonConvert.SerializeObject(msg));
            Int32 count = 0;

            while (true)
            {
               
                if (!String.IsNullOrEmpty(Msg.TcpMsg))
                {
                    String Message = Msg.TcpMsg;
                    Msg.TcpMsg = null;
                    return Message;
                }
                count++;
                Thread.Sleep(1000);
                if (count >= 10)
                {
                    Msg.GetStateMsg = false;
                    var err = new
                    {
                        msg = "error",
                        MsgTxt = tcpState.GetStateSocket ? "接口超时已超过四秒1！" : "未与服务正常连接！"
                    };

                    return JsonConvert.SerializeObject(err);
                }
            }
            
        }
        /// <summary>
        /// 发送消息 并且等待返回
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static void SendAlarmMsg(Object msg)
        {
            if (tcpState.GetStateSocket)
                client.SendMsg(JsonConvert.SerializeObject(msg));
        }
        #region TCP消息处理
        void client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            tcpState.GetStateSocket = true;
            _Log.WriteLog("服务器连接状态", true.ToString());
        }
        void client_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<string> e)
        {
            if (e.Datagram != "Received")
            {
                _Log.WriteLog("服务器消息", string.Format("Server : {0} --> ",
                    e.TcpClient.Client.RemoteEndPoint.ToString()));
                String Datagram = e.Datagram;
                _Log.WriteLog("服务器消息", string.Format("{0}", Datagram) + System.Environment.NewLine);
                //绑定信息

                if (Msg.GetStateMsg)
                {
                    Msg = new GetTcpMOD
                    {
                        TcpMsg = Datagram
                    };
                }
                
            }

        }

        void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {

            _Log.WriteLog("服务器连接状态", (string.Format(CultureInfo.InvariantCulture,
                "TCP server {0} has disconnected.",
                e.ToString()) + System.Environment.NewLine));
            tcpState.GetStateSocket = false;


            {//tcp断线重连

                string name = Dns.GetHostName();
                IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
                List<IPAddress> ipv4 = new List<IPAddress>();
                foreach (IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                        ipv4.Add(ipa);
                }

                client = new AsyncTcpClient(new IPEndPoint(ipv4[0], 9999));
                client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
                client.PlaintextReceived += new EventHandler<TcpDatagramReceivedEventArgs<string>>(client_PlaintextReceived);
                client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
                client.Connect();
            }


        }
        #endregion

    }
}
