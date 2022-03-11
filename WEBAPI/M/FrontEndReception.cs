using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBAPI.M
{
    /// <summary>
    /// Pg数据库 区域存储
    /// </summary>
    class PGAddEegionDataMod
    {
        public String Id { get; set; }
        public String Pid { get; set; }
        public String Name { get; set; }
        public String Platform { get; set; }

    }
    /// <summary>
    /// Pg数据库 设备存储
    /// </summary>
    class PGAddequipmentDataMod
    {
        public String Id { get; set; }
        public String Type { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
        public String Iregion_id { get; set; }
        public String Info { get; set; }
        public String Map { get; set; }

    }
    class GetJson
    {
        /// <summary>
        /// 以图搜图Json
        /// </summary>
        public object JOSNPOST_GoogleSearchImage(Int32 pageSizeI, Int32 totalI, Int32 pageNoI, object[] listS)
        {
            var Object = new
            {
                pageSize = pageSizeI,
                list = listS,
                total = totalI,
                pageNo = pageNoI,
            };
            object serJson = Object;
            return serJson;
        }


        /// <summary>
        /// 以图搜图子对象
        /// </summary>
        public class SearchSubObject
        {
            public String cameraIndexCode { get; set; }
            public String cameraName { get; set; }
            public String captureTime { get; set; }
            public String sex { get; set; }
            public String ageGroup { get; set; }
            public String withGlass { get; set; }
            public String similarity { get; set; }
            public String bkgPicUrl { get; set; }
            public String facePicUrl { get; set; }
            public String X { get; set; }
            public String Y { get; set; }
            public String rect { get; set; }

        }


        /// <summary>
        /// 以图搜图子对象
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public object JOSNPOST_SearchImageSubLevel(SearchSubObject mod)
        {
            var Object = new
            {
                mod
            };
            var serJson = Object;
            return serJson;
        }
    }




    public class FrontEndReception
    {
        public class Vehiclerecords{
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
        }

        public class Faceattributes
        {
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
            public String Sort { get; set; }//排序字段（similarity-按相似度排序，captureTime-按抓拍时间排序）
            public String Order { get; set; }//图片信息（支持传图片url，图片二进制数据，模型数据3种方式，三选一必填）
            public String cameraIndexCodes { get; set; }//监控点编号
        } 
        public class Snapshot
        {
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
            public String beginTime { get; set; }//开始时间格式 2017-06-15T00:00:00.000+08:00
            public String endTime { get; set; }//结束时间格式 2017-06-15T00:00:00.000+08:00
            public String Sort { get; set; }//排序字段（similarity-按相似度排序，captureTime-按抓拍时间排序）
            public String Order { get; set; }//图片信息（支持传图片url，图片二进制数据，模型数据3种方式，三选一必填）
        }
        public class Vehicle
        {
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
            public String beginTime { get; set; }
            public String endTime { get; set; }
            public String plateNo { get; set; }//车牌号，必须为精确车牌号

        }


        public class DHdoor
        {
            public String[] channelCodeList { get; set; }
        }
        public class DHdoorstate
        {
            //public String orgCode { get; set; }//组织编号
            public String channelCode { get; set; }//通道编号
        }
        public class DHdoorrecord
        {
            public Int32 pageNum { get; set; }
            public Int32 pageSize { get; set; }
            public String startSwingTime { get; set; }
            public String endSwingTime { get; set; }
            public String channelCode { get; set; }//通道编号
        }
        public class DHFace
        {
            public Int32 pageNum { get; set; }
            public Int32 pageSize { get; set; }
            public String searchType { get; set; }//1:模糊;2:精确
            public String startTime { get; set; }
            public String endTime { get; set; }
            public String cardId { get; set; }//卡号
        }

        public class speeding
        {
            public Int32 pageNum { get; set; }
            public Int32 pageSize { get; set; }
            public Int32 alarmType { get; set; }//1: 超速,2:区间超速,3:区间低速
            public String captureType { get; set; }//1：单点、2：区间
            public String startTime { get; set; }
            public String endTime { get; set; }
        }

        public class DHPassing
        {
            public String startDateAll { get; set; }//开始时间
            public String endDateAll { get; set; }//结束时间
            public String screenPlaceId { get; set; }//通道编码
           
        }




        public class ByPassDefenceArea {
            public String defenceIndexCode { get; set; }
            public Int32 status { get; set; }
        }

        public class defence        { 
            public List<ByPassDefenceArea> defenceList { get; set; }
        }

        public class DefenceAreaState
        {
            public String[] defenceIndexCodes { get; set; }
        }

        public class DoorEvent {
            public String[] doorIndexCodes { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
        }

        public class CardInfo {
            public string cardNo { get; set; }
        }


        /// <summary>
        /// 门禁列表
        /// </summary>
        public class DoorChannel
        {
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
            /// <summary>
            /// 区域组织编号
            /// </summary>
            public string regionIndexCode { get; set; }
        }
        public class Statecode
        {
            public String[] doorIndexCodes { get; set; }
        }
        /// <summary>
        /// 门禁记录
        /// </summary>
        public class Doorrecord
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public String startTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public String endTime { get; set; }
            /// <summary>
            /// 唯一标识 最多支持10个
            /// </summary>
            public String[] doorIndexCodes { get; set; }

            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
        }

        /// <summary>
        /// 门禁点反控
        /// </summary>
        public class AccessControl
        {
            /// <summary>
            /// 设备编码
            /// </summary>
            public String[] doorIndexCodes { get; set; }
            /// <summary>
            /// 0: 常开  1: 门闭  2: 门开  3: 常闭
            /// </summary>
            public Int32 controlType { get; set; }
        }
        /// <summary>
        /// 门禁状态
        /// </summary>
        public class QueryAccessPointsStatus
        {
            /// <summary>
            /// 门禁编号
            /// </summary>
            public String[] doorIndexCodes { get; set; }
        }
        /// <summary>
        /// 获取编码设备/监控点/门禁设备/门禁点状态
        /// </summary>
        public class DeviceStatus
        {
            /// <summary>
            /// 设备编码
            /// </summary>
            public String[] indexCodes { get; set; }
            /// <summary>
            /// 设备状态
            /// </summary>
            public String status { get; set; }
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
        }
        /// <summary>
        /// 从服务器下载图片
        /// </summary>
        public class GetPicturUrl
        {
            /// <summary>
            /// 图片链接
            /// </summary>
            public String url { get; set; }
        }
        /// <summary>
        /// 以图搜图参数
        /// </summary>
        public class SearchObject
        {
            /// <summary>
            /// 相似度
            /// </summary>
            public Int32 minSimilarity { get; set; }
            /// <summary>
            /// 图片Base
            /// </summary>
            public string facePicBinaryData { get; set; } = null;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string startTime { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public string endTime { get; set; }
            /// <summary>
            /// 图片Url
            /// </summary>
            public string facePicUrl { get; set; }
        }
        /// <summary>
        /// 查询人脸库
        /// </summary>
        public class FaceDatabase
        {
            /// <summary>
            /// 姓名 条件可为空
            /// </summary>
            public String name { get; set; } = null;
            /// <summary>
            /// 身份证件或其他 条件可为空
            /// </summary>
            public String certificateNum { get; set; } = null;
            /// <summary>
            /// 取第几页
            /// </summary>
            public Int32 pageNo { get; set; } = 1;
            /// <summary>
            /// 单页有多少条数
            /// </summary>
            public Int32 pageSize { get; set; } = 1000;
        }
        /// <summary>
        /// 一对多人脸搜索
        /// </summary>
        public class Contrast_1VN
        {
            public Int32 pageNo { get; set; } = 1;
            public Int32 pageSize { get; set; } = 1000;
            /// <summary>
            /// 相似度
            /// </summary>
            public Int32 minSimilarity { get; set; }
            /// <summary>
            /// 人脸图
            /// </summary>
            public Int32 facePicBinaryData { get; set; }
            /// <summary>
            /// 人脸分组
            /// </summary>
            public Int32 faceGroupIndexCodes { get; set; }
        }
        /// <summary>
        /// 对页数有要求的通用模型
        /// </summary>
        public class Tree
        {
            public Int32 pageNo { get; set; }
            public Int32 pageSize { get; set; }
            public String treeCode { get; set; } = "0";
        }
        /// <summary>
        /// 取视频
        /// </summary>

        public class deviceControl{
            public String roadwaySyscode { get; set; }
            public Int32 command { get; set; }
        }

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
        /// <summary>
        /// ISC获取订阅信息
        /// </summary>
        public class GetSubscriptionInformation
        {
            /// <summary>
            /// 事件类型
            /// </summary>
            public long[] eventTypes;
        }
        /// <summary>
        /// 同步到数据库所需字段
        /// </summary>
        public class SynchronousISC_SPCC
        {
            /// <summary>
            /// 厂商类型
            /// </summary>
            public String ManufacturerType { get; set; }
            /// <summary>
            /// 设备类型
            /// </summary>
            public String EquipmentType { get; set; }

        }

        public class SPCC_VehicleCheck
        {
            public Int32 pageNo { get; set; } = 1;
            public Int32 pageSize { get; set; } = 1000;
            /// <summary>
            /// 车牌号
            /// </summary>
            public String PlateNo { get; set; }
            /// <summary>
            /// 离开状态2全部，0进入，1离开
            /// </summary>
            public Int32 direction { get; set; }
            /// <summary>
            ///进入查询开始时间
            /// </summary>
            public string beginInTime { get; set; }
            /// <summary>
            ///进入查询结束时间
            /// </summary>
            public string endInTime { get; set; }
            /// <summary>
            ///离开查询开始时间
            /// </summary>
            public string beginOutTime { get; set; }
            /// <summary>
            ///厉害查询结束时间
            /// </summary>
            public string endOutTime { get; set; }
        }


        /// <summary>
        /// 获取根区域信息
        /// </summary>
        public class GetConditionRootregionData
        {
            /// <summary>
            /// 通过indexCode集合查询指定的识别资源集合 False
            /// </summary>
            public String[] indexCodes { get; set; }
            /// <summary>
            /// 识别资源名称模糊查询 False
            /// </summary>
            public String name { get; set; }
            /// <summary>
            /// 根据识别资源的类型搜索，SUPER_BRAIN 超脑，FACE_RECOGNITION_SERVER 脸谱，COMPARISON 深眸 False
            /// </summary>
            public String recognitionResourceType { get; set; }
        }
        /// <summary>
        /// 云台控制
        /// </summary>
        public class PanTiltControl
        {
            //String EquipmentCode, Int32 PTZCmdID, Int32 PTZCmdPara1 = 2
            /// <summary>
            /// 设备编码
            /// </summary>
            public String EquipmentCode { get; set; }
            /// <summary>
            ///命令参数/类型 ：<font color="#FF0000">云台控制传参 请用八进制!</font> <br />
            ///0x0101： 光圈关停止    <br />
            ///0x0102： 光圈关        <br />
            ///0x0103： 光圈开停止    <br />
            ///0x0104： 光圈开        <br />
            ///0x0201： 近聚集停止    <br />
            ///0x0202： 近聚集        <br />
            ///0x0203： 远聚集 停止   <br />
            ///0x0204： 远聚集        <br />
            ///0x0301： 放大停止      <br />
            ///0x0302： 放大         <br />
            ///0x0303： 缩小停止      <br />
            ///0x0304： 缩小         <br />
            ///0x0401： 向上停止      <br />
            ///0x0402： 向上         <br />
            ///0x0403： 向下停止      <br />
            ///0x0404： 向下         <br />
            ///0x0501： 向右停止      <br />
            ///0x0502： 向右         <br />
            ///0x0503： 向左停止      <br />
            ///0x0504： 向左         <br />
            ///0x0601： 预置位保存    <br />
            ///0x0602： 预置位调用    <br />
            ///0x0603： 预置位删除    <br />
            ///0x0701： 左上停止      <br />
            ///0x0702： 左上         <br />
            ///0x0703： 左下停止      <br />
            ///0x0704： 左下         <br />
            ///0x0801： 右上停止      <br />
            ///0x0802： 右上         <br />
            ///0x0803： 右下停止      <br />
            ///0x0804： 右下         <br />
            ///0x0901： PTZ 全停命令字<br />
            ///0x0907： FI 全停命令字 <br />
            ///0x0902： 绝对移动      <br />
            ///0x0A01： 雨刷开IMOS Restful 基础业务
            /// </summary>
            public Int32 PTZCmdID { get; set; }
            /// <summary>
            /// 云台控速
            /// </summary>
            public Int32 PTZCmdPara1 { get; set; } = 2;


        }
        //(String CameraId,String nBeginStr = "2020-8-15 8:00:00",String nEndStr = "2020-8-16 8:00:00")
        /// <summary>
        /// 大华获取客流量模型
        /// </summary>
        public class DHTotal
        {
            /// <summary>
            /// 相机ID
            /// </summary>
            public String CameraId { get; set; }
            /// <summary>
            /// 开始时间 
            /// </summary>
            public String nBeginStr { get; set; }
            /// <summary>
            /// 结束时间
            /// </summary>
            public String nEndStr { get; set; }

        }
        /// <summary>
        /// 纽贝尔门禁控制
        /// </summary>
        public class OpenDoors
        {
            //Boolean ComType,String DoorNo,String CmdParam = "1"
            /// <summary>
            /// 常开或常关
            /// </summary>
            public Boolean ComType { get; set; }
            /// <summary>
            /// 门禁编码
            /// </summary>
            public String DoorNo { get; set; }
            /// <summary>
            /// 常开关时间 按分钟 
            /// </summary>
            public String CmdParam { get; set; } = "1";
        }
        /// <summary>
        /// 纽贝尔门禁控制V2
        /// </summary>
        public class OpenDoorsV2
        {
            /// <summary>
            /// 门禁编码
            /// </summary>
            public String DoorNo { get; set; }

        }
        /// <summary>
        /// 门状态
        /// </summary>
        public class GateState
        {
            /// <summary>
            /// 门禁编码
            /// </summary>
            public String DoorNo { get; set; }

        }
        /// <summary>
        /// 订阅事件信息
        ///("http://xxx:8081/YS/RequestMe", 0/1/2)
        /// </summary>
        public class AlarmSubscription
        {
            /// <summary>
            /// 注册地址
            /// </summary>
            public String Url { get; set; }
            /// <summary>
            /// 注册类型
            /// </summary>
            public Int32 Type { get; set; } = 999;

        }
        public class Ctrl
        {

            public Int32 ctrlmaintype { get; set; }
            public Int32 ctrlsubtype { get; set; }

            public String uniquecode { get; set; }
        }

    }
}
