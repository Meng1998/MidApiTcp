/**
* 通过net.Server类来创建一个TCP服务器
*/
/* 引入net模块 */
var net = require("net");
/* TCPMOD */
const TCPMOD = require('../DataModel/TcpMOD');
/* 创建TCP客户端 */
var client = net.Socket();
/**
 * 初始化TCP客户端
 */
function InitTcpClient() {

    try {
        /* 设置连接的服务器 */
        client.connect(9999, '127.0.0.1', function () {
            console.log("connect the server");

            /* 向服务器发送数据 */
            //client.write("message from client");
        });

        /* 监听服务器传来的data数据 */
        client.on("data", function (data) {
            console.log("the data of server is " + data.toString());

            if (TCPMOD.GetTcpMOD.GetState) {
                TCPMOD.GetTcpMOD.GetStateMsg = true;
                TCPMOD.GetTcpMOD.TcpMsg = data.toString();
            }
           

        });

        /* 监听end事件 */
        client.on("end", function () {
            console.log("data end");
        });
        TCPMOD.TcpSocketState.GetStateSocket = true;
    } catch (err) { console.log(err);}
   
}
/**
 * 发送信息
 * @param {any} msg 信息
 */
function Tsend(msg) {
    client.write(msg);
}
module.exports = {
    InitTcpClient,
    Tsend
};