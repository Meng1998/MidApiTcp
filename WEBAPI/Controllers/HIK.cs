using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LMWEBAPI.C.Hik;
using LMWEBAPI.ClearCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using WEBAPI.C;
using WEBAPI.M;
using static WEBAPI.M.GetJson;

namespace WEBAPI.Controllers
{
    /// <summary>
    /// ISC平台操作类
    /// </summary>
    /// [Produces("application/json")]
    [Produces("application/json")]
    [Route("ISC")]
    [ApiController]
    public class ISC : ControllerBase
    {
        #region POST
        /// <summary>
        /// 门禁记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DoorEvent")]
        public JObject DoorEvent(FrontEndReception.DoorEvent Parameter)
        {
            Parameter.startTime = DateTime.Parse(Parameter.startTime).ToString("s") + "+08:00";
            Parameter.endTime = DateTime.Parse(Parameter.endTime).ToString("s") + "+08:00";
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 26,
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
        /// 过车记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Vehiclerecords")]
        public JObject Vehiclerecords(FrontEndReception.Vehiclerecords Parameter)
        {
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 25,
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
        /// 开关闸机
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("deviceControl")]
        public JObject deviceControl(FrontEndReception.deviceControl Parameter)
        {
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 20,
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
        /// 防区旁路控制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ByPassDefenceArea")]
        public JObject ByPassDefenceArea(FrontEndReception.defence Parameter)
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
        /// 门禁刷卡人员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CardInfo")]
        public JObject CardInfo(FrontEndReception.CardInfo Parameter)
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
        /// 防区状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DefenceAreaState")]
        public JObject DefenceAreaState(FrontEndReception.DefenceAreaState Parameter)
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
        /// 编码设备列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEquipmentList")]//发现了在 2008 r2中的数据返回缺失字符串被截断，取消由API控制返回全部数据，改为前端控制页数 分页方式获取资源
        public JObject GetEquipmentList(FrontEndReception.Tree Parameter)
        {

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 12,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    total = msg["data"]["total"],
                    msg = "success",
                    list = msg["data"]["list"]
                }));
            else
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
        }

