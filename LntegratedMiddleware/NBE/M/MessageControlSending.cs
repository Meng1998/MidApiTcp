using System;
using System.Collections.Generic;
using System.Text;

namespace LntegratedMiddleware.NBE.M
{
    class MessageControlSending
    {
        /// <summary>
        /// 发送消息是否等待接收第一条信息保存返回信息并且处理发送
        /// </summary>
        public class SendMessage {
            public static Boolean state { get; set; } = false;
            public static String Msg { get; set; }
        }
    }
}
