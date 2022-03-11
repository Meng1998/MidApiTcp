using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
   public class SecretKey
    {
        //三个必要的通信参数
        public String Name { get; set; }
        public String Context { get; set; }
        public String API { get; set; }
        public Int32 Port { get; set; }
        public  String Host { get; set; }
        public  String appKey { get; set; }
        public  String appSecret { get; set; }

    }
   
}
