const express = require('express');
const HIK = require('./Controllers/HIK');
const TCPCloent = require('./Logic/TCPClient');
const app = express();
app.use('/swagger', express.static('swagger'));
app.use('/', HIK);

process.on('uncaughtException', function (err) {
    //��ӡ������
    console.log(err);
    //��ӡ������ĵ���ջ�������
    console.log(err.stack);
});

try { TCPCloent.InitTcpClient(); } catch (err) { console.log("����"); }
app.all('*', function (req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "X-Requested-With");
    res.header("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
    res.header("X-Powered-By", ' 3.2.1');
    res.header("Content-Type", "application/json;charset=utf-8");
    next();
});
//��������
app.get('/', function (req, res) {
    res.send("hello world Hello, this is the app routing page. No logical operation. If you need to test the current API, you can get the current page. The current node centralized version is v1.0");
});

app.listen(1337, function () {
    console.log('Example app listening on port 1337!');
});
