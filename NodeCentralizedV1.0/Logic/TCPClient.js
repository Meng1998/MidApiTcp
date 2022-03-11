/**
* ͨ��net.Server��������һ��TCP������
*/
/* ����netģ�� */
var net = require("net");
/* TCPMOD */
const TCPMOD = require('../DataModel/TcpMOD');
/* ����TCP�ͻ��� */
var client = net.Socket();
/**
 * ��ʼ��TCP�ͻ���
 */
function InitTcpClient() {

    try {
        /* �������ӵķ����� */
        client.connect(9999, '127.0.0.1', function () {
            console.log("connect the server");

            /* ��������������� */
            //client.write("message from client");
        });

        /* ����������������data���� */
        client.on("data", function (data) {
            console.log("the data of server is " + data.toString());

            if (TCPMOD.GetTcpMOD.GetState) {
                TCPMOD.GetTcpMOD.GetStateMsg = true;
                TCPMOD.GetTcpMOD.TcpMsg = data.toString();
            }
           

        });

        /* ����end�¼� */
        client.on("end", function () {
            console.log("data end");
        });
        TCPMOD.TcpSocketState.GetStateSocket = true;
    } catch (err) { console.log(err);}
   
}
/**
 * ������Ϣ
 * @param {any} msg ��Ϣ
 */
function Tsend(msg) {
    client.write(msg);
}
module.exports = {
    InitTcpClient,
    Tsend
};