const TCPMOD = require('../DataModel/TcpMOD');
/**
 * 请求等待返回后再返回 封装
 * @param {any} res 请求
 */
function Return(res) {
    TCPMOD.GetTcpMOD.GetState = true;
    var count = 0;
    var EaitTimer = setInterval(function () {
        if (TCPMOD.GetTcpMOD.GetStateMsg) {
            var Message = TCPMOD.GetTcpMOD.TcpMsg;
            TCPMOD.GetTcpMOD.GetStateMsg = false;//是否有消息
            TCPMOD.GetTcpMOD.TcpMsg = null;//消息内容
            TCPMOD.GetTcpMOD.GetState = false;//停止tcp信息的缓存 （防止延迟信息填充）
            if (GetmsgSuccessfulState(JSON.parse(Message).msg))
            {
                res.status(200),
                    res.json({
                        total : JSON.parse(Message).data.total,
                        msg : "success",
                        list : JSON.parse(Message).data.list
                    });
            }
            else {
                res.status(200),
                    res.json({
                        msg : "error",
                        Remarks : JSON.parse(Message)
                    });
            }
            clearInterval(EaitTimer);
        }
        count++;
        if (count >= 10) {
            TCPMOD.GetTcpMOD.GetState = false;//停止tcp信息的缓存 （防止延迟信息填充）
            res.status(200),
                res.json({
                    msg: "error",
                    MsgTxt: TCPMOD.TcpSocketState.GetStateSocket ? '接口超时超过4秒!' : '未与服务进行正常链接!'
                });
            clearInterval(EaitTimer);
        }
    }, 500);
}
function GetmsgSuccessfulState( state) {
    if (state === "success" || state === "SUCCESS" || state === "Operation succeeded")
        return true;
    else
        return false;
}
module.exports = {
    Return
};