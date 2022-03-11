class Tree {
    constructor() { }
    get pageNo() { return this._pageNo; } set pageNo(v) { this._pageNo = v; return this; }
    get pageSize() { return this._pageSize; } set pageSize(v) { this._pageSize = v; return this; }
    get treeCode() { return this._treeCode; } set treeCode(v) { this._treeCode = v; return this; }
}
module.exports = {
    Tree
};