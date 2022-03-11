using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class map_build
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string id { set; get; }
        public string group_id { set; get; }
        public string build_name { set; get; }
        //public string center_position { set; get; }
        //public int label_position { set; get; }
        //public string remark { set; get; }
    }
}
