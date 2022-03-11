using System;

namespace LntegratedMiddleware.YS.M
{
    class PostJson
    {
        public static String Login(LoingClss Value)
        {


            String setJson = Newtonsoft.Json.JsonConvert.SerializeObject(Value);

            return setJson;

        }

        public class LoingClss
        {
            public String UserName { get; set; }
            public String AccessCode { get; set; }
            public String LoginSignature { get; set; }
        }
    }
}
