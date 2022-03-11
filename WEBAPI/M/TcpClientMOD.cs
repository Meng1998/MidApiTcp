using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEBAPI.M
{
    public class TcpSocketState
    {
        /// <summary>
        /// TCP服务是否开启
        /// </summary>
        public Boolean GetStateSocket { get; set; } = false;
    }
    /// <summary>
    /// 返回的消息
    /// </summary>
    public class GetTcpMOD {

        public Boolean GetStateMsg { get; set; } = false;
        public String TcpMsg { get; set; }

    }
}
