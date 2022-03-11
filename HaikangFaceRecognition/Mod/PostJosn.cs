using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    public class ISCPostJosn
    {
        /// <summary>
        /// 获取根区域信息JOSN
        /// </summary>
        public String JOSNPOST_Rootregion()
        {
            var Object = new
            {
                treeCode = ""
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        /// 取门禁设备列表
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_AccessControlDeviceList(Int32 pages, Int32 Number)
        {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }

        /// <summary>
        /// 查询设备下所有防区
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_DeviceList(String IndexCode)
        {
            var Object = new
            {
                deviceIndexCode = IndexCode,
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        ///  分页取区域
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_PagingArea(Int32 pages, Int32 Number)
        {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        ///  取黑名单人员
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_BlacklistData(Int32 pages, Int32 Number)
        {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        ///  通用性數據
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_SPCCtestList(Int32 pages, Int32 Number)
        {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number,
                treeCode = 0
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        ///  取黑名单人员
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_FlowURL(String cameraIndexCodeS, Int32 streamTypeI, String protocolS, Int32 transmodeI)
        {
            var Object = new
            {
                cameraIndexCode = cameraIndexCodeS,
                streamType = streamTypeI,
                protocol = protocolS,
                transmode = transmodeI
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }

        /// <summary>
        ///  取消订阅
        /// </summary>
        /// <returns></returns>
        public String JOSNPOST_Unsubscribe(Int32[] eventTypesInt)
        {
            var Object = new
            {
                eventTypes = eventTypesInt,
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        ///  提交订阅事件信息
        /// </summary>
        /// <returns></returns>
        public String JOSNPOST_SubscriptionData(Int32[] eventTypesStr, String eventDestStr, Int32 subTypeStr, Int32[] eventLvlInt)
        {
            var Object = new
            {
                eventTypes = eventTypesStr,
                eventDest = eventDestStr,
                subType = subTypeStr,
                //eventLvl = eventLvlInt

            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        ///  取设备信息
        /// </summary>
        /// <param name="pages">页数</param>
        /// <param name="Number">数目</param>
        /// <returns></returns>
        public String JOSNPOST_Equipment(Int32 pages, Int32 Number)
        {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }

        public string JSON_roadway(Int32 pages, Int32 Number, string entranceIndexCodes) {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number,
                entranceIndexCodes= entranceIndexCodes
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }


        public string JSON_tongdao(Int32 pages, Int32 Number, string resourceType)
        {
            var Object = new
            {
                pageNo = pages,
                pageSize = Number,
                resourceType = resourceType
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }


        /// <summary>
        /// 取区域摄像头
        /// </summary>
        /// <param name="IndexCode">区域唯一码IndexCode</param>
        /// <param name="pageNoIndex">第几页(默认1)</param>
        /// <returns></returns>
        public String JOSNPOST_MonitoringPoint(String IndexCode, Int32 pageNoIndex)
        {
            var Object = new
            {
                regionIndexCode = IndexCode,
                pageNo = pageNoIndex,
                pageSize = 1000//pageSizeCount 单页信息个数
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
}
    public  class SPCCPostJson
    {
        /// <summary>
        /// 按事件类型获取事件订阅信息
        /// </summary>
        /// <returns></returns>
        public String JOSNPOST_Subscribe(Int32[] index)
        {
            var Object = new
            {
                eventTypes = index
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        /// 按事件类型获取事件订阅信息
        /// </summary>
        /// <returns></returns>
        public String JOSNPOST_MonitoringPointList(Int32 pageNo, Int32 pageSize)
        {

            var Object = new
            {
                pageNo,
                pageSize,
                treeCode = "0"
            };
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
        /// <summary>
        /// 取监控点视频流
        /// </summary>
        /// <returns></returns>
        public String JOSNPOST_VideoStream(GetVideoStreamSet data)
        {

            var Object = data;
            String serJson = JsonConvert.SerializeObject(Object);
            return serJson;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GetVideoStreamSet
    {
        /// <summary>
        /// 监控点编码
        /// </summary>
        public String cameraIndexCode { get; set; }
        /// <summary>
        /// 视频通道 0为主码流 1为辅码流 :默认为0
        /// </summary>
        public Int32 streamType { get; set; } = 0;
        /// <summary>
        /// 视频流协议类型 类型有rtsp，rtmp，hls :默认rtsp 
        /// </summary>
        public String protocol { get; set; } = "rtsp";


    }
}
