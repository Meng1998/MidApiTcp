using System;

namespace LntegratedMiddleware.HIK.M
{

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
