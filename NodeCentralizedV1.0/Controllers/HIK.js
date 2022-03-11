const app = require('express').Router();
/* http操作模块 */
const bodyParser = require('body-parser');
//const Model = require('../DataModel/FrontEndReception');
/* TCP客户端操作模块 */
const TCPCloent = require('../Logic/TCPClient');
/*请求封装的方法*/
const RequestReturn = require('../Logic/RequestReturn');
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));



app.all('*', function (req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "X-Requested-With");
    res.header("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
    res.header("X-Powered-By", ' 3.2.1');
    res.header("Content-Type", "application/json;charset=utf-8");
    next();
});
//跨域配置
//构造函数

/**
 * ISC编码设备列表
 */
app.post('/v2/ISC/GetEquipmentList', (req, res) => {
    TCPCloent.Tsend(JSON.stringify({
        MsgType: "HIKWEBAPI",
        GetKEYIndex: 12,
        Parameter: req.body,
        eventType: "ISCWEBAPI"//事件名称代码
    }));
    RequestReturn.Return(res);
});

module.exports = app;