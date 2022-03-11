const express = require('express');
const HIK = require('./Controllers/HIK');
const TCPCloent = require('./Logic/TCPClient');
const app = express();
app.use('/swagger', express.static('swagger'));
app.use('/', HIK);

process.on('uncaughtException', function (err) {
    //打印出错误
    console.log(err);
    //打印出错误的调用栈方便调试
    console.log(err.stack);
});

try { TCPCloent.InitTcpClient(); } catch (err) { console.log("报错"); }
app.all('*', function (req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "X-Requested-With");
    res.header("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
    res.header("X-Powered-By", ' 3.2.1');
    res.header("Content-Type", "application/json;charset=utf-8");
    next();
});
//跨域配置
app.get('/', function (req, res) {
    res.send("hello world Hello, this is the app routing page. No logical operation. If you need to test the current API, you can get the current page. The current node centralized version is v1.0");
});

app.listen(1337, function () {
    console.log('Example app listening on port 1337!');
});
