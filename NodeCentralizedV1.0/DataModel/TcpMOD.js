class TcpSocketState {
    constructor() { }
/* TCP服务是否有启动 */
    get GetStateSocket() { return this._GetStateSocket; } set GetStateSocket(v) { this._GetStateSocket = v; return this; }
}
/* TCP数据缓存模型 */
class GetTcpMOD {
    constructor() { }
/* TCP获取信息 */
    get GetStateMsg() { return this._GetStateMsg; } set GetStateMsg(v) { this._GetStateMsg = v; return this; }
/* TCP是否获取信息 */
    get TcpMsg() { return this._TcpMsg; } set TcpMsg(v) { this._TcpMsg = v; return this; }
/* TCP是否保存进来信息 */
    get GetState() { return this._GetState; } set GetState(v) { this._GetState = v; return this; }
}
module.exports = {
    TcpSocketState,
    GetTcpMOD
};