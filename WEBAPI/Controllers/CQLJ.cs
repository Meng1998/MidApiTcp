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
    [Route("CQLJ")]
    [ApiController]
    public class CQLJ : ControllerBase
    {
        /// <summary>
        /// 报警接收
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQAlarmData")]
        public JObject CQAlarmData([FromBody] JObject Parameter)
        {

            try
            {
                if (Parameter["interfaceCode"].ToString() == "EVENT_DEVICE_ALARM_RECORD")
                {
                    Console.WriteLine("");
                }
                else
                {
                    //Parameter["interfaceCode"] = "人脸报警";
                }
                TCPoperation.SendAlarmMsg(new
                {
                    MsgType = "GuangTuo",
                    Parameter,
                    eventType = "GuangTuoState"//事件名称代码

                });
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = "执行成功。"
                }));
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = "执行成功。"
                }));
            }
        }


        /// <summary>
        /// 报警接收测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQAlarmDataCS")]
        public JObject CQAlarmDataCS([FromBody] JObject Parameter)
        {

            try
            {
                TCPoperation.SendAlarmMsg(new
                {
                    MsgType = "GuangTuo",
                    Parameter,
                    eventType = "GuangTuoState"//事件名称代码

                });
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = "执行成功。"
                }));
            }
            catch (Exception ex)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "Success",
                    MsgTxt = "执行成功。"
                }));
            }
        }

        /// <summary>
        /// 门禁控制-开
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQOpendoor")]
        public JObject CQOpendoor(FrontEndReception.DHdoor Parameter)
        {
            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                Index = 0,
                Restype = 1,
                eventType = "H89Door"//事件名称代码

            }));
        }

        /// <summary>
        /// 门禁控制-关
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQClosedoor")]
        public JObject CQClosedoor(FrontEndReception.DHdoor Parameter)
        {
            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                Index = 1,
                Restype = 1,
                eventType = "H89Door"//事件名称代码

            }));
        }

        /// <summary>
        /// 门禁状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQStatedoor")]
        public JObject CQStatedoor(FrontEndReception.DHdoorstate Parameter)
        {
            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                Index = 2,
                Restype = 1,
                eventType = "H89Door"//事件名称代码

            }));
        }

        /// <summary>
        /// 门禁记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQRecorddoor")]
        public JObject CQRecorddoor(FrontEndReception.DHdoorrecord Parameter)
        {
            //JObject msg = JsonConvert.DeserializeObject<JObject>();
            Parameter.channelCode = "ACC_" + Parameter.channelCode;
            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                Index = 3,
                Restype = 1,
                eventType = "H89Door"//事件名称代码

            }));
        }

        /// <summary>
        /// 卡点过车记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQPassing")]
        public JObject CQPassing(FrontEndReception.DHPassing Parameter)
        {
            //JObject msg = JsonConvert.DeserializeObject<JObject>();
            try
            {
                return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
                {
                    MsgType = "DHWEBAPI",
                    Parameter,
                    Index = 4,
                    Restype = 3,
                    eventType = "H89Door"//事件名称代码

                }));

            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg="error",
                    MsgTxt= "未与服务正常连接！"
                }));
            }
        }

        /// <summary>
        /// 人脸轨迹查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQFacerecording")]
        public JObject CQFacerecording(FrontEndReception.DHFace Parameter)
        {
            //JObject msg = JsonConvert.DeserializeObject<JObject>();

            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                Index = 5,
                Restype = 2,
                eventType = "H89Door"//事件名称代码

            }));
        }

        /// <summary>
        /// 超速报警
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CQspeedAlarm")]
        public JObject CQspeedAlarm(FrontEndReception.speeding Parameter)
        {
            //JObject msg = JsonConvert.DeserializeObject<JObject>();

            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
                Parameter,
                Index = 6,
                Restype = 2,
                eventType = "H89Door"//事件名称代码

            }));
        }

        /// <summary>
        /// 卡口设备查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CQdeviceList")]
        public JObject CQdeviceList()
        {
            //JObject msg = JsonConvert.DeserializeObject<JObject>();

            return JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "DHWEBAPI",
               
                eventType = "list"//事件名称代码

            }));
        }

    }
}