        /// <summary>
        /// 编码设备列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEquipmentLisst")]//发现了在 2008 r2中的数据返回缺失字符串被截断，取消由API控制返回全部数据，改为前端控制页数 分页方式获取资源
        public JObject GetEquipmentLisst(FrontEndReception.Tree Parameter)
        {

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 12,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    total = msg["data"]["total"],
                    msg = "success",
                    list = msg["data"]["list"]
                }));
            else
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
        }


        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPagingAreaData")]//发现了在 2008 r2中的数据返回缺失字符串被截断，取消由API控制返回全部数据，改为前端控制页数 分页方式获取资源
        public JObject GetPagingAreaData(FrontEndReception.Tree Parameter)
        {
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 19,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    total = msg["data"]["total"],
                    msg = "success",
                    list = msg["data"]["list"]
                }));
            else
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
        }
        /// <summary>
        /// 按条件查询识别资源
        /// </summary>
        /// <param name="Parameter">数据接收必要参数，具体参数名称请查看文档</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetConditionRootregionData")]
        public JObject GetConditionRootregionData([FromBody] FrontEndReception.GetConditionRootregionData Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 0,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 一对多人脸搜索
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Contrast_1VN")]
        public JObject Contrast_1VN([FromBody] FrontEndReception.Contrast_1VN Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 1,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 根据条件查询人脸库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("FaceDatabase")]
        public JObject FaceDatabase([FromBody] FrontEndReception.FaceDatabase Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 6,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码


            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 以图搜图V2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GoogleSearchImageV2")]
        public JObject GoogleSearchImageV2([FromBody] FrontEndReception.SearchObject Parameter)
        {
            var fff = "." + DateTime.Now.ToString("fff");
            Parameter.startTime = DateTime.Parse(Parameter.startTime).ToString("s") + fff + "+08:00";
            Parameter.endTime = DateTime.Parse(Parameter.endTime).ToString("s") + fff + "+08:00";
            JObject rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 5,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码
            }));

            if (HIK.GetmsgSuccessfulState(rb["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "success",
                    Remarks = rb
                }));
            else
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = rb
                }));
        }
        /// <summary>
        /// 以图搜图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GoogleSearchImage")]
        public JObject GoogleSearchImage([FromBody] FrontEndReception.SearchObject Parameter)
        {

            Parameter.startTime = DateTime.Parse(Parameter.startTime).GetDateTimeFormats('s')[0].ToString() + ".000+08:00";
            Parameter.endTime = DateTime.Parse(Parameter.startTime).GetDateTimeFormats('s')[0].ToString() + ".000+08:00";
            JObject rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 5,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            }));

            if (HIK.GetmsgSuccessfulState(rb["msg"].ToString()))
            {

                List<String[]> SortSet = new List<String[]>();//作为按时间排序的集合 正序 
                int i = 0;
                PGDataProcessing PGdata = new PGDataProcessing();
                GetJson jsonStr = new GetJson();
                //相机名称                       
                String DataTemporary = ""; String X = ""; String Y = "";
                List<object> temporary = new List<object>();
                foreach (var item in rb["data"]["list"])
                {
                    JObject items = JsonConvert.DeserializeObject<JObject>(item.ToString());
                    Boolean PGdataError = true;//数据库错误
                    DataSet dt = null;
                    try
                    {
                        dt = PGdata.ExecuteQuery($"SELECT * FROM \"map_device\" WHERE device_code='{items["cameraIndexCode"].ToString()}'", out PGdataError);
                    }
                    catch (Exception ex)
                    {
                        _Log.WriteLog("DB", ex.Message);
                    }
                    //取数据库中值到json文件
                    if (PGdataError && dt.Tables[0].Rows.Count > 0)
                        DataTemporary = dt.Tables[0].Rows[0]["device_name"].ToString();
                    else
                        continue;
                    PGdataError = true;//数据库错误
                    DataSet map_deviceDt = PGdata.ExecuteQuery($"SELECT * FROM \"map_device\" WHERE device_code='{items["cameraIndexCode"].ToString()}'", out PGdataError);
                    if (PGdataError && map_deviceDt.Tables[0].Rows.Count > 0)
                    {
                        String json = map_deviceDt.Tables[0].Rows[0]["jsondata"].ToString();
                        JObject jsondata = JsonConvert.DeserializeObject<JObject>(json);
                        X = jsondata["position"]["x"].ToString();
                        Y = jsondata["position"]["y"].ToString();
                    }
                    else
                        continue;

                    SortSet.Add(new string[] { items["captureTime"].ToString(), i.ToString() }); i++;

                    temporary.Add(new SearchSubObject()
                    {
                        cameraIndexCode = items["cameraIndexCode"].ToString(),
                        cameraName = DataTemporary,
                        captureTime = items["captureTime"].ToString(),
                        sex = items["sex"].ToString(),
                        ageGroup = items["ageGroup"].ToString(),
                        withGlass = items["withGlass"].ToString(),
                        similarity = items["similarity"].ToString(),
                        bkgPicUrl = items["bkgPicUrl"].ToString(),
                        facePicUrl = items["facePicUrl"].ToString(),
                        X = X,
                        Y = Y,
                        rect = items["rect"].ToString()
                    });
                }

                var result = SortSet.OrderBy(r => DateTime.Parse(r[0])).ToList();//排序
                Object[] Jsons = new Object[temporary.ToArray().Length];//子对象
                for (int Sort = 0; Sort < Jsons.Length; Sort++)//排序
                {
                    Jsons[Sort] = temporary[Int32.Parse(result[Sort][1].ToString())];
                }

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(jsonStr.JOSNPOST_GoogleSearchImage((Int32)rb["data"]["pageSize"], (Int32)rb["data"]["total"], (Int32)rb["data"]["pageNo"], Jsons)));
            }
            else
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = rb
                }));
            }
        }
        /// <summary>
        /// 从服务器下载图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPicturUrl")]
        public String GetPicturUrl([FromBody] FrontEndReception.GetPicturUrl Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 9,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            return TCPoperation.SendMsg((Object));
        }
        /// <summary>
        /// 获取编码设备状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("CodingEquipmentStatus")]
        public JObject CodingEquipmentStatus([FromBody] FrontEndReception.DeviceStatus Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 11,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 获取监控点状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryCameraStatus")]
        public JObject QueryCameraStatus([FromBody] FrontEndReception.DeviceStatus Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 13,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 获取门禁设备状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAccessEquipmentStatus")]
        public JObject GetAccessEquipmentStatus([FromBody] FrontEndReception.DeviceStatus Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 16,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }

        /// <summary>
        /// 获取门禁点状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAccessPointsStatus")]
        public JObject GetAccessPointsStatus([FromBody] FrontEndReception.DeviceStatus Parameter)
        {
            //var Object = new
            //{
            //    MsgType = "HIKWEBAPI",
            //    GetKEYIndex = 16,
            //    Parameter,
            //    eventType = "ISCWEBAPI"//事件名称代码

            //}; TCPoperation.SendMsg((Object)

            JObject msg = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new { msg = "封存待商议" }));
            return msg;
        }
        /// <summary>
        /// 获取门禁状态（是否开关门）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DoorStatus")]
        public JObject DoorStatus([FromBody] FrontEndReception.QueryAccessPointsStatus Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 17,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 门禁点反控（开关门操作）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OpenCloseDoor")]
        public JObject AccessPointCountercontrol([FromBody] FrontEndReception.AccessControl Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 18,
                Parameter,
                eventType = "ISCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        #endregion

        #region GET

        /// <summary>
        /// 全部车辆列表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleInformation")]
        public JObject VehicleInformation()
        {
            List<Object> list = new List<Object>();
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 7,
                Parameter = new FrontEndReception.Tree
                {
                    pageNo = 1,
                    pageSize = 1000
                },
                eventType = "ISCWEBAPI"//事件名称代码

            }));
            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
            {
                JObject rb = msg;
                list.Add(rb["data"]["list"]);//追加第一页
                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));//分页
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)//循环追加
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((new
                        {
                            MsgType = "HIKWEBAPI",
                            GetKEYIndex = 7,
                            Parameter = new FrontEndReception.Tree
                            {
                                pageNo = i,
                                pageSize = 1000
                            },
                            eventType = "ISCWEBAPI"//事件名称代码

                        })));

                        if (!HIK.GetmsgSuccessfulState(msg["msg"].ToString()))//强行中断循环返回报错
                        {
                            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                            {
                                msg = "error",
                                Remarks = rb
                            }));
                        }

                        foreach (var item in rb["data"]["list"])
                        {
                            list.Add(item);
                        }
                    }
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    list = list.ToArray()
                }));
            }
            else
            {

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
            }
        }
        /// <summary>
        /// 全部布控车辆信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VehicleBlacklist")]
        public JObject VehicleBlacklist()
        {
            List<Object> list = new List<Object>();
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 8,
                Parameter = new FrontEndReception.Tree
                {
                    pageNo = 1,
                    pageSize = 1000
                },
                eventType = "ISCWEBAPI"//事件名称代码

            }));
            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
            {
                JObject rb = msg;
                list.Add(rb["data"]["list"]);//追加第一页
                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));//分页
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)//循环追加
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((new
                        {
                            MsgType = "HIKWEBAPI",
                            GetKEYIndex = 8,
                            Parameter = new FrontEndReception.Tree
                            {
                                pageNo = i,
                                pageSize = 1000
                            },
                            eventType = "ISCWEBAPI"//事件名称代码

                        })));

                        if (!HIK.GetmsgSuccessfulState(msg["msg"].ToString()))//强行中断循环返回报错
                        {
                            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                            {
                                msg = "error",
                                Remarks = rb
                            }));
                        }

                        foreach (var item in rb["data"]["list"])
                        {
                            list.Add(item);
                        }
                    }
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    list = list.ToArray()
                }));
            }
            else
            {

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
            }
        }

        /// <summary>
        /// 门禁点列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccessPoints")]
        public JObject GetAccessPoints()
        {
            List<Object> list = new List<Object>();
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 15,
                Parameter = new FrontEndReception.Tree
                {
                    pageNo = 1,
                    pageSize = 1000
                },
                eventType = "ISCWEBAPI"//事件名称代码

            }));
            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
            {
                JObject rb = msg;
                list.Add(rb["data"]["list"]);//追加第一页
                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));//分页
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)//循环追加
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((new
                        {
                            MsgType = "HIKWEBAPI",
                            GetKEYIndex = 15,
                            Parameter = new FrontEndReception.Tree
                            {
                                pageNo = i,
                                pageSize = 1000
                            },
                            eventType = "ISCWEBAPI"//事件名称代码

                        })));

                        if (!HIK.GetmsgSuccessfulState(msg["msg"].ToString()))//强行中断循环返回报错
                        {
                            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                            {
                                msg = "error",
                                Remarks = rb
                            }));
                        }

                        foreach (var item in rb["data"]["list"])
                        {
                            list.Add(item);
                        }
                    }
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    list = list.ToArray()
                }));
            }
            else
            {

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
            }
        }
        /// <summary>
        /// 获取门禁设备
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccessControlEquipment")]
        public JObject GetAccessControlEquipment()
        {
            List<Object> list = new List<Object>();
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 14,
                Parameter = new FrontEndReception.Tree
                {
                    pageNo = 1,
                    pageSize = 1000
                },
                eventType = "ISCWEBAPI"//事件名称代码

            }));
            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
            {
                JObject rb = msg;
                list.Add(rb["data"]["list"]);//追加第一页
                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));//分页
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)//循环追加
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((new
                        {
                            MsgType = "HIKWEBAPI",
                            GetKEYIndex = 14,
                            Parameter = new FrontEndReception.Tree
                            {
                                pageNo = i,
                                pageSize = 1000
                            },
                            eventType = "ISCWEBAPI"//事件名称代码

                        })));

                        if (!HIK.GetmsgSuccessfulState(msg["msg"].ToString()))//强行中断循环返回报错
                        {
                            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                            {
                                msg = "error",
                                Remarks = rb
                            }));
                        }

                        foreach (var item in rb["data"]["list"])
                        {
                            list.Add(item);
                        }
                    }
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    list = list.ToArray()
                }));
            }
            else
            {

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
            }
        }
        /// <summary>
        /// 单兵设备列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IndividualSoldierEquipmentList")]
        public JObject IndividualSoldierEquipmentList()
        {
            List<Object> list = new List<Object>();
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 12,
                Parameter = new FrontEndReception.Tree
                {
                    pageNo = 1,
                    pageSize = 1000
                },
                eventType = "ISCWEBAPI"//事件名称代码

            }));
            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
            {
                JObject rb = msg;
                foreach (var item in rb["data"]["list"])
                {
                    if (item["treatyType"].ToString() == "ehome_reg")//判断有协议是否为单兵数据
                    {
                        list.Add(item);
                    }
                }

                //处理设备超出通用操作，开始追加数据
                String total = rb["data"]["total"].ToString();//取出最大数
                Int32 page = (int)Math.Ceiling(Convert.ToDouble(Int32.Parse(total)) / Convert.ToDouble(1000));//分页
                if (page > 1)
                {
                    for (int i = 2; i < page + 1; i++)//循环追加
                    {
                        rb = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((new
                        {
                            MsgType = "HIKWEBAPI",
                            GetKEYIndex = 12,
                            Parameter = new FrontEndReception.Tree
                            {
                                pageNo = i,
                                pageSize = 1000
                            },
                            eventType = "ISCWEBAPI"//事件名称代码

                        })));

                        if (!HIK.GetmsgSuccessfulState(msg["msg"].ToString()))//强行中断循环返回报错
                        {
                            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                            {
                                msg = "error",
                                Remarks = rb
                            }));
                        }

                        foreach (var item in rb["data"]["list"])
                        {
                            if (item["treatyType"].ToString() == "ehome_reg")//判断有协议是否为单兵数据
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    list = list.ToArray()
                }));
            }
            else
            {

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
            }
        }
        #endregion
    }

    /// <summary>
    /// SPCC平台操作类
    /// </summary>
    [Produces("application/json")]
    [Route("SPCC")]
    [ApiController]
    public class SPCC : ControllerBase
    {
        #region POST

        /// <summary>
        /// 获取门禁列表
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("queryDoorChannel")]
        public JObject queryDoorChannel([FromBody] FrontEndReception.DoorChannel Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 5,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            };

            string a = JsonConvert.SerializeObject(Object);
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }

        /// <summary>
        /// 车辆检测接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("VehicleCheck")]
        public JObject VehicleCheck([FromBody] FrontEndReception.SPCC_VehicleCheck Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 6,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            };

            string a = JsonConvert.SerializeObject(Object);
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }






        /// <summary>
        /// 订阅事件(按事件类型获取事件订阅信息)
        /// </summary>
        /// <param name="Parameter">数据接收必要参数，具体参数名称请查看文档</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSubscriptionInformation")]
        public JObject GetSubscriptionInformation([FromBody] FrontEndReception.GetSubscriptionInformation Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 0,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }

        /// <summary>
        /// 获取监控点视频流
        /// </summary>
        /// <param name="Parameter">数据接收必要参数，具体参数名称请查看文档</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetVideoStream")]
        public Object GetVideoStream([FromBody] FrontEndReception.GetVideoStreamSet Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 2,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            };

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 获取监视点资源(相机)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetMonitoringResources")]//发现了在 2008 r2中的数据返回缺失字符串被截断，取消由API控制返回全部数据，改为前端控制页数 分页方式获取资源
        public JObject GetMonitoringResources(FrontEndReception.Tree Parameter)
        {

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 1,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            }));

            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    total = msg["data"]["total"],
                    msg = "success",
                    list = msg["data"]["list"]
                }));
            else
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
        }

        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPagingAreaData")]//发现了在 2008 r2中的数据返回缺失字符串被截断，取消由API控制返回全部数据，改为前端控制页数 分页方式获取资源
        public JObject GetPagingAreaData(FrontEndReception.Tree Parameter)
        {

            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg(new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 3,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            }));

            if (HIK.GetmsgSuccessfulState(msg["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    total = msg["data"]["total"],
                    msg = "success",
                    list = msg["data"]["list"]
                }));
            else
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = msg
                }));
        }



        /// <summary>
        /// 门禁反控
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("doControl")]
        public JObject doControl([FromBody] FrontEndReception.AccessControl Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 6,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            };

            string a = JsonConvert.SerializeObject(Object);
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        #endregion
        #region GET
        /// <summary>
        /// 查询人脸组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("FaceGrouping")]
        public JObject FaceGrouping()
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 4,
                Parameter = new { },
                eventType = "ISCWEBAPI"//事件名称代码

            };
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }

        #endregion

        #region GET
        /// <summary>
        /// 查询门禁状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DoorState")]
        public JObject DoorState([FromBody] FrontEndReception.Statecode Parameter)
        {
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 5,
                Parameter,
                eventType = "SPCCWEBAPI"//事件名称代码

            };
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }
        /// <summary>
        /// 查询门禁进出记录
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DoorList")]
        public JObject DoorList([FromBody] FrontEndReception.Statecode Parameter)
        {
            string fff = "." + DateTime.Now.ToString("fff");

            FrontEndReception.Doorrecord doorrecord = new FrontEndReception.Doorrecord();
            doorrecord.startTime = DateTime.Now.AddDays(-7).ToString("s") + "+08:00";
            doorrecord.endTime = DateTime.Now.ToString("s") + "+08:00";
            doorrecord.pageNo = 1;
            doorrecord.pageSize = 200;
            doorrecord.doorIndexCodes = Parameter.doorIndexCodes;
            var Object = new
            {
                MsgType = "HIKWEBAPI",
                GetKEYIndex = 4,
                Parameter = doorrecord,
                eventType = "SPCCWEBAPI"//事件名称代码

            };
            JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            return msg;
        }

        #endregion
    }
    /// <summary>
    /// 当前类数据操作方法
    /// </summary>
    public class HIK : ControllerBase
    {
        /// <summary>
        /// 海康接口返回状态是否正确
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Boolean GetmsgSuccessfulState(String state)
        {

            if (state == "success" || state == "SUCCESS" || state == "Operation succeeded")
                return true;
            else
                return false;
        }
        /// <summary>
        /// 同步ISC摄像机设备列表到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SynchronousISC")]
        public JObject SynchronousISC([FromBody] FrontEndReception.SynchronousISC_SPCC Parameter)
        {
            PGDataProcessing PGSQLProcessing = new PGDataProcessing();
            JObject JsonData = new ISC().GetEquipmentList(new FrontEndReception.Tree
            {
                pageNo = 1,
                pageSize = 100
            });

            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = "接口返回错误，或无网络连接，或管理员没有启动服务."
                }));

            Int32 total = Int32.Parse(JsonData["total"].ToString());//取出最大数
            Int32 page = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(100));//分页
            List<JToken> list = new List<JToken>();
            foreach (var item in JsonData["list"])
            {
                list.Add(item);
            }
            for (int pags = 2; pags < page + 1; pags++)//循环追加
            {
                JsonData = new ISC().GetEquipmentList(new FrontEndReception.Tree
                {
                    pageNo = pags,
                    pageSize = 100
                });

                if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = "error",
                        Remarks = "接口循环获取数据被强行中断，因为中途接口有错误！"
                    }));
                else
                    foreach (var item in JsonData["list"])
                    {
                        list.Add(item);
                    }

            }
            List<String> EquipmentPageIndex = new List<String>();
            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = ("error"),
                    MsgTxt = ("执行失败"),
                    Data = JsonData
                }));

            bool error;
            {
                PGSQLProcessing.ExecuteQuery($"DELETE FROM dock_device WHERE on_map = false and device_type='{Parameter.EquipmentType}'", out error);
                DataSet EPage = PGSQLProcessing.ExecuteQuery("SELECT * FROM dock_device", out error);
                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "SynchronousISC_SPCC接口删除数据或查询数据时出现意想不到的错误，请开发人员核查api{SynchronousISC_SPCC}接口处理问题！"),
                    }));
                //-------------------------------------------------------------------------------------
                //存入设备唯一标识码 后期跳过
                for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                {
                    EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                }
            }
            foreach (var item in list)
            {

                var Toadd = new PGAddequipmentDataMod()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Type = Parameter.EquipmentType,
                    Code = item["cameraIndexCode"].ToString(),
                    Name = item["cameraName"].ToString(),
                    Iregion_id = Parameter.ManufacturerType,
                    Info = item.ToString(),
                    Map = "false"
                };
                Boolean whether = true;//是否上图
                foreach (var IndexPage in EquipmentPageIndex)
                {
                    if (IndexPage == Toadd.Code)
                    {
                        whether = false;
                    }
                }
                if (whether)//过已上图数据
                    PGSQLProcessing.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", out error);

                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。数据同步已经开始但是SQL语句或数据库被删除报错了，请相关人员核查API-SynchronousISC_SPCC接口1085行，进行错误排查"),
                    }));
            }

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = (error ? "Success" : "error"),
                MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。"),
                Data = JsonData
            }));

        }
        /// <summary>
        /// 同步SPCC摄像机设备列表到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SynchronousSPCC")]
        public JObject SynchronousSPCC([FromBody] FrontEndReception.SynchronousISC_SPCC Parameter)
        {
            PGDataProcessing PGSQLProcessing = new PGDataProcessing();
            JObject JsonData = new SPCC().GetMonitoringResources(new FrontEndReception.Tree
            {
                pageNo = 1,
                pageSize = 100
            });
            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = "接口返回错误，或无网络连接，或管理员没有启动服务."
                }));

            Int32 total = Int32.Parse(JsonData["total"].ToString());//取出最大数
            Int32 page = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(100));//分页
            List<JToken> list = new List<JToken>();
            foreach (var item in JsonData["list"])
            {
                list.Add(item);
            }
            for (int pags = 2; pags < page + 1; pags++)//循环追加
            {
                JsonData = new SPCC().GetMonitoringResources(new FrontEndReception.Tree
                {
                    pageNo = pags,
                    pageSize = 100
                });

                if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = "error",
                        Remarks = "接口循环获取数据被强行中断，因为中途接口有错误！"
                    }));
                else
                    foreach (var item in JsonData["list"])
                    {
                        list.Add(item);
                    }

            }

            List<String> EquipmentPageIndex = new List<String>();
            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = ("error"),
                    MsgTxt = ("执行失败"),
                    Data = JsonData
                }));

            bool error;
            {
                PGSQLProcessing.ExecuteQuery($"DELETE FROM dock_device WHERE on_map = false and device_type='{Parameter.EquipmentType}'", out error);
                DataSet EPage = PGSQLProcessing.ExecuteQuery("SELECT * FROM dock_device", out error);
                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "SynchronousISC_SPCC接口删除数据或查询数据时出现意想不到的错误，请开发人员核查api{SynchronousISC_SPCC}接口处理问题！"),
                    }));
                //-------------------------------------------------------------------------------------
                //存入设备唯一标识码 后期跳过
                for (int i = 0; i < EPage.Tables[0].Rows.Count; i++)
                {
                    EquipmentPageIndex.Add(EPage.Tables[0].Rows[i]["device_code"].ToString());
                }
            }
            foreach (var item in list)
            {

                var Toadd = new PGAddequipmentDataMod()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Type = Parameter.EquipmentType,
                    Code = item["cameraIndexCode"].ToString(),
                    Name = item["name"].ToString(),
                    Iregion_id = Parameter.ManufacturerType,
                    Info = item.ToString(),
                    Map = "false"
                };
                Boolean whether = true;//是否上图
                foreach (var IndexPage in EquipmentPageIndex)
                {
                    if (IndexPage == Toadd.Code)
                    {
                        whether = false;
                    }
                }
                if (whether)//过已上图数据
                    PGSQLProcessing.ExecuteQuery($"INSERT INTO\"public\".\"dock_device\"(\"id\",\"device_type\",\"device_code\",\"device_name\",\"region_id\",\"device_info\",\"on_map\")VALUES('{Toadd.Id}','{Toadd.Type}','{Toadd.Code}','{Toadd.Name}','{Toadd.Iregion_id}','{Toadd.Info}','{Toadd.Map}')RETURNING*", out error);

                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。数据同步已经开始但是SQL语句或数据库被删除报错了，请相关人员核查API-SynchronousISC_SPCC接口1085行，进行错误排查"),
                    }));
            }

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = (error ? "Success" : "error"),
                MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。"),
                Data = JsonData
            }));

        }

        /// <summary>
        /// 同步SPCC区域列表到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SynchronousSPCC_Region")]
        public JObject SynchronousSPCC_Region([FromBody] FrontEndReception.SynchronousISC_SPCC Parameter)
        {
            PGDataProcessing PGSQLProcessing = new PGDataProcessing();
            JObject JsonData = new SPCC().GetPagingAreaData(new FrontEndReception.Tree
            {
                pageNo = 1,
                pageSize = 100
            });
            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = "接口返回错误，或无网络连接，或管理员没有启动服务."
                }));

            Int32 total = Int32.Parse(JsonData["total"].ToString());//取出最大数
            Int32 page = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(100));//分页
            List<JToken> list = new List<JToken>();
            foreach (var item in JsonData["list"])
            {
                list.Add(item);
            }
            for (int pags = 2; pags < page + 1; pags++)//循环追加
            {
                JsonData = new SPCC().GetPagingAreaData(new FrontEndReception.Tree
                {
                    pageNo = pags,
                    pageSize = 100
                });

                if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = "error",
                        Remarks = "接口循环获取数据被强行中断，因为中途接口有错误！"
                    }));
                else
                    foreach (var item in JsonData["list"])
                    {
                        list.Add(item);
                    }

            }

            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = ("error"),
                    MsgTxt = ("执行失败"),
                    Data = JsonData
                }));


            Boolean error;
            {
                PGSQLProcessing.ExecuteQuery("DELETE FROM dock_region", out error);

                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "SynchronousISC_SPCC接口删除数据数据时出现意想不到的错误 1264"),
                    }));
                //-------------------------------------------------------------------------------------
            }
            foreach (var item in list)
            {

                var Toadd = new PGAddEegionDataMod()
                {
                    Id = item["indexCode"].ToString(),
                    Name = item["name"].ToString(),
                    Pid = item["parentIndexCode"].ToString(),
                    Platform = Parameter.ManufacturerType

                };

                PGSQLProcessing.ExecuteQuery($"INSERT INTO \"public\".\"dock_region\"(\"id\", \"pid\", \"region_name\", \"platform\") VALUES ('{Toadd.Id}', '{Toadd.Pid}', '{Toadd.Name}', '{Toadd.Platform}') RETURNING *", out error);

                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。数据同步已经开始但是SQL语句或数据库被删除报错了，请相关人员核查API-SynchronousISC_SPCC接口1285，进行错误排查"),
                    }));
            }

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = (error ? "Success" : "error"),
                MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。"),
                Data = JsonData
            }));

        }
        /// <summary>
        /// 同步ISC区域列表到数据库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SynchronousISC_Region")]
        public JObject SynchronousISC_Region([FromBody] FrontEndReception.SynchronousISC_SPCC Parameter)
        {
            PGDataProcessing PGSQLProcessing = new PGDataProcessing();
            JObject JsonData = new ISC().GetPagingAreaData(new FrontEndReception.Tree
            {
                pageNo = 1,
                pageSize = 100
            });
            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "error",
                    Remarks = "接口返回错误，或无网络连接，或管理员没有启动服务."
                }));

            Int32 total = Int32.Parse(JsonData["total"].ToString());//取出最大数
            Int32 page = (int)Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(100));//分页
            List<JToken> list = new List<JToken>();
            foreach (var item in JsonData["list"])
            {
                list.Add(item);
            }
            for (int pags = 2; pags < page + 1; pags++)//循环追加
            {
                JsonData = new ISC().GetPagingAreaData(new FrontEndReception.Tree
                {
                    pageNo = pags,
                    pageSize = 100
                });

                if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = "error",
                        Remarks = "接口循环获取数据被强行中断，因为中途接口有错误！"
                    }));
                else
                    foreach (var item in JsonData["list"])
                    {
                        list.Add(item);
                    }

            }

            if (!HIK.GetmsgSuccessfulState(JsonData["msg"].ToString()))
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = ("error"),
                    MsgTxt = ("执行失败"),
                    Data = JsonData
                }));


            bool error;
            {
                PGSQLProcessing.ExecuteQuery("TRUNCATE TABLE dock_region", out error);

                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "SynchronousISC_Region接口删除数据或查询数据时出现意想不到的错误，请开发人员核查api{SynchronousISC_Region}接口处理问题！"),
                    }));
                //-------------------------------------------------------------------------------------
            }
            foreach (var item in list)
            {

                var Toadd = new PGAddEegionDataMod()
                {
                    Id = item["indexCode"].ToString(),
                    Name = item["name"].ToString(),
                    Pid = item["parentIndexCode"].ToString(),
                    Platform = Parameter.ManufacturerType

                };

                PGSQLProcessing.ExecuteQuery($"INSERT INTO \"public\".\"dock_region\"(\"id\", \"pid\", \"region_name\", \"platform\") VALUES ('{Toadd.Id}', '{Toadd.Pid}', '{Toadd.Name}', '{Toadd.Platform}') RETURNING *", out error);

                if (!error)
                    return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                    {
                        msg = (error ? "Success" : "error"),
                        MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。数据同步已经开始但是SQL语句或数据库被删除报错了，请相关人员核查API-SynchronousISC_Region接口1348行，进行错误排查"),
                    }));
            }

            return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
            {
                msg = (error ? "Success" : "error"),
                MsgTxt = (error ? "执行成功,同步完成。" : "执行失败。"),
                Data = JsonData
            }));

        }


        #region 临沂

        /// <summary>
        /// 对外请求接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestMeV2")]
        public String RequestMeV2([FromBody] Object str)
        {

            Boolean err = true;
            JObject data = JObject.Parse(str.ToString());

            var lut = data["params"]["events"][0]["data"];
            try
            {

                String url = new Initevery().UrlDonw(lut["faceRecognitionResult"]["snap"]["bkgUrl"].ToString());
                String urlname = new Initevery().UrlDonw(lut["faceRecognitionResult"]["snap"]["faceUrl"].ToString());

#pragma warning disable CA2000 // 丢失范围之前释放对象
                _ = new PGDataProcessing().ExecuteQuery($"INSERT INTO \"face_Info\" VALUES ('{Guid.NewGuid()}'," +
                     $" '{url}', '{urlname}', '{lut["faceRecognitionResult"]["snap"]["faceTime"]}', '{lut["resInfo"][0]["indexCode"]}');", out err);
#pragma warning restore CA2000 // 丢失范围之前释放对象
                return "200";
            }
            catch (Exception)
            {
                return "400";
            }


        }
        /// <summary>
        /// 对外请求接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestMe")]
        public String RequestMe([FromBody] Object Parameter)
        {
            //var Object = new
            //{
            //    MsgType = "HIKWEBAPI",
            //    Parameter,
            //    eventType = "RequestMe"//事件名称代码
            //};
            //try
            //{
            //    _Log.WriteLog("对外接口消息", Parameter.ToString() + System.Environment.NewLine);
            //}
            //catch (Exception)
            //{
            //}
            //JObject msg = JsonConvert.DeserializeObject<JObject>(TCPoperation.SendMsg((Object)));
            try
            {
                Boolean err = true;String status = ""; 
                JObject data = JObject.Parse(Parameter.ToString());

                _ = new PGDataProcessing().ExecuteQuery($"delete from \"dispatch_Info\" where \"vehicleCode \"='{data["vehicleCode"]}'", out err);

#pragma warning disable CA2000 // 丢失范围之前释放对象
                _ = new PGDataProcessing().ExecuteQuery($"INSERT INTO \"dispatch_Info\" VALUES ('{Guid.NewGuid().ToString()}'," +
                $" '{data["quantity"]}', '{data["vehicleCode"]}', '{data["factoryName"]}', '{data["orderNum"]}'," +
                $" '{data["unloadingDoor"]}', '{data["productName"]}', '{DateTime.Parse(data["sendTime"].ToString())}', '{data["status"]}');", out err);
#pragma warning restore CA2000 // 丢失范围之前释放对象

                return "200";
            }
            catch (Exception)
            {
                return "400";
            }


        }
        /// <summary>
        /// 获取动画一记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DispatchInfoOne")]
        public JObject DispatchInfoOne()
        {
            Boolean err = true;
            try
            {
                String b = DateTime.Now.AddSeconds(-10).ToString();
                DataSet dataset = new PGDataProcessing().ExecuteQuery($"Select * from \"dispatch_Info\" where \"orderNum\" = '1' and \"sendTime\">'{b}' ; ", out err);
                ArrayList arrayList = new ArrayList();
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    arrayList.Add(dataset.Tables[0].Rows[0].ItemArray);
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "200",
                    data = arrayList
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "400",
                    data = "Data err"
                }));
                throw;
            }
        }

        /// <summary>
        /// 获取表格记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DispatchInfoText")]
        public JObject DispatchInfoText()
        {
            Boolean err = true;
            try
            {
                DataSet dataset = new PGDataProcessing().ExecuteQuery($"Select * from \"dispatch_Info\" where \"orderNum\" != '3' ; ", out err);
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                {
                    arrayList.Add(dataset.Tables[0].Rows[i].ItemArray);
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "200",
                    data = arrayList
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "400",
                    data = "Data err"
                }));
                throw;
            }
        }

        /// <summary>
        /// 获取动画记录计总
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DispatchInfoOneExist")]
        public JObject DispatchInfoOneExist()
        {
            Boolean err = true;
            try
            {
                DataSet dataset = new PGDataProcessing().ExecuteQuery($"Select * from \"dispatch_Info\" where \"orderNum\" = '1' limit 1;Select * from \"dispatch_Info\" where \"orderNum\" = '2' and \"unloadingDoor\"='111' limit 1;Select * from \"dispatch_Info\" where \"orderNum\" = '2' and \"unloadingDoor\"='114' limit 1 ; ", out err);
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < dataset.Tables.Count; i++)
                {
                    if (dataset.Tables[i].Rows.Count > 0)
                    {
                        arrayList.Add(dataset.Tables[i].Rows[0].ItemArray);
                    }
                    else
                    {
                        arrayList.Add(new String[0]);
                    }
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "200",
                    data = arrayList
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "400",
                    data = "Data err"
                }));
                throw;
            }
        }

        /// <summary>
        /// 获取动画二记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DispatchInfoTwo")]
        public JObject DispatchInfoTow()
        {
            Boolean err = true;
            try
            {
                String b = DateTime.Now.AddSeconds(-10).ToString();
                DataSet dataset = new PGDataProcessing().ExecuteQuery($"Select * from \"dispatch_Info\" where \"orderNum\" = '2' and \"sendTime\">'{b}' ; ", out err);
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                {
                    arrayList.Add(dataset.Tables[0].Rows[i].ItemArray);
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "200",
                    data = arrayList
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "400",
                    data = "Data err"
                }));
                throw;
            }
        }

        /// <summary>
        /// 获取动画三记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DispatchInfoThree")]
        public JObject DispatchInfoThree()
        {
            Boolean err = true;
            try
            {
                String b = DateTime.Now.AddMinutes(-10).ToString();
                DataSet dataset = new PGDataProcessing().ExecuteQuery($"Select* from \"dispatch_Info\" where \"orderNum\" = '3' and \"sendTime\">'{b}' ; ", out err);
                ArrayList arrayList = new ArrayList();
                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                {
                    arrayList.Add(dataset.Tables[0].Rows[i].ItemArray);
                }
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "200",
                    data = arrayList
                }));
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "400",
                    data = "Data err"
                }));
                throw;
            }
        }
        /// <summary>
        /// 获取人脸记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DispatchCrmerl")]
        public JObject DispatchCrmerl(String code)
        {
            try
            {
                if (code == "1264282a69d94be18d461456536d4ea1")
                {
                    code = "3dd35ce4196c4dd2a293dda1d5ea43e7";
                }
                Boolean err = true;
                DataSet dataset = new PGDataProcessing().ExecuteQuery($"Select * from \"face_Info\" where \"state\"='{code}' ", out err);

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "200",
                    data = dataset.Tables[0]
                }));
            }
            catch (Exception)
            {

                return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new
                {
                    msg = "400",
                    data = "err"
                }));
            }


        }


        #endregion


        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("TestPostBut")]
        public JObject TestPostBut()
        {

            Log.Verbose("这是一个Verbose");
            Log.Information("开始");
            int a = 0;
            int b = 2;
            try
            {
                Log.Debug("计算两者相除");
                Console.WriteLine(b / a);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "计算出现意外的错误");
            }
            Log.Information("结束");
            Log.CloseAndFlush();

            JObject msg = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(new { msg = "无状态" }));
            return msg;
        }



    }


}
