using Newtonsoft.Json.Linq;
using System;

namespace LntegratedMiddleware.YS.C
{
    class DataManipulation
    {
        public static Boolean VerifyReturnValue(JObject json)
        {

            if (json["ErrMsg"].ToString() == "OK")
                return true;
            else
                return false;


        }
    }
}
