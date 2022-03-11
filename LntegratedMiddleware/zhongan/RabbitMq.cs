using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using LntegratedMiddleware.zhongan.WebSocket;
using Serilog;

namespace LntegratedMiddleware.zhongan
{
    public class RabbitMq
    {


        public void Rmq()
        {
            var factory = new ConnectionFactory();
            factory.HostName = "172.16.10.202";//RabbitMQ服务在本地运行
            factory.UserName = "zhongan";//用户名
            factory.Password = "123456";//密码

            var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("external3d", true, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("external3d", true, consumer);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.Span;
                var message = Encoding.UTF8.GetString(body);
                Log.Debug("已接收： {0}", message);
                WebsocketServer.SetWebSocketMsg(message);

            };


        }
    }


}
