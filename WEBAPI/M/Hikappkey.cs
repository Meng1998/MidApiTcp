using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMWEBAPI.M
{
    public class Hikappkey
    {
    }
    public class SecretKey
    {
        //三个必要的通信参数
        public String Context { get; set; }
        public String API { get; set; }
        public Int32 Port { get; set; }
        public String Host { get; set; }
        public String appKey { get; set; }
        public String appSecret { get; set; }

    }
}
