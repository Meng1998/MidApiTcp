using LntegratedMiddleware.GT.C;
using LntegratedMiddleware.NBE.M;
using LntegratedMiddleware.TCP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace LntegratedMiddleware.NBE.C
{
    class TCPGated
    {
        static Boolean Reconnect = false;
        static AsyncTcpClient client;
        public TCPGated()
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
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.Green; //设置背景色

            Log.Debug($"Newbell disconnect auto reconnect status:{Reconnect}");//自动重连状态为

            Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            strattime = DateTime.Now;

            {//连接TCP服务
                client = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("172.30.248.51"), 6000));
                client.ServerDisconnected += new EventHandler<TcpServerDisconnectedEventArgs>(client_ServerDisconnected);
                client.DatagramReceived += new EventHandler<TcpDatagramReceivedEventArgs<byte[]>>(client_PlaintextReceived);
                client.ServerConnected += new EventHandler<TcpServerConnectedEventArgs>(client_ServerConnected);
                client.Connect();
            }




        }
        static Boolean Initbl = false;
        /// <summary>
        /// 远程开门V2
        /// </summary>
        /// <param name="DoorNo">门禁编码</param>
        /// <returns></returns>
        public static String OpenDoorsV2(String DoorNo)
        {
            if (Initbl)
            {
                client.Send($"<?xml version=\"1.0\" encoding=\"gb2312\"?> <MESSAGE NAME =\"OPENDOOR\" TYPE =\"REQ\" SEQNO =\"1\"> <DoorNo>{DoorNo}</DoorNo> </MESSAGE>");
                return JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = "执行成功。"
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Red; //设置背景色

                Log.Debug("Calling interface failed. The service is not started!");

                Console.ResetColor(); //将控制台的前景色和背景色设为默认值


                return JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    MsgTxt = "TCP 请求错误 纽贝尔服务未被管理员设置启动!"
                });
            }
        }
        public static String msgdata;
        static String msgstr;
        /// <summary>
        /// 门状态
        /// </summary>
        /// <param name="DoorNo">门禁编码</param>
        /// <returns></returns>
        public static String GateState(String DoorNo)
        {
            try
            {
                if (Initbl)
                {
                    client.Send($"<?xml version=\"1.0\" encoding=\"gb2312\"?> <MESSAGE NAME =\"DOORSTATE\" TYPE =\"REQ\" SEQNO =\"1\"> <DoorNo>{DoorNo}</DoorNo> </MESSAGE> ");
                    MessageControlSending.SendMessage.state = true;

                    Int32 count = 0;
                    while (true)
                    {
                        if (!MessageControlSending.SendMessage.state)
                        {
                            msgstr = msgdata;
                            msgdata = null;

                            return XmlJson.GetxmlAppjson(msgstr, 0);

                        }
                        //count++;
                        //  Thread.Sleep(1000);

                        //if (count >= 50)
                        //{
                        //    var err = new
                        //    {
                        //        msg = "error",
                        //        MsgTxt = "接口超时已超过四秒！"
                        //    };

                        //    return JsonConvert.SerializeObject(err);
                        //}
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                    Console.BackgroundColor = ConsoleColor.Red; //设置背景色

                    Log.Debug("Calling interface failed. The service is not started!");


                    Console.ResetColor(); //将控制台的前景色和背景色设为默认值

                    return JsonConvert.SerializeObject(new
                    {
                        msg = "error",
                        MsgTxt = "TCP 请求错误 纽贝尔服务未被管理员设置启动!"
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    MsgTxt = "TCP 请求错误 纽贝尔服务未被管理员设置启动!"
                });
            }

        }
        /// <summary>
        /// 远程常开关门
        /// </summary>
        /// <param name="ComType">执行操作</param>
        /// <param name="DoorNo">门禁编码</param>
        /// <param name="CmdParam">常开时间 按分钟</param>
        /// <returns></returns>
        public static String OpenDoors(String ComType, String DoorNo, String CmdParam)
        {
            switch (ComType)
            {
                case "常开":
                    ComType = 12.ToString();
                    break;
                case "常关":
                    ComType = 13.ToString();
                    break;
                case "关门":
                    ComType = 19.ToString();
                    break;
                default:
                    ComType = 19.ToString();
                    break;
            }
            if (Initbl)
            {
                client.Send($"<?xml version=\"1.0\" encoding=\"gb2312\"?> <MESSAGE NAME =\"DOORCMD\" TYPE =\"REQ\" SEQNO =\"1\"> <DoorNo>{DoorNo}</DoorNo>//门编号 <CmdNo>{(ComType)}</CmdNo> //命令号 <CmdParam>{CmdParam}</CmdParam>//命令参数 </MESSAGE>");
                return JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = "执行成功。"
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Red; //设置背景色


                Log.Debug("Calling interface failed. The service is not started!");



                Console.ResetColor(); //将控制台的前景色和背景色设为默认值

                return JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    MsgTxt = "TCP 请求错误 纽贝尔服务未被管理员设置启动!"
                });
            }
        }

        /// <summary>
        /// 跟进卡号查询记录
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        public static String OpenDoors()
        {

            if (record.Count > 0)
            {
                object Msg = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(record));
                return JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = Msg
                });
            }
            else
                return JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    MsgTxt = DateTime.Today + "未产生门禁记录"
                });

        }


        #region TCP消息处理
        static void client_ServerConnected(object sender, TcpServerConnectedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.Green; //设置背景色
            Log.Debug("Newbell access control service·");//纽贝尔和数据连接成功.
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            if (!Initbl)
            {
                Initbl = true;
                //初始化通信
                String init = "<?xml version=\"1.0\" encoding=\"gb2312\"?> <MESSAGE NAME=\"INIT\" TYPE=\"REQ\" SEQNO=\"1\"> NULL </MESSAGE>";
                client.Send(init);
                ////初始化通信密码
                String CommunicationKey = "<?xml version=\"1.0\" encoding=\"gb2312\"?> <MESSAGE NAME=\"AUTH\" TYPE =\"REQ\" SEQNO =\"1\"> <PWD>827CCB0EEA8A706C4C34A16891F84E7B</PWD> </MESSAGE>";
                client.Send(CommunicationKey);
            }
        }
        public static List<Record> record = new List<Record>();

        static DateTime endtime;
        static DateTime strattime;
        static string strlis;
        static void client_PlaintextReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {

            Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
            Console.BackgroundColor = ConsoleColor.DarkGray; //设置背景色
            Log.Debug(string.Format("Server : {0} --> ",
                e.Datagram));
            Console.ResetColor(); //将控制台的前景色和背景色设为默认值


            string str = Encoding.GetEncoding("gb2312").GetString(e.Datagram).Remove(0, 12);
            Log.Debug(string.Format("Server : {0} --> ",
               str));
           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            XmlElement xmlElem = xmlDoc.DocumentElement;//获取根节点
            if (xmlElem.GetElementsByTagName("EmpName").Count>0)
            {
                XmlNode rootNode = xmlDoc.SelectSingleNode("MESSAGE");
                string doorNo = rootNode.SelectSingleNode("EmpName").InnerText;
                Console.WriteLine("姓名：" + doorNo);
            }  ;//取节点名
            
            Byte[] contents = Encoding.UTF8.GetBytes(str);

            ////Log.Debug(string.Format("{0}", System.Text.Encoding.UTF8.GetString(contents)));
            Byte[] temporary = new Byte[contents.Length - 12];

            //// byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(e.Datagram);

            int temporaryInt = 0;//去除和校验

            for (int i = 12; i < contents.Length; i++)
            {
                temporary[temporaryInt] = contents[i];
                temporaryInt++;
            }
            //String a = System.Text.Encoding.UTF8.GetString(temporary);
            //  Log.Debug(string.Format("{0}", e.Datagram));

            //string ss = utf8_gb2312(Encoding.UTF8.GetString(temporary));


            //msgdata += Encoding.UTF8.GetString(temporary);
            ////strlis = e.Datagram;
            //endtime = DateTime.Now;
            //if (getTime.TesRecord(strattime, endtime) > 20)
            //{
            //    record.Clear();
            //    strattime = endtime;
            //}
            //int sty = strlis.Length;
            //if (strlis.Length>20)
            //{
            //    XmlJson.GetxmlAppjson(strlis, 1);
            //}

            if (MessageControlSending.SendMessage.state)
            {
                MessageControlSending.SendMessage.state = false;
                //  MessageControlSending.SendMessage.Msg = System.Text.Encoding.UTF8.GetString(temporary);
            }



        }
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.UTF8;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }

        static void client_ServerDisconnected(object sender, TcpServerDisconnectedEventArgs e)
        {

            if (Reconnect)
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                Initbl = false;
                client = new AsyncTcpClient(new IPEndPoint(IPAddress.Parse("172.30.248.14"), 6000));
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
    public class getTime
    {
        public static DateTime dateBegin { get; set; }
        public static DateTime dateEnd { get; set; }


        public static int SubTest(DateTime dateBegin, DateTime dateEnd)
        {   //TimeSpan类
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            return (int)ts3.TotalMinutes;
        }
        public static int TesRecord(DateTime dateBegin, DateTime dateEnd)
        {   //TimeSpan类
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            return (int)ts3.TotalHours;
        }



    }
}
