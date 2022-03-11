using LMWEBAPI.ClearCache;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBAPI.C;
using WEBAPI.M;
using WEBSERVICE.C;

namespace WEBAPI.Controllers
{
    /// <summary>
    /// 宇视平台
    /// </summary>
    //[EnableCors("any")]
    [Produces("application/json")]
    [Route("YS")]
    [ApiController]
    public class YS : Controller
    {

        /// <summary>
        /// 对外请求接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestMe")]
        public JObject RequestMe([FromBody] Object Parameter)
        {
            var Object = new
            {
                MsgType = "YSWEBAPI",
                Parameter,
                eventType = "RequestMe"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            try
            {
                _Log.WriteLog("对外接口消息", Parameter.ToString() + System.Environment.NewLine);
            }
            catch (Exception)
            {


            }
            
            return msg;
        }

        /// <summary>
        /// 对外请求接口测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("RequestMeV2")]
        public JObject RequestMeV2([FromBody] Object Parameter)
        {
            var Object = new
            {
                MsgType = "YSWEBAPI",
                Parameter,
                eventType = "RequestMe"//事件名称代码
            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            try
            {
                _Log.WriteLog("对外接口消息", Parameter.ToString() + System.Environment.NewLine);
            }
            catch (Exception)
            {


            }
            
            return msg;
        }

        /// <summary>
        /// 宇视云台控制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ControlCloudPlatform")]
        public JObject ControlCloudPlatform([FromBody] FrontEndReception.PanTiltControl Parameter)
        {
            var Object = new
            {
                MsgType = "YSWEBAPI",
                Parameter,
                eventType = "ControlCloudPlatform"//事件名称代码
                
            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }

        /// <summary>
        /// 注册回调事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AlarmSubscription")]
        public JObject AlarmSubscription([FromBody] FrontEndReception.AlarmSubscription Parameter)
        {
            var Object = new 
            {
                MsgType = "YSWEBAPI",
                Parameter,
                eventType = "AlarmSubscription"//事件名称代码

            };
            
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }


        /// <summary>
        /// 查询宇视区域列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAreaList")]
        public JObject GetAreaList()
        {
            var Object = new
            {
                MsgType = "YSWEBAPI",
                Forward = true,
                eventType = "GetAreaList"//事件名称代码
            };
            
            String str = TCPoperation.SendMsg(Object);
            JObject msg = JsonConvert.DeserializeObject<JObject>(str);
            return msg;
        }
        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GETEquipmentList")]
        public JObject GETEquipmentList()
        {
            var Object = new
            {
                MsgType = "YSWEBAPI",
                Forward = true,
                eventType = "GETEquipmentList"//事件名称代码
            };
            
            String str = TCPoperation.SendMsg(Object);
            JObject msg = JsonConvert.DeserializeObject<JObject>(str);
            return msg;
        }
    }
}
