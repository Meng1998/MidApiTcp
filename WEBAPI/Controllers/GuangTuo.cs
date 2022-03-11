using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
    [Produces("application/json")]
    [Route("GuangTuo")]
    [ApiController]
    public class GuangTuo
    {

        /// <summary>
        /// 对外请求接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Alarm")]
        public JObject RequestMe([FromBody] Object Parameter)
        {
            var Object = new
            {
                MsgType = "GuangTuo",
                Parameter,
                eventType = "Alarm"//事件名称代码
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



        /// <summary>
        ///撤布防
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Defense")]
        public JObject WithDrawDefense([FromBody] FrontEndReception.Ctrl Parameter)
        {
            var Object = new
            {
                MsgType = "GuangTuo",
                Parameter,
                eventType = "Defense"//事件名称代码
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



        /// <summary>
        ///广拓设备状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GuangTuoState")]
        public JObject GuangTuoState([FromBody] Object Parameter)
        {
            var Object = new
            {
                MsgType = "GuangTuo",
                Parameter,
                eventType = "GuangTuoState"//事件名称代码
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
