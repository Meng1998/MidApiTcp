using LntegratedMiddleware.TCP;
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Serilog;

namespace LntegratedMiddleware.GM
{
    class TCPGM
    {
        static Boolean Reconnect = false;
        static AsyncTcpClient client;
        public TCPGM()
        {
            //Console.Title = "是否启动纽贝尔TCP通信服务(YES启动NO不启动).";//设置窗口标题
            //Console.Write("Whether to enable the newbell access interface(YES/NO):");//是否启用纽贝尔的接口
            //String condition = Console.ReadLine().ToUpper();//接受控制台输入的一个字符串
            //if (!String.IsNullOrEmpty(condition) && condition == "YES" || condition == "NO")
            //    if (condition == "YES" ? !true : !false)
            //        return;
            //    else;
            //else
            //    return;




            //Console.Write("Do you want to turn on the newbell disconnect auto Cho(YES/NO):");//是否开启断开自动重连
            //condition = Console.ReadLine().ToUpper();//接受控制台输入的一个字符串
            //if (!String.IsNullOrEmpty(condition) && condition == "YES" || condition == "NO")
            //    Reconnect = condition == "YES" ? true : false;


            //Log.Debug($"GM disconnect auto reconnect status:{Reconnect}");//自动重连状态为

            Console.ResetColor(); //将控制台的前景色和背景色设为默认值


            {//连接TCP服务
                client = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("172.30.248.15"), 5757));
                client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
                client.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(client_PlaintextReceived);
                client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
                client.Connect();
            }

        }
        static Boolean Initbl = false;
      
        public static String msgdata;
        static String msgstr;
       

        #region TCP消息处理
        static void client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
         
            Log.Debug("GM access control service·");//gm和数据连接成功.
            if (!Initbl)
            {
                Initbl = true;
                //初始化通信
                String init = "0XFA002A{devno:'_DEV_05_chairman',type:'4011X', msg:'1', group:'05', call:'512011'}";
                client.Send(init);
            }
        }

        static void client_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            String Datagram = Encoding.UTF8.GetString(e.Datagram, 0, e.Datagram.Length);
            if (Datagram != "Received")
            {
                Log.Debug(string.Format("Server : {0} --> ",
                   Datagram));
            }
        }
        static void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {

            if (Reconnect)
            {
                Initbl = false;
                client = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("172.30.248.15"), 5757));
                client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
                client.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(client_PlaintextReceived);
                client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
                client.Connect();
            }
            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.Red; //设置背景色
            Log.Debug(string.Format(CultureInfo.InvariantCulture,
                "TCP server {0} has disconnected.",
                e.ToString()));
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值

        }
        #endregion

    }
    class ClientGM
    {
        private Socket socketClient;  // 客户端套接字
        private Thread threadClient;  // 客户端线程

        /// <summary>
        /// 创建套接字连接到服务端
        /// </summary>
        //private void CreateSocketConnection()
        public ClientGM()
        {
            try
            {
                // 创建一个客户端的套接字 参数(IP4寻址协议,流连接方式,TCP数据传输协议)
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 获取IP
                IPAddress address = IPAddress.Parse("172.30.248.15");
                //创建一个包含IP和端口号的网络节点对象
                Int32 Port = 5757;
                IPEndPoint ipPoint = new IPEndPoint(address, Port);
                // 连接服务端
                socketClient.Connect(ipPoint);
                // 创建一个线程,接受服务端发来的数据
                threadClient = new Thread(ReceiveService);
                // 设置线程为后台线程
                threadClient.IsBackground = true;
                // 启动线程连接服务端
                threadClient.Start();
                // 显示消息
                ShowMsg("与服务器" + address.ToString() + ":" + Port.ToString() + "成功建立连接！");

            }
            catch (Exception)
            {
                ShowMsg("服务器未启动！");
            }
        }

        //private void ReceiveService()
        public void ReceiveService()
        {
            while (true)
            {
                try
                {
                    var b = new byte[1024 * 1024 * 4];
                    int length = socketClient.Receive(b);
                    var msg = System.Text.Encoding.UTF8.GetString(b, 0, length);
                    ShowMsg(socketClient.RemoteEndPoint.ToString() + "对您: " + msg);
                }
                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                    break;
                }
            }
        }

        public void ShowMsg(string msg)
        {
            //txtConneMsg.AppendText("\r\n" + DateTime.Now + "\r\n\r\n" + msg + "\r\n");
            Log.Debug("\r\n" + DateTime.Now + "\r\n\r\n" + msg + "\r\n");
        }

        /// <summary>
        /// 发送数据到服务端
        /// </summary>
        public void Send()
        {
            if (socketClient == null)
            {

                ShowMsg("服务器未启动！");
                return;
            }
            //byte[] b = System.Text.Encoding.UTF8.GetBytes(txtSend.Text.Trim());
            string txtSend = "Hello";
            byte[] b = System.Text.Encoding.UTF8.GetBytes(txtSend);
            // 客户端向服务器发送消息
            socketClient.Send(b);
            //清空文本
            ShowMsg("您对" + socketClient.RemoteEndPoint.ToString() + "说：" + txtSend);
            txtSend = "";
        }

    }

}

