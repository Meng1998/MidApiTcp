using System;
using System.Collections.Generic;
using System.Text;

namespace LntegratedMiddleware.NBE.M
{
    class Record
    {
        public string DoorNo { get; set; }
        public string IoState { get; set; }//出入状态
        public string EventTime { get; set; }
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string DeptName { get; set; }//部门
    }
}
