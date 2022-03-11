
using LntegratedMiddleware.dahua;
using LntegratedMiddleware.Hold_All;
using LntegratedMiddleware.Hold_All.Encryption;
using LntegratedMiddleware.Hold_All.MQTT;
using LntegratedMiddleware.SqlData;
using LntegratedMiddleware.TCP;
using LntegratedMiddleware.Websocket;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LntegratedMiddleware.ClearCache;
using LntegratedMiddleware.CQLJ;

namespace LntegratedMiddleware
{

    class Program
    {
         
        //static HIK.C.GetDataSet GetSetData = new HIK.C.GetDataSet();//海康key数据处理类
       

        static void Main(string[] args)
        {
            //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .WriteTo.Console()
             .MinimumLevel.Debug()
             .WriteTo.File(Path.Combine(DateTime.Now.ToString("yyyyMM") + "logs", $"log.txt"),
                 rollingInterval: RollingInterval.Day,
                 rollOnFileSizeLimit: true)
             .CreateLogger();
            new Encryption().InitRestart();
            ClearCache.ClearCache.init();

            Log.Debug($"Starting TCP and terminal communication");//正在启动 TCP和终端通信
            Console.Title = "正在启动 TCP和终端通信";//设置窗口标题
            try
            {
                new TCPoperation();
                TCPoperation.TCPserverSend("hello world");
            }
            catch (Exception ex)
            {
                Log.Debug(ex.Message);

            }
            try
            {
                #region 陆军学院

                // new xmlInfo();
                Console.WriteLine("订阅报警事件");
               // new alarmSubscribe();
               // new DHDataSet().InitParameters();
                #endregion


                // HIK.M.DHSecretKey key = DHDataSet.GetDHkey(0, 0);
                // Console.WriteLine("00");
                // new authentication();

                //Log.Debug($"Testing connectivity to database...");//测试与数据库的连接是否正常
                ////Console.Title = "测试与数据库的连接是否正常";//设置窗口标题
                ////new PGDataProcessing().TestExecuteQuery();
                //Log.Debug($"Starting hik Haikang service");//正在启动海康服务
                //Console.Title = "正在启动海康服务";//设置窗口标题
                // Log.Debug($"{(new HIK.C.GetDataSet().InitParameters() ? "ISC / SPCC key initialized successfully." : "Failed to initialize ISC / SPCC key Error.")}");// 成功/失败
                //Log.Debug($"Starting GM service");//正在启动海康服务

                // new GM.TCPGM();

                //大华
                //new DahuaInit().Init();


                //中安实时数据

                // new zhongan.RabbitMq().Rmq();

                //new zhongan.WebSocket.WebsocketServer().WebSocketInit();


                //测试HIK接口
                //Console.Title = "测试HIK接口";//设置窗口标题
                //new HIK.C.TestInterface();



                ////测试宇视接口 且初始化
                //   //Console.Title = "测试宇视接口 且初始化";//设置窗口标题
                //new YS.C.VisionOperation();



                // //Console.Title = "启动广拓接口";//设置窗口标题
                ////启动广拓接口
                //new GT.C.NewsOperation();



                //Console.Title = "启动纽贝尔";//设置窗口标题
                ////启动纽贝尔
                // new NBE.C.TCPGated();//启动纽贝尔的与服务器通信TCP线程



                //Console.Title = "启动MQTT SPCC通信";//设置窗口标题
                //初始化MQTT SPCC通信
                //new MQTT();

                //{
                //    Log.Debug($"Starting WebSocket and terminal communication{Environment.NewLine}");//正在启动 WebSocket和终端通信
                //    //Console.Title = "正在启动 WebSocket和终端通信";//设置窗口标题
                //    Console.Title = "中央API集成服务.";//设置窗口标题
                //    PublicMOD.InitStste = true;//初始化完成

                //new WebsocketServer().WebSocketInit();
                //}
                var input = Console.ReadLine();
                while (input != "exit")
                {
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            //Console.ReadLine();
        }

    }
}
