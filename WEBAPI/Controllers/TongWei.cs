using LMWEBAPI.M;
using Microsoft.AspNetCore.Http;
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
    [Route("api/TongWei")]

    [ApiController]
    public class TongWei : ControllerBase
    {
        /// <summary>
        /// 车辆轨迹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TWVehicle")]
        public JObject TWVehicle(FrontEndReception.Faceattributes Parameter)
        {
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 21,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = "success",
                list = msg
            }));
        }
        /// <summary>
        /// 抓拍库以图搜图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TWSnapshot")]
        public JObject Snapshot(FrontEndReception.Snapshot Parameter)
        {

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 22,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = "success",
                list = msg
            }));
        }
        /// <summary>
        /// 抓拍库人脸属性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TWFaceattributes")]
        public JObject TWFaceattributes(FrontEndReception.Vehiclerecords Parameter)
        {

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 23,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = "success",
                list = msg
            }));
        }

        /// <summary>
        /// 报警接受事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TWAlarmList")]
        public JObject TWAlarmList([FromBody] JObject data)
        {
            try
            {
                TCPoperation.SendAlarmMsg(new
                {
                    MsgType = "GuangTuo",
                    Parameter = new
                    {
                        type = "alarm",
                        data
                    },
                    eventType = "GuangTuoState"//事件名称代码

                });
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "success",
                    list = "执行成功"
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    list = "数据异常"
                }));
            }
           
        }

        /// <summary>
        /// 实时抓拍接受事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TWSnapList")]
        public JObject TWSnapList([FromBody] JObject data)
        {
            try
            {
                Mmtel mmtel= new Mmtel();
                mmtel.Name = "aa";


                TCPoperation.SendAlarmMsg(new
                {
                    MsgType = "GuangTuo",
                    Parameter = new
                    {
                        type = "snap",
                        data
                    },
                    eventType = "GuangTuoState"//事件名称代码

                });
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "success",
                    list = "执行成功"
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    list = "数据异常"
                }));
            }
           
        }

       



    }
}
