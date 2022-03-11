using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTools.Mod
{
    class map_model
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string id { get; set; }//随机id
        public string pid { get; set; }//父id
        public string node_name { get; set; }//文件名
        public string node_type { get; set; }//文件是否为模型是为3dtiles否为group
        public string data_type { get; set; }//文件所属normal还是virtual
        public string data_url { get; set; }//文件路径开头$serverURL$
        public bool? visible { get; set; }//模型是否显示t/f

    }
}
