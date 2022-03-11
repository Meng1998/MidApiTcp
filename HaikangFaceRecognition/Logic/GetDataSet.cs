using DeploymentTools.Mod;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeploymentTools.Logic
{
    /// <summary>
    /// 集合工具类 对ISC接口操作
    /// </summary>
    public class GetDataSet
    {
        private static List<SecretKey> ISCusermodelList = new List<SecretKey>();

        private static List<SecretKey> SPCCusermodelList = new List<SecretKey>();

        private static String TimeStr = null;//同步时间
        #region 同步时间
        public static string getTimeStr()
        {
            return TimeStr;
        }
        public static void setTimeStr(String time)
        {
            TimeStr = time;
        }
        #endregion
        #region 初始化key
        /// <summary>
        /// 初始化参数
        /// </summary>
        public void InitParameters()
        {
            List<SecretKey> usermodelListKey = new List<SecretKey>();
            SecretKey Key = new SecretKey();
            {
                for (int i = 1; i < 1000; i++)
                {
                    if (Config.GetConfigValue($"ISCKeyList:{i}KeyName") == "STOP")
                    {
                        break;
                    }

                    Key = new SecretKey();
                    Key.Name = Config.GetConfigValue($"ISCKeyList:{i}KeyName");
                    Key.API = Config.GetConfigValue($"ISCKeyList:{i}KeyAPI");
                    Key.Context = Config.GetConfigValue($"ISCKeyList:KeyContext");
                    Key.Port = Int32.Parse(Config.GetConfigValue($"ISCKeyList:KeyPort"));
                    Key.Host = Config.GetConfigValue($"ISCKeyList:KeyHost");
                    Key.appKey = Config.GetConfigValue($"ISCKeyList:KeyappKey");
                    Key.appSecret = Config.GetConfigValue($"ISCKeyList:KeyappSecret");


                    usermodelListKey.Add(Key);
                }

            }//初始化ISCkey
            ISCusermodelList = new List<SecretKey>(usermodelListKey);
            usermodelListKey.Clear();
            {
                for (int i = 1; i < 1000; i++)
                {
                    if (Config.GetConfigValue($"SPCCKeyList:{i}KeyName") == "STOP")
                    {
                        break;
                    }
                    Key = new SecretKey();
                    Key.Name = Config.GetConfigValue($"SPCCKeyList:{i}KeyName");
                    Key.API = Config.GetConfigValue($"SPCCKeyList:{i}KeyAPI");
                    Key.Context = Config.GetConfigValue($"SPCCKeyList:KeyContext");
                    Key.Port = Int32.Parse(Config.GetConfigValue($"SPCCKeyList:KeyPort"));
                    Key.Host = Config.GetConfigValue($"SPCCKeyList:KeyHost");
                    Key.appKey = Config.GetConfigValue($"SPCCKeyList:KeyappKey");
                    Key.appSecret = Config.GetConfigValue($"SPCCKeyList:KeyappSecret");
                    usermodelListKey.Add(Key);
                }

            }//初始化SPCCkey
            SPCCusermodelList = new List<SecretKey>(usermodelListKey);

           
        }
        #endregion

        /// <summary>
        /// 获取ISC密钥
        /// </summary>
        /// <param name="type">获取类型 0是ISC，1是SPCC</param>
        /// <returns></returns>
        public Object KEY(Int32 type)
        {
            switch (type)
            {
                case 0:
                    return ISCusermodelList;
                case 1:
                    return SPCCusermodelList;

                default:
                    return -1;
            }
        }
        /// <summary>
        /// 返回链接字符串
        /// </summary>
        /// <returns></returns>
        public String StitchingParameters(SecretKey key)
        {

            String HostCharacterString = $"{key.Host}:{key.Port}{key.Context}{key.API}";
            return HostCharacterString;
        }
        /// <summary>
        /// 获取ISC密钥
        /// </summary>
        /// <param name="index">获取什么接口的</param>
        /// <param name="type">获取的数据类型</param>
        /// <returns></returns>
        public SecretKey GetISCkey(Int32 index, Int32 type)
        {

            SecretKey key = new SecretKey();

            switch (type)
            {
                case 0:
                    return new SecretKey()
                    {
                        Name = ISCusermodelList[index].Name,
                        Port = ISCusermodelList[index].Port,
                        Host = ISCusermodelList[index].Host,
                        appKey = ISCusermodelList[index].appKey,
                        Context = ISCusermodelList[index].Context,
                        API = ISCusermodelList[index].API,
                        appSecret = ISCusermodelList[index].appSecret

                    };
                case 1:
                    return new SecretKey()
                    {
                        Name = SPCCusermodelList[index].Name,
                        Port = SPCCusermodelList[index].Port,
                        Host = SPCCusermodelList[index].Host,
                        appKey = SPCCusermodelList[index].appKey,
                        Context = SPCCusermodelList[index].Context,
                        API = SPCCusermodelList[index].API,
                        appSecret = SPCCusermodelList[index].appSecret
                    };

            }

            return key;
        }
        /// <summary>
        /// 获取加密密钥
        /// </summary>
        /// <returns></returns>
        public String GetencryptionKey(SecretKey key)
        {
            String Encryptionkey = EncryptionPostData(key.appKey, key.Context, key.API, key.appSecret);

            return Encryptionkey;
        }
        /// <summary>
        /// 海康接口返回状态是否正确
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Boolean GetISCmsgSuccessfulState(String state)
        {
            JObject rbs = JsonConvert.DeserializeObject<JObject>(state);
            state = rbs["msg"].ToString();
            if (state == "success" || state == "SUCCESS" || state == "Operation succeeded")
                return true;
            else
                return false;
        }
        #region Psot 字段加密操作
        /// <summary>
        /// 取到加密秘钥字段
        /// </summary>
        /// <param name="Key">AppKey</param>
        /// <param name="ApiUrl">Api的连接</param>
        /// <param name="appSecret">appSecret</param>
        /// <returns></returns>
        public String EncryptionPostData(String Key, String ApiUrlTop, String ApiUrl, String appSecret)
        {
            TimeStr = DateTime.Now.ToString("R");
            String Host = $"POST\n*/*\napplication/json\n{TimeStr}\nx-ca-key:{Key}\n{ApiUrlTop + ApiUrl}";///artemis/api/resource/v1/regions/subRegions
            String EncryptingKey = CreateToken(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(Host)), appSecret);
            setTimeStr(TimeStr);
            return EncryptingKey;
        }

        /// <summary>
        /// HMACSHA256 =》 Base64
        /// </summary>
        /// <param name="message">内容</param>
        /// <param name="secret">秘钥</param>
        /// <returns></returns>
        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
        #endregion

    }
}
