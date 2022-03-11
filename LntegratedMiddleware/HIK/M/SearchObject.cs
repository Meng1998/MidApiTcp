using System;

namespace LntegratedMiddleware.HIK.M
{
    /// <summary>
    /// 以图搜图参数
    /// </summary>
    public class SearchObject
    {
        /// <summary>
        /// 相似度
        /// </summary>
        public Int32 minSimilarityS { get; set; }

        /// <summary>
        /// 图片Base
        /// </summary>
        public string Base64 { get; set; }
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
        public string facePicUrlS { get; set; }
    }
    /// <summary>
    /// 根据条件查询人脸库
    /// </summary>
    public class FaceDataObject
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
}
