using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class sys_config
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string id { set; get; }
        public string sys_name { set; get; }
        public string data_server_uri { set; get; }
        //public object center_location { set; get; }
        //public string data_type { set; get; }
        //public string is_stand { set; get; }
        //public string is_login { set; get; }
       
       
    }
}
