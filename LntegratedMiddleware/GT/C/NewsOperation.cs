using LntegratedMiddleware.POST;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LntegratedMiddleware.GT.C
{
    class NewsOperation
    {
        String clientid = null; String url = null;
        public NewsOperation()
        {
            //Console.Title = "是否启广拓API服务(YES启动NO不启动).";//设置窗口标题
            Log.Debug($"Whether to test the extension interface(YES/NO):");
            String condition = Console.ReadLine().ToUpper();//接受控制台输入的一个字符串

            if (!String.IsNullOrEmpty(condition) && condition == "YES" || condition == "NO")
                if (condition == "YES" ? !true : !false)
                    return;
                else
                    ;
            else
                return;


            Log.Debug($"Register extension callback connection URL and ID");//注册广拓回调地址
            Log.Debug($"URL:");
            clientid = Console.ReadLine().ToUpper();
            Log.Debug($"ID:");
            url = Console.ReadLine().ToUpper();
            if (String.IsNullOrEmpty(clientid))
                clientid = "1201B1247BB24C74BE08296892F8FF7A";


            Boolean forbl = true;
            Task<Boolean> t = DataTest();


            Task.Run(async () =>
            {
                Boolean bl = await t;
                forbl = false;
                Thread.Sleep(200);
                Console.SetCursorPosition(0, Console.CursorTop);
                if (bl)
                {


                    Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                    Console.BackgroundColor = ConsoleColor.Green; //设置背景色
                    Log.Debug($"Extension interface initialization is OK");//成功
                    Console.ResetColor(); //将控制台的前景色和背景色设为默认值
                }
                else
                {

                    Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                    Console.BackgroundColor = ConsoleColor.Red; //设置背景色
                    Log.Debug($"Extension interface initialization is error");//成功
                    Console.ResetColor(); //将控制台的前景色和背景色设为默认值

                }
            });

            while (forbl)
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (!forbl)
                        break;
                    Thread.Sleep(100);
                    Log.Debug("                    ");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    string msg = null;
                    for (int a = 0; a < i; a++)
                    {
                        msg += ".";
                    }
                    Log.Debug("Waiting for detection" + msg);
                }
            }
            Thread.Sleep(200);





        }
        private async Task<Boolean> DataTest()
        {
            var list = await Task.Run(() => GTHttp());

            return list;
        }
        private Boolean GTHttp()
        {

            String msg = Post.GTHttpPostRaw("http://172.19.110.65:20433/register/alarmput", out Boolean bl, $"{{\"url\":\"{url}\",\"clientid\":\"{clientid}\",\"platformname\":\"图洋信息科技有限公司\"}}");
            return bl;
        }
    }
}
