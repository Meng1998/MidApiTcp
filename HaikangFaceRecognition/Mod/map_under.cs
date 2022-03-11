using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class map_under
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string id { set; get; }
        public string group_id { set; get; }
        public int order_num { set; get; }
        public string floor_name { set; get; }
        public string model_url { set; get; }
        //public string center_position { set; get; }
        //public string remark { set; get; }
    }
}
