class TcpSocketState {
    constructor() { }
/* TCP�����Ƿ������� */
    get GetStateSocket() { return this._GetStateSocket; } set GetStateSocket(v) { this._GetStateSocket = v; return this; }
}
/* TCP���ݻ���ģ�� */
class GetTcpMOD {
    constructor() { }
/* TCP��ȡ��Ϣ */
    get GetStateMsg() { return this._GetStateMsg; } set GetStateMsg(v) { this._GetStateMsg = v; return this; }
/* TCP�Ƿ��ȡ��Ϣ */
    get TcpMsg() { return this._TcpMsg; } set TcpMsg(v) { this._TcpMsg = v; return this; }
/* TCP�Ƿ񱣴������Ϣ */
    get GetState() { return this._GetState; } set GetState(v) { this._GetState = v; return this; }
}
module.exports = {
    TcpSocketState,
    GetTcpMOD
};