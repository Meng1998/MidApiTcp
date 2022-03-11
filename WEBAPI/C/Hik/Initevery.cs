using LMWEBAPI.M;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEBSERVICE.C.Hik;

namespace LMWEBAPI.C.Hik
{
    public class Initevery
    {
        operationKey operatio = new operationKey();
        readonly GetDataSet GetSetData = new GetDataSet();
        public Initevery( )
        {
            Boolean bl = true;
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                    .AddJsonFile("appsettings.json");
            //AppContext.BaseDirectory + "appsettings.json";
            IConfiguration configuration = builder.Build();
            var a = configuration.GetSection("HikDYpams:eventTypes");
            var parm = new
            {
                eventTypes =Array.ConvertAll<String,Int32>( configuration["HikDYpams:eventTypes"].Split(','),s=>int.Parse(s)),
                eventDest = configuration["HikDYpams:eventDest"],
                subType=0,
                eventLvl= Array.ConvertAll<String, Int32>(configuration["HikDYpams:eventLvl"].Split(','), s => int.Parse(s)),
            };
            JToken list = JToken.Parse(JsonConvert.SerializeObject(parm));
            new GetDataSet().InitParameters();
            var KEY = GetSetData.GetISCkey(0, 1);

            _ = operatio.GetKeyHttpPostRaw(GetSetData.StitchingParameters(KEY), KEY, list, out bl, 0);

        }

        public string UrlDonw(String Url)
        {
            Boolean bl = true;
            var parmst = new
            {
                url = Url
            };
            JToken lists = JToken.Parse(JsonConvert.SerializeObject(parmst));
            var KEYs = GetSetData.GetISCkey(1, 1);
            String parameterurl = operatio.GetKeyHttpPostRaw(GetSetData.StitchingParameters(KEYs), KEYs, lists, out bl, 0);
            JObject msg = JsonConvert.DeserializeObject<JObject>(parameterurl);
            return msg["data"].ToString();
        }
    }
    class operationKey
    {

        GetDataSet GetSetData = new GetDataSet();
        /// <summary>
        /// 对key操作
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="key"></param>
        /// <param name="Parameter"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public String GetKeyHttpPostRaw(String Url, SecretKey key, JToken Parameter, out Boolean error, Int32 type)
        {
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data;
            switch (type)
            {
                case 9:
                    Data = Post.HttpPostObject(Url, JsonConvert.SerializeObject(Parameter), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, out error);
                    break;
                default:
                    Data = Post.HIKHttpPostRaw(Url, JsonConvert.SerializeObject(Parameter), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, out error);
                    break;
            }

            if (error)
                return Data;
            else
                return "报错异常 : " + Data;
        }


        /// <summary>  
        /// 本地时间转成GMT时间  
        /// </summary>  
        public static string ToGMTString(DateTime dt)
        {
            return dt.ToUniversalTime().ToString("r");
        }




    }
}
