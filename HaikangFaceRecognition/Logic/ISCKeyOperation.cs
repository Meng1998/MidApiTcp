using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Logic
{
    //class ISCKeyOperation
    //{
     
    //        MOD.ISCPostJson PostJosnData = new MOD.ISCPostJson();

    //        GetDataSet GetSetData = new GetDataSet();
    //        /// <summary>
    //        /// 取到面库
    //        /// </summary>
    //        /// <param name="Url">链接</param>
    //        /// <returns></returns>
    //        public String GetKeyPersonnel(String Url, FaceDataObject data, out Boolean error)
    //        {
    //            error = true;
    //            SecretKey key = GetSetData.GetISCkey(6, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);
    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_BlacklistData(data), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }
    //        /// <summary>
    //        /// 查询人脸分组
    //        /// </summary>
    //        /// <returns></returns>
    //        public String GetFaceGrouping(String Url)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(4, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, "{}", GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }

    //        /// <summary>
    //        /// 取到车辆库
    //        /// </summary>
    //        /// <param name="Url">链接</param>
    //        /// <param name="pages">页</param>
    //        /// <param name="Number">单页多少条</param>
    //        /// <returns></returns>
    //        public String GetVehicleInformation(String Url, Int32 pages, Int32 Number)
    //        {

    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(7, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);
    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_VehicleData(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }

    //        /// <summary>
    //        /// 取到布控车辆
    //        /// </summary>
    //        /// <param name="Url">链接</param>
    //        /// <param name="pages">页</param>
    //        /// <param name="Number">单页多少条</param>
    //        /// <returns></returns>
    //        public String GetVehicleBlacklist(String Url, Int32 pages, Int32 Number)
    //        {

    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(8, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);
    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_VehicleBlacklistData(pages, Number), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }
    //        /// <summary>
    //        /// 拿到秘钥参数
    //        /// </summary>
    //        /// <returns></returns>
    //        public List<SecretKey> GetKey()
    //        {
    //            return (List<SecretKey>)GetSetData.KEY(0);
    //        }

    //        /// <summary>
    //        /// 拿到获取根区域信息
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetRootregionData(String Url)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(0, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, "{}", GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }

    //        /// <summary>
    //        /// 以图搜图
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetGoogleSearchImage(String Url, Int32 minSimilarityS, String Base64, String startTim, String endTim, String facePicUrlS, out Boolean error)
    //        {
    //            error = true;
    //            SecretKey key = GetSetData.GetISCkey(5, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_GoogleSearchImage(minSimilarityS, Base64, startTim, endTim, facePicUrlS), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }
    //        /// <summary>
    //        /// 按条件查询识别资源
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetConditionRootregionData(String Url, String[] index, String namestr, String type)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(0, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_Facedata(index, namestr, type), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }

    //        /// <summary>
    //        /// 获取监控点是否在线
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <param name="index">获取某些设备状态</param>
    //        /// <param name="_pageNo">指定第几页</param>
    //        /// <param name="_pageSize">返回条数</param>
    //        /// <returns></returns>
    //        public String GetTheEquipmentStatus(String Url, String[] index, String state, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(13, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_SetDeviceStatus(_pageNo, _pageSize, index, state), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }
    //        /// <summary>
    //        /// 按条件下载图片资源
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <param name="picUrl">图片连接</param>
    //        /// <returns></returns>
    //        public String GetPictureData(String Url, String picUrl)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(9, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.HttpPostObject(Url, PostJosnData.JOSNPOST_Picdata(picUrl), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }
    //        /// <summary>
    //        /// 获取编码设备状态
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetCodingEquipmentStatus(String Url, String[] index, String state, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(11, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_DeviceStatus(_pageNo, _pageSize, index, state), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }

    //        /// <summary>
    //        /// 获取编码设备列表
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetEquipmentList(String Url, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(12, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_DeviceList(_pageNo, _pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }

    //        /// <summary>
    //        /// 获取门禁设备列表
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetAccessControlList(String Url, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(14, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_AccessControlDeviceList(_pageNo, _pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }

    //        /// <summary>
    //        /// 获取门禁点列表
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetAccessPointsList(String Url, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(15, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);
    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_AccessPointsList(_pageNo, _pageSize), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }

    //        /// <summary>
    //        /// 获取门禁设备在线状态
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetAccessControlStatus(String Url, String[] index, String state, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(16, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_AccessControlStatus(_pageNo, _pageSize, index, state), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }

    //        /// <summary>
    //        /// 获取门禁点在线状态
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetAccessPointsState(String Url, String[] index, String state, Int32 _pageNo = 1, Int32 _pageSize = 1000)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(17, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_GetAccessPointsState(_pageNo, _pageSize, index, state), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }
    //        /// <summary>
    //        /// 获取门禁点反控
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String PostAccessControl(String Url, AccessControl mod, out Boolean error)
    //        {
    //            error = true;
    //            SecretKey key = GetSetData.GetISCkey(18, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_AccessControl(mod.doorIndexCodes, mod.controlType), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "报错异常：" + Data;
    //        }
    //        /// <summary>
    //        /// 人脸分组1VN检索
    //        /// </summary>
    //        /// <param name="Url">连接</param>
    //        /// <returns></returns>
    //        public String GetContrast_1VNData(String Url, Int32 minSimilarityS, String Base64, String[] IndexCodes)
    //        {
    //            Boolean error = true;
    //            SecretKey key = GetSetData.GetISCkey(1, 0);
    //            //加密秘钥
    //            String Encryptionkey = GetSetData.GetencryptionKey(key);

    //            String Data = Post.ISCHttpPostRaw(Url, PostJosnData.JOSNPOST_ContrastAVN(minSimilarityS, Base64, IndexCodes), GetDataSet.getTimeStr(), Encryptionkey, key.appKey, ref error);
    //            if (error)
    //                return Data;
    //            else
    //                return "error : 远程服务器无法连接";
    //        }



    //        /// <summary>  
    //        /// 本地时间转成GMT时间  
    //        /// </summary>  
    //        public static string ToGMTString(DateTime dt)
    //        {
    //            return dt.ToUniversalTime().ToString("r");
    //        }




    //}
}
