using DeploymentTools.Mod;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    class InitKey
    {

        GetDataSet GetSetData = new GetDataSet();
        ISCPostJosn PostJosnData = new ISCPostJosn();

        /// <summary>
        /// 拿到获取根区域信息
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetRootregionData()
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(1, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_Rootregion(), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        /// <summary>
        /// 获取设备列表（门禁 消防传感器）
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetAccessControlList(String Url, Int32 _pageNo = 1, Int32 _pageSize = 1000)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(9, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(Url, PostJosnData.JOSNPOST_AccessControlDeviceList(_pageNo, _pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);


            var err = new
            {
                msg = "error",
                Remarks = "接口超时或报错！错误信息为:" + Data
            };
            var errormsg = JsonConvert.SerializeObject(err);
            if (error)
                return Data;
            else
                return errormsg;
        }


        /// <summary>
        /// 获取报警主机下防区
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetiasDevicelDefenceList(String Url, String IndexCode)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(12, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(Url, PostJosnData.JOSNPOST_DeviceList(IndexCode), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);


            var err = new
            {
                msg = "error",
                Remarks = "接口超时或报错！错误信息为:" + Data
            };
            var errormsg = JsonConvert.SerializeObject(err);
            if (error)
                return Data;
            else
                return errormsg;
        }
        /// <summary>
        /// 获取消防传感器
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetAccessSensorList(String Url, Int32 _pageNo = 1, Int32 _pageSize = 1000)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(10, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(Url, PostJosnData.JOSNPOST_AccessControlDeviceList(_pageNo, _pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);


            var err = new
            {
                msg = "error",
                Remarks = "接口超时或报错！错误信息为:" + Data
            };
            var errormsg = JsonConvert.SerializeObject(err);
            if (error)
                return Data;
            else
                return errormsg;
        }
        /// <summary>
        /// 获取报警主机列表
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetIasSensorList(String Url, Int32 _pageNo = 1, Int32 _pageSize = 1000)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(11, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(Url, PostJosnData.JOSNPOST_AccessControlDeviceList(_pageNo, _pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);

            var err = new
            {
                msg = "error",
                Remarks = "接口超时或报错！错误信息为:" + Data
            };
            var errormsg = JsonConvert.SerializeObject(err);
            if (error)
                return Data;
            else
                return errormsg;
        }
        /// <summary>
        /// 分页获取区域信息
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetPagingAreaData( Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(1, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_PagingArea(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        /// <summary>
        /// 取当前用户注册了那些事件
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetRegistrationEventsData()
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(4, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), "", GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }
        /// <summary>
        /// 提交订阅给接口
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String SetFullSubscriptionData( Int32[] eventTypesStr, String eventDestStr, Int32 subTypeInT, Int32[] eventLvlInt)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(5, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_SubscriptionData(eventTypesStr, eventDestStr, subTypeInT, eventLvlInt), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }
        /// <summary>
        /// 取设备信息
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetEquipmentData(Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(3, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_Equipment(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        public string GetParkList(Int32 pages, Int32 Number) {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(12, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_Equipment(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        public string roadwayList(Int32 pages, Int32 Number,string entranceIndexCodes) {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(13, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JSON_roadway(pages, Number, entranceIndexCodes), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }


        /// <summary>
        /// 报警主机
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public string GetFangquzhuji(Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(14, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_Equipment(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        /// <summary>
        /// 报警主机通道列表
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public string GetFangquzhujitongdaohao(Int32 pages, Int32 Number,string type)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(15, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JSON_tongdao(pages, Number, type), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }




        /// <summary>
        /// 区域信息取监控点
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetMonitoringPointData( String IndexCode, Int32 NOindex)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(2, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_MonitoringPoint(IndexCode, NOindex), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }
        /// <summary>
        /// 取消订阅信息
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String Unsubscribe( Int32[] eventTypesInt)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(6, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_Unsubscribe(eventTypesInt), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }
        /// <summary>
        /// 取到重点人员信息
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetKeyPersonnel( Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(7, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_BlacklistData(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }
        /// <summary>
        /// 获取监控点预览取流URL
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String GetFlowURL( String cameraIndexCode, Int32 streamType, String protocol, Int32 transmode)
        {

            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(8, 0);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_FlowURL(cameraIndexCode, streamType, protocol, transmode), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        /// <summary>
        /// 测试密钥是否正确
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String TestKetPost(Int32 pages, Int32 Number, String type)
        {
            Boolean error = true; String Encryptionkey = null;
            String Data = null; SecretKey key;

            switch (type)
            {
                case "0":

                    //加密秘钥
                    key = GetSetData.GetISCkey(1, 0);
                    Encryptionkey = GetSetData.GetencryptionKey(key);

                    Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_BlacklistData(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);

                    break;
                case "1":
                    key = GetSetData.GetISCkey(1, 1);
                    Encryptionkey = GetSetData.GetencryptionKey(key);

                    //加密秘钥
                    Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_SPCCtestList(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
                    break;
            }

            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接" + Data;
        }

        ///SPCC
        /// <summary>
        /// 按事件类型获取事件订阅信息
        /// </summary>
        /// <param name="Url">Post的地址</param>
        /// <param name="index">订阅的事件类型</param>
        /// <returns></returns>
        public String GetEquipmentList( Int32[] index, out Boolean error)
        {
            error = true;
            SecretKey key = GetSetData.GetISCkey(0, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), new SPCCPostJson().JOSNPOST_Subscribe(index), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);

            return Data;

        }
        /// <summary>
        /// 分页获取监控点资源
        /// </summary>
        /// <param name="Url">Post的地址</param>
        /// <param name="pageNo">指定第几页，从1开始</param>
        /// <param name="pageSize">每页返回的条数</param>
        /// <returns></returns>
        public String GetMonitoringPoint(Int32 pageNo, Int32 pageSize, out Boolean error)
        {
            error = true;
            SecretKey key = GetSetData.GetISCkey(1, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), new SPCCPostJson().JOSNPOST_MonitoringPointList(pageNo, pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            return Data;
        }
        /// <summary>
        /// 取监控点视频流
        /// </summary>
        /// <param name="Url">Post的地址</param>
        /// <param name="data">接收的数据结构/param>
        /// <returns></returns>
        public String GetVideoStreamData( GetVideoStreamSet data, out Boolean error)
        {
            error = true;
            SecretKey key = GetSetData.GetISCkey(2, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);
            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), new SPCCPostJson().JOSNPOST_VideoStream(data), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            return Data;

        }

        /// <summary>
        /// 分页获取区域信息
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String SPCCGetPagingAreaData(Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(3, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_SPCCtestList(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        /// <summary>
        /// 获取门禁通道
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String SPCCGetAccessControlAccess(Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(4, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_SPCCtestList(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
           
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }  
        /// <summary>
        /// 获取防区通道
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String SPCCGetZoneSynchronization(Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(6, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_SPCCtestList(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }

        /// <summary>
        /// 获取对接通道
        /// </summary>
        /// <param name="Url">连接</param>
        /// <returns></returns>
        public String SPCCGetDockingChannel(Int32 pages, Int32 Number)
        {
            Boolean error = true;
            SecretKey key = GetSetData.GetISCkey(5, 1);
            //加密秘钥
            String Encryptionkey = GetSetData.GetencryptionKey(key);

            String Data = Post.HttpPostRaw(GetSetData.StitchingParameters(key), PostJosnData.JOSNPOST_SPCCtestList(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
            if (error)
                return Data;
            else
                return "error : 远程服务器无法连接";
        }
    }
}
