using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBAPI.C;
using WEBAPI.M;



namespace LMWEBAPI.Controllers
{
    /// <summary>
    /// 大华
    /// </summary>
    [Produces("application/json")]
    [Route("DH")]
    [ApiController]
    public class DH : Controller
    {
        /// <summary>
        /// 大华获取相机客流量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EGTTotal")]
        public JObject EGTTotal([FromBody] FrontEndReception.DHTotal Parameter)
        {
            var Object = new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                eventType = "EGTTotal"//事件名称代码

            };
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));

            return msg;
        }

        [HttpPost]
        [Route("Alarm")]
        public JObject Alarm([FromBody] Object Parameter)
        {
            var Object = new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                eventType = "RequestMe"//事件名称代码
            };
            try
            {
                _Log.WriteLog("对外接口消息", Parameter.ToString() + System.Environment.NewLine);
            }
            catch (Exception)
            {
            }

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));

            return msg;
        }
    }
}