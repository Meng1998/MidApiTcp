using System;

namespace LntegratedMiddleware.HIK.M
{
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
}
