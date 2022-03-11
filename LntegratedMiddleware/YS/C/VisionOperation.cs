using LntegratedMiddleware.Hold_All.Encryption;
using LntegratedMiddleware.POST;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LntegratedMiddleware.YS.C
{
    class VisionOperation
    {
        static String AccessCode = null; static String AccessToken = null;

        public VisionOperation()
        {
            //Console.Title = "是否启宇视API服务(YES启动NO不启动).";//设置窗口标题
            Log.Debug($"Test universal vision interface or not(YES/NO):");
            String condition = Console.ReadLine().ToUpper();//接受控制台输入的一个字符串

            if (!String.IsNullOrEmpty(condition) && condition == "YES" || condition == "NO")
                if (condition == "YES" ? !true : !false)
                    return;
                else
                    ;
            else
                return;


            Boolean forbl = true;
            Task<Boolean> t = DataInit();
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
                    Log.Debug($"Login to universal initialization OK AccessToken:[{AccessToken}] AccessCode:[{AccessCode}]");//获取到AccessCode成功
                    Console.ResetColor(); //将控制台的前景色和背景色设为默认值




                }
                else
                {

                    Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                    Console.BackgroundColor = ConsoleColor.Red; //设置背景色
                    Log.Debug($"Login to universal initialization Error!!");//获取到AccessCode成功
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
            Thread.Sleep(300);





        }

        private async Task<Boolean> DataInit()
        {
            var list = await Task.Run(() => YSHttp());
            return list;
        }

        /// <summary>
        /// 拿到登入验证码
        /// </summary>
        /// <returns></returns>
        public String GetAccessToken()
        {
            return AccessToken;
        }
        /// <summary>
        /// 订阅人脸报警信息
        /// </summary>
        /// <param name="Url">订阅地址</param>
        /// <param name="Type">订阅类型</param>
        /// <returns></returns>
        public static String AlarmSubscription(String Url, Int32 Type = 999)
        {
            JObject rb = JsonConvert.DeserializeObject<JObject>(AccessToken ?? "{\"AccessToken\":\"\"}");
            //0为普通告警，1为人脸布控告警，2为人脸抓拍信息上报，3为过车告警。如果请求参数中不包含该字段默认打开所有告警
            Object setmsg;
            if (Type != 999)
            {
                setmsg = new
                {
                    Data = Url,
                    Type
                };
            }
            else
            {
                setmsg = new
                {
                    Data = Url,
                };
            }
            String msg = Post.YSHttpRaw("http://172.19.1.1:8088/VIID/alarm/open", out Boolean bl, JsonConvert.SerializeObject(setmsg), rb["AccessToken"].ToString(), Method.POST);
            Log.Debug($"AlarmSubscription:{msg}" + Environment.NewLine);//控制
            //返回
            if (bl)
                return msg;
            else
                return JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    MsgTxt = "服务器未响应 或返回为空"
                });

        }
        /// <summary>
        /// 控制云台
        /// </summary>
        /// <param name="EquipmentCode">摄像机编码</param>
        /// <param name="PTZCmdPara1">摄像机转速</param>
        /// <param name="PTZCmdID">摄像机执行命令类型</param>
        public static Boolean ControlCloudPlatform(String EquipmentCode, Int32 PTZCmdID, Int32 PTZCmdPara1 = 2)
        {
            JObject rb = JsonConvert.DeserializeObject<JObject>(AccessToken ?? "{\"AccessToken\":\"\"}");
            //http://server-addr:8088/VIID/ptz/release/{code} 
            var msg = new
            {
                PTZCmdID,//命令类型
                PTZCmdPara1//转速
            };
            #region 命令类型
            //0x0101： 光圈关停止
            //0x0102： 光圈关
            //0x0103： 光圈开停止
            //0x0104： 光圈开
            //0x0201： 近聚集停止
            //0x0202： 近聚集
            //0x0203： 远聚集 停止
            //0x0204： 远聚集
            //0x0301： 放大停止
            //0x0302： 放大
            //0x0303： 缩小停止
            //0x0304： 缩小
            //0x0401： 向上停止
            //0x0402： 向上
            //0x0403： 向下停止
            //0x0404： 向下
            //0x0501： 向右停止
            //0x0502： 向右
            //0x0503： 向左停止
            //0x0504： 向左
            //0x0601： 预置位保存
            //0x0602： 预置位调用
            //0x0603： 预置位删除
            //0x0701： 左上停止
            //0x0702： 左上
            //0x0703： 左下停止
            //0x0704： 左下
            //0x0801： 右上停止
            //0x0802： 右上
            //0x0803： 右下停止
            //0x0804： 右下
            //0x0901： PTZ 全停命令字
            //0x0907： FI 全停命令字
            //0x0902： 绝对移动
            //0x0A01： 雨刷开IMOS Restful 基础业务
            #endregion
            Log.Debug("ControlCloudPlatform:" + Post.YSHttpRaw("http://172.19.1.1:8088/VIID/ptz/ctrl/" + $"{EquipmentCode}", out Boolean bl, JsonConvert.SerializeObject(msg), rb["AccessToken"].ToString(), Method.POST) + Environment.NewLine);//控制
            return bl;
        }
        /// <summary>
        /// 外部获取宇视设备列表
        /// </summary>
        /// <returns></returns>
        public static String GETEquipmentList()
        {
            String Json = "{\"ItemNum\":3,\"Condition\":[{\"QueryType\":256,\"LogicFlag\":0,\"QueryData\":\"1001\"},{\"QueryType\":11,\"LogicFlag\":6,\"QueryData\":\"1001\"},{\"QueryType\":257,\"LogicFlag\":6,\"QueryData\":\"1001\"}],\"QueryCount\":1,\"PageFirstRowNumber\":0,\"PageRowNum\":200}";
            JObject rb = JsonConvert.DeserializeObject<JObject>(AccessToken ?? "{\"AccessToken\":\"\"}");
            String msg = Post.YSHttpRaw("http://172.19.1.1:8088/VIID/query?" + $"org={"iccsid"}&" + $"condition={Json}", out Boolean bl, "", rb["AccessToken"].ToString(), Method.GET);
            if (bl)
            {
                Log.Debug("EquipmentList：" + msg);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Red; //设置背景色
                Log.Debug("EquipmentList：error 404 or 500");
                Console.ResetColor(); //将控制台的前景色和背景色设为默认值
            }
            return msg;
        }
        /// <summary>
        /// 外部获取宇视区域列表
        /// </summary>
        /// <returns></returns>
        public static String GETRegionList()
        {
            String Json = "{\"ItemNum\":3,\"Condition\":[{\"QueryType\":256,\"LogicFlag\":0,\"QueryData\":\"1\"},{\"QueryType\":11,\"LogicFlag\":6,\"QueryData\":\"0\"},{\"QueryType\":1,\"LogicFlag\":6,\"QueryData\":\"0\"}],\"QueryCount\":1,\"PageFirstRowNumber\":0,\"PageRowNum\":200}";
            JObject rb = JsonConvert.DeserializeObject<JObject>(AccessToken ?? "{\"AccessToken\":\"\"}");
            String msg = Post.YSHttpRaw("http://172.19.1.1:8088/VIID/query?" + $"org={"iccsid"}&" + $"condition={Json}", out Boolean bl, "", rb["AccessToken"].ToString(), Method.GET);
            if (bl)
            {
                Log.Debug("RegionList：" + msg);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White; //设置前景色，即字体颜色
                Console.BackgroundColor = ConsoleColor.Red; //设置背景色
                Log.Debug("EquipmentList：error 404 or 500");
                Console.ResetColor(); //将控制台的前景色和背景色设为默认值
                var err = new
                {
                    msg = "error",
                    MsgTxt = "HTTP 请求错误 无法连接远程服务器或返回值为空!"
                };

                return JsonConvert.SerializeObject(err);
            }
            return msg;
        }
        private Boolean YSHttp()
        {
            AccessCode = Post.YSHttpRaw("http://172.19.1.1:8088/VIID/login", out Boolean bl, "");
            if (bl)
            {
                JObject rb = JsonConvert.DeserializeObject<JObject>(AccessCode);
                String CreateToken = Encryption.GetMD5(Encryption.CreateBase64("loadmin") + rb["AccessCode"].ToString() + Encryption.GetMD5("Admin_123"));
                // MD5(BASE64(UserName)+ AccessCode + MD5(用户密码) )
                String PostJson = M.PostJson.Login(new M.PostJson.LoingClss
                {
                    UserName = "loadmin",
                    AccessCode = rb["AccessCode"].ToString(),
                    LoginSignature = CreateToken,
                });
                AccessToken = Post.YSHttpRaw("http://172.19.1.1:8088/VIID/login", out bl, PostJson);
            }


            return bl;
        }
    }
}
