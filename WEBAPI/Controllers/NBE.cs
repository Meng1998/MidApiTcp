using LMWEBAPI.ClearCache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBAPI.C;
using WEBAPI.M;

namespace WEBAPI.Controllers
{
    /// <summary>
    /// 纽贝尔
    /// </summary>
    [Produces("application/json")]
    [Route("NBE")]
    [ApiController]
    public class NBE : Controller
    {
        /// <summary>
        /// 纽贝尔远程常开关门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OpenDoors")]
        public JObject OpenDoors([FromBody] FrontEndReception.OpenDoors Parameter)
        {
            var Object = new
            {
                MsgType = "NBEWEBAPI",
                Parameter,
                eventType = "OpenDoors"//事件名称代码

            };
            
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 纽贝尔远程开门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OpenDoorsV2")]
        public JObject OpenDoorsV2([FromBody] FrontEndReception.OpenDoorsV2 Parameter)
        {
            var Object = new
            {
                MsgType = "NBEWEBAPI",
                Parameter,
                eventType = "OpenDoorsV2"//事件名称代码

            };
            
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 纽贝尔门状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GateState")]
        public JObject GateState([FromBody] FrontEndReception.GateState Parameter)
        {
            var Object = new
            {
                MsgType = "NBEWEBAPI",
                Parameter,
                eventType = "GateState"//事件名称代码

            };
            
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
    }
}
